using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExamMasterDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<exammasterDTO, exammasterDTO> COMMM = new CommonDelegate<exammasterDTO, exammasterDTO>();

        public exammasterDTO Getdetails(exammasterDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterFacade/Getdetails/");
        }
        public exammasterDTO savedetails(exammasterDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterFacade/savedetails/");
        }

        public exammasterDTO editdetails(int data)
        {
            return COMMM.GETexam(data, "ClgExamMasterFacade/editdetails/");
        }

        public exammasterDTO validateordernumber(exammasterDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterFacade/validateordernumber/");
        }

        public exammasterDTO deactivate(exammasterDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMasterFacade/deactivate/");
        }
    }
}
