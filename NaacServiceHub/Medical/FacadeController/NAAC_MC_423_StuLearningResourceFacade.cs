using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class NAAC_MC_423_StuLearningResourceFacade : Controller
    {
        public NAAC_MC_423_StuLearningResourceInterface inter;
        public NAAC_MC_423_StuLearningResourceFacade(NAAC_MC_423_StuLearningResourceInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_MC_423_StuLearningResource_DTO loaddata([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_423_StuLearningResource_DTO save([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.save(data);
        }
       
        [Route("EditData")]

        public NAAC_MC_423_StuLearningResource_DTO EditData([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_423_StuLearningResource_DTO viewuploadflies([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_MC_423_StuLearningResource_DTO deleteuploadfile([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.deleteuploadfile(data);
        }
        [Route("loaddatainfra")]
        public NAAC_MC_423_StuLearningResource_DTO loaddatainfra([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.loaddatainfra(data);
        }
        [Route("saveinfra")]
        public NAAC_MC_423_StuLearningResource_DTO saveinfra([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.saveinfra(data);
        }

        [Route("EditDatainfra")]

        public NAAC_MC_423_StuLearningResource_DTO EditDatainfra([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            return inter.EditDatainfra(data);
        }

    }
}
