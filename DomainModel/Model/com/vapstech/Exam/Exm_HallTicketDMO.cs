using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_HallTicket", Schema = "Exm")]
    public class Exm_HallTicketDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EHT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_Id { get; set; }
        public long AMST_Id { get; set; }
        public string EHT_HallTicketNo { get; set; }
        public bool EHT_ActiveFlag { get; set; }
        public bool? EHT_PublishFlg { get; set; }
    }
}
