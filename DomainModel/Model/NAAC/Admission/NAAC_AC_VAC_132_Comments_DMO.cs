using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Comments")]
   public class NAAC_AC_VAC_132_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACVAC132C_Id { get; set; }
        public string NCACVAC132C_Remarks { get; set; }
        public long? NCACVAC132C_RemarksBy { get; set; }
        public string NCACVAC132C_StatusFlg { get; set; }
        public bool? NCACVAC132C_ActiveFlag { get; set; }
        public long? NCACVAC132C_CreatedBy { get; set; }
        public DateTime? NCACVAC132C_CreatedDate { get; set; }
        public long? NCACVAC132C_UpdatedBy { get; set; }
        public DateTime? NCACVAC132C_UpdatedDate { get; set; }
        public long NCACVAC132_Id { get; set; }
    }
}
