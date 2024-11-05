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
    public class NAAC_HSU_EvaluationRelated_253Facade : Controller
    {
        public NAAC_HSU_EvaluationRelated_253Interface _inter;
        public NAAC_HSU_EvaluationRelated_253Facade(NAAC_HSU_EvaluationRelated_253Interface i)
        {
            _inter = i;
        }

        //[HttpPost]
        [Route("loaddata")]
        public NAAC_HSU_EvaluationRelated_253_DTO loaddata([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_HSU_EvaluationRelated_253_DTO save([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public NAAC_HSU_EvaluationRelated_253_DTO deactive([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public NAAC_HSU_EvaluationRelated_253_DTO EditData([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_HSU_EvaluationRelated_253_DTO deleteuploadfile([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public NAAC_HSU_EvaluationRelated_253_DTO viewuploadflies([FromBody] NAAC_HSU_EvaluationRelated_253_DTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
