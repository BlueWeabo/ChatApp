﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClasses
{
    public class Group
    {
        public int Id { get; set; }
        [Column("Username", TypeName = "varchar")]
        public string Name { get; set; }

        public ICollection<User> Members { get; set; }

        public ICollection<Message> Messages { get; set; }

        public Group()
        {
            Members = new List<User>();
            Messages = new List<Message>();
        }
    }
}
