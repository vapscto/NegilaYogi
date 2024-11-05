using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Admission;
using CollegePreadmission.Interfaces;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class ApplicationFormFacade : Controller
    {
        ApplicationFormInterface _interface;
        public ApplicationFormFacade(ApplicationFormInterface interf)
        {
            _interface = interf;
        }
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails([FromBody]CollegePreadmissionstudnetDto obj)
        {
            return _interface.Getdetails(obj);
        }
        [Route("getCourse")]
        public CollegePreadmissionstudnetDto getCourse([FromBody]CollegePreadmissionstudnetDto data)
        {
            return _interface.getCourse(data);
        }
        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody] CollegePreadmissionstudnetDto dt)
        {
            return _interface.getBranch(dt);
        }
        [Route("Dashboarddetails")]
        public CollegePreadmissionstudnetDto Dashboarddetails([FromBody] CollegePreadmissionstudnetDto enqo)
        {
            return _interface.Dashboarddetails(enqo);
        }

        [Route("paynow")]
        public CollegePreadmissionstudnetDto paynow([FromBody] CollegePreadmissionstudnetDto dt)
        {
            return _interface.paynow(dt);
        }

        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse([FromBody]PaymentDetails response)
        {

            return _interface.payuresponse(response);
        }

        [Route("getpaymentresponsePAYTM/")]
        public PaymentDetails.PAYTM getpaymentresponsePAYTM([FromBody]PaymentDetails.PAYTM response)
        {

            return _interface.paytmresponse(response);
        }

        [Route("getSemester")]
        public CollegePreadmissionstudnetDto getSemester([FromBody] CollegePreadmissionstudnetDto dto)
        {
            return _interface.getSemester(dto);
        }
        [Route("getCaste")]
        public CollegePreadmissionstudnetDto getCaste([FromBody] CollegePreadmissionstudnetDto dtos)
        {
            return _interface.getcaste(dtos);
        }
        [Route("getQuotaCategory")]
        public AdmMasterCollegeStudentDTO getQuotaCategory([FromBody] AdmMasterCollegeStudentDTO obj)
        {
            return _interface.getQuotaCategory(obj);
        }
        [Route("saveStudentDetails")]
        public CollegePreadmissionstudnetDto saveStudentDetails([FromBody] CollegePreadmissionstudnetDto obj)
        {
            return _interface.saveStudentDetails(obj);
        }
        [Route("Edit")]
        public CollegePreadmissionstudnetDto Edit([FromBody] CollegePreadmissionstudnetDto editdata)
        {
            return _interface.Edit(editdata);
        }
        [Route("getprintdata")]
        public CollegePreadmissionstudnetDto getprintdata([FromBody] CollegePreadmissionstudnetDto editdata)
        {
            return _interface.getprintdata(editdata);
        }
        [Route("checkDuplicate")]
        public AdmMasterCollegeStudentDTO checkDuplicate([FromBody] AdmMasterCollegeStudentDTO dup)
        {
            return _interface.checkDuplicate(dup);
        }
        [Route("getdpstate")]
        public CollegePreadmissionstudnetDto getdpstate([FromBody] CollegePreadmissionstudnetDto dups)
        {
            return _interface.getdpstate(dups);
        }
        [Route("saveAddress")]
        public CollegePreadmissionstudnetDto saveAddress([FromBody] CollegePreadmissionstudnetDto addr)
        {
            return _interface.saveAddress(addr);
        }
        [Route("saveParentsDetails")]
        public CollegePreadmissionstudnetDto saveParentsDetails([FromBody] CollegePreadmissionstudnetDto ParentsData)
        {
            return _interface.saveParentsDetails(ParentsData);
        }
        [Route("StateByCountryName")]
        public CollegePreadmissionstudnetDto StateByCountryName([FromBody] CollegePreadmissionstudnetDto country)
        {
            return _interface.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public CollegePreadmissionstudnetDto saveOthersDetails([FromBody] CollegePreadmissionstudnetDto others)
        {
            return _interface.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public CollegePreadmissionstudnetDto saveDocuments([FromBody] CollegePreadmissionstudnetDto docs)
        {
            return _interface.saveDocuments(docs);
        }
        //[Route("SearchByColumn")]
        //public AdmMasterCollegeStudentDTO SearchByColumn([FromBody] AdmMasterCollegeStudentDTO docs)
        //{
        //    return _interface.SearchByColumn(docs);
        //}
        //[Route("DeleteEntry")]
        //public AdmMasterCollegeStudentDTO DeleteEntry([FromBody] AdmMasterCollegeStudentDTO docs)
        //{
        //    return _interface.DeleteEntry(docs);
        //}


        //master competitve exam
        [Route("compExamName")]
        public CollegePreadmissionstudnetDto compExamName([FromBody] CollegePreadmissionstudnetDto country)
        {
            return _interface.compExamName(country);
        }

        //Razor pay
        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            return _interface.razorgetpaymentresponse(response);

        }

        //docs download
        [Route("Clgapplicationstudocs")]
        public CollegePreadmissionstudnetDto Clgapplicationstudocs([FromBody] CollegePreadmissionstudnetDto country)
        {
            return _interface.Clgapplicationstudocs(country);
        }
    }
}
