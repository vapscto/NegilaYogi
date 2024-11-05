using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Candidates_Exam_SubjectiveAnswer")]
    public class OT_Candidates_Exam_SubjectiveAnswerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long OTCANDEXSANS_Id { get; set; }
        public long OTCANDEX_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
        public string OTCANDEXSANS_Answer { get; set; }
        public decimal OTCANDEXSANS_Marks { get; set; }
        public long HRME_Id { get; set; }
        public bool OTCANDEXSANS_ActiveFlg { get; set; }
        public long OTCANDEXSANS_CreatedBy { get; set; }
        public DateTime OTCANDEXSANS_CreatedDate { get; set; }
        public long OTCANDEXSANS_UpdatedBy { get; set; }
        public DateTime OTCANDEXSANS_UpdatedDate { get; set; }
    }
}