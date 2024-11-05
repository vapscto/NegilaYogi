using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Interface
{
    public interface SchoolStaffperiodmappingInterface
    {
        SchoolStaffperiodmappingDTO Getdetails(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO getemployeedetails(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO onchangeclass(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO onchangesection(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO getsearchdetails(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO savedata(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO Getdetailstransaction(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO getsearchdetailstransaction(SchoolStaffperiodmappingDTO data);
        SchoolStaffperiodmappingDTO savedatatransaction(SchoolStaffperiodmappingDTO data);

    }
}
