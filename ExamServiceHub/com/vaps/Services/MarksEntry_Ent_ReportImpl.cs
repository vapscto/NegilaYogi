

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
    public class MarksEntry_Ent_ReportImpl : Interfaces.MarksEntry_Ent_ReportInterface
    {
        private static ConcurrentDictionary<string, MarksEntry_Ent_ReportDTO> _login =
         new ConcurrentDictionary<string, MarksEntry_Ent_ReportDTO>();

        private readonly ExamContext _BaldwinAllReportContext;
        ILogger<MarksEntry_Ent_ReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public MarksEntry_Ent_ReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<MarksEntry_Ent_ReportImpl> acdimpl)
        {
            _BaldwinAllReportContext = cpContext;
            _db = db;
            _acdimpl = acdimpl;
        }
        public async Task<MarksEntry_Ent_ReportDTO> Getdetails(MarksEntry_Ent_ReportDTO data)//int IVRMM_Id
        {
            MarksEntry_Ent_ReportDTO getdata = new MarksEntry_Ent_ReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _BaldwinAllReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _BaldwinAllReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToListAsync();

                getdata.exmstdlist = esmp.ToArray();

                getdata.stafflist = (from a in _BaldwinAllReportContext.Exm_Login_PrivilegeDMO
                                  from b in _BaldwinAllReportContext.Staff_User_Login
                                  where (a.Login_Id == b.IVRMSTAUL_Id && a.MI_Id==b.MI_Id && b.MI_Id==data.MI_Id)
                                  select new MarksEntry_Ent_ReportDTO
                                  {
                                      Login_Id =a.Login_Id,

                                      IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                                      ApplId=b.Id
                                  }).Distinct().ToArray();
                

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public MarksEntry_Ent_ReportDTO get_report(MarksEntry_Ent_ReportDTO data)
        {

            
            try
            {
               
                
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_Count";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Studentcount = retObject.ToArray();
                    }
                }
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Exam_Staffload_Year_Insti";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 90000000;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        data.subjlist = retObject.ToArray();
                //    }
                //}
                string staffids = "0";
                for (int i = 0; i < data.staffarray.Length; i++)
                {
                    staffids = staffids + ',' + data.staffarray[i].ToString();
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                   cmd.CommandText = "Exam_Staffexamentry_studentcount";
                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Userid", SqlDbType.VarChar) { Value = staffids });
                    
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow); 
                            }
                        }
                        data.get_report = retObject.ToArray();
                    }
                }
                data.imgname = _db.Institute.Where(R => R.MI_Id == data.MI_Id).Select(T => T.MI_Logo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
   
        public MarksEntry_Ent_ReportDTO SubjectList(MarksEntry_Ent_ReportDTO data)
        {


            try
            {
                string LoginId = "0";
                if (data.LoginIdLists != null && data.LoginIdLists.Length > 0)
                {
                   foreach(var d  in data.LoginIdLists)
                    {
                        LoginId = LoginId + ',' + d.LoginId;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Staff_SubjectList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.VarChar) { Value = LoginId });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.SubjecList = retObject.ToArray();
                    }
                }




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
