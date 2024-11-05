using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportServiceHub.Interfaces;


namespace TransportServiceHub.Services
{
    public class DriverEmployeeMappingImpl : Interfaces.DriverEmployeeMappingInterface
    {
        public TransportContext _context;
        public ILogger<DriverEmployeeMappingDTO> _log;

        public DriverEmployeeMappingImpl(ILogger<DriverEmployeeMappingDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }



        public DriverEmployeeMappingDTO getdata(int id)
        {
            DriverEmployeeMappingDTO data = new DriverEmployeeMappingDTO();
            data.MI_Id = id;
            try
            {

                data.employeedata = (from b in _context.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new DriverEmployeeMappingDTO
                                     {
                                         HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) +
                                      (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                      (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                         HRME_Id = b.HRME_Id
                                     }).ToArray();

                data.driverdata = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg == true).ToArray();
                data.savedata = (from a in _context.Driver_Employee
                                 from b in _context.MasterEmployee
                                 from c in _context.MasterDriverDMO
                                 where (a.HRME_Id == b.HRME_Id && a.TRMD_Id == c.TRMD_Id && b.MI_Id == data.MI_Id)
                                 select new DriverEmployeeMappingDTO
                                 {
                                     HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) +
                                     (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                     (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                     TRMD_DriverName = c.TRMD_DriverName,
                                     TRDE_Id = a.TRDE_Id,

                                 }).ToArray();



            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }

        public DriverEmployeeMappingDTO savedata(DriverEmployeeMappingDTO data)
        {
            try
            {
                if (data.TRDE_Id > 0)
                {
                    var check_duplicate_empname_update = _context.Driver_Employee.Where(a => a.HRME_Id.Equals(data.HRME_Id) && a.TRDE_Id != data.TRDE_Id).ToList();
                    var check_duplicate_driver_update = _context.Driver_Employee.Where(b => b.TRMD_Id.Equals(data.TRMD_Id) && b.TRDE_Id != data.TRDE_Id).ToList();

                    if (check_duplicate_empname_update.Count == 0 && check_duplicate_driver_update.Count == 0)
                    {
                        var result = _context.Driver_Employee.Single(a => a.TRDE_Id == data.TRDE_Id);
                        result.HRME_Id = data.HRME_Id;
                        result.TRMD_Id = data.TRMD_Id;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
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
                    var check_duplicate_empname = _context.Driver_Employee.Where(a => a.HRME_Id.Equals(data.HRME_Id)).ToList();
                    var check_duplicate_driver = _context.Driver_Employee.Where(b => b.TRMD_Id.Equals(data.TRMD_Id)).ToList();

                    if (check_duplicate_empname.Count == 0 && check_duplicate_driver.Count == 0)
                    {
                        Driver_Employee empdmo = new Driver_Employee();

                        empdmo.HRME_Id = data.HRME_Id;
                        empdmo.TRMD_Id = data.TRMD_Id;
                        empdmo.CreatedDate = DateTime.Now;
                        empdmo.UpdatedDate = DateTime.Now;
                        _context.Add(empdmo);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.retrunval = false;
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
                _log.LogInformation("Transport Error Master Vehicle savedata" + ex.Message);
            }
            return data;
        }

        public DriverEmployeeMappingDTO edit(DriverEmployeeMappingDTO data)
        {
            try
            {
                data.edit = _context.Driver_Employee.Where(a => a.TRDE_Id == data.TRDE_Id).ToArray();


            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle geteditdata " + ex.Message);
            }
            return data;
        }
        public DriverEmployeeMappingDTO deletedata(DriverEmployeeMappingDTO data)
        {
            try
            {
               var deldata = _context.Driver_Employee.Where(a => a.TRDE_Id == data.TRDE_Id).ToList();
                if (deldata.Count>0)
                {
                    foreach (var item in deldata)
                    {
                        _context.Remove(item);
                    }
                }
                int s = _context.SaveChanges();
                if (s>0)
                {
                    data.retrunval = true;
                }
                else
                {
                    data.retrunval = false;
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Vehicle geteditdata " + ex.Message);
            }
            return data;
        }

    }
}


