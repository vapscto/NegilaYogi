using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_Subject", Schema = "CLG")]
    public class PA_College_Student_SubjectDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
       public long PACSS_Id { get; set; }
        public long  PACA_Id { get; set; }
        public long  ISMS_Id { get; set; }
        public bool  PACSS_ActiveFlg { get; set; }
        public long  PACSS_CreatedBy { get; set; }
        public DateTime  PACSS_CreatedDate { get; set; }
        public long  PACSS_UpdatedBy { get; set; }
        public DateTime PACSS_UpdatedDate { get; set; }

    }
}
