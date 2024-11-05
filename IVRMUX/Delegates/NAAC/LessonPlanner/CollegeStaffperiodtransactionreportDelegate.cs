using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner
{
    public class CollegeStaffperiodtransactionreportDelegate
    {
        CommonDelegate<CollegeStaffperiodtransactionreportDTO, CollegeStaffperiodtransactionreportDTO> _comm = new CommonDelegate<CollegeStaffperiodtransactionreportDTO, CollegeStaffperiodtransactionreportDTO>();


        public CollegeStaffperiodtransactionreportDTO Getdetailstransaction(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/Getdetailstransaction");
        }
        public CollegeStaffperiodtransactionreportDTO onselectAcdYear(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/onselectAcdYear");
        }
        public CollegeStaffperiodtransactionreportDTO onselectCourse(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/onselectCourse");
        }
        public CollegeStaffperiodtransactionreportDTO onselectBranch(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/onselectBranch");
        }
        public CollegeStaffperiodtransactionreportDTO getsection(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/getsection");
        }
        public CollegeStaffperiodtransactionreportDTO onchangesection(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/onchangesection");
        }
        public CollegeStaffperiodtransactionreportDTO getreport(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/getreport");
        }
        public CollegeStaffperiodtransactionreportDTO getdevationreport(CollegeStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "CollegeStaffperiodtransactionreportFacade/getdevationreport");
        }
        
    }
}
