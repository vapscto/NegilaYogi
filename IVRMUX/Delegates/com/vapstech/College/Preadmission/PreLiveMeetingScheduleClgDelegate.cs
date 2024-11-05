using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class PreLiveMeetingScheduleClgDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PreLiveMeetingScheduleClgDTO, PreLiveMeetingScheduleClgDTO> COMMM = new CommonDelegate<PreLiveMeetingScheduleClgDTO, PreLiveMeetingScheduleClgDTO>();

        public PreLiveMeetingScheduleClgDTO getempdetails(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/getempdetails/");
        }
        public PreLiveMeetingScheduleClgDTO ondatechangestudent(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/ondatechangestudent/");
        }
        public PreLiveMeetingScheduleClgDTO onschedulecahnge(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/onschedulecahnge/");
        }
        public PreLiveMeetingScheduleClgDTO endmainmeeting(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/endmainmeetingstudent/");
        }
        public PreLiveMeetingScheduleClgDTO onstartmeeting(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/onstartmeeting/");
        }
        public PreLiveMeetingScheduleClgDTO saveremarks(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/saveremarks/");
        }
        public PreLiveMeetingScheduleClgDTO getstudentdetails(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/getstudentdetails/");
        }

        public PreLiveMeetingScheduleClgDTO joinmeetingStudent(PreLiveMeetingScheduleClgDTO data)
        {
            return COMMM.CollegePOSTData(data, "PreLiveMeetingScheduleClgFacade/joinmeetingStudent/");
        }
    }
}
