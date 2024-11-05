using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
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
    public class AutoLedgerCreationImpl : interfaces.FinancialAccounting.AutoLedgerCreationInterface
    {
        private static ConcurrentDictionary<string, AutoLedgerCreationDTO> _login =
         new ConcurrentDictionary<string, AutoLedgerCreationDTO>();

        public FeeGroupContext _context;
        readonly ILogger<AutoLedgerCreationImpl> _logger;
        public AutoLedgerCreationImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<AutoLedgerCreationImpl> log)
        {
            _context = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public AutoLedgerCreationDTO deleterec(AutoLedgerCreationDTO data)
        {
            try
            {
                var result = _context.FA_M_LedgerDMO.Single(t => t.FAMLED_Id == data.FAMLED_Id);

                if (result.FAMLED_ActiveFlg == true)
                {
                    result.FAMLED_ActiveFlg = false;
                }
                else if (result.FAMLED_ActiveFlg == false)
                {
                    result.FAMLED_ActiveFlg = true;
                }
                result.FAMLED_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int returnval = _context.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "active";
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
        public AutoLedgerCreationDTO savedetails(AutoLedgerCreationDTO data)
        {
            try
            {


                foreach (var x in data.Amstid)
                {
                    var datanew = _context.Database.ExecuteSqlCommand("AUtoLedgerCreation @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, data.IMFY_Id, data.FAMCOMP_Id, data.FAMGRP_Id, data.UserId, data.type, x, data.crdrflg);

                    if (datanew >= 1)
                    {
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }
                }

                foreach (var x in data.salesid)
                {
                    var datanew = _context.Database.ExecuteSqlCommand("AUtoLedgerCreation @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, data.IMFY_Id, data.FAMCOMP_Id, data.FAMGRP_Id, data.UserId, data.type, x, data.crdrflg);

                    if (datanew >= 1)
                    {
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }

                }

                foreach (var x in data.itemid)
                {
                    var datanew = _context.Database.ExecuteSqlCommand("AUtoLedgerCreation @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", data.MI_Id, data.IMFY_Id, data.FAMCOMP_Id, data.FAMGRP_Id, data.UserId, data.type, x, data.crdrflg);

                    if (datanew >= 1)
                    {
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
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
        public AutoLedgerCreationDTO savedatatwo(AutoLedgerCreationDTO data)
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
  public AutoLedgerCreationDTO sectionchange(AutoLedgerCreationDTO data)
        {
            try
            {

                data.studentdata = (from a in _context.AdmissionStudentDMO
                                       from b in _context.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                       && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1)
                                       select new FeeOpeningBalanceDTO
                                       {
                                           Amst_Id = a.AMST_Id,
                                           AMST_FirstName = a.AMST_FirstName == null ? "" : a.AMST_FirstName,
                                           AMST_MiddleName = a.AMST_MiddleName == null ? "" : a.AMST_MiddleName,
                                           AMST_LastName = (a.AMST_LastName == null ? "" : a.AMST_LastName) + "-" + a.AMST_AdmNo,
                                       }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public AutoLedgerCreationDTO edit(AutoLedgerCreationDTO data)
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
        public AutoLedgerCreationDTO getdata(AutoLedgerCreationDTO data)
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

                data.classarr = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag==true).Distinct().ToArray();
                data.sectionarr = _context.school_M_Section.Where(z => z.MI_Id == data.MI_Id && z.ASMC_ActiveFlag==1).Distinct().ToArray();
                data.supplierdata = _context.INV_Master_SupplierDMO.Where(q => q.MI_Id == data.MI_Id ).Distinct().ToArray();
                data.itemdata = _context.INV_Master_ItemDMO.Where(w => w.MI_Id == data.MI_Id && w.INVMI_ActiveFlg == true).Distinct().ToArray();


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
