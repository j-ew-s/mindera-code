using System.Text.Json.Serialization;
using Model.Entities;

namespace AppServices.Models
{
	public record PostDTO : BaseDTO
	{
        public string Title { get; set; }
        public string Content { get; set; }
       
    }
}

