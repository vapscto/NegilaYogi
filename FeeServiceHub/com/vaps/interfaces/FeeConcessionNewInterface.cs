using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeConcessionNewInterface
    {
        FeeConcessionDTO getdata(FeeConcessionDTO data);


        FeeConcessionDTO savedatadelegate(FeeConcessionDTO data);
        FeeConcessionDTO fillamount(FeeConcessionDTO data);

        FeeConcessionDTO deleteconcess(FeeConcessionDTO data);
        FeeConcessionDTO EditconcessionDetails(FeeConcessionDTO data);

      

        FeeConcessionDTO getfeegroup(FeeConcessionDTO data);
        FeeConcessionDTO getfeehead(FeeConcessionDTO data);
        FeeConcessionDTO getterm(FeeConcessionDTO data);
        FeeConcessionDTO getinstallment(FeeConcessionDTO data);
        FeeConcessionDTO studentdata(FeeConcessionDTO data);
        FeeConcessionDTO studentdata1(FeeConcessionDTO data);

        FeeConcessionDTO Save(FeeConcessionDTO data);
    }
}
