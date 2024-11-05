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
    public class masterparameterService : Interfaces.masterparameterInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public masterparameterService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Employee_AssesmentparameterDTO getBasicData(HR_Employee_AssesmentparameterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Employee_AssesmentparameterDTO SaveUpdate(HR_Employee_AssesmentparameterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Employee_Assesment_Parameter dmoObj = Mapper.Map<HR_Employee_Assesment_Parameter>(dto);

                var duplicatecountresult = _HRMSContext.HR_Employee_Assementparameter.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_parameter.Equals(dto.HR_Emp_As_parameter) && t.HR_Emp_As_parameterdesc.Equals(dto.HR_Emp_As_parameterdesc)).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HR_Emp_As_paraid > 0)
                    {
                        var QulaificationName = _HRMSContext.HR_Employee_Assementparameter.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_parameter.Equals(dto.HR_Emp_As_parameter) && t.HR_Emp_As_parameterdesc == dto.HR_Emp_As_parameterdesc).Count();
                        if (QulaificationName == 0)
                        {
                            var result = _HRMSContext.HR_Employee_Assementparameter.Single(t => t.HR_Emp_As_paraid == dmoObj.HR_Emp_As_paraid);

                            dto.UpdatedDate = DateTime.Now;
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
                            dto.retrunMsg = "Duplicate";
                        }


                    }
                    else
                    {
                        var QulaificationName = _HRMSContext.HR_Employee_Assementparameter.Where(t => t.MI_Id == dto.MI_Id && t.HR_Emp_As_parameter.Equals(dto.HR_Emp_As_parameter)).Count();
                        if (QulaificationName == 0)
                        {
                            dmoObj.HR_Emp_Assparameter_ActiveFlag = true;
                            dmoObj.HR_Emp_As_parameter = dto.HR_Emp_As_parameter;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HR_Emp_As_parameterdesc = dto.HR_Emp_As_parameterdesc;
                         
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {

                                HR_Employee_AssesmentparameterDTO DTO = Mapper.Map<HR_Employee_AssesmentparameterDTO>(dmoObj);
                              
                                var result = _HRMSContext.HR_Employee_Assementparameter.Single(t => t.HR_Emp_As_paraid == DTO.HR_Emp_As_paraid);
                                Mapper.Map(DTO, result);
                                _HRMSContext.Update(result);
                                _HRMSContext.SaveChanges();

                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }


                }
                else
                {
                    dto.retrunMsg = "AllDuplicate";

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

        public HR_Employee_AssesmentparameterDTO editData(int id)
        {

            HR_Employee_AssesmentparameterDTO dto = new HR_Employee_AssesmentparameterDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Employee_Assesment_Parameter> lorg = new List<HR_Employee_Assesment_Parameter>();
                lorg = _HRMSContext.HR_Employee_Assementparameter.AsNoTracking().Where(t => t.HR_Emp_As_paraid.Equals(id)).ToList();
                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Employee_AssesmentparameterDTO deactivate(HR_Employee_AssesmentparameterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HR_Emp_As_paraid > 0)
                {
                    var result = _HRMSContext.HR_Employee_Assementparameter.Single(t => t.HR_Emp_As_paraid == dto.HR_Emp_As_paraid);

                    if (result.HR_Emp_Assparameter_ActiveFlag == true)
                    {
                        result.HR_Emp_Assparameter_ActiveFlag = false;
                    }
                    else if (result.HR_Emp_Assparameter_ActiveFlag == false)
                    {
                        result.HR_Emp_Assparameter_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HR_Emp_Assparameter_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

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

        public HR_Employee_AssesmentparameterDTO GetAllDropdownAndDatatableDetails(HR_Employee_AssesmentparameterDTO dto)
        {
            List<HR_Employee_Assesment_Parameter> datalist = new List<HR_Employee_Assesment_Parameter>();
            try
            {

                datalist = _HRMSContext.HR_Employee_Assementparameter.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
             dto.bankdetailList = datalist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
