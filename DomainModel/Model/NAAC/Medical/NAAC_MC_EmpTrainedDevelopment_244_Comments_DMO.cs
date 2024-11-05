using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_EmpTrainedDevelopment_244_Comments")]
   public class NAAC_MC_EmpTrainedDevelopment_244_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCETD244C_Id { get; set; }
        public string NCMCETD244C_Remarks { get; set; }
        public long NCMCETD244C_RemarksBy { get; set; }
        public string NCMCETD244C_StatusFlg { get; set; }
        public bool? NCMCETD244C_ActiveFlag { get; set; }
        public long? NCMCETD244C_CreatedBy { get; set; }
        public DateTime? NCMCETD244C_CreatedDate { get; set; }
        public long? NCMCETD244C_UpdatedBy { get; set; }
        public DateTime? NCMCETD244C_UpdatedDate { get; set; }
        public long NCMCETD244_Id { get; set; }
    }
}
