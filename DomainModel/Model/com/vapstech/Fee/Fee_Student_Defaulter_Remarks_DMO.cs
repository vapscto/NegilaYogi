using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Student_Defaulter_Remarks")]
    public class Fee_Student_Defaulter_Remarks_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSDREM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMT_Id { get; set; }
        public long User_Id { get; set; }
        public string FSDREM_Remarks { get; set; }
        public DateTime? FSDREM_RemarksDate { get; set; }
        public bool FSDREM_ActiveFlag { get; set; }
        public DateTime? FSDREM_CreatedDate { get; set; }
        public long FSDREM_CreatedBy { get; set; }
        public DateTime? FSDREM_UpdatedDate { get; set; }
        public long FSDREM_UpdatedBy { get; set; }
    }
}
