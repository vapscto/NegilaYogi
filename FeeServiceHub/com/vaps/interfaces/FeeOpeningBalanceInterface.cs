using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeOpeningBalanceInterface
    {


        FeeOpeningBalanceDTO getdata123(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getstuddet(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getrefund(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getclshead(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getgroup(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO gethead(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO filterstudents(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getlisttwo(FeeOpeningBalanceDTO stu);
        // Task<FeeOpeningBalanceDTO> getreport(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO getreport(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO DeleteEntry(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO searching(FeeOpeningBalanceDTO data);
        FeeOpeningBalanceDTO onselectacademicyear(FeeOpeningBalanceDTO data);
        
    }
}
