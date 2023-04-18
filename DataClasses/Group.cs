using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataClasses
{
    public class Group
    {
        public int Id { get; set; }
        [Column("GroupName", TypeName = "varchar")]
        public string GroupName { get; set; }

        public ICollection<User> Members { get; set; }

        public ICollection<Message> Messages { get; set; }

        public Group()
        {
            Members = new List<User>();
            Messages = new List<Message>();
        }

        public class GroupInUserConverter : JsonConverter<Group>
        {
            public override Group? ReadJson(JsonReader reader, Type objectType, Group? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<Group>(reader);
            }

            public override void WriteJson(JsonWriter writer, Group? value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Id");
                writer.WriteValue(value.Id);
                writer.WritePropertyName("GroupName");
                writer.WriteValue(value.GroupName);
                writer.WritePropertyName("Members");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WritePropertyName("Messages");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
        }
    }
}
