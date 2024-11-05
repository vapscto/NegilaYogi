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
    public class HSU_349_HindexFacade : Controller
    {
        public HSU_349_HindexInterface _inter;
        public HSU_349_HindexFacade(HSU_349_HindexInterface i)
        {
            _inter = i;
        }

        //[HttpPost]
        [Route("loaddata")]
        public HSU_346_EMPApprovedJournalList_DTO loaddata([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_346_EMPApprovedJournalList_DTO save([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_346_EMPApprovedJournalList_DTO deactive([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_346_EMPApprovedJournalList_DTO EditData([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public HSU_346_EMPApprovedJournalList_DTO deleteuploadfile([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public HSU_346_EMPApprovedJournalList_DTO viewuploadflies([FromBody] HSU_346_EMPApprovedJournalList_DTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
