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
    public class NAAC_MC_443_BandWidth_RangeFacade : Controller
    {
        public NAAC_MC_443_BandWidth_RangeInterface inter;
        public NAAC_MC_443_BandWidth_RangeFacade(NAAC_MC_443_BandWidth_RangeInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public NAAC_MC_443_BandWidth_Range_DTO loaddata([FromBody] NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_443_BandWidth_Range_DTO save([FromBody] NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return inter.save(data);
        }
      
        [Route("EditData")]

        public NAAC_MC_443_BandWidth_Range_DTO EditData([FromBody] NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return inter.EditData(data);
        }

       
    }
}
