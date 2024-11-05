using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.College.Preadmission;
using DomainModel.Model.com.vapstech.College.Admission;
using CommonLibrary;
using Microsoft.Extensions.Options;
using corewebapi18072016;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class ApplicationFormController : Controller
    {
        ApplicationFormDelegate delegates = new ApplicationFormDelegate();
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        private readonly DomainModelMsSqlServerContext _context;
        public ApplicationFormController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails([FromBody] CollegePreadmissionstudnetDto dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse")]
        public CollegePreadmissionstudnetDto getCourse([FromBody] CollegePreadmissionstudnetDto data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getCourse(data);
        }

        [Route("Dashboarddetails")]
        public CollegePreadmissionstudnetDto Dashboarddetails([FromBody] CollegePreadmissionstudnetDto ctry)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            ctry.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ctry.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ctry.ASMAY_Id = ASMAY_Id;

            ctry.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            //return sad.getIndependentDropDowns(ctry);
            return delegates.Dashboarddetails(ctry);
        }

        [Route("paynow")]
        public CollegePreadmissionstudnetDto paynow([FromBody] CollegePreadmissionstudnetDto data)
        {
            CollegePreadmissionstudnetDto dt = new CollegePreadmissionstudnetDto();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;



            return delegates.paynow(data);
        }
        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO schoolimp = new StudentApplicationDTO();
            schoolimp.MI_Id = Convert.ToInt64(response.udf3);
            string querystring = "";
            var sub = "";
            StudentApplicationDTO dtoapp = new StudentApplicationDTO();
            dtoapp.MI_Id = Convert.ToInt64(response.udf3);
            dtoapp = getdashboardpage(schoolimp);
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
                dto = delegates.getpaymentresponse(response);
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
                dto = delegates.getpaymentresponsepaytm(response);
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


        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody]CollegePreadmissionstudnetDto dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getBranch(dt);
        }
        [Route("getSemester")]
        public CollegePreadmissionstudnetDto getSemester([FromBody]CollegePreadmissionstudnetDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getSemester(dto);
        }
        [Route("getCaste")]
        public CollegePreadmissionstudnetDto getCaste([FromBody] CollegePreadmissionstudnetDto caste)
        {
            caste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getCaste(caste);
        }
        //[Route("getQuotaCategory/{quotaId:int}")]
        //public AdmMasterCollegeStudentDTO getQuotaCategory(int quotaId)
        //{
        //    AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.ACQ_Id = quotaId;
        //    return delegates.getQuotaCategory(dto);
        //}
        [Route("saveStudentDetails")]
        public CollegePreadmissionstudnetDto saveStudentDetails([FromBody]CollegePreadmissionstudnetDto dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delegates.saveStudentDetails(dto);
        }
        [Route("Edit/{Id:int}")]
        public CollegePreadmissionstudnetDto Edit(int Id)
        {
            CollegePreadmissionstudnetDto edit = new CollegePreadmissionstudnetDto();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            edit.PACA_Id = Id;
            return delegates.Edit(edit);
        }
        [Route("getprintdata/{Id:int}")]
        public CollegePreadmissionstudnetDto getprintdata(int Id)
        {
            CollegePreadmissionstudnetDto edit = new CollegePreadmissionstudnetDto();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.PACA_Id = Id;
            return delegates.getprintdata(edit);
        }
        //[Route("checkDuplicate")]
        //public AdmMasterCollegeStudentDTO checkDuplicate([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.checkDuplicate(data);
        //}
        [Route("getdpstate/{countryId:int}")]
        public CollegePreadmissionstudnetDto getdpstate(int countryId)
        {
            CollegePreadmissionstudnetDto st = new CollegePreadmissionstudnetDto();
            st.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            st.IVRMMC_Id = countryId;
            return delegates.getdpstate(st);
        }

        [Route("saveAddress")]
        public CollegePreadmissionstudnetDto saveAddress([FromBody]CollegePreadmissionstudnetDto add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveAddress(add);
        }
        [Route("saveParentsDetails")]
        public CollegePreadmissionstudnetDto saveParentsDetails([FromBody]CollegePreadmissionstudnetDto parentsData)
        {
            parentsData.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveParentsDetails(parentsData);
        }
        [Route("StateByCountryName")]
        public CollegePreadmissionstudnetDto StateByCountryName([FromBody]CollegePreadmissionstudnetDto country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public CollegePreadmissionstudnetDto saveOthersDetails([FromBody]CollegePreadmissionstudnetDto others)
        {
            others.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public CollegePreadmissionstudnetDto saveDocuments([FromBody]CollegePreadmissionstudnetDto documents)
        {
            documents.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveDocuments(documents);
        }

        //[Route("SearchByColumn")]
        //public AdmMasterCollegeStudentDTO SearchByColumn([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.SearchByColumn(data);
        //}
        //[Route("DeleteEntry")]
        //public AdmMasterCollegeStudentDTO DeleteEntry([FromBody]AdmMasterCollegeStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delegates.DeleteEntry(data);
        //}


        //Document view view application

        [Route("Clgapplicationdata/{id:int}")]
        public CollegePreadmissionstudnetDto Clgapplicationdata(int id)
        {
            CollegePreadmissionstudnetDto dt = new CollegePreadmissionstudnetDto();

            string accountname = "";
            string accesskey = "";

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.ASMAY_Id = ASMAY_Id;

            dt.PACA_Id = id;
            List<ClgMasterCategoryDMO> classcategoryList = new List<ClgMasterCategoryDMO>();
            if (dt.PACA_Id > 0)
            {
                List<PA_College_Application> subCaste = new List<PA_College_Application>();
                subCaste = _context.PA_College_Application.Where(f => f.PACA_Id == id).ToList();

                classcategoryList = (from m in _context.ClgMasterCategoryDMO
                                     from c in _context.ClgMasterCourseCategoryMapDMO
                                     from p in _context.MasterCourseDMO
                                     where (m.AMCOC_Id == c.AMCOC_Id && c.AMCO_Id == p.AMCO_Id && p.AMCO_Id == subCaste.FirstOrDefault().AMCO_Id && c.AMCO_Id == subCaste.FirstOrDefault().AMCO_Id && p.MI_Id == dt.MI_Id && c.AMCOCM_ActiveFlg == true)
                                     select m
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
            //if (classcategoryList.Count > 0)
            //{
            //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + mid, classcategoryList[0].AMCOC_Name + ".html", 0);
            //}
            //else
            //{
            //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + mid, "Admissionform.html", 0);
            //}

            html = obj.getHtmlContentFromAzure(accountname, accesskey, "admissionform/" + mid, "Admissionform.html", 0);


            dt.htmldata = html;

            return delegates.getprintdata(dt);
        }

        //master competitive exam
        [Route("compExamName")]
        public CollegePreadmissionstudnetDto compExamName([FromBody]CollegePreadmissionstudnetDto country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.compExamName(country);
        }

        //Razor pay
        [Route("Razorpaypaymentresponse")]
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
                dto = delegates.razorgetpaymentresponse(response);
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



        [Route("Clgapplicationstudocs")]
        public CollegePreadmissionstudnetDto Clgapplicationstudocs([FromBody] CollegePreadmissionstudnetDto data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delegates.Clgapplicationstudocs(data);
        }

    }
}
