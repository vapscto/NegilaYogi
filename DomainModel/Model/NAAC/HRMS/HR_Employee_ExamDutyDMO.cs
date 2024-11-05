using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_ExamDuty")]
    public class HR_Employee_ExamDutyDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREEXDT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREEXDT_ExamDutyType { get; set; }
        public string HREEXDT_ExaminerType { get; set; }
        public string HREEXDT_CollUniName { get; set; }
        public bool HREEXDT_ActiveFlg { get; set; }
        public long HREEXDT_CreatedBy { get; set; }
        public long HREEXDT_UpdatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }
}
