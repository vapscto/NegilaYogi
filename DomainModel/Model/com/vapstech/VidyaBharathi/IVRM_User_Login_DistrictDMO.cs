using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("IVRM_User_Login_District")]
    public class IVRM_User_Login_DistrictDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMULDT_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long IVRMMD_Id { get; set; }
        public bool IVRMULDT_ActiveFlag { get; set; }
        public DateTime? IVRMULDT_CreatedDate { get; set; }
        public long IVRMULDT_CreatedBy { get; set; }
        public long IVRMULDT_UpdatedBy { get; set; }
        public DateTime? IVRMULDT_UpdatedDate { get; set; }
       // public long MI_ID { get; set; }
    }
}
