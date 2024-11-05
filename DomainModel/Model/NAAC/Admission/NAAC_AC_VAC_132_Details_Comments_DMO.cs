using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Details_Comments")]
   public class NAAC_AC_VAC_132_Details_Comments_DMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACVAC132DC_Id { get; set; }
        public string NCACVAC132DC_Remarks { get; set; }
        public long? NCACVAC132DC_RemarksBy { get; set; }
        public string NCACVAC132DC_StatusFlg { get; set; }
        public bool? NCACVAC132DC_ActiveFlag { get; set; }
        public long? NCACVAC132DC_CreatedBy { get; set; }
        public DateTime? NCACVAC132DC_CreatedDate { get; set; }
        public long? NCACVAC132DC_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DC_UpdatedDate { get; set; }
        public long NCACVAC132D_Id { get; set; }
    }
}
