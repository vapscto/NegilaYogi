using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeLeaveApplyDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO> COMMM = new CommonDelegate<EmployeeDashboardDTO, EmployeeDashboardDTO>();
        CommonDelegate<Adm_TC_Approval_DTO, Adm_TC_Approval_DTO> COMTC = new CommonDelegate<Adm_TC_Approval_DTO, Adm_TC_Approval_DTO>();

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:57234/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/EmployeeLeaveApplyFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }
        public EmployeeDashboardDTO getonlineLeave(EmployeeDashboardDTO student)
        {
            EmployeeDashboardDTO ads = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(student);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/EmployeeLeaveApplyFacade/getonlineLeave", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    ads = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ads;
        }
        //save the data
        public EmployeeDashboardDTO savedetail(EmployeeDashboardDTO report)
        {
            return COMMM.POSTDataOnlineLeave(report, "EmployeeLeaveApplyFacade/save/");
        }

        //======================== TC Class teacher approval
        public Adm_TC_Approval_DTO getdata_CTA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getdata_CTA/");
        }
        public Adm_TC_Approval_DTO SaveEdit_CTA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/SaveEdit_CTA/");
        }
        public Adm_TC_Approval_DTO details_CTA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/details_CTA/");
        }
         public Adm_TC_Approval_DTO deactivate_CTA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/deactivate_CTA/");
        }
        public Adm_TC_Approval_DTO getstudetails_CTA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getstudetails_CTA/");
        }
         //======================== TC Library approval
        public Adm_TC_Approval_DTO getdata_LIB(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getdata_LIB/");
        }
        public Adm_TC_Approval_DTO SaveEdit_LIB(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/SaveEdit_LIB/");
        }
        public Adm_TC_Approval_DTO details_LIB(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/details_LIB/");
        }
         public Adm_TC_Approval_DTO deactivate_LIB(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/deactivate_LIB/");
        }
        public Adm_TC_Approval_DTO getstudetails_LIB(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getstudetails_LIB/");
        }

         //======================== TC FEE approval
        public Adm_TC_Approval_DTO getdata_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getdata_FEE/");
        }
        public Adm_TC_Approval_DTO SaveEdit_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/SaveEdit_FEE/");
        }
        public Adm_TC_Approval_DTO details_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/details_FEE/");
        }
         public Adm_TC_Approval_DTO deactivate_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/deactivate_FEE/");
        }
            public Adm_TC_Approval_DTO getstudetails_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getstudetails_FEE/");
        }
            public Adm_TC_Approval_DTO feeheaddetails_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/feeheaddetails_FEE/");
        }
         public Adm_TC_Approval_DTO feenot_approval_FEE(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/feenot_approval_FEE/");
        }

        //======================== FDA Class teacher approval
        public Adm_TC_Approval_DTO getdata_PDA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getdata_PDA/");
        }
        public Adm_TC_Approval_DTO SaveEdit_PDA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/SaveEdit_PDA/");
        }
        public Adm_TC_Approval_DTO details_PDA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/details_PDA/");
        }
        public Adm_TC_Approval_DTO deactivate_PDA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/deactivate_PDA/");
        }
        public Adm_TC_Approval_DTO getstudetails_PDA(Adm_TC_Approval_DTO dto)
        {
            return COMTC.POSTDataOnlineTC(dto, "EmployeeLeaveApplyFacade/getstudetails_PDA/");
        }

    }
}
