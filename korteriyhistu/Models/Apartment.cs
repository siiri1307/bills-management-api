using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class Apartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int number { get; set; }
        public double surfaceArea { get; set; }
        public double extraSurfaceArea { get; set; } //võetud lisakohustuse all olev pind
        public string owners { get; set; }
    }
}
