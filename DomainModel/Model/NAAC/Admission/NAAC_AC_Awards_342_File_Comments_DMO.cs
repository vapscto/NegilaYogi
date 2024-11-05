using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Awards_342_File_Comments")]
   public class NAAC_AC_Awards_342_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACAW342FC_Id { get; set; }
        public string NCACAW342FC_Remarks { get; set; }
        public long? NCACAW342FC_RemarksBy { get; set; }
        public bool? NCACAW342FC_ActiveFlag { get; set; }
        public long? NCACAW342FC_CreatedBy { get; set; }
        public DateTime? NCACAW342FC_CreatedDate { get; set; }
        public long? NCACAW342FC_UpdatedBy { get; set; }
        public DateTime? NCACAW342FC_UpdatedDate { get; set; }
        public string NCACAW342FC_StatusFlg { get; set; }
        public long NCACAW342F_Id { get; set; }
    }
}
