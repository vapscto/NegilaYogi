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
    public class FAUser_GroupIMPL : interfaces.FinancialAccounting.FAUser_GroupInterface
    {
        private static ConcurrentDictionary<string, FAUser_GroupDTO> _login =
           new ConcurrentDictionary<string, FAUser_GroupDTO>();

        public FeeGroupContext _context;
        readonly ILogger<FAUser_GroupIMPL> _logger;
        public FAUser_GroupIMPL(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<FAUser_GroupIMPL> log)
        {
            _context = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        //Userchange
        public FAUser_GroupDTO Userchange(FAUser_GroupDTO data)
        {
            try
            {
                data.getreporttwo = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && (R.FAUGRP_Id != data.FAUGRP_Id) && R.FAUGRP_ParentId == data.FAUGRP_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public FAUser_GroupDTO deleterec(FAUser_GroupDTO data)
        {
            try
            {
                if (data.FAUGRP_Id > 0)
                {
                    var update = _context.FAUser_GroupDMO.Where(R => R.FAUGRP_Id == data.FAUGRP_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAUGRP_ActiveFlg == true)
                    {
                        update.FAUGRP_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAUGRP_ActiveFlg = true;
                    }
                    update.FAUGRP_UpdatedDate = DateTime.Now;
                    update.FAUGRP_UpdatedBy = data.UserId;
                    _context.Update(update);
                    var contactExists = _context.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "active";

                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                if (data.FAUGRP_IdTwo > 0)
                {
                    var update = _context.FAUser_GroupDMO.Where(R => R.FAUGRP_Id == data.FAUGRP_IdTwo && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAUGRP_ActiveFlg == true)
                    {
                        update.FAUGRP_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAUGRP_ActiveFlg = true;
                    }
                    update.FAUGRP_UpdatedDate = DateTime.Now;
                    update.FAUGRP_UpdatedBy = data.UserId;
                    _context.Update(update);
                    var contactExists = _context.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "active";

                    }
                    else
                    {
                        data.returnval = "notactive";
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
        public FAUser_GroupDTO savedetails(FAUser_GroupDTO data)
        {
            try
            {
                if (data.FAUGRP_Idone > 0)
                {
                    var result = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_UserGroupName == data.FAUGRP_UserGroupName && R.FAMGRP_Id == data.FAMGRP_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _context.FAUser_GroupDMO.Where(P => P.MI_Id == data.MI_Id && P.FAUGRP_Id == data.FAUGRP_Idone).FirstOrDefault();
                        if (update.FAUGRP_Id > 0)
                        {
                            update.FAMGRP_Id = data.FAMGRP_Id;
                            update.FAUGRP_UserGroupName = data.FAUGRP_UserGroupName;
                            update.FAUGRP_AliasName = data.FAUGRP_AliasName;
                            update.FAUGRP_Description = data.FAUGRP_Description;
                            update.FAMCOMP_Id = data.FAMCOMP_Id;
                            update.IMFY_Id = data.IMFY_Id;
                            _context.Update(update);
                            var contactExists = _context.SaveChanges();
                            if (contactExists > 0)

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
                            data.returnval = "admin";
                        }
                    }
                }
                else
                {
                    var result = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_UserGroupName == data.FAUGRP_UserGroupName && R.FAMGRP_Id == data.FAMGRP_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FAUser_GroupDMO obj = new FAUser_GroupDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMGRP_Id = data.FAMGRP_Id;
                        obj.FAUGRP_UserGroupName = data.FAUGRP_UserGroupName;
                        obj.FAUGRP_AliasName = data.FAUGRP_AliasName;
                        obj.FAUGRP_Description = data.FAUGRP_Description;
                        obj.FAMCOMP_Id = data.FAMCOMP_Id;
                        obj.IMFY_Id = data.IMFY_Id;
                        obj.FAUGRP_Position = data.FAUGRP_Position;
                        obj.FAUGRP_ParentId = data.FAUGRP_ParentId;
                        obj.FAUGRP_ActiveFlg = true;
                        obj.FAUGRP_CreatedDate = DateTime.Now;
                        obj.FAUGRP_UpdatedDate = DateTime.Now;
                        obj.FAUGRP_CreatedBy = data.UserId;
                        obj.FAUGRP_UpdatedBy = data.UserId;
                        _context.Add(obj);
                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)
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
        public FAUser_GroupDTO savedatatwo(FAUser_GroupDTO data)
        {
            try
            {
                var result = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_Id == data.FAUGRP_Id).FirstOrDefault();

                if (data.FAUGRP_IdTwo > 0)
                {
                    var resulttwo = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_UserGroupName == data.FAUGRP_UserGroupName && R.FAUGRP_ParentId == data.FAUGRP_ParentId && R.FAUGRP_Id!=data.FAUGRP_IdTwo).ToList();
                    if (resulttwo.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_Id == data.FAUGRP_IdTwo).FirstOrDefault();
                        update.FAUGRP_UserGroupName = data.FAUGRP_UserGroupName;
                        update.FAUGRP_AliasName = data.FAUGRP_AliasName;
                        update.FAUGRP_Description = data.FAUGRP_Description;
                        update.FAUGRP_Position = data.FAUGRP_Position;
                        update.FAUGRP_ParentId = data.FAUGRP_Id;
                        update.FAUGRP_UpdatedDate = DateTime.Now;
                        update.FAUGRP_UpdatedBy = data.UserId;
                        _context.Update(update);
                        var contactExists = _context.SaveChanges();
                        if (contactExists > 0)

                        {
                            data.returnval = "update";

                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                    
                }
                else
                {

                    var resulttwo = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && R.FAUGRP_UserGroupName == data.FAUGRP_UserGroupName && R.FAUGRP_ParentId == data.FAUGRP_ParentId).ToList();
                    if (resulttwo.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FAUser_GroupDMO obj = new FAUser_GroupDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMGRP_Id = result.FAMGRP_Id;
                        obj.FAUGRP_UserGroupName = data.FAUGRP_UserGroupName;
                        obj.FAUGRP_AliasName = data.FAUGRP_AliasName;
                        obj.FAUGRP_Description = data.FAUGRP_Description;
                        obj.FAMCOMP_Id = result.FAMCOMP_Id;
                        obj.IMFY_Id = result.IMFY_Id;
                        obj.FAUGRP_Position = data.FAUGRP_Position;
                        obj.FAUGRP_ParentId = data.FAUGRP_Id;
                        obj.FAUGRP_ActiveFlg = true;
                        obj.FAUGRP_CreatedDate = DateTime.Now;
                        obj.FAUGRP_UpdatedDate = DateTime.Now;
                        obj.FAUGRP_CreatedBy = data.UserId;
                        obj.FAUGRP_UpdatedBy = data.UserId;
                        _context.Add(obj);
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public FAUser_GroupDTO edit(FAUser_GroupDTO data)
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
        //getdata
        public FAUser_GroupDTO getdata(FAUser_GroupDTO data)
        {
            try
            {
                data.usergroupname = _context.FAUser_GroupDMO.Where(R => R.MI_Id == data.MI_Id && (R.FAUGRP_ParentId == R.FAUGRP_Id || R.FAUGRP_ParentId == 0)).Distinct().ToArray();

                data.fyear = _context.IVRM_Master_FinancialYear.Distinct().ToArray();
                data.companyname = _context.FACompanyMasterDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();
                data.getreport = (from a in _context.FAUser_GroupDMO
                                  from b in _context.FAMaster_GroupDMO
                                  from c in _context.FACompanyMasterDMO
                                  where (a.FAMGRP_Id == b.FAMGRP_Id && a.MI_Id == data.MI_Id && a.FAMCOMP_Id == c.FAMCOMP_Id && (a.FAUGRP_ParentId == a.FAUGRP_Id || a.FAUGRP_ParentId == 0))
                                  select new FAUser_GroupDTO
                                  {
                                      FAUGRP_UserGroupName = a.FAUGRP_UserGroupName,
                                      FAMCOMP_CompanyName = c.FAMCOMP_CompanyName,
                                      FAMGRP_GroupName = b.FAMGRP_GroupName,
                                      FAUGRP_Id = a.FAUGRP_Id,
                                      FAUGRP_ActiveFlg = a.FAUGRP_ActiveFlg,
                                      FAUGRP_AliasName = a.FAUGRP_AliasName,
                                      FAUGRP_Description = a.FAUGRP_Description,
                                      FAMCOMP_Id = a.FAMCOMP_Id,
                                      FAMGRP_Id = a.FAMGRP_Id,
                                      IMFY_Id = a.IMFY_Id

                                  }
                                ).Distinct().ToArray();
                //FAUser_GroupDMO
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
