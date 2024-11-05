using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentAttendanceEntryFacadeController : Controller
    {
        private StudentAttendanceEntryInterface _IStdAtt;

        public StudentAttendanceEntryFacadeController(StudentAttendanceEntryInterface IStdAtt)
        {
            _IStdAtt = IStdAtt;
        }
       
        [Route("getstudentdata")]
        public Task<StudentAttendanceEntryDTO> GetStudentData([FromBody]StudentAttendanceEntryDTO attdto)
        {
            return _IStdAtt.GetStudentData(attdto);
        }
        [Route("getinitialdata")]
        public StudentAttendanceEntryDTO GetIntialData([FromBody]StudentAttendanceEntryDTO id)
        {
            return _IStdAtt.GetInitialData(id);
        }

        [Route("savestudentattendance")]
        public Task<StudentAttendanceEntryDTO> SaveStudentAttendance([FromBody]StudentAttendanceEntryDTO attdto)
        {
            return _IStdAtt.SaveStudentAttendance(attdto);
        }

        [Route("Deleteattendance")]
        public Task<StudentAttendanceEntryDTO> Deleteattendance([FromBody]StudentAttendanceEntryDTO attdto)
        {
            return _IStdAtt.Deleteattendance(attdto);
        }        

        [Route("getmonthclassheld")]
        public Task<StudentAttendanceEntryDTO> getmonthclassheld ([FromBody] StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.getmonthclassheld(data);
        }

        [Route("getdatewiseatt")]
        public Task<StudentAttendanceEntryDTO> getdatewiseatt([FromBody] StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.getdatewiseatt(data);
        }

        [Route("ViewAttendanceDetailsStaffWise")]
        public StudentAttendanceEntryDTO ViewAttendanceDetailsStaffWise([FromBody]StudentAttendanceEntryDTO id)
        {
            return _IStdAtt.ViewAttendanceDetailsStaffWise(id);
        }

        [Route("AttendanceDeleteRecordWise")]
        public StudentAttendanceEntryDTO AttendanceDeleteRecordWise([FromBody]StudentAttendanceEntryDTO id)
        {
            return _IStdAtt.AttendanceDeleteRecordWise(id);
        }

        [Route("year")]
        public StudentAttendanceEntryDTO year([FromBody]StudentAttendanceEntryDTO id)
        {
            return _IStdAtt.year(id);
        }

        [Route("getbatchlist")]
        public StudentAttendanceEntryDTO getbatchlist([FromBody] StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.getbatchlist(data);
        }

        [Route("getstdlistperiod")]
        public StudentAttendanceEntryDTO getstdlistperiod([FromBody] StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.getstdlistperiod(data);
        }

        [Route("getSmartCardData")]
        public class_section_list getSmartCardData([FromBody]class_section_list data)
        {
            return _IStdAtt.getSmartCardData(data);
        }

        [Route("SaveSmartCardData")]
        public Task<class_section_list> SaveSmartCardData([FromBody]class_section_list data)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            data.ASA_Network_IP = remoteIpAddress.ToString();
            return _IStdAtt.SaveSmartCardData(data);
        }

        [Route("sendsmsabsent")]
        public StudentAttendanceEntryDTO sendsmsabsent([FromBody]StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.sendsmsabsent(data);
        }

        [Route("saveattendancesmartcard")]
        public StudentAttendanceEntryDTO saveattendancesmartcard([FromBody]StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.saveattendancesmartcard(data);
        }

        [Route("getstudentdetailssmart")]
        public class_section_list getstudentdetailssmart([FromBody]class_section_list data)
        {
            return _IStdAtt.getstudentdetailssmart(data);
        }

        [Route("GETRFIDDATA")]
        public RFIDDATA GETRFIDDATA([FromBody]RFIDDATA data)
        {
            return _IStdAtt.GETRFIDDATA(data);
        }

        //studentattendanceinsert
        [Route("studentattendanceinsert")]
        public StudentAttendanceEntryDTO studentattendanceinsert([FromBody] StudentAttendanceEntryDTO data)
        {
            return _IStdAtt.studentattendanceinsert(data);
        }
    }
}
