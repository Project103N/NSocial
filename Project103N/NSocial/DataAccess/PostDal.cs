using NSocial.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NSocial.DataAccess
{
    public class PostDAL
    {
        private static PostDAL _Methods { get; set; }

        public static PostDAL Methods
        {
            get
            {
                if (_Methods == null)
                    _Methods = new PostDAL();
                return _Methods;
            }
        }
        public List<Post> List()
        {
            string query = $"SELECT * FROM Post;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            return GetPostList(cmd);
        }

        private static List<Post> GetPostList(SqlCommand cmd)
        {
            List<Post> postList = new List<Post>();
            IDataReader reader;
            DbTools.Connection.ConnectDB();
            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    postList.Add(
                    new Post()
                    {
                        ID = reader.GetInt32(0),
                        Text = reader["Text"].ToString(),
                        PostDate = (DateTime)reader["PostDate"],
                        LikesCount = int.Parse(reader["LikesCount"].ToString()),
                        CommentsCount = int.Parse(reader["CommentsCount"].ToString()),
                        UserID = int.Parse(reader["UserID"].ToString()),
                        Comments = reader["Comments"].ToString()
                    }) ;
                }
                reader.Close();
                DbTools.Connection.DisconnectDB();
            }
            catch
            {
                throw;
            }
            return postList;
        }


        public int Add(Post Post)
        {
            string query = $@"INSERT INTO [dbo].[Post] ([Text],[PostDate],[LikesCount],[CommentsCount],[UserID],[Comments]) VALUES (@text, @postDate, @likesCount, @commentsCount,@userId,@comments);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@text", Post.Text);
            cmd.Parameters.AddWithValue("@postDate", Post.PostDate);
            cmd.Parameters.AddWithValue("@likesCount", Post.LikesCount);
            cmd.Parameters.AddWithValue("@commentsCount", Post.CommentsCount);
            cmd.Parameters.AddWithValue("@userId", Post.UserID);
            cmd.Parameters.AddWithValue("@comments", Post.Comments);
            return DbTools.Connection.Create(cmd);
        }

        //update kismi Talha'da

        public int Edit(Post post)
        {
            string query = $@"UPDATE [dbo].[Post] SET [Text]=@text WHERE [ID]=@id;";
            //string query = $@"UPDATE [dbo].[User] SET ([ID],[Text]) VALUES (@id,@text); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@text", post.Text);
            cmd.Parameters.AddWithValue("@id", post.ID);
            return DbTools.Connection.Edit(cmd);
        }
        public int Delete(int id)
        {
            string query = $"DELETE FROM Post WHERE ID=@id;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@id", id);
            return DbTools.Connection.Delete(cmd);
        }

        //Delete Talha'da
        

        //source kismi Birol'da
        public Post GetByID(int id)
        {
            string query = $"SELECT * FROM Post WHERE ID=@id;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                return GetPostList(cmd)[0];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Post> Search(string searchterm)
        {
            string query = $"SELECT * FROM Post WHERE Text LIKE '%' + @searchterm + '%';";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@searchterm", searchterm);
            try
            {
                return GetPostList(cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //models/post.cs kisminda yoruma aldiklarimizi yorumdan kaldirip data, controller, view kismi hafif duzenlemeler olacak
    }
}