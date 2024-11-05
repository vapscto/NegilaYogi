using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegePortals.com.Chairman.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePortals.com.Chairman.Controllers
{
    [Route("api/[controller]")]
    public class Clg_ClassDetailsFacade : Controller
    {
        public Clg_ClassDetailsInterface inter;
        public Clg_ClassDetailsFacade(Clg_ClassDetailsInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public Clg_ClassDetails_DTO loaddata([FromBody] Clg_ClassDetails_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("getcourse")]
        public Clg_ClassDetails_DTO getcourse([FromBody] Clg_ClassDetails_DTO data)
        {
            return inter.getcourse(data);
        }
        [Route("report")]
        public Clg_ClassDetails_DTO report([FromBody] Clg_ClassDetails_DTO data)
        {
            return inter.report(data);
        }
    }
}
