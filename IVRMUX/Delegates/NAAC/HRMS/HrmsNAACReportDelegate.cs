using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.HRMS;

namespace IVRMUX.Delegates.HRMS
{
    public class HrmsNAACReportDelegate
    {
        CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO> _comm = new CommonDelegate<HRMS_NAAC_DTO, HRMS_NAAC_DTO>();

        public HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getdetails");
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/get_depts");
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/get_desig");
        }
        
        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/get_Employe_ob");
        }

        public HRMS_NAAC_DTO SaveData(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/SaveData");
        }

        public HRMS_NAAC_DTO getOrientdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getOrientdata");
        }

        public HRMS_NAAC_DTO getStudentActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getStudentActivitydata");
        }

        public HRMS_NAAC_DTO getProfessionalActivitydata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getProfessionalActivitydata");
        }

        public HRMS_NAAC_DTO getResearchProjectdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getResearchProjectdata");
        }

        public HRMS_NAAC_DTO getResearchGuidedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getResearchGuidedata");
        }

        public HRMS_NAAC_DTO getBOSBOEdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getBOSBOEdata");
        }

        public HRMS_NAAC_DTO getJournaldata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getJournaldata");
        }

        public HRMS_NAAC_DTO getConferencedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getConferencedata");
        }

        public HRMS_NAAC_DTO getBookdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getBookdata");
        }

        public HRMS_NAAC_DTO getBookChapterdata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getBookChapterdata");
        }

        public HRMS_NAAC_DTO getCommetteedata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getCommetteedata");
        }

        public HRMS_NAAC_DTO getOtherDetaildata(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/getOtherDetaildata");
        }

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data)
        {
            return _comm.naacdetailsbypost(data, "HrmsNAACReportFacade/get_EmployeALLDATA");
        }
        
    }
}
