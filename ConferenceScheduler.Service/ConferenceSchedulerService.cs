using ConferenceScheduler.Models;
using ConferenceScheduler.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceScheduler.Service
{
    public class ConferenceSchedulerService : IScheduler
    {
        private readonly ITalksLoader talksLoader;
        private const string TitleNetworking = "Networking";
        private const string TitleLunch = "Lunch";

        public ConferenceSchedulerService(ITalksLoader talksLoader)
        {
            this.talksLoader = talksLoader;
        }

        public IEnumerable<Track> Schedule(ScheduleTalkBy scheduleTalkBy)
        {
            List<Track> tracks = new List<Track>();
            var allTalks = talksLoader.Load(scheduleTalkBy);
            var trackId = 1;

            int totalMinutesAllowed = 60 * (Constants.LUNCHHOUR - Constants.SESSIONSTARTSAT); ;
            int afternoonMinutesAllowed = 60 * (Constants.SESSIONENDSAT - Constants.LUNCHHOUR - 1);

            while (allTalks.Any(p => !p.AlreadyTaken))
            {
                var track = new Track { Name = $"Track {trackId}", Talks = new List<Talk>() };
                bool lunchTalkAdded = false;
                bool networkTalkAdded = false;
                int ellapsedMinutes = 0;
                int afternoonElapsedMinutes = 0;
                var talks = new List<Talk>();
                var today = DateTime.Today.Add(new TimeSpan(09, 00, 00));

                foreach (var talk in allTalks.Where(p => !p.AlreadyTaken))
                {
                    // morning session talks
                    if ((talk.Duration + ellapsedMinutes) <= totalMinutesAllowed && !talk.AlreadyTaken)
                    {                        
                        talks.AddTalk(talk, today);
                        today = today.Add(new TimeSpan(00, talk.Duration, 00));
                        ellapsedMinutes += talk.Duration;
                        continue;
                    }   

                    if (!lunchTalkAdded && ellapsedMinutes <= totalMinutesAllowed)
                    {
                        var tk = allTalks.FirstOrDefault(p => p.Duration == (totalMinutesAllowed - ellapsedMinutes));
                        if (tk != null)
                        {
                            talks.Add(tk);
                            tk.AlreadyTaken = true;
                        }

                        today = AddLunchTalk(talks);
                        lunchTalkAdded = true;
                    }

                    // afternoon session talks
                    if ((talk.Duration + afternoonElapsedMinutes) <= afternoonMinutesAllowed && !talk.AlreadyTaken)
                    {
                        talks.AddTalk(talk, today);
                        today = today.Add(new TimeSpan(00, talk.Duration, 00));
                        afternoonElapsedMinutes += talk.Duration;
                        continue;
                    }

                    if (!networkTalkAdded && afternoonElapsedMinutes <= afternoonMinutesAllowed)
                    {
                        var tk = allTalks.FirstOrDefault(p => p.Duration == (afternoonMinutesAllowed - afternoonElapsedMinutes));
                        if (tk != null)
                        {
                            talks.Add(tk);
                            tk.AlreadyTaken = true;
                        }

                        today = this.AddNetworkTalk(talks, today);
                        networkTalkAdded = true;
                        break;
                    }
                }

                if(!talks.Any(p => p.Title == TitleNetworking))
                {
                    today = this.AddNetworkTalk(talks, today);
                }

                track.Talks.AddRange(talks);
                tracks.Add(track);
                trackId += 1;
            }

            return tracks;
        }

        private DateTime AddLunchTalk(List<Talk> talks)
        {
            talks.Add(new Talk { Title = TitleLunch, Duration = 60, StartTime = DateTime.Today.Add(new TimeSpan(12, 00, 00)).TimeOfDay,
                EndTime = DateTime.Today.Add(new TimeSpan(13, 00, 00)).TimeOfDay });

            return DateTime.Today.Add(new TimeSpan(13, 00, 00));
        }

        private DateTime AddNetworkTalk(List<Talk> talks, DateTime date)
        {
            if (date.Hour < 16)
            {
                date = DateTime.Today.Add(new TimeSpan(16, 00, 00));
            }

            talks.Add(new Talk { Title = TitleNetworking, StartTime = date.TimeOfDay, EndTime = date.Add(new TimeSpan(0, 30, 00)).TimeOfDay });
            return date;
        }
    }
}
