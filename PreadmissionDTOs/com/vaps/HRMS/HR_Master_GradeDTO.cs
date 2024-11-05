using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_GradeDTO:CommonParamDTO
    {
    public long HRMG_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRMG_GradeName { get; set; }
    public string HRMG_GradeDisplayName { get; set; }
    public string HRMG_PayScaleRange { get; set; }
    public decimal HRMG_PayScaleFrom { get; set; }
    public decimal HRMG_IncrementOf { get; set; }
    public decimal HRMG_PayScaleTo { get; set; }
    public Int32? HRMG_Order { get; set; }
    public bool HRMG_ActiveFlag { get; set; }
    public Array gradelList { get; set; }
    public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public HR_Master_GradeDTO[] GradeDTO { get; set; }
    }
}
