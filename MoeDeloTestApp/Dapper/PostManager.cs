using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using MoeDeloTestApp.Models;

namespace MoeDeloTestApp.Dapper
{
    public class PostManager : IPostManager
    {
        private readonly IDbConnection _connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["MoeDeloConnectionString"].ConnectionString);

        private bool _disposed;

        public IEnumerable<Post> GetAll()
        {
            var posts = _connection.Query<Post>(
                @"select 0 as Id, '' as Name
                  union
                  select Id, Name from Post"
                );
            return posts;
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