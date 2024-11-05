using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CLGFeeWaivedOffInterface
    {
        CLGFeeWaivedOffDTO getalldetails(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO get_students(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO get_groups(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO get_heads(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO savedata(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO EditDetails(CLGFeeWaivedOffDTO data);
        CLGFeeWaivedOffDTO DeletRecord(CLGFeeWaivedOffDTO data);       
    }
}
