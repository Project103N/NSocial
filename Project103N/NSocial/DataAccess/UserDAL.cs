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

        //public User GetByID(int id)
        //{
        //    string query = $"SELECT * FROM [User] WHERE ID={id};";

        //    return ListUser(query)[0];
        //}

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
            List<User> activeUserList = new List<User>();
            User user = new User();
            string query = $"SELECT * FROM [User] WHERE IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            IDataReader reader;
            DbTools.Connection.ConnectDB();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read()) // Okunacak satır varsa çalışsın.
                {

                    //user.ID = int.Parse(reader["ID"].ToString());
                    user.ID = reader.GetInt32(0);
                    user.Name = reader["Name"].ToString();
                    user.Surname = reader["Surname"].ToString();
                    user.ProfileImagePath = reader["ProfileImagePath"].ToString();
                    user.FollowersCount = int.Parse(reader["FollowersCount"].ToString());
                    user.FollowingsCount = int.Parse(reader["FollowingsCount"].ToString()); ;
                    user.Email = reader["Email"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.RoleID = int.Parse(reader["RoleID"].ToString());
                    user.RegisterDate = DateTime.ParseExact(reader["RegisterDate"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
                    user.isActive = Convert.ToBoolean(reader["IsActive"].ToString());

                    activeUserList.Add(user);
                    DbTools.Connection.DisconnectDB();

                }
            }
            catch (Exception)
            {
                DbTools.Connection.DisconnectDB();

                throw;
            }

            return activeUserList;
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