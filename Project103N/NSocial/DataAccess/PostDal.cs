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
            try
            {
                DbTools.Methods.ConnectDB();
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
                        Comments = (ICollection<Post>)reader["Comments"]
                    });
                }
                DbTools.Methods.DisconnectDB();
            }
            catch
            {
                throw;
            }
            return postList;
        }


        //public int Add(Post Post)
        //{

        //    //Post.AddressID = Convert.ToInt32(AddressDAL.Methods.Insert(Post.Address));
        //    string query = $@"INSERT INTO [dbo].[Post] ([ID],[Text],[PostDate],[LikesCount],[PunchsCount],[CommentsCount],[UserID],[Comments]) VALUES (@id, @text, @postDate, @likesCount, @punchCount, @commentsCount,@userId,@comments); SELECT CAST(scope_identity() AS int);";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    cmd.Parameters.AddWithValue("@id", Post.ID);
        //    if (String.IsNullOrEmpty(Post.Text))
        //    {
        //        cmd.Parameters.AddWithValue("@text", Post.Text);
        //    }
        //    cmd.Parameters.AddWithValue("@postDate", Post.PostDate);
        //    cmd.Parameters.AddWithValue("@likesCount", Post.LikesCount);
        //    cmd.Parameters.AddWithValue("@punchCount", Post.PunchsCount);
        //    cmd.Parameters.AddWithValue("@commentsCount", Post.CommentsCount);
        //    cmd.Parameters.AddWithValue("@userId", Post.UserID);
        //    cmd.Parameters.AddWithValue("@comments", Post.Comments);
        //    return DbTools.Connection.Create(cmd);
        //}

        //public bool SaveChanges(Post Post)
        //{
        //    string query = $@"UPDATE  [dbo].[Post] SET ([ID],[Text],[PostDate],[LikesCount],[CommentsCount],[UserID],[Comments]) VALUES (@id, @text, @postDate, @likesCount, @commentsCount, @userId, @comments); SELECT CAST(scope_identity() AS int);";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    cmd.Parameters.AddWithValue("@id", Post.ID);
        //    cmd.Parameters.AddWithValue("@text", Post.Text);
        //    cmd.Parameters.AddWithValue("@postDate", Post.PostDate);
        //    cmd.Parameters.AddWithValue("@likesCount", Post.LikesCount);
        //    cmd.Parameters.AddWithValue("@commentsCount", Post.CommentsCount);
        //    cmd.Parameters.AddWithValue("@userId", Post.UserID);
        //    cmd.Parameters.AddWithValue("@roleId", Post.RoleID);
        //    cmd.Parameters.AddWithValue("@comments", Post.Comments);
        //    return DbTools.Connection.Execute(cmd);
        //}

        //public bool Delete(int id)
        //{
        //    string query = $"DELETE FROM Post WHERE ID=@id;";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    cmd.Parameters.AddWithValue("@id", id);
        //    return true;
        //}



        ////değişimler



        //public List<Post> GetPosts(SqlCommand cmd)
        //{
        //    List<Post> activePostList = new List<Post>();
        //    IDataReader reader;
        //    DbTools.Connection.ConnectDB();
        //    try
        //    {
        //        reader = cmd.ExecuteReader();

        //        while (reader.Read()) 
        //        {
        //            Post post = new Post();
        //            post.ID = reader.GetInt32(0);
        //            post.PostImagePath = reader["PostImagePath"].ToString();
        //            post.Text = reader["Text"].ToString();
        //            post.LikesCount = int.Parse(reader["LikesCount"].ToString());
        //            post.PunchsCount = int.Parse(reader["PunchsCount"].ToString());
        //            post.CommentsCount = int.Parse(reader["CommentsCount"].ToString());
        //            post.UserID = int.Parse(reader["UserID"].ToString());
        //            post.RoleID = (User)reader["UserID"];
        //            post.Comments = (ICollection<Post>)reader["Comments"];



        //            activePostList.Add(post);
        //        }
        //        reader.Close();
        //        DbTools.Connection.DisconnectDB();
        //    }
        //    catch (Exception e)
        //    {
        //        DbTools.Connection.DisconnectDB();

        //        throw (e);
        //    }

        //    return activePostList;
        //}

        //public Post Find(int id)
        //{
        //    string query = $"SELECT * FROM [Post] WHERE ID={id};";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    return GetPosts(cmd)[0];
        //}

        //public Post FindX(object obj)
        //{
        //    SqlCommand cmd;
        //    string strObj = obj.ToString();
        //    string query = "";
        //    if (obj is int id)
        //    {
        //        query = $"SELECT * FROM [Post] WHERE ID={id};";
        //        cmd = new SqlCommand(query, DbTools.Connection.con);
        //        return GetPosts(cmd)[0];

        //    }
        //    return null;
        //}
        //public List<Post> All()
        //{
        //    string query = $"SELECT * FROM [Post] WHERE IsActive=1;";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    return GetPosts(cmd);
        //}

        //public List<Post> GetPosts(SqlCommand cmd)
        //{
        //    List<Post> activePostList = new List<Post>();
        //    IDataReader reader;
        //    DbTools.Connection.ConnectDB();
        //    try
        //    {
        //        reader = cmd.ExecuteReader();

        //        while (reader.Read()) // Okunacak satır varsa çalışsın.
        //        {
        //            Post Post = new Post();
        //            //Post.ID = int.Parse(reader["ID"].ToString());
        //            Post.ID = reader.GetInt32(0);
        //            Post.Name = reader["Name"].ToString();
        //            Post.Surname = reader["Surname"].ToString();
        //            Post.Nickname = reader["Nickname"].ToString();
        //            Post.ProfileImagePath = reader["ProfileImagePath"].ToString();
        //            Post.FollowersCount = int.Parse(reader["FollowersCount"].ToString());
        //            Post.FollowingsCount = int.Parse(reader["FollowingsCount"].ToString()); ;
        //            Post.Email = reader["Email"].ToString();
        //            Post.Password = reader["Password"].ToString();
        //            Post.RoleID = int.Parse(reader["RoleID"].ToString());
        //            Post.RegisterDate = (DateTime)(reader["RegisterDate"]);
        //            Post.isActive = Convert.ToBoolean(reader["IsActive"].ToString());

        //            activePostList.Add(Post);
        //        }
        //        reader.Close();
        //        DbTools.Connection.DisconnectDB();
        //    }
        //    catch (Exception e)
        //    {
        //        DbTools.Connection.DisconnectDB();

        //        throw (e);
        //    }

        //    return activePostList;
        //}

        //public Post Find(int id)
        //{
        //    string query = $"SELECT * FROM [Post] WHERE ID={id} AND IsActive=1;";
        //    SqlCommand cmd = new SqlCommand(query, DbTools.Connection.con);
        //    return GetPosts(cmd)[0];
        //}

        //public Post FindX(object obj)
        //{
        //    SqlCommand cmd;
        //    string strObj = obj.ToString();
        //    string query = "";
        //    if (obj is int id)
        //    {
        //        query = $"SELECT * FROM [Post] WHERE ID={id} AND IsActive=1;";
        //        cmd = new SqlCommand(query, DbTools.Connection.con);
        //        return GetPosts(cmd)[0];
        //    }
        //    else if (obj is string)
        //    {
        //        query = $"SELECT * FROM [Post] WHERE (Nickname= @nickname or Email= @email) AND IsActive=1;";
        //        cmd = new SqlCommand(query, DbTools.Connection.con);
        //        cmd.Parameters.AddWithValue("@nickname", strObj);
        //        cmd.Parameters.AddWithValue("@email", strObj);
        //        return GetPosts(cmd)[0];
        //    }
        //    return null;
        //}

    }
}