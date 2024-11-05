using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_Master_SL")]
    public class NaacDocumentUploadDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACMSL_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public long MI_Id { get; set; }
        public string NAACMSL_Status { get; set; }
        public string NAACMSL_Uploadpath { get; set; }
        public string NAACMSL_ConsultantRemarks { get; set; }
        public string NAACMSL_UserRemarks { get; set; }
        public bool NAACMSL_ActiveFlag { get; set; }
        public string NAACMSL_Details { get; set; }
        public long NAACMSL_CreatedBy { get; set; }
        public DateTime NAACMSL_CreatedDate { get; set; }
        public long NAACMSL_UpdatedBy { get; set; }
        public DateTime NAACMSL_UpdatedDate { get; set; }
        public decimal? NAACMSL_CGPA { get; set; }
        public List<NAAC_Master_SL_FileDMO> NAAC_Master_SL_FileDMO { get; set; }
        public List<NAAC_Master_SL_LinkDMO> NAAC_Master_SL_LinkDMO { get; set; }

    }
}
