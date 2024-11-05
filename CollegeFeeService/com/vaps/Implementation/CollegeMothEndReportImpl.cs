using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vapstech.admission;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeMothEndReportImpl : Interfaces.CollegeMothEndReportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public CollegeMothEndReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public FeeMonthEndReportDTO getdata123(FeeMonthEndReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(y => y.ASMAY_Order).ToList();
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
                //DateTime date1 = DateTime.Now;

                //date1 = Convert.ToDateTime(data.frmdate.Value.Date.ToString("yyyy-MM-dd"));
                //// confromdate = fromdatecon.ToString();
                //confromdate = date1.ToString("dd/MM/yyyy");

                //DateTime date2 = DateTime.Now;

                //date2 = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                //// confromdate = fromdatecon.ToString();
                //confromdate2 = date2.ToString("dd/MM/yyyy");

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "college_month_end_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.yearid
                    });

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
                
                    cmd.Parameters.Add(new SqlParameter("@user_id",
                 SqlDbType.BigInt)
                    {
                        Value = data.userid
                    });

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
