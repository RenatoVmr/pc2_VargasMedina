using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pc2_VargasRenatoSebastian.Models
{
    public enum TipoInmueble { Departamento, Casa, Oficina, Local }
    public class Inmueble
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } // único
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        public string Imagen { get; set; }
        [Required]
        public TipoInmueble Tipo { get; set; }
        [Required]
        [StringLength(50)]
        public string Ciudad { get; set; }
        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }
        [Range(0, 100)]
        public int Dormitorios { get; set; }
        [Range(0, 20)]
        public int Banos { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MetrosCuadrados debe ser > 0")]
        public int MetrosCuadrados { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Precio debe ser > 0")]
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        public ICollection<Visita> Visitas { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }

    public enum EstadoVisita { Solicitada, Confirmada, Cancelada }
    public class Visita
    {
        public int Id { get; set; }
        [Required]
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DateGreaterThan("FechaInicio", ErrorMessage = "FechaFin debe ser mayor a FechaInicio")]
        public DateTime FechaFin { get; set; }
        [Required]
        public EstadoVisita Estado { get; set; }
        public string Notas { get; set; }
    }

    public class Reserva
    {
        public int Id { get; set; }
        [Required]
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        [Required]
        public DateTime FechaExpiracion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
    }

    // Custom validation attribute
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        public DateGreaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Property {_otherPropertyName} not found");
            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
            if (value is DateTime thisDate && otherValue is DateTime otherDate)
            {
                if (thisDate > otherDate)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
