using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services.FinancialAccounting
{
    public class FiancialAccountingVoucherIMPL : interfaces.FinancialAccounting.FiancialAccountingVoucherInterface
    {
        private static ConcurrentDictionary<string, FiancialAccountingVoucherDTO> _login =
          new ConcurrentDictionary<string, FiancialAccountingVoucherDTO>();

        public FeeGroupContext _context;
        readonly ILogger<FiancialAccountingVoucherIMPL> _logger;
        public DomainModelMsSqlServerContext _db;
        public FiancialAccountingVoucherIMPL(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<FiancialAccountingVoucherIMPL> log, DomainModelMsSqlServerContext db)
        {
            _context = YearlyFeeGroupMappingContext;
            _logger = log;
            _db = db;

        }
        public FiancialAccountingVoucherDTO deleterec(FiancialAccountingVoucherDTO data)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public FiancialAccountingVoucherDTO savedetails(FiancialAccountingVoucherDTO data)
        {
            try
            {
                var finyear = "";
                var financialyear = _context.IVRM_Master_FinancialYear.Single(a => a.IMFY_FromDate <= DateTime.Now && a.IMFY_ToDate >= DateTime.Now);
                data.IMFY_Id = financialyear.IMFY_Id;
                finyear = financialyear.IMFY_FinancialYear;
                var recno = "";
                if (data.FAMVOU_Id > 0)
                {
                    var duplicate = _context.FA_M_VoucherDMO.Where(R => R.FAMCOMP_Id == data.FAMCOMP_Id && R.FAMVOU_VoucherNo == data.FAMVOU_VoucherNo && R.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _context.FA_M_VoucherDMO.Where(M => M.FAMVOU_Id == data.FAMVOU_Id && M.MI_Id == data.MI_Id).FirstOrDefault();
                        if(update.FAMVOU_Id > 0)
                        {
                            update.IMFY_Id = data.IMFY_Id;
                            update.FAMVOU_VoucherType = data.FAMVOU_VoucherType;
                            update.FAMVOU_VoucherNo = data.FAMVOU_VoucherNo;
                            update.FAMVOU_VoucherDate = data.FAMVOU_VoucherDate;
                            update.FAMVOU_Narration = data.FAMVOU_Narration;
                            update.FAMVOU_Suffix = data.FAMVOU_Suffix;
                            update.FAMVOU_Prefix = data.FAMVOU_Prefix;
                            update.FAMVOU_UserVoucherType = data.FAMVOU_UserVoucherType;
                            update.FAMVOU_BillwiseFlg = data.FAMVOU_BillwiseFlg;
                            update.FAMVOU_Description = data.FAMVOU_Description;
                            update.FAMVOU_UpdatedDate = DateTime.Now;
                            update.FAMVOU_UpdatedBy = data.UserId;
                            _context.Update(update);
                            if(data.FA_T_Voucher !=null)
                            {
                                Array remove_unchecked_details = _context.FA_T_VoucherDMO.Where(a => a.FAMVOU_Id == data.FAMVOU_Id).Distinct().ToArray();
                                foreach (var d in remove_unchecked_details)
                                {
                                    _context.Remove(d);
                                }
                                for (int c = 0; c < data.FA_T_Voucher.Length; c++)
                                {
                                    FA_T_VoucherDMO tobj = new FA_T_VoucherDMO();
                                    tobj.FAMVOU_Id = data.FAMVOU_Id;
                                    tobj.FAMLED_Id = data.FA_T_Voucher[c].FAMLED_Id;
                                    tobj.FATVOU_Amount = data.FA_T_Voucher[c].FATVOU_Amount;
                                    tobj.FATVOU_CRDRFlg = data.FA_T_Voucher[c].FATVOU_CRDRFlg;
                                    tobj.FATVOU_TransactionTypeFlg = data.FA_T_Voucher[c].FATVOU_TransactionTypeFlg;
                                    tobj.FATVOU_Narration = data.FA_T_Voucher[c].FATVOU_Narration;
                                    tobj.FATVOU_ChequNo = data.FA_T_Voucher[c].FATVOU_ChequNo;
                                    tobj.FATVOU_ChequeDate = data.FA_T_Voucher[c].FATVOU_ChequeDate;
                                    tobj.FATVOU_BankName = data.FA_T_Voucher[c].FATVOU_BankName;
                                    tobj.FATVOU_ReferrenceNo = data.FA_T_Voucher[c].FATVOU_ReferrenceNo;
                                    tobj.FATVOU_BillwiseFlg = data.FA_T_Voucher[c].FATVOU_BillwiseFlg;
                                    tobj.FATVOU_Description = data.FA_T_Voucher[c].FATVOU_Description;
                                    tobj.FATVOU_ActiveFlg = true;
                                    tobj.FATVOU_CreatedDate = DateTime.Now;
                                    tobj.FATVOU_UpdatedDate = DateTime.Now;
                                    tobj.FATVOU_CreatedBy = data.UserId;
                                    tobj.FATVOU_UpdatedBy = data.UserId;
                                    _context.Add(tobj);

                                }
                                var i = _context.SaveChanges();
                                if (i > 0)
                                {
                                    data.returnval = "update";

                                }
                                else
                                {
                                    data.returnval = "Notupdate";
                                }
                            }
                            else
                            {

                            }
                            

                        }
                        else
                        {
                            data.returnval = "admin";
                        }
                    }
                }
                else
                {
                    //data.transnumconfigsettings = null;                  
                   GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    Voucher_NumberingDTO objvocher = new Voucher_NumberingDTO();

                    var vocherdetails = _db.Voucher_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IVN_VoucherName == data.FAMVOU_VoucherType).ToList().FirstOrDefault();

                    objvocher.MI_Id = vocherdetails.MI_Id;
                    objvocher.IVN_VoucherID = vocherdetails.IVN_VoucherID;
                    objvocher.IVN_VoucherName = vocherdetails.IVN_VoucherName;
                    objvocher.IVN_AutoManualFlag = vocherdetails.IVN_AutoManualFlag;
                    objvocher.IVN_PrefixFinYearCode = vocherdetails.IVN_PrefixFinYearCode;
                    objvocher.IVN_PrefixParticular = vocherdetails.IVN_PrefixParticular;
                    objvocher.IVN_RestartNumFlag = vocherdetails.IVN_RestartNumFlag;
                    objvocher.IVN_StartingNo = vocherdetails.IVN_StartingNo;
                    objvocher.IVN_SuffixParticular = vocherdetails.IVN_SuffixParticular;
                    objvocher.IVN_SuffixFinYearCode = vocherdetails.IVN_SuffixFinYearCode;
                    objvocher.IVN_WidthNumeric = vocherdetails.IVN_WidthNumeric;
                    objvocher.IVN_ZeroPrefixFlag = vocherdetails.IVN_ZeroPrefixFlag;


                    recno = a.TransactionNumberingFA(objvocher, finyear);


                    var duplicate = _context.FA_M_VoucherDMO.Where(R => R.FAMCOMP_Id == data.FAMCOMP_Id && R.FAMVOU_VoucherNo == data.FAMVOU_VoucherNo && R.MI_Id == data.MI_Id).ToList();
                    if(duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FA_M_VoucherDMO obj = new FA_M_VoucherDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMCOMP_Id = data.FAMCOMP_Id;
                        obj.IMFY_Id = data.IMFY_Id;
                        obj.FAMVOU_VoucherType = data.FAMVOU_VoucherType;
                        obj.FAMVOU_VoucherNo = recno;
                        obj.FAMVOU_VoucherDate = data.FAMVOU_VoucherDate;
                        obj.FAMVOU_Narration = string.Empty;
                        obj.FAMVOU_Suffix = string.Empty;
                        obj.FAMVOU_Prefix = string.Empty;
                        obj.FAMVOU_UserVoucherType = data.FAMVOU_UserVoucherType;
                        obj.FAMVOU_APIReferenceNo = string.Empty;
                        obj.FAMVOU_BillwiseFlg = false;
                        obj.FAMVOU_Description = string.Empty;
                        obj.FAMVOU_ActiveFlg = true;
                        obj.FAMVOU_CreatedDate = DateTime.Now;
                        obj.FAMVOU_UpdatedDate = DateTime.Now;
                        obj.FAMVOU_CreatedBy = data.UserId;
                        obj.FAMVOU_UpdatedBy = data.UserId;
                        _context.Add(obj);
                        if(data.FA_T_Voucher !=null)
                        {
                            for (int c = 0; c < data.FA_T_Voucher.Length; c++)
                            {
                                FA_T_VoucherDMO tobj = new FA_T_VoucherDMO();
                                tobj.FAMVOU_Id = obj.FAMVOU_Id;
                                tobj.FAMLED_Id = data.FA_T_Voucher[c].FAMLED_Id;
                                tobj.FATVOU_Amount = data.FA_T_Voucher[c].FATVOU_Amount;
                                tobj.FATVOU_CRDRFlg = data.FA_T_Voucher[c].FATVOU_CRDRFlg;
                                tobj.FATVOU_TransactionTypeFlg = string.Empty;
                                tobj.FATVOU_Narration = string.Empty; 
                                tobj.FATVOU_ChequNo = string.Empty;
                                tobj.FATVOU_ChequeDate = data.FAMVOU_VoucherDate;
                                tobj.FATVOU_BankName = string.Empty;
                                tobj.FATVOU_ReferrenceNo = string.Empty;
                                tobj.FATVOU_BillwiseFlg = false;
                                tobj.FATVOU_Description = string.Empty;
                                tobj.FATVOU_ActiveFlg = true;
                                tobj.FATVOU_CreatedDate = DateTime.Now;
                                tobj.FATVOU_UpdatedDate = DateTime.Now;
                                tobj.FATVOU_CreatedBy = data.UserId;
                                tobj.FATVOU_UpdatedBy = data.UserId;
                                _context.Add(tobj);

                            }
                            var i = _context.SaveChanges();
                            if (i > 0)
                            {
                                data.returnval = "save";
                            }
                            else
                            {
                                data.returnval = "Notsave";
                            }
                        }
                        else
                        {
                            data.returnval = "admin";
                        }
                       
                    }
                  

                }
                


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //savedatatwo
        public FiancialAccountingVoucherDTO savedatatwo(FiancialAccountingVoucherDTO data)
        {
            try
            {
                //FAMCOMP_Id
                if(data.FAMCOMP_Id > 0)
                {
                    data.ledgerdetails = (from a in _context.FACompanyMasterDMO
                                        from b in _context.FA_M_LedgerDMO
                                        from c in _context.FA_M_Ledger_DetailsDMO
                                        where (a.FAMCOMP_Id == b.FAMCOMP_Id && b.FAMLED_ActiveFlg == true && a.MI_Id == data.MI_Id && b.FAMLED_Id==c.FAMLED_Id )
                                        select new FiancialAccountingVoucherDTO
                                        {
                                           FAMLED_Id=b.FAMLED_Id,
                                            FAMLED_LedgerName = b.FAMLED_LedgerName,
                                            FAMLEDD_OBCRDRFlg=c.FAMLEDD_OBCRDRFlg
                                        }
                                        ).Distinct().ToArray();
                   
                }
                else
                {
                    data.returnval = "admin";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public FiancialAccountingVoucherDTO edit(FiancialAccountingVoucherDTO data)
        {
            try
            {
                if(data.FAMVOU_Id > 0)
                {
                    data.editMvoucher = _context.FA_M_VoucherDMO.Where(R => R.FAMVOU_Id == data.FAMVOU_Id && R.MI_Id == data.MI_Id).Distinct().ToArray();
                    data.editTvoucher = (from a in _context.FA_M_VoucherDMO
                                         from b in _context.FA_T_VoucherDMO
                                         where (a.FAMVOU_Id == b.FAMVOU_Id && b.FAMVOU_Id == data.FAMVOU_Id && a.FAMVOU_ActiveFlg == true)
                                         select b).Distinct().ToArray();
                }
                else
                {
                    data.returnval = "admin";
                }
               
               // 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //getdata
        public FiancialAccountingVoucherDTO getdata(FiancialAccountingVoucherDTO data)
        {
            try
            {
                data.fyear = _context.IVRM_Master_FinancialYear.Distinct().ToArray();
                // data.companyname = _context.FACompanyMasterDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();
                data.companyname = (from a in _context.FACompanyMasterDMO
                                   
                                    where ( a.FAMCOMP_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                    select a).Distinct().ToArray();
                data.getreport = (from a in _context.FA_M_VoucherDMO
                                  from b in _context.FACompanyMasterDMO
                                  from c in _context.IVRM_Master_FinancialYear
                                  where (a.FAMCOMP_Id == b.FAMCOMP_Id && a.IMFY_Id == c.IMFY_Id && a.MI_Id == data.MI_Id)
                                  select new FiancialAccountingVoucherDTO
                                  {
                                      FAMCOMP_CompanyName = b.FAMCOMP_CompanyName,
                                      FAMVOU_Id = a.FAMVOU_Id,
                                      IMFY_FinancialYear = c.IMFY_FinancialYear,
                                      FAMVOU_VoucherType=a.FAMVOU_VoucherType,
                                      FAMVOU_VoucherNo=a.FAMVOU_VoucherNo,
                                      FAMVOU_VoucherDate=a.FAMVOU_VoucherDate,
                                      FAMVOU_UserVoucherType=a.FAMVOU_UserVoucherType,
                                      FAMVOU_ActiveFlg = a.FAMVOU_ActiveFlg

                                  }
                                 ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
