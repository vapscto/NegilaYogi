
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
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamCalculationImpl : Interfaces.ExamCalculationInterface
    {
        private static ConcurrentDictionary<string, ExamReportDTO> _login =
         new ConcurrentDictionary<string, ExamReportDTO>();

        private readonly ExamContext _CumulativeReportContext;
        ILogger<ExamCalculationImpl> _acdimpl;
        public ExamCalculationImpl(ExamContext cpContext, ILogger<ExamCalculationImpl> acdimpl)
        {
            _CumulativeReportContext = cpContext;
            _acdimpl = acdimpl;
        }
        public ExamReportDTO Getdetails(ExamReportDTO data)
        {
            ExamReportDTO getdata = new ExamReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _CumulativeReportContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _CumulativeReportContext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _CumulativeReportContext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _CumulativeReportContext.exammasterDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.exmstdlist = esmp.ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public ExamReportDTO get_cls_sections(ExamReportDTO id)
        {

            try
            {
                var Cat_Id = _CumulativeReportContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMAY_Id == id.ASMAY_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).First();

                var year_cat_id = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true && t.EMCA_Id == Cat_Id).Select(t => t.EYC_Id).First();

                id.seclist = (from m in _CumulativeReportContext.Exm_Category_ClassDMO
                              from o in _CumulativeReportContext.School_M_Section
                              where (m.ASMCL_Id == id.ASMCL_Id && m.ASMS_Id == o.ASMS_Id
                              && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.MI_Id == id.MI_Id
                              && m.ASMAY_Id == id.ASMAY_Id && m.ECAC_ActiveFlag == true)
                              select o).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

                
                id.exmstdlist = (from a in _CumulativeReportContext.masterexam
                                 from b in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == id.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == year_cat_id)
                                 select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public ExamReportDTO Calculation(ExamReportDTO exm)
        {
            try
            {
                using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 1000;
                    cmd.CommandText = "IndExamMarksCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = exm.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = exm.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                  SqlDbType.Int)
                    {
                        Value = exm.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ESTMP_PublishToStudentFlg",
                 SqlDbType.Int)
                    {
                        Value = exm.ESTMP_PublishToStudentFlg
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var a = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Exam Calculation old 1:" + exm.MI_Id + " " + ex.Message + "");
                    }

                    exm.returnval = true;
                }

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                dmo.MI_Id = exm.MI_Id;
                dmo.ASMAY_Id = exm.ASMAY_Id;
                dmo.ASMCL_Id = exm.ASMCL_Id;
                dmo.ASMS_Id = exm.ASMS_Id;
                dmo.EME_Id = exm.EME_Id;
                dmo.IVRMUL_Id = exm.User_Id;
                dmo.ECL_PublishFlag = 0;
                dmo.CreatedDate = indiantime0;
                dmo.UpdatedDate = indiantime0;

                _CumulativeReportContext.Add(dmo);
                try
                {
                    var i = _CumulativeReportContext.SaveChanges();
                    if (i > 0)
                    {
                        _acdimpl.LogInformation("Exam Calculation Old Insert Into Log Success");
                        exm.returnval = true;
                    }
                    else
                    {
                        _acdimpl.LogInformation("Exam Calculation Old Insert Into Log Failed");
                        exm.returnval = true;
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Exam Calculation Old Insert :" + exm.MI_Id + " " + ex.Message + "");
                    Console.WriteLine(ex.Message);
                    exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation old 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }

        // Marks Approved Process To Display In Portals 
        public ExamReportDTO getexam(ExamReportDTO exm)
        {
            try
            {
                exm.exmstdlist = _CumulativeReportContext.masterexam.Where(a => a.MI_Id == exm.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation old 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }
        public ExamReportDTO getclass(ExamReportDTO data)
        {
            try
            {
                data.classlist = (from a in _CumulativeReportContext.Exm_Yearly_CategoryDMO
                                  from b in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                  from c in _CumulativeReportContext.Exm_Category_ClassDMO
                                  from d in _CumulativeReportContext.AcademicYear
                                  from e in _CumulativeReportContext.AdmissionClass
                                  from f in _CumulativeReportContext.masterexam
                                  from g in _CumulativeReportContext.Exm_Master_CategoryDMO
                                  where (a.EYC_Id == b.EYC_Id && b.EME_Id == f.EME_Id && c.ASMCL_Id == e.ASMCL_Id && c.ASMAY_Id == d.ASMAY_Id && a.EMCA_Id == c.EMCA_Id
                                  && a.EMCA_Id == g.EMCA_Id && c.EMCA_Id == g.EMCA_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true
                                  && c.ECAC_ActiveFlag == true && g.EMCA_ActiveFlag == true && b.EME_Id == data.EME_Id && a.ASMAY_Id == data.ASMAY_Id
                                  && c.ASMAY_Id == data.ASMAY_Id)
                                  select e).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamReportDTO saveapprove(ExamReportDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
