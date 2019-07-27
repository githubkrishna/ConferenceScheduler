using ConferenceScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceScheduler.Service
{
    public class ConferenceSchedulerService1 : IScheduler<IEnumerable<Track>>
    {
        private readonly ITalksLoader talksLoader;

        List<Track> tracks;

        public IEnumerable<Track> Tracks { get { return tracks.AsReadOnly(); } }

        public ConferenceSchedulerService1(ITalksLoader talksLoader)
        {
            this.talksLoader = talksLoader;
            tracks = new List<Track>();
        }

        public IEnumerable<Track> Schedule()
        {
            List<Track> tracks = new List<Track>();
            var allTalks = talksLoader.Load(LoadTalkBy.DurationDescending);
            var trackId = 1;
            
            int totalMinutesAllowed = Utilities.LimitForMorningSession;
            int afternoonMinutesAllowed = Utilities.LimitForAfterNoonSession;
            while (allTalks.Any(p => !p.AlreadyTaken))
            {
                var track = new Track { Name = $"Track {trackId}", Talks = new List<Talk>() };
                bool lunchTalkAdded = false, networkTalkAdded = false;
                var ellapsedMinutes = 0;
                int afternoonElapsedMinutes = 0;
                List<Talk> talks = new List<Talk>();
                foreach (var talk in allTalks.Where(p => !p.AlreadyTaken))
                {
                    // morning session talks
                    if ((talk.Duration + ellapsedMinutes) <= totalMinutesAllowed && !talk.AlreadyTaken)
                    {
                        talks.Add(talk);
                        ellapsedMinutes += talk.Duration;
                        talk.AlreadyTaken = true;
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

                        talks.Add(new Talk { Title = "LUNCH", Duration = 60 });
                        lunchTalkAdded = true;
                    }

                    // afternoon session talks
                    if ((talk.Duration + afternoonElapsedMinutes) <= afternoonMinutesAllowed && !talk.AlreadyTaken)
                    {
                        talks.Add(talk);
                        afternoonElapsedMinutes += talk.Duration;
                        talk.AlreadyTaken = true;
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

                        talks.Add(new Talk { Title = "Networking", Duration = 60 });
                        networkTalkAdded = true;
                    }
                }

                track.Talks.AddRange(talks);
                tracks.Add(track);
                trackId += 1;
            }

            return tracks;
        }
    }
}
