using DUT_HelpDesk.Controllers;
using DUT_HelpDesk.DatabaseModels;

namespace DUT_HelpDesk_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void GetUsers()
        {
            List<User> test = StateManager.GetUsers();
            if(test != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void GetFaqs()
        {
            List<Faq> test = StateManager.GetAllFaqs();
            if (test != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void GetTickets()
        {
            List<Ticket> test = StateManager.GetAllTickets();
            if (test != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}