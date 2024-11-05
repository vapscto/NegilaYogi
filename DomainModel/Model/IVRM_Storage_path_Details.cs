using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Azure_Storage_Details")]
    public class IVRM_Storage_path_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_SD { get; set; }      
        public string IVRM_SD_Access_Name { get; set; }
        public string IVRM_SD_Access_Key { get; set; }
        public string IVRM_VMS_Subscription_URL { get; set; }
    }
}
