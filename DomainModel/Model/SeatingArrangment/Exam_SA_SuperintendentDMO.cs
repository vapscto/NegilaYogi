using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Superintendent")]
    public class Exam_SA_SuperintendentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESASINTDNT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public string ESASINTDNT_ChiefSupName { get; set; }
        public string ESASINTDNT_ChiefSupCollege { get; set; }
        public string ESASINTDNT_DeptChiefSupName { get; set; }
        public string ESASINTDNT_DeptChiefSupCollege { get; set; }
        public DateTime ESASINTDNT_FromDate { get; set; }
        public DateTime ESASINTDNT_ToDate { get; set; }
        public bool ESASINTDNT_ActiveFlg { get; set; }
        public DateTime? ESASINTDNT_CreatedDate { get; set; }
        public DateTime? ESASINTDNT_UpdatedDate { get; set; }
        public long ESASINTDNT_CreatedBy { get; set; }
        public long ESASINTDNT_UpdatedBy { get; set; }

    }
}
