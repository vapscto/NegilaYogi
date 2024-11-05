using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class MasterDriverImpl :Interfaces.MasterDriverInterface
    {
        public TransportContext _context;
        public ILogger<MasterDriverImpl> _log;

        public MasterDriverImpl(ILogger<MasterDriverImpl> log,TransportContext context)
        {
            _context = context;
            _log = log;
        }

        public MasterDriverDTO getdata(int id)
        {
            MasterDriverDTO data = new MasterDriverDTO();
            try
            {
                data.getdatamaster = _context.MasterDriverDMO.Where(a => a.MI_Id == id).OrderByDescending(a=>a.TRMD_Id).ToArray();
            }
            catch(Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver getdata" + ex.Message);
            }
            return data;
        }
        //---Checking Driver Code number duplicate--//
        public MasterDriverDTO checkdrivercode(MasterDriverDTO data)
        {
            try
            {
                if (data.TRMD_Id > 0)
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DriverCode.Equals(data.TRMD_DriverCode) && a.TRMD_Id != data.TRMD_Id).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
                else
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DriverCode.Equals(data.TRMD_DriverCode)).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
            }
            catch(Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver checkdrivercode" + ex.Message);
            }
            return data;
        }

        //---Checking DL number duplicate--//
        public MasterDriverDTO checkdriverdl(MasterDriverDTO data)
        {
            try
            {
                if (data.TRMD_Id > 0)
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DLNo.Equals(data.TRMD_DLNo) && a.TRMD_Id != data.TRMD_Id).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
                else
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DLNo.Equals(data.TRMD_DLNo)).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver checkdriverdl" + ex.Message);
            }
            return data;
        }

        //---Checking Driver Badge number duplicate--//
        public MasterDriverDTO checkdriverbno(MasterDriverDTO data)
        {
            try
            {
                if (data.TRMD_Id > 0)
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DriverBadgeNo.Equals(data.TRMD_DriverBadgeNo) && a.TRMD_Id != data.TRMD_Id).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
                else
                {
                    var check_duplicate = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_DriverBadgeNo.Equals(data.TRMD_DriverBadgeNo)).ToList();
                    if (check_duplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "";
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver checkdriverbno" + ex.Message);
            }
            return data;
        }

        //--Save Data--//
        public MasterDriverDTO savedata(MasterDriverDTO data)
        {
            try
            {
                if (data.TRMD_Id > 0)
                {
                    var result = _context.MasterDriverDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMD_Id == data.TRMD_Id);
                    result.TRMD_DriverName = data.TRMD_DriverName;
                    result.TRMD_DriverCode = data.TRMD_DriverCode;
                    result.TRMD_DLNo = data.TRMD_DLNo.ToUpper(); 
                    result.TRMD_RTOName = data.TRMD_RTOName;
                    result.TRMD_DLExpiryDate = data.TRMD_DLExpiryDate;
                    result.TRMD_MTExpiryDate = data.TRMD_MTExpiryDate;
                    result.TRMD_SDExpiryDate = data.TRMD_SDExpiryDate;
                    result.TRMD_DriverBadgeNo = data.TRMD_DriverBadgeNo;
                    result.TRMD_LicenseType = data.TRMD_LicenseType.ToUpper(); 
                    result.TRMD_SpareDriverFlg = data.TRMD_SpareDriverFlg;
                    result.TRMD_MobileNo = data.TRMD_MobileNo;
                    result.TRMD_EmailId = data.TRMD_EmailId;

                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Update";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Update";
                        data.returnval = false;
                    }
                }
                else
                {
                    MasterDriverDMO driver = new MasterDriverDMO();

                    driver.MI_Id = data.MI_Id;
                    driver.TRMD_DriverName = data.TRMD_DriverName;
                    driver.TRMD_DriverCode = data.TRMD_DriverCode;
                    driver.TRMD_DLNo = data.TRMD_DLNo.ToUpper();
                    driver.TRMD_RTOName = data.TRMD_RTOName;
                    driver.TRMD_DLExpiryDate = data.TRMD_DLExpiryDate;
                    driver.TRMD_MTExpiryDate = data.TRMD_MTExpiryDate;
                    driver.TRMD_SDExpiryDate = data.TRMD_SDExpiryDate;
                    driver.TRMD_DriverBadgeNo = data.TRMD_DriverBadgeNo;
                    driver.TRMD_LicenseType = data.TRMD_LicenseType.ToUpper();
                    driver.TRMD_SpareDriverFlg = data.TRMD_SpareDriverFlg;
                    driver.TRMD_MobileNo = data.TRMD_MobileNo;
                    driver.TRMD_EmailId = data.TRMD_EmailId;
                    driver.TRMD_ActiveFlg = true;
                    driver.CreatedDate = DateTime.Now;
                    driver.UpdatedDate = DateTime.Now;
                    _context.Add(driver);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch(Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver savedata" + ex.Message);
            }
            return data;
        }

        //--Edit data--//
        public MasterDriverDTO editdata(MasterDriverDTO data)
        {
            try
            {
                data.getdatamasteredit = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_Id==data.TRMD_Id).OrderByDescending(a => a.TRMD_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver editdata" + ex.Message);
            }
            return data;
        }

        //--Active Deactive--//
        public MasterDriverDTO activedeactive(MasterDriverDTO data)
        {
            try
            {
                var check_driverid_used = (from a in _context.MasterDriverDMO
                                           from b in _context.Driver_Employee
                                           where (a.TRMD_Id == b.TRMD_Id && a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg == true && a.TRMD_Id== data.TRMD_Id)
                                           select new MasterDriverDTO
                                           {
                                               TRMD_Id = a.TRMD_Id
                                           }).ToList();
                var check_driverid_used1 = (from a in _context.MasterDriverDMO
                                            from b in _context.VehicleDriver
                                            where (a.TRMD_Id == b.TRMD_Id && a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg == true && a.TRMD_Id == data.TRMD_Id)
                                            select new MasterDriverDTO
                                            {
                                                TRMD_Id = a.TRMD_Id
                                            }).ToList();


                if(check_driverid_used.Count==0 && check_driverid_used1.Count == 0)
                {
                    var result = _context.MasterDriverDMO.Single(a => a.MI_Id == data.MI_Id && a.TRMD_Id == data.TRMD_Id);
                    if (result.TRMD_ActiveFlg == true)
                    {
                        result.TRMD_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRMD_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    data.message = "You Can Not Deactive This Record It Is Already Mapped";
                }                
            }
            catch(Exception ex)
            {
                data.message = "You Can Not Deactive This Record It Is Already Mapped";
                _log.LogInformation("Transport Error Master Driver activedeactive" + ex.Message);
            }
            return data;
        }
    }
}
