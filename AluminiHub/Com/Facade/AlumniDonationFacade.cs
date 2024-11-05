using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class AlumniDonationFacade : Controller
    {
        private readonly AlumniContext _db;
        AlumniDonationInterface _int;
        public AlumniDonationFacade(AlumniDonationInterface stu, AlumniContext db)
        {
            _int = stu;
            _db = db;
        }
        [Route("Pageload")]
        public AlumniStudentDTO Pageload([FromBody] AlumniStudentDTO stud)
        {
            return _int.Pageload(stud);
        }

        [Route("getamount")]
        public AlumniStudentDTO getamount([FromBody] AlumniStudentDTO dto)
        {
            return _int.getamount(dto);
        }
        [Route("getpayment_details")]
        public AlumniStudentDTO getpayment_details([FromBody] AlumniStudentDTO dto)
        {
            return _int.getpayment_details(dto);
        }
        [Route("paymentsave")]
        public AlumniStudentDTO paymentsave([FromBody] AlumniStudentDTO dto)
        {
            return _int.paymentsave(dto);
        }

        [Route("getdonationreport")]
        public AlumniStudentDTO getdonationreport([FromBody] AlumniStudentDTO dto)
        {
            return _int.getdonationreport(dto);
        }
        //=============master donation 
        [Route("getdata_donation")]
        public AlumniStudentDTO getdata_donation([FromBody] AlumniStudentDTO dto)
        {
            return _int.getdata_donation(dto);
        }


        [Route("save_donation")]
        public AlumniStudentDTO save_donation([FromBody] AlumniStudentDTO dto)
        {
            return _int.save_donation(dto);
        }

        [Route("deactive_donation")]
        public AlumniStudentDTO deactive_donation([FromBody] AlumniStudentDTO dto)
        {
            return _int.deactive_donation(dto);
        }

        [Route("edit_donation")]
        public AlumniStudentDTO edit_donation([FromBody] AlumniStudentDTO dto)
        {
            return _int.edit_donation(dto);
        }
         [Route("alumnidetails")]
        public AlumniStudentDTO alumnidetails([FromBody] AlumniStudentDTO dto)
        {
            return _int.alumnidetails(dto);
        }
         [Route("getdonationprint")]
        public AlumniStudentDTO getdonationprint([FromBody] AlumniStudentDTO dto)
        {
            return _int.getdonationprint(dto);
        }

    }
}
