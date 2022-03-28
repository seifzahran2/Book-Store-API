using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPIProject.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "First Name cqan't be more than 100 character")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Last Name cqan't be more than 100 character")]
        public string LastName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<BookAuthors> BookAuthors { get; set; }

    }
}
