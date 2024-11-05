using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_714_LEDBulbs_Comments")]
   public class NAAC_AC_714_LEDBulbs_Comments_DMO
    {
        [Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC714LEDBUC_Id { get; set; }
        public string NCAC714LEDBUC_Remarks { get; set; }
        public long? NCAC714LEDBUC_RemarksBy { get; set; }
        public string NCAC714LEDBUC_StatusFlg { get; set; }
        public bool? NCAC714LEDBUC_ActiveFlag { get; set; }
        public long? NCAC714LEDBUC_CreatedBy { get; set; }
        public DateTime? NCAC714LEDBUC_CreatedDate { get; set; }
        public long? NCAC714LEDBUC_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBUC_UpdatedDate { get; set; }
        public long NCAC714LEDBU_Id { get; set; }
    }
}
