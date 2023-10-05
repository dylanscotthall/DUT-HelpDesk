using DUT_HelpDesk.DatabaseModels;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace DUT_HelpDesk.Controllers
{
    public static class StateManager
    {
        //state wide variables for easy access
        private static DutHelpdeskdbContext db = new DutHelpdeskdbContext();
        public static DatabaseModels.User user;
        public static Technician technician;
        public static string token;
        public static List<Ticket> filteredTickets; //used to save the user's filtered ticket list for report generation
        public static string email;

        //returns a list of all users
        public static List<DatabaseModels.User> GetUsers()
        {
            return db.Users.ToList();
        }
        //returns a single user from the database
        public static DatabaseModels.User GetUser(int id)
        {
            return db.Users.Where(x => x.UserId == id).FirstOrDefault();
        }
        //returns a single user from the database by firebase ID
        public static DatabaseModels.User GetUserByFBId(string id)
        {
            return db.Users.Where(x => x.FbId == id).FirstOrDefault();
        }
        //adds a user to the database
        public static void AddUser(DatabaseModels.User user)
        {
            db.Users.Add(user);
        }
        //deletes a user (to be implemented)
        public static void DeleteUser(int id)
        {

        }
        //update a user in the database
        public static void UpdateUser(DatabaseModels.User user)
        {
            db.Users.Update(user);
        }
        //returns a list of all tickets for a single user
        public static List<Ticket> GetUserTickets()
        {
            return db.Tickets.Where(x => x.UserId == user.UserId).ToList();
        }

        public static List<Faq> GetAllFaqs()
        {
            return db.Faqs.ToList();
        }

        public static void CreateFaq(Faq faq)
        { 
        db.Faqs.Add(faq);
            db.SaveChanges();
        }


        //returns a list of tickets a technician is working on
        public static List<Ticket> GetTechnicianTickets()
        {
            List<TicketTechnician> tt = db.TicketTechnicians.Where(x => x.TechnicianId == technician.TechnicianId).ToList();
            List<Ticket> t = new List<Ticket>();
            foreach (var item in tt)
            {
                if (item.TechnicianId == technician.TechnicianId)
                {
                    t.Add(db.Tickets.Where(x => x.TicketId == item.TicketId && item.IsAssigned == true).FirstOrDefault());
                }
            }
            return t;
        }
        //returns all tickets in database (not for website)
        public static List<Technician> GetAllTechnicians()
        {
            List<Technician> technicians = db.Technicians.Include(x => x.TicketTechnicians).ToList();
            return technicians;
        }
        public static List<Ticket> GetAllTickets()
        {
            List<Ticket> tickets = db.Tickets.Include(i => i.TicketStatuses).ThenInclude(i => i.Status).Include(i => i.TicketTechnicians).ToList();
            return tickets;
        }
        //returns all ticket technicians
        public static List<TicketTechnician> GetAllTicketTechnicians()
        {
            return db.TicketTechnicians.ToList();
        }
        //returns a list of tickets that have not been assigned to a technician
        public static List<Ticket> GetAllUnassignedTickets()
        {
            List<TicketTechnician> tt = db.TicketTechnicians.ToList();
            List<Ticket> toDelete = db.Tickets.ToList();
            List<Ticket> tickets = toDelete;
            List<int> toRemove = new List<int>();
            foreach (var item in tt)
            {
                if(!toRemove.Contains((int)item.TicketId) && item.IsAssigned == true)
                {
                    toRemove.Add((int)item.TicketId);
                }
            }
            foreach(var tr in toRemove)
            {
                tickets.Remove(db.Tickets.Where(x => x.TicketId == tr).FirstOrDefault());
            }
            return tickets;
        }
        //gets a single ticket from the database 
        public static Ticket GetTicket(int id)
        {
            return db.Tickets.Where(x => x.TicketId == id).FirstOrDefault();
        }
        //returns a bool to see if a user saved in state is a technician
        public static string getUserType()
        {
            return user.Type;
        }
        //returns a technician if the user is already assigned as a technician
        public static Technician GetTechnician()
        {
            return db.Technicians.Where(x => x.UserId == user.UserId).FirstOrDefault();
        }
        //returns a single technician by ID
        public static Technician GetTechnician(int id)
        {
            return db.Technicians.Where(x => x.UserId == id).FirstOrDefault();
        }
        //create a ticket technician using technician ID and ticket ID
        public static void CreateTicketTechnician(int id, int techId)
        {
            TicketTechnician? tt = db.TicketTechnicians.Where(x => x.TicketId == id && x.TechnicianId == techId).FirstOrDefault();
            Ticket? t = db.Tickets.Include(i => i.TicketStatuses).ThenInclude(i => i.Status).Where(x => x.TicketId == id).FirstOrDefault();

            //add active to ticketstatus for ticket
            if(db.TicketStatuses.Where(x => x.TicketId == id).OrderByDescending(o => o.TimeStamp).FirstOrDefault().StatusId != 2)
            {
                Status status = db.Statuses.Where(x => x.StatusId == 2).FirstOrDefault();
                TicketStatus ticketStatus = new TicketStatus() { TicketId = t.TicketId, StatusId = status.StatusId, TimeStamp = DateTime.UtcNow, Status = status, Ticket = t };
                t.TicketStatuses.Add(ticketStatus);
                db.Entry(t).State = EntityState.Modified;
            }
            
            
            //Checking if an existing tt entry exists for the technician and ticket
            if (tt != null)
            {
                //If the entry already exists, modify it.
                tt.IsAssigned = true;
                db.Entry(tt).State = EntityState.Modified;
            }
            else{
                //If the entry does not already exist create a new one.
                TicketTechnician newTT = new TicketTechnician()
                {
                    TicketId = id,
                    TechnicianId = techId,
                    IsAssigned = true,
                    TimeStamp = DateTime.UtcNow
                };
                db.TicketTechnicians.Add(newTT);
            }
            db.SaveChanges(); //save changes to TicketTechnician Table

            //finds all ticketTechnician records where isAssigned = true, this gets an accurate technician count for a ticket.
            List<TicketTechnician> ticketIsAssigned = db.TicketTechnicians.Where(x => x.TicketId == id && x.IsAssigned == true).ToList();
            int technicianCount = ticketIsAssigned.Count;
            t.TechnicianCount = technicianCount;
            db.Entry(t).State = EntityState.Modified;

            db.SaveChanges(); //save changes to Ticket Table
        }
        //unassign a ticket from a technician
        public static void UnassignTicketTechnician(int id, int techId)
        {
            TicketTechnician tt = db.TicketTechnicians.Where(x => x.TicketId == id && x.TechnicianId == techId).FirstOrDefault();
            Ticket t = db.Tickets.Where(x => x.TicketId == id).FirstOrDefault();

            //check whether ticket will be active or available after unassign
            List<TicketTechnician> ticketTechnicians = db.TicketTechnicians.Where(x => x.TicketId == id).ToList();
            bool stillActive = false;
            foreach (TicketTechnician ticketTech in ticketTechnicians)
            {
                if(ticketTech != tt)
                {
                    if (ticketTech.IsAssigned == true)
                    {
                        stillActive = true;
                        break;
                    }
                }
            }
            if (!stillActive)
            {
                db.TicketStatuses.Add(new TicketStatus() { TicketId = t.TicketId, StatusId = 1, TimeStamp = DateTime.UtcNow, Status = db.Statuses.Where(x => x.StatusId == 1).FirstOrDefault(), Ticket = t });
            }
            List<TicketTechnician> ticketIsAssigned = db.TicketTechnicians.Where(x => x.TicketId == id && x.IsAssigned == true).ToList();
            int technicianCount = ticketIsAssigned.Count-1;
            t.TechnicianCount = technicianCount;

            tt.IsAssigned = false;
            db.Entry(tt).State = EntityState.Modified;
            db.Entry(t).State = EntityState.Modified;
            db.SaveChanges();
        }
        //returns a list of replies for a single ticket
        public static List<Reply> GetTicketReplies(int id)
        {
            return db.Replies.Where(x => x.TicketId == id).ToList();
        }
        //creates a new ticket from a user
        public static async Task CreateTicket(TicketViewModel model, FirebaseAuthProvider auth)
        {
            //get current user  
            string token = StateManager.token;
            var userFbId = await auth.GetUserAsync(token);

            var currentUser = db.Users.Where(u => u.FbId.Equals(userFbId.LocalId)).FirstOrDefault();
            Ticket ticket = new Ticket()
            {
                //userid 
                UserId = StateManager.user.UserId,
                //technicianid to be assigned later
                Subject = model.Subject,
                QueryBody = model.QueryBody,
                Priority = "Low",
                DateCreated = DateTime.UtcNow
            };

            Status status = db.Statuses.Where(x => x.StatusId == 1).FirstOrDefault();
            List<TicketStatus> ticketStatuses = new List<TicketStatus>();
            TicketStatus ticketStatus = new TicketStatus() { TicketId = ticket.TicketId, StatusId = status.StatusId, TimeStamp = DateTime.UtcNow, Status = status, Ticket = ticket };
            ticketStatuses.Add(ticketStatus);
            ticket.TicketStatuses = ticketStatuses;
            await db.TicketStatuses.AddAsync(ticketStatus);
            await db.Tickets.AddAsync(ticket);
            await db.SaveChangesAsync();

            if (model.File != null)
            {

                using (var stream = new MemoryStream())
                {
                    await model.File.CopyToAsync(stream);

                    stream.Seek(0, SeekOrigin.Begin);

                    Ticket[] currentTicket = db.Tickets.Where(t => t == ticket).ToArray();
                    var uploadedFile = new Attachment()
                    {
                        TicketId = currentTicket[0].TicketId,
                        FileName = model.File.Name,
                        FileContent = stream.ToArray(),
                        ContentType = model.File.ContentType
                    };

                    await db.Attachments.AddAsync(uploadedFile);
                    await db.SaveChangesAsync();
                }

            }
        }

        public static async Task MyReplies(ReplyTicketViewModel model)
        {

            Reply reply = new Reply()
            {
                TicketId = model.id,
                Message = model.Message,
                Date = DateTime.Now,
                
                
            };
            
            await db.Replies.AddAsync(reply);
            await db.SaveChangesAsync();

            if (model.file != null)
            {

                using (var stream = new MemoryStream())
                {
                    await model.file.CopyToAsync(stream);

                    stream.Seek(0, SeekOrigin.Begin);

                    Reply[] currentReply = db.Replies.Where(r => r == reply).ToArray();
                    var uploadedFile = new Attachment()
                    {
                        ReplyId = currentReply[0].ReplyId,
                        FileName = model.file.Name,
                        FileContent = stream.ToArray(),
                        ContentType = model.file.ContentType
                    };

                    await db.Attachments.AddAsync(uploadedFile);
                    await db.SaveChangesAsync();
                }

            }

        }


    }
}
