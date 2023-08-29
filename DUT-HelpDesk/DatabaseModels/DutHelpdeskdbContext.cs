using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk.DatabaseModels;

public partial class DutHelpdeskdbContext : DbContext
{
    public DutHelpdeskdbContext()
    {
    }

    public DutHelpdeskdbContext(DbContextOptions<DutHelpdeskdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=tcp:bitdevs.database.windows.net,1433;Initial Catalog=DUT_Helpdeskdb;Persist Security Info=False;User ID=BitDevs;Password=Codebit7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__Attachme__97E3B2DF09FC50EB");

            entity.ToTable("Attachment");

            entity.Property(e => e.AttachmentId).HasColumnName("Attachment_id");
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.ReplyId).HasColumnName("Reply_id");
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Reply).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.ReplyId)
                .HasConstraintName("FK__Attachmen__Reply__00200768");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Attachmen__Ticke__7F2BE32F");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__CDC95E70C940C92A");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("Feedback_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Feedback__Ticket__7C4F7684");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__B660369CBC5BE137");

            entity.ToTable("Reply");

            entity.Property(e => e.ReplyId).HasColumnName("Reply_id");
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Replies)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Reply__Ticket_id__797309D9");
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__E70521DB1D357979");

            entity.ToTable("Technician");

            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.DateJoined)
                .HasColumnType("date")
                .HasColumnName("Date_joined");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Technicians)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Technicia__User___72C60C4A");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7364E1150AA720");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");
            entity.Property(e => e.DateClosed).HasColumnType("smalldatetime");
            entity.Property(e => e.DateCreated).HasColumnType("smalldatetime");
            entity.Property(e => e.Priority)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.QueryBody)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Technician).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TechnicianId)
                .HasConstraintName("FK__Ticket__Technici__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ticket__User_id__75A278F5");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF8EAAA4D75");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.FbId)
                .IsUnicode(false)
                .HasColumnName("Fb_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
