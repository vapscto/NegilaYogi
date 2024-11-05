using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface StaffFeeGroupMappingInterface
    {
        FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO savedetails(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO deleterec(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO getsearchdata(int id, FeeStudentGroupMappingDTO org);
        FeeStudentGroupMappingDTO EditMasterscetionDetails(int id);
        FeeStudentGroupMappingDTO getstucls(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO getradiofiltereddata(FeeStudentGroupMappingDTO data);

        FeeStudentGroupMappingDTO getdataaspercategory(FeeStudentGroupMappingDTO data);

        FeeStudentGroupMappingDTO studentsavedgroupfacfun(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searching(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searchingstu(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO editstudata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO saveeditdata(FeeStudentGroupMappingDTO pgmod);
    }
}
