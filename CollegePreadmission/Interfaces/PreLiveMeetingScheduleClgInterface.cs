using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
    public interface PreLiveMeetingScheduleClgInterface
    {
        PreLiveMeetingScheduleClgDTO getempdetails(PreLiveMeetingScheduleClgDTO data);
        PreLiveMeetingScheduleClgDTO ondatechangestudent(PreLiveMeetingScheduleClgDTO data);

        PreLiveMeetingScheduleClgDTO onschedulecahnge(PreLiveMeetingScheduleClgDTO data);

        PreLiveMeetingScheduleClgDTO endmainmeetingstudent(PreLiveMeetingScheduleClgDTO data);
        PreLiveMeetingScheduleClgDTO onstartmeeting(PreLiveMeetingScheduleClgDTO data);
        PreLiveMeetingScheduleClgDTO saveremarks(PreLiveMeetingScheduleClgDTO data);

        PreLiveMeetingScheduleClgDTO getstudentdetails(PreLiveMeetingScheduleClgDTO data);

        PreLiveMeetingScheduleClgDTO joinmeetingStudent(PreLiveMeetingScheduleClgDTO data);
    }
}
