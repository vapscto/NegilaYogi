
using System;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class AttendanceStaffWiseDelegates
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AttendanceStaffWiseDTO, AttendanceStaffWiseDTO> COMMM = new CommonDelegate<AttendanceStaffWiseDTO, AttendanceStaffWiseDTO>();
        public AttendanceStaffWiseDTO Getdetails(AttendanceStaffWiseDTO data)
        {
            return COMMM.POSTPORTALData(data, "AttendanceStaffWiseFacade/Getdetails/");
        }
        public AttendanceStaffWiseDTO Getdepartment(AttendanceStaffWiseDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "AttendanceStaffWiseFacade/Getdepartment/");
        }
        public AttendanceStaffWiseDTO get_designation(AttendanceStaffWiseDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "AttendanceStaffWiseFacade/get_designation/");
        }
        public AttendanceStaffWiseDTO get_department(AttendanceStaffWiseDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "AttendanceStaffWiseFacade/get_department/");
        }
        public AttendanceStaffWiseDTO get_employee(AttendanceStaffWiseDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "AttendanceStaffWiseFacade/get_employee/");
        }
    }
}
