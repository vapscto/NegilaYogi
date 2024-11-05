using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Students_COB", Schema = "CLG")]
    public class BranchChangeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSCOB_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long ACSCOB_AMB_Id { get; set; }
        public long? ACSCOB_AMSE_Id_Old { get; set; }
        public long? ACSCOB_AMSE_Id_New { get; set; }
        public long AMB_Id { get; set; }
        public string ACSCOB_OldRegNo { get; set; }
        public string ACSCOB_NewRegNo { get; set; }
        public string ACSCOB_COBFees { get; set; }
        public string ACSCOB_Remarks { get; set; }
        public string ACSCOB_CreatedBy { get; set; }
        public string ACSCOB_UpdatedBy { get; set; }
        public bool ACSCOB_ActiveFlag { get; set; }
        public DateTime? ACSCOB_COBDate { get; set; }
    }
}
