using ExamenNetCoreMacarena.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenNetCoreMacarena.Data
{
    public class ZapatillaContext: DbContext
    {
        public ZapatillaContext(DbContextOptions<ZapatillaContext> options) : base(options) { }
        
        public DbSet<Zapatilla> Zapatillas { get; set; }
        public DbSet<ImagenesZapatilla> ImagenesZapatillas { get; set; }
    }
}
