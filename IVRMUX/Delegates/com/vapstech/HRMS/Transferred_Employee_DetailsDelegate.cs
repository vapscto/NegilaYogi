using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class Transferred_Employee_DetailsDelegate
    {
        CommonDelegate<EmployeeReportsDTO, EmployeeReportsDTO> COMMM = new CommonDelegate<EmployeeReportsDTO, EmployeeReportsDTO>();
        public EmployeeReportsDTO getvalue(EmployeeReportsDTO data)
        {
            return COMMM.POSTDataHRMS(data, "Transferred_Employee_DetailsFacade/getvalue");
        }
    }
}
