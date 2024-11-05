using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.admission;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeChallanReportFacadeController : Controller
    {
        public FeeChallanReportInterface _feetar;

        public FeeChallanReportFacadeController(FeeChallanReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values
   
        [HttpPost]
        [Route("getalldetails123")]
        public FeeChallanReportDTO Getdet([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.getdata123(data);
        }
        
       
        [Route("getreport")]
        public Task<FeeChallanReportDTO> getreport([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("getinstallment")]
        public FeeChallanReportDTO getinstallment([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.getinstallment(data);
        }
        [Route("generateHutchingChallan")]
        public FeeChallanReportDTO generateHutchingChallan([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.generateHutchingChallan(data);
        }
        [Route("checkforchallan")]
        public FeeChallanReportDTO checkforchallan([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.checkforchallan(data);
        }
        
       [Route("getChallandetails")]
        public FeeChallanReportDTO getChallandetails([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.getChallandetails(data);
        }
        

       [Route("delrec")]
        public FeeChallanReportDTO delrec([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.delrec(data);
        }
        [Route("getstudlistgroup")]
        public FeeChallanReportDTO getstudlistgroup([FromBody] FeeChallanReportDTO data)
        {
            return _feetar.getstudlistgroup(data);
        }

        [Route("searching")]
        public FeeStudentTransactionDTO searching([FromBody] FeeStudentTransactionDTO data)
        {
            return _feetar.searching(data);
        }
    }
}
