using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class MonthEndReportImpl : interfaces.MonthendReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        public MonthEndReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeMonthEndReportDTO getdata123(FeeMonthEndReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active==true).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.acayear = year.ToArray();

                List<Month> mnth = new List<Month>();
                mnth = _FeeGroupContext.month.Where(t => t.Is_Active == true).ToList();
                data.fillmonth = mnth.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<FeeMonthEndReportDTO> getreport(FeeMonthEndReportDTO data)
        {
            List<FeeMonthEndReportDTO> AllInOne = new List<FeeMonthEndReportDTO>();

            string confromdate = "";
            string confromdate2 = "";

            try
            {
                string startmonth = "";
                startmonth = _FeeGroupContext.FeeGroupDMO.Where(a => a.user_id == data.userid && a.MI_Id == data.MI_ID).ToList().OrderBy(a => a.FMG_Id).FirstOrDefault().FMG_CompulsoryFlag;
                data.monthpass = startmonth;

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Feemonthend_report_1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                  SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@amay_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.yearid
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.userid
                    });
                    cmd.Parameters.Add(new SqlParameter("@acayid",
                 SqlDbType.VarChar)
                    {
                        Value = data.acayid
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
                        data.reportdatelist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
