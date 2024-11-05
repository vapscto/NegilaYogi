using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagTransferImpl : Interface.AssetTagTransferInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagTransferImpl> _logAT;
        public AssetTagTransferImpl(AssetTrackingContext ATContext, ILogger<AssetTagTransferImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public async Task<AssetTagTransferDTO> getloaddata(AssetTagTransferDTO data)
        {
            try
            {
                data.get_fromlocations = (from a in _ATContext.INV_Master_LocationDMO
                                          from b in _ATContext.INV_AssetTag_CheckOutDMO
                                          from c in _ATContext.INV_Asset_AssetTagDMO
                                          where (a.INVMLO_Id == b.INVMLO_Id && b.INVAAT_Id == c.INVAAT_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && c.INVAAT_DisposedFlg == false)
                                          select new AssetTagTransferDTO
                                          {
                                              INVMLOFrom_Id = a.INVMLO_Id,
                                              from_Location = a.INVMLO_LocationRoomName,
                                          }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new AssetTagTransferDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTag_TransferDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_ATTransfer = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Check Out load Page:" + ex.Message);
            }
            return data;
        }
        public AssetTagTransferDTO getitems(AssetTagTransferDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                  from b in _ATContext.INV_Master_ItemDMO
                                  where (a.INVMI_Id == b.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVATCO_ActiveFlg == true && a.INVMLO_Id == data.INVMLO_Id)
                                  select new CheckOutAssetsDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = b.INVMI_ItemName,
                                  }).Distinct().OrderByDescending(m => m.INVMI_ItemName).ToArray();

                data.get_tolocations = (from a in _ATContext.INV_Master_LocationDMO
                                        from b in _ATContext.INV_AssetTag_CheckOutDMO
                                        from c in _ATContext.INV_Asset_AssetTagDMO
                                        where (a.INVMLO_Id == b.INVMLO_Id && b.INVAAT_Id == c.INVAAT_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && c.INVAAT_DisposedFlg == false && b.INVMLO_Id != data.INVMLO_Id)
                                        select new AssetTagTransferDTO
                                        {
                                            INVMLOTo_Id = a.INVMLO_Id,
                                            to_Location = a.INVMLO_LocationRoomName,
                                        }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Transfer  get items:" + ex.Message);
            }
            return data;
        }
        public AssetTagTransferDTO gettolocation(AssetTagTransferDTO data)
        {
            try
            {
                //data.get_tolocations = (from a in _ATContext.INV_Master_LocationDMO
                //                        from b in _ATContext.INV_AssetTag_CheckOutDMO
                //                        from c in _ATContext.INV_Asset_AssetTagDMO
                //                        where (a.INVMLO_Id == b.INVMLO_Id && b.INVAAT_Id == c.INVAAT_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && c.INVAAT_DisposedFlg == false && b.INVMLO_Id != data.INVMLO_Id)
                //                        select new AssetTagTransferDTO
                //                        {
                //                            INVMLOTo_Id = a.INVMLO_Id,
                //                            to_Location = a.INVMLO_LocationRoomName,
                //                        }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Transfer get items:" + ex.Message);
            }
            return data;
        }

        public async Task<AssetTagTransferDTO> getitemtagdata(AssetTagTransferDTO data)
        {
            try
            {
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagTransferItemData";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMLO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_itemtagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.get_itemtagdata = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                //                        from b in _ATContext.INV_Asset_AssetTagDMO
                //                        from d in _ATContext.INV_Master_ItemDMO
                //                        from e in _ATContext.INV_Master_LocationDMO
                //                        where (a.INVAAT_Id == b.INVAAT_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id && a.INVATCO_CheckInFlg == false && a.MI_Id == b.MI_Id && a.INVATCO_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMI_Id == data.INVMI_Id)
                //                        select new AssetTagDisposeDTO
                //                        {
                //                            INVATCO_Id = a.INVATCO_Id,
                //                            INVAAT_Id = a.INVAAT_Id,
                //                            INVMI_Id = a.INVMI_Id,
                //                            INVMLO_Id = a.INVMLO_Id,
                //                            INVMLO_LocationRoomName = e.INVMLO_LocationRoomName,
                //                            INVMI_ItemName = d.INVMI_ItemName,
                //                            INVAAT_AssetId = b.INVAAT_AssetId,
                //                            INVAAT_AssetDescription = b.INVAAT_AssetDescription,
                //                            INVAAT_ModelNo = b.INVAAT_ModelNo,
                //                            INVAAT_SerialNo = b.INVAAT_SerialNo
                //                        }).Distinct().OrderByDescending(m => m.INVAAT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagTransferDTO savedata(AssetTagTransferDTO data)
        {
            try
            {

                foreach (var t in data.tagTransferArray)
                {
                    DateTime? troutdate = null;

                    if (data.INVATTR_CheckoutDate != null)
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        troutdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATTR_CheckoutDate), INDIAN_ZONE);
                    }

                    INV_AssetTag_TransferDMO ttag = new INV_AssetTag_TransferDMO();
                    ttag.MI_Id = data.MI_Id;
                    ttag.INVMLOFrom_Id = data.INVMLOFrom_Id;
                    ttag.INVMI_Id = data.INVMI_Id;
                    ttag.INVAAT_Id = t.INVAAT_Id;
                    ttag.INVMLOTo_Id = data.INVMLOTo_Id;
                    ttag.INVATTR_CheckoutDate = troutdate;
                    ttag.INVATTR_CheckOutQty = data.INVATTR_CheckOutQty;
                    ttag.INVATTR_ReceivedBy = t.INVATTR_ReceivedBy;
                    ttag.INVATTR_CheckOutRemarks = t.INVATTR_CheckOutRemarks;
                    ttag.INVATTR_ActiveFlg = true;
                    ttag.UpdatedDate = DateTime.Now;
                    ttag.CreatedDate = DateTime.Now;
                    _ATContext.Add(ttag);
                }

                var contextOB = _ATContext.SaveChanges();
                if (contextOB > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Asset tag Transfer savedata :" + ex.Message);
            }
            return data;
        }


        public AssetTagTransferDTO deactive(AssetTagTransferDTO data)
        {
            try
            {
                var result = _ATContext.INV_AssetTag_TransferDMO.Single(t => t.INVATTR_Id == data.INVATTR_Id);

                if (result.INVATTR_ActiveFlg == true)
                {
                    result.INVATTR_ActiveFlg = false;
                }
                else if (result.INVATTR_ActiveFlg == false)
                {
                    result.INVATTR_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
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

    }
}
