using ConferenceScheduler.Models;
using ConferenceScheduler.Service;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ConferenceScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started scheduling tasks");
            var scheduler = new ConferenceSchedulerService(new FileTalksLoader(ConfigurationManager.AppSettings["InputFilePath"]));
            WriteOnConsole(scheduler.Schedule(ScheduleTalkBy.None));
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void WriteOnConsole(IEnumerable<Track> tracks)
        {
            foreach (var track in tracks)
            {
                DateTime today = DateTime.Today;
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine(track.Name);
                Console.WriteLine("---------------------------------------------------------------");
                foreach (var talk in track.Talks)
                {
                    Console.WriteLine(talk.Format(today));
                }
            }
        }
    }
}
