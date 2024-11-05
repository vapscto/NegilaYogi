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
    public class FAMasterGroupIMPL : interfaces.FinancialAccounting.FAMasterGroupInterface
    {
        private static ConcurrentDictionary<string, FAMasterGroupDTO> _login =
           new ConcurrentDictionary<string, FAMasterGroupDTO>();

        public FeeGroupContext _context;
        readonly ILogger<FAMasterGroupIMPL> _logger;
        public FAMasterGroupIMPL(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<FAMasterGroupIMPL> log)
        {
            _context = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public FAMasterGroupDTO deleterec(FAMasterGroupDTO data)
        {
            try
            {
                if (data.FAMGRP_Id > 0)
                {
                    var update = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAMGRP_ActiveFlg == true)
                    {
                        update.FAMGRP_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAMGRP_ActiveFlg = true;
                    }
                    update.FAMGRP_UpdatedDate = DateTime.Now;
                    update.FAMGRP_UpdatedBy = data.UserId;
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
                if (data.FAMGRP_IdTwo > 0)
                {
                    var update = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_IdTwo && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAMGRP_ActiveFlg == true)
                    {
                        update.FAMGRP_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAMGRP_ActiveFlg = true;
                    }
                    update.FAMGRP_UpdatedDate = DateTime.Now;
                    update.FAMGRP_UpdatedBy = data.UserId;
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
        public FAMasterGroupDTO savedetails(FAMasterGroupDTO data)
        {
            try
            {
                if (data.FAMGRP_Id > 0)
                {
                    var result = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_GroupName == data.FAMGRP_GroupName && R.FAMGRP_GroupCode == data.FAMGRP_GroupCode && R.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                        if (update.FAMGRP_Id > 0)
                        {
                            update.FAMGRP_GroupName = data.FAMGRP_GroupName;
                            update.FAMGRP_GroupCode = data.FAMGRP_GroupCode;
                            update.FAMGRP_Description = data.FAMGRP_Description;
                            update.FAMGRP_BSPLFlg = data.FAMGRP_BSPLFlg;
                            update.FAMGRP_CRDRFlg = data.FAMGRP_CRDRFlg;
                            update.FAMGRP_UpdatedDate = DateTime.Now;
                            update.FAMGRP_UpdatedBy = data.UserId;
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
                    var result = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_GroupName == data.FAMGRP_GroupName && R.FAMGRP_GroupCode == data.FAMGRP_GroupCode && R.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FAMaster_GroupDMO obj = new FAMaster_GroupDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMGRP_GroupName = data.FAMGRP_GroupName;
                        obj.FAMGRP_GroupCode = data.FAMGRP_GroupCode;
                        obj.FAMGRP_Description = data.FAMGRP_Description;
                        obj.FAMGRP_BSPLFlg = data.FAMGRP_BSPLFlg;
                        obj.FAMGRP_CRDRFlg = data.FAMGRP_CRDRFlg;
                        obj.FAMGRP_Position = data.FAMGRP_Position;
                        obj.FAMGRP_ParentId = data.FAMGRP_ParentId;
                        obj.FAMGRP_ActiveFlg = true;
                        obj.FAMGRP_CreatedDate = DateTime.Now;
                        obj.FAMGRP_UpdatedDate = DateTime.Now;
                        obj.FAMGRP_CreatedBy = data.UserId;
                        obj.FAMGRP_UpdatedBy = data.UserId;
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
        public FAMasterGroupDTO savedatatwo(FAMasterGroupDTO data)
        {
            try
            {

                
                if (data.FAMGRP_IdTwo > 0)
                {
                    var list = _context.FAMaster_GroupDMO.Where(M => M.FAMGRP_GroupName == data.FAMGRP_GroupName && M.MI_Id == data.MI_Id && M.FAMGRP_ParentId == data.FAMGRP_Id && M.FAMGRP_Id !=data.FAMGRP_IdTwo).ToList();
                    if (list.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var result = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_Id == data.FAMGRP_IdTwo && R.MI_Id == data.MI_Id).FirstOrDefault();
                        result.FAMGRP_GroupName = data.FAMGRP_GroupName;
                        result.FAMGRP_GroupCode = data.FAMGRP_GroupCode;
                        result.FAMGRP_Description = data.FAMGRP_Description;
                        result.FAMGRP_BSPLFlg = data.FAMGRP_BSPLFlg;
                        result.FAMGRP_CRDRFlg = data.FAMGRP_CRDRFlg;
                        result.FAMGRP_Position = data.FAMGRP_Position;
                        result.FAMGRP_ParentId = data.FAMGRP_Id;
                        result.FAMGRP_UpdatedDate = DateTime.Now;
                        result.FAMGRP_UpdatedBy = data.UserId;
                        _context.Update(result);
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
                    var list = _context.FAMaster_GroupDMO.Where(M => M.FAMGRP_GroupName == data.FAMGRP_GroupName && M.MI_Id == data.MI_Id && M.FAMGRP_ParentId == data.FAMGRP_Id).ToList();
                    if (list.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FAMaster_GroupDMO obj = new FAMaster_GroupDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMGRP_GroupName = data.FAMGRP_GroupName;
                        obj.FAMGRP_GroupCode = data.FAMGRP_GroupCode;
                        obj.FAMGRP_Description = data.FAMGRP_Description;
                        obj.FAMGRP_BSPLFlg = data.FAMGRP_BSPLFlg;
                        obj.FAMGRP_CRDRFlg = data.FAMGRP_CRDRFlg;
                        obj.FAMGRP_Position = data.FAMGRP_Position;
                        obj.FAMGRP_ParentId = data.FAMGRP_Id;
                        obj.FAMGRP_ActiveFlg = true;
                        obj.FAMGRP_CreatedDate = DateTime.Now;
                        obj.FAMGRP_UpdatedDate = DateTime.Now;
                        obj.FAMGRP_CreatedBy = data.UserId;
                        obj.FAMGRP_UpdatedBy = data.UserId;
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

        public FAMasterGroupDTO edit(FAMasterGroupDTO data)
        {
            try
            {
                data.getreporttwo = _context.FAMaster_GroupDMO.Where(R => R.FAMGRP_ParentId == data.FAMGRP_Id && R.FAMGRP_Id !=data.FAMGRP_Id && R.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //getdata
        public FAMasterGroupDTO getdata(FAMasterGroupDTO data)
        {
            try
            {
                // data.getreport = _context.FAMaster_GroupDMO.Where(P => P.MI_Id == data.MI_Id).Distinct().ToArray();
              // data.getgroupname = _context.FAMaster_GroupDMO.Where(P => P.MI_Id == data.MI_Id).Distinct().ToArray();
              
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
                        data.getreport = retObject.Distinct().ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.getgroupname = data.getreport;
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
