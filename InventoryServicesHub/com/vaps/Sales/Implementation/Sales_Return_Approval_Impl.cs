using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class Sales_Return_Approval_Impl:Interface.Sales_Return_Approval_Interface
    {
        public InventoryContext _INVContext;
        public Sales_Return_Approval_Impl(InventoryContext INVContext)
        {
            _INVContext = INVContext;
        }
        public Sales_Return_Apply_DTO getloaddata(Sales_Return_Apply_DTO dto)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SalesReturn_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.UserId
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
                        dto.sales_m_return = retObject.ToArray();
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
            }
            return dto;
        }

        public async Task<Sales_Return_Apply_DTO> getpidetails(Sales_Return_Apply_DTO dto)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SalesReturnItem_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSLRET_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.INVMSLRET_Id
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
                        dto.sales_item_return = retObject.ToArray();
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
            }
            return dto;
        }

        public Sales_Return_Apply_DTO savedetails(Sales_Return_Apply_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                INV_M_Sales_Return_Apply_DMO re = new INV_M_Sales_Return_Apply_DMO();
                re.MI_Id = data.MI_Id;
                re.INVMST_Id = data.INVMST_Id;
                re.INVMSL_Id = data.INVMSL_Id;
                re.INVMSLRETAPP_SalesReturnNo = data.INVMSLRETAPP_SalesReturnNo;
                re.INVMSLRETAPP_SalesReturnDate = indianTime;
                re.INVMSLRETAPP_TotalReturnAmount = data.INVMSLRETAPP_TotalReturnAmount;
                re.INVMSLRETAPP_ReturnRemarks = data.INVMSLRETAPP_ReturnRemarks;
                re.INVMSLRETAPP_StatusFlg = "APPROVED";
                re.INVMSLRETAPP_ActiveFlg = true;
                re.INVMSLRETAPP_CreatedBy = data.UserId;
                re.INVMSLRETAPP_UpdatedBy = data.UserId;
                re.INVMSLRETAPP_CreatedDate = DateTime.Today;
                re.INVMSLRETAPP_UpdatedDate = DateTime.Today;
                _INVContext.Add(re);

                foreach (var item in data.arrayretsales) 
                {
                    INV_T_Sales_Return_Apply_DMO ra = new INV_T_Sales_Return_Apply_DMO();
                    ra.INVMSLRETAPP_Id = re.INVMSLRETAPP_Id;
                    ra.INVMI_Id = item.INVMI_Id;
                    ra.INVMUOM_Id = item.INVMUOM_Id;
                    ra.INVMP_Id = item.INVMP_Id;
                    ra.INVTSLRETAPP_SalesReturnQty = item.INVTPIAPP_ApprovedQty;
                    ra.INVTSLRETAPP_SalesReturnAmount = item.invtpI_ApproxAmount;
                    ra.INVTSLRETAPP_SalesReturnNaration = item.INVTGRNRETAPP_ReturnNaration;
                    ra.INVTSLRETAPP_ActiveFlg = true;
                    ra.INVTSLRETAPP_CreatedBy = data.UserId;
                    ra.INVTSLRETAPP_UpdatedBy = data.UserId;
                    ra.INVTSLRETAPP_ReturnDate = indianTime;
                    ra.INVTSLRETAPP_ApproveFlg = item.flag;
                    ra.INVTSLRETAPP_UpdatedDate = DateTime.Today;
                    ra.INVTSLRETAPP_UpdatedDate = DateTime.Today;
                    _INVContext.Add(ra);

                }
                var result = _INVContext.INV_M_Sales_Return_DMO_con.Single(a => a.INVMSLRET_Id == data.INVMSLRET_Id);
                result.INVMSLRETAPP_Id = re.INVMSLRETAPP_Id;
               // var INVMST_Id = result.INVMST_Id;
              //  var INVMSL_Id = result.INVMSL_Id;
                _INVContext.Update(result);
              
                var cnt = _INVContext.SaveChanges();
                if (cnt > 0)
                {
                    //added
                    foreach (var item in data.arrayretsales)
                    {


                        var result1 = _INVContext.INV_T_SalesDMO.Single(a => a.INVMI_Id == item.INVMI_Id && a.INVMSL_Id == data.INVMSL_Id ) ;
                        var INVMI_Id = result1.INVMI_Id;

                        var INVTSL_SalesPrice = result1.INVTSL_SalesPrice;

                       

                        var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_SalesReturnStockUpdate @p0, @p1,@p2,@p3,@p4",  data.MI_Id, data.INVMST_Id, data.INVMSL_Id, INVMI_Id, INVTSL_SalesPrice);

                    if (contactExistsP > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                    }
                    //
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
