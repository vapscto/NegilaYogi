using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeWizardFacade : Controller

    {
        public FeeWizardInterface _feegrouppage;

        public FeeWizardFacade(FeeWizardInterface maspag)
        {
            _feegrouppage = maspag;
        }
        [Route("getdetails")]
        public FeeWizardDTO getorgdet([FromBody] FeeWizardDTO data)
        {
            return _feegrouppage.getdetails(data);
        }
        [HttpPost]
        [Route("SaveYearlyGrpdata/")]
        public FeeWizardDTO SaveYearlyGrpdata([FromBody] FeeWizardDTO reg)
        {


            return _feegrouppage.SaveYearlyGroupData(reg);
           
           

           
        }


        [Route("changacademicyear")]
        public FeeWizardDTO changacademicyear([FromBody] FeeWizardDTO data)
        {
            return _feegrouppage.changacademicyear(data);
        }
        [HttpPost]
        [Route("deactivateY")]
        public FeeWizardDTO deactivateY([FromBody] FeeWizardDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivateY(id);
        }

        [HttpPost]
        [Route("savedetailsFGH")]
        public FeeWizardDTO savedetailsFGH([FromBody] FeeWizardDTO reg)
        {
            return _feegrouppage.savedetailsFGH(reg);
            
        }
        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public FeeWizardDTO Delete(int id)
        {
            return _feegrouppage.deleterec(id);
        }

        [HttpPost]
        [Route("savedetailYCC")]
        public FeeWizardDTO savedetailYCC([FromBody] FeeWizardDTO reg)
        {
            return _feegrouppage.savedetailYCC(reg);

        }

        [HttpDelete]
        [Route("deletedetailsY/{id:int}")]
        public FeeWizardDTO DeleterecY(int id)
        {
            return _feegrouppage.deleterecY(id);
        }
        [HttpPost]
        [Route("savedetailFMA")]
        public FeeWizardDTO savedetailFMA([FromBody] FeeWizardDTO reg)
        {
            return _feegrouppage.savedetailFMA(reg);

        }
        [HttpPost]
        [Route("savedetailFMAG")]
        public FeeWizardDTO savedetailFMAG([FromBody] FeeWizardDTO reg)
        {
            return _feegrouppage.savedetailFMAG(reg);

        }


        [Route("deletemodpages")]
        public FeeWizardDTO Delete([FromBody] FeeWizardDTO data)
        {
            return _feegrouppage.deleterecfma(data);
        }
        [Route("delete")]
        public FeeWizardDTO deletedata([FromBody] FeeWizardDTO data)
        {
            return _feegrouppage.deletedta(data);
        }

    }
}
