using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeReceiptImportStthomasImpl : interfaces.FeeReceiptImportStthomasInterface
    {
        public FeeGroupContext _FeeGroupHeadContext;
        public DomainModelMsSqlServerContext _context;
        public FeeReceiptImportStthomasImpl(FeeGroupContext context, DomainModelMsSqlServerContext contextnew)
        {
            _FeeGroupHeadContext = context;
            _context = contextnew;
        }

        public async Task<FeeReceiptImportStthomasDTO> Savedata(FeeReceiptImportStthomasDTO data)
        {
            try
            {
                data.dupcnt = 0;
                data.failcnt = 0;
                data.suscnt = 0;
                var cccccc = 3;
                long fineamtdet = 0;
                try
                {


                    if (data.FeeReceiptimport != null && data.FeeReceiptimport.Length > 0)
                    {

                        var parid = 0;
                        foreach (var item in data.FeeReceiptimport)
                        {


                            var Amstidnew = _FeeGroupHeadContext.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_AdmNo == item.AMST_AdmNo).ToList(); ;

                            if (Amstidnew.Count > 0)

                            {
                                long Amstid = 0;
                                Amstid = Amstidnew.FirstOrDefault().AMST_Id;
                                if (Amstid > 0)
                                {
                                    List<FeeReceiptImportStthomasDTO> employeedetails = new List<FeeReceiptImportStthomasDTO>();


                                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "FeeImportFineCalculation";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                                SqlDbType.VarChar)
                                        {
                                            Value = data.ASMAY_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                SqlDbType.VarChar)
                                        {
                                            Value = data.MI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                                                SqlDbType.VarChar)
                                        {
                                            Value = Amstid
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FYP_Date",
                                         SqlDbType.DateTime)
                                        {
                                            Value = Convert.ToDateTime(item.FYP_Date)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FYP_Amount",
                                      SqlDbType.Decimal)
                                        {
                                            Value = item.FYP_Tot_Amount
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
                                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                                        );
                                                    }



                                                    employeedetails.Add(new FeeReceiptImportStthomasDTO
                                                    {
                                                        FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                                        FMT_Id = Convert.ToInt64(dataReader["FMT_Id"].ToString())
                                                    });
                                                    fineamtdet = Convert.ToInt64(dataReader["FSS_FineAmt"].ToString());

                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            data.FYP_Receipt_No = "L";
                                        }
                                    }

                                    long datewisefineamount = 0;

                                    if (employeedetails.Count > 0)
                                    {
                                        if (item.FYP_Tot_Amount >= employeedetails[0].FSS_TobePaid)
                                        {
                                            List<FeeReceiptImportStthomasDTO> Feepaymentpendingdet = new List<FeeReceiptImportStthomasDTO>();
                                            var sqlConnectionString = "Data Source=stthomas.database.windows.net,1433;Initial Catalog=sthomas;Persist Security Info=False;User ID=stthomas;Password=Vaps@123;Connection Timeout=30;";


                                            //var sqlConnectionString = "Data Source=172.16.32.20;Initial Catalog=StThomas_Test;Integrated Security=False;User ID=sa;Password=vts@123;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";

                                            DataSet ds = new DataSet();
                                            long Total_FSS_TobePaid = 0;

                                            try
                                            {
                                                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                                {

                                                    using (SqlConnection conn = new SqlConnection(sqlConnectionString))
                                                    {
                                                        SqlCommand sqlComm = new SqlCommand("FeeImportInstallmentDetails_test", conn);
                                                        sqlComm.Parameters.AddWithValue("@ASMAY_Id", data.ASMAY_Id);
                                                        sqlComm.Parameters.AddWithValue("@MI_Id", data.MI_Id);
                                                        sqlComm.Parameters.AddWithValue("@AMST_Id", Amstid);
                                                        sqlComm.Parameters.AddWithValue("@FMT_Id", employeedetails[0].FMT_Id);
                                                        sqlComm.Parameters.AddWithValue("@Import_FSS_Paid", employeedetails[0].FSS_TobePaid);

                                                        sqlComm.CommandType = CommandType.StoredProcedure;

                                                        SqlDataAdapter da = new SqlDataAdapter(sqlComm);
                                                        //da.SelectCommand = sqlComm;

                                                        da.Fill(ds);
                                                    }
                                                }
                                                foreach (DataTable table in ds.Tables)
                                                {
                                                    foreach (DataRow dr in table.Rows)
                                                    {
                                                        Feepaymentpendingdet.Add(new FeeReceiptImportStthomasDTO
                                                        {
                                                            FSS_TobePaid = Convert.ToInt64(dr["FSS_Tobepaid"]),
                                                            FMG_Id = Convert.ToInt64(dr["FMG_Id"]),
                                                            FMH_Id = Convert.ToInt64(dr["FMH_Id"]),
                                                            FMA_Id = Convert.ToInt64(dr["FMA_Id"]),
                                                            FTI_Id = Convert.ToInt64(dr["FTI_Id"]),
                                                        });

                                                        Total_FSS_TobePaid += Convert.ToInt64(dr["FSS_Tobepaid"]);
                                                    }
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                data.FYP_Receipt_No = "N";
                                            }


                                            data.FMG_Id = Feepaymentpendingdet[0].FMG_Id;


                                            var ASMCLid = _FeeGroupHeadContext.SchoolYearWiseStudent.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == Amstid).Select(t => t.ASMCL_Id).Single();
                                            var FMCC_Id = (from a in _FeeGroupHeadContext.feeCC
                                                           from b in _FeeGroupHeadContext.feeYCC
                                                           from c in _FeeGroupHeadContext.feeYCCC
                                                           where (a.FMCC_Id == b.FMCC_Id && c.FYCC_Id == b.FYCC_Id
                                                           && c.ASMCL_Id == ASMCLid && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                                           select b.FMCC_Id).Distinct().Single();


                                            int j = 0;
                                            string list_values = "";
                                            data.FYP_Receipt_No = "Q";
                                            FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                                            if (item.FYP_Tot_Amount > 0)
                                            {
                                                pmm.MI_Id = data.MI_Id;
                                                pmm.ASMAY_ID = data.ASMAY_Id;
                                                pmm.user_id = data.UserId;
                                                //pgmod.temp_head_list = pgmod.savetmpdata;

                                                pmm.FYP_Date = Convert.ToDateTime(item.FYP_Date);

                                                //multimode


                                                if (item.FYP_Tot_Amount > 0)
                                                {

                                                    pmm.FYP_Bank_Name = "";
                                                    pmm.FYP_DD_Cheque_Date = Convert.ToDateTime(item.FYP_Date);
                                                    pmm.FYP_DD_Cheque_No = "";
                                                    pmm.FYP_Bank_Or_Cash = item.FYP_Bank_Or_Cash;

                                                }


                                                pmm.FYP_Chq_Bounce = "CL";
                                                pmm.FYP_Remarks = "Fee Receipt Imported";
                                                pmm.FTCU_Id = 1;
                                                pmm.FYP_Tot_Amount = item.FYP_Tot_Amount;
                                                pmm.FYP_Tot_Concession_Amt = 0;
                                                pmm.FYP_Tot_Fine_Amt = 0;
                                                pmm.FYP_Tot_Waived_Amt = 0;
                                                pmm.DOE = DateTime.Now;
                                                pmm.CreatedDate = DateTime.Now;
                                                pmm.UpdatedDate = DateTime.Now;
                                                pmm.FYP_OnlineChallanStatusFlag = "Sucessfull";
                                                pmm.FYP_PayModeType = "APP";
                                                pmm.FYP_PayGatewayType = "";

                                                get_grp_reptno(data);

                                                pmm.FYP_Receipt_No = data.FYP_Receipt_No;
                                                //  pmm.FYP_Receipt_No = item.FYP_Receipt_No;
                                            }

                                            _FeeGroupHeadContext.FeePaymentDetailsDMO.Add(pmm);

                                            //Multimode of Payment

                                            pmm.FYP_DeviseFlg = "Single";


                                            if (item.FYP_Tot_Amount > 0)
                                            {
                                                Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
                                                obj2.FYP_Id = pmm.FYP_Id;

                                                obj2.FYP_TransactionTypeFlag = item.FYP_Bank_Or_Cash;

                                                obj2.FYPPM_TotalPaidAmount = item.FYP_Tot_Amount;
                                                obj2.FYPPM_LedgerId = 0;
                                                obj2.FYPPM_BankName = "";
                                                obj2.FYPPM_DDChequeNo = "";
                                                obj2.FYPPM_DDChequeDate = Convert.ToDateTime(item.FYP_Date);
                                                obj2.FYPPM_TransactionId = "";
                                                obj2.FYPPM_PaymentReferenceId = "";
                                                obj2.FYPPM_ClearanceStatusFlag = "0";
                                                obj2.FYPPM_ClearanceDate = Convert.ToDateTime(item.FYP_Date);
                                                _FeeGroupHeadContext.Add(obj2);
                                            }



                                            if (item.FYP_Tot_Amount > 0)
                                            {

                                                Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

                                                temppayment.AMST_Id = Amstid;
                                                temppayment.ASMAY_Id = pmm.ASMAY_ID;
                                                temppayment.FTP_TotalPaidAmount = item.FYP_Tot_Amount;
                                                temppayment.FTP_TotalWaivedAmount = 0;
                                                temppayment.FTP_TotalConcessionAmount = 0;
                                                temppayment.FTP_TotalFineAmount = 0;
                                                temppayment.FYP_Id = pmm.FYP_Id;
                                                _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);

                                                long fineamount = 0;
                                                fineamount = fineamtdet;
                                                //if (fineamtdet =0)
                                                //    {
                                                //        fineamount = 0;
                                                //    }
                                                //    else if (item.FYP_Tot_Amount > employeedetails[0].FSS_TobePaid)
                                                //    {
                                                //        fineamount = fineamtdet;
                                                //    }

                                                foreach (var feehead in Feepaymentpendingdet)
                                                {
                                                    var fmaid = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(t => t.FMG_Id == feehead.FMG_Id && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id && t.FMCC_Id == FMCC_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.FMA_Id).Single();

                                                    FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

                                                    //added on 11-07-2018
                                                    var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                                      from b in _FeeGroupHeadContext.FeeHeadDMO
                                                                      where (a.MI_Id == data.MI_Id && a.FMA_Id == fmaid && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == Amstid && a.FMH_Id == b.FMH_Id)
                                                                      select new FeeStudentTransactionDTO
                                                                      {
                                                                          FMH_Id = a.FMH_Id,
                                                                      }
                                                       ).Distinct().Take(1);

                                                    // if (feehead.FSS_TobePaid > 0)
                                                    //{
                                                    feetrapay.FTP_Id = 0;
                                                    feetrapay.FMA_Id = fmaid;

                                                    if (fineheadss.Count() > 0)
                                                    {
                                                        feetrapay.FTP_Paid_Amt = fineamount;
                                                    }
                                                    else
                                                    {
                                                        feetrapay.FTP_Paid_Amt = feehead.FSS_TobePaid;
                                                    }
                                                    feetrapay.FTP_Concession_Amt = 0;
                                                    feetrapay.FTP_Fine_Amt = 0;
                                                    feetrapay.FTP_Waived_Amt = 0;
                                                    feetrapay.ftp_remarks = "Fee Receipt Imported";

                                                    feetrapay.FYP_Id = pmm.FYP_Id;


                                                    _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(feetrapay);

                                                    var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                                    && t.AMST_Id == Amstid && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id
                                                    && t.FMG_Id == feehead.FMG_Id && t.FMA_Id == fmaid && t.FSS_ActiveFlag == true).FirstOrDefault();

                                                    obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + feehead.FSS_TobePaid;

                                                    if (fineheadss.Count() > 0)
                                                    {
                                                        obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + fineamount;
                                                        obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + fineamount;
                                                    }
                                                    //added on 11-07-2018


                                                    if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
                                                    {
                                                        obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - feehead.FSS_TobePaid;
                                                    }
                                                    else
                                                    {
                                                        obj_status_stf.FSS_ToBePaid = 0;
                                                    }

                                                    _FeeGroupHeadContext.Update(obj_status_stf);
                                                    //}


                                                }

                                                var contactexisttransaction = 0;
                                                using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                                                {
                                                    try
                                                    {
                                                        contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                                                        dbCtxTxn.Commit();

                                                        //pgmod.returnval = "true";
                                                        //pgmod.displaymessage = "Saved";

                                                        data.suscnt += 1;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                        dbCtxTxn.Rollback();
                                                        data.failcnt += 1;
                                                        data.FYP_Receipt_No = "O";
                                                        //pgmod.returnval = "false";
                                                        //pgmod.displaymessage = "Not Saved";
                                                    }
                                                }
                                            }
                                            //}
                                            //else
                                            //{
                                            //    var outputval = 0;
                                            //    data.FYP_Receipt_No = "R";
                                            //    outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Fine Amount Mismatsh", data.UserId, data.ASMAY_Id);

                                            //    data.dupcnt += 1;
                                            //    data.returnval = true;
                                            //}
                                        }
                                        else
                                        {
                                            data.failcnt += 1;
                                            var outputval = 0;
                                            data.FYP_Receipt_No = "S";
                                            outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Amount Mismatsh", data.UserId, data.ASMAY_Id, item.FYP_Bank_Or_Cash);

                                        }
                                    }
                                    else
                                    {
                                        data.failcnt += 1;
                                        var outputval = 0;
                                        data.FYP_Receipt_No = "T";
                                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Amount Mismatsh", data.UserId, data.ASMAY_Id, item.FYP_Bank_Or_Cash);

                                    }
                                }
                                else
                                {
                                    data.failcnt += 1;
                                    var outputval = 0;
                                    data.FYP_Receipt_No = "TT";
                                    outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Record Not Exists", data.UserId, data.ASMAY_Id, item.FYP_Bank_Or_Cash);

                                }
                            }
                            else
                            {

                                data.failcnt += 1;
                                var outputval = 0;
                                data.FYP_Receipt_No = "U";
                                outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8", data.MI_Id, 0, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Not Exists", data.UserId, data.ASMAY_Id, item.FYP_Bank_Or_Cash);

                            }
                        }
                        data.returnval = true;

                    }


                }
                catch (Exception ex)
                {
                    data.FYP_Receipt_No = "PPP";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.failcnt += 1;
                data.returnval = false;

                //data.FYP_Receipt_No = "Z";
            }
            return data;
        }

        //public FeeReceiptImportStthomasDTO Savedata(FeeReceiptImportStthomasDTO data)
        //{
        //    try
        //    {
        //        data.dupcnt = 0;
        //        data.failcnt = 0;
        //        data.suscnt = 0;
        //        var cccccc = 3;

        //        if (data.FeeReceiptimport != null)
        //        {
        //            if (data.FeeReceiptimport.Length > 0)
        //            {
        //                var parid = 0;
        //                foreach (var item in data.FeeReceiptimport)
        //                {

        //                    var Amstidnew = _FeeGroupHeadContext.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_AdmNo == item.AMST_AdmNo).ToList(); 

        //                    if (Amstidnew.Count > 0)

        //                    {
        //                        long Amstid = 0;
        //                        Amstid = Amstidnew.FirstOrDefault().AMST_Id;
        //                        if (Amstid > 0)
        //                        {
        //                            List<FeeReceiptImportStthomasDTO> employeedetails = new List<FeeReceiptImportStthomasDTO>();


        //                            using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                            {
        //                                cmd.CommandText = "FeeImportTermDetails";
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = data.ASMAY_Id
        //                                });

        //                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = data.MI_Id
        //                                });



        //                                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = Amstid
        //                                });

        //                                if (cmd.Connection.State != ConnectionState.Open)
        //                                    cmd.Connection.Open();
        //                                var retObject = new List<dynamic>();
        //                                try
        //                                {
        //                                    using (var dataReader = cmd.ExecuteReader())
        //                                    {
        //                                        while (dataReader.Read())
        //                                        {
        //                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                            {
        //                                                dataRow.Add(
        //                                                    dataReader.GetName(iFiled),
        //                                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                );
        //                                            }



        //                                            employeedetails.Add(new FeeReceiptImportStthomasDTO
        //                                            {
        //                                                FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
        //                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
        //                                                FMT_Id = Convert.ToInt64(dataReader["FMT_Id"].ToString())



        //                                            });

        //                                        }
        //                                    }

        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    Console.WriteLine(ex.Message);
        //                                }
        //                            }

        //                            long datewisefineamount = 0;

        //                            if (employeedetails.Count > 0)
        //                            {
        //                                if (item.FYP_Tot_Amount >= employeedetails[0].FSS_TobePaid)
        //                                {
        //                                    long fineamtdet = 0;

        //                                    fineamtdet = item.FYP_Tot_Amount - employeedetails[0].FSS_TobePaid;

        //                                    var fineslab = _FeeGroupHeadContext.feeTFineSlabDMO.Where(t => t.FTFS_FineType == "Day Slab").Select(t => t.FTFS_Amount).Distinct().ToList();

        //                                    List<FeeReceiptImportStthomasDTO> Feepaymentpendingdetfinecal = new List<FeeReceiptImportStthomasDTO>();


        //                                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                    {
        //                                        cmd.CommandText = "FeeImportInstallmentDetailsFineAmt";
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = data.ASMAY_Id
        //                                        });

        //                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = data.MI_Id
        //                                        });



        //                                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = Amstid
        //                                        });
        //                                        cmd.Parameters.Add(new SqlParameter("@FMT_Id",
        //                                              SqlDbType.VarChar)
        //                                        {
        //                                            Value = employeedetails[0].FMT_Id
        //                                        });

        //                                        if (cmd.Connection.State != ConnectionState.Open)
        //                                            cmd.Connection.Open();
        //                                        var retObject = new List<dynamic>();
        //                                        try
        //                                        {
        //                                            using (var dataReader = cmd.ExecuteReader())
        //                                            {
        //                                                while (dataReader.Read())
        //                                                {
        //                                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                                    {
        //                                                        dataRow.Add(
        //                                                            dataReader.GetName(iFiled),
        //                                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                        );
        //                                                    }



        //                                                    Feepaymentpendingdetfinecal.Add(new FeeReceiptImportStthomasDTO
        //                                                    {
        //                                                        FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),

        //                                                        FMG_Id = Convert.ToInt64(dataReader["FMG_Id"].ToString()),
        //                                                        FMH_Id = Convert.ToInt64(dataReader["FMH_Id"].ToString()),
        //                                                        FTI_Id = Convert.ToInt64(dataReader["FTI_Id"].ToString()),
        //                                                        FMA_Id = Convert.ToInt64(dataReader["FMA_Id"].ToString()),



        //                                                    });

        //                                                }
        //                                            }

        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            Console.WriteLine(ex.Message);
        //                                        }
        //                                    }


        //                                    List<FeeReceiptImportStthomasDTO> fines_fma_ids = new List<FeeReceiptImportStthomasDTO>();
        //                                    var fine_amount = 0;
        //                                    foreach (FeeReceiptImportStthomasDTO x in Feepaymentpendingdetfinecal)
        //                                    {
        //                                        FeeReceiptImportStthomasDTO sew = new FeeReceiptImportStthomasDTO();
        //                                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                        {

        //                                            cmd.CommandText = "Sp_Calculate_Fine";



        //                                            cmd.CommandType = CommandType.StoredProcedure;
        //                                            cmd.Parameters.Add(new SqlParameter("@On_Date",
        //                                                SqlDbType.DateTime)
        //                                            {
        //                                                Value = Convert.ToDateTime(item.FYP_Date)
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@fma_id",
        //                                               SqlDbType.BigInt)
        //                                            {
        //                                                Value = x.FMA_Id
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
        //                                           SqlDbType.BigInt)
        //                                            {
        //                                                Value = data.ASMAY_Id
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@amt",
        //                                SqlDbType.Float)
        //                                            {
        //                                                Direction = ParameterDirection.Output
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@flgArr",
        //                               SqlDbType.Int)
        //                                            {
        //                                                Direction = ParameterDirection.Output
        //                                            });
        //                                            if (cmd.Connection.State != ConnectionState.Open)
        //                                                cmd.Connection.Open();

        //                                            var data1 = cmd.ExecuteNonQuery();
        //                                            //  x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
        //                                            fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

        //                                            sew.FMA_Id = x.FMA_Id;
        //                                            datewisefineamount += Convert.ToInt32(cmd.Parameters["@amt"].Value);
        //                                            fines_fma_ids.Add(sew);
        //                                        }
        //                                    }




        //                                    if ((fineslab.Contains(fineamtdet) || fineamtdet == 0) && (datewisefineamount == 0 || datewisefineamount == fineamtdet))
        //                                    {


        //                                        List<FeeReceiptImportStthomasDTO> Feepaymentpendingdet = new List<FeeReceiptImportStthomasDTO>();


        //                                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                        {
        //                                            cmd.CommandText = "FeeImportInstallmentDetails";
        //                                            cmd.CommandType = CommandType.StoredProcedure;
        //                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = data.ASMAY_Id
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = data.MI_Id
        //                                            });



        //                                            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = Amstid
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@FMT_Id",
        //                                                  SqlDbType.VarChar)
        //                                            {
        //                                                Value = employeedetails[0].FMT_Id
        //                                            });

        //                                            if (cmd.Connection.State != ConnectionState.Open)
        //                                                cmd.Connection.Open();
        //                                            var retObject = new List<dynamic>();
        //                                            try
        //                                            {
        //                                                using (var dataReader = cmd.ExecuteReader())
        //                                                {
        //                                                    while (dataReader.Read())
        //                                                    {
        //                                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                                        {
        //                                                            dataRow.Add(
        //                                                                dataReader.GetName(iFiled),
        //                                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                            );
        //                                                        }



        //                                                        Feepaymentpendingdet.Add(new FeeReceiptImportStthomasDTO
        //                                                        {
        //                                                            FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),

        //                                                            FMG_Id = Convert.ToInt64(dataReader["FMG_Id"].ToString()),
        //                                                            FMH_Id = Convert.ToInt64(dataReader["FMH_Id"].ToString()),
        //                                                            FTI_Id = Convert.ToInt64(dataReader["FTI_Id"].ToString()),
        //                                                            FMA_Id = Convert.ToInt64(dataReader["FMA_Id"].ToString()),



        //                                                        });

        //                                                    }
        //                                                }

        //                                            }
        //                                            catch (Exception ex)
        //                                            {
        //                                                Console.WriteLine(ex.Message);
        //                                            }
        //                                        }

        //                                        data.FMG_Id = Feepaymentpendingdet[0].FMG_Id;


        //                                        var ASMCLid = _FeeGroupHeadContext.SchoolYearWiseStudent.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == Amstid).Select(t => t.ASMCL_Id).Single();
        //                                        var FMCC_Id = (from a in _FeeGroupHeadContext.feeCC
        //                                                       from b in _FeeGroupHeadContext.feeYCC
        //                                                       from c in _FeeGroupHeadContext.feeYCCC
        //                                                       where (a.FMCC_Id == b.FMCC_Id && c.FYCC_Id == b.FYCC_Id
        //                                                       && c.ASMCL_Id == ASMCLid && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
        //                                                       select b.FMCC_Id).Distinct().Single();


        //                                        int j = 0;
        //                                        string list_values = "";
        //                                        FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {
        //                                            pmm.MI_Id = data.MI_Id;
        //                                            pmm.ASMAY_ID = data.ASMAY_Id;
        //                                            pmm.user_id = data.UserId;
        //                                            //pgmod.temp_head_list = pgmod.savetmpdata;

        //                                            pmm.FYP_Date = Convert.ToDateTime(item.FYP_Date);

        //                                            //multimode


        //                                            if (item.FYP_Tot_Amount > 0)
        //                                            {

        //                                                pmm.FYP_Bank_Name = "";
        //                                                pmm.FYP_DD_Cheque_Date = Convert.ToDateTime(item.FYP_Date);
        //                                                pmm.FYP_DD_Cheque_No = "";
        //                                                pmm.FYP_Bank_Or_Cash = item.FYP_Bank_Or_Cash;

        //                                            }


        //                                            pmm.FYP_Chq_Bounce = "CL";
        //                                            pmm.FYP_Remarks = "Fee Receipt Imported";
        //                                            pmm.FTCU_Id = 1;
        //                                            pmm.FYP_Tot_Amount = item.FYP_Tot_Amount;
        //                                            pmm.FYP_Tot_Concession_Amt = 0;
        //                                            pmm.FYP_Tot_Fine_Amt = 0;
        //                                            pmm.FYP_Tot_Waived_Amt = 0;
        //                                            pmm.DOE = DateTime.Now;
        //                                            pmm.CreatedDate = DateTime.Now;
        //                                            pmm.UpdatedDate = DateTime.Now;
        //                                            pmm.FYP_OnlineChallanStatusFlag = "Sucessfull";
        //                                            pmm.FYP_PayModeType = "APP";
        //                                            pmm.FYP_PayGatewayType = "";

        //                                            get_grp_reptno(data);

        //                                            pmm.FYP_Receipt_No = data.FYP_Receipt_No;
        //                                            //  pmm.FYP_Receipt_No = item.FYP_Receipt_No;
        //                                        }

        //                                        _FeeGroupHeadContext.FeePaymentDetailsDMO.Add(pmm);

        //                                        //Multimode of Payment

        //                                        pmm.FYP_DeviseFlg = "Single";


        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {
        //                                            Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
        //                                            obj2.FYP_Id = pmm.FYP_Id;

        //                                            obj2.FYP_TransactionTypeFlag = item.FYP_Bank_Or_Cash;

        //                                            obj2.FYPPM_TotalPaidAmount = item.FYP_Tot_Amount;
        //                                            obj2.FYPPM_LedgerId = 0;
        //                                            obj2.FYPPM_BankName = "";
        //                                            obj2.FYPPM_DDChequeNo = "";
        //                                            obj2.FYPPM_DDChequeDate = Convert.ToDateTime(item.FYP_Date);
        //                                            obj2.FYPPM_TransactionId = "";
        //                                            obj2.FYPPM_PaymentReferenceId = "";
        //                                            obj2.FYPPM_ClearanceStatusFlag = "0";
        //                                            obj2.FYPPM_ClearanceDate = Convert.ToDateTime(item.FYP_Date);
        //                                            _FeeGroupHeadContext.Add(obj2);
        //                                        }



        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {

        //                                            Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

        //                                            temppayment.AMST_Id = Amstid;
        //                                            temppayment.ASMAY_Id = pmm.ASMAY_ID;
        //                                            temppayment.FTP_TotalPaidAmount = item.FYP_Tot_Amount;
        //                                            temppayment.FTP_TotalWaivedAmount = 0;
        //                                            temppayment.FTP_TotalConcessionAmount = 0;
        //                                            temppayment.FTP_TotalFineAmount = 0;
        //                                            temppayment.FYP_Id = pmm.FYP_Id;
        //                                            _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);








        //                                            long fineamount = 0;
        //                                            if (item.FYP_Tot_Amount == employeedetails[0].FSS_TobePaid)
        //                                            {
        //                                                fineamount = 0;
        //                                            }
        //                                            else if (item.FYP_Tot_Amount > employeedetails[0].FSS_TobePaid)
        //                                            {
        //                                                fineamount = Convert.ToInt64(item.FYP_Tot_Amount) - Convert.ToInt64(employeedetails[0].FSS_TobePaid);
        //                                            }

        //                                            foreach (var feehead in Feepaymentpendingdet)
        //                                            {
        //                                                var fmaid = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(t => t.FMG_Id == feehead.FMG_Id && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id && t.FMCC_Id == FMCC_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.FMA_Id).Single();

        //                                                FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

        //                                                //added on 11-07-2018
        //                                                var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
        //                                                                  from b in _FeeGroupHeadContext.FeeHeadDMO
        //                                                                  where (a.MI_Id == data.MI_Id && a.FMA_Id == fmaid && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == Amstid && a.FMH_Id == b.FMH_Id)
        //                                                                  select new FeeStudentTransactionDTO
        //                                                                  {
        //                                                                      FMH_Id = a.FMH_Id,
        //                                                                  }
        //                                                   ).Distinct().Take(1);

        //                                                // if (feehead.FSS_TobePaid > 0)
        //                                                //{
        //                                                feetrapay.FTP_Id = 0;
        //                                                feetrapay.FMA_Id = fmaid;

        //                                                if (fineheadss.Count() > 0)
        //                                                {
        //                                                    feetrapay.FTP_Paid_Amt = fineamount;
        //                                                }
        //                                                else
        //                                                {
        //                                                    feetrapay.FTP_Paid_Amt = feehead.FSS_TobePaid;
        //                                                }
        //                                                feetrapay.FTP_Concession_Amt = 0;
        //                                                feetrapay.FTP_Fine_Amt = 0;
        //                                                feetrapay.FTP_Waived_Amt = 0;
        //                                                feetrapay.ftp_remarks = "Fee Receipt Imported";

        //                                                feetrapay.FYP_Id = pmm.FYP_Id;


        //                                                _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(feetrapay);

        //                                                var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
        //                                                && t.AMST_Id == Amstid && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id
        //                                                && t.FMG_Id == feehead.FMG_Id && t.FMA_Id == fmaid && t.FSS_ActiveFlag == true).FirstOrDefault();

        //                                                obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + feehead.FSS_TobePaid;



        //                                                if (fineheadss.Count() > 0)
        //                                                {
        //                                                    obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + fineamount;
        //                                                }
        //                                                //added on 11-07-2018


        //                                                if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
        //                                                {
        //                                                    obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - feehead.FSS_TobePaid;
        //                                                }
        //                                                else
        //                                                {
        //                                                    obj_status_stf.FSS_ToBePaid = 0;
        //                                                }

        //                                                _FeeGroupHeadContext.Update(obj_status_stf);
        //                                                //}


        //                                            }

        //                                            var contactexisttransaction = 0;
        //                                            using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
        //                                            {
        //                                                try
        //                                                {
        //                                                    contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
        //                                                    dbCtxTxn.Commit();

        //                                                    //pgmod.returnval = "true";
        //                                                    //pgmod.displaymessage = "Saved";

        //                                                    data.suscnt += 1;
        //                                                }
        //                                                catch (Exception ex)
        //                                                {
        //                                                    Console.WriteLine(ex.Message);
        //                                                    dbCtxTxn.Rollback();
        //                                                    data.failcnt += 1;
        //                                                    //pgmod.returnval = "false";
        //                                                    //pgmod.displaymessage = "Not Saved";
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        var outputval = 0;

        //                                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Fine Amount Mismatsh", data.UserId, data.ASMAY_Id);

        //                                        data.dupcnt += 1;
        //                                        data.returnval = true;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    data.failcnt += 1;
        //                                    var outputval = 0;

        //                                    outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Amount Mismatsh", data.UserId, data.ASMAY_Id);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                data.failcnt += 1;
        //                                var outputval = 0;

        //                                outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Amount Mismatsh", data.UserId, data.ASMAY_Id);

        //                            }
        //                        }
        //                        else
        //                        {
        //                            data.failcnt += 1;
        //                            var outputval = 0;

        //                            outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Record Not Exists", data.UserId, data.ASMAY_Id);

        //                        }
        //                    }
        //                    else
        //                    {

        //                        data.failcnt += 1;
        //                        var outputval = 0;

        //                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, 0, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Not Exists", data.UserId, data.ASMAY_Id);

        //                    }
        //                }
        //                data.returnval = true;
        //            }
        //        }





        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        data.failcnt += 1;
        //        data.returnval = false;
        //    }
        //    return data;
        //}



        //public FeeReceiptImportStthomasDTO Savedata(FeeReceiptImportStthomasDTO data)
        //{
        //    try
        //    {
        //        data.dupcnt = 0;
        //        data.failcnt = 0;
        //        data.suscnt = 0;
        //        var cccccc = 3;

        //        if (data.FeeReceiptimport != null)
        //        {
        //            if (data.FeeReceiptimport.Length > 0)
        //            {
        //                var parid = 0;
        //                foreach (var item in data.FeeReceiptimport)
        //                {

        //                    var Amstidnew = _FeeGroupHeadContext.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_AdmNo == item.AMST_AdmNo).ToList(); ;

        //                    if (Amstidnew.Count > 0)

        //                    {
        //                        long Amstid = 0;
        //                        Amstid = Amstidnew.FirstOrDefault().AMST_Id;
        //                        if (Amstid > 0)
        //                        {
        //                            List<FeeReceiptImportStthomasDTO> employeedetails = new List<FeeReceiptImportStthomasDTO>();


        //                            using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                            {
        //                                cmd.CommandText = "FeeImportTermDetails";
        //                                cmd.CommandType = CommandType.StoredProcedure;
        //                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = data.ASMAY_Id
        //                                });

        //                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = data.MI_Id
        //                                });



        //                                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                        SqlDbType.VarChar)
        //                                {
        //                                    Value = Amstid
        //                                });

        //                                if (cmd.Connection.State != ConnectionState.Open)
        //                                    cmd.Connection.Open();
        //                                var retObject = new List<dynamic>();
        //                                try
        //                                {
        //                                    using (var dataReader = cmd.ExecuteReader())
        //                                    {
        //                                        while (dataReader.Read())
        //                                        {
        //                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                            {
        //                                                dataRow.Add(
        //                                                    dataReader.GetName(iFiled),
        //                                                    dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                );
        //                                            }



        //                                            employeedetails.Add(new FeeReceiptImportStthomasDTO
        //                                            {
        //                                                FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
        //                                                AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
        //                                                FMT_Id = Convert.ToInt64(dataReader["FMT_Id"].ToString())



        //                                            });

        //                                        }
        //                                    }

        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    Console.WriteLine(ex.Message);
        //                                }
        //                            }

        //                            long datewisefineamount = 0;

        //                            if (employeedetails.Count > 0)
        //                            {
        //                                if (item.FYP_Tot_Amount >= employeedetails[0].FSS_TobePaid)
        //                                {
        //                                    long fineamtdet = 0;

        //                                    fineamtdet = item.FYP_Tot_Amount - employeedetails[0].FSS_TobePaid;

        //                                    var fineslab = _FeeGroupHeadContext.feeTFineSlabDMO.Where(t => t.FTFS_FineType == "Day Slab").Select(t => t.FTFS_Amount).Distinct().ToList();

        //                                    List<FeeReceiptImportStthomasDTO> Feepaymentpendingdetfinecal = new List<FeeReceiptImportStthomasDTO>();


        //                                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                    {
        //                                        cmd.CommandText = "FeeImportInstallmentDetailsFineAmt";
        //                                        cmd.CommandType = CommandType.StoredProcedure;
        //                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = data.ASMAY_Id
        //                                        });

        //                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = data.MI_Id
        //                                        });



        //                                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                                SqlDbType.VarChar)
        //                                        {
        //                                            Value = Amstid
        //                                        });
        //                                        cmd.Parameters.Add(new SqlParameter("@FMT_Id",
        //                                              SqlDbType.VarChar)
        //                                        {
        //                                            Value = employeedetails[0].FMT_Id
        //                                        });

        //                                        if (cmd.Connection.State != ConnectionState.Open)
        //                                            cmd.Connection.Open();
        //                                        var retObject = new List<dynamic>();
        //                                        try
        //                                        {
        //                                            using (var dataReader = cmd.ExecuteReader())
        //                                            {
        //                                                while (dataReader.Read())
        //                                                {
        //                                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                                    {
        //                                                        dataRow.Add(
        //                                                            dataReader.GetName(iFiled),
        //                                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                        );
        //                                                    }



        //                                                    Feepaymentpendingdetfinecal.Add(new FeeReceiptImportStthomasDTO
        //                                                    {
        //                                                        FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),

        //                                                        FMG_Id = Convert.ToInt64(dataReader["FMG_Id"].ToString()),
        //                                                        FMH_Id = Convert.ToInt64(dataReader["FMH_Id"].ToString()),
        //                                                        FTI_Id = Convert.ToInt64(dataReader["FTI_Id"].ToString()),
        //                                                        FMA_Id = Convert.ToInt64(dataReader["FMA_Id"].ToString()),



        //                                                    });

        //                                                }
        //                                            }

        //                                        }
        //                                        catch (Exception ex)
        //                                        {
        //                                            Console.WriteLine(ex.Message);
        //                                        }
        //                                    }


        //                                    List<FeeReceiptImportStthomasDTO> fines_fma_ids = new List<FeeReceiptImportStthomasDTO>();
        //                                    var fine_amount = 0;
        //                                    foreach (FeeReceiptImportStthomasDTO x in Feepaymentpendingdetfinecal)
        //                                    {
        //                                        FeeReceiptImportStthomasDTO sew = new FeeReceiptImportStthomasDTO();
        //                                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                        {

        //                                            cmd.CommandText = "Sp_Calculate_Fine";



        //                                            cmd.CommandType = CommandType.StoredProcedure;
        //                                            cmd.Parameters.Add(new SqlParameter("@On_Date",
        //                                                SqlDbType.DateTime)
        //                                            {
        //                                                Value = Convert.ToDateTime(item.FYP_Date)
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@fma_id",
        //                                               SqlDbType.BigInt)
        //                                            {
        //                                                Value = x.FMA_Id
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
        //                                           SqlDbType.BigInt)
        //                                            {
        //                                                Value = data.ASMAY_Id
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@amt",
        //                                SqlDbType.Float)
        //                                            {
        //                                                Direction = ParameterDirection.Output
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@flgArr",
        //                               SqlDbType.Int)
        //                                            {
        //                                                Direction = ParameterDirection.Output
        //                                            });
        //                                            if (cmd.Connection.State != ConnectionState.Open)
        //                                                cmd.Connection.Open();

        //                                            var data1 = cmd.ExecuteNonQuery();
        //                                            //  x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
        //                                            fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

        //                                            sew.FMA_Id = x.FMA_Id;
        //                                            datewisefineamount += Convert.ToInt32(cmd.Parameters["@amt"].Value);
        //                                            fines_fma_ids.Add(sew);
        //                                        }
        //                                    }




        //                                    if ((fineslab.Contains(fineamtdet) || fineamtdet == 0) && (datewisefineamount == 0 || datewisefineamount == fineamtdet))
        //                                    {


        //                                        List<FeeReceiptImportStthomasDTO> Feepaymentpendingdet = new List<FeeReceiptImportStthomasDTO>();


        //                                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                                        {
        //                                            cmd.CommandText = "FeeImportInstallmentDetails";
        //                                            cmd.CommandType = CommandType.StoredProcedure;
        //                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = data.ASMAY_Id
        //                                            });

        //                                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = data.MI_Id
        //                                            });



        //                                            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
        //                                                    SqlDbType.VarChar)
        //                                            {
        //                                                Value = Amstid
        //                                            });
        //                                            cmd.Parameters.Add(new SqlParameter("@FMT_Id",
        //                                                  SqlDbType.VarChar)
        //                                            {
        //                                                Value = employeedetails[0].FMT_Id
        //                                            });

        //                                            if (cmd.Connection.State != ConnectionState.Open)
        //                                                cmd.Connection.Open();
        //                                            var retObject = new List<dynamic>();
        //                                            try
        //                                            {
        //                                                using (var dataReader = cmd.ExecuteReader())
        //                                                {
        //                                                    while (dataReader.Read())
        //                                                    {
        //                                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                                                        {
        //                                                            dataRow.Add(
        //                                                                dataReader.GetName(iFiled),
        //                                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
        //                                                            );
        //                                                        }



        //                                                        Feepaymentpendingdet.Add(new FeeReceiptImportStthomasDTO
        //                                                        {
        //                                                            FSS_TobePaid = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),

        //                                                            FMG_Id = Convert.ToInt64(dataReader["FMG_Id"].ToString()),
        //                                                            FMH_Id = Convert.ToInt64(dataReader["FMH_Id"].ToString()),
        //                                                            FTI_Id = Convert.ToInt64(dataReader["FTI_Id"].ToString()),
        //                                                            FMA_Id = Convert.ToInt64(dataReader["FMA_Id"].ToString()),



        //                                                        });

        //                                                    }
        //                                                }

        //                                            }
        //                                            catch (Exception ex)
        //                                            {
        //                                                Console.WriteLine(ex.Message);
        //                                            }
        //                                        }

        //                                        data.FMG_Id = Feepaymentpendingdet[0].FMG_Id;


        //                                        var ASMCLid = _FeeGroupHeadContext.SchoolYearWiseStudent.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == Amstid).Select(t => t.ASMCL_Id).Single();
        //                                        var FMCC_Id = (from a in _FeeGroupHeadContext.feeCC
        //                                                       from b in _FeeGroupHeadContext.feeYCC
        //                                                       from c in _FeeGroupHeadContext.feeYCCC
        //                                                       where (a.FMCC_Id == b.FMCC_Id && c.FYCC_Id == b.FYCC_Id
        //                                                       && c.ASMCL_Id == ASMCLid && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
        //                                                       select b.FMCC_Id).Distinct().Single();


        //                                        int j = 0;
        //                                        string list_values = "";
        //                                        FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {
        //                                            pmm.MI_Id = data.MI_Id;
        //                                            pmm.ASMAY_ID = data.ASMAY_Id;
        //                                            pmm.user_id = data.UserId;
        //                                            //pgmod.temp_head_list = pgmod.savetmpdata;

        //                                            pmm.FYP_Date = Convert.ToDateTime(item.FYP_Date);

        //                                            //multimode


        //                                            if (item.FYP_Tot_Amount > 0)
        //                                            {

        //                                                pmm.FYP_Bank_Name = "";
        //                                                pmm.FYP_DD_Cheque_Date = Convert.ToDateTime(item.FYP_Date);
        //                                                pmm.FYP_DD_Cheque_No = "";
        //                                                pmm.FYP_Bank_Or_Cash = item.FYP_Bank_Or_Cash;

        //                                            }


        //                                            pmm.FYP_Chq_Bounce = "CL";
        //                                            pmm.FYP_Remarks = "Fee Receipt Imported";
        //                                            pmm.FTCU_Id = 1;
        //                                            pmm.FYP_Tot_Amount = item.FYP_Tot_Amount;
        //                                            pmm.FYP_Tot_Concession_Amt = 0;
        //                                            pmm.FYP_Tot_Fine_Amt = 0;
        //                                            pmm.FYP_Tot_Waived_Amt = 0;
        //                                            pmm.DOE = DateTime.Now;
        //                                            pmm.CreatedDate = DateTime.Now;
        //                                            pmm.UpdatedDate = DateTime.Now;
        //                                            pmm.FYP_OnlineChallanStatusFlag = "Sucessfull";
        //                                            pmm.FYP_PayModeType = "APP";
        //                                            pmm.FYP_PayGatewayType = "";

        //                                            get_grp_reptno(data);

        //                                            pmm.FYP_Receipt_No = data.FYP_Receipt_No;
        //                                            //  pmm.FYP_Receipt_No = item.FYP_Receipt_No;
        //                                        }

        //                                        _FeeGroupHeadContext.FeePaymentDetailsDMO.Add(pmm);

        //                                        //Multimode of Payment

        //                                        pmm.FYP_DeviseFlg = "Single";


        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {
        //                                            Fee_Y_Payment_PaymentModeSchool obj2 = new Fee_Y_Payment_PaymentModeSchool();
        //                                            obj2.FYP_Id = pmm.FYP_Id;

        //                                            obj2.FYP_TransactionTypeFlag = item.FYP_Bank_Or_Cash;

        //                                            obj2.FYPPM_TotalPaidAmount = item.FYP_Tot_Amount;
        //                                            obj2.FYPPM_LedgerId = 0;
        //                                            obj2.FYPPM_BankName = "";
        //                                            obj2.FYPPM_DDChequeNo = "";
        //                                            obj2.FYPPM_DDChequeDate = Convert.ToDateTime(item.FYP_Date);
        //                                            obj2.FYPPM_TransactionId = "";
        //                                            obj2.FYPPM_PaymentReferenceId = "";
        //                                            obj2.FYPPM_ClearanceStatusFlag = "0";
        //                                            obj2.FYPPM_ClearanceDate = Convert.ToDateTime(item.FYP_Date);
        //                                            _FeeGroupHeadContext.Add(obj2);
        //                                        }



        //                                        if (item.FYP_Tot_Amount > 0)
        //                                        {

        //                                            Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

        //                                            temppayment.AMST_Id = Amstid;
        //                                            temppayment.ASMAY_Id = pmm.ASMAY_ID;
        //                                            temppayment.FTP_TotalPaidAmount = item.FYP_Tot_Amount;
        //                                            temppayment.FTP_TotalWaivedAmount = 0;
        //                                            temppayment.FTP_TotalConcessionAmount = 0;
        //                                            temppayment.FTP_TotalFineAmount = 0;
        //                                            temppayment.FYP_Id = pmm.FYP_Id;
        //                                            _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);








        //                                            long fineamount = 0;
        //                                            if (item.FYP_Tot_Amount == employeedetails[0].FSS_TobePaid)
        //                                            {
        //                                                fineamount = 0;
        //                                            }
        //                                            else if (item.FYP_Tot_Amount > employeedetails[0].FSS_TobePaid)
        //                                            {
        //                                                fineamount = Convert.ToInt64(item.FYP_Tot_Amount) - Convert.ToInt64(employeedetails[0].FSS_TobePaid);
        //                                            }

        //                                            foreach (var feehead in Feepaymentpendingdet)
        //                                            {
        //                                                var fmaid = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(t => t.FMG_Id == feehead.FMG_Id && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id && t.FMCC_Id == FMCC_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.FMA_Id).Single();

        //                                                FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

        //                                                //added on 11-07-2018
        //                                                var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
        //                                                                  from b in _FeeGroupHeadContext.FeeHeadDMO
        //                                                                  where (a.MI_Id == data.MI_Id && a.FMA_Id == fmaid && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == Amstid && a.FMH_Id == b.FMH_Id)
        //                                                                  select new FeeStudentTransactionDTO
        //                                                                  {
        //                                                                      FMH_Id = a.FMH_Id,
        //                                                                  }
        //                                                   ).Distinct().Take(1);

        //                                                // if (feehead.FSS_TobePaid > 0)
        //                                                //{
        //                                                feetrapay.FTP_Id = 0;
        //                                                feetrapay.FMA_Id = fmaid;

        //                                                if (fineheadss.Count() > 0)
        //                                                {
        //                                                    feetrapay.FTP_Paid_Amt = fineamount;
        //                                                }
        //                                                else
        //                                                {
        //                                                    feetrapay.FTP_Paid_Amt = feehead.FSS_TobePaid;
        //                                                }
        //                                                feetrapay.FTP_Concession_Amt = 0;
        //                                                feetrapay.FTP_Fine_Amt = 0;
        //                                                feetrapay.FTP_Waived_Amt = 0;
        //                                                feetrapay.ftp_remarks = "Fee Receipt Imported";

        //                                                feetrapay.FYP_Id = pmm.FYP_Id;


        //                                                _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(feetrapay);

        //                                                var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
        //                                                && t.AMST_Id == Amstid && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == feehead.FTI_Id
        //                                                && t.FMG_Id == feehead.FMG_Id && t.FMA_Id == fmaid && t.FSS_ActiveFlag == true).FirstOrDefault();

        //                                                obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + feehead.FSS_TobePaid;



        //                                                if (fineheadss.Count() > 0)
        //                                                {
        //                                                    obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + fineamount;
        //                                                }
        //                                                //added on 11-07-2018


        //                                                if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
        //                                                {
        //                                                    obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - feehead.FSS_TobePaid;
        //                                                }
        //                                                else
        //                                                {
        //                                                    obj_status_stf.FSS_ToBePaid = 0;
        //                                                }

        //                                                _FeeGroupHeadContext.Update(obj_status_stf);
        //                                                //}


        //                                            }

        //                                            var contactexisttransaction = 0;
        //                                            using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
        //                                            {
        //                                                try
        //                                                {
        //                                                    contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
        //                                                    dbCtxTxn.Commit();

        //                                                    //pgmod.returnval = "true";
        //                                                    //pgmod.displaymessage = "Saved";

        //                                                    data.suscnt += 1;
        //                                                }
        //                                                catch (Exception ex)
        //                                                {
        //                                                    Console.WriteLine(ex.Message);
        //                                                    dbCtxTxn.Rollback();
        //                                                    data.failcnt += 1;
        //                                                    //pgmod.returnval = "false";
        //                                                    //pgmod.displaymessage = "Not Saved";
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        var outputval = 0;

        //                                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Fine Amount Mismatsh", data.UserId, data.ASMAY_Id);

        //                                        data.dupcnt += 1;
        //                                        data.returnval = true;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    data.failcnt += 1;
        //                                    var outputval = 0;

        //                                    outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Amount Mismatsh", data.UserId, data.ASMAY_Id);

        //                                }
        //                            }
        //                            else
        //                            {
        //                                data.failcnt += 1;
        //                                var outputval = 0;

        //                                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo,item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date),"Amount Mismatsh", data.UserId,data.ASMAY_Id);

        //                            }
        //                        }
        //                        else
        //                        {
        //                            data.failcnt += 1;
        //                            var outputval = 0;

        //                            outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, Amstid, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Record Not Exists", data.UserId, data.ASMAY_Id);

        //                        }
        //                    }
        //                    else
        //                    {

        //                        data.failcnt += 1;
        //                        var outputval = 0;

        //                        outputval = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Fee_Excel_Imports @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, 0, item.AMST_AdmNo, item.FYP_Tot_Amount, Convert.ToDateTime(item.FYP_Date), "Student Not Exists", data.UserId, data.ASMAY_Id);

        //                    }
        //                }
        //                data.returnval = true;
        //            }
        //        }





        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        data.failcnt += 1;
        //        data.returnval = false;
        //    }
        //    return data;
        //}

        public FeeReceiptImportStthomasDTO getdetails(int id)
        {
            FeeReceiptImportStthomasDTO data = new FeeReceiptImportStthomasDTO();
            try
            {
                data.FeeGroup = _FeeGroupHeadContext.feeGroup.Where(t => t.MI_Id == id && t.FMG_ActiceFlag == true).Distinct().ToArray();
                data.FeeHead = _FeeGroupHeadContext.feehead.Where(t => t.MI_Id == id && t.FMH_ActiveFlag == true).Distinct().ToArray();
                //data.Academicyearlist = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == id && t.ASMAY_ActiveFlag == 1).Distinct().ToArray();

                var year = _FeeGroupHeadContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).ToList();
                data.Academicyearlist = year.Distinct().OrderByDescending(t => t.ASMAY_Order).GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeReceiptImportStthomasDTO deactiveY(FeeReceiptImportStthomasDTO data)
        {
            try
            {
                //var result = _LibraryContext.MasterAuthorDMO.Single(t => t.LMBA_Id == data.LMBA_Id);

                //if (result.LMBA_ActiveFlg == true)
                //{
                //    result.LMBA_ActiveFlg = false;
                //}
                //else if (result.LMBA_ActiveFlg == false)
                //{
                //    result.LMBA_ActiveFlg = true;
                //}
                //result.UpdatedDate = DateTime.Now;
                //_LibraryContext.Update(result);
                //int rowAffected = _LibraryContext.SaveChanges();
                //if (rowAffected > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeReceiptImportStthomasDTO get_grp_reptno(FeeReceiptImportStthomasDTO data)
        {
            try
            {

                var final_rept_no = "";
                List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && b.FGAR_Id == c.FGAR_Id)

                            select new FeeStudentTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                            }
                     ).Distinct().ToList();


                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.VarChar, 100)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmayid",
                       SqlDbType.NVarChar, 100)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fmgid",
                   SqlDbType.NVarChar, 100)
                    {
                        Value = data.FMG_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@receiptno",
        SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        //Added By PraveenGouda 05/01/2024
        public FeeReceiptImportStthomasDTO getreportdetails(FeeReceiptImportStthomasDTO data)
        {
            try
            {
                string startdate = "";
                startdate = data.fypdate.ToString("yyyy-MM-dd");


                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeReceipt_ImportDelete";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FYP_Date",
                  SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    //
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
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

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.receiptdelete = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public FeeReceiptImportStthomasDTO deletereceipt(FeeReceiptImportStthomasDTO data)
        {
            try
            {
                foreach (var i in data.receipt)
                {

                    try
                    {
                        var confirmstatus = _FeeGroupHeadContext.Database.ExecuteSqlCommand("ImportedReciept_UpdateDelete @p0,@p1,@p2,@p3",
                        data.MI_Id, i.ASMAY_Id, i.AMST_Id, i.FYP_Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
