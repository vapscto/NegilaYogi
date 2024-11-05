using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LostBookFacade : Controller
    {
        public LostBookInterface _objInter;
        public LostBookFacade(LostBookInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public LostBook_DTO getdetails([FromBody]LostBook_DTO data)
        {
            return _objInter.getdetails(data);
        }

        [Route("searchfilter")]
        public LostBook_DTO searchfilter([FromBody]LostBook_DTO data)
        {
            return _objInter.searchfilter(data);
        }

        [Route("get_authorNm")]
        public Task<LostBook_DTO> get_authorNm([FromBody]LostBook_DTO data)
        {
            return _objInter.get_authorNm(data);
        }

        [Route("get_radiochange")]
        public Task<LostBook_DTO> get_radiochange([FromBody]LostBook_DTO data)
        {
            return _objInter.get_radiochange(data);
        }

        [Route("saverecord")]
        public LostBook_DTO saverecord([FromBody]LostBook_DTO data)
        {
            return _objInter.saverecord(data);
        }

    }
}
