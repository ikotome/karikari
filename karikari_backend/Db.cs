using Microsoft.EntityFrameworkCore;

namespace karikari_backend;

public class Db : DbContext
{
    public DbSet<Models.User> Users { get; set; }
    public DbSet<Models.Event> Events { get; set; }
    public DbSet<Models.Group> Groups { get; set; }
    
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
}