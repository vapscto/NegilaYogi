using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_Competition_Category")]
    public class VBSC_Master_Competition_CategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBSCMCC_Id { get; set; }
        public long MT_Id { get; set; }
        public string VBSCMCC_CompetitionCategory { get; set; }
        public string VBSCMCC_CCDesc { get; set; }
        public bool VBSCMCC_CCAgeFlag { get; set; }
        public long VBSCMCC_CCAgeFromYear { get; set; }
        public long VBSCMCC_CCAgeFromMonth { get; set; }
        public long VBSCMCC_CCAgeFromDays { get; set; }
        public long VBSCMCC_CCAgeToYear { get; set; }
        public long VBSCMCC_CCAgeToMonth { get; set; }
        public long VBSCMCC_CCAgeToDays { get; set; }
        public bool  VBSCMCC_CCWeightFlag { get; set; }
        public int VBSCMCC_CCFromWeight { get; set; }
        public int VBSCMCC_CCToWeight { get; set; }
        public bool VBSCMCC_CCClassFlg { get; set; }
        public bool VBSCMCC_ActiveFlag { get; set; }
        public DateTime? VBSCMCC_CreatedDate { get; set; }
        public DateTime? VBSCMCC_UpdatedDate { get; set; }
        public long VBSCMCC_CreatedBy { get; set; }
        public long VBSCMCC_UpdatedBy { get; set; }
    }
}
