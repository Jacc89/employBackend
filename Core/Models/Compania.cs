using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Compania
    {
        [Key]
        public int Id { get; set; }
        public string NombreCompania { get; set; }
        public string Dirreccion { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        
    }
}