using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeCardDetailsEntryInterface
    {
        FeeCardDetailEntryDTO getdata(FeeCardDetailEntryDTO data);        
        FeeCardDetailEntryDTO savedata(FeeCardDetailEntryDTO data);       
        FeeCardDetailEntryDTO getsearchfilter(FeeCardDetailEntryDTO data);
        FeeCardDetailEntryDTO getstudlistgroup(FeeCardDetailEntryDTO data);
        FeeCardDetailEntryDTO getgroupmappedheads(FeeCardDetailEntryDTO data);
        FeeCardDetailEntryDTO editdetails(int id);
        FeeCardDetailEntryDTO Deletedetails(int id);
    }
}
