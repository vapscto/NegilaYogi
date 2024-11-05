using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
  public interface OralTestScheduleClgInterface
    {
        DocumentViewClgDTO GetOralTestScheduleData(DocumentViewClgDTO classwisestudent);
        DocumentViewClgDTO coursewisestudent(DocumentViewClgDTO classwisestudent);

        DocumentViewClgDTO GetSelectedRowDetails(int ID);

        Task<DocumentViewClgDTO> OralTestScheduleData(DocumentViewClgDTO mas);

        DocumentViewClgDTO OralTestScheduleDeletesData(int ID);

        DocumentViewClgDTO checkaddparticipants(DocumentViewClgDTO data);

        Task<DocumentViewClgDTO> scheduleGetreportdetails(DocumentViewClgDTO data);
        Task<DocumentViewClgDTO> remarksGetreportdetails(DocumentViewClgDTO data);
    }
}
