using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_653_IQAC_File_Comments")]
    public class NAAC_AC_653_IQAC_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC653IQACFC_Id { get; set; }
        public string NCAC653IQACFC_Remarks { get; set; }
        public long? NCAC653IQACFC_RemarksBy { get; set; }
        public bool? NCAC653IQACFC_ActiveFlag { get; set; }
        public long? NCAC653IQACFC_CreatedBy { get; set; }
        public DateTime? NCAC653IQACFC_CreatedDate { get; set; }
        public long? NCAC653IQACFC_UpdatedBy { get; set; }
        public DateTime? NCAC653IQACFC_UpdatedDate { get; set; }
        public string NCAC653IQACFC_StatusFlg { get; set; }
        public long NCAC653IQACF_Id { get; set; }
    }
}
