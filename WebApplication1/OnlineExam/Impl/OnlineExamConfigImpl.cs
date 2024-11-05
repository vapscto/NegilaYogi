using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class OnlineExamConfigImpl : Interfaces.OnlineExamConfigInterface
    {
        private static ConcurrentDictionary<string, MasterQuestionDTO> _login =
             new ConcurrentDictionary<string, MasterQuestionDTO>();

        ILogger<OnlineExamConfigImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public OnlineExamConfigImpl(DomainModelMsSqlServerContext dbcontext, ILogger<OnlineExamConfigImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {
            try
            {
                data.getQdetails = _dbContext.LMS_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();



                data.getclass = _dbContext.admissioncls.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            try
            {
                if (data.LMSMOES_Id > 0)
                {
                    var result = _dbContext.LMS_Master_OE_SettingDMO.Single(t => t.LMSMOES_Id == data.LMSMOES_Id);
                    result.MI_Id = data.MI_Id;
                    result.LMSMOES_NoofQns = data.Noofques;
                    result.LMSMOES_TotalMarks = data.totmrks;
                    result.LMSMOES_TotalDuration = data.totdur;
                    result.LMSMOES_EachQnsMarks = data.echmrkques;
                    result.LMSMOES_EachQnsDuration = data.echquesdur;
                    result.LMSMOES_NoOfOptions = data.noopt;                    
                    _dbContext.Update(result);
                    var contactExists = _dbContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {

                    var res = _dbContext.LMS_Master_OE_SettingDMO.Where(t => t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        var result = _dbContext.LMS_Master_OE_SettingDMO.Single(t => t.MI_Id == data.MI_Id);
                        result.LMSMOES_NoofQns = data.Noofques;
                        result.LMSMOES_TotalMarks = data.totmrks;
                        result.LMSMOES_TotalDuration = data.totdur;
                        result.LMSMOES_EachQnsMarks = data.echmrkques;
                        result.LMSMOES_EachQnsDuration = data.echquesdur;
                        result.LMSMOES_NoOfOptions = data.noopt;
                        _dbContext.Update(result);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {

                        LMS_Master_OE_SettingDMO result1 = new LMS_Master_OE_SettingDMO();
                        result1.MI_Id = data.MI_Id;
                        result1.LMSMOES_NoofQns = data.Noofques;
                        result1.LMSMOES_TotalMarks = data.totmrks;
                        result1.LMSMOES_TotalDuration = data.totdur;
                        result1.LMSMOES_EachQnsMarks = data.echmrkques;
                        result1.LMSMOES_EachQnsDuration = data.echquesdur;
                        result1.LMSMOES_NoOfOptions = data.noopt;

                        _dbContext.Add(result1);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            try
            {
                data.editQus = _dbContext.LMS_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id && a.LMSMOES_Id == data.LMSMOES_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            bool returnresult = false;
            MasterQuestionDTO del = new MasterQuestionDTO();
            try
            {

                var delete = _dbContext.LMS_Master_OE_SettingDMO.Where(t => t.LMSMOES_Id == data.LMSMOES_Id).ToList();
                try
                {
                    if (delete.Any())
                    {
                        _dbContext.Remove(delete.ElementAt(0));

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            del.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            del.returnval = returnresult;
                        }
                    }
                }
                catch (Exception ee)
                {

                    Console.WriteLine(ee.Message);
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return del;

        }
        public async Task<MasterQuestionDTO> getreport(MasterQuestionDTO data)
        {
            try
            {

                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Online_Exam_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                        SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                       SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.result = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }



        public MasterQuestionDTO getsection(MasterQuestionDTO data)
        {
            try
            {
                //&& a.ASMAY_Id == data.ASMAY_Id
                data.getsection = (from a in _dbContext.School_Adm_Y_StudentDMO
                                    from b in _dbContext.Section
                                   where (a.ASMS_Id == b.ASMS_Id  && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_Id)
                                    select new MasterQuestionDTO
                                    {
                                        ASMS_Id = b.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,

                                    }
                        ).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public MasterQuestionDTO getonlinereport(MasterQuestionDTO data)
        {
            try
            {
                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PA_OnlineExamStudentsMarks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                   
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                     cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                     cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                       SqlDbType.VarChar)
                    {
                        Value = data.fromdate1
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                       SqlDbType.VarChar)
                    {
                        Value = data.todate1
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        data.onlinereport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }



    }
}
