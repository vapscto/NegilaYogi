using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.HRMS;

namespace IVRMUX.Delegates.HRMS
{
    public class HrmsConsolidatedReportDelegate
    {
        CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO> _comm = new CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO>();

        public HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/getdetails");
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/get_depts");
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/get_desig");
        }
        
        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/get_Employe_ob");
        }

        public HRMS_NAAC_DTO getEmployeReport(HRMS_NAAC_DTO data)
        {
            //return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/getEmployeReport");
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/getEmployeReportAsync"); 
        }

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsConsolidatedReportFacade/get_EmployeALLDATA");
        }
        
    }
}
