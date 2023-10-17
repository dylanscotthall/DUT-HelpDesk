using DUT_HelpDesk.DatabaseModels;
using Firebase.Auth;
using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk.Controllers
{
    public static class StateManager
    {
        //state wide variables for easy access
        private static DutHelpdeskdbContext db = new DutHelpdeskdbContext();
        public static DatabaseModels.User? user;
        public static Technician? technician;
        public static string? token;
        public static List<Ticket>? filteredTickets; //used to save the user's filtered ticket list for report generation
        public static string? email;

        //returns a list of all users
        public static List<DatabaseModels.User> GetUsers()
        {
            return db.Users.ToList();
        }

        //returns a single user from the database
        public static DatabaseModels.User GetUser(int? id)
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

        //returns a list of all FAQs
        public static List<Faq> GetAllFaqs()
        {
            return db.Faqs.ToList();
        }

        //creates a new FAQ in the database
        public static void CreateFaq(Faq faq)
        {
            db.Faqs.Add(faq);
            db.SaveChanges();
        }

        //returns a list of tickets a technician is working on
        public static List<Ticket> GetTechnicianTickets()
        {
            using (var db = new DutHelpdeskdbContext())
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

                t.RemoveAll(item => item == null);
                return t;
            }
        }

        //returns all tickets in database (not for website)
        public static List<Technician> GetAllTechnicians()
        {
            List<Technician> technicians = db.Technicians.Include(x => x.TicketTechnicians).ToList();
            return technicians;
        }
        public static List<Ticket> GetAllTickets()
        {
            using (var context = new DutHelpdeskdbContext())
            {
                List<Ticket> tickets = context.Tickets.Include(i => i.TicketStatuses).ThenInclude(i => i.Status).Include(i => i.TicketTechnicians).ToList();
                return tickets;
            }
        }

        //returns all ticket technicians
        public static List<TicketTechnician> GetAllTicketTechnicians()
        {
            return db.TicketTechnicians.ToList();
        }

        //returns a list of tickets that have not been assigned to a technician
        public static List<Ticket> GetAllUnassignedTickets()
        {
            using (var db = new DutHelpdeskdbContext())
            {
                List<TicketTechnician> tt = db.TicketTechnicians.ToList();
                List<Ticket> toDelete = db.Tickets.ToList();
                List<Ticket> tickets = toDelete;
                List<int> toRemove = new List<int>();
                foreach (var item in tt)
                {
                    if (!toRemove.Contains((int)item.TicketId) && item.IsAssigned == true)
                    {
                        toRemove.Add((int)item.TicketId);
                    }
                }
                foreach (var tr in toRemove)
                {
                    tickets.Remove(db.Tickets.Where(x => x.TicketId == tr).FirstOrDefault());
                }
                return tickets;
            }
        }

        //gets a single ticket from the database 
        public static Ticket GetTicket(int? id)
        {
            using (var db = new DutHelpdeskdbContext())
            {
                if (id != null)
                {
                    return db.Tickets.Where(x => x.TicketId == id).FirstOrDefault()!;
                }
                else
                {
                    return null!;
                }
            }
        }

        //returns a bool to see if a user saved in state is a technician
        public static string getUserType()
        {
            return user.Type;
        }

        //returns a technician if the user is already assigned as a technician
        public static Technician GetTechnician()
        {
            using (var db = new DutHelpdeskdbContext())
            {
                return db.Technicians.Where(x => x.UserId == user.UserId).FirstOrDefault();
            }
        }

        //returns a single technician by ID
        public static Technician GetTechnician(int id)
        {
            using (var db = new DutHelpdeskdbContext())
            {
                return db.Technicians.Where(x => x.UserId == id).FirstOrDefault();
            }
        }

        //create a ticket technician using technician ID and ticket ID
        public static void CreateTicketTechnician(int id, int techId)
        {
            using (var db = new DutHelpdeskdbContext())
            {
                TicketTechnician? tt = db.TicketTechnicians.Where(x => x.TicketId == id && x.TechnicianId == techId).FirstOrDefault();
                Ticket? t = db.Tickets.Include(i => i.TicketStatuses).ThenInclude(i => i.Status).Where(x => x.TicketId == id).FirstOrDefault();

                //add active to ticketstatus for ticket
                if (db.TicketStatuses.Where(x => x.TicketId == id).OrderByDescending(o => o.TimeStamp).FirstOrDefault().StatusId != 2)
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
                else
                {
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
                if (ticketTech != tt)
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
            int technicianCount = ticketIsAssigned.Count - 1;
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
                Priority = model.Priority.ToString(),
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
                UserId = user.UserId,

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

        //returns the number tickets that a technician has closed.
        public static int GetTechnicianClosedTicketCount(int techId)
        {
            using var db = new DutHelpdeskdbContext();
            List<TicketTechnician> tt = db.TicketTechnicians.Where(x => x.TechnicianId == techId && x.IsAssigned == true).ToList();
            List<TicketStatus> ts = db.TicketStatuses.Where(x => x.StatusId == 3).ToList();
            db.Dispose();
            List<TicketStatus> tts = new List<TicketStatus>();
            foreach (TicketTechnician ticketTechnician in tt)
            {
                foreach (TicketStatus status in ts)
                {
                    if (status.TicketId == ticketTechnician.TicketId)
                    {
                        tts.Add(status);
                    }
                }
            }
            return tts.Count();
        }

        //checks if a ticket is closed, returns true if closed.
        public static bool TicketIsClosed(int ticketID)
        {
            //gets the tickets latest status
            using var db = new DutHelpdeskdbContext();
            TicketStatus? ts = db.TicketStatuses.Where(x => x.TicketId == ticketID).OrderByDescending(x => x.TimeStamp).FirstOrDefault();
            db.Dispose();
            if (ts == null) //if no ticket status, ticket is not closed
            {
                return false;
            }
            else
            {
                if (ts.StatusId == 3) //ticket status 3 = closed
                {
                    return true;
                }
                else
                {
                    return false; //else not closed
                }
            }

        }

        //closes a ticket given the ticketID
        public static async Task CloseTicket(int ticketID)
        {
            using var db = new DutHelpdeskdbContext();
            Ticket? ticket = db.Tickets.Find(ticketID);
            if (ticket != null)
            {
                ticket.DateClosed = DateTime.UtcNow;
            }
            await db.SaveChangesAsync();

            var ticketStatus = new TicketStatus
            {
                TicketId = ticketID,
                StatusId = 3,
                TimeStamp = DateTime.UtcNow
            };

            db.TicketStatuses.Add(ticketStatus);
            await db.SaveChangesAsync();
            db.Dispose();
        }

        //returns the name of the ticket's latest status
        public static string GetTicketStatus(int ticketID)
        {
            //gets the tickets latest status
            using var db = new DutHelpdeskdbContext();
            TicketStatus? ts = db.TicketStatuses.Where(x => x.TicketId == ticketID).OrderByDescending(x => x.TimeStamp).FirstOrDefault();
            if (ts != null)
            {
                Status? status = db.Statuses.Where(x => x.StatusId == ts.StatusId).FirstOrDefault();
                db.Dispose();
                if (status != null && status.Name != null)
                {
                    return status.Name;
                }
                else
                {
                    return "N/A";
                }
            }
            else
            {
                return "N/A";
            }
        }

        //returns true if the student is authorised to access the ticket. False if not.
        public static bool authoriseStudentTicketAccess(int? ticketID)
        {
            Ticket ts = GetTicket(ticketID);
            if (ts != null && user != null)
            {
                if (ts.UserId == user.UserId)
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        public static bool authoriseStudentReplyAccess(int replyID)
        {
            using var db = new DutHelpdeskdbContext();
            Reply? r = db.Replies.Where(x => x.ReplyId == replyID).FirstOrDefault();
            db.Dispose();
            if (r != null)
            {
                return authoriseStudentTicketAccess(r.TicketId);
            }
            return false;
        }

        public static async Task ChangeTicketPriority(int ticketId, string p)
        {
            using var db = new DutHelpdeskdbContext();
            Ticket? ticket = db.Tickets.Find(ticketId);
            if (ticket != null)
            {
                ticket.Priority = p;
            }
            await db.SaveChangesAsync();
            db.Dispose();
        }

        //gets the closed tickets that a specific technician is assigned to.
        public static List<Ticket> GetTechClosedTickets(int techId)
        {
            using var db = new DutHelpdeskdbContext();
            List<TicketTechnician> tt = db.TicketTechnicians.Where(x => x.TechnicianId == techId && x.IsAssigned == true).ToList();
            List<int?> ticketIds = tt.Select(ttItem => ttItem.TicketId).ToList();

            List<Ticket> tickets = db.Tickets
                .Where(t => ticketIds.Contains(t.TicketId) && t.DateClosed != null)
                .ToList();
            db.Dispose();
            return tickets;
        }


        //returns the formatted average resolution time for a list of tickets.
        public static string GetAverageResolutionTime(List<Ticket> tickets)
        {
            List<TimeSpan?> timeSpans = new();
            DateTime? time1 = null;
            DateTime? time2 = null;
            foreach (var ticket in tickets)
            {
                if (ticket.DateCreated == null || ticket.DateClosed == null)
                {
                    continue;
                }
                time1 = ticket.DateCreated;
                time2 = ticket.DateClosed;
                TimeSpan? timeDifference = time2 - time1;
                timeSpans.Add(timeDifference);
            }
            if (timeSpans.Count == 0)
            {
                return "N/A";
            }
            TimeSpan totalDifference = (TimeSpan)timeSpans.Aggregate((x, y) => x + y)!;
            TimeSpan averageDifference = TimeSpan.FromTicks(totalDifference.Ticks / timeSpans.Count);
            string formattedDifference =
                $"{(averageDifference.Days != 0 ? $"{averageDifference.Days} day{(averageDifference.Days != 1 ? "s" : "")}\n" : "")}" +
                $"{(averageDifference.Hours != 0 ? $"{averageDifference.Hours} hr{(averageDifference.Hours != 1 ? "s" : "")}\n" : "")}" +
                $"{(averageDifference.Minutes != 0 ? $"{averageDifference.Minutes} min{(averageDifference.Minutes != 1 ? "s" : "")}\n" : "")}";
            return formattedDifference;
        }


        //checks if a ticket has recieved feedback, returns true if it does.
        public static bool TicketHasFeedback(int ticketId)
        {
            using var db = new DutHelpdeskdbContext();
            Feedback? feedback = null;
            feedback = db.Feedbacks.Where(x => x.TicketId == ticketId).FirstOrDefault();
            db.Dispose();
            if (feedback != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //stores user feedback for a ticket
        public static async Task SubmitFeedback(int ticketId, int rating, string comments)
        {
            if (!TicketHasFeedback(ticketId))
            {
                using var db = new DutHelpdeskdbContext();
                var feedback = new Feedback
                {
                    TicketId = ticketId,
                    Rating = rating,
                    Comments = comments,
                    Date = DateTime.UtcNow
                };
                db.Feedbacks.Add(feedback);
                await db.SaveChangesAsync();
                db.Dispose();
            }
        }

        public static Feedback? GetTicketFeedback(int ticketId)
        {
            using var db = new DutHelpdeskdbContext();
            Feedback? feedback = null;
            if (TicketHasFeedback(ticketId))
            {
                feedback = db.Feedbacks.FirstOrDefault(x => x.TicketId == ticketId);
                db.Dispose();
            }
            return feedback;
        }

    }
}
