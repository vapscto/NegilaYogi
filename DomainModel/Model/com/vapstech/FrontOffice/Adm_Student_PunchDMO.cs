using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("Adm_Student_Punch")]
    public class Adm_Student_PunchDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASPU_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ASPU_PunchDate { get; set; }
        public bool ASPU_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool ASPU_ManualEntryFlg { get; set; }
        public long? ASPU_CreatedBy { get; set; }
        public long? ASPU_UpdatedBy { get; set; }
    }
}     
     
















       
