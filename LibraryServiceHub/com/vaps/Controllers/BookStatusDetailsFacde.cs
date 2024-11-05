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
    public class BookStatusDetailsFacde : Controller
    {
        public BookStatusDetailsInterface _objInter;
        public BookStatusDetailsFacde(BookStatusDetailsInterface data)
        {
            _objInter = data;
        }
        
        [Route("getdetails")] 
        public BookStatusDetailsDTO getdetails([FromBody]BookStatusDetailsDTO data)
        {
            return _objInter.getdetails(data);
        }
        

        [Route("searchfilter")] 
        public Task<BookStatusDetailsDTO> searchfilter([FromBody]BookStatusDetailsDTO data)
        {
            return _objInter.searchfilter(data);
        }
        [Route("get_bookdetails")] 
        public Task<BookStatusDetailsDTO> get_bookdetails([FromBody]BookStatusDetailsDTO data)
        {
            return _objInter.get_bookdetails(data);
        }
    }
}
