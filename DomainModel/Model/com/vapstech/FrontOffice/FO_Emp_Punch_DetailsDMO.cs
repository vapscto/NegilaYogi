using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Punch_Details", Schema ="FO")]
    public class FO_Emp_Punch_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long FOEPD_Id { get; set; }
        public long MI_Id { get; set; }
        public long FOEP_Id { get; set; }
        public string FOEPD_PunchTime { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public string FOEPD_Flag { get; set; }
        public string FOEPD_Temperature { get; set; }
       
    }
}     
     
















       
