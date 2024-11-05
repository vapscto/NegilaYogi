using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_331_Comments")]
   public class NAAC_AC_331_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC331C_Id { get; set; }
        public string NCAC331C_Remarks { get; set; }
        public long? NCAC331C_RemarksBy { get; set; }
        public string NCAC331C_StatusFlg { get; set; }
        public bool? NCAC331C_ActiveFlag { get; set; }
        public long? NCAC331C_CreatedBy { get; set; }
        public DateTime? NCAC331C_CreatedDate { get; set; }
        public long? NCAC331C_UpdatedBy { get; set; }
        public DateTime? NCAC331C_UpdatedDate { get; set; }
        public long NCAC331_Id { get; set; }
    }
}
