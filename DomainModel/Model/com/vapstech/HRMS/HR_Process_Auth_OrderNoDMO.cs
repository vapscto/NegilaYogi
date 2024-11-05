using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Process_Auth_OrderNo")]
    public class HR_Process_Auth_OrderNoDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRPAON_Id { get; set; }
        [ForeignKey("HRPA_Id")]
        public long HRPA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public int HRPAON_SanctionLevelNo { get; set; }
        public bool HRPAON_FinalFlg { get; set; }
        public long? HRPAON_UpdatedBy { get; set; }
        public long? HRPAON_CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
