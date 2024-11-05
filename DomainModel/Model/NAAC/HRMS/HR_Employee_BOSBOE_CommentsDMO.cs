using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.HRMS
{
    [Table("NAAC_HR_Employee_BOSBOE_Comments")]
    public class HR_Employee_BOSBOE_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCHRBOSC_Id { get; set; }
        public long HREBOS_Id { get; set; }
        public string NCHRBOSC_Remarks { get; set; }
        public long NCHRBOSC_RemarksBy { get; set; }
        public string NCHRBOSC_StatusFlg { get; set; }
        public bool NCHRBOSC_ActiveFlag { get; set; }
        public long NCHRBOSC_CreatedBy { get; set; }
        public DateTime? NCHRBOSC_CreatedDate { get; set; }
        public long NCHRBOSC_UpdatedBy { get; set; }
        public DateTime? NCHRBOSC_UpdatedDate { get; set; }
    }
}
