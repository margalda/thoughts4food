using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace WebRole1
{
    public class QueriesRunner
    {
        public static bool ValueExists(string table, string column, string value)
        {
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append($"SELECT * FROM {table} WHERE {column} = '{value}';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return reader.Read();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool InsertToTable(string table, List<string> values)
        {
            try
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder
                    {
                        DataSource = "thoughts4food.database.windows.net",
                        UserID = "thoughts4food",
                        Password = "Kfc369nba",
                        InitialCatalog = "thoughts4foodSQL"
                    };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    StringBuilder sb = new StringBuilder();
                    sb.Append($"INSERT INTO {table} ");
                    sb.Append("VALUES(");
                    foreach (var value in values)
                    {
                        sb.Append($"'{value}', ");
                    }
                    sb.Length = sb.Length - 2;
                    sb.Append(");");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}