using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class SwimmingAttendanceDelegate
    {
        CommonDelegate<SwimmingAttendanceDTO, SwimmingAttendanceDTO> _comm = new CommonDelegate<SwimmingAttendanceDTO, SwimmingAttendanceDTO>();

        public SwimmingAttendanceDTO loaddata(SwimmingAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceFacade/loaddata");
        }
        public SwimmingAttendanceDTO onchnageyear(SwimmingAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceFacade/onchnageyear");
        }
        public SwimmingAttendanceDTO onchangeclass(SwimmingAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceFacade/onchangeclass");
        }
        public SwimmingAttendanceDTO search(SwimmingAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceFacade/search");
        }
        public SwimmingAttendanceDTO save(SwimmingAttendanceDTO data)
        {
            return _comm.POSTDataADM(data, "SwimmingAttendanceFacade/save");
        }
        


    }
}
