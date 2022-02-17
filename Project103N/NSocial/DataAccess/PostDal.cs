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
                        DislikesCount = int.Parse(reader["DislikesCount"].ToString()),
                        CommentsCount = int.Parse(reader["CommentsCount"].ToString()),
                        UserID = int.Parse(reader["UserID"].ToString()),
                        Comments = reader["Comments"].ToString(),
                        PostImagePath = reader["PostImagePath"].ToString()
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
            string query = $@"INSERT INTO [dbo].[Post] ([Text],[PostDate],[LikesCount],[DislikesCount],[CommentsCount],[UserID],[Comments],[PostImagePath]) VALUES (@text, @postDate, @likesCount,@dislikesCount, @commentsCount,@userId,@comments,@postImagePath); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@text", Post.Text);
            cmd.Parameters.AddWithValue("@postDate", Post.PostDate);
            cmd.Parameters.AddWithValue("@likesCount", 0);
            cmd.Parameters.AddWithValue("@dislikesCount", 0);
            cmd.Parameters.AddWithValue("@commentsCount", Post.CommentsCount);
            cmd.Parameters.AddWithValue("@userId", Post.UserID);
            cmd.Parameters.AddWithValue("@comments", Post.Comments);
            cmd.Parameters.AddWithValue("@postImagePath", Post.PostImagePath);
            return DbTools.Connection.Create(cmd);
        }

        //update kismi Talha'da

        public int Edit(Post post)
        {
            string query = $@"UPDATE [dbo].[Post] SET [Text]=@text, [PostImagePath]=@postImagePath WHERE [ID]=@id;";
            //string query = $@"UPDATE [dbo].[User] SET ([ID],[Text]) VALUES (@id,@text); SELECT CAST(scope_identity() AS int);";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@text", post.Text);
            cmd.Parameters.AddWithValue("@id", post.ID);
            cmd.Parameters.AddWithValue("@postImagePath", post.PostImagePath);
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
        public List<Post> GetByUserID(int id)
        {
            string query = $"SELECT * FROM Post WHERE UserID=@userid;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@userid", id);
            try
            {
                return GetPostList(cmd);
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

        public int GetLikeCount(int id)
        {
            string query = "SELECT LikesCount FROM Post WHERE ID=@postid;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@postid", id);
            return DbTools.Connection.Create(cmd);

        }
        public int GetDislikeCount(int id)
        {
            string query = "SELECT DislikesCount FROM Post WHERE ID=@postid;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@postid", id);
            return DbTools.Connection.Create(cmd);
        }
        public int AddLike(string value, string postidstr) // 1 veya -1 alabilir.
        {
            int count = Convert.ToInt32(value);
            int postid = Convert.ToInt32(postidstr);
            string query = "UPDATE Post SET [LikesCount] +=@count WHERE ID = @postid;SELECT [LikesCount] FROM Post WHERE ID=@postid;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@count", count);
            cmd.Parameters.AddWithValue("@postid", postid);
            return DbTools.Connection.Create(cmd);
        }
        public int AddDislike(string value, string postidstr) // 1 veya -1 alabilir.
        {
            int count = Convert.ToInt32(value);
            int postid = Convert.ToInt32(postidstr);
            string query = "UPDATE Post SET [DislikesCount] +=@count WHERE ID = @postid;SELECT [DislikesCount] FROM Post WHERE ID=@postid;";
            SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
            cmd.Parameters.AddWithValue("@count", count);
            cmd.Parameters.AddWithValue("@postid", postid);
            return DbTools.Connection.Create(cmd);
        }
    }
}