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
using Newtonsoft.Json;

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
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public class UserImportantInfo : JsonConverter<User>
        {
            public override User? ReadJson(JsonReader reader, Type objectType, User? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<User>(reader);
            }

            public override void WriteJson(JsonWriter writer, User? value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Id");
                writer.WriteValue(value.Id);
                writer.WritePropertyName("Username");
                writer.WriteValue(value.Username);
                writer.WritePropertyName("Password");
                writer.WriteNull();
                writer.WritePropertyName("IpAddress");
                writer.WriteNull();
                writer.WritePropertyName("Groups");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
        }
    }
}
