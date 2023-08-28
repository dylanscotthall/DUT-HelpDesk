using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.Models;

public partial class Reply
{
    public int ReplyId { get; set; }

    public int? TicketId { get; set; }

    public string? Message { get; set; }

    public byte[]? Attachment { get; set; }

    public DateTime? Date { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
