using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_HSU_511_NonGovScholarship")]
    public class NAAC_HSU_511_NonGovScholarshipDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
    
        public long NCAC512NGSCH_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC512NGSCH_Year { get; set; }
        public string  NCAC512NGSCH_SchemeName { get; set; }
        public string NCAC512NGSCH_StatusFlg { get; set; }
        public long  NCAC512NGSCH_NoOfStudents { get; set; }
        public bool  NCAC512NGSCH_ActiveFlg { get; set; }
        public long  NCAC512NGSCH_CreatedBy { get; set; }
        public long  NCAC512NGSCH_UpdatedBy { get; set; }
        public DateTime  NCAC512NGSCH_CreatedDate { get; set; }
        public DateTime NCAC512NGSCH_UpdatedDate { get; set; }
        public List<NAAC_HSU_511_NonGovScholarship_FilesDMO> NAAC_HSU_511_NonGovScholarship_FilesDMO { get; set; }

    }
}
