using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("API_MOBILE")]
    public class API_MOBILE
    {
        [Key]
        public long API_ID { get; set; }
        public string API_URL { get; set; } 
        public string API_NAME { get; set; }
        public long INT_ID { get; set; }
    }
}
