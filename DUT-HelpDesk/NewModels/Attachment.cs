using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.NewModels;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public int? TicketId { get; set; }

    public int? ReplyId { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileContent { get; set; }

    public string? ContentType { get; set; }

    public virtual Reply? Reply { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
