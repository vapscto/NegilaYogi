using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Interfaces
{
  public  interface IVRSRechargeInterface
    {
        IVRS_Acc_RechargeDTO getdetails(IVRS_Acc_RechargeDTO data);
        IVRS_Acc_RechargeDTO savedetail(IVRS_Acc_RechargeDTO data);
        IVRS_Acc_RechargeDTO getdetails_page(int id);
        IVRS_Acc_RechargeDTO deactivate(IVRS_Acc_RechargeDTO data);
    }
}
