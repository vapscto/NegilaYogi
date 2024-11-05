using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_718_WasteManagement_Comments")]
   public class NAAC_AC_718_WasteManagement_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC718WAMANC_Id { get; set; }
        public string NCAC718WAMANC_Remarks { get; set; }
        public long? NCAC718WAMANC_RemarksBy { get; set; }
        public string NCAC718WAMANC_StatusFlg { get; set; }
        public bool? NCAC718WAMANC_ActiveFlag { get; set; }
        public long? NCAC718WAMANC_CreatedBy { get; set; }
        public DateTime? NCAC718WAMANC_CreatedDate { get; set; }
        public long? NCAC718WAMANC_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMANC_UpdatedDate { get; set; }
        public long NCAC718WAMAN_Id { get; set; }
    }
}
