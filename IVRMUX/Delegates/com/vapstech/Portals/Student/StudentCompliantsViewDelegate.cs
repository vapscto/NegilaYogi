using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Student
{
    public class StudentCompliantsViewDelegate
    {
        CommonDelegate<StudentCompliantsView_DTO, StudentCompliantsView_DTO> comm = new CommonDelegate<StudentCompliantsView_DTO, StudentCompliantsView_DTO>();
        public StudentCompliantsView_DTO loaddata(StudentCompliantsView_DTO data)
        {
            return comm.POSTPORTALData(data, "StudentCompliantsViewFacade/loaddata");
        }
        public StudentCompliantsView_DTO report1(StudentCompliantsView_DTO data)
        {
            return comm.POSTPORTALData(data, "StudentCompliantsViewFacade/report1");
        }
    }
}
