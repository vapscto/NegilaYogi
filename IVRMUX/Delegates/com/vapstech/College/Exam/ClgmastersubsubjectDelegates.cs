using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgmastersubsubjectDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<mastersubsubjectDTO, mastersubsubjectDTO> COMMM = new CommonDelegate<mastersubsubjectDTO, mastersubsubjectDTO>();

        public mastersubsubjectDTO GetmastersubsubjectData(mastersubsubjectDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmastersubsubjectFacade/Getdetails/");
        }
        public mastersubsubjectDTO savedetails(mastersubsubjectDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmastersubsubjectFacade/savedetails/");
        }

        public mastersubsubjectDTO editdeatils(int data)
        {
            return COMMM.GETexam(data, "ClgmastersubsubjectFacade/editdeatils/");
        }

        public mastersubsubjectDTO validateordernumber(mastersubsubjectDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmastersubsubjectFacade/validateordernumber/");
        }

        public mastersubsubjectDTO deactivate(mastersubsubjectDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmastersubsubjectFacade/deactivate/");
        }
    }
}
