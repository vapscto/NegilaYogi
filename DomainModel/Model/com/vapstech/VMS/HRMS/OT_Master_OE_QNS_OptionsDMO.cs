using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Master_OE_QNS_Options")]
    public class OT_Master_OE_QNS_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OTMOEQOA_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
        public bool OTMOEQOA_ActiveFlg { get; set; }
        public long OTMOEQOA_CreatedBy { get; set; }
        public DateTime OTMOEQOA_CreatedDate { get; set; }
        public long OTMOEQOA_UpdatedBy { get; set; }
        public DateTime OTMOEQOA_UpdatedDate { get; set; }
    }
}