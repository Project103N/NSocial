using NSocial.Models;
using NSocial.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;




namespace NSocial.DataAccess
{
    public class FollowDAL
    {
        private static FollowDAL _Methods { get; set; }

        public static FollowDAL Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new FollowDAL();
                return _Methods;
            }
        }
        //Takip edilen kişi sayısını verir (Okuma)
        public int GetFollower(int id)
        {
            try
            {
                string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                return UserDAL.Methods.GetUsers(cmd)[0].FollowersCount;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        //Takip ettiği kişi sayısı(Okuma)
        public int GetFollowing(int id)
        {
            try
            {
                string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                return UserDAL.Methods.GetUsers(cmd)[0].FollowingsCount;
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        //İstek atma sadece 
        public int FriendRequest(int id)
        {
            try
            {
                //user.AddressID = Convert.ToInt32(AddressDAL.Methods.Insert(user.Address));
                string query = $@"INSERT INTO [dbo].[Follow] ([FollowerID],[FollowedID],[IsAccepted]) VALUES (@followerID, @followedID, @isaccepted);";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                cmd.Parameters.AddWithValue("@followerID", SessionPersister.ID);
                cmd.Parameters.AddWithValue("@followedID", id);
                cmd.Parameters.AddWithValue("@isaccepted", false);

                return DbTools.Connection.Create(cmd); // follower id yi dönderecek yani istek atanı
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        // Kabul etti mi etmedi mi ?
        public bool IsAccepted(int id)
        {
            try
            {
                string query = $"SELECT IsAccepted from [Follow] where FollowerID={SessionPersister.ID} and FollowedID={id};";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                return DbTools.Connection.Execute(cmd); // Kabul etmişse true etmemmişse false dönderecek.
            }
            catch (Exception )
            {
                throw;
            }
            // Arkadaşlığı kabul edince IsAccepted ı true yapma
        } public void FriendRequestAccepted(int id)
            {
                try
                {
                    string query = $@"UPDATE [dbo].[User] SET [IsAccepted]=True WHERE [FollowerID]=@followerid and [FollowedD] =@followedid;";
                    //update[User] set [FollowersCount] = 1 where ID = 3;
                    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                    cmd.Parameters.AddWithValue("@followerid", SessionPersister.ID);
                    cmd.Parameters.AddWithValue("@followedid", id);

                    DbTools.Connection.Execute(cmd);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }


            //Ben kaç kişiyi takip ediyorum ve
            // Arkadaş eklediyse toplam arkadaşı user tablosuna yazma
            public void AddFollower()
            {
                try
                {
                    string query = $"SELECT COUNT(*) as count from [Follow] where FollowerID={SessionPersister.ID} and IsAccepted=1;";
                    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                    int count = Count(cmd);
                    FollowerCountAdd(count);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            /// Kaç kişi bizi takip ediyor
            /// Arkadaş eklediyse toplam arkadaşı user tablosuna yazma
            public void AddFollowing()
            {
                try
                {
                    string query = $"SELECT COUNT(*) as count from [Follow] where FollowedID={SessionPersister.ID} and IsAccepted=1;";
                    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                    int count = Count(cmd);
                    FollowingsCountAdd(count);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }

            public int Count(SqlCommand cmd)
            {
                try
                {
                    DbTools.Connection.ConnectDB();
                    int count = -1;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader["count"]);
                    }
                    reader.Close();
                    DbTools.Connection.DisconnectDB();
                    return count;
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            //User tablosuna toplam followercount sayısını yazma
            public void FollowerCountAdd(int count)
            {
                try
                {
                    string query = $@"UPDATE [dbo].[User] SET [FollowersCount]=@followerscount WHERE [ID]=@id;";
                    //update[User] set [FollowersCount] = 1 where ID = 3;
                    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                    cmd.Parameters.AddWithValue("@followerscount", count);
                    cmd.Parameters.AddWithValue("@id", SessionPersister.ID);

                    DbTools.Connection.Execute(cmd);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            // followingscount  e yazma
            public void FollowingsCountAdd(int count)
            {
                string query = $@"UPDATE [dbo].[User] SET [FollowingsCount]=@followerscount WHERE [ID]=@id;";
                //update[User] set [FollowersCount] = 1 where ID = 3;
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                cmd.Parameters.AddWithValue("@followerscount", count);
                cmd.Parameters.AddWithValue("@id", SessionPersister.ID);

                DbTools.Connection.Execute(cmd);
            }

            //Takip ettigim kişilerin id sini dönderir
            public List<User> ShowMyFollowings()
            {

                string query = $"SELECT FollowedID FROM [Follow] WHERE FollowerID={SessionPersister.ID} and IsAccepted = 1;";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                return GetFollowingsList(cmd);
            }
            //Beni takip eden kişilerin id sini dönderir

            public List<User> ShowMyFollower()
            {
                string query = $"SELECT FollowerID FROM [Follow] WHERE FollowedID={SessionPersister.ID} and IsAccepted = 1;";
                SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
                return GetFollowerList(cmd);
            }

            public List<User> GetFollowingsList(SqlCommand cmd)
            {
                List<User> FollowingsList = new List<User>();
                IDataReader reader;
                DbTools.Connection.ConnectDB();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User();
                        user.ID = reader.GetInt32(0);
                        FollowingsList.Add(user);
                    }
                    reader.Close();
                    DbTools.Connection.DisconnectDB();
                }
                catch (Exception e)
                {
                    DbTools.Connection.DisconnectDB();

                    throw (e);
                }

                return FollowingsList;
            }

            public List<User> GetFollowerList(SqlCommand cmd)
            {
                List<User> FollowerList = new List<User>();
                IDataReader reader;
                DbTools.Connection.ConnectDB();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User();
                        user.ID = reader.GetInt32(0);
                        FollowerList.Add(user);
                    }
                    reader.Close();
                    DbTools.Connection.DisconnectDB();
                }
                catch (Exception e)
                {
                    DbTools.Connection.DisconnectDB();

                    throw (e);
                }

                return FollowerList;
            }
        }
    }
