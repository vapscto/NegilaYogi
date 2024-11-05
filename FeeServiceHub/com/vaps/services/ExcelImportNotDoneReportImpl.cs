using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class ExcelImportNotDoneReportImpl : interfaces.ExcelImportNotDoneReportInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public ExcelImportNotDoneReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public ExcelImportNotDoneReportDTO getdata123(ExcelImportNotDoneReportDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).ToList();
                data.acayear = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).OrderByDescending(y => y.ASMAY_Order).ToArray();


         

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public ExcelImportNotDoneReportDTO getsection(ExcelImportNotDoneReportDTO data)
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

        public ExcelImportNotDoneReportDTO getstudent(ExcelImportNotDoneReportDTO data)
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

        public ExcelImportNotDoneReportDTO getstuddet(ExcelImportNotDoneReportDTO data)
        {

            try
            {


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<ExcelImportNotDoneReportDTO> getreport([FromBody] ExcelImportNotDoneReportDTO data)
        {
            var retObject1 = new List<dynamic>();


            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Fee_Excel_ImportsReport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.asmay_id)
                });
                cmd.Parameters.Add(new SqlParameter("@Fromdate",
                   SqlDbType.DateTime)
                {
                    Value = data.fromdate
                });

                cmd.Parameters.Add(new SqlParameter("@Todate",
               SqlDbType.DateTime)
                {
                    Value = data.todate
                });
               


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                // var retObject = new List<dynamic>();

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

                            retObject1.Add((ExpandoObject)dataRow);
                        }

                    }

                    data.getreportdata = retObject1.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;
        }


        public ExcelImportNotDoneReportDTO deleterec(int id)
        {
            ExcelImportNotDoneReportDTO page = new ExcelImportNotDoneReportDTO();

            try
            {


                var outputval = 0;

                outputval = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Excel_ImportsDelete @p0",id);
                if (outputval > 0)
                {
                    page.returnval = true;
                }
                else
                {
                    page.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }




        public ExcelImportNotDoneReportDTO get_groups(ExcelImportNotDoneReportDTO data)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
