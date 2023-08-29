using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? TicketId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? Date { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
