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
    public class MasterVehicleImpl : Interfaces.MasterVehicleInterface
    {
        public TransportContext _context;
        public ILogger<MasterVehicleDTO> _log;
        public MasterVehicleImpl(TransportContext context, ILogger<MasterVehicleDTO> log)
        {
            _context = context;
            _log = log;
        }
        public MasterVehicleDTO getdata(int id)
        {
            MasterVehicleDTO data = new MasterVehicleDTO();
            try
            {
                data.getloaddata = _context.Master_VehicleDMO.Where(a => a.MI_Id == id).OrderByDescending(a => a.TRMV_Id).ToArray();
                data.getfuletype = _context.MasterFuelDMO.Where(a => a.MI_Id == id && a.TRMFT_ActiveFlg == true).ToArray();
                data.getvehicletype = _context.MasterVehicleTypeDMO.Where(a => a.MI_Id == id && a.TRMVT_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle getdata" + ex.Message);
            }
            return data;
        }
        public MasterVehicleDTO savedata(MasterVehicleDTO data)
        {
            try
            {
                if (data.TRMV_Id > 0)
                {
                    var check_duplicate_areaname_update = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_VehicleName.Equals(data.TRMV_VehicleName) 
                    && a.TRMV_CompanyName.Equals(data.TRMV_CompanyName)
                    && a.TRMV_Id != data.TRMV_Id).ToList();
                    if (check_duplicate_areaname_update.Count == 0)
                    {

                        var result = _context.Master_VehicleDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id);
                        result.TRMV_VehicleName = data.TRMV_VehicleName;
                        result.TRMV_VehicleNo = data.TRMV_VehicleNo;
                        result.TRMVT_Id = data.TRMVT_Id;
                        result.TRMFT_Id = data.TRMFT_Id;
                        result.TRMV_PurchaseDate = data.TRMV_PurchaseDate;
                        result.TRMV_ChassisNo = data.TRMV_ChassisNo;
                        result.TRMV_EngineNo = data.TRMV_EngineNo;
                        result.TRMV_PurchasedFrom = data.TRMV_PurchasedFrom;
                        result.TRMV_Cost = data.TRMV_Cost;
                        result.TRMV_Capacity = data.TRMV_Capacity;
                        result.TRMV_Desc = data.TRMV_Desc;
                        result.TRMV_VehicleImage = data.TRMV_VehicleImage;                      
                        result.UpdatedDate = DateTime.Now;
                        result.TRMV_CompanyName = data.TRMV_CompanyName;
                        result.TRMV_SWDOff = data.TRMV_SWDOff;
                        result.TRMV_Address = data.TRMV_Address;
                        result.TRMV_Model = data.TRMV_Model;
                        result.TRMV_Fuel = data.TRMV_Fuel;
                        result.TRMV_Make = data.TRMV_Make;
                        result.TRMV_Manufacturer = data.TRMV_Manufacturer; 
                        result.TRMV_ManufacturedDate = data.TRMV_ManufacturedDate;
                        result.TRMV_Class = data.TRMV_Class;
                        result.TRMV_Color = data.TRMV_Color;
                        result.TRMV_Body = data.TRMV_Body;
                        result.TRMV_NoOfCylinder = data.TRMV_NoOfCylinder;
                        result.TRMV_WheelBase = data.TRMV_WheelBase;
                        result.TRMV_UnladenWeight = data.TRMV_UnladenWeight;
                        result.TRMV_Seating = data.TRMV_Seating;
                        result.TRMV_CC = data.TRMV_CC;
                        result.TRMV_RegFCUpToDate = data.TRMV_RegFCUpToDate;
                        result.TRMV_TaxUpTo = data.TRMV_TaxUpTo;
                        result.TRMV_OwnersName = data.TRMV_OwnersName;
                        result.TRMV_RegistrationDate = data.TRMV_RegistrationDate;
                    _context.Update(result);

                        var filelidt = _context.TR_Master_Vehicle_DocumentsDMO.Where(r => r.TRMV_Id == data.TRMV_Id).ToList();

                        if (filelidt.Count>0)
                        {
                            foreach (var item in filelidt)
                            {
                                _context.Remove(item);
                            }
                        }



                        if (data.filelist.Length > 0)
                        {
                            foreach (var item in data.filelist)
                            {

                                if (item.cfilepath != null && item.cfilepath != "")
                                {
                                    TR_Master_Vehicle_DocumentsDMO obb = new TR_Master_Vehicle_DocumentsDMO();

                                    obb.MI_Id = data.MI_Id;
                                    obb.TRMV_Id = data.TRMV_Id;
                                    obb.TRMVDO_DocumentFileName = item.cfilename;
                                    obb.TRMVDO_DocumentFilePath = item.cfilepath;
                                    obb.TRMVDO_DocumentFileDesc = item.cfiledesc;
                                    obb.TRMVDO_DocumentName = item.name;
                                    obb.TRMVDO_ActiveFlg = true;
                                    obb.TRMVDO_IsActiveFlg = true;
                                    obb.TRMVDO_CreatedBy = data.UserId;
                                    obb.TRMVDO_UpdatedBy = data.UserId;
                                    obb.CreatedDate = DateTime.Now;
                                    obb.UpdatedDate = DateTime.Now;
                                    _context.Add(obb);
                                }


                            }
                        }


                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    //var check_duplicate_areaname = _context.MasterAreaDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMA_AreaName.Equals(data.TRMV_VehicleName)).ToList();
                    
                    //if (check_duplicate_areaname.Count == 0)
                    //{
                        Master_VehicleDMO areadmo = new Master_VehicleDMO();
                        areadmo.TRMV_VehicleName = data.TRMV_VehicleName;
                        areadmo.TRMV_VehicleNo = data.TRMV_VehicleNo;
                        areadmo.TRMVT_Id = data.TRMVT_Id;
                        areadmo.TRMFT_Id = data.TRMFT_Id;
                        areadmo.MI_Id = data.MI_Id;
                        areadmo.TRMV_PurchaseDate = data.TRMV_PurchaseDate;
                        areadmo.TRMV_ChassisNo = data.TRMV_ChassisNo;
                        areadmo.TRMV_EngineNo = data.TRMV_EngineNo;
                        areadmo.TRMV_PurchasedFrom = data.TRMV_PurchasedFrom;
                        areadmo.TRMV_Cost = data.TRMV_Cost;
                        areadmo.TRMV_Capacity = data.TRMV_Capacity;
                        areadmo.TRMV_Desc = data.TRMV_Desc;
                        areadmo.TRMV_VehicleImage = data.TRMV_VehicleImage;
                        areadmo.TRMV_ActiveFlag = true;
                        areadmo.TRMV_CompanyName = data.TRMV_CompanyName;
                        areadmo.CreatedDate = DateTime.Now;
                        areadmo.UpdatedDate = DateTime.Now;
                    areadmo.TRMV_SWDOff = data.TRMV_SWDOff;
                    areadmo.TRMV_Address = data.TRMV_Address;
                    areadmo.TRMV_Model = data.TRMV_Model;
                    areadmo.TRMV_Fuel = data.TRMV_Fuel;
                    areadmo.TRMV_Make = data.TRMV_Make;
                    areadmo.TRMV_Manufacturer = data.TRMV_Manufacturer;
                    areadmo.TRMV_ManufacturedDate = data.TRMV_ManufacturedDate;
                    areadmo.TRMV_Class = data.TRMV_Class;
                    areadmo.TRMV_Color = data.TRMV_Color;
                    areadmo.TRMV_Body = data.TRMV_Body;
                    areadmo.TRMV_NoOfCylinder = data.TRMV_NoOfCylinder;
                    areadmo.TRMV_WheelBase = data.TRMV_WheelBase;
                    areadmo.TRMV_UnladenWeight = data.TRMV_UnladenWeight;
                    areadmo.TRMV_Seating = data.TRMV_Seating;
                    areadmo.TRMV_CC = data.TRMV_CC;
                    areadmo.TRMV_RegFCUpToDate = data.TRMV_RegFCUpToDate;
                    areadmo.TRMV_TaxUpTo = data.TRMV_TaxUpTo;
                    areadmo.TRMV_OwnersName = data.TRMV_OwnersName;
                    areadmo.TRMV_RegistrationDate = data.TRMV_RegistrationDate;
                    _context.Add(areadmo);

                    if (data.filelist.Length > 0)
                    {
                        foreach (var item in data.filelist)
                        {

                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                TR_Master_Vehicle_DocumentsDMO obb = new TR_Master_Vehicle_DocumentsDMO();

                                obb.MI_Id = data.MI_Id;
                                obb.TRMV_Id = areadmo.TRMV_Id;
                                obb.TRMVDO_DocumentFileName = item.cfilename;
                                obb.TRMVDO_DocumentFilePath = item.cfilepath;
                                obb.TRMVDO_DocumentFileDesc = item.cfiledesc;
                                obb.TRMVDO_DocumentName = item.name;
                                obb.TRMVDO_ActiveFlg = true;
                                obb.TRMVDO_IsActiveFlg = true;
                                obb.TRMVDO_CreatedBy = data.UserId;
                                obb.TRMVDO_UpdatedBy = data.UserId;
                                obb.CreatedDate = DateTime.Now;
                                obb.UpdatedDate = DateTime.Now;
                                _context.Add(obb);
                            }


                        }
                    }




                    int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }


                    //}
                    //else
                    //{
                    //    data.message = "Duplicate";
                    //}


                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle savedata" + ex.Message);
            }
            return data;
        }

        public MasterVehicleDTO edit(MasterVehicleDTO data)
        {
            try
            {
                data.geteditdata = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id).ToArray();


                data.editfiles = (from a in _context.TR_Master_Vehicle_DocumentsDMO

                                  where (a.TRMV_Id == data.TRMV_Id)
                                  select new MasterVehicledocDTO
                                  {
                                      cfilename = a.TRMVDO_DocumentFileName,
                                      cfilepath = a.TRMVDO_DocumentFilePath,
                                      cfiledesc = a.TRMVDO_DocumentFileDesc,
                                      name = a.TRMVDO_DocumentName,

                                  }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle geteditdata " + ex.Message);
            }
            return data;
        }

        public MasterVehicleDTO activedeactive(MasterVehicleDTO data)
        {
            try
            {
                var check_Area_Used = (from a in _context.VehicleDriver
                                       from b in _context.Master_VehicleDMO
                                       where (a.TRMV_Id == b.TRMV_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMV_ActiveFlag == true && a.TRMV_Id == data.TRMV_Id)
                                       select new MasterVehicleDTO
                                       {
                                           TRMV_Id = a.TRMV_Id
                                       }).ToList();

                var check_Area_Used1 = (from a in _context.VehicleRouteDMo
                                       from b in _context.Master_VehicleDMO
                                       where (a.TRMV_Id == b.TRMV_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMV_ActiveFlag == true && a.TRMV_Id== data.TRMV_Id)
                                       select new MasterVehicleDTO
                                       {
                                           TRMV_Id = a.TRMV_Id
                                       }).ToList();


                if (check_Area_Used.Count > 0 || check_Area_Used1.Count>0)
                {
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                }
                else
                {
                    var result = _context.Master_VehicleDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMV_Id == data.TRMV_Id);
                    if (result.TRMV_ActiveFlag == false)
                    {
                        result.TRMV_ActiveFlag = true;
                    }
                    else
                    {
                        result.TRMV_ActiveFlag = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrunval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _log.LogInformation("Transport Error Master Vehicle activedeactive" + ex.Message);
            }
            return data;
        }

        public MasterVehicleDTO validaevehicleno(MasterVehicleDTO data)
        {

            try
            {
                if (data.TRMV_Id > 0)
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_VehicleNo.Equals(data.TRMV_VehicleNo) && a.TRMV_Id != data.TRMV_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
                else
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_VehicleNo.Equals(data.TRMV_VehicleNo)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle validaevehicleno" + ex.Message);
            }
            return data;
        }
        public MasterVehicleDTO rcreport(MasterVehicleDTO data)
        {

            try
            {

                List<long> grpid = new List<long>();
                foreach (var item in data.vclids)
                {
                    grpid.Add(item.TRMV_Id);
                }

                string groupidss = "0";
             
               

                for (int r = 0; r < grpid.Count(); r++)
                {
                    groupidss = groupidss + ',' + grpid[r];
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_RC_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMV_Id",
                      SqlDbType.VarChar)
                    {
                        Value = groupidss
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
                        data.rcreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle validaevehicleno" + ex.Message);
            }
            return data;
        }

        public MasterVehicleDTO validaevhassiseno(MasterVehicleDTO data)
        {

            try
            {
                if (data.TRMV_Id > 0)
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_ChassisNo.Equals(data.TRMV_ChassisNo) && a.TRMV_Id != data.TRMV_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
                else
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_ChassisNo.Equals(data.TRMV_ChassisNo)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle validaevhassiseno" + ex.Message);
            }
            return data;
        }

        public MasterVehicleDTO validaeengineno(MasterVehicleDTO data)
        {
            try
            {
                if (data.TRMV_Id > 0)
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_EngineNo.Equals(data.TRMV_EngineNo) && a.TRMV_Id != data.TRMV_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
                else
                {
                    var checkduplicate = _context.Master_VehicleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMV_EngineNo.Equals(data.TRMV_EngineNo)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle validaeengineno" + ex.Message);
            }
            return data;
        }


        public MasterVehicleDTO viewuploadflies(MasterVehicleDTO data)
        {
            try
            {

                data.editfiles = (from a in _context.TR_Master_Vehicle_DocumentsDMO

                                  where (a.TRMV_Id == data.TRMV_Id)
                                  select new MasterVehicledocDTO
                                  {
                                      gridid = a.TRMV_Id,
                                      cfileid = a.TRMVDO_Id,
                                      cfilename = a.TRMVDO_DocumentFileName,
                                      cfilepath = a.TRMVDO_DocumentFilePath,
                                      cfiledesc = a.TRMVDO_DocumentFileDesc,
                                      name = a.TRMVDO_DocumentName,

                                  }).Distinct().ToArray();

            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public MasterVehicleDTO deleteuploadfile(MasterVehicleDTO data)
        {
            try
            {


                if (data.TRMVDO_Id > 0)
                {
                    var deletefile = _context.TR_Master_Vehicle_DocumentsDMO.Where(e => e.TRMVDO_Id == data.TRMVDO_Id).ToList();

                    if (deletefile.Count > 0)
                    {
                        foreach (var item in deletefile)
                        {
                            _context.Remove(item);
                        }


                        int y = _context.SaveChanges();
                        if (y > 0)
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
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }
    }
}
