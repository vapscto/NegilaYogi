using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_TDS_QUARTERDTO : CommonParamDTO
    {
        public long HRETDSQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long HRMQ_Id { get; set; }
        public string HRETDSR_ReceiptNo { get; set; }
        public decimal? HRETDS_AmountPaid { get; set; }
        public bool HRETDSQ_ActiveFlg { get; set; }
        public long HRETDSQ_CreatedBy { get; set; }
        public long HRETDSQ_UpdatedBy { get; set; }
        public decimal? HRETDS_TaxDeposited { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public Array employeedropdown { get; set; }


        public string hrmE_EmployeeFirstName { get; set; }

        public string HRMQ_QuarterName{get;set;}
        public string IMFY_FinancialYear { get; set; }
        public Array leaveyeardropdown { get; set; }
       public long LogInUserId { get; set; }
        //Academic Year
        public decimal? empGrossSal { get; set; }
        public Array quarterdropdown { get; set; }
        public DateTime? IMFY_FromDate { get; set; }
        public DateTime? IMFY_ToDate { get; set; }
    }

}
