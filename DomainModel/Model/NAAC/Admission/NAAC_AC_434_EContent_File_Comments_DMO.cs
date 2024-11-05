using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_434_EContent_File_Comments")]
   public class NAAC_AC_434_EContent_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC434ECTFC_Id { get; set; }
        public string NCAC434ECTFC_Remarks { get; set; }
        public long? NCAC434ECTFC_RemarksBy { get; set; }
        public bool? NCAC434ECTFC_ActiveFlag { get; set; }
        public long? NCAC434ECTFC_CreatedBy { get; set; }
        public DateTime? NCAC434ECTFC_CreatedDate { get; set; }
        public long? NCAC434ECTFC_UpdatedBy { get; set; }
        public DateTime? NCAC434ECTFC_UpdatedDate { get; set; }
        public string NCAC434ECTFC_StatusFlg { get; set; }
        public long NCAC434ECTF_Id { get; set; }
    }
}
