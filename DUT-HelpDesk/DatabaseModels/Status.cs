using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Status
{
    public int StatusId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TicketStatus> TicketStatuses { get; set; } = new List<TicketStatus>();
}
