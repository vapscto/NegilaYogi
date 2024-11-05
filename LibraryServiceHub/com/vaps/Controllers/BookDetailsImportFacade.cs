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
    public class BookDetailsImportFacade : Controller
    {
        public BookDetailsImportInterface _ObjInter;

        public BookDetailsImportFacade(BookDetailsImportInterface para)
        {
            _ObjInter = para;
        }

        [Route("save_excel_data")]
        public Task<BookDetailsImportDTO> save_excel_data([FromBody] BookDetailsImportDTO data)
        {
            return _ObjInter.save_excel_data(data);
        }
        [Route("checkvalidation")]
        public Task<BookDetailsImportDTO> checkvalidation([FromBody] BookDetailsImportDTO data)
        {
            return _ObjInter.checkvalidation(data);
        }
    }
}
