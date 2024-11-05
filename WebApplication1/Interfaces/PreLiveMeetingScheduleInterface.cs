using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface PreLiveMeetingScheduleInterface
    {
         LiveMeetingScheduleDTO getdatastuacadgrp(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO getempdetails(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO getclass(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO onstartmeeting(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO ondatechange(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO onschedulecahnge(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO ondatechangestudent(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO endmainmeeting(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO saveremarks(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO joinmeeting(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO getsection(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO savedata(LiveMeetingScheduleDTO data);
         LiveMeetingScheduleDTO editdata(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getstudentdetails(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO endmainmeetingstudent(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO joinmeetingStudent(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO deactive(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getsubject(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO checkduplicate(LiveMeetingScheduleDTO data);

        //REPORT

        LiveMeetingScheduleDTO getschrptdetailsprofile(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getschrptdetails(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getschedulereport(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getstaffprofilereport(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getstudentprofiledata(LiveMeetingScheduleDTO data);
        LiveMeetingScheduleDTO getstudentprofilereport(LiveMeetingScheduleDTO data);

    }
}
