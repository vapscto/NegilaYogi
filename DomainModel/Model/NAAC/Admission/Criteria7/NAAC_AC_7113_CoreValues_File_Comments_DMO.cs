using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{

    [Table("NAAC_AC_7113_CoreValues_File_Comments")]
   public class NAAC_AC_7113_CoreValues_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7113CORVALFC_Id { get; set; }
        public string NCAC7113CORVALFC_Remarks { get; set; }
        public long? NCAC7113CORVALFC_RemarksBy { get; set; }
        public bool? NCAC7113CORVALFC_ActiveFlag { get; set; }
        public long? NCAC7113CORVALFC_CreatedBy { get; set; }
        public DateTime? NCAC7113CORVALFC_CreatedDate { get; set; }
        public long? NCAC7113CORVALFC_UpdatedBy { get; set; }
        public DateTime? NCAC7113CORVALFC_UpdatedDate { get; set; }
        public string NCAC7113CORVALFC_StatusFlg { get; set; }
        public long NCAC7113CORVALF_Id { get; set; }
    }
}
