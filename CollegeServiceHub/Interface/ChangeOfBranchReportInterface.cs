using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface ChangeOfBranchReportInterface
    {
        ChangeOfBranchReportDTO loaddata(ChangeOfBranchReportDTO data);
        ChangeOfBranchReportDTO getcourse(ChangeOfBranchReportDTO data);
        ChangeOfBranchReportDTO getbranch(ChangeOfBranchReportDTO data);
       Task<ChangeOfBranchReportDTO> Report(ChangeOfBranchReportDTO data);
    }
}
