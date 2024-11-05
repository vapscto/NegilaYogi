using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_623_EGovernance_File_Comments")]
    public class NAAC_AC_623_EGovernance_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC623EGOVFC_Id { get; set; }
        public string NCAC623EGOVFC_Remarks { get; set; }
        public long? NCAC623EGOVFC_RemarksBy { get; set; }
        public bool? NCAC623EGOVFC_ActiveFlag { get; set; }
        public long? NCAC623EGOVFC_CreatedBy { get; set; }
        public DateTime? NCAC623EGOVFC_CreatedDate { get; set; }
        public long? NCAC623EGOVFC_UpdatedBy { get; set; }
        public DateTime? NCAC623EGOVFC_UpdatedDate { get; set; }
        public string NCAC623EGOVFC_StatusFlg { get; set; }
        public long NCAC623EGOVF_Id { get; set; }
    }
}
