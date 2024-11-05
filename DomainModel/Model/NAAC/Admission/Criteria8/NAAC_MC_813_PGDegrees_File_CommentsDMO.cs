using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_813_PGDegrees_File_Comments")] 
   public class NAAC_MC_813_PGDegrees_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC813PGDEFC_Id { get; set; }
        public string NCMC813PGDEFC_Remarks { get; set; }
        public string NCMC813PGDEFC_StatusFlg { get; set; }
        public long NCMC813PGDEFC_RemarksBy { get; set; }
        public long NCMC813PGDEFC_CreatedBy { get; set; }
        public long NCMC813PGDEFC_UpdatedBy { get; set; }
        public bool NCMC813PGDEFC_ActiveFlag { get; set; }
        public DateTime? NCMC813PGDEFC_CreatedDate { get; set; }
        public DateTime? NCMC813PGDEFC_UpdatedDate { get; set; }
        public long NCMC813PGDEF_Id { get; set; }
    }
}
