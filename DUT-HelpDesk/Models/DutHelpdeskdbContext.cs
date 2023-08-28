using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk.Models;

public partial class DutHelpdeskdbContext : DbContext
{
    public DutHelpdeskdbContext()
    {
    }

    public DutHelpdeskdbContext(DbContextOptions<DutHelpdeskdbContext> options)
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
        => optionsBuilder.UseSqlServer("Server=tcp:bitdevs.database.windows.net,1433;Initial Catalog=DUT_Helpdeskdb;Persist Security Info=False;User ID=BitDevs;Password=Codebit7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__CDC95E701F1F5AF8");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("Feedback_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");

            entity.HasOne(d => d.Technician).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TechnicianId)
                .HasConstraintName("FK__Feedback__Techni__6477ECF3");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__B660369C410D26AE");

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
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__E70521DB60BEA32A");

            entity.ToTable("Technician");

            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.DateJoined)
                .HasColumnType("date")
                .HasColumnName("Date_joined");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Technicians)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Technicia__User___66603565");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7364E1D09245EC");

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
                .HasConstraintName("FK__Ticket__Technici__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ticket__User_id__6754599E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF8D504C402");

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
