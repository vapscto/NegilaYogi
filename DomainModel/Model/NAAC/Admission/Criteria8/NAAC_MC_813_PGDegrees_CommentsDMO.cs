using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_813_PGDegrees_Comments")] 
   public class NAAC_MC_813_PGDegrees_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC813PGDEC_Id { get; set; }
        public string NCMC813PGDEC_Remarks { get; set; }
        public string NCMC813PGDEC_StatusFlg { get; set; }
        public long NCMC813PGDEC_RemarksBy { get; set; }
        public long NCMC813PGDEC_CreatedBy { get; set; }
        public long NCMC813PGDEC_UpdatedBy { get; set; }
        public bool NCMC813PGDEC_ActiveFlag { get; set; }
        public DateTime? NCMC813PGDEC_CreatedDate { get; set; }
        public DateTime? NCMC813PGDEC_UpdatedDate { get; set; }
        public long NCMC813PGDE_Id { get; set; }
    }
}
