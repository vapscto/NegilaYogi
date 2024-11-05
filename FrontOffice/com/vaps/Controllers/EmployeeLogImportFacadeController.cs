using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeLogImportFacadeController : Controller
    {       
      
            public EmployeeLogImportInterface _objInter;
            public EmployeeLogImportFacadeController(EmployeeLogImportInterface data)
            {
                _objInter = data;
            }

            [Route("Savedata")]
            public EmployeeLogImportDTO Savedata([FromBody] EmployeeLogImportDTO data)
            {
                return _objInter.Savedata(data);
            }

        }
    }
