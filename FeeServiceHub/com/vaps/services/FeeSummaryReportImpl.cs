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
    public class FeeSummaryReportImpl : interfaces.FeeSummaryReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeSummaryReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public FeeSummaryReportDTO getdata123(FeeSummaryReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active==true).ToList();
                data.acayear = year.Distinct().ToArray();

                List<FeeGroupDMO> allpages = new List<FeeGroupDMO>();
                allpages = _FeeGroupContext.feeGroup.OrderBy(t => t.FMG_Id).ToList();
                data.fgrp = allpages.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<FeeSummaryReportDTO> getreport(FeeSummaryReportDTO data)
        {
            string grpidsget = "";
            for (int i = 0; i < data.TempararyArrayListnew.Length; i++)
            {
              
                string Id = data.TempararyArrayListnew[i].FMG_Id.ToString();
                if (Id != "0" && Id != null)
                {

                    grpidsget = Id + "," + grpidsget;
                }
            }
            try
            {
               
               
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "feesummary_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(data.Falgout=="acyearwis")
                    {
                        data.fromdate = DateTime.Now;
                        data.todate = DateTime.Now;
                    }
                    else
                    {

                    }
                    cmd.Parameters.Add(new SqlParameter("@yerid",
                      SqlDbType.VarChar)
                    {
                        Value = data.yerid
                    });
                    cmd.Parameters.Add(new SqlParameter("@fypfromdate",
                       SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@fyptodate",
                                   SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@groupids",
                                 SqlDbType.VarChar)
                    {
                        Value = grpidsget
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                               SqlDbType.VarChar)
                    {
                        Value = data.FalgIn
                    });
                    cmd.Parameters.Add(new SqlParameter("@flagDate",
                              SqlDbType.VarChar)
                    {
                        Value = data.Falgout
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
