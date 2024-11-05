

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
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ExmSectionPerformImpl : Interfaces.ExmSectionPerformInterface
    {
        private static ConcurrentDictionary<string, ExmSectionPerformDTO> _login =
         new ConcurrentDictionary<string, ExmSectionPerformDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        //public ExamContext _exm;
        public ExmSectionPerformImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
            //_exm = exm;
        }

        public ExmSectionPerformDTO Getdetails(ExmSectionPerformDTO data)//int IVRMM_Id
        {



            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            return data;

        }




        public ExmSectionPerformDTO getclassexam(ExmSectionPerformDTO data)//int IVRMM_Id
        {


            try
            {



                data.classlist = (from a in _ChairmanDashboardContext.School_M_Class
                                  from b in _ChairmanDashboardContext.Exm_Category_ClassDMO
                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true))
                                  select new ExmSectionPerformDTO
                                  {
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Id = a.ASMCL_Id
                                  }).Distinct().ToArray();



                data.exmstdlist = (from a in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                   from b in _ChairmanDashboardContext.Exm_Yearly_Category_ExamsDMO
                                   from c in _ChairmanDashboardContext.exammasterDMO

                                   where (a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && c.EME_ActiveFlag.Equals(true))
                                   select new ExmSectionPerformDTO
                                   {
                                       EME_ExamName = c.EME_ExamName,
                                       EME_Id = c.EME_Id
                                   }).Distinct().OrderBy(t => t.EME_Id).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public ExmSectionPerformDTO getcategory(ExmSectionPerformDTO data)//int IVRMM_Id
        {


            try
            {

                data.fillcategory = (from a in _ChairmanDashboardContext.Exm_Master_CategoryDMO
                                     from b in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg.Equals(true) && a.EMCA_ActiveFlag.Equals(true))


                                     select new ExmSectionPerformDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName
                                     }).Distinct().OrderBy(t => t.EMCA_Id).ToArray();




            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public ExmSectionPerformDTO showreport(ExmSectionPerformDTO data)
        {


            try
            {
                data.seclist = (
                                 from a in _ChairmanDashboardContext.ExmStudentMarksProcessSubjectwise
                                 from b in _ChairmanDashboardContext.School_M_Section
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id   && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id && a.ASMS_Id==b.ASMS_Id)
                                 select new ExmSectionPerformDTO
                                 {
                                     ASMC_SectionName = b.ASMC_SectionName,
                                     ESTMPS_SectionAverage = a.ESTMPS_SectionAverage,

                                 }
                               ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public ExmSectionPerformDTO getsubject(ExmSectionPerformDTO data)
        {
            try
            {

                data.subjlist = (from a in _ChairmanDashboardContext.Exm_Category_ClassDMO
                                 from b in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                 from c in _ChairmanDashboardContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _ChairmanDashboardContext.Ch_Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from e in _ChairmanDashboardContext.IVRM_Master_SubjectsDMO
                                 from f in _ChairmanDashboardContext.ExmStudentMarksProcessSubjectwise
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id  && a.ASMCL_Id==f.ASMCL_Id &&  f.ASMAY_Id==a.ASMAY_Id
                                 && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.MI_Id==f.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && c.EME_Id==f.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id && e.ISMS_Id == f.ISMS_Id)
                                 select new ExmSectionPerformDTO
                                 {
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                     
                                 }
                               ).Distinct().ToArray();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

    }
}
