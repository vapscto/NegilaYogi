using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class HSU_CR2_ReportDelegate
    {
        CommonDelegate<HSU_CR2_Report_DTO, HSU_CR2_Report_DTO> _commnbranch = new CommonDelegate<HSU_CR2_Report_DTO, HSU_CR2_Report_DTO>();

        public HSU_CR2_Report_DTO getdata(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/getdata/");
        }
        public HSU_CR2_Report_DTO HSU_211_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_211_Report/");
        }
        public HSU_CR2_Report_DTO HSU_212_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_212_Report/");
        }
        public HSU_CR2_Report_DTO HSU_213_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_213_Report/");
        }
        public HSU_CR2_Report_DTO HSU_221_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_221_Report/");
        }
        public HSU_CR2_Report_DTO HSU_222_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_222_Report/");
        }
        public HSU_CR2_Report_DTO HSU_232_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_232_Report/");
        }
        public HSU_CR2_Report_DTO HSU_234_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_234_Report/");
        }
        public HSU_CR2_Report_DTO HSU_241_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_241_Report/");
        }
        public HSU_CR2_Report_DTO HSU_242_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_242_Report/");
        }
        public HSU_CR2_Report_DTO HSU_243_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_243_Report/");
        }
        public HSU_CR2_Report_DTO HSU_244_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_244_Report/");
        }
        public HSU_CR2_Report_DTO HSU_245_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_245_Report/");
        }
        public HSU_CR2_Report_DTO HSU_251_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_251_Report/");
        }
        public HSU_CR2_Report_DTO HSU_252_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_252_Report/");
        }
        public HSU_CR2_Report_DTO HSU_253_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_253_Report/");
        }
        public HSU_CR2_Report_DTO HSU_255_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_255_Report/");
        }
        public HSU_CR2_Report_DTO HSU_262_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_262_Report/");
        }
        public HSU_CR2_Report_DTO HSU_271_Report(HSU_CR2_Report_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "HSU_CR2_ReportFacade/HSU_271_Report/");
        }
       
    }
}
