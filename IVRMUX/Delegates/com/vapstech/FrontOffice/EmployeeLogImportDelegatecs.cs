using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class EmployeeLogImportDelegatecs
    {
        CommonDelegate<EmployeeLogImportDTO, EmployeeLogImportDTO> _commnbranch = new CommonDelegate<EmployeeLogImportDTO, EmployeeLogImportDTO>();

        public EmployeeLogImportDTO Savedata(EmployeeLogImportDTO data)
        {
            return _commnbranch.POSTDataHolidayReport(data, "EmployeeLogImportFacade/Savedata/");
        }
    }
}
