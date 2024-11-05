using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface ClgBookTransactionReportInterface
    {

        CLGBookTransactionDTO getdetails(CLGBookTransactionDTO id);
        Task<CLGBookTransactionDTO> get_report(CLGBookTransactionDTO data);

    }
}
