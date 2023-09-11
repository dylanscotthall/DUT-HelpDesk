using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Technician
{
    public int TechnicianId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DateJoined { get; set; }

    public virtual ICollection<TicketTechnician> TicketTechnicians { get; set; } = new List<TicketTechnician>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User? User { get; set; }
}
