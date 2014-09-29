using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MoeDeloTestApp.Models;

namespace MoeDeloTestApp.Dapper
{
    public interface IUserManager : IDisposable
    {
        User Add(User user);
        
        IEnumerable<User> GetAll();

        IEnumerable<User> Filter(int current, int rowCount, string searchPhrase, out int total);
        
        User Update(User user);

        void Remove(int id);
    }
}
