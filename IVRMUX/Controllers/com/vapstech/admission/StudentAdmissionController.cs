using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Drawing;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    // [EnableCors("AllowSpecificOrigin")]

    public class StudentAdmissionController : Controller
    {
        // GET: /<controller>/
        StudentAdmissionDelegates StudentAdmissionStr = new StudentAdmissionDelegates();
        private readonly IHostingEnvironment _hostingEnvironment;
        public StudentAdmissionController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails/")]
        public Adm_M_StudentDTO Getdetails(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
            Adm_M_StudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Adm_M_StudentDTO.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            Adm_M_StudentDTO.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.GetData(Adm_M_StudentDTO);
        }

        [Route("getdpstate/{id:int}")]
        public Adm_M_StudentDTO getdpstate(int id)
        {
            return StudentAdmissionStr.getdpstate(id);
        }

        [Route("getdpdistrict/{id:int}")]
        public Adm_M_StudentDTO getdpdistrict(int id)
        {
            return StudentAdmissionStr.getdpdistrict(id);
        }

        [Route("onchangebithplacecountry/{id:int}")]
        public Adm_M_StudentDTO onchangebithplacecountry(int id)
        {
            return StudentAdmissionStr.onchangebithplacecountry(id);
        }

        [Route("onchangenationality/{id:int}")]
        public Adm_M_StudentDTO onchangenationality(int id)
        {
            return StudentAdmissionStr.onchangenationality(id);
        }       

        [Route("getdpcities/{id:int}")]
        public Adm_M_StudentDTO getdpcities(int id)
        {
            return StudentAdmissionStr.getdpcities(id);
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public Adm_M_StudentDTO GetSelectedRowDetails(int ID)
        {
            Adm_M_StudentDTO data = new Adm_M_StudentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = ID;
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.GetSelectedRowDetails(data);
        }

        [HttpPost]
        public Adm_M_StudentDTO SaveData([FromBody] Adm_M_StudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.SaveData(MMD);
        }

        [HttpDelete]
        [Route("DeleteBondEntry/{id:int}")]
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            return StudentAdmissionStr.DeleteBondEntry(ID);
        }

        
        [Route("DeleteEntry")]
        public Adm_M_StudentDTO DeleteEntry([FromBody] Adm_M_StudentDTO data)
        {             
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.DeletedEntry(data);
        }

        [Route("checkDuplicate")]
        public Adm_M_StudentDTO checkDuplicate([FromBody] Adm_M_StudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.checkDuplicate(MMD);
        }

        [Route("getCaste")]
        public Adm_M_StudentDTO getCaste([FromBody] Adm_M_StudentDTO MMD)
        {
            MMD.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            MMD.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.getCaste(MMD);
        }

        [Route("yearwisetcstd")]
        public Adm_M_StudentDTO yearwisetcstd([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.yearwisetcstd(data);
        }

        [Route("addtocart")]
        public Adm_M_StudentDTO addtocart([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.addtocart(data);
        }

        [Route("SearchByColumn")]
        public Adm_M_StudentDTO SearchByColumn([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.searchByClmn(data);
        }

        [Route("StateByCountryName")]
        public Adm_M_StudentDTO StateByCountryName([FromBody]Adm_M_StudentDTO countryname)
        {
            countryname.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.StateByCountryName(countryname);
        }

        [Route("classchangemaxminage")]
        public Adm_M_StudentDTO getmaxminage([FromBody] Adm_M_StudentDTO maxmin)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.MI_Id = mid;
            maxmin.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.getmaxminage(maxmin);
        }

        [Route("savefirsttab")]
        public Adm_M_StudentDTO savefirsttab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savefirsttab(data);
        }

        [Route("savesixthtab")]
        public Adm_M_StudentDTO savesixthtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savesixthtab(data);
        }

        [Route("savesecondtab")]
        public Adm_M_StudentDTO savesecondtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savesecondtab(data);
        }

        [Route("savethirdtab")]
        public Adm_M_StudentDTO savethirdtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savethirdtab(data);
        }

        [Route("savefourthtab")]
        public Adm_M_StudentDTO savefourthtab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savefourthtab(data);
        }

        [Route("savefinaltab")]
        public Adm_M_StudentDTO savefinaltab([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.savefinaltab(data);
        }

        [Route("checkbiometriccode")]
        public Adm_M_StudentDTO checkbiometriccode([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.checkbiometriccode(data);
        }

        [Route("checkrfcardduplicate")]
        public Adm_M_StudentDTO checkrfcardduplicate([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.checkrfcardduplicate(data);
        }

        [Route("onchangefathernationality")]
        public Adm_M_StudentDTO onchangefathernationality([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.onchangefathernationality(data);
        }

        [Route("onchangemothernationality")]
        public Adm_M_StudentDTO onchangemothernationality([FromBody] Adm_M_StudentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.onchangemothernationality(data);
        }


        // Admission Cancel OR Widthdraw

        [Route("OnLoadAdmissionCancel/{id:int}")]
        public Adm_M_StudentDTO OnLoadAdmissionCancel(int id)
        {
            Adm_M_StudentDTO data = new Adm_M_StudentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.OnLoadAdmissionCancel(data);
        }

        [Route("OnChangeAdmissionCancelYear")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelYear([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id")); 
            return StudentAdmissionStr.OnChangeAdmissionCancelYear(data);
        }

        [Route("OnChangeAdmissionCancelStudent")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id")); 
            return StudentAdmissionStr.OnChangeAdmissionCancelStudent(data);
        }

        [Route("SaveAdmissionCancelStudent")]
        public Adm_M_StudentDTO SaveAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.SaveAdmissionCancelStudent(data);
        }

        [Route("EditAdmissionCancelStudent")]
        public Adm_M_StudentDTO EditAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.EditAdmissionCancelStudent(data);
        }

        // Admission Cancel Report
        [Route("OnLoadAdmissionCancelReport/{id:int}")]
        public Adm_M_StudentDTO OnLoadAdmissionCancelReport(int id)
        {
            Adm_M_StudentDTO data = new Adm_M_StudentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return StudentAdmissionStr.OnLoadAdmissionCancelReport(data);
        }

        [Route("OnChangeAdmissionCancelReportYear")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelReportYear([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return StudentAdmissionStr.OnChangeAdmissionCancelReportYear(data);
        }

        [Route("ViewStudentProfile")]
        public Adm_M_StudentDTO ViewStudentProfile([FromBody] Adm_M_StudentDTO data)
        {
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return StudentAdmissionStr.ViewStudentProfile(data);
        }


        [Route("GetSubjectsofinstitute")]
        public Adm_M_StudentDTO GetSubjectsofinstitute([FromBody] Adm_M_StudentDTO maxmin)
        {
            maxmin.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            maxmin.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return StudentAdmissionStr.GetSubjectsofinstitute(maxmin);
        }
    }
}
