using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Lib_stu_punch_reportFacade : Controller
    {

        Lib_stu_punch_reportInterface _interface;
        public Lib_stu_punch_reportFacade(Lib_stu_punch_reportInterface _inter)
        {
            _interface = _inter;
        }


        [Route("Getdetails")]
        public Lib_stu_punch_reportDTO Getdetails([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.Getdetails(data);
        }

        [Route("get_classes")]
        public Lib_stu_punch_reportDTO get_classes([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.get_classes(data);
        }
        [Route("get_sections")]
        public Lib_stu_punch_reportDTO get_sections([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.get_sections(data);
        }
        [Route("get_students_category_grade")]
        public Lib_stu_punch_reportDTO get_students([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.get_students_category_grade(data);
        }

        [Route("get_report")]
        public Lib_stu_punch_reportDTO get_report([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.get_report(data);
        }
        [Route("get_biometric_device")]
        public Lib_stu_punch_reportDTO biometric_install([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.biometric_install(data);
        }
        //
        [Route("get_biometric_deviceclg")]
        public Lib_stu_punch_reportDTO get_biometric_deviceclg([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.get_biometric_deviceclg(data);
        }


        [Route("onloadpage")]
        public Lib_stu_punch_reportDTO onloadpage([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.onloadpage(data);
        }
        [Route("loadcourse")]
        public Lib_stu_punch_reportDTO loadcourse([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.loadcourse(data);
        }
        [Route("loadbranch")]
        public Lib_stu_punch_reportDTO loadbranch([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.loadbranch(data);
        }
        [Route("loadsemester")]
        public Lib_stu_punch_reportDTO loadsemester([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.loadsemester(data);
        }
        [Route("loaadsection")]
        public Lib_stu_punch_reportDTO loaadsection([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.loaadsection(data);
        }
        [Route("loadstudents")]
        public Lib_stu_punch_reportDTO loadstudents([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.loadstudents(data);
        }
        [Route("clgpunchreport")]
        public Lib_stu_punch_reportDTO clgpunchreport([FromBody]Lib_stu_punch_reportDTO data)
        {
            return _interface.clgpunchreport(data);
        }
    }
}
