using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeGroupWiseStudentReportFacade: Controller
    {
        public FeeGroupWiseStudentReportInterface _IStatus;

        public FeeGroupWiseStudentReportFacade(FeeGroupWiseStudentReportInterface IStatus)
        {
            _IStatus = IStatus;

        }
        // load initial dropdown
        [Route("Getclass")]
        public FeeGroupWiseStudentReportDTO Getclass([FromBody] FeeGroupWiseStudentReportDTO data)
        {
            return _IStatus.Getclass(data);
        }
        [Route("GetSection")]
        public FeeGroupWiseStudentReportDTO GetSection([FromBody] FeeGroupWiseStudentReportDTO data)
        {
            return _IStatus.GetSection(data);
        }
        [Route("GetStudent")]
        public FeeGroupWiseStudentReportDTO GetStudent([FromBody] FeeGroupWiseStudentReportDTO data)
        {
            return _IStatus.GetStudent(data);
        }
        

        [Route("getinitialdata")]
        public FeeGroupWiseStudentReportDTO getInitialData([FromBody] FeeGroupWiseStudentReportDTO data)
        {
            return _IStatus.getInitailData(data);
        }

        [HttpPost]
        public FeeGroupWiseStudentReportDTO SearchData([FromBody] FeeGroupWiseStudentReportDTO Clscatag)
        {
            return _IStatus.SearchData(Clscatag);
        }
    
    }
}
