using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgSubjectMasterDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterSubjectAllMDTO, MasterSubjectAllMDTO> COMMM = new CommonDelegate<MasterSubjectAllMDTO, MasterSubjectAllMDTO>();

        public MasterSubjectAllMDTO savedetails(MasterSubjectAllMDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubjectMasterFacade/savedetail/");
        }

        public MasterSubjectAllMDTO validateordernumber(MasterSubjectAllMDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubjectMasterFacade/validateordernumber/");
        }

        public MasterSubjectAllMDTO getdetails(int data)
        {
            return COMMM.GETexam(data, "ClgSubjectMasterFacade/getalldetails/");
        }

        public MasterSubjectAllMDTO GetmasterSubdetails(MasterSubjectAllMDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubjectMasterFacade/getdetails/");
        }

        public MasterSubjectAllMDTO DeleteMasterRecord(int data)
        {
            return COMMM.GETexam(data, "ClgSubjectMasterFacade/Deletedetails/");
        }

        public MasterSubjectAllMDTO EditDetails(int data)
        {
            return COMMM.GETexam(data, "ClgSubjectMasterFacade/Editdetails/");
        }

        public MasterSubjectAllMDTO savedata2(MasterSubjectAllMDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgSubjectMasterFacade/savedata2/");
        }
        

    }
}
