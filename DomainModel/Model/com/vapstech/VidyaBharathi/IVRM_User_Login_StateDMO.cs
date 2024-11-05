using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("IVRM_User_Login_State")]
    public class IVRM_User_Login_StateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMULST_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public bool IVRMULST_ActiveFlag { get; set; }
        public DateTime? IVRMULST_CreatedDate { get; set; }
        public long IVRMULST_CreatedBy { get; set; }
        public long IVRMULST_UpdatedBy { get; set; }
        public DateTime? IVRMULST_UpdatedDate { get; set; }
       // public long MI_ID { get; set; }
    }
}
