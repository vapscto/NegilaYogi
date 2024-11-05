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
    public class PDARefundableReportImpl:PDARefundableReportInterface
    {
        private static ConcurrentDictionary<string, PDATransactionDTO> _login =
new ConcurrentDictionary<string, PDATransactionDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDARefundableReportImpl> _logger;
        public PDARefundableReportImpl(PDAContext frgContext, ILogger<PDARefundableReportImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PDATransactionDTO getalldetails(PDATransactionDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _PdaheadContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.ASMAY_Id == data.ASMAY_Id).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classlst = new List<School_M_Class>();
                classlst = _PdaheadContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMCL_Id).ToList();
                data.classlist = classlst.ToArray();

                List<School_M_Section> sectionlst = new List<School_M_Section>();
                sectionlst = _PdaheadContext.school_M_Section.Where(t => t.MI_Id == data.MI_ID).OrderBy(t => t.ASMS_Id).ToList();
                data.searcharray = sectionlst.ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public async Task<PDATransactionDTO> radiobtndata(PDATransactionDTO data)
        {


            var From_date = data.From_Date.ToString();
            var To_date = data.To_Date.ToString();
            using (var cmd = _PdaheadContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "PDA_Headwise_Report";
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
                cmd.Parameters.Add(new SqlParameter("@PDAMH_Id",
                SqlDbType.VarChar)
                {
                    Value = data.PDAMH_Id
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
            return data;
        }
    }
}
