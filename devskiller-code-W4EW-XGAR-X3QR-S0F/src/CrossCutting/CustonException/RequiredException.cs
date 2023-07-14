using System;
namespace CrossCutting.CustonException
{
	public class RequiredException : Exception
	{
		public RequiredException(string message):base(message)
		{
		}
	}
}

