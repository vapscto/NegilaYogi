using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_File_Comments")]
   public class NAAC_AC_VAC_132_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACVAC132FC_Id { get; set; }
        public string NCACVAC132FC_Remarks { get; set; }
        public long? NCACVAC132FC_RemarksBy { get; set; }
        public bool? NCACVAC132FC_ActiveFlag { get; set; }
        public long? NCACVAC132FC_CreatedBy { get; set; }
        public DateTime? NCACVAC132FC_CreatedDate { get; set; }
        public long? NCACVAC132FC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132FC_UpdatedDate { get; set; }
        public string NCACVAC132FC_StatusFlg { get; set; }
        public long NCACVAC132F_Id { get; set; }
    }
}
