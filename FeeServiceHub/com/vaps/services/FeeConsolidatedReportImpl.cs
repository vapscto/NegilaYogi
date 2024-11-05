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
    public class FeeConsolidatedReportImpl : interfaces.FeeConsolidatedReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeConsolidatedReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public FeeConsolidatedReportDTO getdata123(FeeConsolidatedReportDTO data)
        {
           

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active==true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.classlist = allclas.ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag==1).ToList();
                data.sectionlist = allsetion.ToArray();


                data.fillinstallment = (from a in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                        from b in _FeeGroupContext.FeeInstallmentDMO
                                        from c in _FeeGroupContext.FeeAmountEntryDMO
                                        from d in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        where (a.FMI_Id == b.FMI_Id && b.MI_Id == data.MI_Id && b.FMI_ActiceFlag == true && a.FTI_Id == c.FTI_Id && c.FMG_Id == d.FMG_ID && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && d.User_Id == data.userid)

                                        select new FeeTransactionPaymentDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name,

                                        }
                  ).Distinct().OrderBy(t => t.FTI_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<FeeConsolidatedReportDTO> getreport(FeeConsolidatedReportDTO data)
        {
           
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_ClassWise_Report_1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                               SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.acyrid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                   SqlDbType.VarChar)
                    {
                        Value = data.claspass
                    });
                  
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                               SqlDbType.VarChar)
                    {
                        Value = data.secidpass
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                              SqlDbType.VarChar)
                    {
                        Value = data.flagrpt
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
      

    }
}
