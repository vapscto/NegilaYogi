

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class GradeSlabReportImpl:Interfaces.GradeSlabReportInterface
    {

        public  ExamContext _examcatereportcontext;
        ILogger<GradeSlabReportImpl> _acdimpl;        
        public GradeSlabReportImpl(ExamContext cpContext, ILogger<GradeSlabReportImpl> _log)
        {
            _examcatereportcontext = cpContext;
            _acdimpl = _log;
        }

        public GradeSlabReportDTO getdetails(GradeSlabReportDTO data)
        {
            try
            {
                List<Exm_Master_GradeDMO> grlist = new List<Exm_Master_GradeDMO>();
                grlist = _examcatereportcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id && t.EMGR_ActiveFlag==true).ToList();
                data.grlist = grlist.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }



        public GradeSlabReportDTO getAttendetails(GradeSlabReportDTO data)
        {
            try
            {             
               var grlist = _examcatereportcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.grlist = grlist.ToArray();
                if (data.type == "1")
                {
                   data.studentAttendanceList = (from a in _examcatereportcontext.Exm_Master_GradeDMO
                                                 from b in _examcatereportcontext.Exm_Master_Grade_DetailsDMO
                                                 where(a.MI_Id==data.MI_Id && a.EMGR_Id==b.EMGR_Id && a.EMGR_ActiveFlag== true && b.EMGD_ActiveFlag==true)
                                                 select new GradeSlabReportDTO
                                                 {
                                                     EMGR_Id=a.EMGR_Id,
                                                     EMGR_GradeName=a.EMGR_GradeName,
                                                     EMGD_GradePoints=Convert.ToDecimal(b.EMGD_GradePoints),
                                                     EMGR_MarksPerFlag=a.EMGR_MarksPerFlag,
                                                     EMGD_Name=b.EMGD_Name,
                                                     EMGD_From=b.EMGD_From,
                                                     EMGD_To=b.EMGD_To
                                                 }).Distinct().ToArray();
                }
                else if (data.type == "2")
                {
                    data.studentAttendanceList = (from a in _examcatereportcontext.Exm_Master_GradeDMO
                                                  from b in _examcatereportcontext.Exm_Master_Grade_DetailsDMO
                                                  where (a.MI_Id == data.MI_Id && a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.EMGR_Id==data.EMGR_Id)
                                                  select new GradeSlabReportDTO
                                                  {
                                                      EMGR_Id = a.EMGR_Id,
                                                      EMGR_GradeName = a.EMGR_GradeName,
                                                      EMGD_GradePoints =Convert.ToDecimal(b.EMGD_GradePoints),
                                                      EMGR_MarksPerFlag = a.EMGR_MarksPerFlag,
                                                      EMGD_Name = b.EMGD_Name,
                                                      EMGD_From = b.EMGD_From,
                                                      EMGD_To = b.EMGD_To
                                                  }).Distinct().ToArray();
                }

                data.masterinstitution = _examcatereportcontext.Institution_master.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
    }
}
