using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.NAAC.Reports;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class CurricularAspectsDelegate
    {
        CommonDelegate<CurricularAspects_DTO, CurricularAspects_DTO> _commnbranch = new CommonDelegate<CurricularAspects_DTO, CurricularAspects_DTO>();

        public CurricularAspects_DTO getdata(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/getdata/");
        }
        public CurricularAspects_DTO get_report(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_report/");
        }
        public CurricularAspects_DTO get_nCourse_report(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_nCourse_report/");
        }

        public CurricularAspects_DTO get_report_113(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_report_113/");
        }

        public CurricularAspects_DTO get_report_123(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_report_123/");
        }

        public CurricularAspects_DTO get_report_133(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_report_133/");
        }
        public CurricularAspects_DTO get_report_132(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_report_132/");
        }
        public CurricularAspects_DTO get_122CBCSsystemReport(CurricularAspects_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "CurricularAspectsFacade/get_122CBCSsystemReport/");
        }

       
        
    }
}
