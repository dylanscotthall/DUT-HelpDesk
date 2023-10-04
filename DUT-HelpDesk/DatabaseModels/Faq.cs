using System;
using System.Collections.Generic;

namespace DUT_HelpDesk.DatabaseModels;

public partial class Faq
{
    public int FaqId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public int TechnicianId { get; set; }

    public virtual Technician Technician { get; set; } = null!;
}
