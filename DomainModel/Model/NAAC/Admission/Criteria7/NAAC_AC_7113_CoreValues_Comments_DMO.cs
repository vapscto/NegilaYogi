using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7113_CoreValues_Comments")]

   public class NAAC_AC_7113_CoreValues_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7113CORVALC_Id { get; set; }
        public string NCAC7113CORVALC_Remarks { get; set; }
        public long? NCAC7113CORVALC_RemarksBy { get; set; }
        public string NCAC7113CORVALC_StatusFlg { get; set; }
        public bool? NCAC7113CORVALC_ActiveFlag { get; set; }
        public long? NCAC7113CORVALC_CreatedBy { get; set; }
        public DateTime? NCAC7113CORVALC_CreatedDate { get; set; }
        public long? NCAC7113CORVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7113CORVALC_UpdatedDate { get; set; }
        public long NCAC7113CORVAL_Id { get; set; }
    }
}
