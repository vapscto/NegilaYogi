
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
    public class BBHSCUMReportImpl : Interfaces.BBHSCUMReportInterface
    {
        //: Interfaces.HHSAllReportInterface
        private static ConcurrentDictionary<string, BBHSCUMReportDTO> _login =
         new ConcurrentDictionary<string, BBHSCUMReportDTO>();

        private readonly ExamContext _HHSReport_5to7Context;
        ILogger<BaldwinAllReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public BBHSCUMReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<BaldwinAllReportImpl> acdimpl)
        {
            _HHSReport_5to7Context = cpContext;
            _acdimpl = acdimpl;
            _db = db;
        }

        public async Task<BBHSCUMReportDTO> Getdetails(BBHSCUMReportDTO data)//int IVRMM_Id
        {
            BBHSCUMReportDTO getdata = new BBHSCUMReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _HHSReport_5to7Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = await _HHSReport_5to7Context.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToListAsync();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = await _HHSReport_5to7Context.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToListAsync();
                getdata.classlist = admlist.ToArray();

                getdata.grade_list = _HHSReport_5to7Context.Exm_Master_GradeDMO.Where(t => t.MI_Id == data.MI_Id && t.EMGR_ActiveFlag == true).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;
        }
        public async Task<BBHSCUMReportDTO> savedetails(BBHSCUMReportDTO data)
        {
            try
            {

                data.instname = _HHSReport_5to7Context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                var getcategoryid = _HHSReport_5to7Context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                data.termlist = _HHSReport_5to7Context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getcategoryid).OrderBy(a => a.ECT_TermName).ToArray();

                data.studlist = (from t in _HHSReport_5to7Context.Exm_Stu_MP_Promo_SubjectwiseDMO
                                 where t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id
                                 select new BBHSCUMReportDTO
                                 {
                                     AMST_Id = t.AMST_Id
                                 }).Distinct().ToArray();

                data.subjectslist = (from t in _HHSReport_5to7Context.Exm_Stu_MP_Promo_SubjectwiseDMO
                                     where t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id
                                     select new BBHSCUMReportDTO
                                     {
                                         ISMS_Id = t.ISMS_Id
                                     }).Distinct().ToArray();

                //Eaxm Subject Sub-Subject Marks
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_BBHS_CUMULATIVE_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.eam_sub_mrks_list = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_BBHS_CUMULATIVE_REPORT_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.attendencelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                } 

                data.grade_detailslist = _HHSReport_5to7Context.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == data.EMGR_Id && t.EMGD_ActiveFlag == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public BBHSCUMReportDTO Getsection(BBHSCUMReportDTO data)//int IVRMM_Id
        {
            try
            {
                data.fillsection = (from b in _db.admissioncls
                                    from c in _db.Section
                                    from d in _db.Exm_Category_ClassDMO
                                    where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == data.ASMCL_Id
                                    && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                    select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public BBHSCUMReportDTO getclass(BBHSCUMReportDTO data)//int IVRMM_Id
        {
            try
            {
                data.fillclass = (from c in _db.admissioncls
                                  from d in _db.Exm_Category_ClassDMO
                                  where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();                 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public BBHSCUMReportDTO GetAttendence(BBHSCUMReportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                data.fillstudents = (from a in _db.School_Adm_Y_StudentDMO
                                     from b in _db.admissioncls
                                     from c in _db.Adm_M_Student
                                     from d in _db.Section
                                     where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                                     && sol.Contains(c.AMST_SOL) && ids.Contains(c.AMST_ActiveFlag) && ids.Contains(a.AMAY_ActiveFlag) && b.ASMCL_Id == a.ASMCL_Id 
                                     && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                     select new BBHSCUMReportDTO
                                     {
                                         AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),

                                         AMST_Id = a.AMST_Id
                                     }).Distinct().OrderBy(t => t.AMST_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
