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
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeClasswiseConcessionReportImpl : interfaces.FeeClasswiseConcessionReportInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public FeeClasswiseConcessionReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeTransactionPaymentDTO getdetails(int id)
        {
            FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.ToList();
                data.adcyear = year.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true).ToList();
                data.fillmastergroup = group.ToArray();

                List<FeeHeadDMO> head = new List<FeeHeadDMO>();
                head = _FeeGroupContext.FeeHeadDMO.Where(t => t.FMH_ActiveFlag == true).ToList();
                data.fillmasterhead = head.ToArray();
                
                List<School_M_Class> classes = new List<School_M_Class>();
                classes = _FeeGroupContext.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = classes.ToArray();

                List<School_M_Section> section = new List<School_M_Section>();
                section = _FeeGroupContext.school_M_Section.ToList();
                data.fillsection = section.ToArray();

              

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



        public async Task<FeeTransactionPaymentDTO> concessiondtl([FromBody] FeeTransactionPaymentDTO temp)
        {
            List<long> GrpId = new List<long>();




            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "feeclasswiseconcession_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                   SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(temp.ASMAY_Id)
                });

                cmd.Parameters.Add(new SqlParameter("@fmg_id",
               SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(temp.FMG_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fmh_id",
                SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(temp.FMH_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
              SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(temp.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmc_id",
             SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(temp.AMSC_Id)
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
                 
                        temp.groupalldata = retObject.ToArray();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }
        }
        
    }
}
