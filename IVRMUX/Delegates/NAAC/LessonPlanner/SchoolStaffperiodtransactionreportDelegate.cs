using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class SchoolStaffperiodtransactionreportDelegate
    {
        CommonDelegate<SchoolStaffperiodtransactionreportDTO, SchoolStaffperiodtransactionreportDTO> _comm = new CommonDelegate<SchoolStaffperiodtransactionreportDTO, SchoolStaffperiodtransactionreportDTO>();

        public SchoolStaffperiodtransactionreportDTO Getdetailstransaction(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/Getdetailstransaction");
        }
        public SchoolStaffperiodtransactionreportDTO onselectAcdYear(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/onselectAcdYear");
        }
        public SchoolStaffperiodtransactionreportDTO onselectclass(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/onselectclass");
        }       
        public SchoolStaffperiodtransactionreportDTO onchangesection(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/onchangesection");
        }
        public SchoolStaffperiodtransactionreportDTO getreport(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/getreport");
        }
        public SchoolStaffperiodtransactionreportDTO getdevationreport(SchoolStaffperiodtransactionreportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SchoolStaffperiodtransactionreportFacade/getdevationreport");
        }
    }
}
