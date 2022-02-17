using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NSocial.Models;

namespace NSocial.DataAccess
{
    public class UserDAL
    {
        private static UserDAL _Methods { get; set; }

        public static UserDAL Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new UserDAL();
                return _Methods;
            }
        }



        public int Add(User user)
        {

            //user.AddressID = Convert.ToInt32(AddressDAL.Methods.Insert(user.Address));
            string query = $@"INSERT INTO [dbo].[User] ([Name],[Surname],[Nickname],[ProfileImagePath],[FollowersCount],[FollowingsCount],[Email],[Password],[RoleID],[RegisterDate],[IsActive]) VALUES (@name, @surname, @nickname, @profileimagepath, @followerscount,@followingscount,@email,@password,@roleid,@registerdate,@isactive); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@surname", user.Surname);
            cmd.Parameters.AddWithValue("@nickname", user.Nickname);
            cmd.Parameters.AddWithValue("@profileimagepath", user.ProfileImagePath);
            cmd.Parameters.AddWithValue("@followerscount", user.FollowersCount);
            cmd.Parameters.AddWithValue("@followingscount", user.FollowingsCount);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@roleid", user.RoleID);
            cmd.Parameters.AddWithValue("@registerdate", user.RegisterDate);
            cmd.Parameters.AddWithValue("@isactive", user.isActive);
            return DbTools.Connection.Create(cmd);
        }

        public bool SaveChanges(User user)
        {
            string query = $@"UPDATE [dbo].[User] SET [Name]=@name,[Surname]=@surname,[Nickname]=@nickname,[ProfileImagePath]=@profileimagepath, [FollowersCount]=@followerscount,[FollowingsCount]=@followingscount, [Email]=@email,[Password]=@password,[RoleID]=@roleid,[IsActive]=@isactive WHERE [ID]=@id;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@surname", user.Surname);
            cmd.Parameters.AddWithValue("@nickname", user.Nickname);
            cmd.Parameters.AddWithValue("@profileimagepath", user.ProfileImagePath);
            cmd.Parameters.AddWithValue("@followerscount", user.FollowersCount);
            cmd.Parameters.AddWithValue("@followingscount", user.FollowingsCount);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@roleid", user.RoleID);
            cmd.Parameters.AddWithValue("@id", user.ID);
            cmd.Parameters.AddWithValue("@isactive", user.isActive);
            return DbTools.Connection.Execute(cmd);
        }

        public List<User> All()
        {
            string query = $"SELECT * FROM [User] WHERE IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return GetUsers(cmd);
        }

        public List<User> GetUsers(SqlCommand cmd)
        {
            List<User> activeUserList = new List<User>();
            IDataReader reader;
            DbTools.Connection.ConnectDB();
            try
            {
                reader = cmd.ExecuteReader();
                
                while (reader.Read()) // Okunacak satır varsa çalışsın.
                {
                    User user = new User();
                    //user.ID = int.Parse(reader["ID"].ToString());
                    user.ID = reader.GetInt32(0);
                    user.Name = reader["Name"].ToString();
                    user.Surname = reader["Surname"].ToString();
                    user.Nickname = reader["Nickname"].ToString();
                    user.ProfileImagePath = reader["ProfileImagePath"].ToString();
                    user.FollowersCount = int.Parse(reader["FollowersCount"].ToString());
                    user.FollowingsCount = int.Parse(reader["FollowingsCount"].ToString()); ;
                    user.Email = reader["Email"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.RoleID = int.Parse(reader["RoleID"].ToString());
                    user.RegisterDate = (DateTime)(reader["RegisterDate"]);
                    user.isActive = Convert.ToBoolean(reader["IsActive"].ToString());

                    activeUserList.Add(user);
                }
                reader.Close();
                DbTools.Connection.DisconnectDB();
            }
            catch (Exception e)
            {
                DbTools.Connection.DisconnectDB();

                throw(e);
            }

            return activeUserList;
        }

        public User Find(int id)
        {
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return GetUsers(cmd)[0];
        }

        public User FindX(object obj)
        {
            SqlCommand cmd;
            string strObj = obj.ToString();
            string query = "";
            if (obj is int id)
            {
                query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
                cmd = new SqlCommand(query, DbTools.Connection.con);
                return GetUsers(cmd)[0];
            }
            else if (obj is string)
            {
                query = $"SELECT * FROM [User] WHERE (Nickname= @nickname or Email= @email) AND IsActive=1;";
                cmd = new SqlCommand(query, DbTools.Connection.con);
                cmd.Parameters.AddWithValue("@nickname", strObj);
                cmd.Parameters.AddWithValue("@email", strObj);
                return GetUsers(cmd)[0];
            }
            return null;
        }




        //public User GetByEmail(string email)
        //{
        //    string query = $"SELECT * FROM [User] WHERE Email='{email}';";
        //    try
        //    {
        //        return ListUser(query)[0];
        //    }
        //    catch (Exception)
        //    {
        //        return new User();
        //    }
        //}


        //public User Login(string email, string password)
        //{
        //    string query = $"SELECT * FROM [User] WHERE Email='{email}' AND Password='{password}';";
        //    // TODO: addwithvalue ile parametreler girilecek.
        //    try
        //    {
        //        return ListUser(query)[0];
        //    }
        //    catch (Exception)
        //    {
        //        return new User(); // Boş user döndük.
        //    }
        //}
    }
}