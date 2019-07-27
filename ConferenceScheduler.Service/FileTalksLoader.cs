using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceScheduler.Models;
using System.IO;

namespace ConferenceScheduler.Service
{
    public class FileTalksLoader : ITalksLoader
    {
        public string Path { get; private set; }

        public FileTalksLoader(string filePath)
        {
            this.Path = filePath;
        }

        public IEnumerable<Talk> Load(ScheduleTalkBy loadTaskBy)
        {
            var talks = this.Load();
            switch(loadTaskBy)
            {
                case ScheduleTalkBy.DurationDescending :
                    talks = talks.OrderByDescending(p => p.Duration);
                    break;

                case ScheduleTalkBy.DurationAscending :
                    talks = talks.OrderBy(p => p.Duration);
                    break;

                case ScheduleTalkBy.TitleAscending:
                    talks = talks.OrderBy(p => p.Title);
                    break;

                case ScheduleTalkBy.TitleDescending:
                    talks = talks.OrderByDescending(p => p.Title);
                    break;
            }

            return talks;
        }

        private IEnumerable<Talk> Load()
        {
            var rowTitles = File.ReadLines(Path);
            var talks = new List<Talk>();
            foreach (var title in rowTitles)
            {
                var words = title.Trim().Split(' ');
                string lastWord = words[words.Length - 1];
                int duration = (lastWord == "lightning") ? 5 : Convert.ToInt32(lastWord.Substring(0, lastWord.IndexOf('m')));
                Array.Resize(ref words, words.Length - 1);
                string talkTitle = string.Join(" ", words);
                talks.Add(new Talk { Title = talkTitle, Duration = duration });
            }
            return talks;
        }
    }
}
