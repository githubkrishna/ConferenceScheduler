using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenceScheduler.Models;
using System.IO;

namespace ConferenceScheduler.Service
{
    public class FileTalksLoader : ITalksLoader
    {
        public string Path { get; private set; }

        public FileTalksLoader()
        {
            this.Path = Constants.INPUTFILEPATH;
        }

        public IEnumerable<Talk> Load(LoadTalkBy loadTaskBy)
        {
            var talks = this.Load();
            switch(loadTaskBy)
            {
                case LoadTalkBy.DurationDescending :
                    talks = talks.OrderByDescending(p => p.Duration);
                    break;

                case LoadTalkBy.DurationAscending :
                    talks = talks.OrderBy(p => p.Duration);
                    break;

                case LoadTalkBy.TalkAscending:
                    talks = talks.OrderBy(p => p.Title);
                    break;

                case LoadTalkBy.TalkDescending:
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
                var words = title.Split(' ');
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
