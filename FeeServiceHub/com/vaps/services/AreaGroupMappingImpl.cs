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
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using System.Dynamic;
using System.Data;
using System.Data.SqlClient;

namespace FeeServiceHub.com.vaps.services
{
    public class AreaGroupMappingImpl : interfaces.AreaGroupMappingInterface
    {
        private static ConcurrentDictionary<string, AreaGroupMappingDTO> _login =
        new ConcurrentDictionary<string, AreaGroupMappingDTO>();

        public FeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<AreaGroupMappingImpl> _logger;
        public AreaGroupMappingImpl(FeeGroupContext frgContext, ILogger<AreaGroupMappingImpl> log)
        {
            _logger = log;
            _FeeGroupHeadContext = frgContext;

        }

        public AreaGroupMappingDTO getdetails(int id)
        {
            AreaGroupMappingDTO FGRDT = new AreaGroupMappingDTO();

            try
            {


                var date = DateTime.Now;
                FGRDT.yearlist = _FeeGroupHeadContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id ==id).Distinct().ToArray();

                List<MasterAreaDMO> feearea = new List<MasterAreaDMO>();
                feearea = _FeeGroupHeadContext.MasterAreaDMO.Where(t => t.MI_Id == id && t.TRMA_ActiveFlg==true).ToList();
                FGRDT.fillarea = feearea.ToArray();

                List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();
                feegrp = _FeeGroupHeadContext.feeGroup.Where(t => t.MI_Id == id && t.FMG_ActiceFlag==true).ToList();
                FGRDT.fillgroup = feegrp.ToArray();

                FGRDT.fillareagroup = (from a in _FeeGroupHeadContext.areaGroupMappingDMO
                                       from b in _FeeGroupHeadContext.feeGroup
                                       from c in _FeeGroupHeadContext.MasterAreaDMO
                                       where (a.FMG_Id==b.FMG_Id && a.TRMA_Id==c.TRMA_Id && a.MI_Id==id)
                                      select new AreaGroupMappingDTO
                                      {
                                        FGAM_Id= a.FGAM_Id,
                                        FMG_Id=a.FMG_Id,
                                        TRMA_Id = a.TRMA_Id,
                                        FGAM_ActiveFlag = a.FGAM_ActiveFlag,
                                        FGAM_WayFlag = a.FGAM_WayFlag,
                                        FMG_GroupName =b.FMG_GroupName,
                                        TRMA_AreaName = c.TRMA_AreaName,

                                      }).Distinct().OrderByDescending(t=>t.FGAM_Id).ToArray();

                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Area_wise_amount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
        SqlDbType.VarChar)
                    {
                        Value = id

                    });
                    //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //SqlDbType.BigInt)
                    //        {
                    //            Value = data.ASMAY_Id
                    //        });


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
                        FGRDT.areadata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                

                //List<AreaGroupMappingDMO> feedata = new List<AreaGroupMappingDMO>();
                //feedata = _FeeGroupHeadContext.areaGroupMappingDMO.Where(t => t.MI_Id == id).ToList();
                //FGRDT.fillareagroup = feedata.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }

