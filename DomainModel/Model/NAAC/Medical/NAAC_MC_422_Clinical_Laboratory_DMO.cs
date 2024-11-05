using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_422_Clinical_Laboratory")]
    public class NAAC_MC_422_Clinical_Laboratory_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC422CL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC422CL_Year { get; set; }
        public long NCMC422CL_NoOfOutpatientsTreated { get; set; }
        public string NCMC422CL_OutStuPatientRatio { get; set; }
        public long NCMC422CL_NoofInPatientsTreated { get; set; }
        public DateTime NCMC422CL_CreatedDate { get; set; }
        public DateTime NCMC422CL_UpdatedDate { get; set; }
        public long NCMC422CL_CreatedBy { get; set; }
        public long NCMC422CL_UpdatedBy { get; set; }
        public bool NCMC422CL_ActiveFlag { get; set; }
        public string NCMC422CL_InStuPatientRatio { get; set; }

        public List<NAAC_MC_422_Clinical_Laboratory_files_DMO> NAAC_MC_422_Clinical_Laboratory_files_DMO { get; set; }

    }
}
