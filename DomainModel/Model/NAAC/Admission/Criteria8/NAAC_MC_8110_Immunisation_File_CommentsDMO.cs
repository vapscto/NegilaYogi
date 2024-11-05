using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_8110_Immunisation_File_Comments")]
   public class NAAC_MC_8110_Immunisation_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC8110IMMFC_Id { get; set; }
        public string NCMC8110IMMFC_Remarks { get; set; }
        public string NCMC8110IMMFC_StatusFlg { get; set; }
        public long NCMC8110IMMFC_RemarksBy { get; set; }
        public long NCMC8110IMMFC_CreatedBy { get; set; }
        public long NCMC8110IMMFC_UpdatedBy { get; set; }
        public bool NCMC8110IMMFC_ActiveFlag { get; set; }
        public DateTime? NCMC8110IMMFC_CreatedDate { get; set; }
        public DateTime? NCMC8110IMMFC_UpdatedDate { get; set; }
        public long NCMC8110IMMF_Id { get; set; }
    }
}
