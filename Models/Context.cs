using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVClog.Models
{
    public class Context : DbContext
    {
        //dbset es para mapear
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Contaminante> Contaminantes { get; set; }

        public DbSet<AutomaticoPM> AutomaticoPMs { get; set; }
        public DbSet<ActivoPM> ActivoPMs { get; set; }

        public DbSet<RangoICA> RangoICAs { get; set; }

        public DbSet<PmActivo> PmActivos { get; set; }
        public DbSet<NivelContaminacion> NivelContaminantes { get; set; }

        public DbSet<Pasivo> Pasivoes { get; set; }

        public DbSet<Estacion> Estacions { get; set; }
      
        
       
    }

}