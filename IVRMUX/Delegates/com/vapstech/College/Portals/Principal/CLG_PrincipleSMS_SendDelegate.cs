using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Portals.Principal
{
    public class CLG_PrincipleSMS_SendDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SendSMSDTO, SendSMSDTO> COMMM = new CommonDelegate<SendSMSDTO, SendSMSDTO>();
        
        public SendSMSDTO Getdetails(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/Getdetails/");
        }
        public SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }
        public SendSMSDTO Getdepartment(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/Getdepartment/");
        }
        public SendSMSDTO get_designation(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/get_designation/");
        }
        public SendSMSDTO get_employee(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/get_employee/");
        }
        public SendSMSDTO savedetail(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/savedetail/");
        }
        public SendSMSDTO getCourse(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/getCourse/");
        }
        public SendSMSDTO getBranch(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/getBranch/");
        }
        public SendSMSDTO getSemister(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/getSemister/");
        }
        public SendSMSDTO GetStudentDetails(SendSMSDTO data)
        {
            return COMMM.clgadmissionbypost(data, "CLG_PrincipleSMS_SendFacade/GetStudentDetails/");            
        }
    }
}
