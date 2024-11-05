using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface StaffAndOtherFeeGroupMappingInterface
    {
        Clg_StudentFeeGroupMapping_DTO getdata(Clg_StudentFeeGroupMapping_DTO data);

        Clg_StudentFeeGroupMapping_DTO savedetails_s(Clg_StudentFeeGroupMapping_DTO pgmod);
        Clg_StudentFeeGroupMapping_DTO savedetails_o(Clg_StudentFeeGroupMapping_DTO pgmod);
   
        Clg_StudentFeeGroupMapping_DTO deleterec_s(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO deleterec_o(Clg_StudentFeeGroupMapping_DTO data);
        //Clg_StudentFeeGroupMapping_DTO getsearchdata(int id, Clg_StudentFeeGroupMapping_DTO org);
        Clg_StudentFeeGroupMapping_DTO EditMasterscetionDetails(int id);
    
        Clg_StudentFeeGroupMapping_DTO getradiofiltereddata(Clg_StudentFeeGroupMapping_DTO data);

        Clg_StudentFeeGroupMapping_DTO getdataaspercategory(Clg_StudentFeeGroupMapping_DTO data);

        Clg_StudentFeeGroupMapping_DTO studentsavedgroupfacfun(Clg_StudentFeeGroupMapping_DTO data);
      
        Clg_StudentFeeGroupMapping_DTO searching_s(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO searching_o(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO searchingstu(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO searchingstaff(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO searchingothers(Clg_StudentFeeGroupMapping_DTO data);
        Clg_StudentFeeGroupMapping_DTO editstudata(Clg_StudentFeeGroupMapping_DTO data);
        //Clg_StudentFeeGroupMapping_DTO saveeditdata(Clg_StudentFeeGroupMapping_DTO pgmod);
        Clg_StudentFeeGroupMapping_DTO getacademicyr(Clg_StudentFeeGroupMapping_DTO pgmod);
        //Clg_StudentFeeGroupMapping_DTO fillstudentsroute(Clg_StudentFeeGroupMapping_DTO pgmod);

        Clg_StudentFeeGroupMapping_DTO geteditdatastaffothers(Clg_StudentFeeGroupMapping_DTO pgmod);

        Clg_StudentFeeGroupMapping_DTO saveeditdataothers(Clg_StudentFeeGroupMapping_DTO pgmod);

        Clg_StudentFeeGroupMapping_DTO saveeditdatastaff(Clg_StudentFeeGroupMapping_DTO pgmod);

    }
}
