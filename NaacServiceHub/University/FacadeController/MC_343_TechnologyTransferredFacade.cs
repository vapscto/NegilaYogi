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
    public class MC_343_TechnologyTransferredFacade : Controller
    {
       
            public MC_343_TechnologyTransferredInterface _inter;
            public MC_343_TechnologyTransferredFacade(MC_343_TechnologyTransferredInterface i)
            {
                _inter = i;
            }

            [Route("loaddata")]
            public MC_343_TechnologyTransferredDTO loaddata([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.loaddata(data);
            }
            [Route("save")]
            public MC_343_TechnologyTransferredDTO save([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.save(data);
            }
            [Route("deactive")]
            public MC_343_TechnologyTransferredDTO deactive([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.deactive(data);
            }
            [Route("EditData")]
            public MC_343_TechnologyTransferredDTO EditData([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.EditData(data);
            }
            
            [Route("deleteuploadfile")]
            public MC_343_TechnologyTransferredDTO deleteuploadfile([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.deleteuploadfile(data);
            }

            [Route("viewuploadflies")]
            public MC_343_TechnologyTransferredDTO viewuploadflies([FromBody] MC_343_TechnologyTransferredDTO data)
            {
                return _inter.viewuploadflies(data);
            }
        }
}
