using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class HSU_352_RevenueGeneratedFacade : Controller
    {
        public HSU_352_RevenueGeneratedInterface _inter;
        public HSU_352_RevenueGeneratedFacade(HSU_352_RevenueGeneratedInterface i)
        {
            _inter = i;
        }

        [Route("loaddata")]
        public HSU_352_RevenueGeneratedDTO loaddata([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_352_RevenueGeneratedDTO save([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_352_RevenueGeneratedDTO deactive([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_352_RevenueGeneratedDTO EditData([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.EditData(data);
        }
        [Route("deleteuploadfile")]
        public HSU_352_RevenueGeneratedDTO deleteuploadfile([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public HSU_352_RevenueGeneratedDTO viewuploadflies([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
