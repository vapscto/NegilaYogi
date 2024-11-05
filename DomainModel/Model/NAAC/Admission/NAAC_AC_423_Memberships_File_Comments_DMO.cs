using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_423_Memberships_File_Comments")]
   public class NAAC_AC_423_Memberships_File_Comments_DMO
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC423MEMFC_Id { get; set; }
        public string NCAC423MEMFC_Remarks { get; set; }
        public long? NCAC423MEMFC_RemarksBy { get; set; }
        public bool? NCAC423MEMFC_ActiveFlag { get; set; }
        public long? NCAC423MEMFC_CreatedBy { get; set; }
        public DateTime? NCAC423MEMFC_CreatedDate { get; set; }
        public long? NCAC423MEMFC_UpdatedBy { get; set; }
        public DateTime? NCAC423MEMFC_UpdatedDate { get; set; }
        public string NCAC423MEMFC_StatusFlg { get; set; }
        public long NCAC423MEMF_Id { get; set; }
    }
}
