using accesosIp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.DBContexts
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tAcceso>().ToTable("tAcceso");
            modelBuilder.Entity<tusuario>().ToTable("tusuario");
          
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configuracion = ConfigurationHelper.GetConfiguration(Directory.GetCurrentDirectory());
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(configuracion.GetSection("ConnectionStrings")["DefaultConnection"]);
        //}

        public virtual void Save()
        {
            base.SaveChanges();
        }
    
        public DbSet<tAcceso> Acceso { get; set; }
        public DbSet<tusuario> Usuario { get; set; }
    }
}
