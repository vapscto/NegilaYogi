using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeAdjustmentInterface
    {
        FeeStudentAdjustmentDTO getdata(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getdataclassdet(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getdatasectiondet(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getdatastudentdet(FeeStudentAdjustmentDTO data);        
        FeeStudentAdjustmentDTO getdatabothgroupdet(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getdatafromheaddet(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getdatatoheaddet(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO savedatadelegate(FeeStudentAdjustmentDTO data);
        FeeStudentAdjustmentDTO getpageedit(int id);
         FeeStudentAdjustmentDTO deleterec(int id);
        FeeStudentAdjustmentDTO searching(FeeStudentAdjustmentDTO data);
    }
}
