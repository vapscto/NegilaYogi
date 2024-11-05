using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_434_EContent_Comments")]
   public class NAAC_AC_434_EContent_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC434ECTC_Id { get; set; }
        public string NCAC434ECTC_Remarks { get; set; }
        public long? NCAC434ECTC_RemarksBy { get; set; }
        public string NCAC434ECTC_StatusFlg { get; set; }
        public bool? NCAC434ECTC_ActiveFlag { get; set; }
        public long? NCAC434ECTC_CreatedBy { get; set; }
        public DateTime? NCAC434ECTC_CreatedDate { get; set; }
        public long? NCAC434ECTC_UpdatedBy { get; set; }
        public DateTime? NCAC434ECTC_UpdatedDate { get; set; }
        public long NCAC434ECT_Id { get; set; }
    }
}
