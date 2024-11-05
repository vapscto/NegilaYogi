using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
    public class INV_ProductReportImpl : INV_ProductReportInterface
    {

        private static ConcurrentDictionary<string, INV_Master_ProductDTO> _login =
new ConcurrentDictionary<string, INV_Master_ProductDTO>();

        public InventoryContext _INVContext;
        ILogger<INV_ProductReportImpl> _logInv;
        public INV_ProductReportImpl(InventoryContext InvContext, ILogger<INV_ProductReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_ProductDTO getalldetails(INV_Master_ProductDTO data)
        {
            
            try
            {
                
                data.get_product = _INVContext.INV_Master_ProductDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMP_ActiveFlg == true).OrderBy(m => m.INVMP_Id).ToArray();

                
            }
            catch (Exception ee)
            {
                _logInv.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        

        public INV_Master_ProductDTO getdata(INV_Master_ProductDTO data)
        {
          
            try
            {
                data.get_productlist = _INVContext.INV_Master_Product_StagesDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPS_ActiveFlg == true && m.INVMP_Id==data.INVMP_Id).OrderBy(m => m.INVMPS_Id).ToArray();

                data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                                        from b in _INVContext.INV_Master_Product_ItemDMO
                                        where (a.INVMI_Id == b.INVMI_Id && a.MI_Id == data.MI_Id && b.INVMP_Id==data.INVMP_Id && b.INVMPI_ActiveFlg==true)
                                        select new INV_Master_ProductDTO
                                        {
                                            INVMI_Id = a.INVMI_Id,
                                            INVMP_Id=b.INVMP_Id,
                                            INVMI_ItemName = a.INVMI_ItemName,
                                           

                                        }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        

        public async Task<INV_Master_ProductDTO> radiobtndata(INV_Master_ProductDTO data)
        {
            
            for(int a=1;a<=2;a++)
            {
                
            using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Inv_Product_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000;


                cmd.Parameters.Add(new SqlParameter("@MI_Id",
           SqlDbType.VarChar)
                {
                    Value = data.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@INVMP_Id",
           SqlDbType.VarChar)
                {
                    Value = data.INVMP_Id
                });

                cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                {
                    Value = a
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

                    if(a==1)
                        {
                            data.get_productTax = retObject.ToArray();
                        }
                        else
                        {
                            data.gridproductTax = retObject.ToArray();
                        }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
                
            }
            
            return data;
        }


    }
}
