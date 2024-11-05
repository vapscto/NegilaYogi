using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeArrearRegisterReportInterface
    {


        FeeArrearRegisterReportDTO getdata123(FeeArrearRegisterReportDTO data);

        FeeArrearRegisterReportDTO getsection(FeeArrearRegisterReportDTO data);
        FeeArrearRegisterReportDTO getstudent(FeeArrearRegisterReportDTO data);
        FeeArrearRegisterReportDTO getstuddet(FeeArrearRegisterReportDTO data);
        Task<FeeArrearRegisterReportDTO> getreport(FeeArrearRegisterReportDTO data);

        FeeArrearRegisterReportDTO get_groups(FeeArrearRegisterReportDTO data);
    }
}
