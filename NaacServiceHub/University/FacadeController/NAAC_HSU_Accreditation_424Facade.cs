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
    public class NAAC_HSU_Accreditation_424Facade : Controller
    {
        public NAAC_HSU_Accreditation_424Interface inter;
        public NAAC_HSU_Accreditation_424Facade(NAAC_HSU_Accreditation_424Interface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_HSU_Accreditation_424_DTO loaddata([FromBody] NAAC_HSU_Accreditation_424_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_HSU_Accreditation_424_DTO save([FromBody] NAAC_HSU_Accreditation_424_DTO data)
        {
            return inter.save(data);
        }
      
        [Route("EditData")]

        public NAAC_HSU_Accreditation_424_DTO EditData([FromBody] NAAC_HSU_Accreditation_424_DTO data)
        {
            return inter.EditData(data);
        }

       
    }
}
