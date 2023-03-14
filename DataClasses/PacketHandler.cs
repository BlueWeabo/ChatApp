using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataClasses
{
    public class PacketHandler
    {
        public static string EncodeLoginPacket(User user)
        {
            return "Logging:" + JsonConvert.SerializeObject(user);
        }

        public static User? DecodeLoginPacket(string jsonUser)
        {
            return JsonConvert.DeserializeObject<User>(jsonUser);
        }

        public static string EncodeRegisterPacket(User user)
        {
            return "Register:" + JsonConvert.SerializeObject(user);
        }

        public static User? DecodeRegisterPacket(string jsonUser)
        {
            return JsonConvert.DeserializeObject<User>(jsonUser);
        }

        public static string EncodeGroupPacket(Group group)
        {
            return "Group:" + JsonConvert.SerializeObject(group);
        }

        public static Group? DecodeGroupPacket(string jsonGroup)
        {
            return JsonConvert.DeserializeObject<Group>(jsonGroup);
        }

        public static string EncodeMessagePacket(Message message)
        {
            return "Message:" + JsonConvert.SerializeObject(message);
        }

        public static Message? DecodeMessagePacket(string jsonMessage)
        {
            return JsonConvert.DeserializeObject<Message>(jsonMessage);
        }

    }
}
