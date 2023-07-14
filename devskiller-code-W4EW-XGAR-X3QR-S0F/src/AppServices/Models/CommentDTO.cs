using System.ComponentModel.DataAnnotations;
using Model.Entities;

namespace AppServices.Models
{
	public record CommentDTO : BaseDTO
	{
        
        public Guid PostId { get; set; } 
        public string Content { get; set; }
        public string Author { get; set; }
        public Post Post { get; set; }
    }
}

