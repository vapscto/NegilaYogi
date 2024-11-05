using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface Fee_PaymentManagementInterface
    {
        Fee_Payment_ManagementDTO getFee_PaymentManagement(Fee_Payment_ManagementDTO dto);
    }
}
