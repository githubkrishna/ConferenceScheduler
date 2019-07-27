using ConferenceScheduler.Models;
using System.Collections.Generic;

namespace ConferenceScheduler.Service
{
    public interface ITalksLoader
    {
        IEnumerable<Talk> Load(ScheduleTalkBy loadTaskBy);
    }

    public enum ScheduleTalkBy
    {
        None,
        DurationAscending,
        DurationDescending,
        TitleAscending,
        TitleDescending
    }
}
