using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class SchoolStaffperiodmappingDelegate
    {
        CommonDelegate<SchoolStaffperiodmappingDTO, SchoolStaffperiodmappingDTO> _comm = new CommonDelegate<SchoolStaffperiodmappingDTO, SchoolStaffperiodmappingDTO>();

        public SchoolStaffperiodmappingDTO Getdetails(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/Getdetails");
        }

        public SchoolStaffperiodmappingDTO getemployeedetails(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/getemployeedetails");
        }
        public SchoolStaffperiodmappingDTO onchangeclass(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/onchangeclass");
        }
        public SchoolStaffperiodmappingDTO onchangesection(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/onchangesection");
        }
        public SchoolStaffperiodmappingDTO getsearchdetails(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/getsearchdetails");
        }
        public SchoolStaffperiodmappingDTO savedata(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/savedata");
        }

        public SchoolStaffperiodmappingDTO Getdetailstransaction(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/Getdetailstransaction");
        }
        public SchoolStaffperiodmappingDTO getsearchdetailstransaction(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/getsearchdetailstransaction");
        }
        public SchoolStaffperiodmappingDTO savedatatransaction(SchoolStaffperiodmappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodmappingFacade/savedatatransaction");
        }

    }
}
