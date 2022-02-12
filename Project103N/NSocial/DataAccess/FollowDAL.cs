using NSocial.Models;
using NSocial.Security;
using System;
using System.Collections.Generic;
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
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return UserDAL.Methods.GetUsers(cmd)[0].FollowersCount;
        }

        //Takip ettiği kişi sayısı(Okuma)
        public int GetFollowing(int id)
        {
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return UserDAL.Methods.GetUsers(cmd)[0].FollowingsCount;
        }


        //İstek atma sadece 
        public int FriendRequest(int id)
        {
            //user.AddressID = Convert.ToInt32(AddressDAL.Methods.Insert(user.Address));
            string query = $@"INSERT INTO [dbo].[Follow] ([FollowerID],[FollowedID],[IsAccepted]) VALUES (@followerID, @followedID, @isaccepted); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@followerID", SessionPersister.ID);
            cmd.Parameters.AddWithValue("@followedID", id);
            cmd.Parameters.AddWithValue("@isaccepted", false);

            return DbTools.Connection.Create(cmd); // follower id yi dönderecek yani istek atanı
        }

        // Kabul etti mi etmedi mi ?
        public bool IsAccepted(int id)
        {
            // Buttondan id geliyor

            string query = $"SELECT IsAccepted from [Follow] where FollowerID={SessionPersister.ID} and FollowedID={id};";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return DbTools.Connection.Execute(cmd); // Kabul etmişse true etmemmişse false dönderecek.

        }
        // Arkadaşlığı kabul edince IsAccepted ı true yapma
        public void FriendRequestAccepted(int id)
        {
            string query = $@"UPDATE [dbo].[User] SET [IsAccepted]=True WHERE [FollowerID]=@followerid and [FollowedD] =@followedid;";
            //update[User] set [FollowersCount] = 1 where ID = 3;
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@followerid", SessionPersister.ID);
            cmd.Parameters.AddWithValue("@followedid", id);

            DbTools.Connection.Execute(cmd);
        }


        //Ben kaç kişiyi takip ediyorum ve
        // Arkadaş eklediyse toplam arkadaşı user tablosuna yazma
        public void AddFollower()
        {

            string query = $"SELECT COUNT(*) as count from [Follow] where FollowerID={SessionPersister.ID} and IsAccepted=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            int count = Count(cmd);
            FollowerCountAdd(count);
        }
        /// Kaç kişi bizi takip ediyor
        /// Arkadaş eklediyse toplam arkadaşı user tablosuna yazma
        public void AddFollowing()
        {

            string query = $"SELECT COUNT(*) as count from [Follow] where FollowedID={SessionPersister.ID} and IsAccepted=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            int count = Count(cmd);
            FollowingsCountAdd(count);
        }

        public int Count(SqlCommand cmd)
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
        //User tablosuna toplam followercount sayısını yazma
        public void FollowerCountAdd(int count)
        {
            string query = $@"UPDATE [dbo].[User] SET [FollowersCount]=@followerscount WHERE [ID]=@id;";
            //update[User] set [FollowersCount] = 1 where ID = 3;
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@followerscount", count);
            cmd.Parameters.AddWithValue("@id", SessionPersister.ID);

            DbTools.Connection.Execute(cmd);
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
    }
}