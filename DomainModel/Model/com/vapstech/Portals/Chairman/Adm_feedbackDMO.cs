using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.Portals.Chairman
{
    [Table("Adm_feedback")]
    public class Adm_feedbackDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long adm_fid { get; set; }
      
        public string adm_name { get; set; }
        public string adm_emailid { get; set; }
        public long adm_contactno { get; set; }
      
        public string adm_comment { get; set; }
        public DateTime adm_date { get; set; }



    }

}
