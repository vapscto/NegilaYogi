using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class StudentAttendanceEntryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentAttendanceEntryDTO, StudentAttendanceEntryDTO> COMMM = new CommonDelegate<StudentAttendanceEntryDTO, StudentAttendanceEntryDTO>();
       
        public StudentAttendanceEntryDTO GetInitialData (StudentAttendanceEntryDTO enqdto)
        {
            return COMMM.POSTDataADM(enqdto, "StudentAttendanceEntryFacade/getinitialdata/");
        }
        public StudentAttendanceEntryDTO GetStudentData (StudentAttendanceEntryDTO attdto)
        {
            return COMMM.POSTDataADM(attdto, "StudentAttendanceEntryFacade/getstudentdata/");
        }
        public StudentAttendanceEntryDTO SaveStudentAttendance(StudentAttendanceEntryDTO attdto)
        {
            return COMMM.POSTDataADM(attdto, "StudentAttendanceEntryFacade/savestudentattendance/");
        }
        public StudentAttendanceEntryDTO getmonthclassheld(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/getmonthclassheld/");
        }
        public StudentAttendanceEntryDTO getdatewiseatt(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/getdatewiseatt/");
        }
        public StudentAttendanceEntryDTO year(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/year/");
        }
        public StudentAttendanceEntryDTO getbatchlist(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/getbatchlist/");
        }
        public StudentAttendanceEntryDTO getstdlistperiod(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/getstdlistperiod/");
        }
        public StudentAttendanceEntryDTO Deleteattendance(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/Deleteattendance/");
        }
        public StudentAttendanceEntryDTO ViewAttendanceDetailsStaffWise(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/ViewAttendanceDetailsStaffWise/");
        }
        public StudentAttendanceEntryDTO AttendanceDeleteRecordWise(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/AttendanceDeleteRecordWise/");
        }

        //studentattendanceinsert
        public StudentAttendanceEntryDTO studentattendanceinsert(StudentAttendanceEntryDTO data)
        {
            return COMMM.POSTDataADM(data, "StudentAttendanceEntryFacade/studentattendanceinsert/");
        }

    }
}
