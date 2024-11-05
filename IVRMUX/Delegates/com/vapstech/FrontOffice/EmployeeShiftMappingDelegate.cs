using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class EmployeeShiftMappingDelegate
    {
        CommonDelegate<EmployeeShiftMappingDTO, EmployeeShiftMappingDTO> COMFRNT = new CommonDelegate<EmployeeShiftMappingDTO, EmployeeShiftMappingDTO>();
        public EmployeeShiftMappingDTO savedetail(EmployeeShiftMappingDTO categorypage)
        {

            return COMFRNT.POSTDataHolidayReport(categorypage, "EmployeeShiftMappingFacade/savedetail");
        }

        
        public EmployeeShiftMappingDTO getdetails(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "EmployeeShiftMappingFacade/getdetails/");
        }

        public EmployeeShiftMappingDTO Shiftname(EmployeeShiftMappingDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeShiftMappingFacade/Shiftname/");
        }

        public EmployeeShiftMappingDTO editdetails(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "EmployeeShiftMappingFacade/editdetails/");
        }
        public EmployeeShiftMappingDTO deleterec(EmployeeShiftMappingDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeShiftMappingFacade/deletedetails/");
        }

        public EmployeeShiftMappingDTO get_departments(EmployeeShiftMappingDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeShiftMappingFacade/get_departments/");
        }
        
        public EmployeeShiftMappingDTO get_designation(EmployeeShiftMappingDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeShiftMappingFacade/get_designation/");
        }
        
        public EmployeeShiftMappingDTO get_employee(EmployeeShiftMappingDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeShiftMappingFacade/get_employee/");
        }
    }

}
