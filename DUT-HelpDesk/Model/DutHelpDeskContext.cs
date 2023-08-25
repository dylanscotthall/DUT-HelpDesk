using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk.Model;

public partial class DutHelpDeskContext : DbContext
{
    public DutHelpDeskContext()
    {
    }

    public DutHelpDeskContext(DbContextOptions<DutHelpDeskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:duthelpdeskserver.database.windows.net,1433;Initial Catalog=Dut-HelpDesk;Persist Security Info=False;User ID=admin0;Password=admin12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__CDC95E703A4A9335");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("Feedback_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");

            entity.HasOne(d => d.Technician).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TechnicianId)
                .HasConstraintName("FK__Feedback__Techni__68487DD7");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__B660369C65E5E171");

            entity.ToTable("Reply");

            entity.Property(e => e.ReplyId).HasColumnName("Reply_id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Replies)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Reply__Ticket_id__656C112C");
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__E70521DB10F159BE");

            entity.ToTable("Technician");

            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.DateJoined)
                .HasColumnType("date")
                .HasColumnName("Date_joined");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Technicians)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Technicia__User___5EBF139D");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7364E1EDB961AF");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");
            entity.Property(e => e.DateClosed).HasColumnType("date");
            entity.Property(e => e.DateCreated).HasColumnType("date");
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
                .HasConstraintName("FK__Ticket__Technici__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ticket__User_id__619B8048");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF83AFD1B36");

            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
