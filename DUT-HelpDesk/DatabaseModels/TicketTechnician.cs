using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class TicketTechnician
{
    public int TicketTechnicianId { get; set; }

    public int? TicketId { get; set; }

    public int? TechnicianId { get; set; }

    public bool? IsAssigned { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual Technician? Technician { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
