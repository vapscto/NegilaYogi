using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services.FinancialAccounting
{
    public class FiancialAccuntingLedgerIMPL : interfaces.FinancialAccounting.FiancialAccuntingLedgerInterface
    {
        private static ConcurrentDictionary<string, FiancialAccuntingLedgerDTO> _login =
           new ConcurrentDictionary<string, FiancialAccuntingLedgerDTO>();

        public FeeGroupContext _context;
        readonly ILogger<FiancialAccuntingLedgerIMPL> _logger;
        public FiancialAccuntingLedgerIMPL(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<FiancialAccuntingLedgerIMPL> log)
        {
            _context = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public FiancialAccuntingLedgerDTO deleterec(FiancialAccuntingLedgerDTO data)
        {
            try
            {
                var result = _context.FA_M_LedgerDMO.Single(t => t.FAMLED_Id == data.FAMLED_Id);                if (result.FAMLED_ActiveFlg == true)                {                    result.FAMLED_ActiveFlg = false;                }                else if (result.FAMLED_ActiveFlg == false)                {                    result.FAMLED_ActiveFlg = true;                }                result.FAMLED_UpdatedDate = DateTime.Now;                _context.Update(result);                int returnval = _context.SaveChanges();                if (returnval > 0)                {                    data.returnval = "active";                }                else                {                    data.returnval = "admin";                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public FiancialAccuntingLedgerDTO savedetails(FiancialAccuntingLedgerDTO data)
        {
            try
            {
                       
               if(data.FAMLED_Id > 0)
                {
                    // var result = _context.FA_M_LedgerDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_Id && R.FAMCOMP_Id == data.FAMCOMP_Id && R.FAUGRP_Id == data.FAUGRP_Id && R.FAMLED_LedgerName == data.FAMLED_LedgerName && R.MI_Id == data.MI_Id).ToList();

                    var result = (from a in _context.FA_M_LedgerDMO
                                  from b in _context.FA_M_Ledger_DetailsDMO
                                  where( a.FAMLED_Id == b.FAMLED_Id && a.FAMLED_Id == data.FAMLED_Id && a.FAMGRP_Id == data.FAMGRP_Id && a.FAUGRP_Id == data.FAUGRP_Id &&
                                  a.FAMLED_LedgerName == data.FAMLED_LedgerName && a.MI_Id == data.MI_Id && b.FAMLEDD_OpeningBalance == data.FAMLEDD_OpeningBalance)
                                  select new FiancialAccuntingLedgerDTO
                                  {
                                      FAMLED_Id = a.FAMLED_Id

                                  }).ToList();
                                 

                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _context.FA_M_LedgerDMO.Where(M => M.FAMLED_Id == data.FAMLED_Id && M.MI_Id == data.MI_Id).FirstOrDefault();
                        if(update.FAMLED_Id > 0)
                        {
                            update.MI_Id = data.MI_Id;
                            update.FAMCOMP_Id = data.FAMCOMP_Id;
                            update.IMFY_Id = data.IMFY_Id;
                            update.FAMGRP_Id = data.FAMGRP_Id;
                            update.FAUGRP_Id = data.FAUGRP_Id;
                            update.FAMLED_LedgerName = data.FAMLED_LedgerName;
                            update.FAMLED_LedgerAliasName = data.FAMLED_LedgerAliasName;
                            update.FAMLED_LedgerCreatedDate = data.FAMLED_LedgerCreatedDate;
                            update.FAMLED_Remarks = data.FAMLED_Remarks;
                            update.FAMLED_PostalAddress = data.FAMLED_PostalAddress;
                            update.FAMLED_EmailAddress = data.FAMLED_EmailAddress;
                            update.FAMLED_Type = data.FAMLED_Type;
                            update.FAMLED_Under = data.FAMLED_Under;
                            update.FAMLED_BillwiseFlg = data.FAMLED_BillwiseFlg;
                            update.FAMLED_ActiveFlg = true;
                            update.FAMLED_UpdatedDate = DateTime.Now;
                            update.FAMLED_UpdatedBy = data.UserId;
                            _context.Update(update);


                            //Array remove_unchecked_details = _context.FA_M_Ledger_DetailsDMO.Where(a => a.FAMLED_Id == data.FAMLED_Id).ToArray();
                            //foreach (var d in remove_unchecked_details)
                            //{
                            //    _context.Remove(d);
                            //}
                            // FA_M_Ledger_DetailsDMO dobj = new FA_M_Ledger_DetailsDMO();

                            var dobj = _context.FA_M_Ledger_DetailsDMO.Single(a => a.FAMLED_Id == data.FAMLED_Id);

                            dobj.FAMLED_Id = dobj.FAMLED_Id;
                            dobj.IMFY_Id = data.IMFY_Id;
                            dobj.FAMLEDD_OpeningBalance = data.FAMLEDD_OpeningBalance;
                            dobj.FAMLEDD_OBCRDRFlg = data.FAMLEDD_OBCRDRFlg;
                            dobj.FAMLEDD_ClosingBalance = data.FAMLEDD_ClosingBalance;
                            dobj.FAMLEDD_CBCRDRFlg = data.FAMLEDD_CBCRDRFlg;
                            dobj.FAMLEDD_OBDate = data.FAMLED_LedgerCreatedDate;
                            dobj.FAMLEDD_BudgetAmount = data.FAMLEDD_BudgetAmount;
                            dobj.FAMLEDD_Remarks = data.FAMLEDD_Remarks;
                            dobj.FAMLEDD_ActiveFlg = true;
                            dobj.FAMLEDD_CreatedDate = DateTime.Now;
                            dobj.FAMLEDD_UpdatedDate = DateTime.Now;
                            dobj.FAMLEDD_CreatedBy = data.UserId;
                            dobj.FAMLEDD_UpdatedBy = data.UserId;
                            _context.Update(dobj);

                            var i = _context.SaveChanges();
                                if (i > 0)
                                {
                                    data.returnval = "update";

                                }
                                else
                                {
                                    data.returnval = "notupdate";
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
                    var result = _context.FA_M_LedgerDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_Id && R.FAMCOMP_Id == data.FAMCOMP_Id && R.FAUGRP_Id == data.FAUGRP_Id && R.FAMLED_LedgerName == data.FAMLED_LedgerName && R.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FA_M_LedgerDMO obj = new FA_M_LedgerDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMCOMP_Id = data.FAMCOMP_Id;
                        obj.IMFY_Id = data.IMFY_Id;
                        obj.FAMGRP_Id = data.FAMGRP_Id;
                        obj.FAUGRP_Id = data.FAUGRP_Id;
                        obj.FAMLED_LedgerName = data.FAMLED_LedgerName;
                        obj.FAMLED_LedgerAliasName = data.FAMLED_LedgerAliasName;
                        obj.FAMLED_LedgerCreatedDate = data.FAMLED_LedgerCreatedDate;
                        obj.FAMLED_Remarks = data.FAMLED_Remarks;
                        obj.FAMLED_PostalAddress = data.FAMLED_PostalAddress;
                        obj.FAMLED_EmailAddress = data.FAMLED_EmailAddress;
                        obj.FAMLED_Type = data.FAMLED_Type;
                        obj.FAMLED_Under = data.FAMLED_Under;
                        obj.FAMLED_BillwiseFlg = data.FAMLED_BillwiseFlg;
                        obj.FAMLED_ActiveFlg = true;
                        obj.FAMLED_CreatedDate = DateTime.Now;
                        obj.FAMLED_UpdatedDate = DateTime.Now;
                        obj.FAMLED_CreatedBy = data.UserId;
                        obj.FAMLED_UpdatedBy = data.UserId;
                        _context.Add(obj);
                       
                                FA_M_Ledger_DetailsDMO dobj = new FA_M_Ledger_DetailsDMO();
                                dobj.FAMLED_Id = obj.FAMLED_Id;
                                dobj.IMFY_Id = data.IMFY_Id;
                                dobj.FAMLEDD_OpeningBalance = data.FAMLEDD_OpeningBalance;
                                dobj.FAMLEDD_OBCRDRFlg = data.FAMLEDD_OBCRDRFlg;
                                dobj.FAMLEDD_ClosingBalance = data.FAMLEDD_ClosingBalance;
                                dobj.FAMLEDD_CBCRDRFlg = data.FAMLEDD_CBCRDRFlg;
                                dobj.FAMLEDD_OBDate = data.FAMLED_LedgerCreatedDate;
                                dobj.FAMLEDD_BudgetAmount = data.FAMLEDD_BudgetAmount;
                                dobj.FAMLEDD_Remarks = data.FAMLEDD_Remarks;
                                dobj.FAMLEDD_ActiveFlg = true;
                                dobj.FAMLEDD_CreatedDate = DateTime.Now;
                                dobj.FAMLEDD_UpdatedDate = DateTime.Now;
                                dobj.FAMLEDD_CreatedBy = data.UserId;
                                dobj.FAMLEDD_UpdatedBy = data.UserId;
                                _context.Add(dobj);
                            
                            var context = _context.SaveChanges();
                            if (context > 0)
                            {
                                data.returnval = "save";

                            }
                            else
                            {
                                data.returnval = "Notsave";
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
        public FiancialAccuntingLedgerDTO savedatatwo(FiancialAccuntingLedgerDTO data)
        {
            try
            {


                data.usergroupname = (from a in _context.FAMaster_GroupDMO
                                      from b in _context.FAUser_GroupDMO
                                      where (a.FAMGRP_Id == b.FAMGRP_Id && a.FAMGRP_ActiveFlg == true && b.FAUGRP_ActiveFlg == true && b.FAMGRP_Id == data.FAMGRP_Id)
                                      select new FiancialAccuntingLedgerDTO
                                      {
                                          FAUGRP_UserGroupName = b.FAUGRP_UserGroupName,
                                          FAUGRP_Id = b.FAUGRP_Id
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

        public FiancialAccuntingLedgerDTO edit(FiancialAccuntingLedgerDTO data)
        {
            try
            {
                if (data.FAMLED_Id > 0)
                {
                    data.editledger = _context.FA_M_LedgerDMO.Where(R => R.FAMLED_Id == data.FAMLED_Id && R.MI_Id == data.MI_Id).Distinct().ToArray();

                    data.editledgerdetail = (from a in _context.FA_M_LedgerDMO
                                             from b in _context.FA_M_Ledger_DetailsDMO
                                             where (a.FAMLED_Id == b.FAMLED_Id && b.FAMLED_Id == data.FAMLED_Id)
                                             select b).Distinct().ToArray();

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
        //getdata
        public FiancialAccuntingLedgerDTO getdata(FiancialAccuntingLedgerDTO data)
        {
            try
            {
                //  
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FALedgerDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getreport = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FAMasterGroupList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.getgroupname = retObject.Distinct().ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.companyname = _context.FACompanyMasterDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();
                data.fyear = _context.IVRM_Master_FinancialYear.Distinct().ToArray();

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
