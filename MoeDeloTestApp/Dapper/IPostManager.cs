using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MoeDeloTestApp.Models;

namespace MoeDeloTestApp.Dapper
{
    public interface IPostManager : IDisposable
    {
        IEnumerable<Post> GetAll();
    }
}
