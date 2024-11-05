using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Class_Subject_Room_College")]
    public class CLGTT_Course_Subject_RoomDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TTCSRMC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMRM_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public bool TTCSRMC_ActiveFlg { get; set; }
        public long TTCSRMC_CreatedBy { get; set; }
        public long TTCSRMC_UpdatedBy { get; set; }
        public DateTime TTCSRMC_CreatedDate { get; set; }
        public DateTime TTCSRMC_UpdatedDate { get; set; }

    }
}
