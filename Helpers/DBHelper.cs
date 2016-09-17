using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using MehNotifications.Data;

namespace MehNotifications.Helpers
{
    public class DBHelper
    {
        private static string _connectionString = "**connection string**";
        
        public DBHelper()
        {
        }

        public static List<Notification> GetUsers()
        {
            var data = ExecuteCommand("dbo.GetUserSettings");
            var notifications = new List<Notification>();

            if (data != null)
            {
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    var rows = data.Tables[0].Rows;

                    foreach (DataRow dr in rows)
                    {
                        notifications.Add(new Notification()
                        {
                            ID = int.Parse(dr["ID"].ToString()),
                            Email = dr["Email"].ToString(),
                            SoldOutSent = bool.Parse(dr["SoldOutSent"].ToString()),
                            AllSoldOutSent = bool.Parse(dr["AllSoldOutSent"].ToString()),
                            SendNotification = bool.Parse(dr["SendNotifications"].ToString())
                        });
                    }
                }
                else return null;
            }
            else return null;

            return notifications;
        }

        private static DataSet ExecuteCommand(string procedureName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(procedureName, connection) 
                { 
                    CommandType = CommandType.StoredProcedure
                })
                {
                    DataSet data = new DataSet();

                    try
                    {
                        connection.Open();
                        var adapter = new SqlDataAdapter(command);
                        adapter.Fill(data);
                    }
                    catch (Exception e)
                    {
                        //Todo: implement logging
                    }

                    return data;
                }
            }
        }

        public static void ExecuteCommand(string procedureName, int parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@userId", parameter);
                    var rows = command.ExecuteNonQuery();
                }
            }
        }
    }
}