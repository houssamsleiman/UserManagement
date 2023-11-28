using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Common.Models
{
    /// <summary>
    ///  Data structure for roles saved in the database
    /// </summary>
    public class UserRole
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } 

        public virtual User User { get; set; }
    }
}
