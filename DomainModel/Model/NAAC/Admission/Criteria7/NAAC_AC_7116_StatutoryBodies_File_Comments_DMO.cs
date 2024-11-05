using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7116_StatutoryBodies_File_Comments")]
   public class NAAC_AC_7116_StatutoryBodies_File_Comments_DMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC7116STABODFC_Id { get; set; }
        public string NCAC7116STABODFC_Remarks { get; set; }
        public long? NCAC7116STABODFC_RemarksBy { get; set; }
        public bool? NCAC7116STABODFC_ActiveFlag { get; set; }
        public long? NCAC7116STABODFC_CreatedBy { get; set; }
        public DateTime? NCAC7116STABODFC_CreatedDate { get; set; }
        public long? NCAC7116STABODFC_UpdatedBy { get; set; }
        public DateTime? NCAC7116STABODFC_UpdatedDate { get; set; }
        public string NCAC7116STABODFC_StatusFlg { get; set; }
        public long NCAC7116STABODF_Id { get; set; }
    }
}
