using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface IVRM_ClassWorkInterface
    {
        IVRM_ClassWorkDTO savedetail(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO Getdetails(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO deactivate(IVRM_ClassWorkDTO data);
        Task<IVRM_ClassWorkDTO> get_classes(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getsectiondata(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getsubject(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO editData(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO viewData(IVRM_ClassWorkDTO data);

        //============= class work marks enter
        IVRM_ClassWorkDTO getclasswork_student(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getclasswork_list(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getsubjectlist(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO classwork_marks_update(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO edit_classwork_mark(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO viewclasswork(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO viewstudentupload(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO stfupload(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO Getdata_class(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getreportnotice(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO Getdataview(IVRM_ClassWorkDTO data);
        IVRM_ClassWorkDTO getclasswork_Topiclist(IVRM_ClassWorkDTO data);
    }
}
