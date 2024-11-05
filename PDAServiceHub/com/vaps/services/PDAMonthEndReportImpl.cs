using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
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
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.PDA;
using DataAccessMsSqlServerProvider.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.PDA;
using DomainModel.Model.com.vaps.admission;
using PDAServiceHub.com.vaps.interfaces;
using DomainModel.Model.com.vapstech.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace PDAServiceHub.com.vaps.services
{
    public class PDAMonthEndReportImpl:PDAMonthEndReportInterface
    {
        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDAMonthEndReportImpl> _logger;
        public PDAMonthEndReportImpl(PDAContext frgContext, ILogger<PDAMonthEndReportImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getdata123(PDATransactionDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();


                List<Month> mnth = new List<Month>();
                mnth = _PdaheadContext.month.Where(t => t.Is_Active == true).ToList();
                data.fillmonth = mnth.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<PDATransactionDTO> getreport(PDATransactionDTO data)
        {
          

            string confromdate = "";
            string confromdate2 = "";

            try
            {
                //DateTime date1 = DateTime.Now;

                //date1 = Convert.ToDateTime(data.frmdate.Value.Date.ToString("yyyy-MM-dd"));
                //// confromdate = fromdatecon.ToString();
                //confromdate = date1.ToString("dd/MM/yyyy");

                //DateTime date2 = DateTime.Now;

                //date2 = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                //// confromdate = fromdatecon.ToString();
                //confromdate2 = date2.ToString("dd/MM/yyyy");

                //string startmonth = "";
                //startmonth = _PdaheadContext.FeeGroupDMO.Where(a => a.user_id == data.userid && a.MI_Id == data.MI_ID).ToList().OrderBy(a => a.FMG_Id).FirstOrDefault().FMG_CompulsoryFlag;
                //data.monthpass = startmonth;

                using (var cmd = _PdaheadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PDA_Monthend_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //  cmd.Parameters.Add(new SqlParameter("@fromdate",
                    //     SqlDbType.VarChar)
                    //  {
                    //      Value = confromdate
                    //  });
                    //  cmd.Parameters.Add(new SqlParameter("@todate",
                    //SqlDbType.VarChar)
                    //  {
                    //      Value = confromdate2
                    //  });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Year
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.rolename
                    });

                  //  cmd.Parameters.Add(new SqlParameter("@type",
                  //SqlDbType.VarChar)
                  //  {
                  //      Value = data.type
                  //  });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@amay_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });
                 //   cmd.Parameters.Add(new SqlParameter("@acayid",
                 //SqlDbType.VarChar)
                 //   {
                 //       Value = data.type
                 //   });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        data.searcharray = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
