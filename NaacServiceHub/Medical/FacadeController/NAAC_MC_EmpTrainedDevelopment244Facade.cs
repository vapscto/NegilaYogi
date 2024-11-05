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
    public class NAAC_MC_EmpTrainedDevelopment244Facade : Controller
    {

        public NAAC_MC_EmpTrainedDevelopment244Interface _Interface;
        public NAAC_MC_EmpTrainedDevelopment244Facade(NAAC_MC_EmpTrainedDevelopment244Interface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO loaddata([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO save([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.save(data);
        }
        [Route("deactive")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO deactive([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.deactive(data);
        }
        [Route("EditData")]

        public NAAC_MC_EmpTrainedDevelopment244_DTO EditData([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.EditData(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO viewuploadflies([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO deleteuploadfile([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO savemedicaldatawisecomments([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO savefilewisecomments([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO getcomment([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_MC_EmpTrainedDevelopment244_DTO getfilecomment([FromBody] NAAC_MC_EmpTrainedDevelopment244_DTO data)
        {
            return _Interface.getfilecomment(data);
        }
    }
}