        public AreaGroupMappingDTO getpageedit(int id)
        {
            AreaGroupMappingDTO page = new AreaGroupMappingDTO();
            try
            {
                List<AreaGroupMappingDMO> lorg = new List<AreaGroupMappingDMO>();
                lorg = _FeeGroupHeadContext.areaGroupMappingDMO.AsNoTracking().Where(t => t.FGAM_Id.Equals(id)).ToList();
                page.fillareagroup = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public AreaGroupMappingDTO Savedetails(AreaGroupMappingDTO data)
        {
            bool returnresult = false;
            AreaGroupMappingDMO feepge = Mapper.Map<AreaGroupMappingDMO>(data);
            
            try
            {
                if (feepge.FGAM_Id > 0)
                {
                    var res = _FeeGroupHeadContext.areaGroupMappingDMO.Where(t => t.MI_Id == feepge.MI_Id && t.FMG_Id == feepge.FMG_Id && t.TRMA_Id == feepge.TRMA_Id && t.FGAM_ActiveFlag == feepge.FGAM_ActiveFlag).ToList();
                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result11 = _FeeGroupHeadContext.areaGroupMappingDMO.Single(t => t.MI_Id == feepge.MI_Id && t.FGAM_Id == feepge.FGAM_Id);
                        result11.FMG_Id = feepge.FMG_Id;
                        result11.TRMA_Id = feepge.TRMA_Id;
                        result11.FGAM_ActiveFlag = feepge.FGAM_ActiveFlag;
                        result11.FGAM_WayFlag = feepge.FGAM_WayFlag;
                        result11.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Update(result11);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnduplicatestatus = "Update";
                        }
                        else
                        {
                            data.returnduplicatestatus = "NotUpdate";
                        }
                    }
                }
                else
                {
                    string returntxt = "";
                    for (int i = 0; i < data.TempararyArrayList.Count; i++)
                    {
                        AreaGroupMappingDMO feepgeY = Mapper.Map<AreaGroupMappingDMO>(data.TempararyArrayList[i]);
                        var result = _FeeGroupHeadContext.areaGroupMappingDMO.Where(t => t.MI_Id == feepge.MI_Id && t.FMG_Id == feepge.FMG_Id && t.TRMA_Id == feepgeY.TRMA_Id && t.FGAM_WayFlag == data.FGAM_WayFlag).ToList();
                        if (result.Count() > 0)
                        {
                            returntxt = "Duplicate";
                        }
                        else
                        {
                            feepgeY.MI_Id = data.MI_Id;
                            feepgeY.FMG_Id = data.FMG_Id;
                            feepgeY.TRMA_Id = feepgeY.TRMA_Id;
                            feepgeY.FGAM_ActiveFlag = data.FGAM_ActiveFlag;
                            feepgeY.FGAM_WayFlag = data.FGAM_WayFlag;
                            feepgeY.CreatedDate = DateTime.Now;
                            feepgeY.UpdatedDate = DateTime.Now;
                            _FeeGroupHeadContext.Add(feepgeY);
                        }
                    }
                    if (returntxt == "Duplicate")
                    {
                        data.returnduplicatestatus = returntxt;
                    }
                    else
                    {
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnduplicatestatus = "Save";
                        }
                        else
                        {
                            data.returnduplicatestatus = "NotSave";
                        }
                    }
                }            
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
           
            return data;
        }
        public AreaGroupMappingDTO deactivate(AreaGroupMappingDTO data)
        {
            AreaGroupMappingDTO FGRDT = new AreaGroupMappingDTO();
            try
            {
                AreaGroupMappingDMO feepge = Mapper.Map<AreaGroupMappingDMO>(data);
                if (feepge.FGAM_Id > 0)
                {
                    var result = _FeeGroupHeadContext.areaGroupMappingDMO.Single(t => t.FGAM_Id == feepge.FGAM_Id);
                    result.UpdatedDate = DateTime.Now;

                    if (result.FGAM_ActiveFlag == true)
                    {
                        result.FGAM_ActiveFlag = false;
                    }
                    else
                    {
                        result.FGAM_ActiveFlag = true;
                    }
                    _FeeGroupHeadContext.Update(result);
                    var flag = _FeeGroupHeadContext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }

        public TR_Area_AmountDTO savedataamount(TR_Area_AmountDTO data)
        {
            try
            {
                if (data.TRMAAMT_Id > 0)
                {
                    //     var check_duplicate_areaname_update = _FeeGroupHeadContext.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id && a.TRLFM_Id != data.TRLFM_Id && a.TRLFM_WayFlag == data.TRLFM_WayFlag).ToList();

                    var check_duplicate_areaname_update = _FeeGroupHeadContext.TR_Area_AmountDMO.Where(a => a.TRMA_Id == data.TRMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.TRMAAMT_OneWayAmount == data.TRMAAMT_OneWayAmount && a.TRMAAMT_TwoWayAmount == data.TRMAAMT_TwoWayAmount && a.TRMAAMT_Id == data.TRMAAMT_Id).ToList();
         
                    if (check_duplicate_areaname_update.Count == 0)
                    {
                        var result = _FeeGroupHeadContext.TR_Area_AmountDMO.Single(a => a.TRMAAMT_Id == data.TRMAAMT_Id && a.ASMAY_Id == data.ASMAY_Id);
                        result.ASMAY_Id = data.ASMAY_Id;

                        result.TRMA_Id = data.TRMA_Id;
                        result.TRMAAMT_UpdatedDate = DateTime.Now;
                        result.TRMAAMT_OneWayAmount = data.TRMAAMT_OneWayAmount;
                        result.TRMAAMT_TwoWayAmount = data.TRMAAMT_TwoWayAmount;
                        result.TRMAAMT_UpdatedBy = data.user_id;
                        _FeeGroupHeadContext.Update(result);
                        int n = _FeeGroupHeadContext.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                 

                    var check_duplicate_areaname = _FeeGroupHeadContext.TR_Area_AmountDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.TRMAAMT_OneWayAmount == data.TRMAAMT_OneWayAmount && a.TRMA_Id == data.TRMA_Id && a.TRMAAMT_TwoWayAmount == data.TRMAAMT_TwoWayAmount).ToList();


                    if (check_duplicate_areaname.Count == 0)
                    {
                        TR_Area_AmountDMO areadmo = new TR_Area_AmountDMO();
                        areadmo.ASMAY_Id = data.ASMAY_Id;
                        areadmo.TRMAAMT_ActiveFlg = true;
                        areadmo.TRMA_Id = data.TRMA_Id;
                       

                        areadmo.TRMAAMT_OneWayAmount = data.TRMAAMT_OneWayAmount;
                        areadmo.TRMAAMT_TwoWayAmount = data.TRMAAMT_TwoWayAmount;
                        areadmo.TRMAAMT_CreatedDate = DateTime.Now;
                        areadmo.TRMAAMT_UpdatedDate = DateTime.Now;
                        areadmo.TRMAAMT_CreatedBy = data.user_id;
                        areadmo.TRMAAMT_UpdatedBy = data.user_id;
                        _FeeGroupHeadContext.Add(areadmo);
                        int n = _FeeGroupHeadContext.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrval = true;
                        }


                    }
                    else
                    {
                        data.message = "Duplicate";
                    }

                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Area_wise_amount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id

                        });
                        //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //SqlDbType.BigInt)
                        //        {
                        //            Value = data.ASMAY_Id
                        //        });


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
                            data.areadata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public TR_Area_AmountDTO geteditdataamount(TR_Area_AmountDTO data)
        {
            try
            {
             
                data.editdatadetails = _FeeGroupHeadContext.TR_Area_AmountDMO.Where(a => a.TRMAAMT_Id == data.TRMAAMT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public TR_Area_AmountDTO activedeactiveamount(TR_Area_AmountDTO data)
        {
            try
            {

                var result = _FeeGroupHeadContext.TR_Area_AmountDMO.Single(a => a.TRMAAMT_Id == data.TRMAAMT_Id);
                if (result.TRMAAMT_ActiveFlg == false)
                {
                    result.TRMAAMT_ActiveFlg = true;
                }
                else
                {
                    result.TRMAAMT_ActiveFlg = false;
                }
                result.TRMAAMT_UpdatedDate = DateTime.Now;
                _FeeGroupHeadContext.Update(result);
                int n = _FeeGroupHeadContext.SaveChanges();
                if (n > 0)
                {
                    data.retrval = true;
                }

            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _logger.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }

    }
}
