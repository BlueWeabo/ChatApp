using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClasses;

namespace ChatApp
{
    public class GroupButton : Button
    {
        private Group? internalGroup;

        public void SetGroup(Group group)
        {
            internalGroup = group;
            Text = group.GroupName;
        }

        public Group? GetGroup()
        {
            return internalGroup;
        }
    }
}
