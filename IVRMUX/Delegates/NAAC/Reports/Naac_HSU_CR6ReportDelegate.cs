using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class Naac_HSU_CR6ReportDelegate
    {
        
        CommonDelegate<Naac_HSU_CR6Report_DTO, Naac_HSU_CR6Report_DTO> comm = new CommonDelegate<Naac_HSU_CR6Report_DTO, Naac_HSU_CR6Report_DTO>();
        public Naac_HSU_CR6Report_DTO loaddata(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/loaddata");
        }
        public Naac_HSU_CR6Report_DTO HSUEGovernance623Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUEGovernance623Report");
        }
        public Naac_HSU_CR6Report_DTO HSUFinancialSupport632Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUFinancialSupport632Report");
        }
        public Naac_HSU_CR6Report_DTO HSUDevPrograms633Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUDevPrograms633Report");
        }
        public Naac_HSU_CR6Report_DTO HSUGovtFunding642Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUGovtFunding642Report");
        }
        public Naac_HSU_CR6Report_DTO HSUDevPrograms634Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUDevPrograms634Report");
        }
        public Naac_HSU_CR6Report_DTO HSUQualityAssurance652Report(Naac_HSU_CR6Report_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_HSU_CR6ReportFacade/HSUQualityAssurance652Report");
        }
    }
}
