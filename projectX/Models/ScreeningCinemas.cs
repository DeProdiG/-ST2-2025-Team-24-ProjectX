namespace projectX.Models
{
    public class ScreeningCinemas
    {
        public int ScreeningId { get; set; }
        public Screening? Screening { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
    }
}
