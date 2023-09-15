using System;
using System.Collections.Generic;
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

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Technician> Technicians { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketStatus> TicketStatuses { get; set; }

    public virtual DbSet<TicketTechnician> TicketTechnicians { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:bitdevs.database.windows.net,1433;Initial Catalog=DUT_Helpdeskdb;Persist Security Info=False;User ID=BitDevs;Password=Codebit7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__Attachme__97E3B2DFDF5C18B9");

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
                .HasConstraintName("FK__Attachmen__Reply__17036CC0");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Attachmen__Ticke__160F4887");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__CDC95E708BF422E3");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("Feedback_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Feedback__Ticket__1332DBDC");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__B660369C96436297");

            entity.ToTable("Reply");

            entity.Property(e => e.ReplyId).HasColumnName("Reply_id");
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Replies)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__Reply__Ticket_id__10566F31");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Status__5191052464E0DACE");

            entity.ToTable("Status");

            entity.Property(e => e.StatusId).HasColumnName("Status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK__Technici__E70521DBA9F6FF85");

            entity.ToTable("Technician");

            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.DateJoined)
                .HasColumnType("date")
                .HasColumnName("Date_joined");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Technicians)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Technicia__User___09A971A2");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__ED7364E1662C3DFE");

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
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Technician).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TechnicianId)
                .HasConstraintName("FK__Ticket__Technici__0D7A0286");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ticket__User_id__0C85DE4D");
        });

        modelBuilder.Entity<TicketStatus>(entity =>
        {
            entity.HasKey(e => e.TicketStatusId).HasName("PK__TicketSt__72C70E96A8837C8C");

            entity.ToTable("TicketStatus");

            entity.Property(e => e.TicketStatusId).HasColumnName("TicketStatus_id");
            entity.Property(e => e.StatusId).HasColumnName("Status_id");
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");
            entity.Property(e => e.TimeStamp).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Status).WithMany(p => p.TicketStatuses)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__TicketSta__Statu__1CBC4616");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketStatuses)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__TicketSta__Ticke__1BC821DD");
        });

        modelBuilder.Entity<TicketTechnician>(entity =>
        {
            entity.HasKey(e => e.TicketTechnicianId).HasName("PK__TicketTe__FA15DDB859E6946E");

            entity.ToTable("TicketTechnician");

            entity.Property(e => e.TicketTechnicianId).HasColumnName("TicketTechnician_id");
            entity.Property(e => e.IsAssigned).HasColumnName("isAssigned");
            entity.Property(e => e.TechnicianId).HasColumnName("Technician_id");
            entity.Property(e => e.TicketId).HasColumnName("Ticket_id");
            entity.Property(e => e.TimeStamp).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Technician).WithMany(p => p.TicketTechnicians)
                .HasForeignKey(d => d.TechnicianId)
                .HasConstraintName("FK__TicketTec__Techn__208CD6FA");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketTechnicians)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK__TicketTec__Ticke__1F98B2C1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__206A9DF84B2B4043");

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
