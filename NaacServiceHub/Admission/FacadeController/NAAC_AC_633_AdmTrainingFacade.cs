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
    public class NAAC_AC_633_AdmTrainingFacade : Controller
    {
        public NAAC_AC_633_AdmTrainingInterface inter;
        public NAAC_AC_633_AdmTrainingFacade(NAAC_AC_633_AdmTrainingInterface q)
        {
            inter = q;
        }

      [Route("loaddata")] 
      public NAAC_Criteria_6_DTO loaddata([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_Criteria_6_DTO save([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactiveStudent")]
        public NAAC_Criteria_6_DTO deactiveStudent([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.deactiveStudent(data);
        }
        [Route("EditData")]

        public NAAC_Criteria_6_DTO EditData([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAAC_Criteria_6_DTO viewuploadflies([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_Criteria_6_DTO deleteuploadfile([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAAC_Criteria_6_DTO savemedicaldatawisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_Criteria_6_DTO savefilewisecomments([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_Criteria_6_DTO getcomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_Criteria_6_DTO getfilecomment([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.getfilecomment(data);
        }

    }
}
