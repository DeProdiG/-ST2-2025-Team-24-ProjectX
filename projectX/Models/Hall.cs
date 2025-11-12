using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class Hall
    {
        public int Id { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
    }
}
