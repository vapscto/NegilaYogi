using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using VidyaBharathiServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VidyaBharathiServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VBSC_MasterCompetition_CategoryFacade : Controller
    {
        public VBSC_MasterCompetition_CategoryInterface _cms;

        public VBSC_MasterCompetition_CategoryFacade(VBSC_MasterCompetition_CategoryInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public VBSC_MasterCompetition_CategoryDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            
        }
        //getdata
        [HttpPost]
        [Route("savedata")]
        public VBSC_MasterCompetition_CategoryDTO savedata([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            return _cms.savedata(data);
        }
        //Deactivate
        [Route("Deactivate")]
        public VBSC_MasterCompetition_CategoryDTO Deactivate([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            return _cms.Deactivate(data);
        }
        //Organsation
        [Route("Organsation")]
        public VBSC_MasterCompetition_CategoryDTO Organsation([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            return _cms.Organsation(data);
        }
        //savedataCl
        [Route("savedataCl")]
        public Master_Competition_Category_ClassesDTO savedataCl([FromBody]Master_Competition_Category_ClassesDTO data)
        {
            return _cms.savedataCl(data);
        }
        [Route("DeactivateCl")]
        public Master_Competition_Category_ClassesDTO DeactivateCl([FromBody]Master_Competition_Category_ClassesDTO data)
        {
            return _cms.DeactivateCl(data);
        }
        [HttpGet]
        [Route("getdata/{id:int}")]
        public VBSC_Master_Competition_Category_LevelsDTO getdata(int id)
        {
            return _cms.getdata(id);

        }
        [HttpPost]
        [Route("savedataVCl")]
        public VBSC_Master_Competition_Category_LevelsDTO savedataVCl([FromBody]VBSC_Master_Competition_Category_LevelsDTO data)
        {
            return _cms.savedataVCl(data);
        }
        [Route("DeactivateVCl")]
        public VBSC_Master_Competition_Category_LevelsDTO DeactivateVCl([FromBody]VBSC_Master_Competition_Category_LevelsDTO data)
        {
            return _cms.DeactivateVCl(data);
        }

        //getdata
    }
}
