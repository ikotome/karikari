using Microsoft.EntityFrameworkCore;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.Backend;

public class Db : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        // デバッグ中は、Dockerで立っているDBに外から接続する
        optionsBuilder.UseSqlServer(@"Data Source=localhost; Initial Catalog=todolist; User ID=sa; Password=jMJWpbHG75Gw; TrustServerCertificate=true;");
#else
        // 本番環境では、Docker内のDBに接続する
        optionsBuilder.UseSqlServer(@"Data Source=db; Initial Catalog=todolist; User ID=sa; Password=jMJWpbHG75Gw; TrustServerCertificate=true;");
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 問題のある外部キー制約に対してNO ACTIONを指定
        modelBuilder.Entity<Models.Loan>()
            .HasOne(l => l.Repayer)
            .WithMany()
            .HasForeignKey(l => l.RepayerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}