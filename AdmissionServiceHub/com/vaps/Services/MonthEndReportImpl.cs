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
using DomainModel.Model.com.vapstech.admission;


namespace FeeServiceHub.com.vaps.services
{
    public class monthendreportImpl : interfaces.monthendreportInterface
    {


        public monthendreportContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public monthendreportImpl(monthendreportContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public MonthEndReportDTO getdata123(MonthEndReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_ID && t.Is_Active==true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();


                List<Month> mnth = new List<Month>();
                mnth = _FeeGroupContext.mnth.Where(t => t.Is_Active == true).ToList();
                data.Month_array = mnth.ToArray();


                var cat = _FeeGroupContext.GenConfig.Where(g => g.MI_Id == data.MI_ID && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    data.category_list = _FeeGroupContext.mastercategory.Where(f => f.MI_Id == data.MI_ID && f.AMC_ActiveFlag == 1).ToArray();
                    data.categoryflag = true;
                }
                else
                {
                    data.categoryflag = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<MonthEndReportDTO> getreport(MonthEndReportDTO data)
        {
            List<MonthEndReportDTO> AllInOne = new List<MonthEndReportDTO>();
            try
            {
                #region
                //FeeMonthEndReportDTO temp = new FeeMonthEndReportDTO();
                //List<FeePaymentDetailsDMO> BCount = new List<FeePaymentDetailsDMO>();
                //BCount = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.FYP_Date >= (data.frmdate) && t.FYP_Date <= (data.todate)).ToList();
                //if (BCount.Count != 0)
                //{
                //    temp.bankcount = BCount[0].ToString();
                //    if (BCount.Count == 2)
                //    {
                //        temp.cashcount = BCount[1].ToString();
                //        if (BCount.Count == 3)
                //        {
                //            temp.onlinecount = BCount[2].ToString();
                //            if (BCount.Count == 4)
                //            {
                //                temp.esccount = BCount[3].ToString();
                //            }
                //            else
                //            {
                //                temp.esccount = "0";
                //            }
                //        }
                //        else
                //        {
                //            temp.onlinecount = "0";
                //        }

                //    }
                //    else
                //    {
                //        temp.cashcount = "0";
                //    }
                //}
                //else
                //{
                //    temp.bankcount = "0";
                //    temp.cashcount = "0";
                //    temp.onlinecount = "0";
                //    temp.esccount = "0";
                //}

                //List<FeePaymentDetailsDMO> Ccount = new List<FeePaymentDetailsDMO>();
                //Ccount = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.FYP_Bank_Or_Cash.Equals("C") && t.FYP_Date >= (data.frmdate) && t.FYP_Date <= (data.todate)).ToList();
                //if (Ccount.Count != 0)
                //{
                //    temp.cashcount = Ccount[0].ToString();
                //}
                //else
                //{
                //    temp.cashcount = "0";
                //}            

                //List<FeePaymentDetailsDMO> Ocount = new List<FeePaymentDetailsDMO>();
                //Ocount = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.FYP_Bank_Or_Cash.Equals("O") && t.FYP_Date >= (data.frmdate) && t.FYP_Date <= (data.todate)).ToList();
                //if (Ocount.Count != 0)
                //{
                //    temp.onlinecount = Ocount[0].ToString();
                //}
                //else
                //{
                //    temp.onlinecount = "0";
                //}  
                //List<FeePaymentDetailsDMO> Ecount = new List<FeePaymentDetailsDMO>();
                //Ecount = _FeeGroupContext.FeePaymentDetailsDMO.Where(t => t.FYP_Bank_Or_Cash.Equals("E") && t.FYP_Date >= (data.frmdate) && t.FYP_Date <= (data.todate)).ToList();
                //if (Ecount.Count != 0)
                //{
                //    temp.esccount = Ecount[0].ToString();
                //}
                //else
                //{
                //    temp.esccount = "0";
                //}


                //AllInOne.Add(temp);
                #endregion


                if (data.AMC_Id == null || data.AMC_Id == 0)
                {
                    data.AMC_Id = 0;

                }


                var amcid = data.AMC_Id.ToString();

                data.AMC_logo = _FeeGroupContext.mastercategory.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.MI_ID && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();




                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admissionmonthend_report_category";
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add(new SqlParameter("@Year",
                       SqlDbType.VarChar)
                    {
                        Value = data.acayid
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });
                    cmd.Parameters.Add(new SqlParameter("@amay",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMC_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.AMC_Id
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
