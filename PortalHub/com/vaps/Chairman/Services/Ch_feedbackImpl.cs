

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
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class Ch_feedbackImpl : Interfaces.Ch_feedbackInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<Ch_LopImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public Ch_feedbackImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public Ch_feedbackDTO getdata(Ch_feedbackDTO dto)
        {
            try
            {

                dto.academicyearlst = _ChairmanDashboardContext.AcademicYearDMO.Where(q => q.MI_Id == dto.MI_Id && q.Is_Active == true).OrderByDescending(s => s.ASMAY_Order).ToArray();

                dto.feedbackdetails = (from a in _ChairmanDashboardContext.Adm_School_Student_GFeedbackDMO
                                       from b in _ChairmanDashboardContext.AcademicYearDMO
                                       from c in _ChairmanDashboardContext.School_M_Class
                                       from d in _ChairmanDashboardContext.School_M_Section
                                       from e in _ChairmanDashboardContext.Adm_M_Student
                                       from f in _ChairmanDashboardContext.School_Adm_Y_StudentDMO
                                       where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == e.MI_Id && a.MI_Id == dto.MI_Id && a.AMST_Id == e.AMST_Id && a.AMST_Id == f.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == f.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == f.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMS_Id == f.ASMS_Id && a.ASGFE_ActiveFlag==true
                                       select new Ch_feedbackDTO
                                       {
                                       AMST_Id=a.AMST_Id,
                                       AMST_FirstName=e.AMST_FirstName,
                                       AMST_MiddleName=e.AMST_MiddleName,
                                       AMST_LastName=e.AMST_LastName,
                                       AMST_AdmNo=e.AMST_AdmNo,
                                       ASMCL_Id=a.ASMCL_Id,
                                       ASMCL_ClassName=c.ASMCL_ClassName,
                                       ASMS_Id=a.ASMS_Id,
                                       ASMC_SectionName=d.ASMC_SectionName,
                                       ASMAY_Id=a.ASMAY_Id,
                                       ASMAY_Year=b.ASMAY_Year,
                                        ASGFE_FeedBack=a.ASGFE_FeedBack,
                                        ASGFE_FeedbackDate=a.ASGFE_FeedbackDate
                                       }).OrderByDescending(p=>p.ASGFE_FeedbackDate).ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

       

        public Ch_feedbackDTO onmonth(Ch_feedbackDTO dto)
        {
            try
            {
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }return dto;
        }
    }
}
