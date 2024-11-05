using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class Master_ExamQualified_ClassDelegate
    {
        CommonDelegate<Master_ExamQualified_ClassDTO, Master_ExamQualified_ClassDTO> COMM = new CommonDelegate<Master_ExamQualified_ClassDTO, Master_ExamQualified_ClassDTO>();

        public Master_ExamQualified_ClassDTO getalldata(Master_ExamQualified_ClassDTO id)
        {
            return COMM.POSTDataADM(id, "Master_ExamQualified_ClassFacade/getalldata/");
        }

        public Master_ExamQualified_ClassDTO SaveClass(Master_ExamQualified_ClassDTO dTO)
        {
            return COMM.POSTDataADM(dTO, "Master_ExamQualified_ClassFacade/SaveClass");
        }
        public Master_ExamQualified_ClassDTO Editdetails(int id)
        {

            return COMM.GetDataByIdADM(id, "Master_ExamQualified_ClassFacade/Editdetails/");


        }

        public Master_ExamQualified_ClassDTO deactiveCat(Master_ExamQualified_ClassDTO dTO)
        {

            return COMM.POSTDataADM(dTO, "Master_ExamQualified_ClassFacade/deactiveCat/");
        }
    }
}
