using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface Naac_HSU_CR6ReportInterface
    {
        Naac_HSU_CR6Report_DTO loaddata(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUEGovernance623Report(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUFinancialSupport632Report(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms633Report(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUDevPrograms634Report(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUGovtFunding642Report(Naac_HSU_CR6Report_DTO data);
        Task<Naac_HSU_CR6Report_DTO> HSUQualityAssurance652Report(Naac_HSU_CR6Report_DTO data);
       
     

    }
}
