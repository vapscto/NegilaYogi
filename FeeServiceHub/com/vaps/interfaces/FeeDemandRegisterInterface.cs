using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeDemandRegisterInterface
    {
       Task<FeeDemandRegisterDTO> getinitialdata(FeeDemandRegisterDTO data);
        Task<FeeDemandRegisterDTO> getStudentByYrClsSec(FeeDemandRegisterDTO data);
       Task<FeeDemandRegisterDTO> getgroupByCG(FeeDemandRegisterDTO data);
       Task<FeeDemandRegisterDTO> getReport(FeeDemandRegisterDTO data);
        


    }
}
