using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
  public interface Naac_HSU_CR4ReportInterface
    {
        Naac_HSU_CR4Report_DTO loaddata(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSUclinicalinfra423Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> ClinicalLabReport(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSUMembership433Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSU_ExpenditureBook434Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSUEcontent435Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSUClassSeminarhall441Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSUBandwidthRangeReport(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSU_PhyAcaFacility451Report(Naac_HSU_CR4Report_DTO data);
        Task<Naac_HSU_CR4Report_DTO> HSU_Infrastructureexpenditure414Report(Naac_HSU_CR4Report_DTO data);
    }
}
