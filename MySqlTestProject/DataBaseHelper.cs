using System;
using System.Data.SQLite;
using System.IO;

namespace MySqlTestProject
{
	public class DataBaseHelper
	{
		#region Fields...
		public bool IsOpen;
		public SQLiteConnection sqlConnection;
		#endregion

		#region Constructor
		/// <summary>
		/// Inject db connection string...
		/// </summary>
		/// <param name="dbConnectionString"></param>
		public DataBaseHelper(string dbConnectionString)
		{
			InitDatabase(dbConnectionString);
			IsOpen = false;
			sqlConnection = new SQLiteConnection(dbConnectionString);
		}
		#endregion

		#region Initialize Database
		/// <summary>
		/// Initialize Database
		/// </summary>
		/// <param name="dbConnectionString"></param>
		private void InitDatabase(string dbConnectionString)
		{
			sqlConnection = new SQLiteConnection(dbConnectionString);
			if(!File.Exists("./Employee.db"))
			{
				SQLiteConnection.CreateFile("Employee.db");
				Console.WriteLine("Employee Database is created!!!");
			}
		}
		#endregion

		#region Open SQL db Connection
		/// <summary>
		/// Open Sql connection
		/// </summary>
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
		#endregion

		#region Closed sql db connection..
		/// <summary>
		/// Close sql connection.
		/// </summary>
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
		#endregion
	}
}
