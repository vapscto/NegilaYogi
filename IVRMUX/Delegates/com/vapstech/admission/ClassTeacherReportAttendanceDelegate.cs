using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class ClassTeacherReportAttendanceDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClassTeacherReportAttendanceDTO, ClassTeacherReportAttendanceDTO> _comm = new CommonDelegate<ClassTeacherReportAttendanceDTO, ClassTeacherReportAttendanceDTO>();
        public ClassTeacherReportAttendanceDTO getdata(ClassTeacherReportAttendanceDTO id)
        {
            return _comm.POSTDataADM(id, "ClassTeacherReportAttendanceFacade/getdata/");
        }
        public ClassTeacherReportAttendanceDTO getreport(ClassTeacherReportAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "ClassTeacherReportAttendanceFacade/getreport/");
        }
    }
}
