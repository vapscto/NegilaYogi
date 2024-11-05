using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Details_File_Comments")]
   public class NAAC_AC_VAC_132_Details_File_Comments_DMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACVAC132DFC_Id { get; set; }
        public string NCACVAC132DFC_Remarks { get; set; }
        public long? NCACVAC132DFC_RemarksBy { get; set; }
        public bool? NCACVAC132DFC_ActiveFlag { get; set; }
        public long? NCACVAC132DFC_CreatedBy { get; set; }
        public DateTime? NCACVAC132DFC_CreatedDate { get; set; }
        public long? NCACVAC132DFC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DFC_UpdatedDate { get; set; }
        public string NCACVAC132DFC_StatusFlg { get; set; }
        public long NCACVAC132DF_Id { get; set; }
    }
}
