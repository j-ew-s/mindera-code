using System;
namespace AppServices.Models.Results
{
	public record ResultDTO<T>
	{
		public T Content { get; set; }

		public ResultDTO(T content)
		{
			this.Content = content;
		}
	}
}

