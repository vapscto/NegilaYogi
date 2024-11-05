using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("MOBILE_INSTITUTION")]
    public class MOBILE_INSTITUTION 
    {
        [Key]
        public long INSTITUTIONID { get; set; }
        public string INT_NAME { get; set; }
        public long MI_ID { get; set; }

        public string INSTITUTION_NAME { get; set; }

        public string INSTITUTION_LOGO { get; set; }
    }
}
