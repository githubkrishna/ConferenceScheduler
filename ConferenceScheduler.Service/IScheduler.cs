using ConferenceScheduler.Models;
using System.Collections.Generic;

namespace ConferenceScheduler.Service
{
    public interface IScheduler
    {
        IEnumerable<Track> Schedule(ScheduleTalkBy scheduleTalkBy);
    }
}
