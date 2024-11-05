using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MeritListReportImpl : Interfaces.MeritListReportInterface
    {
        public ScheduleReportContext _SReportContext;
        public enquiryreportContext _SEEReportContext;
        public StudentApplicationContext _StudentApplicationContext;
        public MeritListReportImpl(ScheduleReportContext DomainModelContext, enquiryreportContext DomainModelContext12, StudentApplicationContext StudentApplicationContext)
        {
            _SReportContext = DomainModelContext;
            _SEEReportContext = DomainModelContext12;
            _StudentApplicationContext = StudentApplicationContext;
        }

        public SMSReportDTO getdetails(SMSReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).ToList();
                data.yeardropDown = year.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<SMSReportDTO> Getreportdetails(SMSReportDTO data)
        {
            try
            {
                List<SMSReportDTO> result = new List<SMSReportDTO>();
                //to get data according to search criteria.
                using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "Get_WrittenTest_MeritList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.asmayid });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.mid });
              
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SMSReportDTO
                                {
                                    pasr_id = Convert.ToInt64(dataReader["PASR_Id"]),
                                    studentFName = dataReader["PASR_FirstName"].ToString(),
                                    studentMName = dataReader["PASR_MiddleName"].ToString(),
                                    studentLName = dataReader["PASR_LastName"].ToString(),
                                    obtainedmarks =Convert.ToDecimal(dataReader["PASR_TotalMarksScored"].ToString()),
                                    passfail_flag = dataReader["PASR_Status"].ToString(),
                                });
                                data.meritlistdata = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;

        }
    }
}
