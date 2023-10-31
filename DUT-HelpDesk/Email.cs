namespace DUT_HelpDesk
{
    using MailKit.Net.Smtp;
    using MailKit.Net.Pop3;
    using MailKit;
    using MimeKit;
    using MailKit.Net.Imap;
    using DUT_HelpDesk.DatabaseModels;

    public class Email
    {
        public async static void CreatedTicketMailAsync(string studentMail, int ticketId)
        {
            Console.WriteLine(studentMail);
            IConfiguration config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

            string sendMail = config["MyEmail"];
            string student = studentMail.Split('@')[0];

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("DUT Support", sendMail));
                message.To.Add(new MailboxAddress("Student", studentMail));
                message.Subject = "Ticket Submission";


                message.Body = new TextPart("plain")
                {
                    Text = @$"Hi {student},

                    Your ticket has been received. Your ticket id is {ticketId}."
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    Console.WriteLine("connecting to server...");
                    await client.AuthenticateAsync(sendMail, config["MyPassword"]);

                    await client.SendAsync(message);
                    Console.WriteLine("Message sent!");
                    await client.DisconnectAsync(true);
                    Console.WriteLine("Disconnecting from server...");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async static void SelectedMailAsync(string mail,int id, DateTime? openDate,string priority, string subject, string body)
        {
            Console.WriteLine(mail);
            IConfiguration config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

            string sendMail = config["MyEmail"];
            string receiver = mail.Split('@')[0];

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("DUT Support", sendMail));
                message.To.Add(new MailboxAddress("Technician", mail));
                message.Subject = "Ticket Submission";


                message.Body = new TextPart("plain")
                {
                    Text = $"Hi {receiver}, you are receiving a forwarded ticket." + $"\nHere are the ticket details:" + 
                    $"\nTicketId : {id}" +
                    $"\nOpenDate : {openDate}" + $"\nPriority : {priority}" +
                    $"\nSubject : {subject}" +  $"\nQuery : {body}"
                    
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    Console.WriteLine("connecting to server...");
                    await client.AuthenticateAsync(sendMail, config["MyPassword"]);

                    await client.SendAsync(message);
                    Console.WriteLine("Message sent!");
                    await client.DisconnectAsync(true);
                    Console.WriteLine("Disconnecting from server...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
