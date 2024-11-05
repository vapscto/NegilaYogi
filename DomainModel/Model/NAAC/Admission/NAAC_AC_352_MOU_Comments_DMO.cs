using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_352_MOU_Comments")]
   public class NAAC_AC_352_MOU_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC352MOUC_Id { get; set; }
        public string NCAC352MOUC_Remarks { get; set; }
        public long? NCAC352MOUC_RemarksBy { get; set; }
        public string NCAC352MOUC_StatusFlg { get; set; }
        public bool? NCAC352MOUC_ActiveFlag { get; set; }
        public long? NCAC352MOUC_CreatedBy { get; set; }
        public DateTime? NCAC352MOUC_CreatedDate { get; set; }
        public long? NCAC352MOUC_UpdatedBy { get; set; }
        public DateTime? NCAC352MOUC_UpdatedDate { get; set; }
        public long NCAC352MOU_Id { get; set; }
    }
}
