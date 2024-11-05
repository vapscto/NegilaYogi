using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Yrly_Cat_Exams_Subwise_PT", Schema = "Exm")]
    public class Exm_Yrly_Cat_Exams_Subwise_PTDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EYCESPT_Id { get; set; }
        public int EYCES_Id { get; set; }
        public int EMPATY_Id { get; set; }
        public int EMGR_Id { get; set; }
        public bool? EYCESPT_ActiveFlg { get; set; }
        public long? EYCESPT_CreatedBy { get; set; }
        public DateTime? EYCESPT_CreatedDate { get; set; }
        public long? EYCESPT_UpdatedBy { get; set; }
        public DateTime? EYCESPT_UpdatedDate { get; set; }
    }
}