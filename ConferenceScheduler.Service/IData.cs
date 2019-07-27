using ConferenceScheduler.Models;
using System.Collections.Generic;

namespace ConferenceScheduler.Service
{
    public interface IData
    {
        Dictionary<int, List<Talk>> FetchTalksGroupBy();
        List<Talk> Fetch();
    }
}
