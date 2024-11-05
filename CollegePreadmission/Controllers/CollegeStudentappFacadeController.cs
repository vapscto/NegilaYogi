using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePreadmission.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class CollegeStudentappFacadeController : Controller
    {
        public CollegeStudentappInterface _stu;

        public CollegeStudentappFacadeController(CollegeStudentappInterface enqui)
        {
            _stu = enqui;
        }
        // GET: api/values
        [HttpGet]
        public Task<CountryDTO> Get(CountryDTO enqo)
        {
            return _stu.countrydrp(enqo);
        }

        [Route("country")]
        public Task<CountryDTO> Gets([FromBody] CountryDTO enqo)
        {
            return _stu.getIndependentDropDowns(enqo);
        }

        [Route("ActivateDactivate")]
        public CountryDTO ActivateDactivate([FromBody] CountryDTO enqo)
        {
            return _stu.ActivateDactivate(enqo);
        }

        [Route("Getcountofstudents")]
        public Task<CountryDTO> Getcountofstudents([FromBody] CountryDTO enqo)
        {
            return _stu.Getcountofstudents(enqo);
        }

        [Route("Dashboarddetails")]
        public Task<CountryDTO> Dashboarddetails([FromBody] CountryDTO enqo)
        {
            return _stu.Dashboarddetails(enqo);
        }

        [Route("classchangemaxminage")]
        public StudentApplicationDTO getmaxminage([FromBody] StudentApplicationDTO maxmin)
        {
            return _stu.getmaxminage(maxmin);
        }

        [Route("GetSubjectsofinstitute")]
        public Task<StudentApplicationDTO> GetSubjectsofinstitute([FromBody] StudentApplicationDTO maxmin)
        {
            return _stu.GetSubjectsofinstitute(maxmin);
        }

        //[Route("getenquirycontroller/{id:int}")]
        ////[Route("getenquirycontroller")]
        //public StateDTO Getcountrydata(int id)
        //{
        //    return _enq.enqdrpcountrydata(id);
        //}

        // POST api/values
        [HttpPost]
        public Task<StudentApplicationDTO> studdet([FromBody] StudentApplicationDTO studeta)
        {
            return _stu.studdet(studeta);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        // Added on 6-10-2016
        [Route("getStudentEditDataFacades")]
        public StudentApplicationDTO getStudentEditData([FromBody] StudentApplicationDTO dt)
        {
            return _stu.getStudentEditData(dt);
        }


        [Route("paynow")]
        public StudentApplicationDTO paynow([FromBody] StudentApplicationDTO dt)
        {
            return _stu.paynow(dt);
        }
        [Route("getstudentprintDataFacades")]
        public StudentApplicationDTO getstudentprintDataFacades([FromBody] StudentApplicationDTO dt)
        {
            return _stu.getstudentprintData(dt);
        }


        [Route("getdashboardpage")]
        public StudentApplicationDTO getdashboardpage([FromBody] StudentApplicationDTO dt)
        {
            return _stu.getdashboardpage(dt);
        }
        // Added on 6-10-2016

        // Added on 19-9-2016
        [Route("getdpcity/{id:int}")]
        public Task<CityDTO> getCity(int id)
        {
            return _stu.getCityByCountry(id);
        }

        [Route("getroutes/{id:int}")]
        public Task<StateDTO> getroutes(int id)
        {
            return _stu.getroutes(id);
        }
        [Route("getrouteslocation/{id:int}")]
        public Task<StateDTO> getrouteslocation(int id)
        {
            return _stu.getrouteslocation(id);
        }

        [Route("getdpstate/{id:int}")]
        public Task<StateDTO> getdpstate(int id)
        {
            return _stu.getStateByCountry(id);
        }

        [Route("getdprospectusdetails/{id:int}")]
        public Task<StateDTO> getdprospectusdetails(int id)
        {
            return _stu.getdprospectusdetails(id);
        }

        [Route("getdpstatesubcatse/{id:int}")]
        public Task<StateDTO> getdpstatesubcatse(int id)
        {
            return _stu.getdpstatesubcatse(id);
        }

        [Route("getdpstatesubcatsefather/{id:int}")]
        public Task<StateDTO> getdpstatesubcatsefather(int id)
        {
            return _stu.getdpstatesubcatsefather(id);
        }

        //[Route("getapplicationhtml/{id:int}")]
        //public Task<StateDTO> getapplicationhtml(int id)
        //{
        //    return _stu.getdpstatesubcatsefather(id);
        //}

        [Route("getapplicationhtml")]
        public StudentApplicationDTO getapplicationhtml([FromBody] StudentApplicationDTO maxmin)
        {
            return _stu.getapplicationhtml(maxmin);
        }

        [Route("getdpstatesubcatsemother/{id:int}")]
        public Task<StateDTO> getdpstatesubcatsemother(int id)
        {
            return _stu.getdpstatesubcatsemother(id);
        }


        [Route("getdpcities/{id:int}")]
        public Task<CityDTO> getCities(int id)
        {
            return _stu.getCityByState(id);
        }

        [Route("deletedetails/{id:int}")]
        public void delrec(int id)
        {
            _stu.deleterec(id);
        }

        //[Route("getdpArea/{id:int}")]
        //public CountryDTO getArea(int id)
        //{
        //    return _stu.getAreaByCity(id);
        //}
        // Added on 19-9-2016

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            return _stu.payuresponse(response);
        }

        [Route("savehealthcertificatedetail")]
        public Task<StudentHelthcertificateDTO> savehealthcertificatedetail([FromBody] StudentHelthcertificateDTO studeta)
        {
            return _stu.savehealthcertificatedetail(studeta);
        }

        [Route("getstudata")]
        public Task<StudentHelthcertificateDTO> getstudata([FromBody] StudentHelthcertificateDTO enqo)
        {
            return _stu.getstudata(enqo);
        }
        [Route("getEdithelthData")]
        public StudentHelthcertificateDTO getEdithelthData([FromBody] StudentHelthcertificateDTO dt)
        {
            return _stu.getEdithelthData(dt);
        }
        [Route("deletehelthdetails")]
        public StudentHelthcertificateDTO deletehelthdetails([FromBody] StudentHelthcertificateDTO dt)
        {
            return _stu.deletehelthdetails(dt);
        }
        [Route("printgethelthData")]
        public StudentHelthcertificateDTO printgethelthData([FromBody] StudentHelthcertificateDTO dt)
        {
            return _stu.printgethelthData(dt);
        }
        [Route("fill_prospectus")]
        public StudentApplicationDTO fill_prospectus([FromBody] StudentApplicationDTO data)
        {
            return _stu.fill_prospectus(data);
        }
    }
}
