using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgMastersubexamDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<mastersubexamDTO, mastersubexamDTO> COMMM = new CommonDelegate<mastersubexamDTO, mastersubexamDTO>();

        public mastersubexamDTO Getdetails(mastersubexamDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubExamMasterFacade/Getdetails/");
        }
        public mastersubexamDTO savedetails(mastersubexamDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubExamMasterFacade/savedetails/");
        }

        public mastersubexamDTO editdeatils(int data)
        {
            return COMMM.GETexam(data, "ClgSubExamMasterFacade/editdetails/");
        }

        public mastersubexamDTO validateordernumber(mastersubexamDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubExamMasterFacade/validateordernumber/");
        }

        public mastersubexamDTO deactivate(mastersubexamDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubExamMasterFacade/deactivate/");
        }

    }
}
