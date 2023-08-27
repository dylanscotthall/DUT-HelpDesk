using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DUT_HelpDesk;

public partial class BitdevsdbContext : DbContext
{
    public BitdevsdbContext()
    {
    }

    public BitdevsdbContext(DbContextOptions<BitdevsdbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:bitdevs.database.windows.net,1433;Initial Catalog=bitdevsdb;Persist Security Info=False;User ID=BitDevs;Password=Codebit7;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
