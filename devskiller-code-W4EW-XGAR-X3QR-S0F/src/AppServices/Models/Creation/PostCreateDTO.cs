using System.ComponentModel.DataAnnotations;

namespace AppServices.Models
{
	public class PostCreateDTO
	{
        
        [Display(Name = "Title")]
       [StringLength(30, ErrorMessage = "Title should contain equal or more than {1} and less or equals to {2} characters")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Content")]
        [StringLength(1200, ErrorMessage = "Content should contain equal or more than {1} and less or equals to {2} characters")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "CreationDate")]
        public DateTime CreationDate { get; set; }
    }
}

