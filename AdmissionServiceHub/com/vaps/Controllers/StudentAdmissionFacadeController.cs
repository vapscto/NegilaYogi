using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class StudentAdmissionFacadeController : Controller
    {
        public StudentAdmissionInterface _Adm_M_StudentDTO;
        public StudentAdmissionFacadeController(StudentAdmissionInterface StudentAdmissionInterface)
        {
            _Adm_M_StudentDTO = StudentAdmissionInterface;
        }
       
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public async Task<Adm_M_StudentDTO> Getdetails([FromBody] Adm_M_StudentDTO dto)//int IVRMM_Id
        {
           return await _Adm_M_StudentDTO.GetData(dto);    
        }

        [Route("getdpstate/{id:int}")]
        public Adm_M_StudentDTO getdpstate(int id)
        {
            return _Adm_M_StudentDTO.getdpstate(id);
        }

        [Route("getdpdistrict/{id:int}")]
        public Adm_M_StudentDTO getdpdistrict(int id)
        {
            return _Adm_M_StudentDTO.getdpdistrict(id);
        }


        [Route("onchangebithplacecountry/{id:int}")]
        public Adm_M_StudentDTO onchangebithplacecountry(int id)
        {
            return _Adm_M_StudentDTO.onchangebithplacecountry(id);
        }

        [Route("onchangenationality/{id:int}")]
        public Adm_M_StudentDTO onchangenationality(int id)
        {
            return _Adm_M_StudentDTO.onchangenationality(id);
        }

        [Route("getdpcities/{id:int}")]
        public Adm_M_StudentDTO getdpcities(int id)
        {
            return _Adm_M_StudentDTO.getdpcities(id);
        }

        [Route("GetSelectedRowDetails")]
        public Adm_M_StudentDTO GetSelectedRowDetails([FromBody]Adm_M_StudentDTO ID)
        {
            return _Adm_M_StudentDTO.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public Adm_M_StudentDTO Post([FromBody] Adm_M_StudentDTO masterMDT)
        {
            return  _Adm_M_StudentDTO.SaveData(masterMDT);
        }

        [Route("checkDuplicate")]
        public Adm_M_StudentDTO checkDuplicate([FromBody] Adm_M_StudentDTO masterMDT)
        {
            return _Adm_M_StudentDTO.checkDuplicate(masterMDT);
        }

        [Route("getcaste")]
        public Adm_M_StudentDTO getcaste([FromBody] Adm_M_StudentDTO masterMDT)
        {
            return _Adm_M_StudentDTO.getcaste(masterMDT);
        }
        
        [HttpDelete]
        [Route("DeleteBondEntry/{id:int}")]
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            return _Adm_M_StudentDTO.DeleteBondEntry(ID);
        }


        [Route("DeleteEntry")]
        public Adm_M_StudentDTO DeleteEntry([FromBody] Adm_M_StudentDTO data)
        { 
            return _Adm_M_StudentDTO.DeleteEntry(data);
        }

        [Route("yearwisetcstd")]
        public Adm_M_StudentDTO yearwisetcstd([FromBody]Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.yearwisetcstd(data);
        }

        [Route("addtocart")]
        public Adm_M_StudentDTO addtocart([FromBody]Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.addtocart(data);
        }

        [Route("searchbycolumn")]
        public Adm_M_StudentDTO SearchByColumn([FromBody] Adm_M_StudentDTO dto)
        {
            return _Adm_M_StudentDTO.searchByColumn(dto);
        }

        [Route("StateByCountryName")]
        public Adm_M_StudentDTO StateByCountryName([FromBody] Adm_M_StudentDTO dt)
        {
            return _Adm_M_StudentDTO.StateByCountryName(dt);
        }

        [Route("classchangemaxminage")]
        public Adm_M_StudentDTO getmaxminage([FromBody] Adm_M_StudentDTO maxmin)
        {
            return _Adm_M_StudentDTO.getmaxminage(maxmin);
        }

        [Route("savefirsttab")]
        public Task<Adm_M_StudentDTO> savefirsttab([FromBody] Adm_M_StudentDTO maxmin)
        {
            return  _Adm_M_StudentDTO.savefirsttab(maxmin);
        }

        [Route("savesecondtab")]
        public Adm_M_StudentDTO savesecondtab([FromBody] Adm_M_StudentDTO maxmin)
        {
            return _Adm_M_StudentDTO.savesecondtab(maxmin);
        }

        [Route("savesixthtab")]
        public Adm_M_StudentDTO savesixthtab([FromBody] Adm_M_StudentDTO maxmin)
        {
            return _Adm_M_StudentDTO.savesixthtab(maxmin);
        }

        [Route("savethirdtab")]
        public Task<Adm_M_StudentDTO> savethirdtab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savethirdtab(data);
        }

        [Route("savefourthtab")]
        public Adm_M_StudentDTO savefourthtab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savefourthtab(data);
        }

        [Route("savefinaltab")]
        public Adm_M_StudentDTO savefinaltab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savefinaltab(data);
        }

        [Route("checkbiometriccode")]
        public Adm_M_StudentDTO checkbiometriccode([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.checkbiometriccode(data);
        }

        [Route("checkrfcardduplicate")]
        public Adm_M_StudentDTO checkrfcardduplicate([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.checkrfcardduplicate(data);
        }

        [Route("onchangefathernationality")]
        public Adm_M_StudentDTO onchangefathernationality([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.onchangefathernationality(data);
        }

        [Route("onchangemothernationality")]
        public Adm_M_StudentDTO onchangemothernationality([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.onchangemothernationality(data);
        }


        // Admission Cancel OR Widthdraw

        [Route("OnLoadAdmissionCancel")]
        public Adm_M_StudentDTO OnLoadAdmissionCancel([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.OnLoadAdmissionCancel(data);
        }

        [Route("OnChangeAdmissionCancelYear")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelYear([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.OnChangeAdmissionCancelYear(data);
        }

        [Route("OnChangeAdmissionCancelStudent")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
           
            return _Adm_M_StudentDTO.OnChangeAdmissionCancelStudent(data);
        }

        [Route("SaveAdmissionCancelStudent")]
        public Adm_M_StudentDTO SaveAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.SaveAdmissionCancelStudent(data);
        }

        [Route("EditAdmissionCancelStudent")]
        public Adm_M_StudentDTO EditAdmissionCancelStudent([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.EditAdmissionCancelStudent(data);
        }

        // Admission Cancel Report

        [Route("OnLoadAdmissionCancelReport")]
        public Adm_M_StudentDTO OnLoadAdmissionCancelReport([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.OnLoadAdmissionCancelReport(data);
        }

        [Route("OnChangeAdmissionCancelReportYear")]
        public Adm_M_StudentDTO OnChangeAdmissionCancelReportYear([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.OnChangeAdmissionCancelReportYear(data);
        }

        [Route("ViewStudentProfile")]
        public Adm_M_StudentDTO ViewStudentProfile([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.ViewStudentProfile(data);
        }

        [Route("GetSubjectsofinstitute")]
        public Task<Adm_M_StudentDTO> GetSubjectsofinstitute([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.GetSubjectsofinstitute(data);
        }

    }
}
