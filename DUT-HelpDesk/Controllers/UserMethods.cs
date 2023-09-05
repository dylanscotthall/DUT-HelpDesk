using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk.Controllers
{
    public static class UserMethods
    {
        private static DatabaseModels.DutHelpdeskdbContext db = new DatabaseModels.DutHelpdeskdbContext();
        public static User user = null;
        public static Technician technician = null;
        public static List<User> GetUsers()
        {
            return db.Users.ToList();
        }
        public static User GetUser(int id)
        {
            return db.Users.Where(x => x.UserId == id).FirstOrDefault();
        }
        public static User GetUserByFBId(string id)
        {
            return db.Users.Where(x => x.FbId == id).FirstOrDefault();
        }
        public static void AddUser(User user)
        {
            db.Users.Add(user);
        }
        public static void DeleteUser(int id)
        {

        }
        public static void UpdateUser(User user)
        {
            db.Users.Update(user);
        }
        public static List<Ticket> GetUserTickets()
        {
            return db.Tickets.Where(x => x.UserId == user.UserId).ToList();
        }
        public static List<Ticket> GetTechnicianTickets()
        {
            return db.Tickets.Where(x => x.TechnicianId == technician.TechnicianId).ToList();
        }
        public static List<Ticket> GetAllTickets()
        {
            return db.Tickets.ToList();
        }
        public static Ticket GetTicket(int id)
        {
            return db.Tickets.Where(x => x.TicketId == id).FirstOrDefault();
        }
        public static bool isTechnician()
        {
            return user.Type == "Student" ? false : true;
        }
        public static Technician GetTechnician()
        {
            return db.Technicians.Where(x => x.UserId == user.UserId).FirstOrDefault();
        }
        public static Technician GetTechnician(int id)
        {
            return db.Technicians.Where(x => x.UserId == id).FirstOrDefault();
        }
    }
}
