using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NaacEGovernance623Facade : Controller
    {
        public NaacEGovernance623Interface _Interface;
        public NaacEGovernance623Facade(NaacEGovernance623Interface i)
        {
            _Interface = i;
        }

        [Route("loaddata")]
        public NAAC_AC_623_EGovernance_DTO loaddata([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAAC_AC_623_EGovernance_DTO save([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.save(data);
        }
        [Route("deactive")]
        public NAAC_AC_623_EGovernance_DTO deactive([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.deactive(data);
        }
        [Route("EditData")]
        public NAAC_AC_623_EGovernance_DTO EditData([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.EditData(data);
        }



        [Route("deleteuploadfile")]
        public NAAC_AC_623_EGovernance_DTO deleteuploadfile([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_623_EGovernance_DTO viewuploadflies([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_623_EGovernance_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_623_EGovernance_DTO savefilewisecomments([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_623_EGovernance_DTO getcomment([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_623_EGovernance_DTO getfilecomment([FromBody] NAAC_AC_623_EGovernance_DTO data)
        {
            return _Interface.getfilecomment(data);
        }


    }
}
