using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    
    [Route("api/[controller]")]
    public class ClientWise_Module_FeatureFacadeController : Controller
    {
        public ClientWise_Module_Feature_Interface _inter;
        public ClientWise_Module_FeatureFacadeController(ClientWise_Module_Feature_Interface inter)
        {
            _inter = inter;
        }
        [Route("getmodule")]
        public ClientWise_Module_Feature_DTO getmodule([FromBody]ClientWise_Module_Feature_DTO data)
        {
            return _inter.getmodule(data);
        }

        [Route("getreport")]
        public ClientWise_Module_Feature_DTO getreport([FromBody]ClientWise_Module_Feature_DTO data)
        {
            return _inter.getreport(data);
        }
    }
}