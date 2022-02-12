using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace NSocial.DataAccess
{
    public class DbTools
    {
        //post kisim bunu ekledi
        private static DbTools _Methods { get; set; }

        public static DbTools Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new DbTools();
                return _Methods;
            }
        }
        //





        static string strConnection = ConfigurationManager.ConnectionStrings["NSocialCS"].ConnectionString;
        public SqlConnection con = new SqlConnection(strConnection);
        private static DbTools _Con { get; set; }
        public static DbTools Connection
        {
            get
            {
                if (_Con == null)
                    _Con = new DbTools();
                return _Con;
            }
        }
        public bool ConnectDB()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool DisconnectDB()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // This class adds the object to db and returns primary key
        public int Create(SqlCommand cmd)
        {
            object insertedID = -1;
            try
            {
                ConnectDB();
                insertedID = cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DisconnectDB();
            }
            return Convert.ToInt32(insertedID);
        }
        public bool Execute(SqlCommand cmd)
        {
            int affectedRows = -1;
            try
            {
                ConnectDB();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                DisconnectDB();
            }
            if (affectedRows > 0)
                return true;
            else
                return false;
        }

    }
}