using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7115_ProfessionalEthics_Comments")]
   public class NAAC_AC_7115_ProfessionalEthics_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC7115PROETHC_Id { get; set; }
        public string NCAC7115PROETHC_Remarks { get; set; }
        public long? NCAC7115PROETHC_RemarksBy { get; set; }
        public string NCAC7115PROETHC_StatusFlg { get; set; }
        public bool? NCAC7115PROETHC_ActiveFlag { get; set; }
        public long? NCAC7115PROETHC_CreatedBy { get; set; }
        public DateTime? NCAC7115PROETHC_CreatedDate { get; set; }
        public long? NCAC7115PROETHC_UpdatedBy { get; set; }
        public DateTime? NCAC7115PROETHC_UpdatedDate { get; set; }
        public long NCAC7115PROETH_Id { get; set; }
    }
}
