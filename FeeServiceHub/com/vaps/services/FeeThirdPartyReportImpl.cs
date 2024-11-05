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
using DomainModel.Model.com.vaps.Fee;
using System.Collections;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeThirdPartyReportImpl : interfaces.FeeThirdPartyReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeThirdPartyReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public FeeThirdPartyReportDTO getdata123(FeeThirdPartyReportDTO data)
        {

            try
            {
              
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.usersnameslist = year.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<FeeThirdPartyReportDTO> getreport(FeeThirdPartyReportDTO data)
        {
           
            try
            {
            
               
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_ThirdParty_ReportNew";
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add(new SqlParameter("@frmdate",
                        SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                                   SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@mid",
                                 SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ayar",
                               SqlDbType.BigInt)
                    {
                        Value = data.acayid
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
