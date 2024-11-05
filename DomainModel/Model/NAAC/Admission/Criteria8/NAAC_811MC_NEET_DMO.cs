using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_811_NEET")]
    public  class NAAC_811MC_NEET_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC811NEET_Id { get; set; }
        public long MI_Id { get; set; }      
        public long NCMC811NEET_Year { get; set; }
        public long NCMC811NEET_NoOfStudentsEnrolled { get; set; }
        public string NCMC811NEET_Range { get; set; }
        public string NCMC811NEET_StatusFlg { get; set; }
        public decimal NCMC811NEET_Mean { get; set; }
        public decimal NCMC811NEET_StandardDeviation { get; set; }
        public bool NCMC811NEET_ActiveFlg { get; set; }
        public long NCMC811NEET_CreatedBy { get; set; }
        public long NCMC811NEET_UpdatedBy { get; set; }
        public DateTime NCMC811NEET_CreatedDate { get; set; }
        public DateTime NCMC811NEET_UpdatedDate { get; set; }
        public List<NAAC_811MC_NEET_Files_DMO> NAAC_811MC_NEET_Files_DMO { get; set; }
    }
}
