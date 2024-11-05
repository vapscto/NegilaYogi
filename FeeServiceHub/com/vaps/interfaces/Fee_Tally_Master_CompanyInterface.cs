using PreadmissionDTOs.com.vaps.Fees.Tally;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface Fee_Tally_Master_CompanyInterface
    {
        Fee_Tally_Master_CompanyDTO loaddata(Fee_Tally_Master_CompanyDTO data);
        Fee_Tally_Master_CompanyDTO savedata(Fee_Tally_Master_CompanyDTO data);
        Fee_Tally_Master_CompanyDTO deletedata(Fee_Tally_Master_CompanyDTO data);
    }
}
