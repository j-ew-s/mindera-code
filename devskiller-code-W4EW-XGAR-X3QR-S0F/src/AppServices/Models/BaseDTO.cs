namespace AppServices.Models
{
	public record BaseDTO
	{
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

