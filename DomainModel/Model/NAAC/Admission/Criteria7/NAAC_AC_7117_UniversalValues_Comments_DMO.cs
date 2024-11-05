using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7117_UniversalValues_Comments")]
   public class NAAC_AC_7117_UniversalValues_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC7117UNIVALC_Id { get; set; }
        public string NCAC7117UNIVALC_Remarks { get; set; }
        public long? NCAC7117UNIVALC_RemarksBy { get; set; }
        public string NCAC7117UNIVALC_StatusFlg { get; set; }
        public bool? NCAC7117UNIVALC_ActiveFlag { get; set; }
        public long? NCAC7117UNIVALC_CreatedBy { get; set; }
        public DateTime? NCAC7117UNIVALC_CreatedDate { get; set; }
        public long? NCAC7117UNIVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVALC_UpdatedDate { get; set; }
        public long NCAC7117UNIVAL_Id { get; set; }
    }
}
