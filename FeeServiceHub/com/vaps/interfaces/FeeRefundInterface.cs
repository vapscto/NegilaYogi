using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeRefundInterface
    {

       // Task<FeeRefundDTO > getalldetails(int id);
        FeeRefundDTO getalldetails(FeeRefundDTO data);
        FeeRefundDTO getsection(FeeRefundDTO data);
        FeeRefundDTO getstudent(FeeRefundDTO data);
      
        Task<FeeRefundDTO> getreport(FeeRefundDTO data);
    }
}
