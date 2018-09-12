using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FileStorage.Models
{
    public class FileContext : DbContext

    {
        public FileContext(): base("FileContext") { }

        public DbSet<File> Files { get; set; }

    }
}