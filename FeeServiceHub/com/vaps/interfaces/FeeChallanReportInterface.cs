using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
namespace FeeServiceHub.com.vaps.interfaces
{
  public  interface FeeChallanReportInterface 
    {


        FeeChallanReportDTO getdata123(FeeChallanReportDTO data);
      //  FeeChallanReportDTO getstuddet(FeeChallanReportDTO data);
        Task<FeeChallanReportDTO> getreport(FeeChallanReportDTO data);
        FeeChallanReportDTO getinstallment(FeeChallanReportDTO data);
        FeeChallanReportDTO generateHutchingChallan(FeeChallanReportDTO data);
        FeeChallanReportDTO checkforchallan(FeeChallanReportDTO data);
        FeeChallanReportDTO getChallandetails(FeeChallanReportDTO data); 
         FeeChallanReportDTO delrec(FeeChallanReportDTO data);
        FeeChallanReportDTO getstudlistgroup(FeeChallanReportDTO data);
        FeeStudentTransactionDTO searching(FeeStudentTransactionDTO data);
    }
}
