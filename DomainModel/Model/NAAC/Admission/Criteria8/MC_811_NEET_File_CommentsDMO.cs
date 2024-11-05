using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_811_NEET_File_Comments")]
    public class MC_811_NEET_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC811NEETFC_Id { get; set; }
        public string NCMC811NEETFC_Remarks { get; set; }
        public string NCMC811NEETFC_StatusFlg { get; set; }
        public long NCMC811NEETFC_RemarksBy { get; set; }
        public long NCMC811NEETFC_CreatedBy { get; set; }
        public long NCMC811NEETFC_UpdatedBy { get; set; }
        public bool NCMC811NEETFC_ActiveFlag { get; set; }
        public DateTime? NCMC811NEETFC_CreatedDate { get; set; }
        public DateTime? NCMC811NEETFC_UpdatedDate { get; set; }
        public long NCMC811NEETF_Id { get; set; }
    }
}
