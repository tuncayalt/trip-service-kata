using System.Collections.Generic;
using TripServiceKata.Exception;
using TripServiceKata.User;
using System.Linq;

namespace TripServiceKata.Trip
{
    public class TripService
    {
        private readonly User.User loggedInUser;
        private readonly TripDAO tripDAO;

        public TripService(User.User loggedInUser, TripDAO tripDAO)
        {
            this.loggedInUser = loggedInUser;
            this.tripDAO = tripDAO;
        }

        public List<Trip> GetTripsByUser(User.User user)
        {
            ValidateLoggedInUser(loggedInUser);

            List<Trip> tripList = new List<Trip>();
            if (CheckIsFriend(user, loggedInUser))
            {
                tripList = GetTripsBy(user);
            }
            return tripList;

        }

        private static void ValidateLoggedInUser(User.User loggedUser)
        {
            if (loggedUser == null)
            {
                throw new UserNotLoggedInException();
            }
        }

        protected virtual List<Trip> GetTripsBy(User.User user)
        {
            return tripDAO.FindTripsBy(user);
        }

        private bool CheckIsFriend(User.User user, User.User loggedUser)
        {
            return user.GetFriends().Any(u => u.Equals(loggedUser));
        }
    }
}
