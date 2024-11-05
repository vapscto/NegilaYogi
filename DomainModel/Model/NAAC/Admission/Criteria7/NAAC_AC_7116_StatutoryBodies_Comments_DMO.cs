using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7116_StatutoryBodies_Comments")]
   public class NAAC_AC_7116_StatutoryBodies_Comments_DMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC7116STABODC_Id { get; set; }
        public string NCAC7116STABODC_Remarks { get; set; }
        public long? NCAC7116STABODC_RemarksBy { get; set; }
        public string NCAC7116STABODC_StatusFlg { get; set; }
        public bool? NCAC7116STABODC_ActiveFlag { get; set; }
        public long? NCAC7116STABODC_CreatedBy { get; set; }
        public DateTime? NCAC7116STABODC_CreatedDate { get; set; }
        public long? NCAC7116STABODC_UpdatedBy { get; set; }
        public DateTime? NCAC7116STABODC_UpdatedDate { get; set; }
        public long NCAC7116STABOD_Id { get; set; }
    }
}
