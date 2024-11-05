using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class IVRM_ClassWorkDelegats
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO> COMMM = new CommonDelegate<IVRM_ClassWorkDTO, IVRM_ClassWorkDTO>();
        public IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/savedetail/");
        }
        public IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/Getdetails/");
        }
        public IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/deactivate/");
        }
        public IVRM_ClassWorkDTO get_classes(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/get_classes/");
        }

        public IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getsectiondata/");
        }
        public IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getsubject/");
        }

        public IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/editData/");
        }
         public IVRM_ClassWorkDTO viewData(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/viewData/");
        }
        //==================Class work marks enter=======

        public IVRM_ClassWorkDTO getclasswork_student(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getclasswork_student/");
        }
        public IVRM_ClassWorkDTO getclasswork_list(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getclasswork_list/");
        }
        public IVRM_ClassWorkDTO getsubjectlist(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getsubjectlist/");
        }
        public IVRM_ClassWorkDTO classwork_marks_update(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/classwork_marks_update/");
        }

        public IVRM_ClassWorkDTO edit_classwork_mark(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/edit_classwork_mark/");
        }

        public IVRM_ClassWorkDTO viewclasswork(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/viewclasswork/");
        }

        public IVRM_ClassWorkDTO viewstudentupload(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/viewstudentupload/");
        }
        public IVRM_ClassWorkDTO stfupload(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/stfupload/");
        }

        //noticeboard consolidated report
        public IVRM_ClassWorkDTO Getdata_class(IVRM_ClassWorkDTO dto)
        {
            return COMMM.POSTPORTALData(dto, "IVRM_ClassWorkFacade/Getdata_class/");
        }
        public IVRM_ClassWorkDTO getreportnotice(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getreportnotice/");
        }

        public IVRM_ClassWorkDTO Getdataview(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/Getdataview/");
        }
        public IVRM_ClassWorkDTO getclasswork_Topiclist(IVRM_ClassWorkDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_ClassWorkFacade/getclasswork_Topiclist/");
        }
    }
}
