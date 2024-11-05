using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
  public interface StudentFeeGroupMappingGroupDeletionInterface
    {
        FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO deleterecord(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO Getreport(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO onclickClass(FeeStudentGroupMappingDTO data);
    }
}
