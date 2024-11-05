using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface External_Training_ApprovalInterface
    {
        External_Training_ApprovalDTO onloaddata(External_Training_ApprovalDTO data);
        External_Training_ApprovalDTO approvalstatus(External_Training_ApprovalDTO data);
        External_Training_ApprovalDTO deactiveY(External_Training_ApprovalDTO data);
        External_Training_ApprovalDTO trainingdetails(External_Training_ApprovalDTO data);
    }
}