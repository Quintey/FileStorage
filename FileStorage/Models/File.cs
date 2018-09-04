using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileStorage.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        
        public byte[] NewFile { get; set; }

        public string FileExt { get; set; }
        public string Date { get; set; }
        public string FileSize { get; set; }
        public string UserName { get; set; }
    }
}