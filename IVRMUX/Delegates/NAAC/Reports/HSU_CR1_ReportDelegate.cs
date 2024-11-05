using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class HSU_CR1_ReportDelegate
    {

        CommonDelegate<HSU_CR1_Report_DTO, HSU_CR1_Report_DTO> _commnbranch = new CommonDelegate<HSU_CR1_Report_DTO, HSU_CR1_Report_DTO>();

        public HSU_CR1_Report_DTO getdata(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/getdata/");
        }
        public HSU_CR1_Report_DTO HSU_112_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_112_Report/");
        }
        public HSU_CR1_Report_DTO HSU_132_133_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_132_133_Report/");
        }
        public HSU_CR1_Report_DTO HSU_141_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_141_Report/");
        }
        public HSU_CR1_Report_DTO HSU_142_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_142_Report/");
        }
        public HSU_CR1_Report_DTO HSU_121_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_121_Report/");
        }
        public HSU_CR1_Report_DTO HSU_122_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_122_Report/");
        }
        public HSU_CR1_Report_DTO HSU_123_Report(HSU_CR1_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR1_ReportFacade/HSU_123_Report/");
        }
    }
}
