using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Fee_Y_Payment_Alumni", Schema = "ALU")]
    public class Fee_Y_Payment_AlumniDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYPALUM_Id { get; set; }
        public long FYP_Id { get; set; }
        public long ALSREG_Id { get; set; }
        public long ALMST_Id { get; set; }
        public bool FYPALUM_ActiveFlg { get; set; }
        public long FYPALUM_CreatedBy { get; set; }
        public DateTime? FYPALUM_CreatedDate { get; set; }
        public long FYPALUM_UpdatedBy { get; set; }
        public DateTime? FYPALUM_UpdatedDate { get; set; }
    }
}
