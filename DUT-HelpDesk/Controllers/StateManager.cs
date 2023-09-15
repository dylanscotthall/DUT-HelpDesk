﻿using DUT_HelpDesk.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DUT_HelpDesk.Controllers
{
    public static class StateManager
    {
        private static DatabaseModels.DutHelpdeskdbContext db = new DatabaseModels.DutHelpdeskdbContext();
        public static DatabaseModels.User user;
        public static Technician technician;
        public static string token;
        public static List<DatabaseModels.User> GetUsers()
        {
            return db.Users.ToList();
        }
        public static DatabaseModels.User GetUser(int id)
        {
            return db.Users.Where(x => x.UserId == id).FirstOrDefault();
        }
        public static DatabaseModels.User GetUserByFBId(string id)
        {
            return db.Users.Where(x => x.FbId == id).FirstOrDefault();
        }
        public static void AddUser(DatabaseModels.User user)
        {
            db.Users.Add(user);
        }
        public static void DeleteUser(int id)
        {

        }
        public static void UpdateUser(DatabaseModels.User user)
        {
            db.Users.Update(user);
        }
        public static List<Ticket> GetUserTickets()
        {
            return db.Tickets.Where(x => x.UserId == user.UserId).ToList();
        }
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
        public static List<Ticket> GetAllTickets()
        {
            return db.Tickets.ToList();
        }
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
        public static void CreateTicketTechnician(int id, int techId)
        {
            TicketTechnician tt = db.TicketTechnicians.Where(x => x.TicketId == id && x.TechnicianId == techId).FirstOrDefault();
            if(tt != null)
            {
                tt.IsAssigned = true;
                db.Entry(tt).State = EntityState.Modified;
            }
            else{
                TicketTechnician newTT = new TicketTechnician()
                {
                    TicketId = id,
                    TechnicianId = techId,
                    IsAssigned = true,
                    TimeStamp = DateTime.Now
                };
                db.TicketTechnicians.Add(newTT);
            }
            
            db.SaveChanges();
        }
        public static void UnassignTicketTechnician(int id, int techId)
        {
            TicketTechnician tt = db.TicketTechnicians.Where(x => x.TicketId == id && x.TechnicianId == techId).FirstOrDefault();
            tt.IsAssigned = false;
            db.Entry(tt).State = EntityState.Modified;
            db.SaveChanges();
        }
        public static async void CreateTicket(TicketViewModel model, FirebaseAuthProvider auth)
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
                DateCreated = DateTime.Now,

            };

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
    }
}
