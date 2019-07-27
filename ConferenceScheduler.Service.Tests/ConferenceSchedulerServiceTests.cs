using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ConferenceScheduler.Models;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceScheduler.Service.Tests
{
    [TestClass]
    public class ConferenceSchedulerServiceTests
    {
        private readonly IScheduler conferenceSchedulerService;

        private Mock<ITalksLoader> fileTalksLoader;

        public ConferenceSchedulerServiceTests()
        {
            this.fileTalksLoader = new Mock<ITalksLoader>();
            this.conferenceSchedulerService = new ConferenceSchedulerService(this.fileTalksLoader.Object);
        }

        [TestMethod]
        public void GivenInputThenItShouldScheduleConferenceTalks()
        {
            // Arrange
            var talks = new List<Talk>
            {
                new Talk { Title = "Lua for the Masses", Duration = 30 },
                new Talk { Title = "Ruby Errors from Mismatched Gem Versions", Duration = 45 },
                new Talk { Title = "Common Ruby Errors", Duration = 45 },
                new Talk { Title = "Rails for Python Developers", Duration = 5 }
            };

            this.fileTalksLoader.Setup(p => p.Load(It.IsAny<ScheduleTalkBy>())).Returns(talks);

            // Act
            var result = this.conferenceSchedulerService.Schedule(ScheduleTalkBy.DurationAscending);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 1);
            var track = result.FirstOrDefault();
            Assert.IsNotNull(track.Talks);
            Assert.IsTrue(track.Talks.Count() == 5);
        }
    }
}
