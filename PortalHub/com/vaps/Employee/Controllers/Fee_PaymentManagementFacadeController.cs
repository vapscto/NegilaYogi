using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Fees;

namespace PortalHub.com.vaps.Employee.Controllers
{
    
    [Route("api/[controller]")]
    public class Fee_PaymentManagementFacadeController : Controller
    {
        public Fee_PaymentManagementInterface _inter;
        public Fee_PaymentManagementFacadeController(Fee_PaymentManagementInterface inter)
        {
            _inter = inter;
        }

        [Route("getFee_PaymentManagement")]
        public Fee_Payment_ManagementDTO getFee_PaymentManagement([FromBody]Fee_Payment_ManagementDTO dto)
        {
            return _inter.getFee_PaymentManagement(dto);
        }
    }
}