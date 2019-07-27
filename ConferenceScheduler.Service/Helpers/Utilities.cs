using ConferenceScheduler.Models;
using System;

namespace ConferenceScheduler.Service
{
    public static class Utilities
    {
        public static int TotalMinimumForTrack = (Constants.SESSIONENDSAT - Constants.SESSIONSTARTSAT - 1) * Constants.MINUTESPERHOUR;
        public static int LimitForMorningSession = 60 * (Constants.LUNCHHOUR - Constants.SESSIONSTARTSAT);
        public static int LimitForAfterNoonSession = 60 * (Constants.SESSIONENDSAT - Constants.LUNCHHOUR - 1) - 5; // This is based on assumption

        public static DateTime FourPM = DateTime.Today.Add(new TimeSpan(16, 00, 00));
        public static DateTime FivePM = DateTime.Today.Add(new TimeSpan(17, 00, 00));
        public static DateTime LunchTime = DateTime.Today.Add(new TimeSpan(12, 00, 00));

    }
}
