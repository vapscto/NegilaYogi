using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;
using System.Data;
using System.Net.NetworkInformation;
using System.Net;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    //[ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentApplicationController : Controller
    {
        private static FacadeUrl _config;
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        private FacadeUrl fdu = new FacadeUrl();
        private readonly DomainModelMsSqlServerContext _context;
        StudentApplicationDelegate od = new StudentApplicationDelegate();
        private readonly IHostingEnvironment _hostingEnvironment;
        public StudentApplicationController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context, IHostingEnvironment hostingEnvironment)
        {
            _config = settings.Value;
            new StudentApplicationDelegate(_config);
            fdu = _config;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public CountryDTO Get([FromQuery] int adminId)
        {
            adminId = 2;
            return sad.getcountrydata(adminId);
        }

        [Route("getenquirycontroller/{id:int}")]
        public void Getstates(int id)
        {

        }
        [Route("getEditData/{id:int}")]
        public StudentApplicationDTO getStudentEditData(int id)
        {
            StudentApplicationDTO dt = new StudentApplicationDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.ASMAY_Id = ASMAY_Id;


            dt.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));


            dt.pasR_Id = id;

            return sad.getStudentEditData(dt);
        }
        [Route("getprintdata/{id:int}")]
        public StudentApplicationDTO getprintdata(int id)
        {
            StudentApplicationDTO dt = new StudentApplicationDTO();

            string accountname = "";
            string accesskey = "";

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.ASMAY_Id = ASMAY_Id;

            dt.pasR_Id = id;
            List<MasterCategory> classcategoryList = new List<MasterCategory>();
            if (dt.pasR_Id > 0)
            {
                List<StudentApplication> subCaste = new List<StudentApplication>();
                subCaste = _context.StudentApplication.Where(f => f.pasr_id == id).ToList();

                classcategoryList = (from m in _context.Masterclasscategory
                                     from c in _context.mastercategory
                                     from o in _context.AcademicYear
                                     from p in _context.School_M_Class
                                     where (m.ASMAY_Id == o.ASMAY_Id && m.AMC_Id == c.AMC_Id && m.ASMCL_Id == p.ASMCL_Id && o.MI_Id == p.MI_Id && p.ASMCL_Id == subCaste.FirstOrDefault().ASMCL_Id && m.ASMCL_Id == subCaste.FirstOrDefault().ASMCL_Id && o.ASMAY_Id == dt.ASMAY_Id && p.MI_Id == dt.MI_Id && c.AMC_ActiveFlag == 1)
                                     select c
                                        ).ToList();

            }


            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }
            string html = "";
            if (classcategoryList != null && classcategoryList.Count > 0)
            {
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + mid, classcategoryList[0].AMC_PAApplicationName + ".html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + mid, "Admissionform.html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            dt.htmldata = html;

            return sad.getstudentprintdata(dt);
        }
        // POST api/values
        [HttpPost]
        public StudentApplicationDTO studentdetails([FromBody] StudentApplicationDTO stu)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            stu.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            stu.Id = UserId;

            stu.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            // Assign institute id from session 10-11-2016
            //stu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            //string hostName = Dns.GetHostName();
            //var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
            //string myIP1 = ip_list.ToString();

            //stu.PASRSTUL_IPAdd = myIP1;

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            stu.PASRSTUL_MAACAdd = sMacAddress;


            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            stu.PASRSTUL_NetIp = remoteIpAddress.ToString();

            return sad.savestudentdetails(stu);
        }
        [Route("paynow")]
        public StudentApplicationDTO paynow([FromBody] StudentApplicationDTO data)
        {
            StudentApplicationDTO dt = new StudentApplicationDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return sad.paynow(data);
        }
        [Route("getemployees")]
        public StudentApplicationDTO getemployees([FromBody] StudentApplicationDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return sad.getemployees(data);
        }
        [Route("getdashboardpage")]
        public StudentApplicationDTO getdashboardpage([FromBody] StudentApplicationDTO data)
        {
            if (data.MI_Id == 0)
            {
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            else
            {
                data.MI_Id = data.MI_Id;

            }
            return sad.getdashboardpage(data);
        }
        [HttpPost]
        [Route("SearchData")]
        public StudentApplicationDTO searchDatas([FromBody] StudentApplicationDTO stu)
        {
            return sad.searchData(stu);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public void Delete(int id)
        {
            sad.deleterec(id);
        }
        [HttpPost]
        // Added on 19-9-2016
        [Route("getdpforreg")]
        public CountryDTO getDpData([FromBody] CountryDTO ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            ctry.username = Convert.ToString(HttpContext.Session.GetString("UserName"));


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //return sad.getIndependentDropDowns(ctry);
            return sad.getIndependentDropDowns(ctry);
        }
        [Route("ActivateDactivate")]
        public CountryDTO ActivateDactivate([FromBody] CountryDTO ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //return sad.getIndependentDropDowns(ctry);
            return sad.ActivateDactivate(ctry);
        }
        [Route("Getcountofstudents")]
        public CountryDTO Getcountofstudents([FromBody] CountryDTO ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //return sad.getIndependentDropDowns(ctry);
            return sad.Getcountofstudents(ctry);
        }
        [Route("Dashboarddetails")]
        public CountryDTO Dashboarddetails([FromBody] CountryDTO ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            String hostName = Dns.GetHostName();
            var uploads = _hostingEnvironment.WebRootPath;
            uploads = _hostingEnvironment.WebRootFileProvider.ToString();

            //return sad.getIndependentDropDowns(ctry);
            return sad.Dashboarddetails(ctry);
        }
        [Route("classchangemaxminage")]
        public StudentApplicationDTO getmaxminage([FromBody] StudentApplicationDTO maxmin)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.MI_Id = mid;

            maxmin.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            maxmin.Id = UserId;

            return sad.getmaxminage(maxmin);
        }
        [Route("classoverunderage")]
        public StudentApplicationDTO classoverunderage([FromBody] StudentApplicationDTO maxmin)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.MI_Id = mid;

            maxmin.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));


            return sad.classoverunderage(maxmin);
        }
        [Route("GetSubjectsofinstitute")]
        public StudentApplicationDTO GetSubjectsofinstitute([FromBody] StudentApplicationDTO maxmin)
        {
            maxmin.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return sad.GetSubjectsofinstitute(maxmin);
        }
        [Route("getstreams")]
        public StudentApplicationDTO getstreams([FromBody] StudentApplicationDTO maxmin)
        {
            maxmin.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            maxmin.Id = UserId;
            maxmin.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getstreams(maxmin);
        }
        [Route("getdpcity/{adminId:int}")]
        public CityDTO getCity(int adminId)
        {
            return sad.getCityByCountry(adminId);
        }
        [Route("getroutes/{id:int}")]
        public StateDTO getroutes(int id)
        {
            return sad.getroutes(id);
        }
        [Route("getrouteslocation/{id:int}")]
        public StateDTO getrouteslocation(int id)
        {
            return sad.getrouteslocation(id);
        }
        [Route("getdpstate/{id:int}")]
        public StateDTO getstate(int id)
        {
            return sad.getStateByCountry(id);
        }
        [Route("getdpdistrict/{id:int}")]
        public DistrictDTO getdistrict(int id)
        {
            return sad.getDistrictByState(id);
        }
        [Route("getdprospectusdetails/{id:int}")]
        public StateDTO getdprospectusdetails(int id)
        {
            return sad.getdprospectusdetails(id);
        }
        [Route("getdpstatesubcatse/{id:int}")]
        public StateDTO getdpstatesubcatse(int id)
        {
            return sad.getdpstatesubcatse(id);
        }
        //[Route("getapplicationhtml/{id:int}")]
        //public StateDTO getapplicationhtml(int id)
        //{
        //    return sad.getapplicationhtml(id);
        //}
        [Route("getapplicationhtml")]
        public StudentApplicationDTO getapplicationhtml([FromBody] StudentApplicationDTO maxmin)
        {
            maxmin.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            maxmin.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return sad.getapplicationhtml(maxmin);
        }
        [Route("getdpstatesubcatsefather/{id:int}")]
        public StateDTO getdpstatesubcatsefather(int id)
        {
            return sad.getdpstatesubcatsefather(id);
        }
        [Route("getdpstatesubcatsemother/{id:int}")]
        public StateDTO getdpstatesubcatsemother(int id)
        {
            return sad.getdpstatesubcatsemother(id);
        }
        [Route("getdpcities/{id:int}")]
        public CityDTO getCities(int id)
        {
            return sad.getCityByState(id);
        }
        //[Route("getdpArea/{id:int}")]
        //public CountryDTO getDpArea(int id)
        //{
        //    return sad.getAreaByCity(id);
        //}
        // Added on 19-9-2016
        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            dtoapp.MI_Id = Convert.ToInt64(response.udf3);
            dtoapp = getdashboardpage(dtoapp);
            if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "hutchings";
            }

            try
            {
                dto = od.getpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }
        [Route("Razorpaypaymentresponse/")]
        public ActionResult Razorpaypaymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            string querystring = "";
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            dtoapp.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string payid = response.razorpay_payment_id;

            dtoapp = getdashboardpage(dtoapp);
            if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "hutchings";
            }

            try
            {
                dto = od.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            return Redirect(querystring);
        }
        [Route("paymentresponsepaytm/")]
        public ActionResult paymentresponse(PaymentDetails.PAYTM response)
        {

            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();

            string[] tokens = response.MERC_UNQ_REF.Split('_');

            dtoapp.MI_Id = Convert.ToInt32(tokens[6]);

            dtoapp.MI_Id = Convert.ToInt64(dtoapp.MI_Id);
            dtoapp = getdashboardpage(dtoapp);
            if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "hutchings";
            }

            PaymentDetails.PAYTM dto = new PaymentDetails.PAYTM();

            string querystring = "";
            try
            {
                dto = od.getpaymentresponsepaytm(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/ 12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        //easebuzz
        [Route("paymentresponseeasybuzz/")]
        public ActionResult paymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();

           
             if (dtoapp.dashboardpage != null)
            {
                sub = dtoapp.dashboardpage;
            }
            else
            {
                sub = "BCEHS";
            }
            PaymentDetails.easybuzz dto = new PaymentDetails.easybuzz();
            string querystring = "";

            try
            {
                dto = od.getpaymentresponseeasybuzz(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/" + sub + "/12?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

        //student health certificate
        [Route("savehealthcertificatedetail")]
        public StudentHelthcertificateDTO savehealthcertificatedetail([FromBody] StudentHelthcertificateDTO maxmin)
        {
            maxmin.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return sad.savehealthcertificatedetail(maxmin);
        }
        [HttpPost]
        [Route("getstudata")]
        public StudentHelthcertificateDTO getstudata([FromBody] StudentHelthcertificateDTO ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return sad.getstudata(ctry);
        }
        [Route("getEdithelthData/{id:int}")]
        public StudentHelthcertificateDTO getEdithelthData(int id)
        {
            StudentHelthcertificateDTO dt = new StudentHelthcertificateDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;

            dt.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.ASMAY_Id = ASMAY_Id;

            dt.PASHD_Id = id;

            return sad.getEdithelthData(dt);
        }
        [HttpDelete]
        [Route("deletehelthdetails/{id:int}")]
        public StudentHelthcertificateDTO deletehelthdetails(int id)
        {
            StudentHelthcertificateDTO dt = new StudentHelthcertificateDTO();
            dt.PASHD_Id = Convert.ToInt64(id);
            return sad.deletehelthdetails(dt);
        }
        [Route("printgethelthData/{id:int}")]
        public StudentHelthcertificateDTO printgethelthData(int id)
        {
            StudentHelthcertificateDTO dt = new StudentHelthcertificateDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;
            dt.PASHD_Id = id;

            return sad.printgethelthData(dt);
        }
        [Route("fill_prospectus")]
        public StudentApplicationDTO fill_prospectus([FromBody]StudentApplicationDTO data)
        {
            //int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.MI_Id = mid;
            return sad.fill_prospectus(data);
        }


        //class change

        [Route("classchange")]
        public StudentApplicationDTO classchange([FromBody] StudentApplicationDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return sad.classchange(data);
        }
    }
}
