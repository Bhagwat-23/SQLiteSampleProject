using System;
using System.Data.SQLite;
using System.IO;

namespace MySqlTestProject
{
	public class DataBaseHelper
	{
		
		public bool IsOpen;
		public SQLiteConnection sqlConnection;
		public DataBaseHelper(string dbConnectionString)
		{
			InitDatabase(dbConnectionString);
			IsOpen = false;
			sqlConnection = new SQLiteConnection(dbConnectionString);
		}

		private void InitDatabase(string dbConnectionString)
		{
			sqlConnection = new SQLiteConnection(dbConnectionString);
			if(!File.Exists("./Employee.db"))
			{
				SQLiteConnection.CreateFile("Employee.db");
				Console.WriteLine("Employee Database is created!!!");
			}
		}

		public void OpenSqlConnection()
		{
			try
			{
				sqlConnection.Open();
				IsOpen = true;
				
			}
			catch(Exception ex)
			{
				Console.WriteLine("Can not open sql connection.");
			}
		}

		public void CloseSqlConnection()
		{
			try
			{
				sqlConnection.Close();
				IsOpen = false;
			}
			catch(Exception ex)
			{
				Console.WriteLine("Can not close sql connection.");
			}
		}
	}
}
