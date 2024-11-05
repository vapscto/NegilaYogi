using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_413_ICT_File_Comments")]
   public class NAAC_AC_413_ICT_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCAC413ICTFC_Id { get; set; }
        public string NCAC413ICTFC_Remarks { get; set; }
        public long? NCAC413ICTFC_RemarksBy { get; set; }
        public bool? NCAC413ICTFC_ActiveFlag { get; set; }
        public long? NCAC413ICTFC_CreatedBy { get; set; }
        public DateTime? NCAC413ICTFC_CreatedDate { get; set; }
        public long? NCAC413ICTFC_UpdatedBy { get; set; }
        public DateTime? NCAC413ICTFC_UpdatedDate { get; set; }
        public string NCAC413ICTFC_StatusFlg { get; set; }
        public long NCAC413ICTF_Id { get; set; }

    }
}
