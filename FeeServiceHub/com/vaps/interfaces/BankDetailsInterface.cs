using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
     public interface BankDetailsInterface
    {
        //BankDetailsDTO save(BankDetailsDTO data);
        BankDetailsDTO getdata (BankDetailsDTO data);
        BankDetailsDTO getalldetails(BankDetailsDTO data);
        BankDetailsDTO edittab1(BankDetailsDTO data);
        BankDetailsDTO deactive(BankDetailsDTO data);
    }
}
