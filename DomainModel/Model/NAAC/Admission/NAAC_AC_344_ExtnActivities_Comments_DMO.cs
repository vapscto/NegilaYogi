using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_Comments")]
   public class NAAC_AC_344_ExtnActivities_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET344C_Id { get; set; }
        public string NCACET344C_Remarks { get; set; }
        public long? NCACET344C_RemarksBy { get; set; }
        public string NCACET344C_StatusFlg { get; set; }
        public bool? NCACET344C_ActiveFlag { get; set; }
        public long? NCACET344C_CreatedBy { get; set; }
        public DateTime? NCACET344C_CreatedDate { get; set; }
        public long? NCACET344C_UpdatedBy { get; set; }
        public DateTime? NCACET344C_UpdatedDate { get; set; }
        public long NCACET343_Id { get; set; }
    }
}
