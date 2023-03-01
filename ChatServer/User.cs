using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        public string IpAddress { get; set; }

        public ICollection<Group> Groups { get; set; }

        public User()
        {
            Groups = new List<Group>();
        }
    }
}
