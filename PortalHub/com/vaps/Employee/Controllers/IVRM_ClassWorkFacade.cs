using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_ClassWorkFacade : Controller
    {
        // GET: api/values
        public IVRM_ClassWorkInterface _work;
        public IVRM_ClassWorkFacade(IVRM_ClassWorkInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public IVRM_ClassWorkDTO savedetail([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public IVRM_ClassWorkDTO Getdetails([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_ClassWorkDTO deactivate([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.deactivate(data);
        }
        [Route("get_classes")]
        public Task<IVRM_ClassWorkDTO> get_classes([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_ClassWorkDTO getsectiondata([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_ClassWorkDTO getsubject([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getsubject(data);
        }
        [Route("editData")]
        public IVRM_ClassWorkDTO editData([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.editData(data);
        }

        [Route("viewData")]
        public IVRM_ClassWorkDTO viewData([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.viewData(data);
        }

        //============= Class work mark enter=======
        [Route("getclasswork_student")]
        public IVRM_ClassWorkDTO getclasswork_student([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getclasswork_student(data);
        }
        [Route("getclasswork_list")]
        public IVRM_ClassWorkDTO getclasswork_list([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getclasswork_list(data);
        }
        [Route("getsubjectlist")]
        public IVRM_ClassWorkDTO getsubjectlist([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getsubjectlist(data);
        }
        [Route("classwork_marks_update")]
        public IVRM_ClassWorkDTO classwork_marks_update([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.classwork_marks_update(data);
        }
        [Route("edit_classwork_mark")]
        public IVRM_ClassWorkDTO edit_classwork_mark([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.edit_classwork_mark(data);
        }
        [Route("viewclasswork")]
        public IVRM_ClassWorkDTO viewclasswork([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.viewclasswork(data);
        }
        [Route("viewstudentupload")]
        public IVRM_ClassWorkDTO viewstudentupload([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.viewstudentupload(data);
        }
         [Route("stfupload")]
        public IVRM_ClassWorkDTO stfupload([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.stfupload(data);
        }

        //noticeboard consolidated report
        [Route("Getdata_class")]
        public IVRM_ClassWorkDTO Getdata_class([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.Getdata_class(data);
        }
        [Route("getreportnotice")]
        public IVRM_ClassWorkDTO getreportnotice([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getreportnotice(data);
        }

        [Route("Getdataview")]
        public IVRM_ClassWorkDTO Getdataview([FromBody] IVRM_ClassWorkDTO dto)
        {
            return _work.Getdataview(dto);
        }
        [Route("getclasswork_Topiclist")]
        public IVRM_ClassWorkDTO getclasswork_Topiclist([FromBody] IVRM_ClassWorkDTO dto)
        {
            return _work.getclasswork_Topiclist(dto);
        }
    }
}
