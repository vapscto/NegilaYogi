using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class Logindelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CommonDTO, CommonDTO> COMMM = new CommonDelegate<CommonDTO, CommonDTO>();
        CommonDelegate<LoginDTO, LoginDTO> COMM = new CommonDelegate<LoginDTO, LoginDTO>();
        CommonDelegate<CommonDTO, LoginDTO> Config = new CommonDelegate<CommonDTO, LoginDTO>();
        CommonDelegate<LoginDTO, CommonDTO> COM = new CommonDelegate<LoginDTO, CommonDTO>();
        CommonDelegate<LoginDTO, regis> CO = new CommonDelegate<LoginDTO, regis>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        CommonDelegate<FileDescriptionDTO, FileDescriptionDTO> alureg = new CommonDelegate<FileDescriptionDTO, FileDescriptionDTO>();

        public CommonDTO setSessionDataForVirtualSchool(int id)
        {
            return COMMM.GetDataById(id, "LoginFacade/setVirtualSession/");
        }
        public CommonDTO setSessionDataForVirtualSchoolsubdomain(CommonDTO subdomainname)
        {
            return COMMM.POSTData(subdomainname, "LoginFacade/setVirtualSessionsubdomain/");
        }            

        public LoginDTO VerifyUserName(LoginDTO UserName)
        {
            return COMM.POSTData(UserName, "LoginFacade/VerifyUserName/");
        }
        public LoginDTO getRoleData(CommonDTO cmn)
        {
            return COM.POSTDataa(cmn, "LoginFacade/getRoleData");
        }

        // Changed return type from string on 5-11-2016
        public LoginDTO getregdata(regis reg)
        {
            return CO.POSTDataa(reg, "LoginFacade/");
        }
        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTData(response, "LoginFacade/getpaymentresponse/");

        }
        public LoginDTO getMIdata(regis reg)
        {
            return CO.POSTDataa(reg, "LoginFacade/getMIdata/");
        }
        public LoginDTO getMIdataMaster(regis reg)
        {
            return CO.POSTDataa(reg, "LoginFacade/getMIdataMaster/");
        }
        public LoginDTO getsiblinglist(regis reg)
        {
            return CO.POSTDataa(reg, "LoginFacade/getsiblinglist/");
        }
        // forgot password added on 10-11-2016
        public LoginDTO forgotPassword(LoginDTO email)
        {
            return COMM.POSTData(email, "LoginFacade/forgotpassword");
        }

        public LoginDTO getconfig(CommonDTO email)
        {
            return COM.POSTDataa(email, "LoginFacade/getconfig");
        }

        public LoginDTO getconfigtrust(CommonDTO email)
        {
            return COM.POSTDataa(email, "LoginFacade/getconfigTrust");
        }
        // forgot password added on 10-11-2016

        public CommonDTO setSessionDataForSubDomainName(CommonDTO dto)
        {
            return COMMM.POSTData(dto, "LoginFacade/setSessionDataForSubDomainName/");
        }

        //Get Data by MI_ID & RoleType_Name

        // forgot password added on 10-11-2016
        public LoginDTO getInstituteDataByRoleTypeName(int id)
        {
            return COMM.GetDataById(id, "LoginFacade/getInstituteDataByRoleTypeName/");
        }

        public LoginDTO getrolpagewisedata(LoginDTO dto)
        {
            return COMM.POSTData(dto, "LoginFacade/getrolewisepage/");
        }
        public FileDescriptionDTO getgateway(FileDescriptionDTO dto)
        {
            return alureg.POSTData(dto, "LoginFacade/getgateway/");
        }
        public FileDescriptionDTO GetpaymentDetails(FileDescriptionDTO dto)
        {
            return alureg.POSTData(dto, "LoginFacade/GetpaymentDetails/");
        }
        public FileDescriptionDTO paymentsave(FileDescriptionDTO dto)
        {
            return alureg.POSTData(dto, "LoginFacade/paymentsave/");
        }

        public LoginDTO SaveRemaindersRemarks(LoginDTO dto)
        {
            return COMM.POSTData(dto, "LoginFacade/SaveRemaindersRemarks/");
        }
    }
}
