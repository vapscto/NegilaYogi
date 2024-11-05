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

namespace FeeServiceHub.com.vaps.services
{
    public class FeeReceiptReportImpl : interfaces.FeeReceiptReportInterface
    {


        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeReceiptReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public FeeReceiptDTO getdata123(FeeReceiptDTO data)
        {

            try
            {
               
                data.newreplist = (from bb in _FeeGroupContext.FeePaymentDetailsDMO
                                   where (bb.MI_Id == data.MI_ID)
                                   select new FeeReceiptDTO
                                   {
                                       receiptNo = bb.FYP_Receipt_No,
                                       paymentid = bb.FYP_Id,
                                   }
            ).GroupBy(r=>r.receiptNo).Select(x=>x.First()).ToArray();

            //    data.categoryarray = (from bb in _FeeGroupContext.Institutionds
            //                          where (bb.MI_Id == data.MI_ID)
            //                          select new FeeReceiptDTO
            //                          {
            //                              miid = bb.MI_Id,
            //                              miname = bb.MI_Name,
            //                          }
            //).ToArray();

                List<FeeClassCategoryDMO> fee_category = new List<FeeClassCategoryDMO>();
                fee_category = _FeeGroupContext.feeCC.Where(c => c.MI_Id == data.MI_ID && c.FMCC_ActiveFlag == true).ToList();
                data.categoryarray = fee_category.ToArray();
                //List<FeePaymentDetailsDMO> receiptnos = new List<FeePaymentDetailsDMO>();
                //receiptnos = _FeeGroupContext.FeePaymentDetailsDMO.Where(r => r.MI_Id == data.MI_ID).ToList();
                //data.newreplist=receiptnos.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).ToList();
                data.yearlist = allyear.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeReceiptDTO getinsdetils(FeeReceiptDTO data)
        {

            try
            {
              
             data.insdata = (from bb in _FeeGroupContext.Institutionds
                                      where (bb.MI_Id == data.MI_ID)
                                      select new FeeReceiptDTO
                                      {
                                          insname = bb.MI_Name,
                                          insaddress = bb.MI_Address1,
                                      }
            ).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<FeeReceiptDTO> getreport(FeeReceiptDTO data)
        {
            
                try
                {
                if (data.radioval == "ReceiptNoWise")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "feeReceiptReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                                     SqlDbType.TinyInt)
                        {
                            Value = data.MI_ID
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmyid",
                                   SqlDbType.TinyInt)
                        {
                            Value = data.acayyearid
                        });
                        cmd.Parameters.Add(new SqlParameter("@recpno",
                                  SqlDbType.VarChar)
                        {
                            Value = data.recpno
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
                else if(data.radioval== "dateWise")
                {
                    return data;
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
