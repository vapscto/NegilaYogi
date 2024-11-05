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
using DomainModel.Model.com.vapstech.Transport;

namespace FeeServiceHub.com.vaps.services
{
    public class DateWiseFeeCollectionImpl : interfaces.DateWiseFeeCollectionInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public DateWiseFeeCollectionImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public CategoryWiseFeeCollectionDTO getdetails(CategoryWiseFeeCollectionDTO data)
        {
            try
            {


                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();


                data.studentname = (from a in _FeeGroupContext.Adm_M_Student

                                    where (a.AMST_ActiveFlag == 1 && a.MI_Id == data.MI_ID)
                                    select new CategoryWiseFeeCollectionDTO
                                    {
                                        amstid = a.AMST_Id,
                                        AMST_Firstname = a.AMST_FirstName,
                                    }
                          ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        public async Task<CategoryWiseFeeCollectionDTO> onchangeacademic([FromBody] CategoryWiseFeeCollectionDTO temp)
        {


            try
            {

                temp.studentname = (from a in _FeeGroupContext.Adm_M_Student
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.MI_Id == temp.MI_ID && b.ASMAY_Id == temp.ASMAY_Id)
                                    select new CategoryWiseFeeCollectionDTO
                                    {
                                        amstid = a.AMST_Id,
                                        AMST_Firstname = a.AMST_FirstName,
                                    }
                          ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return temp;
        }



        public async Task<CategoryWiseFeeCollectionDTO> radiobtndata([FromBody] CategoryWiseFeeCollectionDTO temp)
        {
            //var amsay_id = "";
            //foreach (var x in temp.ASMAY_Ids)
            //{
            //    amsay_id += x + ",";
            //}
            //amsay_id = amsay_id.Substring(0, (amsay_id.Length - 1));
         
            try { 
            if (temp.type == "date")
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                       // [Datewisefeeamtdetailstemp1]
        cmd.CommandText = "Datewisefeeamountdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                        {
                            Value = temp.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = temp.MI_ID
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.DateTime)
                    {
                        Value = temp.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                SqlDbType.DateTime)
                    {
                        Value = temp.todate
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader =  cmd.ExecuteReader())
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
                        temp.studentalldata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }

            }
            else if (temp.type == "month")
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MonthWisefeeamountdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.VarChar)
                    {
                        Value = temp.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = temp.ASMAY_Id
                    });
                    //  cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    //SqlDbType.VarChar)
                    //  {
                    //      Value = temp.amstid
                    //  });

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
                        temp.studentalldata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }

            }
            else if (temp.type == "year")
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "yearwisetotalamt";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.VarChar)
                    {
                        Value = temp.MI_ID
                    });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = 2
                        });
                        //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //   SqlDbType.VarChar)
                        //{
                        //    Value = temp.ASMAY_Id
                        //});
                        //  cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                        //SqlDbType.VarChar)
                        //  {
                        //      Value = temp.amstid
                        //  });

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
                        temp.studentalldata = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return temp;
                }

            }
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return temp;

        }
    }
}
