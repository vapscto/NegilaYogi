using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Class_Subject_Room")]
    public class TT_Class_Subject_RoomDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTCSRM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMRM_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public bool TTCSRM_ActiveFlg { get; set; }
        public long TTCSRM_CreatedBy { get; set; }
        public long TTCSRM_UpdatedBy { get; set; }
        public DateTime TTCSRM_CreatedDate { get; set; }
        public DateTime TTCSRM_UpdatedDate { get; set; }

    }
}
