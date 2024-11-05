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
    public class MasterGradeService : Interfaces.MasterGradeInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterGradeService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_GradeDTO getBasicData(HR_Master_GradeDTO dto)
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

        //Onchange

        public HR_Master_GradeDTO changeorderData(HR_Master_GradeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.GradeDTO.Count() > 0)
                {
                    foreach (HR_Master_GradeDTO mob in dto.GradeDTO)
                    {
                        if (mob.HRMG_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_Grade.Single(t => t.HRMG_Id == mob.HRMG_Id);

                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }

                    }

                    dto.retrunMsg = "Order updated successfully";
                }
                else
                {
                    dto.retrunMsg = "Someting is wrong please Check it !!!";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_GradeDTO SaveUpdate(HR_Master_GradeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_Grade dmoObj = Mapper.Map<HR_Master_Grade>(dto);

                var duplicatecountresultorder = _HRMSContext.HR_Master_Grade.Where(t => t.HRMG_GradeName == dto.HRMG_GradeName && t.MI_Id == dto.MI_Id && t.HRMG_GradeDisplayName.Equals(dto.HRMG_GradeDisplayName) && t.HRMG_PayScaleFrom == dto.HRMG_PayScaleFrom && t.HRMG_PayScaleRange == dto.HRMG_PayScaleRange && t.HRMG_PayScaleTo == dto.HRMG_PayScaleTo && t.HRMG_IncrementOf == dto.HRMG_IncrementOf).Count();
                if (duplicatecountresultorder == 0)
                {
                    if (dmoObj.HRMG_Id > 0)
                    {
                        var duplicatecount = _HRMSContext.HR_Master_Grade.Where(t => t.HRMG_GradeName == dto.HRMG_GradeName && t.MI_Id == dto.MI_Id && t.HRMG_Id !=dto.HRMG_Id).Count();
                        if (duplicatecount == 0)
                        {
                            var result = _HRMSContext.HR_Master_Grade.Single(t => t.HRMG_Id == dmoObj.HRMG_Id);

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
                            return dto;
                        }
                    }
                    else
                    {
                        var duplicatecount = _HRMSContext.HR_Master_Grade.Where(t => t.HRMG_GradeName == dto.HRMG_GradeName && t.MI_Id == dto.MI_Id).Count();
                        if (duplicatecount == 0)
                        {
                            dmoObj.HRMG_ActiveFlag = true;
                            dmoObj.HRMG_GradeName = dto.HRMG_GradeName;
                            dmoObj.HRMG_GradeDisplayName = dto.HRMG_GradeDisplayName;
                            dmoObj.HRMG_PayScaleRange = dto.HRMG_PayScaleRange;
                            dmoObj.HRMG_PayScaleFrom = dto.HRMG_PayScaleFrom;
                            dmoObj.HRMG_IncrementOf = dto.HRMG_IncrementOf;
                            dmoObj.HRMG_PayScaleTo = dto.HRMG_PayScaleTo;
                            dmoObj.HRMG_Order = dto.HRMG_Order;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {

                                HR_Master_GradeDTO DTO = Mapper.Map<HR_Master_GradeDTO>(dmoObj);
                                DTO.HRMG_Order = Convert.ToInt32(DTO.HRMG_Id);
                                var result = _HRMSContext.HR_Master_Grade.Single(t => t.HRMG_Id == DTO.HRMG_Id);
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
                            return dto;
                        }

                    }

                }

                else if (duplicatecountresultorder > 0)
                {
                    dto.retrunMsg = "AllDuplicate";
                    return dto;
                }

               
                List<HR_Master_Grade> datalist = new List<HR_Master_Grade>();
                datalist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.gradelList = datalist.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_GradeDTO editData(int id)
        {

            HR_Master_GradeDTO dto = new HR_Master_GradeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_Grade> lorg = new List<HR_Master_Grade>();
                lorg = _HRMSContext.HR_Master_Grade.AsNoTracking().Where(t => t.HRMG_Id.Equals(id)).ToList();
                dto.gradelList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Master_GradeDTO deactivate(HR_Master_GradeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMG_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_Grade.Single(t => t.HRMG_Id == dto.HRMG_Id);

                    if (result.HRMG_ActiveFlag == true)
                    {
                        result.HRMG_ActiveFlag = false;
                    }
                    else if (result.HRMG_ActiveFlag == false)
                    {
                        result.HRMG_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMG_ActiveFlag == true)
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

                    List<HR_Master_Grade> datalist = new List<HR_Master_Grade>();
                    datalist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.gradelList = datalist.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_GradeDTO GetAllDropdownAndDatatableDetails(HR_Master_GradeDTO dto)
        {
            List<HR_Master_Grade> datalist = new List<HR_Master_Grade>();
            try
            {
                
                    datalist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.gradelList = datalist.OrderBy(t=>t.HRMG_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
