using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class StudentRouteMappingFacadeController : Controller
    {
        public StudentRouteMappingInterface _feetar;

        public StudentRouteMappingFacadeController(StudentRouteMappingInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
   
        [HttpPost]
        [Route("getalldetails123")]
        public StudentRouteMappingDTO Getdet([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.getdata123(data);
        }
        
        [Route("get_sections")]
        public StudentRouteMappingDTO get_sections([FromBody] StudentRouteMappingDTO value)
        {
            return _feetar.get_sections(value);
        }
        [Route("getreport")]
        public StudentRouteMappingDTO getreport([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.getreport(data);
        }

   [Route("getreportedit")]
        public StudentRouteMappingDTO getreportedit([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.getreportedit(data);
        }

        
        [Route("get_cls_secs")]
        public StudentRouteMappingDTO get_cls_secs([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.get_cls_secs(data);
        }

        [Route("on_pic_route_change")]
        public StudentRouteMappingDTO on_pic_route_change([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.on_pic_route_change(data);
        }
        [Route("on_drp_route_change")]
        public StudentRouteMappingDTO on_drp_route_change([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.on_drp_route_change(data);
        }
        
        [HttpPost]
        [Route("savedata")]
        public StudentRouteMappingDTO getclassstudentlist([FromBody] StudentRouteMappingDTO student)
        {
            return _feetar.getlisttwo(student);
        }

        
        [Route("deactivate")]
        public StudentRouteMappingDTO deactivate([FromBody] StudentRouteMappingDTO student)
        {
            return _feetar.deactivate(student);
        }
        [HttpPost]
        [Route("searching")]
        public StudentRouteMappingDTO searching([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.searching(data);
        }
        [Route("get_loca_sches")]
        public StudentRouteMappingDTO get_loca_sches([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.get_loca_sches(data);
        }
        [Route("viewrecordspopup")]
        public StudentRouteMappingDTO viewrecordspopup([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.viewrecordspopup(data);
        }
        [Route("SearchByColumn")]
        public StudentRouteMappingDTO SearchByColumn([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.SearchByColumn(data);
        }
        [Route("checkduplicateno")]
        public StudentRouteMappingDTO checkduplicateno([FromBody] StudentRouteMappingDTO data)
        {
            return _feetar.checkduplicateno(data);
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
    }
}
