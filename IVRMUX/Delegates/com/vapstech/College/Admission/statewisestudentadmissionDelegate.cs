using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class statewisestudentadmissionDelegate
    {
        CommonDelegate<statewisestudentadmissionDTO, statewisestudentadmissionDTO> _commbranch = new CommonDelegate<statewisestudentadmissionDTO, statewisestudentadmissionDTO>();
        public statewisestudentadmissionDTO getdetails(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/getdetails/");
        }
        public statewisestudentadmissionDTO onselectAcdYear(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onselectAcdYear/");
        }
        public statewisestudentadmissionDTO onselectCourse(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onselectCourse/");
        }
        public statewisestudentadmissionDTO onselectBranch(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onselectBranch/");
        }
        public statewisestudentadmissionDTO onreport(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onreport/");
        }
        public statewisestudentadmissionDTO onreportcountry(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onreportcountry/");
        }
        public statewisestudentadmissionDTO onreportreligionruralurban(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onreportreligionruralurban/");
        }
        public statewisestudentadmissionDTO CategoryCasteWiseStudentReport(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/CategoryCasteWiseStudentReport/");
        }
        public statewisestudentadmissionDTO onreportbirthday(statewisestudentadmissionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "statewisestudentadmissionFacade/onreportbirthday/");
        }
        
    }
}
