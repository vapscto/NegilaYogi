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
    public class BookRegisterFacade : Controller
    {

        public BookRegisterInterface _objInter;
        public BookRegisterFacade(BookRegisterInterface data)
        {
            _objInter = data;
        }
        [Route("getdetails")]
        public BookRegisterDTO getdetails([FromBody]BookRegisterDTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("Ckeck_ISBNNO")]
        public BookRegisterDTO Ckeck_ISBNNO([FromBody]BookRegisterDTO data)
        {
            return _objInter.Ckeck_ISBNNO(data);
        }

        [Route("chekAccno")]
        public BookRegisterDTO chekAccno([FromBody] BookRegisterDTO data)
        {
            return _objInter.chekAccno(data);
        }
        [Route("Addaccnno")]
        public BookRegisterDTO Addaccnno([FromBody] BookRegisterDTO data)
        {
            return _objInter.Addaccnno(data);
        }
        [Route("Savedata")]
        public BookRegisterDTO Savedata([FromBody] BookRegisterDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("Tab1Savedata")]
        public BookRegisterDTO Tab1Savedata([FromBody] BookRegisterDTO data)
        {
            return _objInter.Tab1Savedata(data);
        }
        [Route("Tab2Savedata")]
        public BookRegisterDTO Tab2Savedata([FromBody] BookRegisterDTO data)
        {
            return _objInter.Tab2Savedata(data);
        }
        [Route("Ckeck_LMBANO_AccessionNo")]
        public BookRegisterDTO Ckeck_LMBANO_AccessionNo([FromBody] BookRegisterDTO data)
        {
            return _objInter.Ckeck_LMBANO_AccessionNo(data);
        }


        [Route("deactiveY")]
        public BookRegisterDTO deactiveY([FromBody] BookRegisterDTO data)
        {
            return _objInter.deactiveY(data);
        }

        
        [Route("Editdata")]
        public Task<BookRegisterDTO> Editdata([FromBody] BookRegisterDTO data)
        {
            return _objInter.Editdata(data);
        }


        [Route("searching")]
        public BookRegisterDTO searching([FromBody] BookRegisterDTO data)
        {
            return _objInter.searching(data);
        }
        
        [Route("searchfilter")]
        public BookRegisterDTO searchfilter([FromBody] BookRegisterDTO data)
        {
            return _objInter.searchfilter(data);
        }

        [Route("changelibrary")]
        public Task<BookRegisterDTO> changelibrary([FromBody] BookRegisterDTO data)
        {
            return _objInter.changelibrary(data);
        }

    }
}
