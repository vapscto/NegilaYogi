using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_718_WasteManagement_File_Comments")]
   public class NAAC_AC_718_WasteManagement_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC718WAMANFC_Id { get; set; }
        public string NCAC718WAMANFC_Remarks { get; set; }
        public long? NCAC718WAMANFC_RemarksBy { get; set; }
        public bool? NCAC718WAMANFC_ActiveFlag { get; set; }
        public long? NCAC718WAMANFC_CreatedBy { get; set; }
        public DateTime? NCAC718WAMANFC_CreatedDate { get; set; }
        public long? NCAC718WAMANFC_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMANFC_UpdatedDate { get; set; }
        public string NCAC718WAMANFC_StatusFlg { get; set; }
        public long NCAC718WAMANF_Id { get; set; }

    }
}
