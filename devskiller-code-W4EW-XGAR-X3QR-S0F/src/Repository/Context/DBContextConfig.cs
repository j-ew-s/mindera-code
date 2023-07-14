using System;
namespace Repository.Context
{
	public class DBContextConfig : IDBContextConfig
    {
		public DBContextConfig()
		{
		}

        public string ConnectionString { get; set; }
    }
}

