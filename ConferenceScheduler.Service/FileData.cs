using System;
using System.Collections.Generic;
using ConferenceScheduler.Models;
using System.IO;
using System.Linq;

namespace ConferenceScheduler.Service
{
    public class FileData : IData
    {
        public string Path { get; private set; }
        
        public FileData()
        {
            this.Path = Constants.INPUTFILEPATH;
        }
        public List<Talk> Fetch()
        {
           var rowTitles = File.ReadLines(Path);
            var talks = new List<Talk>();
            foreach(var title in rowTitles)
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

        public Dictionary<int, List<Talk>> FetchTalksGroupBy()
        {
            Dictionary<int, List<Talk>> talksDictionary = new Dictionary<int, List<Talk>>();
            var talks = Fetch();

            var all60minTalks = talks.Where(x => x.Duration == 60).ToList();
            var all45minTalks = talks.Where(x => x.Duration == 45).ToList();
            var all30minTalks = talks.Where(x => x.Duration == 30).ToList();
            var all5minTalks = talks.Where(x => x.Duration == 5).ToList();

            talksDictionary[60] = all60minTalks;
            talksDictionary[45] = all45minTalks;
            talksDictionary[30] = all30minTalks;
            talksDictionary[5] = all5minTalks;

            return talksDictionary;
        }
    }
}
