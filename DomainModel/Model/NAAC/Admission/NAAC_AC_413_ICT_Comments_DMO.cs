using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_413_ICT_Comments")]
   public class NAAC_AC_413_ICT_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC413ICTC_Id { get; set; }
        public string NCAC413ICTC_Remarks { get; set; }
        public long? NCAC413ICTC_RemarksBy { get; set; }
        public string NCAC413ICTC_StatusFlg { get; set; }
        public bool? NCAC413ICTC_ActiveFlag { get; set; }
        public DateTime? NCAC413ICTC_CreatedDate { get; set; }
        public long? NCAC413ICTC_CreatedBy { get; set; }
        public long? NCAC413ICTC_UpdatedBy { get; set; }
        public DateTime? NCAC413ICTC_UpdatedDate { get; set; }
        public long NCAC413ICT_Id { get; set; }
    }
}
