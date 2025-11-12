using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace projectX.Models
{
    public class Screening
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public DateTime ScreeningTime { get; set; }
        public ICollection<ScreeningMovies> ScreeningMovies { get; set; } = new List<ScreeningMovies>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<ScreeningCinemas> ScreeningCinemas { get; set; } = new List<ScreeningCinemas>();

    }
}
