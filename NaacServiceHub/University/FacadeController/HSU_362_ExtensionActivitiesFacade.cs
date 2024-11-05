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
    public class HSU_362_ExtensionActivitiesFacade : Controller
    {
        public HSU_362_ExtensionActivitiesInterface _inter;
        public HSU_362_ExtensionActivitiesFacade(HSU_362_ExtensionActivitiesInterface i)
        {
            _inter = i;
        }

        [Route("loaddata")]
        public HSU_362_ExtensionActivitiesDTO loaddata([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_362_ExtensionActivitiesDTO save([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_362_ExtensionActivitiesDTO deactive([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_362_ExtensionActivitiesDTO EditData([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.EditData(data);
        }
        [Route("deleteuploadfile")]
        public HSU_362_ExtensionActivitiesDTO deleteuploadfile([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public HSU_362_ExtensionActivitiesDTO viewuploadflies([FromBody] HSU_362_ExtensionActivitiesDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
