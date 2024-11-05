using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface StudentFeeGroupMappingInterface
    {
        FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO savedetails(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO savedetails_s(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO savedetails_o(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO deleterec(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO deleterec_s(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO deleterec_o(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO getsearchdata(int id, FeeStudentGroupMappingDTO org);
        FeeStudentGroupMappingDTO EditMasterscetionDetails(int id);
        FeeStudentGroupMappingDTO getstucls(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO getradiofiltereddata(FeeStudentGroupMappingDTO data);

        FeeStudentGroupMappingDTO getdataaspercategory(FeeStudentGroupMappingDTO data);

        FeeStudentGroupMappingDTO studentsavedgroupfacfun(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searching(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searching_s(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searching_o(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searchingstu(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searchingstaff(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO searchingothers(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO editstudata(FeeStudentGroupMappingDTO data);
        FeeStudentGroupMappingDTO saveeditdata(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO getacademicyr(FeeStudentGroupMappingDTO pgmod);
        FeeStudentGroupMappingDTO fillstudentsroute(FeeStudentGroupMappingDTO pgmod);

        FeeStudentGroupMappingDTO geteditdatastaffothers(FeeStudentGroupMappingDTO pgmod);

        FeeStudentGroupMappingDTO saveeditdataothers(FeeStudentGroupMappingDTO pgmod);

        FeeStudentGroupMappingDTO saveeditdatastaff(FeeStudentGroupMappingDTO pgmod);

    }
}
