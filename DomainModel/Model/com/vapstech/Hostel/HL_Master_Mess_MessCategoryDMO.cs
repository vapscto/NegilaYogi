using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Mess_MessCategory")]
    public class HL_Master_Mess_MessCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMMMC_Id { get; set; }
        public long HLMM_Id { get; set; }
        public long HLMMC_Id { get; set; }
      
        public bool HLMMC_ActiveFlag { get; set; }
        public DateTime? HLMMC_CreatedDate { get; set; }
        public DateTime? HLMMC_UpdatedDate { get; set; }
        public long HLMMC_CreatedBy { get; set; }
        public long HLMMC_UpdatedBy { get; set; }
        //public List<HL_Master_Mess_DMO> HL_Master_Mess_DMO { get; set; }
        //public List<HL_Master_MessCategory_DMO> HL_Master_MessCategory_DMO { get; set; }
    }
}
