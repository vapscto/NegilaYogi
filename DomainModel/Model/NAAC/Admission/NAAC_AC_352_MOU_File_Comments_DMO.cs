using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_352_MOU_File_Comments")]
   public class NAAC_AC_352_MOU_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC352MOUFC_Id { get; set; }
        public string NCAC352MOUFC_Remarks { get; set; }
        public long? NCAC352MOUFC_RemarksBy { get; set; }
        public bool? NCAC352MOUFC_ActiveFlag { get; set; }
        public long? NCAC352MOUFC_CreatedBy { get; set; }
        public DateTime? NCAC352MOUFC_CreatedDate { get; set; }
        public long? NCAC352MOUFC_UpdatedBy { get; set; }
        public DateTime? NCAC352MOUFC_UpdatedDate { get; set; }
        public string NCAC352MOUFC_StatusFlg { get; set; }
        public long NCAC352MOUF_Id { get; set; }
    }
}
