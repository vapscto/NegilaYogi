using PreadmissionDTOs.com.vaps.Portals.IVRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.IVRM.Interfaces
{
   public interface Clg_IVRM_InteractionsInterface
    {

        Task<IVRM_School_InteractionsDTO> getloaddata(IVRM_School_InteractionsDTO data);
        Task<IVRM_School_InteractionsDTO> getdetails(IVRM_School_InteractionsDTO data);
        Task<IVRM_School_InteractionsDTO> getstudent(IVRM_School_InteractionsDTO data);
        IVRM_School_InteractionsDTO Getbranch(IVRM_School_InteractionsDTO data);
        IVRM_School_InteractionsDTO Getsemester(IVRM_School_InteractionsDTO data);
        IVRM_School_InteractionsDTO Getsection(IVRM_School_InteractionsDTO data);
        IVRM_School_InteractionsDTO savedetails(IVRM_School_InteractionsDTO data);
        IVRM_School_InteractionsDTO savereply(IVRM_School_InteractionsDTO data);
        Task<IVRM_School_InteractionsDTO> reply(IVRM_School_InteractionsDTO data);
       IVRM_School_InteractionsDTO deletemsg(IVRM_School_InteractionsDTO data);
       IVRM_School_InteractionsDTO deleteinboxmsg(IVRM_School_InteractionsDTO data);
       IVRM_School_InteractionsDTO seen(IVRM_School_InteractionsDTO data);

    }
}
