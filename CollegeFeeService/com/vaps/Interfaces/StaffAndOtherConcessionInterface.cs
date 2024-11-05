using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface StaffAndOtherConcessionInterface
    {
        CollegeConcessionDTO getdata(CollegeConcessionDTO data);

        CollegeConcessionDTO selectcatorclass(CollegeConcessionDTO data);
        CollegeConcessionDTO fillheaddetailsss(CollegeConcessionDTO data);

        CollegeConcessionDTO fillamount(CollegeConcessionDTO data);

        CollegeConcessionDTO savedatadelegate(CollegeConcessionDTO data);

        CollegeConcessionDTO deleteconcess(CollegeConcessionDTO data);
        CollegeConcessionDTO EditconcessionDetails(CollegeConcessionDTO data);

        CollegeConcessionDTO filstaff(CollegeConcessionDTO data);

        CollegeConcessionDTO getacademir(CollegeConcessionDTO data);

        CollegeConcessionDTO checkpaiddetails(CollegeConcessionDTO data);
    }
}
