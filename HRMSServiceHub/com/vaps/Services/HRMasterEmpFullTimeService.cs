using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class HRMasterEmpFullTimeService : Interfaces.HRMasterEmpFullTimeInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRMasterEmpFullTimeService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }

        public NAACHRMasterEmpFullTimeDTO getalldetails(NAACHRMasterEmpFullTimeDTO dto)
        {
            dto.employeelist = (from a in _HRMSContext.MasterEmployee
                                where (a.HRME_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_LeftFlag == false)
                                select new NAACHRMasterEmpFullTimeDTO
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName.Trim() == "" ? "" : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName.Trim() == "" ? "" : a.HRME_EmployeeLastName)
                                }).ToArray();
            dto.academicyearlist = (from a in _HRMSContext.AcademicYear
                                    where (a.ASMAY_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == dto.MI_Id)
                                    select new NAACHRMasterEmpFullTimeDTO
                                    {
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).ToArray();

            dto.empdata = (from a in _HRMSContext.Institution
                           from b in _HRMSContext.NAACHRMasterEmpFullTimeDMO
                           from c in _HRMSContext.MasterEmployee
                           from d in _HRMSContext.AcademicYear
                           where (a.MI_Id == dto.MI_Id && b.HRME_Id == c.HRME_Id && c.HRME_ActiveFlag == true && a.MI_Id == c.MI_Id && c.HRME_LeftFlag == false && b.HRMEPT_Year == d.ASMAY_Id && d.Is_Active == true)
                           select new NAACHRMasterEmpFullTimeDTO
                           {
                               HRME_Id = b.HRME_Id,
                               HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName.Trim() == "" ? "" : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName.Trim() == "" ? "" : c.HRME_EmployeeLastName),
                               HRMEPT_Year = b.HRMEPT_Year,
                               ASMAY_Year = d.ASMAY_Year,
                               ASMAY_From_Date = d.ASMAY_From_Date,
                               ASMAY_To_Date = d.ASMAY_To_Date,
                               HRMEPT_ActiveFlag = b.HRMEPT_ActiveFlag,
                               HRMEPT_Id = b.HRMEPT_Id
                           }).ToArray();
                
            return dto;
        }

        public NAACHRMasterEmpFullTimeDTO savedata(NAACHRMasterEmpFullTimeDTO dto)
        {
            try
            {
                if (dto.HRMEPT_Id > 0)
                {
                    for (int i = 0; i < dto.selectedEmp.Length; i++)
                    {
                        var tempid = dto.selectedEmp[i].HRME_Id;
                        //var mi_id = _Context.HR_Master_Employee_DMO.Where(t => t.HRME_Id == tempid).SingleOrDefault().MI_Id;
                        var duplicate = _HRMSContext.NAACHRMasterEmpFullTimeDMO.Where(t => t.HRMEPT_Id!=dto.HRMEPT_Id && t.HRME_Id == tempid && t.HRMEPT_Year == dto.HRMEPT_Year).ToList();

                        if (duplicate.Count > 0)
                        {
                            dto.duplicate = true;
                            dto.count += 1;
                        }
                        else
                        {
                            dto.count1 += 1;
                            var tabledata = _HRMSContext.NAACHRMasterEmpFullTimeDMO.Where(t => t.HRMEPT_Id == dto.HRMEPT_Id).FirstOrDefault();
                            //NAACHRMasterEmpFullTimeDMO obj = new NAACHRMasterEmpFullTimeDMO();
                            //obj.HRMEPT_Id = dto.HRMEPT_Id;
                            tabledata.HRMEPT_Year = dto.HRMEPT_Year;
                            tabledata.HRME_Id = tempid;
                            tabledata.HRMEPT_ActiveFlag = true;

                            _HRMSContext.Update(tabledata);
                            
                        }
                    }
                    if (dto.count1 != 0)
                    {
                        int s = _HRMSContext.SaveChanges();
                        if (s > 0)
                        {
                            dto.msg = "updated";
                        }
                        else
                        {
                            dto.msg = "updateFailed";
                        }
                    }
                    else
                    {
                        dto.msg = "duplicate";
                    }

                }
                else
                {
                    for (int i = 0; i < dto.selectedEmp.Length; i++)
                    {
                        var tempid = dto.selectedEmp[i].HRME_Id;

                        var duplicate = _HRMSContext.NAACHRMasterEmpFullTimeDMO.Where(t => t.HRME_Id == tempid && t.HRMEPT_Year == dto.HRMEPT_Year).ToList();
                        if (duplicate.Count > 0)
                        {
                            dto.msg = "duplicate";
                            dto.count += 1;
                        }
                        else
                        {
                            dto.count1 += 1;
                            NAACHRMasterEmpFullTimeDMO obj = new NAACHRMasterEmpFullTimeDMO();

                            obj.HRMEPT_Year = dto.HRMEPT_Year;
                            obj.HRME_Id = tempid;
                            obj.HRMEPT_ActiveFlag = true;
                            _HRMSContext.Add(obj);
                            
                        }
                    }
                   
                    if (dto.count1 != 0)
                    {
                        int s = _HRMSContext.SaveChanges();
                        if (s > 0)
                        {
                            dto.msg = "saved";
                        }
                        else
                        {
                            dto.msg = "savingFailed";
                        }
                    }
                    else
                    {
                        dto.msg = "duplicate";
                    }


                }

                dto.empdata = (from a in _HRMSContext.Institution
                               from b in _HRMSContext.NAACHRMasterEmpFullTimeDMO
                               from c in _HRMSContext.MasterEmployee
                               from d in _HRMSContext.AcademicYear
                               where (a.MI_Id == dto.MI_Id && b.HRME_Id == c.HRME_Id && c.HRME_ActiveFlag == true && a.MI_Id == c.MI_Id && c.HRME_LeftFlag == false && b.HRMEPT_Year == d.ASMAY_Id && d.Is_Active == true)
                               select new NAACHRMasterEmpFullTimeDTO
                               {
                                   HRME_Id = b.HRME_Id,
                                   HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName.Trim() == "" ? "" : c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName.Trim() == "" ? "" : c.HRME_EmployeeLastName),
                                   HRMEPT_Year = b.HRMEPT_Year,
                                   ASMAY_Year = d.ASMAY_Year,
                                   ASMAY_From_Date = d.ASMAY_From_Date,
                                   ASMAY_To_Date = d.ASMAY_To_Date,
                                   HRMEPT_ActiveFlag = b.HRMEPT_ActiveFlag,
                                   HRMEPT_Id = b.HRMEPT_Id
                               }).ToArray();
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dto;
        }

        public NAACHRMasterEmpFullTimeDTO editRecord(NAACHRMasterEmpFullTimeDTO dto)
        {
            
            try
            {
                List<NAACHRMasterEmpFullTimeDMO> lorg = new List<NAACHRMasterEmpFullTimeDMO>();
                lorg = _HRMSContext.NAACHRMasterEmpFullTimeDMO.Where(t => t.HRMEPT_Id.Equals(dto.HRMEPT_Id)).ToList();
                dto.fulltimedetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);                
            }

            return dto;
        }

        public NAACHRMasterEmpFullTimeDTO ActiveDeactiveRecord(NAACHRMasterEmpFullTimeDTO dto)
        {
            
            try
            {
                if (dto.HRMEPT_Id > 0)
                {
                    var result = _HRMSContext.NAACHRMasterEmpFullTimeDMO.Single(t => t.HRMEPT_Id == dto.HRMEPT_Id);
                    if (result.HRMEPT_ActiveFlag == true)
                    {
                        result.HRMEPT_ActiveFlag = false;
                    }
                    else if (result.HRMEPT_ActiveFlag == false)
                    {
                        result.HRMEPT_ActiveFlag = true;
                    }

                    _HRMSContext.Update(result);

                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMEPT_ActiveFlag == true)
                        {
                            dto.returnval = true;
                            dto.msg = "Staff Mapping Activated Successfully.";
                        }
                        else
                        {
                            dto.returnval = false;
                            dto.msg = "Staff Mapping Deactivated Successfully.";
                        }
                    }
                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);               
            }
            return dto;
        }
    }
}
