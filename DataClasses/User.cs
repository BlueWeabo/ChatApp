using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataClasses
{
    public class User
    {
        
        public int Id { get; set; }
        [MaxLength(45)]
        [Column("Username", TypeName = "varchar")]
        [Index(IsUnique = true)]
        public string Username { get; set; }
        [MaxLength(250)]
        [Column("Password", TypeName = "varchar")]
        public string Password { get; set; }
        [MaxLength(15)]
        [Column("IpAddress", TypeName = "varchar")]
        public string IpAddress { get; set; }

        public ICollection<Group> Groups { get; set; }

        public User()
        {
            Groups = new List<Group>();
        }

        public static string Sha256(string randomString)
        {
            SHA256 crypt = SHA256.Create();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
