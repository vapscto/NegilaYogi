using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterTemplateController : Controller
    {
        MasterTemplateDelegate od = new MasterTemplateDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterTemplateDTO Get([FromQuery] int id)
        {
            return od.getAllDetails(id);
        }

        [HttpGet]
        [Route("getSaletypes/{id:int}")]
        public MasterTemplateDTO getSaletypes(int id)
        {
            return od.getSaletypes(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterTemplateDTO getdetail(int id)
        {
            return od.mastrtmpDetails(id);
        }


        [HttpPost]
        public MasterTemplateDTO savedetail([FromBody] MasterTemplateDTO mst)
        {
            return od.savedetails(mst);
        }

        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterTemplateDTO Delete(int id)
        {
            return od.deleterec(id);
        }
    }
}
