using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class AlumniMembershipFacade : Controller
    {

        public AlumniMembershipInterface _Alumnimember;

        public AlumniMembershipFacade(AlumniMembershipInterface Alumnimember)
        {
            _Alumnimember = Alumnimember;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Get_Intial_data/")]
        public AlumniStudentDTO Getdetails([FromBody] AlumniStudentDTO AlumniStudentDTO)//int IVRMM_Id
        {
            return _Alumnimember.Get_Intial_data(AlumniStudentDTO);
        }

        [Route("getstudata")]
        public AlumniStudentDTO getstudata([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.getstudata(sddto);
        }


        [Route("Getstudentlist")]
        public Task<AlumniStudentDTO> Getstudentlist([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.Getstudentlist(sddto);
        }

        [Route("Getstudentlistapp")]
        public Task<AlumniStudentDTO> Getstudentlistapp([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.Getstudentlistapp(sddto);
        }

        [Route("checkstudent")]
        public AlumniStudentDTO checkstudent([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.checkstudent(sddto);
        }

        [Route("aproovedata")]
        public AlumniStudentDTO aproovedata([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.aproovedata(sddto);
        }

        [Route("savedata")]
        public AlumniStudentDTO savedataa([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.svedata(sddto);
        }

        [Route("svedatanewalumni")]
        public AlumniStudentDTO svedatanewalumni([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.svedatanewalumni(sddto);
        }

        [Route("onchangecountry")]
        public AlumniStudentDTO onchangecountry([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.onchangecountry(sddto);
        }

        [Route("onchangedistrict")]
        public AlumniStudentDTO onchangedistrict([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.onchangedistrict(sddto);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [Route("searchfilter")]
        public AlumniStudentDTO searchfilter([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.searchfilter(sddto);
        }

        [Route("viewData")]
        public AlumniStudentDTO viewData([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.viewData(sddto);
        }
        [Route("onchangestate")]
        public AlumniStudentDTO onchangestate([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.onchangestate(sddto);
        }
        [Route("deactive")]
        public AlumniStudentDTO deactive([FromBody]AlumniStudentDTO sddto)
        {
            return _Alumnimember.deactive(sddto);
        }
      
        [Route("AlumniWedding/{id:int}")]
        public void Check_SMS_Mail_Status(int id)
        {
             _Alumnimember.AlumniWedding(id);
        }
        //Akash
        [Route("EditAlumniHomepages")]
        public AlumniStudentDTO EditAlumniHomepages([FromBody] AlumniStudentDTO data)
        {
            return _Alumnimember.EditAlumniHomepages(data);
        }

        [Route("AlumniHomepageActiveDeactives")]
        public AlumniStudentDTO AlumniHomepageActiveDeactives([FromBody] AlumniStudentDTO data)
        {
            return _Alumnimember.AlumniHomepageActiveDeactives(data);
        }
    }
}
