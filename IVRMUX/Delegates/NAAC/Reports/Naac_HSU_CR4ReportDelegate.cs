using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class Naac_HSU_CR4ReportDelegate
    {
        CommonDelegate<Naac_HSU_CR4Report_DTO, Naac_HSU_CR4Report_DTO> comm = new CommonDelegate<Naac_HSU_CR4Report_DTO, Naac_HSU_CR4Report_DTO>();
        public Naac_HSU_CR4Report_DTO loaddata(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/loaddata");
        }
        public Naac_HSU_CR4Report_DTO HSUclinicalinfra423Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSUclinicalinfra423Report");
        }
        public Naac_HSU_CR4Report_DTO ClinicalLabReport(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/ClinicalLabReport");
        }
        public Naac_HSU_CR4Report_DTO HSUMembership433Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSUMembership433Report");
        }
        public Naac_HSU_CR4Report_DTO HSU_ExpenditureBook434Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSU_ExpenditureBook434Report");
        }
        public Naac_HSU_CR4Report_DTO HSUEcontent435Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSUEcontent435Report");
        }
        public Naac_HSU_CR4Report_DTO HSUClassSeminarhall441Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSUClassSeminarhall441Report");
        }
        public Naac_HSU_CR4Report_DTO HSUBandwidthRangeReport(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSUBandwidthRangeReport");
        }
        public Naac_HSU_CR4Report_DTO HSU_PhyAcaFacility451Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSU_PhyAcaFacility451Report");
        }
        public Naac_HSU_CR4Report_DTO HSU_Infrastructureexpenditure414Report(Naac_HSU_CR4Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR4ReportFacade/HSU_Infrastructureexpenditure414Report");
        }

    }
}
