using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Candidates_Exam_Answer")]
    public class OT_Candidates_Exam_AnswerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long OTCANDEXANS_Id { get; set; }
        public long OTCANDEX_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
        public long OTMOEQOA_Id { get; set; }
        public bool OTCANDEXANS_CorrectAnsFlg { get; set; }
        public bool OTCANDEXANS_ActiveFlg { get; set; }
        public long OTCANDEXANS_CreatedBy { get; set; }
        public DateTime OTCANDEXANS_CreatedDate { get; set; }
        public long OTCANDEXANS_UpdatedBy { get; set; }
        public DateTime OTCANDEXANS_UpdatedDate { get; set; }
    }
}