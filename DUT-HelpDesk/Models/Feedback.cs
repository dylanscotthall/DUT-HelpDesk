using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? TechnicianId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? Date { get; set; }

    public virtual Technician? Technician { get; set; }
}
