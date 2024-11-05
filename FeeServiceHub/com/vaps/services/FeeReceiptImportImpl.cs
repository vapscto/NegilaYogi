using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeReceiptImportImpl : interfaces.FeeReceiptImportInterface
    {
        public FeeGroupContext _FeeGroupHeadContext;
        public DomainModelMsSqlServerContext _context;
        public FeeReceiptImportImpl(FeeGroupContext context, DomainModelMsSqlServerContext contextnew)
        {
            _FeeGroupHeadContext = context;
            _context = contextnew;
        }

        public FeeReceiptImportDTO Savedata(FeeReceiptImportDTO data)
        {
            try
            {
                data.dupcnt = 0;
                data.failcnt = 0;
                data.suscnt = 0;
                var cccccc = 3;

                if (data.FeeReceiptimport != null)
                {
                    if (data.FeeReceiptimport.Length > 0)
                    {
                        var parid = 0;
                        foreach (var item in data.FeeReceiptimport)
                        {


                            var fmgid = _FeeGroupHeadContext.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_GroupName == item.FMG_GroupName).Select(t => t.FMG_Id).Single();
                            data.FMG_Id = fmgid;
                           var ftiid = _FeeGroupHeadContext.FeeInstallmentsyearlyDMO.Where(t => t.FTI_Name == item.FTI_Name && t.MI_ID==data.MI_Id).Select(t => t.FTI_Id).Single();
                            var Amstid = _FeeGroupHeadContext.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_AdmNo == item.AMST_AdmNo).Select(t => t.AMST_Id).Single();
                            var ASMCLid = _FeeGroupHeadContext.SchoolYearWiseStudent.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == Amstid).Select(t => t.ASMCL_Id).Single();
                            var FMCC_Id = (from a in _FeeGroupHeadContext.feeCC
                                           from b in _FeeGroupHeadContext.feeYCC
                                           from c in _FeeGroupHeadContext.feeYCCC
                                           where (a.FMCC_Id == b.FMCC_Id && c.FYCC_Id == b.FYCC_Id
                                           && c.ASMCL_Id == ASMCLid && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                           select b.FMCC_Id).Distinct().Single();


                            int j = 0;
                            string list_values = "";
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
                                    if (item.FYP_Bank_Or_Cash == "C")
                                    {
                                        pmm.FYP_Bank_Name = "";
                                        pmm.FYP_DD_Cheque_Date = Convert.ToDateTime(item.FYP_DD_Cheque_Date);
                                        pmm.FYP_DD_Cheque_No = "";
                                        pmm.FYP_Bank_Or_Cash = item.FYP_Bank_Or_Cash;
                                    }
                                    else if (item.FYP_Bank_Or_Cash == "B" || item.FYP_Bank_Or_Cash == "E" || item.FYP_Bank_Or_Cash == "S" || item.FYP_Bank_Or_Cash == "R")
                                    {
                                        pmm.FYP_Bank_Name = item.FYP_Bank_Name;
                                        pmm.FYP_DD_Cheque_Date = Convert.ToDateTime(item.FYP_DD_Cheque_Date);
                                        pmm.FYP_DD_Cheque_No = item.FYP_DD_Cheque_No;
                                        pmm.FYP_Bank_Or_Cash = item.FYP_Bank_Or_Cash;
                                    }
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
                                obj2.FYPPM_BankName = item.FYP_Bank_Or_Cash == "C" ? "" : item.FYP_Bank_Name;
                                obj2.FYPPM_DDChequeNo = item.FYP_Bank_Or_Cash == "C" ? "" : item.FYP_DD_Cheque_No;
                                obj2.FYPPM_DDChequeDate = item.FYP_Bank_Or_Cash == "C" ? Convert.ToDateTime(item.FYP_Date) : Convert.ToDateTime(item.FYP_DD_Cheque_Date);
                                obj2.FYPPM_TransactionId = "";
                                obj2.FYPPM_PaymentReferenceId = "";
                                obj2.FYPPM_ClearanceStatusFlag = "0";
                                obj2.FYPPM_ClearanceDate = Convert.ToDateTime(item.FYP_DD_Cheque_Date);
                                _FeeGroupHeadContext.Add(obj2);
                            }



                            if (item.FYP_Tot_Amount > 0)
                            {

                                Fee_Y_Payment_School_StudentDMO temppayment = new Fee_Y_Payment_School_StudentDMO();

                                temppayment.AMST_Id = Amstid;
                                temppayment.ASMAY_Id = pmm.ASMAY_ID;
                                temppayment.FTP_TotalPaidAmount = item.FYP_Tot_Amount;
                                temppayment.FTP_TotalWaivedAmount = 0;
                                temppayment.FTP_TotalConcessionAmount =0;
                                temppayment.FTP_TotalFineAmount = 0;
                                temppayment.FYP_Id = pmm.FYP_Id;
                                _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO.Add(temppayment);

                                foreach (var feehead in item.Feeheadimport)
                                {
                                    var fmaid = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(t => t.FMG_Id == fmgid && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == ftiid && t.FMCC_Id == FMCC_Id && t.MI_Id==data.MI_Id && t.ASMAY_Id==data.ASMAY_Id).Select(t => t.FMA_Id).Single();

                                    FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();

                                    if (feehead.Amount > 0)
                                    {
                                        feetrapay.FTP_Id = 0;
                                        feetrapay.FMA_Id = fmaid;
                                        feetrapay.FTP_Paid_Amt = feehead.Amount;
                                        feetrapay.FTP_Concession_Amt =0;
                                        feetrapay.FTP_Fine_Amt =0;
                                        feetrapay.FTP_Waived_Amt = 0;
                                        feetrapay.ftp_remarks ="Fee Receipt Imported";

                                        feetrapay.FYP_Id = pmm.FYP_Id;

                                       
                                        _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(feetrapay);

                                        var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                                        && t.AMST_Id == Amstid && t.FMH_Id == feehead.FMH_Id && t.FTI_Id == ftiid
                                        && t.FMG_Id == fmgid && t.FMA_Id == fmaid && t.FSS_ActiveFlag == true).FirstOrDefault();

                                        obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + feehead.Amount;

                                        //added on 11-07-2018
                                        var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                          from b in _FeeGroupHeadContext.FeeHeadDMO
                                                          where (a.MI_Id == data.MI_Id && a.FMA_Id == fmaid && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMST_Id == Amstid && a.FMH_Id == b.FMH_Id)
                                                          select new FeeStudentTransactionDTO
                                                          {
                                                              FMH_Id = a.FMH_Id,
                                                          }
                                           ).Distinct().Take(1);

                                        if (fineheadss.Count() > 0)
                                        {
                                            obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + feehead.Amount;
                                        }
                                        //added on 11-07-2018


                                        if (obj_status_stf.FSS_NetAmount != 0 || obj_status_stf.FSS_OBArrearAmount != 0)
                                        {
                                            obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - feehead.Amount;
                                        }
                                        else
                                        {
                                            obj_status_stf.FSS_ToBePaid = 0;
                                        }

                                        _FeeGroupHeadContext.Update(obj_status_stf);
                                    }

                                  
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
                                        //pgmod.returnval = "false";
                                        //pgmod.displaymessage = "Not Saved";
                                    }
                                }
                            }

                        }
                        data.returnval = true;
                    }
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.failcnt += 1;
                data.returnval = false;
            }
            return data;
        }

        public FeeReceiptImportDTO getdetails(int id)
        {
            FeeReceiptImportDTO data = new FeeReceiptImportDTO();
            try
            {
                data.FeeGroup = _FeeGroupHeadContext.feeGroup.Where(t=>t.MI_Id==id && t.FMG_ActiceFlag==true).Distinct().ToArray();
                data.FeeHead = _FeeGroupHeadContext.feehead.Where(t=>t.MI_Id==id && t.FMH_ActiveFlag==true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeReceiptImportDTO deactiveY(FeeReceiptImportDTO data)
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


        public FeeReceiptImportDTO get_grp_reptno(FeeReceiptImportDTO data)
        {
            try
            {
              
                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_Id==data.FMG_Id && b.FGAR_Id == c.FGAR_Id)

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

    }
}
