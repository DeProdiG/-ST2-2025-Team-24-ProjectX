using System.ComponentModel.DataAnnotations;

namespace projectX.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public ICollection<MovieActors>? MovieActors { get; set; } = new List<MovieActors>();
    }
}
