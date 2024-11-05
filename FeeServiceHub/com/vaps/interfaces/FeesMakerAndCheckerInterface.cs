using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeesMakerAndCheckerInterface
    {
        FeesMakerAndCheckerDTO getdetails(FeesMakerAndCheckerDTO data);
        FeesMakerAndCheckerDTO Getreportdetails(FeesMakerAndCheckerDTO data);

        FeesMakerAndCheckerDTO savedetails(FeesMakerAndCheckerDTO data);
    }
}
