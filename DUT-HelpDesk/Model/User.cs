using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.Model;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Technician> Technicians { get; set; } = new List<Technician>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
