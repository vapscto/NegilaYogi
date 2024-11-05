using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class StaffLoginFacade : Controller
    {
        public StaffLoginInterface _staffL;
        //private readonly UserManager<ApplicationUser> _UserManager;

        public StaffLoginFacade(StaffLoginInterface stafflogin)
        {
            _staffL = stafflogin;
           
        }

        [HttpPost]
        [Route("getmoduledetails")]
        public StaffLoginDTO Get([FromBody] StaffLoginDTO data)    
        {
            return _staffL.getmoduledet(data);
        }

        [HttpGet]
        [Route("getmodulerolesinswise/{id:int}")]
        public StaffLoginDTO getmodroleins(int id)
        {
            return _staffL.getmoduleroledetails(id);
        }

        [Route("getpagedetailsrolemodulewise")]
        public async Task<StaffLoginDTO> getpagename([FromBody] StaffLoginDTO pgmod)
        {
            return await _staffL.getpagedetails(pgmod);
        }


        [Route("updateusername")]
        public Task<StaffLoginDTO> updateusername([FromBody] StaffLoginDTO pgmod)
        {
            return _staffL.updateusername(pgmod);
        } 


        // POST api/values
        [HttpPost]
        public async Task<StaffLoginDTO> Post([FromBody]StaffLoginDTO pgmod)
        {
            var result = await _staffL.saveorgdet(pgmod);
            return pgmod;
        }

        [HttpPost("{id}")]
        public StaffLoginDTO Put(int id, [FromBody]StaffLoginDTO value)
        {
            return _staffL.getfilterdet(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

      
        

        [HttpPost]
        [Route("checkdupli")]
        public StaffLoginDTO checkduplicateusername([FromBody] StaffLoginDTO pgmodu)
        {
            return _staffL.checkusernmedup(pgmodu);
        }

        [Route("deletemodpages")]
        public StaffLoginDTO Delete([FromBody] StaffLoginDTO id)
        {
            return _staffL.deleterec(id);
        }

        [Route("searchfilter")]
        public StaffLoginDTO searchfilter([FromBody]StaffLoginDTO sddto)
        {
            return _staffL.searchfilter(sddto);
        }

        [Route("getstudata")]
        public StaffLoginDTO getstudata([FromBody]StaffLoginDTO sddto)
        {
            return _staffL.getstudata(sddto);
        }

        [Route("onchangeuser")]
        public async Task<StaffLoginDTO> onchangeuser([FromBody] StaffLoginDTO sddto)
        {
            return await _staffL.onchangeuser(sddto);
        }

        [Route("multionchangeuser")]
        public async Task<StaffLoginDTO> multionchangeuser([FromBody] StaffLoginDTO sddto)
        {
            return await _staffL.multionchangeuser(sddto);
        }

        [Route("multiuserdeletpages")]
        public async Task<StaffLoginDTO> multiuserdeletpages([FromBody] StaffLoginDTO sddto)
        {
            return await _staffL.multiuserdeletpages(sddto);
        }

        [Route("changeinsti")]
        public StaffLoginDTO changeinstitu([FromBody] StaffLoginDTO data)
        {
            return _staffL.changeinstitu(data);
        }


        [HttpPost]
        [Route("checktrust")]
        public StaffLoginDTO checktrustfun([FromBody] StaffLoginDTO pgmodu)
        {
            return _staffL.checktrustfunction(pgmodu);
        }

        [HttpPost]
        [Route("getstaffmobilepages")]
        public StaffLoginDTO getstaffmobilepages([FromBody] StaffLoginDTO pgmodu)
        {
            return _staffL.getstaffmobilepages(pgmodu);
        }

    }
}
