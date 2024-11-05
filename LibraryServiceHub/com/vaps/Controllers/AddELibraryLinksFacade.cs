using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using LibraryServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddELibraryLinksFacade : Controller
    {
        public  AddELibraryLinksInterface _LibInter;
        public  AddELibraryLinksFacade( AddELibraryLinksInterface para)
        {
            _LibInter = para;
        }
        [Route("Savedata")]
        public  AddELibraryLinksDTO Savedata([FromBody] AddELibraryLinksDTO data)
        {
            return _LibInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public  AddELibraryLinksDTO getdetails(int id)
        {
            return _LibInter.getdetails(id);
        }
        [Route("GetELibrary/{id:int}")]
        public  AddELibraryLinksDTO GetELibrary(int id)
        {
            return _LibInter.GetELibrary(id);
        }
        [Route("deactiveY")]
        public  AddELibraryLinksDTO deactiveY([FromBody] AddELibraryLinksDTO data)
        {
            return _LibInter.deactiveY(data);
        }
        [Route("geteditdata")]
        public  AddELibraryLinksDTO geteditdata([FromBody] AddELibraryLinksDTO data)
        {
            return _LibInter.geteditdata(data);
        }
    }
}
