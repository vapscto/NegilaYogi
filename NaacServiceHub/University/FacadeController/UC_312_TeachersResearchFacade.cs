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
    public class UC_312_TeachersResearchFacade : Controller
    {
        public UC_312_TeachersResearchInterface _Interface;
        public UC_312_TeachersResearchFacade(UC_312_TeachersResearchInterface para)
        {
            _Interface = para;
        }

        [Route("loaddata")]
        public UC_312_TeachersResearchDTO loaddata([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("save")]
        public UC_312_TeachersResearchDTO save([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactive")]
        public UC_312_TeachersResearchDTO deactive([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.deactive(data);
        }

        [Route("EditData")]
        public UC_312_TeachersResearchDTO EditData([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.EditData(data);
        }

        [Route("deleteuploadfile")]
        public UC_312_TeachersResearchDTO deleteuploadfile([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public UC_312_TeachersResearchDTO viewuploadflies([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.viewuploadflies(data);
        }

        [Route("get_dept")]
        public UC_312_TeachersResearchDTO get_dept([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_dept(data);
        }

        [Route("get_emp")]
        public UC_312_TeachersResearchDTO get_emp([FromBody] UC_312_TeachersResearchDTO data)
        {
            return _Interface.get_emp(data);
        }
    }
}
