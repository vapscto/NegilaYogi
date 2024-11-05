using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_441_ExpAcaFacility_Comments")]
   public class NAAC_AC_441_ExpAcaFacility_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC441EXACFCC_Id { get; set; }
        public string NCAC441EXACFCC_Remarks { get; set; }
        public long? NCAC441EXACFCC_RemarksBy { get; set; }
        public string NCAC441EXACFCC_StatusFlg { get; set; }
        public bool? NCAC441EXACFCC_ActiveFlag { get; set; }
        public long? NCAC441EXACFCC_CreatedBy { get; set; }
        public DateTime? NCAC441EXACFCC_CreatedDate { get; set; }
        public long? NCAC441EXACFCC_UpdatedBy { get; set; }
        public DateTime? NCAC441EXACFCC_UpdatedDate { get; set; }
        public long NCAC441EXACFC_Id { get; set; }
    }
}
