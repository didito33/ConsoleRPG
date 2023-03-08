using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using RPGInterviewTask.DTOs;

namespace RPGInterviewTask.Data
{
    internal class ConsoleRPGDatabase : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.connectionString);
        }
    }
}
