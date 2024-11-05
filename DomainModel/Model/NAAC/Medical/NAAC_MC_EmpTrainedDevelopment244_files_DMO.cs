using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_EmpTrainedDevelopment_244_Files")]
    public class NAAC_MC_EmpTrainedDevelopment244_files_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCETD244F_Id { get; set; }
        [ForeignKey("NCMCETD244_Id")]
        public long NCMCETD244_Id { get; set; }
        public string NCMCETD244F_FileDesc { get; set; }
        public string NCMCETD244F_FileName { get; set; }
        public string NCMCETD244F_FilePath { get; set; }
        public bool NCMCETD244F_ActiveFlg { get; set; }
        public long NCMCETD244F_CreatedBy { get; set; }
        public long NCMCETD244F_UpdatedBy { get; set; }
        public DateTime NCMCETD244F_CreatedDate { get; set; }
        public DateTime NCMCETD244F_UpdatedDate { get; set; }
        public string NCMCETD244F_StatusFlg { get; set; }
        public string NCMCETD244F_Remarks { get; set; }
        public bool? NCMCETD244F_ApprovedFlg { get; set; }

        


    }
}
