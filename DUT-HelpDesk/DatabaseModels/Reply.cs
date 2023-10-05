using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Reply
{
    public int ReplyId { get; set; }

    public int? TicketId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Ticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
