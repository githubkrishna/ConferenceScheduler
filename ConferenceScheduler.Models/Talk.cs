using System;

namespace ConferenceScheduler.Models
{
    public class Talk
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public bool AlreadyTaken { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Format(DateTime today)
        {
            string formattedString = this.Duration!=0 ? string.Format("{0} {1} {2}min", today.Add(this.StartTime).ToString("hh:mm tt"), this.Title,this.Duration): string.Format("{0} {1}", today.Add(this.StartTime).ToString("hh:mm tt"), this.Title, this.Duration);

            return formattedString;
        }
    }
}
