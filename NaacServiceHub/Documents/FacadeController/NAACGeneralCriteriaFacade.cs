using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Documents.Interface;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NAACGeneralCriteriaFacade : Controller
    {
        public NAACGeneralCriteriaInterface _Interface;

        public NAACGeneralCriteriaFacade(NAACGeneralCriteriaInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACGeneralCriteriaDTO loaddata([FromBody] NAACGeneralCriteriaDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACGeneralCriteriaDTO save([FromBody] NAACGeneralCriteriaDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGeneralCriteriaDTO deactiveStudent([FromBody] NAACGeneralCriteriaDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACGeneralCriteriaDTO EditData([FromBody]NAACGeneralCriteriaDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACGeneralCriteriaDTO viewuploadflies([FromBody]NAACGeneralCriteriaDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACGeneralCriteriaDTO deleteuploadfile([FromBody]NAACGeneralCriteriaDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("viewlink")]
        public NAACGeneralCriteriaDTO viewlink([FromBody]NAACGeneralCriteriaDTO category)
        {
            return _Interface.viewlink(category);
        }
         [Route("deletelink")]
        public NAACGeneralCriteriaDTO deletelink([FromBody]NAACGeneralCriteriaDTO category)
        {
            return _Interface.deletelink(category);
        }

    }
}
