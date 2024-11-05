using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.HRMS
{
    [Table("NAAC_HR_Employee_BOSBOE_File_Comments")]
    public class HR_Employee_BOSBOE_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHRBOSFC_Id { get; set; }
        public long NCHREBOSF_Id { get; set; }
        public string NCHRBOSFC_Remarks { get; set; }
        public long NCHRBOSFC_RemarksBy { get; set; }
        public string NCHRBOSFC_StatusFlg { get; set; }
        public bool NCHRBOSFC_ActiveFlag { get; set; }
        public long NCHRBOSFC_CreatedBy { get; set; }
        public DateTime? NCHRBOSFC_CreatedDate { get; set; }
        public long NCHRBOSFC_UpdatedBy { get; set; }
        public DateTime? NCHRBOSFC_UpdatedDate { get; set; }
    }
}
