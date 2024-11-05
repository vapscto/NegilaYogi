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

namespace FeeServiceHub.com.vaps.services
{
    public class FeeItReceiptReportImpl : interfaces.FeeITReceiptReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeItReceiptReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;

        }

        public FeeITReceiptDTO getdata123(FeeITReceiptDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active==true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.academicyr = year.Distinct().ToArray();

                data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                       from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmyid && a.AMST_SOL == "S")
                                       select new FeeITReceiptDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                       }
                             ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeITReceiptDTO getstuddet(FeeITReceiptDTO data)
        {
            if (data.filterinitialdata == null)
            {
                data.filterinitialdata = "NameRegNo";
            }

            try
            {
                if (data.filterinitialdata == "NameRegNo")
                {
                    data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmyid && a.AMST_SOL == "S")
                                           select new FeeITReceiptDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                               AMST_FirstName = a.AMST_FirstName,
                                               AMST_MiddleName = a.AMST_MiddleName,
                                               AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                           }
                           ).ToArray();


                }
                else if (data.filterinitialdata == "RegNoName")
                {
                    data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                           from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmyid && a.AMST_SOL == "S")
                                           select new FeeITReceiptDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                               AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
                                               AMST_MiddleName = a.AMST_MiddleName,
                                               AMST_LastName = a.AMST_LastName,
                                           }
                  ).ToArray();


                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<FeeITReceiptDTO> getreport(FeeITReceiptDTO data)
        {
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeItReceipt_studentsNames_Get";
                    cmd.CommandType = CommandType.StoredProcedure;                 
                    cmd.Parameters.Add(new SqlParameter("@amstid",
                                SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                       SqlDbType.BigInt)
                    {
                        Value = data.asmyid
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
                        data.studentsnames = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeItReceipt_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@amstid",
                                SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@yearid",
                       SqlDbType.BigInt)
                    {
                        Value = data.asmyid
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

        //On Change Academic Year
        public FeeITReceiptDTO selectacademicyear(FeeITReceiptDTO data)
        {
            try
            {
                data.admsudentslist = (from a in _FeeGroupContext.AdmissionStudentDMO
                                       from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmyid && a.AMST_SOL == "S")
                                       select new FeeITReceiptDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName,
                                           AMST_LastName = a.AMST_LastName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                       }
                             ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


    }
}
