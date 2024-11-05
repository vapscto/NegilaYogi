using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("IVRM_Master_District")]
    public class DistrictDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMD_Id { get; set; }
        public string IVRMMD_Name { get; set; }
     
        public string IVRMMD_Code { get; set; }
        public long IVRMMS_Id { get; set; }
        public DateTime IVRMMD_CreatedDate { get; set; }
        public DateTime IVRMMD_UpdatedDate { get; set; }
        public long? IVRMMD_CreatedBy { get; set; }
        public long? IVRMMD_UpdatedBy { get; set; }
        public bool? IVRMMD_ActiveFlag { get; set; }
      
        



    }
}
