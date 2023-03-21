using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClasses;

namespace ChatApp
{
    public static class ButtonExtension
    {
        private static Group? internalGroup;

        public static void SetGroup(this Button button, Group group)
        {
            internalGroup = group;
            button.Text = group.Name;
        }

        public static Group GetGroup(this Button button)
        {
            return internalGroup;
        }
    }
}
