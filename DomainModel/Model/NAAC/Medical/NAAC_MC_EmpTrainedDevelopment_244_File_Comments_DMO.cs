using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_EmpTrainedDevelopment_244_File_Comments")]
    public class NAAC_MC_EmpTrainedDevelopment_244_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCETD244FC_Id { get; set; }
        public string NCMCETD244FC_Remarks { get; set; }
        public long? NCMCETD244FC_RemarksBy { get; set; }
        public bool? NCMCETD244FC_ActiveFlag { get; set; }
        public long? NCMCETD244FC_CreatedBy { get; set; }
        public DateTime? NCMCETD244FC_CreatedDate { get; set; }
        public long? NCMCETD244FC_UpdatedBy { get; set; }
        public DateTime? NCMCETD244FC_UpdatedDate { get; set; }
        public string NCMCETD244FC_StatusFlg { get; set; }
        public long NCMCETD244F_Id { get; set; }
    }
}
