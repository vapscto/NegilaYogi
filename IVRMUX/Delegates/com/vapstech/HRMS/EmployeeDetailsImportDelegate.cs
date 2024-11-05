using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeDetailsImportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterEmployeeImportDTO, MasterEmployeeImportDTO> COMMM = new CommonDelegate<MasterEmployeeImportDTO, MasterEmployeeImportDTO>();

        public MasterEmployeeImportDTO save_excel_data(MasterEmployeeImportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeDetailsImportFacade/save_excel_data/");
        }

    }
}
