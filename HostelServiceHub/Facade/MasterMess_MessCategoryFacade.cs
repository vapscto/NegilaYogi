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
    public class MasterMess_MessCategoryFacade : Controller
    {

        public MasterMess_MessCategoryInterface _Interface;


        public MasterMess_MessCategoryFacade(MasterMess_MessCategoryInterface parameter)
        {
            _Interface = parameter;
        }
        [Route("get_Mmessdata")]
        public MasterMess_MessCategoryDTO get_Mmessdata([FromBody] MasterMess_MessCategoryDTO data)
        {
            return _Interface.get_Mmessdata(data);
        }

        [Route("save_Mmessdata")]
        public HL_Master_Mess_DTO save_Mmessdata([FromBody] HL_Master_Mess_DTO data)
        {
            return _Interface.save_Mmessdata(data);
        }

        [Route("edit_Mmessdata")]
        public MasterMess_MessCategoryDTO edit_Mmessdata([FromBody] MasterMess_MessCategoryDTO data)
        {
            return _Interface.edit_Mmessdata(data);
        }

        [Route("deactiveY_Mmessdata")]
        public MasterMess_MessCategoryDTO deactiveY_Mmessdata([FromBody] MasterMess_MessCategoryDTO data)
        {
            return _Interface.deactiveY_Mmessdata(data);
        }

    }
}
