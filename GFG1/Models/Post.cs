using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFG1.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
  

        [Required]
        public string Area { get; set; }

        [Required]
        public string FullAddress { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        public string ShortDescription { get; set; }
        public string Status { get; set; } = "unclaimed";
        public string Photo { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}
