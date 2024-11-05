using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class StudentIdCardFormatDelegate
    {
        CommonDelegate<StudentIdCardFormatDTO, StudentIdCardFormatDTO> _comm = new CommonDelegate<StudentIdCardFormatDTO, StudentIdCardFormatDTO>();

        public StudentIdCardFormatDTO OnLoadStudentIdCardDetails(StudentIdCardFormatDTO data)
        {
            return _comm.POSTDataADM(data, "StudentIdCardFormatFacade/OnLoadStudentIdCardDetails");
        }
        public StudentIdCardFormatDTO OnChangeYear(StudentIdCardFormatDTO data)
        {
            return _comm.POSTDataADM(data, "StudentIdCardFormatFacade/OnChangeYear");
        }
        public StudentIdCardFormatDTO OnChangeClass(StudentIdCardFormatDTO data)
        {
            return _comm.POSTDataADM(data, "StudentIdCardFormatFacade/OnChangeClass");
        }
        public StudentIdCardFormatDTO OnChangeSection(StudentIdCardFormatDTO data)
        {
            return _comm.POSTDataADM(data, "StudentIdCardFormatFacade/OnChangeSection");
        }
        public StudentIdCardFormatDTO GetReportDetails(StudentIdCardFormatDTO data)
        {
            return _comm.POSTDataADM(data, "StudentIdCardFormatFacade/GetReportDetails");
        }
    }
}
