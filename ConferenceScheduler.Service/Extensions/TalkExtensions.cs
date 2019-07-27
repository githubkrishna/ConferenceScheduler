using ConferenceScheduler.Models;
using System;
using System.Collections.Generic;

namespace ConferenceScheduler.Service.Extensions
{
    public static class TalkExtensions
    {
        public static void AddTalk(this List<Talk> talks, Talk talk, DateTime today)
        {
            talk.StartTime = today.TimeOfDay;
            talk.EndTime = new TimeSpan(00, talk.Duration, 00);
            talk.AlreadyTaken = true;
            talks.Add(talk);            
        }
    }
}
