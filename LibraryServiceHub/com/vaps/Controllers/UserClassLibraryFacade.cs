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
    public class UserClassLibraryFacade : Controller
    {
        // GET: api/<controller>
      public  UserClassLibraryInterface _objInter;

        public UserClassLibraryFacade(UserClassLibraryInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails/{id:int}")]
        public LIB_Library_Class_DTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }


    }
}
