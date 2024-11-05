using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Awards_342_Comments")]
   public class NAAC_AC_Awards_342_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACAW342C_Id { get; set; }
        public string NCACAW342C_Remarks { get; set; }
        public long? NCACAW342C_RemarksBy { get; set; }
        public string NCACAW342C_StatusFlg { get; set; }
        public bool? NCACAW342C_ActiveFlag { get; set; }
        public long? NCACAW342C_CreatedBy { get; set; }
        public DateTime? NCACAW342C_CreatedDate { get; set; }
        public long? NCACAW342C_UpdatedBy { get; set; }
        public DateTime? NCACAW342C_UpdatedDate { get; set; }
        public long NCACAW342_Id { get; set; }
    }
}
