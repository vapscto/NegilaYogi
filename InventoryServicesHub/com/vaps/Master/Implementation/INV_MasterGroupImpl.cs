using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Implementation
{
    public class INV_MasterGroupImpl : Interface.INV_MasterGroupInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterGroupImpl> _logInv;
        public INV_MasterGroupImpl(InventoryContext InvContext, ILogger<INV_MasterGroupImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_GroupDTO getloaddata(INV_Master_GroupDTO data)
        {
            try
            {
                data.get_maingroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "MG").OrderBy(m => m.INVMG_Id).ToArray();
                data.get_maingroupdd = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "MG" && m.INVMG_ActiveFlg==true).OrderBy(m => m.INVMG_Id).ToArray();
                data.get_usergp = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "UG" && m.INVMG_ActiveFlg==true).OrderBy(m => m.INVMG_Id).ToArray();

                data.get_usergroup = (from a in _INVContext.INV_Master_GroupDMO
                                      from b in _INVContext.INV_Master_GroupDMO
                                      where a.INVMG_ParentId == b.INVMG_Id && a.INVMG_MGUGIGFlg == "UG" && a.MI_Id == data.MI_Id
                                      select new INV_Master_GroupDTO
                                      {
                                          INVMG_Id = a.INVMG_Id,
                                          INVMG_GroupName = a.INVMG_GroupName,
                                          INVMG_AliasName = a.INVMG_AliasName,
                                          INVMG_ActiveFlg = a.INVMG_ActiveFlg,
                                          INVMG_GroupName_mn = b.INVMG_GroupName
                                      }).OrderByDescending(a=>a.INVMG_Id).ToArray();

                //data.get_usergroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "UG").OrderBy(m => m.INVMG_Id).ToArray();

                data.get_itemgroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "IG").OrderBy(m => m.INVMG_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Master Group getloaddata :" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_GroupDTO savedetails(INV_Master_GroupDTO data)
        {
            try
            {
                if (data.INVMG_Id != 0)
                {

                    var level_no = "";
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "MG" && a.INVMG_Id == data.INVMG_Id)
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();

                    level_no = level.FirstOrDefault().INVMG_Level;

                    var res = _INVContext.INV_Master_GroupDMO.Where(t => t.INVMG_GroupName == data.INVMG_GroupName && t.MI_Id == data.MI_Id &&
                    t.INVMG_Id != data.INVMG_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_GroupDMO.Single(t => t.INVMG_Id == data.INVMG_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMG_GroupName = data.INVMG_GroupName;
                        result.INVMG_AliasName = data.INVMG_AliasName;
                        result.INVMG_MGUGIGFlg = "MG";
                        result.INVMG_Level = level_no;
                        result.INVMG_ParentId = result.INVMG_Id;
                        result.INVMG_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        result.INVMG_GroupStartingNo = data.INVMG_GroupStartingNo;
                        result.INVMG_GroupSuffix = data.INVMG_GroupSuffix;
                        result.INVMG_GroupPrefix = data.INVMG_GroupPrefix;
                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {

                    int level_no = 0;
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "MG")
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();
                    var max_level = level.Count();
                    if (max_level <= 0)
                    {
                        level_no = 1;
                    }
                    else
                    {
                        level_no = max_level + 1;
                    }

                    var res = _INVContext.INV_Master_GroupDMO.Where(t => (t.INVMG_GroupName == data.INVMG_GroupName) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_GroupDMO grp = new INV_Master_GroupDMO();
                        grp.MI_Id = data.MI_Id;
                        grp.INVMG_GroupName = data.INVMG_GroupName;
                        grp.INVMG_AliasName = data.INVMG_AliasName;
                        grp.INVMG_MGUGIGFlg = "MG";
                        grp.INVMG_Level = level_no.ToString();
                        grp.INVMG_ActiveFlg = true;
                        grp.INVMG_GroupStartingNo = data.INVMG_GroupStartingNo;
                        grp.INVMG_GroupSuffix = data.INVMG_GroupSuffix;
                        grp.INVMG_GroupPrefix = data.INVMG_GroupPrefix;
                        grp.CreatedDate = DateTime.Now;
                        grp.UpdatedDate = DateTime.Now;
                        _INVContext.Add(grp);
                        _INVContext.SaveChanges();

                        INV_Master_GroupDMO grpupdate = new INV_Master_GroupDMO();
                        grp.INVMG_ParentId = grp.INVMG_Id;

                        _INVContext.Update(grp);
                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Group save data :" + ex.Message);
            }
            return data;
        }

        public INV_Master_GroupDTO savedetailsUG(INV_Master_GroupDTO data)
        {
            try
            {

                if (data.INVMG_Id != 0)
                {
                    var level_no = "";
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "UG" && a.INVMG_Id == data.INVMG_Id)
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();

                    level_no = level.FirstOrDefault().INVMG_Level;
                    var res = _INVContext.INV_Master_GroupDMO.Where(t => t.INVMG_GroupName == data.INVMG_GroupName && t.MI_Id == data.MI_Id &&
                    t.INVMG_Id != data.INVMG_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_GroupDMO.Single(t => t.INVMG_Id == data.INVMG_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMG_GroupName = data.INVMG_GroupName;
                        result.INVMG_AliasName = data.INVMG_AliasName;
                        result.INVMG_MGUGIGFlg = "UG";
                        result.INVMG_Level = level_no;
                        result.INVMG_ParentId = data.INVMG_ParentId;
                        result.INVMG_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    int level_no = 0;
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "UG" && a.INVMG_ParentId == data.INVMG_ParentId)
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();

                    var max_level = level.Count();
                    if (max_level <= 0)
                    {
                        level_no = 1;
                    }
                    else
                    {
                        level_no = max_level + 1;
                    }
                    var res = _INVContext.INV_Master_GroupDMO.Where(t => (t.INVMG_GroupName == data.INVMG_GroupName) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_GroupDMO grp = new INV_Master_GroupDMO();
                        grp.MI_Id = data.MI_Id;
                        grp.INVMG_GroupName = data.INVMG_GroupName;
                        grp.INVMG_AliasName = data.INVMG_AliasName;
                        grp.INVMG_MGUGIGFlg = "UG";
                        grp.INVMG_Level = level_no.ToString();
                        grp.INVMG_ParentId = data.INVMG_ParentId;
                        grp.INVMG_ActiveFlg = true;

                        grp.CreatedDate = DateTime.Now;
                        grp.UpdatedDate = DateTime.Now;
                        _INVContext.Add(grp);
                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Group save data UG :" + ex.Message);
            }
            return data;
        }

        public INV_Master_GroupDTO groupChange(INV_Master_GroupDTO data)
        {
            data.getusergroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "UG" && m.INVMG_ParentId == data.INVMG_ParentId &&
            m.INVMG_ActiveFlg == true).OrderBy(m => m.INVMG_Id).ToArray();

            return data;
        }
        public INV_Master_GroupDTO savedetailsIG(INV_Master_GroupDTO data)
        {
            try
            {
                if (data.INVMG_Id != 0)
                {
                    var level_no = "";
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "IG" && a.INVMG_Id == data.INVMG_Id)
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();

                    level_no = level.FirstOrDefault().INVMG_Level;

                    var res = _INVContext.INV_Master_GroupDMO.Where(t => t.INVMG_GroupName == data.INVMG_GroupName && t.MI_Id == data.MI_Id &&
                    t.INVMG_Id != data.INVMG_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_GroupDMO.Single(t => t.INVMG_Id == data.INVMG_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMG_GroupName = data.INVMG_GroupName;
                        result.INVMG_AliasName = data.INVMG_AliasName;
                        result.INVMG_MGUGIGFlg = "IG";
                        result.INVMG_Level = level_no;
                        result.INVMG_ParentId = data.INVMG_ParentId;
                        result.INVMG_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    int level_no = 0;
                    var level = (from a in _INVContext.INV_Master_GroupDMO
                                 where (a.MI_Id == data.MI_Id && a.INVMG_MGUGIGFlg == "IG" && a.INVMG_ParentId == data.INVMG_ParentId)
                                 select new INV_Master_GroupDTO
                                 {
                                     INVMG_Level = a.INVMG_Level,
                                 }).Distinct().ToList();

                    var max_level = level.Count();
                    if (max_level <= 0)
                    {
                        level_no = 1;
                    }
                    else
                    {
                        level_no = max_level + 1;
                    }
                    var res = _INVContext.INV_Master_GroupDMO.Where(t => (t.INVMG_GroupName == data.INVMG_GroupName) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_GroupDMO grp = new INV_Master_GroupDMO();
                        grp.MI_Id = data.MI_Id;
                        grp.INVMG_GroupName = data.INVMG_GroupName;
                        grp.INVMG_AliasName = data.INVMG_AliasName;
                        grp.INVMG_MGUGIGFlg = "IG";
                        grp.INVMG_Level = level_no.ToString();
                        grp.INVMG_ParentId = data.INVMG_ParentId;
                        grp.INVMG_ActiveFlg = true;

                        grp.CreatedDate = DateTime.Now;
                        grp.UpdatedDate = DateTime.Now;
                        _INVContext.Add(grp);
                        var contactExists = _INVContext.SaveChanges();

                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Group save data UG :" + ex.Message);
            }
            return data;
        }

        public INV_Master_GroupDTO deactive(INV_Master_GroupDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_GroupDMO.Single(t => t.INVMG_Id == data.INVMG_Id);

                if (result.INVMG_ActiveFlg == true)
                {
                    result.INVMG_ActiveFlg = false;
                }
                else if (result.INVMG_ActiveFlg == false)
                {
                    result.INVMG_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public INV_Master_GroupDTO usergroup(INV_Master_GroupDTO data)
        {
            try
            {
                data.get_usergroup = (from a in _INVContext.INV_Master_GroupDMO
                                      from b in _INVContext.INV_Master_GroupDMO
                                      where a.INVMG_ParentId == b.INVMG_Id && a.INVMG_MGUGIGFlg == "UG" && a.MI_Id == data.MI_Id
                                      select new INV_Master_GroupDTO
                                      {
                                          INVMG_Id = a.INVMG_Id,
                                          INVMG_GroupName= a.INVMG_GroupName,
                                          INVMG_AliasName= a.INVMG_AliasName,
                                          INVMG_ActiveFlg= a.INVMG_ActiveFlg,
                                          INVMG_GroupName_mn= b.INVMG_GroupName
                                      }).ToArray();
                                     
                                      
                    
                    //_INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "UG" && m.INVMG_ParentId == data.INVMG_ParentId).OrderBy(m => m.INVMG_Id).ToArray();

                data.maingrp = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "MG" && m.INVMG_Id == data.INVMG_Id).OrderBy(m => m.INVMG_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product Uergroup :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
        public INV_Master_GroupDTO Itemgroup(INV_Master_GroupDTO data)
        {
            try
            {
                data.get_itemgroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "IG" && m.INVMG_ParentId == data.INVMG_ParentId).OrderBy(m => m.INVMG_Id).ToArray();
                data.usergrp = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_MGUGIGFlg == "UG" && m.INVMG_Id == data.INVMG_Id).OrderBy(m => m.INVMG_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product ItemGroup :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
    }
}
