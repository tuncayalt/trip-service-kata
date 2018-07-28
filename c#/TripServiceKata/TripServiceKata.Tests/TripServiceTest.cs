using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TripServiceKata.Exception;
using TripServiceKata.Trip;
using TripServiceKata.User;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class TripServiceTest
    {
        TripService tripService;
        private static User.User loggedInUser = null;
        User.User aUser;
        Mock<TripDAO> tripDao;

        [SetUp]
        public void Setup()
        {
            tripDao = new Mock<TripDAO>();
            tripDao.SetupAllProperties();
            aUser = new User.User();
        }


        [Test]
        public void Should_throw_exception_when_user_is_not_logged_in()
        {
            try
            {
                loggedInUser = null;
                tripService = new TripService(loggedInUser, tripDao.Object);
                tripService.GetTripsByUser(aUser);
                Assert.Fail();
            }
            catch (UserNotLoggedInException ex)
            {
                Assert.Pass();
            }
            catch (System.Exception ex)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Should_return_zero_trips_when_not_friends_with_logged_in_user()
        {
            loggedInUser = new User.User();
            tripService = new TripService(loggedInUser, tripDao.Object);
            List<Trip.Trip> trips = tripService.GetTripsByUser(aUser);

            Assert.AreEqual(0, trips.Count);
        }

        [Test]
        public void Should_return_0_trips_when_friends_with_logged_in_user_and_no_trips()
        {
            loggedInUser = new User.User();
            aUser.AddFriend(loggedInUser);
            tripDao.Setup(x => x.FindTripsBy(It.IsAny<User.User>())).Returns(aUser.Trips);
            tripService = new TripService(loggedInUser, tripDao.Object);
            List<Trip.Trip> trips = tripService.GetTripsByUser(aUser);

            Assert.AreEqual(0, trips.Count);
        }

        [Test]
        public void Should_return_2_trips_when_friends_with_logged_in_user_and_2_trips()
        {
            loggedInUser = new User.User();
            aUser.AddFriend(loggedInUser);
            aUser.AddTrip(new Trip.Trip());
            aUser.AddTrip(new Trip.Trip());
            tripDao.Setup(x => x.FindTripsBy(aUser)).Returns(aUser.Trips);
            tripService = new TripService(loggedInUser, tripDao.Object);
            List<Trip.Trip> trips = tripService.GetTripsByUser(aUser);

            Assert.AreEqual(2, trips.Count);
        }
    }
}
