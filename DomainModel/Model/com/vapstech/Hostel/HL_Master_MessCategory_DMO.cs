using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_MessCategory")]
    public class HL_Master_MessCategory_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMMC_Name { get; set; }
        public bool HLMMC_ActiveFlag { get; set; }
        public long HLMMC_CreatedBy { get; set; }
        public long HLMMC_UpdatedBy { get; set; }

    }
}
