using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Ivrm_User_StudentApp_login")]
    public class StudentAppUserLoginDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMUSLAPP_ID { get; set; }
        public long AMST_ID { get; set; }
        public int STD_APP_ID { get; set; }
        public int FAT_APP_ID { get; set; }
        public int MOT_APP_ID { get; set; } 
    }
}
