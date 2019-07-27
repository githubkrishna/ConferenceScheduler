using System.Collections.Generic;

namespace ConferenceScheduler.Models
{
    public class Conference
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
