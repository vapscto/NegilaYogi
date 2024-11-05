using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_StudentActivities")]
    public class HR_Employee_StudentActivitiesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRESACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRESACT_TypeOfActivity { get; set; }
        public DateTime HRESACT_ActivityDate { get; set; }
        public string HRESACT_OrgAgency { get; set; }
        public string HRESACT_Place { get; set; }
        public string HRESACT_Duration { get; set; }
        public string HRESACT_Role { get; set; }
        public string HRESACT_Document { get; set; }
        public bool HRESACT_ActiveFlg { get; set; }
        public long HRESACT_CreatedBy { get; set; }
        public long HRESACT_UpdatedBy { get; set; }
        public string HRESACT_Year { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }
}
