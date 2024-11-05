using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class TransferAssetsImpl : Interface.TransferAssetsInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<TransferAssetsImpl> _logAT;
        public TransferAssetsImpl(AssetTrackingContext ATContext, ILogger<TransferAssetsImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }
        public async Task<TransferAssetsDTO> getloaddata(TransferAssetsDTO data)
        {
            try
            {
                //    data.get_locations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_ActiveFlg == true).OrderBy(m => m.INVMLO_Id).ToArray();
                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_Asset_CheckOutDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_ActiveFlg == true)
                                      select new CheckInAssetsDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                      }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

                //data.get_transfer = (from AT1 in _ATContext.INV_Asset_TransferDMO
                //                     from ML1 in _ATContext.INV_Master_LocationDMO
                //                     from AT2 in _ATContext.INV_Asset_TransferDMO
                //                     from ML2 in _ATContext.INV_Master_LocationDMO
                //                     from IT in _ATContext.INV_Master_ItemDMO

                //                     where (AT1.INVMLOFrom_Id == ML1.INVMLO_Id && AT1.INVMI_Id == IT.INVMI_Id && AT2.MI_Id == AT1.MI_Id && AT2.INVMLOTo_Id == ML2.INVMLO_Id && AT1.MI_Id == data.MI_Id)
                //                     select new TransferAssetsDTO
                //                     {
                //                         INVATR_Id = AT1.INVATR_Id,
                //                         INVMLO_Id = ML1.INVMLO_Id,
                //                         INVMLOFrom_Id = AT1.INVMLOFrom_Id,
                //                         INVMI_Id = AT1.INVMI_Id,
                //                         INVMLOTo_Id = AT2.INVMLOTo_Id,
                //                         FINVMLO_LocationRoomName = ML1.INVMLO_LocationRoomName,
                //                         INVMI_ItemName = IT.INVMI_ItemName,
                //                         TINVMLO_LocationRoomName = ML2.INVMLO_LocationRoomName,
                //                         INVATR_CheckoutDate = AT1.INVATR_CheckoutDate,
                //                         INVATR_CheckOutQty = AT1.INVATR_CheckOutQty,
                //                         INVATR_CheckOutRemarks = AT1.INVATR_CheckOutRemarks,
                //                         INVATR_ActiveFlg = AT1.INVATR_ActiveFlg

                //                     }).Distinct().OrderBy(m => m.INVATR_Id).ToArray();

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AT_AssetsTransferDetails";
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
                        data.get_transfer = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new AT_MasterLocationDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Transfer load Page:" + ex.Message);
            }
            return data;
        }
        public TransferAssetsDTO gettolocations(TransferAssetsDTO data)
        {
            try
            {             
                data.get_items = (from a in _ATContext.INV_Asset_CheckOutDMO
                                  from b in _ATContext.INV_Master_LocationDMO
                                  from c in _ATContext.INV_Master_StoreDMO
                                  from d in _ATContext.INV_Master_ItemDMO
                                  from e in _ATContext.INV_StockDMO
                                  where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && e.INVMST_Id == c.INVMST_Id && e.INVMI_Id == d.INVMI_Id
                                  && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                  select new CheckInAssetsDTO
                                  {
                                      //  INVACO_Id = a.INVACO_Id,
                                      INVMLO_Id = a.INVMLO_Id,
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      //  INVSTO_PurchaseDate = e.INVSTO_PurchaseDate,
                                      INVSTO_SalesRate = e.INVSTO_SalesRate,
                                      INVSTO_AvaiableStock = e.INVSTO_AvaiableStock,
                                      // INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                      INVMI_ItemName = d.INVMI_ItemName
                                  }).Distinct().OrderBy(m => m.INVMI_ItemName).ToArray();

                data.get_tolocations = (from a in _ATContext.INV_Master_LocationDMO
                                        from b in _ATContext.INV_Asset_CheckOutDMO
                                        where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_ActiveFlg == true && a.INVMLO_Id != data.INVMLO_Id)
                                        select new CheckInAssetsDTO
                                        {
                                            INVMLO_Id = a.INVMLO_Id,
                                            INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                        }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

                // data.get_tolocations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_Id != data.INVMLO_Id && m.INVMLO_ActiveFlg == true).OrderBy(m => m.INVMLO_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets transfer get Tolocation:" + ex.Message);
            }
            return data;
        }
        public TransferAssetsDTO getitemdetails(TransferAssetsDTO data)
        {
            try
            {
                //      data.get_itemdetails = _ATContext.INV_Asset_CheckOutDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_Id == data.INVMLO_Id && m.INVMI_Id == data.INVMI_Id && m.INVACO_ActiveFlg == true).OrderBy(m => m.INVACO_Id).ToArray();
                data.get_itemdetails = (from a in _ATContext.INV_Asset_CheckOutDMO
                                        from b in _ATContext.INV_Master_LocationDMO
                                        from c in _ATContext.INV_Master_StoreDMO
                                        from d in _ATContext.INV_Master_ItemDMO
                                        from e in _ATContext.INV_StockDMO
                                        where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == b.MI_Id
                                        && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                        select new CheckInAssetsDTO
                                        {
                                            INVACO_Id = a.INVACO_Id,
                                            INVMLO_Id = a.INVMLO_Id,
                                            INVMST_Id = a.INVMST_Id,
                                            INVMI_Id = a.INVMI_Id,
                                            INVSTO_SalesRate = e.INVSTO_SalesRate,
                                            INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                            INVMI_ItemName = d.INVMI_ItemName
                                        }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets transfer item details:" + ex.Message);
            }
            return data;
        }
        public TransferAssetsDTO savedetails(TransferAssetsDTO data)
        {
            try
            {
                INV_Asset_TransferDMO transfer = new INV_Asset_TransferDMO();
                transfer.MI_Id = data.MI_Id;
                transfer.INVMLOFrom_Id = data.INVMLOFrom_Id;
                transfer.INVMI_Id = data.INVMI_Id;
                transfer.INVSTO_SalesRate = data.INVSTO_SalesRate;
                transfer.INVMLOTo_Id = data.INVMLOTo_Id;
                transfer.INVATR_CheckoutDate = data.INVATR_CheckoutDate;
                transfer.INVATR_CheckOutQty = data.INVATR_CheckOutQty;
                transfer.INVATR_ReceivedBy = data.INVATR_ReceivedBy;
                transfer.INVATR_CheckOutRemarks = data.INVATR_CheckOutRemarks;
                transfer.HRME_Id = data.HRME_Id;
                transfer.INVATR_ActiveFlg = true;
                transfer.CreatedDate = DateTime.Now;
                transfer.UpdatedDate = DateTime.Now;
                _ATContext.Add(transfer);

                var contactExists = _ATContext.SaveChanges();
                if (contactExists > 0)
                {
                    try
                    {
                        var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_TransferAssetsItem @p0, @p1,@p2, @p3,@p4,@p5,@p6", data.MI_Id, transfer.INVATR_Id, data.INVMLOFrom_Id, data.INVMLOTo_Id, data.INVMI_Id, data.INVSTO_SalesRate, data.INVATR_CheckOutQty);
                        if (contactExistsP > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.message = "Error";
                        _logAT.LogInformation("Transfer Assets Procedure :" + ex.Message);
                    }
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset Transfer savedata :" + ex.Message);
            }
            return data;
        }
        public TransferAssetsDTO deactive(TransferAssetsDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_TransferDMO.Single(t => t.INVATR_Id == data.INVATR_Id);

                if (result.INVATR_ActiveFlg == true)
                {
                    result.INVATR_ActiveFlg = false;
                }
                else if (result.INVATR_ActiveFlg == false)
                {
                    result.INVATR_ActiveFlg = true;
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
