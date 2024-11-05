using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_online_application_download_details")]
    public class Registration : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AOAD_ID { get; set; }
        public string student_name { get; set; }
        public int AMCL_ID { get; set; }
        public string Email_id { get; set; }
        public string Mobileno { get; set; }
        //public DateTime download_date { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string user_ip_address { get; set; }
    }
}


