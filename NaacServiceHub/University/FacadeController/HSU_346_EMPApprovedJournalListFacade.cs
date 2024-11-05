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
    public class HSU_346_EMPApprovedJournalListFacade : Controller
    {
        public HSU_346_EMPApprovedJournalListInterface inter;
        public HSU_346_EMPApprovedJournalListFacade(HSU_346_EMPApprovedJournalListInterface q)
        {
            inter = q;
        }

        [Route("loaddata")]
        public HSU_346_EMPApprovedJournalList_DTO loaddata([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("save")]
        public HSU_346_EMPApprovedJournalList_DTO save([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.save(data);
        }
        [Route("deactive")]
        public HSU_346_EMPApprovedJournalList_DTO deactive([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.deactive(data);
        }
        [Route("EditData")]

        public HSU_346_EMPApprovedJournalList_DTO EditData([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("viewuploadflies")]
        public HSU_346_EMPApprovedJournalList_DTO viewuploadflies([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public HSU_346_EMPApprovedJournalList_DTO deleteuploadfile([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return inter.deleteuploadfile(data);
        }
    }
}
