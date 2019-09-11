using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace korteriyhistu.Models
{
    public class LogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string LogEntryId { get; set; }
        public string Message {get; set;}
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int BillId { get; set; } //foreign key

        /*
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                LogEntry log = (LogEntry)obj;
                return (log.BillId == BillId) && (log.Message.Equals(Message)) &&
                    (log.Comment.Equals(Comment)) && (log.BillId == BillId);
            }
        }
        */
    }
}
