using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class MasterClgFeePrevilegeFacade : Controller
    {
        public MasterClgFeePrevilegeInterface _feegrouppage;

        public MasterClgFeePrevilegeFacade(MasterClgFeePrevilegeInterface maspag)
        {
            _feegrouppage = maspag;
        }


        [Route("getpagedetails")]

        public MasterClgFeePrevilegeDTO getpagedetails([FromBody] MasterClgFeePrevilegeDTO org)
        {
            // id = 12;
            return _feegrouppage.getdetails(org);

        }

        [Route("getusername")]
        public MasterClgFeePrevilegeDTO getusername([FromBody] MasterClgFeePrevilegeDTO data)
        {
            return _feegrouppage.getusername(data);
        }

        [Route("delete/{id:int}")]
        public MasterClgFeePrevilegeDTO delete(int id)
        {
            return _feegrouppage.delete(id);
        }

        [Route("savedetail")]
        public MasterClgFeePrevilegeDTO Post([FromBody] MasterClgFeePrevilegeDTO org)
        {
            return _feegrouppage.savedetail(org);
        }

        [Route("edit/{id:int}")]
        public MasterClgFeePrevilegeDTO edit(int id)
        {
            return _feegrouppage.edit(id);
        }

    }
}
