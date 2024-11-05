using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Documents.Interface;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Documents.FacadeController
{
    [Route("api/[controller]")]
    public class NaacMarksSlabFacade : Controller
    {
        public NaacMarksSlabInterface _Interface;
        public NaacMarksSlabFacade(NaacMarksSlabInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("Getdetails")]
        public NAAC_AC_Criteria_MarksSlab_DTO Getdetails([FromBody] NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return _Interface.Getdetails(data);
        }
        [Route("savedata")]
        public NAAC_AC_Criteria_MarksSlab_DTO savedata([FromBody] NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return _Interface.savedata(data);
        }
        [Route("editdata")]
        public NAAC_AC_Criteria_MarksSlab_DTO editdata([FromBody] NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return _Interface.editdata(data);
        }
        [Route("deactive")]
        public NAAC_AC_Criteria_MarksSlab_DTO deactive([FromBody] NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            return _Interface.deactive(data);
        }
        
    }
}
