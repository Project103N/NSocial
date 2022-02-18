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
    public class CommentsDAL
    {
        private static CommentsDAL _Methods { get; set; }

        public static CommentsDAL Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new CommentsDAL();
                return _Methods;
            }
        }
        public int Add(Comments comments)
        {
            string query = $@"INSERT INTO [dbo].[Comments] ([UserID],[PostID],[Text]) VALUES (@userId, @postId, @text); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@userId", SessionPersister.ID);
            cmd.Parameters.AddWithValue("@postId", comments.PostID);
            cmd.Parameters.AddWithValue("@text", comments.Text);
            return DbTools.Connection.Create(cmd);
        }
        public List<Comments> List()
        {
            string query = $"SELECT * FROM Comments;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return GetCommentsList(cmd);
        }
        private List<Comments> GetCommentsList(SqlCommand cmd)
        {
            List<Comments> commentsList = new List<Comments>();
            IDataReader reader;
            DbTools.Connection.ConnectDB();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    commentsList.Add(
                    new Comments()
                    {
                        ID = int.Parse(reader["ID"].ToString()),
                        PostID = int.Parse(reader["PostID"].ToString()),
                        UserID = int.Parse(reader["UserID"].ToString()),
                        Text = reader["Text"].ToString()
                    });
                }
                reader.Close();
                DbTools.Connection.DisconnectDB();
            }
            catch
            {
                throw;
            }
            return commentsList;
        }
        public List<Comments> GetByPostID(int id)
        {
            string query = $"SELECT * FROM Comments WHERE PostID=@postId ORDER BY ID DESC;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@postId", id);
            try
            {
                return GetCommentsList(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}