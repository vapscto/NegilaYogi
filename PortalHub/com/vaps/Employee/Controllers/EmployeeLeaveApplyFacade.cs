using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeLeaveApplyFacade : Controller
    {

        public EmployeeLeaveApplyInterface _leave;
        public EmployeeLeaveApplyFacade(EmployeeLeaveApplyInterface leave)
        {
            _leave = leave;
        }
        [HttpPost]

        [Route("getonlineLeave")]
        public EmployeeDashboardDTO getonlineLeave([FromBody] EmployeeDashboardDTO data)
        {
            return _leave.getonlineLeave(data);
        }
        [Route("save")]

        public EmployeeDashboardDTO saveLeave([FromBody] EmployeeDashboardDTO data)
        {
            return _leave.saveonlineLeave(data);
        }

        //======================== TC Class teacher approval

        [Route("getdata_CTA")]
        public Adm_TC_Approval_DTO getdata_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getdata_CTA(dto);
        }

        [Route("SaveEdit_CTA")]
        public Adm_TC_Approval_DTO SaveEdit_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.SaveEdit_CTA(dto);
        }
        [Route("details_CTA")]
        public Adm_TC_Approval_DTO details_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.details_CTA(dto);
        }
        [Route("deactivate_CTA")]
        public Adm_TC_Approval_DTO deactivate_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.deactivate_CTA(dto);
        }

        [Route("getstudetails_CTA")]
        public Adm_TC_Approval_DTO getstudetails_CTA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getstudetails_CTA(dto);
        }
         //======================== TC Library approval

        [Route("getdata_LIB")]
        public Adm_TC_Approval_DTO getdata_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getdata_LIB(dto);
        }

        [Route("SaveEdit_LIB")]
        public Adm_TC_Approval_DTO SaveEdit_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.SaveEdit_LIB(dto);
        }
        [Route("details_LIB")]
        public Adm_TC_Approval_DTO details_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.details_LIB(dto);
        }
        [Route("deactivate_LIB")]
        public Adm_TC_Approval_DTO deactivate_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.deactivate_LIB(dto);
        }
        [Route("getstudetails_LIB")]
        public Adm_TC_Approval_DTO getstudetails_LIB([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getstudetails_LIB(dto);
        }
        //======================== TC FEE approval

        [Route("getdata_FEE")]
        public Adm_TC_Approval_DTO getdata_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getdata_FEE(dto);
        }

        [Route("SaveEdit_FEE")]
        public Adm_TC_Approval_DTO SaveEdit_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.SaveEdit_FEE(dto);
        }
        [Route("details_FEE")]
        public Adm_TC_Approval_DTO details_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.details_FEE(dto);
        }
        [Route("deactivate_FEE")]
        public Adm_TC_Approval_DTO deactivate_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.deactivate_FEE(dto);
        }
        [Route("getstudetails_FEE")]
        public Adm_TC_Approval_DTO getstudetails_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getstudetails_FEE(dto);
        }
         [Route("feeheaddetails_FEE")]
        public Adm_TC_Approval_DTO feeheaddetails_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.feeheaddetails_FEE(dto);
        }
        [Route("feenot_approval_FEE")]
        public Adm_TC_Approval_DTO feenot_approval_FEE([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.feenot_approval_FEE(dto);
        }

        //======================== FDA Class teacher approval

        [Route("getdata_PDA")]
        public Adm_TC_Approval_DTO getdata_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getdata_PDA(dto);
        }

        [Route("SaveEdit_PDA")]
        public Adm_TC_Approval_DTO SaveEdit_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.SaveEdit_PDA(dto);
        }
        [Route("details_PDA")]
        public Adm_TC_Approval_DTO details_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.details_PDA(dto);
        }
        [Route("deactivate_PDA")]
        public Adm_TC_Approval_DTO deactivate_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.deactivate_PDA(dto);
        }

        [Route("getstudetails_PDA")]
        public Adm_TC_Approval_DTO getstudetails_PDA([FromBody] Adm_TC_Approval_DTO dto)
        {
            return _leave.getstudetails_PDA(dto);
        }

    }
}
