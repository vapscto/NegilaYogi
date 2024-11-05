using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Implementation
{
    public class INV_ConfigurationImpl : Interface.INV_ConfigurationInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_ConfigurationImpl> _logInv;
        public INV_ConfigurationImpl(InventoryContext InvContext, ILogger<INV_ConfigurationImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_ConfigurationDTO> getloaddata(INV_ConfigurationDTO data)
        {
            try
            {
                data.get_store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                //**************************************** Configuration Details ************************************//
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_ConfigurationDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_configdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory Configuration load Page:" + ex.Message);
            }
            return data;
        }      
        public INV_ConfigurationDTO savedetails(INV_ConfigurationDTO data)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Inventory Configuration savedata :" + ex.Message);
            }
            return data;
        }

    }
}
