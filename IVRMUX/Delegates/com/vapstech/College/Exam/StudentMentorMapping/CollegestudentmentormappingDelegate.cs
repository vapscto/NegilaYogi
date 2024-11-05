using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.StudentMentorMapping
{
    public class CollegestudentmentormappingDelegate
    {
        CommonDelegate<CollegestudentmentormappingDTO, CollegestudentmentormappingDTO> _com = new CommonDelegate<CollegestudentmentormappingDTO, CollegestudentmentormappingDTO>();

        public CollegestudentmentormappingDTO Getdetails(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/Getdetails");
        }
        public CollegestudentmentormappingDTO onchangeyear(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/onchangeyear");
        }
        public CollegestudentmentormappingDTO getbranch(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getbranch");
        }
        public CollegestudentmentormappingDTO getsemester(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getsemester");
        }
        public CollegestudentmentormappingDTO getsection(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getsection");
        }
        public CollegestudentmentormappingDTO getemployee(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getemployee");
        }
        public CollegestudentmentormappingDTO getstudentdata(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getstudentdata");
        }
        public CollegestudentmentormappingDTO savedata(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/savedata");
        }
        public CollegestudentmentormappingDTO viewrecordspopup(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/viewrecordspopup");
        }
        public CollegestudentmentormappingDTO Deletedata(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/Deletedata");
        }

        //Report
        public CollegestudentmentormappingDTO Getreportdetails(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/Getreportdetails");
        }
        public CollegestudentmentormappingDTO getreport(CollegestudentmentormappingDTO data)
        {
            return _com.POSTcolExam(data, "CollegestudentmentormappingFacade/getreport");
        }

    }
}
