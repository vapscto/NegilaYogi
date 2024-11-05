using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Adm_Student_Activities")]
    public class Adm_Student_Activities 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool ASA_ApprovedFlg { get; set; }
        public long ASA_ApprovedBy { get; set; }
        public bool ASA_ActiveFlg { get; set; }
        public long ASA_CreatedBy { get; set; }
        public DateTime ASA_CreatedDate { get; set; }
        public long ASA_UpdatedBy { get; set; }
        public DateTime ASA_UpdatedDate { get; set; }
    }
}
