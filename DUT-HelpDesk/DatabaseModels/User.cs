using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class User
{
    public int UserId { get; set; }

    public string FbId { get; set; } = null!;

    public string? Type { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual ICollection<Technician> Technicians { get; set; } = new List<Technician>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
