using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class CompaniaDto
    {
            
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre  de la compania es requerido")]
        [MaxLength(100, ErrorMessage ="No sea mayor de 100")]
        public string NombreCompania { get; set; }

        [Required(ErrorMessage ="El nombre  de la Direccion es requerido")]
        [MaxLength( 150, ErrorMessage ="No sea mayor de 150")]
        public string Dirreccion { get; set; }

        [Required(ErrorMessage ="El nombre  de la Telefono es requerido")]
        [MaxLength( 40, ErrorMessage ="No sea mayor de 40")]
        public string Telefono { get; set; }
        
        [MaxLength( 40, ErrorMessage ="No sea mayor de 40")]
        public string Telefono2 { get; set; }                                                                                                                

    }
}