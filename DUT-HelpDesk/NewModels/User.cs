using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.NewModels;

public partial class User
{
    public int UserId { get; set; }

    public string FbId { get; set; } = null!;

    public string? Type { get; set; }

    public virtual ICollection<Technician> Technicians { get; set; } = new List<Technician>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
