using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class clg_CB_SEM_MappingFacadeController : Controller
    {
        public clg_CB_SEM_MappingInterface _enq;
        public clg_CB_SEM_MappingFacadeController(clg_CB_SEM_MappingInterface Instit)
        {
            _enq = Instit;
        }

        [Route("getAllDetails")]
        public clg_CB_SEM_MappingDTO Getdata([FromBody]clg_CB_SEM_MappingDTO sct)
        {
            return _enq.GetDropDownList(sct);
        }

  
     

        [Route("Getbranch")]
        public clg_CB_SEM_MappingDTO Getbranch([FromBody] clg_CB_SEM_MappingDTO sct)
        {
            return _enq.Getbranch(sct);
        }



        [Route("savesem")]
        public clg_CB_SEM_MappingDTO savesem([FromBody] clg_CB_SEM_MappingDTO sct)
        {
            return _enq.savesem(sct);
        }


        [Route("Editrecord")]
        public clg_CB_SEM_MappingDTO Editrecord([FromBody] clg_CB_SEM_MappingDTO sct)
        {
            return _enq.Editrecord(sct);
        }

        [Route("sempopup")]
        public clg_CB_SEM_MappingDTO sempopup([FromBody] clg_CB_SEM_MappingDTO sct)
        {
            return _enq.sempopup(sct);
        }
        [Route("deactivate")]
        public clg_CB_SEM_MappingDTO deactivate([FromBody] clg_CB_SEM_MappingDTO sct)
        {
            return _enq.deactivate(sct);
        }

        
    }
}
