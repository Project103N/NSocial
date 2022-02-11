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
        public int GetFollower(int id)
        {
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return UserDAL.Methods.GetUsers(cmd)[0].FollowersCount;
        }
     
        public int GetFollowing(int id)
        {
            string query = $"SELECT * FROM [User] WHERE ID={id} AND IsActive=1;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return UserDAL.Methods.GetUsers(cmd)[0].FollowingsCount;
        }

        public bool IsAccepted(int id)
        {
            // Buttondan id geliyor

            string query = $"SELECT IsAccepted from [Follow] where FollowerID={SessionPersister.ID} and FollowedID={id};";
            return true;
        }

        






    }
}