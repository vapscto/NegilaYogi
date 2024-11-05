using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeStudenttctransactionDelegate
    {
        CommonDelegate<CollegeStudenttctransactionDTO, CollegeStudenttctransactionDTO> _Comm = new CommonDelegate<CollegeStudenttctransactionDTO, CollegeStudenttctransactionDTO>();

        public CollegeStudenttctransactionDTO loaddata(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/loaddata");
        }
        public CollegeStudenttctransactionDTO onchangeyear(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangeyear");
        }
        public CollegeStudenttctransactionDTO onchangecourse(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangecourse");
        }
        public CollegeStudenttctransactionDTO onchangebranch(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangebranch");
        }
        public CollegeStudenttctransactionDTO onchangesemester(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangesemester");
        }
        public CollegeStudenttctransactionDTO onchangesection(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangesection");
        }
        public CollegeStudenttctransactionDTO searchfilter(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/searchfilter");
        }        

        public CollegeStudenttctransactionDTO onchangestudent(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/onchangestudent");
        }
        public CollegeStudenttctransactionDTO chk_dup_tc(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/chk_dup_tc");
        }
        public CollegeStudenttctransactionDTO savetc(CollegeStudenttctransactionDTO data)
        {
            return _Comm.clgadmissionbypost(data, "CollegeStudenttctransactionFacade/savetc");
        }


    }
}
