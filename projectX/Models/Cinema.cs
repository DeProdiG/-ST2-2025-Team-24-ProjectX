using System;
using System.ComponentModel.DataAnnotations;

namespace projectX.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Adress { get; set; }
        public ICollection<Hall>? Halls { get; set; }
        public ICollection<ScreeningCinemas>? ScreeningCinemas { get; set; }
    }
}
