using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_HallTicket_College", Schema = "CLG")]
    public class Exm_HallTicketCollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EHTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string EHTC_HallTicketNo { get; set; }
        public bool EHTC_PublishFlg { get; set; }
        public bool EHTC_ActiveFlag { get; set; }
        public DateTime? EHTC_CreatedDate { get; set; }
        public DateTime? EHTC_UpdatedDate { get; set; }
      
        public int EME_Id { get; set; }
    }
}
