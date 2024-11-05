using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_QuestionPaperType")]
    public class OT_QuestionPaperTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long OTQPTYP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_Id { get; set; }
        public string OTQPTYP_QuestionPaperName { get; set; }
        public string OTQPTYP_QuestionPaperDesc { get; set; }
        public bool OTQPTYP_ActiveFlg { get; set; }
        public DateTime OTQPTYP_CreatedDate { get; set; }
        public DateTime OTQPTYP_UpdatedDate { get; set; }
        public long OTQPTYP_CreatedBy { get; set; }
        public long OTQPTYP_UpdatedBy { get; set; }

    }
}