using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.IO;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeStatusReportImpl : interfaces.FeeStatusReportInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public FeeStatusReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();


                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.adcyear = year.ToArray();

              


            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
         public FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data)
        {

            try
            {
                data.section_list = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                     from b in _FeeGroupContext.school_M_Section
                                     where a.ASMS_Id == b.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID && a.AMAY_ActiveFlag==1 && a.ASMAY_Id==data.ASMAY_Id
                                     select b).Distinct().ToArray();
                                    
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public  FeeTransactionPaymentDTO get_fee_and_stu([FromBody] FeeTransactionPaymentDTO data)
        {

            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Feeheads_student_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    }); cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (  dataReader.Read())
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

                        data.get_studentlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Feeheads_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    }); cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
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

                        data.feeheads = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }

                data.class_list = _FeeGroupContext.School_M_Class.Where(a => a.MI_Id == data.MI_ID && a.ASMCL_ActiveFlag==true).ToArray();
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public  FeeTransactionPaymentDTO getfeehead_statement_report([FromBody] FeeTransactionPaymentDTO data)
        {

            try
            {

                data.institutionDetails = _FeeGroupContext.Institution.Where(t => t.MI_Id.Equals(data.MI_ID)).ToArray();

               
             


                string FMH_Id1 = "0";
                foreach (var item in data.feeheadarray)
                {
                    FMH_Id1 = FMH_Id1 + ',' + item.FMH_Id;
                }

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Feeheads_student_statement_report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                       SqlDbType.VarChar)
                    {
                        Value = FMH_Id1
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
                     cmd.Parameters.Add(new SqlParameter("@typeflag",
                       SqlDbType.VarChar)
                    {
                        Value = data.typeflag
                     });  cmd.Parameters.Add(new SqlParameter("@Studying",
                       SqlDbType.BigInt)
                    {
                        Value = data.studying
                     });

                    cmd.Parameters.Add(new SqlParameter("@Left",
                       SqlDbType.BigInt)
                    {
                        Value = data.left
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (  dataReader.Read())
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

                        data.getfeedheadlist = retObject.ToArray();

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
        public async Task<FeeTransactionPaymentDTO> radiobtndata([FromBody] FeeTransactionPaymentDTO temp)
        {
            List<long> GrpId = new List<long>();

            


            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "feestatus_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                   SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(temp.ASMAY_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@fromdate",
               SqlDbType.DateTime)
                {
                    Value = temp.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@todate",
                SqlDbType.DateTime)
                {
                    Value = temp.To_Date
                });
                cmd.Parameters.Add(new SqlParameter("@type",
              SqlDbType.VarChar)
                {
                    Value = temp.type
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
                    if (temp.type == "group")
                    {
                        temp.groupalldata = retObject.ToArray();
                    }
                    else if (temp.type == "head")
                    {
                        temp.headalldata = retObject.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }
        }

        //===================fee status report
        public FeeTransactionPaymentDTO GetSettlementReport(FeeTransactionPaymentDTO data)
        {

            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SettlementDiffFeeCollection_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                       SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                       SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
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

                        data.feestatusreportlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
