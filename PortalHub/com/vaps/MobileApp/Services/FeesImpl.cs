using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.MobileApp;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.MobileApp.Services
{
    public class FeesImpl : Interfaces.FeesInterface
    {

        private PortalContext _Feecontext;
        private FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        public FeesImpl(PortalContext Feecontext, FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _Feecontext = Feecontext;
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
        }

        public FeeDTO.getLoadData getloaddata(FeeDTO.getLoadData data)
        {
            try
            {
                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_FEE_ACADEMICYEAR_CLASS_SECTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
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
                        data.yearclsList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        public FeeDTO.getDetails Getdetails(FeeDTO.getDetails fddto)
        {
            try
            {
                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_FEE_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                  SqlDbType.BigInt)
                    {
                        Value = fddto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = fddto.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.BigInt)
                    {
                        Value = fddto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = fddto.type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        fddto.getfeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                fddto.status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                fddto.status = false;
            }
            return fddto;
        }
        public FeeDTO.feeReceiptGetLoadData feereceiptgetloaddata(FeeDTO.feeReceiptGetLoadData data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();




                data.yearlist = allyear.Distinct().ToArray();

                data.status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        public FeeDTO.getReceiptDetail getrecdetails(FeeDTO.getReceiptDetail data)
        {
            try
            {



                data.recnolist = (from a in _Feecontext.FeePaymentDetailsDMO
                                  from b in _Feecontext.Fee_Y_Payment_School_StudentDMO
                                  where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && b.AMST_Id == data.AMST_Id)
                                  orderby a.FYP_Receipt_No

                                  select new FeeDTO.getReceiptDetail
                                  {
                                      FYP_Id = a.FYP_Id,
                                      FYP_Receipt_No = a.FYP_Receipt_No
                                  }
                 ).Distinct().ToArray();
                data.status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        public FeeDTO.printReceipt printreceipt(FeeDTO.printReceipt data)
        {
            try
            {
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                string accountname = "", accesskey = "", html = "";
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }




                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PortalFeeReceipt.html", 0);
                }
                catch (Exception ex)
                { html = ""; }

                data.htmldata = html;

                if (html != "")
                {
                    if (data.minstall == "0")
                    {
                        Printterm(data);
                    }
                    else
                    {
                        Printterm(data);
                    }
                }
                else { printcommon(data); }


                data.status = true;



            }
            catch (Exception ex)
            {
                data.status = false;

                Console.WriteLine(ex.Message);
            }

            return data;

        }
        public FeeDTO.printReceipt Printterm(FeeDTO.printReceipt data)
        {
            try
            {





                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeDTO.printReceipt
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }).Distinct().ToArray();


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_fillstudentviewdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
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
                            data.fillstudentviewdetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }





                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               from h in _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                               where (h.ASMAY_ID == data.ASMAY_Id && h.FMT_Id == e.FMT_Id && e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.AMST_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeDTO.printReceipt
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = h.FMTP_Year
                               }

                 ).Distinct().OrderBy(t => t.fmt_order).ToArray();


                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0";
                fmt_id_int = feeterm[0].fmt_id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].fmt_order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.fmt_id);
                }

                List<FeeDTO.printReceipt> temp_group_head = new List<FeeDTO.printReceipt>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (b.FYP_Id == c.FYP_Id && a.AMST_Id == data.AMST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeDTO.printReceipt
                                   {
                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id
                                   }

                           ).Distinct().ToList();

                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                List<FeeDTO.printReceipt> fordate = new List<FeeDTO.printReceipt>();
                List<FeeDTO.printReceipt> fordateinfyp = new List<FeeDTO.printReceipt>();

                var nextduedate = "0";

                if (term_ids.Count() > 0)
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end) + 1).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeDTO.printReceipt
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().ToList();

                }
                else
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end)).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeDTO.printReceipt
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().ToList();
                }

                termmidsnew.Add(Convert.ToInt32(nextduedate));

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeDTO.printReceipt
                                {
                                    month = f.FTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeDTO.printReceipt item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeDTO.printReceipt itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }

                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                        data.year = nextyr.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;

                        data.year = curyear.Year.ToString();

                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();

                    maxmonth = months1.Max().ToString();
                    if (startperiod.Count >= 4)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        maxmonth = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        monthnameinitial = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
                    {
                        data.date = dates[i].ToString();
                    }
                }

                if (months.Count == 0)
                {
                    foreach (var item in startperiod)
                    {
                        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                        {
                            monthnameinitial = startperiod.Min().ToString();
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year;
                            data.year = curyear.Year.ToString();
                            // data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
                            // var nextyr = curyear.Year - 1;
                            //data.year = nextyr.ToString();
                        }
                    }
                    if (monthnameinitial != "")
                    {
                        monthnameinitial = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    }

                    if (monthnameend != "")
                    {
                        monthnameend = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        monthnameend = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                }


                var compusoryflag = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(d => d.MI_Id == data.MI_Id && grp_ids.Contains(d.FMG_Id)).Select(t => t.FMG_CompulsoryFlag).Distinct().ToArray();

                if (compusoryflag[0].ToString() == "T")
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].Transport_FromMonth.ToString();
                    monthnameend = termperiodlistend[0].Transport_ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }
                else
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                    monthnameend = termperiodlistend[0].ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }







            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        private FeeDTO.printReceipt printcommon(FeeDTO.printReceipt data)
        {
            try
            {

                var fillstudent = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                   from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                   from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                   from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   from g in _YearlyFeeGroupMappingContext.admissioncls
                                   from h in _YearlyFeeGroupMappingContext.school_M_Section
                                   from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                   from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                   where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.AMST_Id && b.FYP_Id == data.FYP_Id)
                                   select new FeeDTO.printReceipt
                                   {
                                       AMST_Id = f.AMST_Id,
                                       AMST_FirstName = i.AMST_FirstName,
                                       AMST_MiddleName = i.AMST_MiddleName,
                                       AMST_LastName = i.AMST_LastName,
                                       FMH_Id = d.FMH_Id,
                                       FMH_FeeName = d.FMH_FeeName,
                                       FTI_Name = e.FTI_Name,
                                       FTI_Id = e.FTI_Id,
                                       FYP_Receipt_No = b.FYP_Receipt_No,
                                       FTP_Paid_Amt = a.FTP_Paid_Amt,
                                       FTP_Concession_Amt = a.FTP_Concession_Amt,
                                       FTP_Waived_Amt = a.FTP_Waived_Amt,
                                       FTP_Fine_Amt = a.FTP_Fine_Amt,
                                       FYP_Date = b.FYP_Date,
                                       classname = g.ASMCL_ClassName,
                                       sectionname = h.ASMC_SectionName,
                                       rollno = f.AMAY_RollNo,
                                       admno = i.AMST_AdmNo,
                                       fathername = i.AMST_FatherName,
                                       mothername = i.AMST_MotherName,
                                       FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                                       FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                                       FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                                       FYP_Bank_Name = b.FYP_Bank_Name,
                                       FYP_Remarks = b.FYP_Remarks,
                                       AMST_RegistrationNo = i.AMST_RegistrationNo,
                                       FMCC_ConcessionName = k.FMCC_ConcessionName,
                                       totalcharges = c.FMA_Amount,
                                       fyp_transaction_id = b.fyp_transaction_id
                                   }
               ).Distinct().ToList();

                data.fillstudentviewdetails = fillstudent.ToArray();

                List<long> lstheadid = new List<long>();

                FeeDTO.printReceipt fst = new FeeDTO.printReceipt();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeDTO.printReceipt
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FTP_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                               FTP_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
              ).Distinct().ToArray();




                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.AMST_Id && a.FYP_Id == data.FYP_Id)
                               select new FeeDTO.printReceipt
                               {
                                   fmt_id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t => t.fmt_id).ToArray();

                long fmt_id_new = 0;

                long initialfmtids = Convert.ToInt64(feeterm[0].fmt_id);

                fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;

                List<FeeDTO.printReceipt> temp_group_head = new List<FeeDTO.printReceipt>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.AMST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeDTO.printReceipt
                                   {

                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id

                                   }

                           ).Distinct().ToList();
                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                List<FeeDTO.printReceipt> fordate = new List<FeeDTO.printReceipt>();
                List<FeeDTO.printReceipt> fordateinfyp = new List<FeeDTO.printReceipt>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                           select new FeeDTO.printReceipt
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeDTO.printReceipt
                                {
                                    month = f.FTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeDTO.printReceipt item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeDTO.printReceipt itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }

                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        data.year = curyear.Year.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year + 1;
                        data.year = nextyr.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    // maxmonth = (Convert.ToInt32(data.month) - 1).ToString();
                    maxmonth = months1.Max().ToString();
                    if (startperiod.Count >= 4)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        maxmonth = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        monthnameinitial = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
                    {
                        data.date = dates[i].ToString();
                    }
                }

                if (months.Count == 0)
                {
                    foreach (var item in startperiod)
                    {
                        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                        {
                            monthnameinitial = startperiod.Min().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year + 1;
                            data.year = nextyr.ToString();
                        }
                    }
                    if (monthnameinitial != "")
                    {
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    }

                    if (monthnameend != "")
                    {
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                }

                var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                monthnameinitial = termperiodlist[0].FromMonth.ToString();
                monthnameend = termperiodlist[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeDTO.dueDate getduedates(FeeDTO.dueDate data)
        {
            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_due_dates";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmayid", SqlDbType.NVarChar, 100)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getduedates = retObject1.ToArray();
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


            try
            {
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getfeereciptformat";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.FYP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.BigInt, 100)
                    {
                        Value = data.AMST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.fillstudentviewdetails = retObject1.ToArray();
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

        public FeeDTO.getFeetotalamount getFeetotalamount(FeeDTO.getFeetotalamount data)
        {
            try
            {
                List<FeeStudentTransactionDTO> customgrp = new List<FeeStudentTransactionDTO>();
                customgrp = (from a in _YearlyFeeGroupMappingContext.feegm
                             from b in _YearlyFeeGroupMappingContext.feeGGG
                             from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                             where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.ASMAY_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 FMG_Id = b.FMG_Id
                             }
         ).Distinct().ToList();

                string groupudss = "0";
                List<long> grp_ids = new List<long>();
                foreach (var item in customgrp)
                {
                    grp_ids.Add(item.FMG_Id);
                    groupudss = groupudss + ',' + item.FMG_Id;
                }



                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "GET_Fees_totalPortal";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)                    {                        Value = data.AMST_Id                    });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)                    {                        Value = data.ASMAY_Id                    });                    cmd.Parameters.Add(new SqlParameter("@GROUPID", SqlDbType.VarChar)                    {                        Value = groupudss                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject1 = new List<dynamic>();                    try                    {                        using (var dataReader1 = cmd.ExecuteReader())                        {                            while (dataReader1.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)                                {                                    dataRow1.Add(                                        dataReader1.GetName(iFiled1),                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}                                    );                                }                                retObject1.Add((ExpandoObject)dataRow1);                            }                        }                        data.filonlinepaymentgrid = retObject1.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;
        }

        public FeeDTO.feeAnalysis feeAnalysisgetloaddata(FeeDTO.feeAnalysis data)        {            try            {

                var studentfeedetails = (from c in _Feecontext.FeeStudentTransactionDMO                                         from a in _Feecontext.feeMTH                                         from b in _Feecontext.feeTr                                         from d in _Feecontext.FeeHeadDMO                                         where (a.FMT_Id == b.FMT_Id && c.FMH_Id == d.FMH_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id && c.FSS_NetAmount > 0) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                         select new FeeDTO.feeAnalysis                                         {                                             FSS_CurrentYrCharges = c.FSS_CurrentYrCharges,                                             FSS_ToBePaid = c.FSS_ToBePaid,                                             FSS_PaidAmount = c.FSS_PaidAmount,                                             FSS_ConcessionAmount = c.FSS_ConcessionAmount,                                             FTI_Name = b.FMT_Name,                                             FMH_FeeName = d.FMH_FeeName                                         }        ).ToList();                data.studentfeedetails = (from i in studentfeedetails                                          group i by new { i.FTI_Name, i.FMH_FeeName } into g                                          select new FeeDTO.feeAnalysis                                          {                                              FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),                                              FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),                                              FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),                                              FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),                                              FTI_Name = g.Key.FTI_Name,                                              FMH_FeeName = g.Key.FMH_FeeName                                          }).Distinct().ToArray();




                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@mi_id",             SqlDbType.VarChar)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@amst_id",                     SqlDbType.VarChar)                    {                        Value = data.AMST_Id                    });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",                    SqlDbType.BigInt)                    {                        Value = data.ASMAY_Id                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.feeAnalysisList = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }        public FeeDTO.feeTransactionlog feeTransactionlog(FeeDTO.feeTransactionlog data)        {            try            {                List<Translogsresults> razorpayparam = new List<Translogsresults>();                List<FeeDTO.feeTransactionlog> razorpaytransactionlogs = new List<FeeDTO.feeTransactionlog>();                data.transactionsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_M_Online_TransactionDMO                                            where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_ID == data.ASMAY_Id )                                            select new FeeDTO.feeTransactionlog                                            {                                                trans_id = a.FMOT_Trans_Id,                                                FYP_PayModeType = a.FYP_PayModeType,                                                FYP_Date = a.FMOT_Date,                                                FMOT_PayGatewayType = a.FMOT_PayGatewayType,                                                Amount = a.FMOT_Amount,                                            }     ).ToArray();
            

            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }        public FeeDTO.feeTransactionlog feeTransactiondetail(FeeDTO.feeTransactionlog data)        {            try            {                List<FeeDTO.feeTransactionlog> razorpaytransactionlogs = new List<FeeDTO.feeTransactionlog>();                razorpaytransactionlogs = (from a in _YearlyFeeGroupMappingContext.Fee_M_Online_TransactionDMO                                           where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_ID == data.ASMAY_Id && a.FMOT_PayGatewayType == data.FMOT_PayGatewayType && a.FMOT_Trans_Id == data.trans_id)                                           select new FeeDTO.feeTransactionlog                                           {                                               trans_id = a.FMOT_Trans_Id,                                               FYP_PayModeType = a.FYP_PayModeType,                                               FYP_Date = a.FMOT_Date,                                               FMOT_PayGatewayType = a.FMOT_PayGatewayType                                           }     ).ToList();                List<Translogsresults> razorpayparam = new List<Translogsresults>();


                string PaymentStatusurl = "https://api.razorpay.com/v1/orders/ID/payments";

                PaymentDetails response1 = new PaymentDetails();
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == data.FMOT_PayGatewayType).Distinct().ToList();
                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                string method = "GET";
                PaymentStatusurl = PaymentStatusurl.Replace("ID", data.trans_id);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PaymentStatusurl);
                request.Method = method.ToString();
                request.ContentLength = 0;
                request.ContentType = "application/json";

                string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                request.UserAgent = "razorpay-dot-net/" + userAgent;

                string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(authString));

                System.Net.WebResponse resp = request.GetResponseAsync().Result;
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string s = sr.ReadToEnd().Trim();
                JObject joResponse1 = JObject.Parse(s);
                JArray array1 = (JArray)joResponse1["items"];

                foreach (JObject root1 in array1)
                {
                    razorpayparam.Add(new Translogsresults
                    {
                        payment_id = (String)root1["id"],
                        responsestatuslogs = (String)root1["status"],
                        error_description = (String)root1["error_description"],
                        order_id = (String)root1["order_id"],
                        FYP_Date = razorpaytransactionlogs[0].FYP_Date,
                        FMA_Amount = (Int32)root1["amount"],
                        FYP_PayModeType = razorpaytransactionlogs[0].FYP_PayModeType,
                        FMOT_PayGatewayType = razorpaytransactionlogs[0].FMOT_PayGatewayType
                    });
                }

                data.translogresults = razorpayparam.ToArray();


            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }        public FeeDTO.gatewayRate paymentGatewayrate(FeeDTO.gatewayRate data)        {            try            {                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "GatewayRateDetails";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id",             SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                     SqlDbType.VarChar)                    {                        Value = data.FMOT_PayGatewayType                    });

                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.gatewayRatedetails = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }        private static string getAppDetailsUa()        {            List<Dictionary<string, string>> appsDetails = RazorpayClient.AppsDetails;            string appsDetailsUa = string.Empty;            foreach (Dictionary<string, string> appsDetail in appsDetails)            {                string appUa = string.Empty;                if (appsDetail.ContainsKey("title"))                {                    appUa = appsDetail["title"];                    if (appsDetail.ContainsKey("version"))                    {                        appUa += appsDetail["version"];                    }                }                appsDetailsUa += appUa;            }            return appsDetailsUa;        }
    }
}
