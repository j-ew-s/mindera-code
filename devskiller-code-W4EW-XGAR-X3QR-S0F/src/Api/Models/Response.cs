namespace Api.Models
{
	public class Response<T>
	{
		public T Content { get; set; }

		public Response()
		{
		}

		public Response(T content)
		{
			this.Content = content;
		}
	}
}

