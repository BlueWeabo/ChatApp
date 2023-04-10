using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static DataClasses.Group;
using static DataClasses.User;

namespace DataClasses
{
    public class PacketHandler
    {
        public static string EncodeLoginPacket(User user)
        {
            return "Logging:" + JsonConvert.SerializeObject(user);
        }

        public static string EncodeUserGetPacket(User user)
        {
            return "User:" + "Get:" + JsonConvert.SerializeObject(user);
        }

        public static User? DecodeUserPacket(string jsonUser)
        {
            return JsonConvert.DeserializeObject<User>(jsonUser);
        }

        public static string EncodeUserPacket(User user)
        {
            return JsonConvert.SerializeObject(user, new GroupInUserConverter());
        }

        public static string EncodeRegisterPacket(User user)
        {
            return "Register:" + JsonConvert.SerializeObject(user);
        }

        public static string EncodeGroupPacket(Group group)
        {
            return JsonConvert.SerializeObject(group, new UserInGroupConverter());
        }

        public static string EncodeGroupAddPacket(Group group)
        {
            return "Group:" + "Add:" + JsonConvert.SerializeObject(group);
        }

        public static Group? DecodeGroupPacket(string jsonGroup)
        {
            return JsonConvert.DeserializeObject<Group>(jsonGroup);
        }

        public static string EncodeMessagePacket(Message message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public static Message? DecodeMessagePacket(string jsonMessage)
        {
            return JsonConvert.DeserializeObject<Message>(jsonMessage);
        }

    }
}
