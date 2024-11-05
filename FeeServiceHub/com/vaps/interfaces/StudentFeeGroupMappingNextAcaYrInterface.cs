using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface StudentFeeGroupMappingNextAcaYrInterface
    {
        FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO getgroupmappedheads(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO savedetails(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searching(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO Deletedetails(FeeStudentGroupMappingDTO data);
        
    }
}
