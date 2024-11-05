using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Staff_Deputation")]
    public class TT_Staff_DeputationDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public long TTSD_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TTSD_Date { get; set; }
        public long ASMAY_Id { get; set; }
        // public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }                                                       
        public long ASMS_Id { get; set;}
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long TTSD_AbsentStaff { get; set; }
        public long TTSD_DeputedStaff { get; set; }      
        public string TTSD_Remarks { get; set; }
    }
}
