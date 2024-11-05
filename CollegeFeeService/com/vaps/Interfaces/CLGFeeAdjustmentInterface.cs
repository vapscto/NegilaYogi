using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CLGFeeAdjustmentInterface
    {
        CLGFeeAdjustmentDTO getdata(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getdataclassdet(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getdatasectiondet(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getdatastudentdet(CLGFeeAdjustmentDTO data);        
        CLGFeeAdjustmentDTO getdatabothgroupdet(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getdatafromheaddet(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getdatatoheaddet(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO savedatadelegate(CLGFeeAdjustmentDTO data);
        CLGFeeAdjustmentDTO getpageedit(int id);
         CLGFeeAdjustmentDTO deleterec(int id);
        CLGFeeAdjustmentDTO searching(CLGFeeAdjustmentDTO data);
    }
}
