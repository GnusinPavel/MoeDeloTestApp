using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MoeDeloTestApp.Models;

namespace MoeDeloTestApp.Dapper
{
    public class UserManager : IUserManager
    {
        private readonly IDbConnection _connection = 
            new SqlConnection(ConfigurationManager.ConnectionStrings["MoeDeloConnectionString"].ConnectionString);

        private bool _disposed;


        public User Add(User user)
        {
            var sql = @"insert into [User] (FirstName, LastName, PostId) 
                        values (@FirstName, @LastName, @PostId);
                        select cast(SCOPE_IDENTITY() as int)";
            user.Id = _connection.Query<int>(sql, new {user.FirstName, user.LastName, PostId = user.Post.Id}).First();

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            string sql = "select * from [User]";
            return _connection.Query<User>(sql);
        }

        public IEnumerable<User> Filter(int current, int rowCount, string searchPhrase, out int total)
        {
            var searchExpression = "";
            if (!String.IsNullOrEmpty(searchPhrase))
            {
                searchExpression = String.Format(" where [User].FirstName like '%{0}%' or [User].LastName like '%{0}%'", searchPhrase);
            }

            total = _connection.ExecuteScalar<int>("select count(*) from [User] " + searchExpression);
            var sql =
                @"select * from [User]
                  left join Post on Post.Id = [User].PostId " + searchExpression +
                 "order by [User].Id";

            var offset = 0;
            if (rowCount != -1)
            {
                sql += " offset @offset rows fetch next @rowCount rows only";
                offset = (current - 1) * rowCount;
            }

            var users = _connection.Query<User, Post, User>(
                sql,
                (u, p) => { u.Post = p; return u; },
                new
                {
                    offset, 
                    rowCount
                },
                splitOn: "PostId"
            );

            return users;
        }

        public User Update(User user)
        {
            var sql = @"update [User]
                        set FirstName = @FirstName,
                            LastName = @LastName,
                            PostId = @PostId
                        where Id = @Id";
            _connection.Execute(sql,
                new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    PostId = user.Post.Id != 0 ? (int?)user.Post.Id : null
                });
            return user;
        }

        public void Remove(int id)
        {
            _connection.Execute("delete from [User] where id = @id", new {id});
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}