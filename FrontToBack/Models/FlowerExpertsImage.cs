using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class FlowerExpertsImage
    {
        public int Id { get; set; }
        public string Image { get; set; }

        [Required (ErrorMessage ="Zehmet olmasa xanani dolurun"),MaxLength(50)]
        public string Name { get; set; }

        [Required (ErrorMessage ="Zehmet olmasa xanani dolurun"), MaxLength(50)]
        public string Experts { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
