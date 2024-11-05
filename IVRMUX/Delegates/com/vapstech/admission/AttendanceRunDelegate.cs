using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class AttendanceRunDelegate
    {
        CommonDelegate<AttendanceRunDTO, AttendanceRunDTO> comm = new CommonDelegate<AttendanceRunDTO, AttendanceRunDTO>();
        public AttendanceRunDTO loaddata(AttendanceRunDTO data)
        {
            return comm.POSTDataADM(data, "AttendanceRunFacade/loaddata");
        }

        public AttendanceRunDTO savedetails(AttendanceRunDTO data)
        {
            return comm.POSTDataADM(data, "AttendanceRunFacade/savedetails/");
        }

        public AttendanceRunDTO griddetails(AttendanceRunDTO data)
        {
            return comm.POSTDataADM(data, "AttendanceRunFacade/griddetails/");
        }

    }
}
