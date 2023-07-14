using System;
using System.ComponentModel.DataAnnotations;

namespace AppServices.Models.Creation
{
	public class CommentCreateDTO
	{
        [Required]
        public Guid PostId { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "Content should contain equal or more than {1} and less or equals to {2} characters")]
        public string Content { get; set; }
        [Required]
        [StringLength( 30, ErrorMessage = "Author should contain equal or more than {1} and less or equals to {2} characters")]
        public string Author { get; set; }

        [Display(Name = "CreationDate")]
        public DateTime CreationDate { get; set; }
    }
}

