using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class StudentTCDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<StudentTCDTO, StudentTCDTO> _tcdelegate = new CommonDelegate<StudentTCDTO, StudentTCDTO>();

        public StudentTCDTO LoadInitialData(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getstudenttcdata/");
        }

        public StudentTCDTO getTcDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/gettcDetails/");
        }

        public StudentTCDTO getActiveDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getactiveDetails/");
        }
        public StudentTCDTO getStatusDetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getStatusDetails/");
        }
        public StudentTCDTO savedetails(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/savedetails/");
        }
        public StudentTCDTO chk_tc_dup(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/chk_tc_dup/");
        }
        public StudentTCDTO getstudent_name_list(StudentTCDTO name_list)
        {
            return _tcdelegate.POSTDataADM(name_list, "StudentTCFacade/getstudent_name_list/");
        }
        public StudentTCDTO saveOtherdetails(StudentTCDTO org)
        {
            return _tcdelegate.POSTDataADM(org, "StudentTCFacade/otherdetails/");
        }
        public StudentTCDTO getsearchfilter(StudentTCDTO enqdto)
        {
            return _tcdelegate.POSTDataADM(enqdto, "StudentTCFacade/searchfilter/");
        }

        public StudentTCDTO searchfilter(StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/searchfilter/");
        }

        // TC Cancel     
        public StudentTCDTO GetTCCancelDetails( StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/GetTCCancelDetails/");
        }
        public StudentTCDTO OnChangeAcademicYear( StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/OnChangeAcademicYear/");
        }
        public StudentTCDTO OnStudentNameChange( StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/OnStudentNameChange/");
        }
        public StudentTCDTO SaveTCCancelDetails( StudentTCDTO data)
        {
            return _tcdelegate.POSTDataADM(data, "StudentTCFacade/SaveTCCancelDetails/");
        }

        //

        public StudentTCDTO sourcecntdata(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/sourcecntdata/");
        }
        public StudentTCDTO getallsourcedetails(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/getallsourcedetails/");
        }
        //MotherTongueWise
        public StudentTCDTO languagecntdata(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/languagecntdata/");
        }

        public StudentTCDTO statecntdata(StudentTCDTO tcDto)
        {
            return _tcdelegate.POSTDataADM(tcDto, "StudentTCFacade/statecntdata/");
        }
        

    }
}
