using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class MasterHostelFacade : Controller
    {
       public MasterHostelInterface _Interface;
        public MasterHostelFacade(MasterHostelInterface parameter)
        {
            _Interface = parameter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        #region
        [Route("Page_loaddata")]
        public HL_Master_Hostel_DTO Page_loaddata([FromBody] HL_Master_Hostel_DTO data)
        {
            return _Interface.Page_loaddata(data);
        }

        [Route("Get_StateData")]
        public HL_Master_Hostel_DTO Get_StateData([FromBody] HL_Master_Hostel_DTO data)
        {
            return _Interface.Get_StateData(data);
        }

        [Route("Save_Hostel_Data")]
        public HL_Master_Hostel_DTO Save_Hostel_Data([FromBody] HL_Master_Hostel_DTO data)
        {
            return _Interface.Save_Hostel_Data(data);
        }

        [Route("Edit_Hostel_Row")]
        public HL_Master_Hostel_DTO Edit_Hostel_Row([FromBody] HL_Master_Hostel_DTO data)
        {
            return _Interface.Edit_Hostel_Row(data);
        }

        [Route("Deactive_Hostel_Row")]
        public HL_Master_Hostel_DTO Deactive_Hostel_Row([FromBody]HL_Master_Hostel_DTO data)
        {
            return _Interface.Deactive_Hostel_Row(data);
        }
        [Route("Get_MappedFacility")]
        public HL_Master_Hostel_DTO Get_MappedFacility([FromBody]HL_Master_Hostel_DTO data)
        {
            return _Interface.Get_MappedFacility(data);
        }
        [Route("Get_MappedEmpl")]
        public HL_Master_Hostel_DTO Get_MappedEmpl([FromBody]HL_Master_Hostel_DTO data)
        {
            return _Interface.Get_MappedEmpl(data);
        }
        [Route("viewuploadflies")]
        public HL_Master_Hostel_DTO viewuploadflies([FromBody]HL_Master_Hostel_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        
        [Route("deleteuploadfile")]
        public HL_Master_Hostel_DTO deleteuploadfile([FromBody]HL_Master_Hostel_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }
        
        #endregion

    }
}
