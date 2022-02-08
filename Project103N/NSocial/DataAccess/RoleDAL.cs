using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSocial.Models;
using System.Data;
using System.Data.SqlClient;

namespace NSocial.DataAccess
{
    public class RoleDAL
    {
        static private RoleDAL _Methods { get; set; }
        static public RoleDAL Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new RoleDAL();
                return _Methods;
            }
        }
        internal Role GetByID(int id)
        {
            Role role = new Role();
            string query = $"SELECT * FROM Role WHERE ID = {id}";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            IDataReader reader;
            try
            {
                DbTools.Connection.ConnectDB();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    role.ID = int.Parse(reader["ID"].ToString());
                    role.Name = reader["Name"].ToString();
                }
            }
            catch
            {
                Console.WriteLine("HATA OLUŞTU");
            }
            finally
            {
                DbTools.Connection.DisconnectDB();
            }
            return role;
        }



        public List<Role> ListRole(string query)
        {
            List<Role> roles = new List<Role>();
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            IDataReader reader;
            try
            {
                DbTools.Connection.ConnectDB();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new Role()
                    {
                        ID = int.Parse(reader["ID"].ToString()),
                        Name = reader["Name"].ToString()
                    });
                }
            }
            catch
            {
                Console.WriteLine("HATA OLUŞTU");
            }
            finally
            {
                DbTools.Connection.DisconnectDB();
            }
            return roles;
        }
    }
}