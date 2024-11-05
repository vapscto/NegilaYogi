using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class EmployeeDataImportDelegate
    {
        CommonDelegate<EmployeeDataImportDTO, EmployeeDataImportDTO> _commnbranch = new CommonDelegate<EmployeeDataImportDTO, EmployeeDataImportDTO>();

        public EmployeeDataImportDTO Savedata(EmployeeDataImportDTO data)
        {
            return _commnbranch.POSTDataHRMS(data, "EmployeeDataImportFacade/Savedata/");
        }
        public EmployeeDataImportDTO getdetails(int id)
        {
            return _commnbranch.GetDataByIdHRMS(id, "EmployeeDataImportFacade/getdetails/");
        }
        public EmployeeDataImportDTO deactiveY(EmployeeDataImportDTO data)
        {
            return _commnbranch.POSTDataHRMS(data, "EmployeeDataImportFacade/deactiveY/");
        }
    }
}
