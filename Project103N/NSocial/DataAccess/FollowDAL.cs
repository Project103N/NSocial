using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NSocial.Models;


namespace NSocial.DataAccess
{
    public class FollowDAL
    {

        public User Find(int id)
        {
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return GetUsers(cmd);
        }


    }
}