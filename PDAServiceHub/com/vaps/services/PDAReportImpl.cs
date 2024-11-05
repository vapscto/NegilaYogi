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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace PDAServiceHub.com.vaps.services
{
    public class PDAReportImpl : PDAReportInterface
    {

        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
   new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDAReportImpl> _logger;
        public PDAReportImpl(PDAContext frgContext, ILogger<PDAReportImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getalldetails(PDATransactionDTO data)
        {
        

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classlst = new List<School_M_Class>();
                classlst = _PdaheadContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMCL_Id).ToList();
                data.classlist = classlst.ToArray();

                //List<School_M_Section> sectionlst = new List<School_M_Section>();
                //sectionlst = _PdaheadContext.school_M_Section.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMS_Id).ToList();
                //data.searcharray = sectionlst.ToArray();

             //   data.fillstudent = (       from c in _PdaheadContext.AdmissionStudentDMO
             //                              from d in _PdaheadContext.School_Adm_Y_StudentDMO
             //                              from e in _PdaheadContext.School_M_Class
             //                              from f in _PdaheadContext.school_M_Section
             //                              where (c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id  && d.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1 && c.MI_Id==data.MI_ID && d.ASMAY_Id==data.ASMAY_Id)
             //                              select new PDATransactionDTO
             //                              {
             //                                  Amst_Id = c.AMST_Id,
             //                                  AMST_FirstName = c.AMST_FirstName,
             //                                  AMST_MiddleName = c.AMST_MiddleName,
             //                                  AMST_LastName = c.AMST_LastName,
             //                                  classname = e.ASMCL_ClassName,
             //                                  sectionname = f.ASMC_SectionName,
             //                                  AMST_AdmNo = c.AMST_AdmNo
             //                              }
             //).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public PDATransactionDTO getsection(PDATransactionDTO data)
        {
            
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsection = (from a in _PdaheadContext.School_Adm_Y_StudentDMO
                                    from b in _PdaheadContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName
                                    }
                          ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public PDATransactionDTO getstudent(PDATransactionDTO data)
        {
           
            try
            {

                if (data.AMSC_Id!=0)
                {
                    var fillstudent = (from m in _PdaheadContext.AdmissionStudentDMO
                                       from n in _PdaheadContext.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.AMSC_Id
                                       select new FeeTransactionPaymentDTO
                                       {
                                           Amst_Id = m.AMST_Id,
                                           ASMAY_Id = m.ASMAY_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                           AMST_MiddleName = m.AMST_MiddleName,
                                           AMST_LastName = m.AMST_LastName,
                                           AMST_AdmNo = m.AMST_AdmNo

                                       }).Distinct().ToList();

                    if (fillstudent.Count > 0)
                    {
                        data.fillstudent = fillstudent.OrderBy(t => t.AMST_FirstName).ToArray();
                       
                    }
                }
                else
                {
                    var fillstudent = (from m in _PdaheadContext.AdmissionStudentDMO
                                       from n in _PdaheadContext.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id 
                                       select new FeeTransactionPaymentDTO
                                       {
                                           Amst_Id = m.AMST_Id,
                                           ASMAY_Id = m.ASMAY_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + " " + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + " " + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                           AMST_MiddleName = m.AMST_MiddleName,
                                           AMST_LastName = m.AMST_LastName,
                                           AMST_AdmNo = m.AMST_AdmNo

                                       }).Distinct().ToList();

                    if (fillstudent.Count > 0)
                    {
                        data.fillstudent = fillstudent.OrderBy(t => t.AMST_FirstName).ToArray();
                       
                    }
                }
             


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        
        public async Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data)
        {
            try
            {
                var fmh_id = _PdaheadContext.feehead.Where(t => t.FMH_PDAFlag == true && t.MI_Id == data.MI_ID).ToList();

                List<long> fmh_ids = new List<long>();
                List<long> Amst_ids = new List<long>();

                foreach (var item in fmh_id)
                {
                    fmh_ids.Add(Convert.ToInt64(item.FMH_Id));
                }


                if (data.AMSC_Id != 0)
                {
                    var fillstudent = (from m in _PdaheadContext.AdmissionStudentDMO
                                       from n in _PdaheadContext.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.AMSC_Id
                                       select new PDATransactionDTO
                                       {
                                           Amst_Id = m.AMST_Id,
                                       }).Distinct().ToList();

                    foreach (var item1 in fillstudent)
                    {
                        Amst_ids.Add(Convert.ToInt64(item1.Amst_Id));
                    }
                }
                else
                {
                    var fillstudent = (from m in _PdaheadContext.AdmissionStudentDMO
                                       from n in _PdaheadContext.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_ID && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                       select new PDATransactionDTO
                                       {
                                           Amst_Id = m.AMST_Id,
                                       }).Distinct().ToList();

                    foreach (var item1 in fillstudent)
                    {
                        Amst_ids.Add(Convert.ToInt64(item1.Amst_Id));
                    }
                }
                
                
                using (var cmd1 = _PdaheadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Fee_Not_Paid_Students";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@asmcl_id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@asms_id",
                      SqlDbType.BigInt)
                    {
                        Value = data.AMSC_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
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
                        data.transnumconfig = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                
                data.searcharray = (from a in _PdaheadContext.FeeStudentTransactionDMO
                                   where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_ID && fmh_ids.Contains(a.FMH_Id) && Amst_ids.Contains(a.AMST_Id))
                                   group a by new { a.AMST_Id } into g
                                   select new PDATransactionDTO
                                    { 
                                        Amst_Id=g.Key.AMST_Id,
                                        FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                        FSS_CurrentYrCharges=g.Sum(t=>t.FSS_ConcessionAmount)
                                    }).Distinct().ToArray();


                data.admsudentslist = (from a in _PdaheadContext.PDA_StatusDMO
                                    where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_ID && Amst_ids.Contains                  (a.AMST_Id))
                                    select new PDATransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        PDAS_CYRefundAmt=a.PDAS_CYRefundAmt,
                                        PDAS_CBStudentDue=a.PDAS_CBStudentDue,
                                        PDAS_CBExcessPaid=a.PDAS_CBExcessPaid,
                                        PDAS_OBStudentDue=a.PDAS_OBStudentDue,
                                        PDAS_OBExcessPaid=a.PDAS_OBExcessPaid
                                    }).Distinct().OrderBy(a=>a.Amst_Id).ToArray();
                
                var From_date = data.From_Date.ToString();
                var To_date = data.To_Date.ToString();
                using (var cmd = _PdaheadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PDA_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@date1",
               SqlDbType.DateTime)
                    {
                        Value = data.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@date2",
               SqlDbType.DateTime)
                    {
                        Value = data.To_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
              SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id",
                    SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSC_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMSC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                  SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@detailed",
                 SqlDbType.BigInt)
                    {
                        Value = data.studentdata
                    });
                    cmd.Parameters.Add(new SqlParameter("@stored",
                 SqlDbType.BigInt)
                    {
                        Value = data.refundamt
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

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
                        data.headwise = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch
            {

            }
            return data;
        }
    }
}
