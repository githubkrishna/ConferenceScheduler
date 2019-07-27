using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ConferenceScheduler.Service.Tests
{
    [TestClass]
    public class FileTalksLoaderTests
    {
        private readonly ITalksLoader fileTalksLoader;
        public FileTalksLoaderTests()
        {
            this.fileTalksLoader = new FileTalksLoader("TestInput.txt");
        }

        [TestMethod]
        public void Given_LoadTalkByDurationDescending_Then_ItShouldLoadTheTalksInDescendingOrderOfDuration()
        {
            // Arrange
            ScheduleTalkBy loadTalkBy = ScheduleTalkBy.DurationDescending;

            // ACt
            var result = this.fileTalksLoader.Load(loadTalkBy);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual(60, result.First().Duration);
        }

        [TestMethod]
        public void Given_LoadTalkByDurationAscending_Then_ItShouldLoadTheTalksInAscendingOrderOfDuration()
        {
            // Arrange
            ScheduleTalkBy loadTalkBy = ScheduleTalkBy.DurationAscending;

            // Act
            var result = this.fileTalksLoader.Load(loadTalkBy);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual(5, result.First().Duration);
        }

        [TestMethod]
        public void Given_LoadTalkByTitleDescending_Then_ItShouldLoadTheTalksInDescendingOrderOfTalk()
        {
            // Arrange
            ScheduleTalkBy loadTalkBy = ScheduleTalkBy.TitleDescending;

            // Act
            var result = this.fileTalksLoader.Load(loadTalkBy);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual("Woah", result.First().Title);
        }

        [TestMethod]
        public void Given_LoadTalkByTitleAscending_Then_ItShouldLoadTheTalksInAscendingOrderOTalk()
        {
            // Arrange
            ScheduleTalkBy loadTalkBy = ScheduleTalkBy.TitleAscending;

            // ACt
            var result = this.fileTalksLoader.Load(loadTalkBy);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual("A World Without HackerNews", result.First().Title);
        }
    }
}
