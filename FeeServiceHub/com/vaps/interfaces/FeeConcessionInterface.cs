using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeConcessionInterface
    {
        FeeConcessionDTO getdata(FeeConcessionDTO data);

        FeeConcessionDTO selectcatorclass(FeeConcessionDTO data);
        FeeConcessionDTO fillheaddetailsss(FeeConcessionDTO data);

        FeeConcessionDTO fillamount(FeeConcessionDTO data);

        FeeConcessionDTO savedatadelegate(FeeConcessionDTO data);

        FeeConcessionDTO deleteconcess(FeeConcessionDTO data);
        FeeConcessionDTO EditconcessionDetails(FeeConcessionDTO data);

        FeeConcessionDTO filstaff(FeeConcessionDTO data);

        FeeConcessionDTO getacademir(FeeConcessionDTO data);

        FeeConcessionDTO checkpaiddetails(FeeConcessionDTO data);
        FeeConcessionDTO searchfilter(FeeConcessionDTO data);

    }
}
