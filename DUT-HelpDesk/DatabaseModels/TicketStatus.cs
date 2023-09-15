using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class TicketStatus
{
    public int TicketStatusId { get; set; }

    public int? TicketId { get; set; }

    public int? StatusId { get; set; }

    public DateTime? TimeStamp { get; set; }

    public virtual Status? Status { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
