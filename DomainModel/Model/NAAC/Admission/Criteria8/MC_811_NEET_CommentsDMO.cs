using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_811_NEET_Comments")]
    public class MC_811_NEET_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC811NEETC_Id { get; set; }
        public string NCMC811NEETC_Remarks { get; set; }
        public string NCMC811NEETC_StatusFlg { get; set; }
        public long NCMC811NEETC_RemarksBy { get; set; }
        public long NCMC811NEETC_CreatedBy { get; set; }
        public long NCMC811NEETC_UpdatedBy { get; set; }
        public bool NCMC811NEETC_ActiveFlag { get; set; }
        public DateTime? NCMC811NEETC_CreatedDate { get; set; }
        public DateTime? NCMC811NEETC_UpdatedDate { get; set; }
        public long NCMC811NEET_Id { get; set; }
    }
}
