using System;
namespace CrossCutting.CustonException
{
	public class NotExistException : Exception
	{
		public NotExistException(string message) :base(message)
		{
		}
	}
}

