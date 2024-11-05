using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class QRCode_GenerationFacade : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        QRCode_Generation_Interface _Inv;

        public QRCode_GenerationFacade(QRCode_Generation_Interface Inv)
        {
            _Inv = Inv;
        }

        [Route("Getdetails")]
        public QRCode_GenerationDTO Getdetails([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.Getdetails(data);
        }

        [Route("SaveQR_Code")]
        public QRCode_GenerationDTO SaveQR_Code([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.SaveQR_Code(data);
        }
        [Route("STAFFSaveQR_Code")]
        public QRCode_GenerationDTO STAFFSaveQR_Code([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.STAFFSaveQR_Code(data);
        }
        [Route("get_classes")]
        public QRCode_GenerationDTO get_classes([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.get_classes(data);
        }
        [Route("get_cls_sections")]
        public QRCode_GenerationDTO get_cls_sections([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.get_cls_sections(data);
        }
        [Route("GetStudents")]
        public QRCode_GenerationDTO GetStudents([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.GetStudents(data);
        }
        [Route("QRReportDetails")]
        public QRCode_GenerationDTO QRReportDetails([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.QRReportDetails(data);
        }
        [Route("StaffGetdetails")]
        public QRCode_GenerationDTO StaffGetdetails([FromBody] QRCode_GenerationDTO data)
        {
            return _Inv.StaffGetdetails(data);
        }

        [Route("onloadgetdetails")]
        public QRCode_GenerationDTO getinitialdata([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.getBasicData(dto);
        }

        
        [Route("get_depts")]
        public QRCode_GenerationDTO get_depts([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.get_depts(dto);
        }
        [Route("get_desig")]
        public QRCode_GenerationDTO get_desig([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.get_desig(dto);
        }
        [Route("filterEmployeedetailsBySelection")]
        public QRCode_GenerationDTO FilterEmployeedetailsBySelection([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.FilterEmployeedetailsBySelection(dto);
        }

        [Route("QRcodegeneration")]
        public QRCode_GenerationDTO QRcodegeneration([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.QRcodegeneration(dto);
        }

        [Route("StudentQRCode")]
        public QRCode_GenerationDTO StudentQRCode([FromBody]QRCode_GenerationDTO dto)
        {
            return _Inv.StudentQRCode(dto);
        }

    }
}
