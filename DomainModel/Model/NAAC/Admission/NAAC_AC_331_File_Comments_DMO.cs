using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_331_File_Comments")]
   public class NAAC_AC_331_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC331FC_Id { get; set; }
        public string NCAC331FC_Remarks { get; set; }
        public long? NCAC331FC_RemarksBy { get; set; }
        public bool? NCAC331FC_ActiveFlag { get; set; }
        public long? NCAC331FC_CreatedBy { get; set; }
        public DateTime? NCAC331FC_CreatedDate { get; set; }
        public long? NCAC331FC_UpdatedBy { get; set; }
        public DateTime? NCAC331FC_UpdatedDate { get; set; }
        public string NCAC331FC_StatusFlg { get; set; }
        public long NCAC331F_Id { get; set; }
    }
}
