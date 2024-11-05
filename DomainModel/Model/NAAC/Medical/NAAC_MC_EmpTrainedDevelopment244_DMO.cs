using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_EmpTrainedDevelopment_244")]
    public class NAAC_MC_EmpTrainedDevelopment244_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCETD244_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCETD244_Year { get; set; }
        public long NCMCETD244_NoOfTechersTrainedForDevOfEcontents { get; set; }
        public long NCMCETD244_TotalNoOfTeachers { get; set; }
        public bool NCMCETD244_ActiveFlag { get; set; }
        public long NCMCETD244_CreatedBy { get; set; }
        public long NCMCETD244_UpdatedBy { get; set; }
        public DateTime NCMCETD244_CreatedDate { get; set; }
        public DateTime NCMCETD244_UpdatedDate { get; set; }
        public string NCMCETD244_StatusFlg { get; set; }
        public string NCMCETD244_Remarks { get; set; }
        public bool? NCMCETD244_ApprovedFlg { get; set; }
        

        public List<NAAC_MC_EmpTrainedDevelopment244_files_DMO> NAAC_MC_EmpTrainedDevelopment244_files_DMO { get; set; }
    }
}
