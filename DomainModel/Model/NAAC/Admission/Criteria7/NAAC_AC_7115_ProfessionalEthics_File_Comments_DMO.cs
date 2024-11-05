using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7115_ProfessionalEthics_File_Comments")]
  public class NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC7115PROETHFC_Id { get; set; }
        public string NCAC7115PROETHFC_Remarks { get; set; }
        public long? NCAC7115PROETHFC_RemarksBy { get; set; }
        public bool? NCAC7115PROETHFC_ActiveFlag { get; set; }
        public long? NCAC7115PROETHFC_CreatedBy { get; set; }
        public DateTime? NCAC7115PROETHFC_CreatedDate { get; set; }
        public long? NCAC7115PROETHFC_UpdatedBy { get; set; }
        public DateTime? NCAC7115PROETHFC_UpdatedDate { get; set; }
        public string NCAC7115PROETHFC_StatusFlg { get; set; }
        public long NCAC7115PROETHF_Id { get; set; }
    }
}
