using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Services
{
    public class PAOnlineExamConfigImpl : Interface.PAOnlineExamConfigInterface
    {
        ILogger<PAOnlineExamConfigImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public PAOnlineExamConfigImpl(DomainModelMsSqlServerContext dbcontext, ILogger<PAOnlineExamConfigImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public PAOnlineExamConfigDTO getloaddata(PAOnlineExamConfigDTO data)
        {
            try
            {
                data.getQdetails = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PAOnlineExamConfigDTO savedetails(PAOnlineExamConfigDTO data)
        {
            try
            {
                if (data.PAMOES_Id > 0)
                {
                    var result = _dbContext.PA_Master_OE_SettingDMO.Single(t => t.PAMOES_Id == data.PAMOES_Id);
                    result.MI_Id = data.MI_Id;
                    result.PAMOES_NoofQns = data.Noofques;
                    result.PAMOES_TotalMarks = data.totmrks;
                    result.PAMOES_TotalDuration = data.totdur;
                    result.PAMOES_EachQnsMarks = data.echmrkques;
                    result.PAMOES_EachQnsDuration = data.echquesdur;
                    result.PAMOES_NoOfOptions = data.noopt;
                    result.UpdatedDate = DateTime.Now;
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

                    var res = _dbContext.PA_Master_OE_SettingDMO.Where(t => t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        var result = _dbContext.PA_Master_OE_SettingDMO.Single(t => t.MI_Id == data.MI_Id);
                        result.PAMOES_NoofQns = data.Noofques;
                        result.PAMOES_TotalMarks = data.totmrks;
                        result.PAMOES_TotalDuration = data.totdur;
                        result.PAMOES_EachQnsMarks = data.echmrkques;
                        result.PAMOES_EachQnsDuration = data.echquesdur;
                        result.PAMOES_NoOfOptions = data.noopt;
                        result.UpdatedDate = DateTime.Now;
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

                        PA_Master_OE_SettingDMO result1 = new PA_Master_OE_SettingDMO();

                        result1.MI_Id = data.MI_Id;
                        result1.PAMOES_NoofQns = data.Noofques;
                        result1.PAMOES_TotalMarks = data.totmrks;
                        result1.PAMOES_TotalDuration = data.totdur;
                        result1.PAMOES_EachQnsMarks = data.echmrkques;
                        result1.PAMOES_EachQnsDuration = data.echquesdur;
                        result1.PAMOES_NoOfOptions = data.noopt;
                        result1.UpdatedDate = DateTime.Now;
                        result1.CreatedDate = DateTime.Now;

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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PAOnlineExamConfigDTO editQuestion(PAOnlineExamConfigDTO data)
        {
            try
            {
                data.editQus = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOES_Id == data.PAMOES_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PAOnlineExamConfigDTO Deletedetails(PAOnlineExamConfigDTO data)
        {
            bool returnresult = false;
            PAOnlineExamConfigDTO del = new PAOnlineExamConfigDTO();
            try
            {

                var delete = _dbContext.PA_Master_OE_SettingDMO.Where(t => t.PAMOES_Id == data.PAMOES_Id).ToList();
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
        public async Task<PAOnlineExamConfigDTO> getreport(PAOnlineExamConfigDTO data)
        {
            try
            {

                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Online_Exam_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
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
    }
}
