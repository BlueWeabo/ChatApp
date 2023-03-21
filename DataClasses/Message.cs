using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClasses
{
    public class Message
    {
        public int Id { get; set; }
        [Column("MessageText", TypeName = "varchar")]
        public string Text { get; set; }
        public User Sender { get; set; } 
    }
}
