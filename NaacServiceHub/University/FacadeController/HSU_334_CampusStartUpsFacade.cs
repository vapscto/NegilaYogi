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
    public class HSU_334_CampusStartUpsFacade : Controller
    {
        public HSU_334_CampusStartUpsInterface _inter;
        public HSU_334_CampusStartUpsFacade(HSU_334_CampusStartUpsInterface i)
        {
            _inter = i;
        }

        //[HttpPost]
        [Route("loaddata")]
        public HSU_334_CampusStartUpsDTO loaddata([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_334_CampusStartUpsDTO save([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_334_CampusStartUpsDTO deactive([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_334_CampusStartUpsDTO EditData([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public HSU_334_CampusStartUpsDTO deleteuploadfile([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public HSU_334_CampusStartUpsDTO viewuploadflies([FromBody] HSU_334_CampusStartUpsDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
