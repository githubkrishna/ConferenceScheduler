using System.Collections.Generic;
namespace ConferenceScheduler.Models
{
    public class Track
    {
        public string Name { get; set; }

        public List<Talk> Talks { get; set; }
    }
}
