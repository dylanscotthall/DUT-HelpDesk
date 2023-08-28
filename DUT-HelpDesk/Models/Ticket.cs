using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? UserId { get; set; }

    public int? TechnicianId { get; set; }

    public string? Subject { get; set; }

    public string? QueryBody { get; set; }

    public byte[]? Attachment { get; set; }

    public string? Priority { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Status { get; set; }

    public DateTime? DateClosed { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual Technician? Technician { get; set; }

    public virtual User? User { get; set; }
}
