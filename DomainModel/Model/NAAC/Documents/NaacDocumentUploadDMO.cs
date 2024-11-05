using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_SL")]
    public class NaacDocumentUploadDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACSL_Id { get; set; }
        public string NAACSL_SLNo { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public long NAACSL_ParentId { get; set; }
        public int NAACSL_SLNoOrder { get; set; }
        public string NAACSL_SLNote { get; set; }
        public bool NAACSL_ActiveFlag { get; set; }
        public long NAACSL_CreatedBy { get; set; }
        public long NAACSL_UpdatedBy { get; set; }
        public DateTime? NAACSL_CreatedDate { get; set; }
        public DateTime? NAACSL_UpdatedDate { get; set; }
        public string NAASCL_Template { get; set; }
        public bool? NAACSL_TextBoxFlg { get; set; }
        public bool? NAACSL_UploadReq { get; set; }
        public decimal? NAACSL_Percentage { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
    }
}