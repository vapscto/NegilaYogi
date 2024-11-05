using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class LocationFeeGroupMappingImpl : Interfaces.LocationFeeGroupMappingInterface
    {
        public TransportContext _context;
        ILogger<LocationFeeGroupMappingImpl> _areaimpl;

        public LocationFeeGroupMappingImpl(ILogger<LocationFeeGroupMappingImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {
            TR_Location_FeeGroup_MappingDTO data = new TR_Location_FeeGroup_MappingDTO();
            try
            {
                data.MI_Id = id;



                var date = DateTime.Now;
                data.yearlist = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id ).Distinct().ToArray();
                data.locationlist = _context.MasterLocationDMO.Where(a => a.MI_Id == id && a.TRML_ActiveFlg == true).Distinct().Distinct().ToArray();

                var year = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id && y.ASMAY_From_Date <= date && y.ASMAY_To_Date >=date).Distinct().ToList();
                if (year.Count >0)
                {
                    data.ASMAY_Id = year[0].ASMAY_Id;
                }
               



                data.grouplist = _context.FeeGroupDMO.Where(w => w.MI_Id == data.MI_Id && w.FMG_ActiceFlag == true && w.FMG_TransportFlg==true).Distinct().ToArray();

                data.griddata = (from a in _context.TR_Location_FeeGroup_MappingDMO
                                 from b in _context.AcademicYear
                                 from c in _context.FeeGroupDMO
                                 from d in _context.MasterLocationDMO
                                 where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && a.TRML_Id == d.TRML_Id && d.TRML_ActiveFlg == true)
                                 select new TR_Location_FeeGroup_MappingDTO
                                 {
                                     TRLFM_Id = a.TRLFM_Id,
                                     FMG_Id=c.FMG_Id,
                                     FMG_GroupName=c.FMG_GroupName,
                                     TRML_Id=d.TRML_Id,
                                     TRML_LocationName=d.TRML_LocationName,
                                     ASMAY_Id=b.ASMAY_Id,
                                     ASMAY_Year=b.ASMAY_Year,
                                     TRLFM_ActiveFlag =a.TRLFM_ActiveFlag,
                                     TRLFM_WayFlag=a.TRLFM_WayFlag,
                                 }).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "location_wise_amount";
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
                        data.studentdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //  data.getmasterarea = _context.MasterAreaDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMA_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public TR_Location_FeeGroup_MappingDTO savedata(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                if (data.TRLFM_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id && a.TRLFM_Id != data.TRLFM_Id && a.TRLFM_WayFlag == data.TRLFM_WayFlag).ToList();
                   

                    if (check_duplicate_areaname_update.Count == 0)
                    {
                        var result = _context.TR_Location_FeeGroup_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id );
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.FMG_Id = data.FMG_Id;
                        result.TRML_Id = data.TRML_Id;
                        result.UpdatedDate = DateTime.Now;
                        result.TRLFM_WayFlag = data.TRLFM_WayFlag;
                        _context.Update(result);
                        int n = _context.SaveChanges();
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
                    var check_duplicate_areaname = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id ).ToList();
                   

                    if (check_duplicate_areaname.Count == 0)
                    {
                        TR_Location_FeeGroup_MappingDMO areadmo = new TR_Location_FeeGroup_MappingDMO();
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.ASMAY_Id = data.ASMAY_Id;
                        areadmo.TRML_Id = data.TRML_Id;
                        areadmo.FMG_Id = data.FMG_Id;
                        areadmo.TRLFM_ActiveFlag = true;
                        areadmo.TRLFM_WayFlag = data.TRLFM_WayFlag;
                        areadmo.CreatedDate = DateTime.Now;
                        areadmo.UpdatedDate = DateTime.Now;

                        _context.Add(areadmo);
                        int n = _context.SaveChanges();
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


                }
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public TR_Location_FeeGroup_MappingDTO geteditdata(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                data.editdatadetails = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public TR_Location_FeeGroup_MappingDTO activedeactive(TR_Location_FeeGroup_MappingDTO data)
        {
            try
            {
                //var check_Area_Used = (from a in _context.MasterRouteDMO
                //                       from b in _context.MasterAreaDMO
                //                       where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMA_ActiveFlg == true && a.TRMA_Id == data.TRMA_Id && a.TRMR_ActiveFlg == true)
                //                       select new TR_Location_FeeGroup_MappingDTO
                //                       {
                //                           TRMA_Id = a.TRMA_Id
                //                       }).ToList();


            //    if (check_Area_Used.Count > 0)
             ///   {
                  //  data.message = "You Can Not Deactivate This Record Its Already Mapped";
              //  }
              //  else
              //  {
                    var result = _context.TR_Location_FeeGroup_MappingDMO.Single(a => a.MI_Id == data.MI_Id && a.TRLFM_Id == data.TRLFM_Id);
                    if (result.TRLFM_ActiveFlag == false)
                    {
                        result.TRLFM_ActiveFlag = true;
                    }
                    else
                    {
                        result.TRLFM_ActiveFlag = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrval = true;
                    }
                //}
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }

        public TR_Location_AmountDTO savedataamount(TR_Location_AmountDTO data)
        {
            try
            {
                if (data.TRMLAMT_Id > 0)
                {
                    //     var check_duplicate_areaname_update = _context.TR_Location_FeeGroup_MappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id && a.TRML_Id == data.TRML_Id && a.TRLFM_Id != data.TRLFM_Id && a.TRLFM_WayFlag == data.TRLFM_WayFlag).ToList();

                    var check_duplicate_areaname_update = _context.TR_Location_AmountDMO.Where(a => a.TRML_Id == data.TRML_Id && a.ASMAY_Id == data.ASMAY_Id && a.TRMLAMT_OneWayAmount == data.TRMLAMT_OneWayAmount && a.TRMLAMT_TwoWayAmount == data.TRMLAMT_TwoWayAmount && a.TRMLAMT_Id== data.TRMLAMT_Id).ToList();


                    if (check_duplicate_areaname_update.Count == 0)
                    {
                                      var result = _context.TR_Location_AmountDMO.Single(a => a.TRMLAMT_Id == data.TRMLAMT_Id && a.ASMAY_Id == data.ASMAY_Id);
                        result.ASMAY_Id = data.ASMAY_Id;
                     
                        result.TRML_Id = data.TRML_Id;
                        result.TRMLAMT_UpdatedDate = DateTime.Now;
                        result.TRMLAMT_OneWayAmount = data.TRMLAMT_OneWayAmount;
                        result.TRMLAMT_TwoWayAmount = data.TRMLAMT_TwoWayAmount;
                        result.TRMLAMT_UpdatedBy = data.user_id;
                        _context.Update(result);
                        int n = _context.SaveChanges();
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
                   

          var check_duplicate_areaname = _context.TR_Location_AmountDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.TRMLAMT_OneWayAmount == data.TRMLAMT_OneWayAmount && a.TRML_Id == data.TRML_Id && a.TRMLAMT_TwoWayAmount==data.TRMLAMT_TwoWayAmount).ToList();


                    if (check_duplicate_areaname.Count == 0)
                    {
                        TR_Location_AmountDMO areadmo = new TR_Location_AmountDMO();
                        areadmo.ASMAY_Id = data.ASMAY_Id;
                        areadmo.TRMLAMT_ActiveFlg = true;
                        areadmo.TRML_Id = data.TRML_Id;
                        areadmo.TRMLAMT_OneWayAmount = data.TRMLAMT_OneWayAmount;
                       
                        areadmo.TRMLAMT_TwoWayAmount = data.TRMLAMT_TwoWayAmount;
                        areadmo.TRMLAMT_CreatedDate = DateTime.Now;
                        areadmo.TRMLAMT_UpdatedDate = DateTime.Now;
                        areadmo.TRMLAMT_CreatedBy = data.user_id;
                        areadmo.TRMLAMT_UpdatedBy = data.user_id;
                        _context.Add(areadmo);
                        int n = _context.SaveChanges();
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


                }
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public TR_Location_AmountDTO geteditdataamount(TR_Location_AmountDTO data)
        {
            try
            {
           
                data.editdatadetails = _context.TR_Location_AmountDMO.Where(a => a.TRMLAMT_Id == data.TRMLAMT_Id ).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public TR_Location_AmountDTO activedeactiveamount(TR_Location_AmountDTO data)
        {
            try
            {
                
                var result = _context.TR_Location_AmountDMO.Single(a => a.TRMLAMT_Id == data.TRMLAMT_Id );
                if (result.TRMLAMT_ActiveFlg == false)
                {
                    result.TRMLAMT_ActiveFlg = true;
                }
                else
                {
                    result.TRMLAMT_ActiveFlg = false;
                }
                result.TRMLAMT_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int n = _context.SaveChanges();
                if (n > 0)
                {
                    data.retrval = true;
                }
               
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }

    }
}
