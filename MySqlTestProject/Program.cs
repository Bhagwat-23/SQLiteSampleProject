using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace MySqlTestProject
{
	public class Program
	{
		private static string dbConnectionString = @"Data Source=Employee.db";
		private static DataBaseHelper dbHelper;
		private static List<Tuple<int, string, double>> empInfo;

		public static void Main(string[] args)
		{
			dbHelper = new DataBaseHelper(dbConnectionString);
			//Open database connection..
			dbHelper.OpenSqlConnection();

			//Crate Employee table..
			CreateEmployeeTable();

			//fetching last employee's id
			int lastEmpID = FetchLastEmployeeID();

			//Insert data into Employee table..
			InsertEmployeeTable(++lastEmpID,"bhagwat",1200.50);
			InsertEmployeeTable(++lastEmpID, "sanjay", 1300);
			InsertEmployeeTable(++lastEmpID, "Rahul", 1500.67);

			//Select Employees record
			SelectEmployeeTable();
			//Display Employees record
			Console.WriteLine("\nAfter inserting, employee data\n");
			DisplayEmployeeRecords();

			//Count Employees record..
			int count = CountEmployee();

			//Update employee table..
			UpdateEmployeeTable();

			//Display Employees record
			Console.WriteLine("\nAfter updating, employee data\n");
			DisplayEmployeeRecords();

			//Delete employee whose id is 5
			DeleteEmployee(5);

			//Display Employees record
			Console.WriteLine("\nAfter deleting, employee data\n");
			DisplayEmployeeRecords(); ;

			//Close database connection...
			dbHelper.CloseSqlConnection();
			Console.ReadKey();
		}

		


		#region Create Employee Table..
		/// <summary>
		/// Employee Table..
		/// </summary>
		private static void CreateEmployeeTable()
		{
			try
			{
				if (!dbHelper.IsOpen)
					dbHelper.OpenSqlConnection();

				if (dbHelper.IsOpen)
				{ 
					string sql = "create table if not exists Employee(empID int auto increment primary key,name varchar(20),salary decimal(15,6))";
					using (SQLiteCommand sqlCommand = new SQLiteCommand(sql, dbHelper.sqlConnection))
					{
						sqlCommand.ExecuteNonQuery();
						Console.WriteLine("Table Created successfully.");
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Can not create table.");
			}
		}
		#endregion

		#region Insert into employee table...
		/// <summary>
		/// Employee Details..
		/// </summary>
		/// <param name="empID">Employee ID (int)</param>
		/// <param name="name">Employee Name (String)</param>
		/// <param name="salary">Employee Salary (Double)</param>
		private static void InsertEmployeeTable(int empID,string name,double salary)
		{
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{ 
				string command = "insert into Employee (empID, name, salary) values(@id,@name, @salary)";
				using(SQLiteCommand sqlCommand=new SQLiteCommand(command, dbHelper.sqlConnection))
				{
					sqlCommand.CommandType = CommandType.Text;
					sqlCommand.Parameters.Add(new SQLiteParameter("@id", empID));
					sqlCommand.Parameters.Add(new SQLiteParameter("@name", name));
					sqlCommand.Parameters.Add(new SQLiteParameter("@salary", salary));
					sqlCommand.ExecuteNonQuery();
				}
			}
		}
		#endregion

		#region Select Employee
		/// <summary>
		/// Retrieve/Select employee records.
		/// </summary>
		private static void SelectEmployeeTable()
		{
			empInfo = new List<Tuple<int, string, double>>();
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{ 
				string sql = "select * from Employee";
				using(SQLiteCommand command=new SQLiteCommand(sql, dbHelper.sqlConnection))
				{
					command.CommandType = CommandType.Text;
					using(SQLiteDataReader reader = command.ExecuteReader())
					{
						while(reader.Read())
						{
							int id = Convert.ToInt32(reader["empID"]);
							string name = (string)reader["name"];
							double salary = Convert.ToDouble(reader["salary"]);
							empInfo.Add(new Tuple<int, string, double>(id,name ,salary) );
						}
					}
				}
			}
		}
		#endregion

		#region Update Employee
		/// <summary>
		/// UpdateEmployeeTable....
		/// </summary>
		private static void UpdateEmployeeTable()
		{
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{
				string sql = "update Employee set name='bicky' where name='bhagwat'";
				using(SQLiteCommand command = new SQLiteCommand(sql, dbHelper.sqlConnection))
				{
					command.CommandType = CommandType.Text;
					command.ExecuteNonQuery();
				}
			}
		}
		#endregion

		#region Delete Employee record
		/// <summary>
		/// Pass employee id as a parameter...
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static void DeleteEmployee(int id)
		{
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{
				string sql = "delete from Employee where empID=" + "'" + id + "'";
				using(SQLiteCommand command = new SQLiteCommand(sql, dbHelper.sqlConnection))
				{
					command.CommandType = CommandType.Text;
					command.ExecuteNonQuery();
				}
			}
		}
		#endregion

		#region Count statement
		/// <summary>
		/// Count total number of employees
		/// </summary>
		/// <returns></returns>
		public static int CountEmployee()
		{
			int count = 0;
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{
				string sql = "select count(*) from Employee";
				using(SQLiteCommand command = new SQLiteCommand(sql, dbHelper.sqlConnection))
				{
					command.CommandType = CommandType.Text;
					count = Convert.ToInt32(command.ExecuteScalar());
				}
			}
			return count;
		}
		#endregion

		#region Fetch last employee id...
		/// <summary>
		/// Fetch last employee id...
		/// </summary>
		/// <returns></returns>
		private static int FetchLastEmployeeID()
		{
			int lastEmpID = -1;
			if (!dbHelper.IsOpen)
				dbHelper.OpenSqlConnection();

			if (dbHelper.IsOpen)
			{
				string sql = "select *from Employee order by empID desc limit 1";
				using(SQLiteCommand command = new SQLiteCommand(sql, dbHelper.sqlConnection))
				{
					command.CommandType = CommandType.Text;
					lastEmpID = Convert.ToInt32(command.ExecuteScalar());
				}
			}
			return lastEmpID;
		}
		#endregion

		#region Display employee record...
		private static void DisplayEmployeeRecords()
		{
			SelectEmployeeTable();
			foreach(var emp in empInfo)
			{
				Console.WriteLine(string.Format("EmpID: {0}\t Name: {1}\t Salary: {2}",emp.Item1,emp.Item2,emp.Item3));
			}
		}
		#endregion
	}
}
