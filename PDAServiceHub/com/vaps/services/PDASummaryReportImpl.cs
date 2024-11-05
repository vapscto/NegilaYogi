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
using System.Dynamic;
using System.Data.SqlClient;

namespace PDAServiceHub.com.vaps.services
{
    public class PDASummaryReportImpl:PDASummaryReportInterface
    {

        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDASummaryReportImpl> _logger;
        public PDASummaryReportImpl(PDAContext frgContext, ILogger<PDASummaryReportImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getalldetails(PDATransactionDTO data)
        {
            
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_ActiveFlag==1).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classlst = new List<School_M_Class>();
                classlst = _PdaheadContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMCL_Id).ToList();
                data.classlist = classlst.ToArray();


                data.pdaheadlist = _PdaheadContext.pdahead.Where(t => t.MI_Id == data.MI_ID && t.PDAMH_ActiveFlag==true).OrderBy(t => t.PDAMH_Id).ToList().ToArray();

                data.pdaheadlist = (from a in _PdaheadContext.pdahead
                                    from b in _PdaheadContext.pdaexpenditurehead
                                    where (a.PDAMH_Id==b.PDAMH_Id && a.PDAMH_ActiveFlag == true && a.MI_Id == data.MI_ID)
                                    select new PDATransactionDTO
                                    {
                                        PDAMH_Id = b.PDAMH_Id,
                                        PDAR_RefundRemarks = a.PDAMH_HeadName
                                    }
                        ).Distinct().ToArray();


                data.invheadlist = (from a in _PdaheadContext.INV_Master_GroupDMO
                                    from b in _PdaheadContext.INV_Master_ItemDMO
                                    from c in _PdaheadContext.INV_StockDMO
                                    where (a.INVMG_Id == b.INVMG_Id && b.INVMI_Id == c.INVMI_Id && a.INVMG_ActiveFlg == true && b.MI_Id == data.MI_ID)
                                    select new PDATransactionDTO
                                    {
                                        INVMG_Id = b.INVMG_Id,
                                        INVMG_GroupName = a.INVMG_GroupName
                                    }
                        ).Distinct().ToArray();

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
            List<long> GrpId = new List<long>();
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
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();


                data.fillstudent = (from a in _PdaheadContext.AdmissionStudentDMO
                                    from b in _PdaheadContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.AMSC_Id && a.AMST_SOL == "S")
                                    select new FeeTransactionPaymentDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
         ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data)
        {


            var From_date = data.From_Date.ToString();
            var To_date = data.To_Date.ToString();


            for(int a=1;a<=4;a++)
            {
                
            using (var cmd = _PdaheadContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "PDA_INV_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000;


                cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.VarChar)
                {
                    Value = data.MI_ID
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
           SqlDbType.VarChar)
                {
                    Value = data.ASMAY_Id
                });

                cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                {
                    Value = a
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
                cmd.Parameters.Add(new SqlParameter("@Amst_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
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

                    if(a==1)
                        {
                            data.invdetails = retObject.ToArray();
                        }
                    else if(a==2)
                        {
                            data.pdadetails = retObject.ToArray();
                        }
                        else if(a==3)
                        {
                            data.obdetails = retObject.ToArray();
                        }
                        else
                        {
                            data.transnumconfig= retObject.ToArray();
                        }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
                
            }





            return data;
        }


    }
}
