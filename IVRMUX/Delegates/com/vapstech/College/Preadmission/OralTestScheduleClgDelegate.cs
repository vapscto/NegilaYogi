using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class OralTestScheduleClgDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<DocumentViewClgDTO, DocumentViewClgDTO> COMMM = new CommonDelegate<DocumentViewClgDTO, DocumentViewClgDTO>();

        public DocumentViewClgDTO GetOralTestScheduleData(DocumentViewClgDTO lo)
        {
            return COMMM.CollegePOSTData(lo, "OralTestScheduleClgFacade/Getdetails/");
        }

        public DocumentViewClgDTO coursewisestudent(DocumentViewClgDTO OralTestScheduleDTO)
        {
            return COMMM.CollegePOSTData(OralTestScheduleDTO, "OralTestScheduleClgFacade/coursewisestudent/");
        }

        public DocumentViewClgDTO GetSelectedRowDetails(int ID)
        {
            return COMMM.CollegeGetDataById(ID, "OralTestScheduleClgFacade/GetSelectedRowDetails/");
        }
        public DocumentViewClgDTO OralTestScheduleData(DocumentViewClgDTO OralTestScheduleDTO)
        {
            return COMMM.CollegePOSTData(OralTestScheduleDTO, "OralTestScheduleClgFacade/");
        }
        public DocumentViewClgDTO OralTestScheduleDeletesData(int ID)
        {
            return COMMM.CollegeDeleteDataById(ID, "OralTestScheduleClgFacade/OralTestScheduleDeletesData/");
        }

        public DocumentViewClgDTO checkaddparticipants(DocumentViewClgDTO OralTestScheduleDTO)
        {
            return COMMM.CollegePOSTData(OralTestScheduleDTO, "OralTestScheduleClgFacade/checkaddparticipants/");
        }

        public DocumentViewClgDTO scheduleGetreportdetails(DocumentViewClgDTO OralTestScheduleDTO)
        {
            return COMMM.CollegePOSTData(OralTestScheduleDTO, "OralTestScheduleClgFacade/scheduleGetreportdetails/");
        }
        public DocumentViewClgDTO remarksGetreportdetails(DocumentViewClgDTO OralTestScheduleDTO)
        {
            return COMMM.CollegePOSTData(OralTestScheduleDTO, "OralTestScheduleClgFacade/remarksGetreportdetails/");
        }
    }
}
