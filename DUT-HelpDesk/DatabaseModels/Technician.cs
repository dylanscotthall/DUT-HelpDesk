using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Technician
{
    public int TechnicianId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DateJoined { get; set; }

    public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();

    public virtual ICollection<TicketTechnician> TicketTechnicians { get; set; } = new List<TicketTechnician>();

    public virtual User? User { get; set; }
}
