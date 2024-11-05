using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using DomainModel.Model.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeStudentAdmissionController : Controller
    {
        CollegeStudentAdmissionDelegate delegates = new CollegeStudentAdmissionDelegate();

        private readonly DomainModelMsSqlServerContext _context;

        public CollegeStudentAdmissionController(DomainModelMsSqlServerContext context)
        {
            _context = context;
        }
        [Route("Getdetails")]
        public AdmMasterCollegeStudentDTO Getdetails()
        {
            AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse/{YearId:int}")]
        public AdmMasterCollegeStudentDTO getCourse(int YearId)
        {
            AdmMasterCollegeStudentDTO obj = new AdmMasterCollegeStudentDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = YearId;
            return delegates.getCourse(obj);
        }
        [Route("getBranch")]
        public AdmMasterCollegeStudentDTO getBranch([FromBody]AdmMasterCollegeStudentDTO dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getBranch(dt);
        }
        [Route("getSemester")]
        public AdmMasterCollegeStudentDTO getSemester([FromBody]AdmMasterCollegeStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getSemester(dto);
        }
        [Route("getCaste")]
        public AdmMasterCollegeStudentDTO getCaste([FromBody] AdmMasterCollegeStudentDTO caste)
        {
            caste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getCaste(caste);
        }
        [Route("getQuotaCategory/{quotaId:int}")]
        public AdmMasterCollegeStudentDTO getQuotaCategory(int quotaId)
        {
            AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ACQ_Id = quotaId;
            return delegates.getQuotaCategory(dto);
        }
        [HttpPost]
        [Route("saveStudentDetails")]
        public save_firsttab_details saveStudentDetails([FromBody]save_firsttab_details dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));      
            return delegates.saveStudentDetails(dto);
        }
        [Route("Edit/{Id:int}")]
        public AdmMasterCollegeStudentDTO Edit(int Id)
        {
            AdmMasterCollegeStudentDTO edit = new AdmMasterCollegeStudentDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.AMCST_Id = Id;
            edit.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegates.Edit(edit);
        }
        [Route("checkDuplicate")]
        public AdmMasterCollegeStudentDTO checkDuplicate([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.checkDuplicate(data);
        }
        [Route("getdpstate/{countryId:int}")]
        public AdmMasterCollegeStudentDTO getdpstate(int countryId)
        {
            AdmMasterCollegeStudentDTO st = new AdmMasterCollegeStudentDTO();
            st.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            st.IVRMMC_Id = countryId;
            return delegates.getdpstate(st);
        }
        [Route("saveAddress")]
        public AdmMasterCollegeStudentDTO saveAddress([FromBody]AdmMasterCollegeStudentDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveAddress(add);
        }
        [Route("saveParentsDetails")]
        public AdmMasterCollegeStudentDTO saveParentsDetails([FromBody]AdmMasterCollegeStudentDTO parentsData)
        {
            parentsData.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveParentsDetails(parentsData);
        }
        [Route("StateByCountryName")]
        public AdmMasterCollegeStudentDTO StateByCountryName([FromBody]AdmMasterCollegeStudentDTO country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public AdmMasterCollegeStudentDTO saveOthersDetails([FromBody]AdmMasterCollegeStudentDTO others)
        {
            others.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public AdmMasterCollegeStudentDTO saveDocuments([FromBody]AdmMasterCollegeStudentDTO documents)
        {
            documents.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveDocuments(documents);
        }

        [Route("SearchByColumn")]
        public AdmMasterCollegeStudentDTO SearchByColumn([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.SearchByColumn(data);
        }
        [Route("DeleteEntry")]
        public AdmMasterCollegeStudentDTO DeleteEntry([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.DeleteEntry(data);
        }

        [Route("ViewStudentProfile")]
        public AdmMasterCollegeStudentDTO ViewStudentProfile([FromBody] AdmMasterCollegeStudentDTO data)
        {
           // data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegates.ViewStudentProfile(data);
        }

        //master competitive exam
        [Route("compExamName")]
        public AdmMasterCollegeStudentDTO compExamName([FromBody]AdmMasterCollegeStudentDTO country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.compExamName(country);
        }

        //document view
        [Route("Clgapplicationdata/{id:int}")]
        public AdmMasterCollegeStudentDTO Clgapplicationdata(int id)
        {
            AdmMasterCollegeStudentDTO dt = new AdmMasterCollegeStudentDTO();

            string accountname = "";
            string accesskey = "";

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.MI_Id = mid;


            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dt.ID = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.ASMAY_Id = ASMAY_Id;

            dt.AMCST_Id = id;
            List<ClgMasterCategoryDMO> classcategoryList = new List<ClgMasterCategoryDMO>();
            if (dt.AMCST_Id > 0)
            {
                List<Adm_Master_College_StudentDMO> subCaste = new List<Adm_Master_College_StudentDMO>();
                subCaste = _context.Adm_Master_College_StudentDMO.Where(f => f.AMCST_Id == id).ToList();

                classcategoryList = (from m in _context.ClgMasterCategoryDMO
                                     from c in _context.ClgMasterCourseCategoryMapDMO
                                     from p in _context.MasterCourseDMO
                                     where (m.AMCOC_Id == c.AMCOC_Id && c.AMCO_Id == p.AMCO_Id && p.AMCO_Id == subCaste.FirstOrDefault().AMCO_Id && c.AMCO_Id == subCaste.FirstOrDefault().AMCO_Id && p.MI_Id == dt.MI_Id && c.AMCOCM_ActiveFlg == true)
                                     select m
                                    ).ToList();

            }



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
        [Route("checkbiometriccode")]
        public AdmMasterCollegeStudentDTO checkbiometriccode([FromBody] AdmMasterCollegeStudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
         //   data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegates.checkbiometriccode(data);
        }
        [Route("checkrfcardduplicate")]
        public AdmMasterCollegeStudentDTO checkrfcardduplicate([FromBody] AdmMasterCollegeStudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
         //   data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegates.checkrfcardduplicate(data);
        }

    }
}
