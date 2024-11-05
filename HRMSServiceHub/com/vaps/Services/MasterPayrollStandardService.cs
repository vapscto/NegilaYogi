using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterPayrollStandardService : Interfaces.MasterPayrollStandardInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterPayrollStandardService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;

        }
        public HR_ConfigurationDTO getBasicData(HR_ConfigurationDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public HR_ConfigurationDTO SaveUpdate(HR_ConfigurationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Configuration dmoObj = Mapper.Map<HR_Configuration>(dto);

                if (dmoObj.HRC_Id > 0)
                {
                    var result = _HRMSContext.HR_Configuration.Single(t => t.HRC_Id == dmoObj.HRC_Id);
                    Mapper.Map(dto, result);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    dmoObj.MI_Id = dto.MI_Id;
                    _HRMSContext.Add(dmoObj);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "False";
                    }

                }

                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_ConfigurationDTO editData(int id)
        {

            HR_ConfigurationDTO dto = new HR_ConfigurationDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Configuration> lorg = new List<HR_Configuration>();
                lorg = _HRMSContext.HR_Configuration.AsNoTracking().Where(t => t.HRC_Id.Equals(id)).ToList();
                dto.hrStandardList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_ConfigurationDTO deactivate(HR_ConfigurationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRC_Id > 0)
                {
                    var result = _HRMSContext.HR_Configuration.Single(t => t.HRC_Id == dto.HRC_Id);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_ConfigurationDTO GetAllDropdownAndDatatableDetails(HR_ConfigurationDTO dto)
        {
            List<HR_Configuration> PayrollStandard = new List<HR_Configuration>();
            // List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();

            try
            {

                    PayrollStandard = _HRMSContext.HR_Configuration.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.hrStandardList = PayrollStandard.ToArray();


                dto.monthdropdown = _Context.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
