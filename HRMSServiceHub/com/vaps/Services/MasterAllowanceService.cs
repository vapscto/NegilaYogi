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
    public class MasterAllowanceService : Interfaces.MasterAllowanceInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterAllowanceService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public MasterAllowanceDTO getBasicData(MasterAllowanceDTO dto)
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

        public MasterAllowanceDTO SaveUpdate(MasterAllowanceDTO dto)
         {
            dto.retrunMsg = "";
            try
            {
                HR_Master_Allowance dmoObj = Mapper.Map<HR_Master_Allowance>(dto);

                var alldata = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRMAL_AllowanceName.Equals(dto.HRMAL_AllowanceName) &&  t.HRMAL_MaxLimit == dto.HRMAL_MaxLimit).Count();
                var alldata1 = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id == dto.MI_Id  && t.HRMAL_MaxLimit == dto.HRMAL_MaxLimit).Count();


                if (alldata == 0)
                {
                    if (dmoObj.HRMAL_Id > 0)
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRMAL_AllowanceName.Equals(dto.HRMAL_AllowanceName) && t.HRMAL_MaxLimit != dmoObj.HRMAL_MaxLimit && t.HRMAL_Id != dmoObj.HRMAL_Id).Count();

                        if (duplicateHRMBD_BankName == 0)
                        {
                            var result = _HRMSContext.HR_Master_Allowance.Single(t => t.HRMAL_Id == dmoObj.HRMAL_Id);

                            dto.HRMAL_ActiveFlg = true;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "AllDuplicate";
                            }
                        }
                    }



                    else
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRMAL_AllowanceName.Equals(dto.HRMAL_AllowanceName)).Count();
                        if (duplicateHRMBD_BankName == 0)
                        {
                            dmoObj.HRMAL_ActiveFlg = true;
                            dmoObj.HRMAL_AllowanceName = dto.HRMAL_AllowanceName;
                            dmoObj.HRMAL_MaxLimitAplFlg = dto.HRMAL_MaxLimitAplFlg;
                            dmoObj.HRMAL_MaxLimit = dto.HRMAL_MaxLimit;

                            //dmoObj.HRMQM_CreatedBy = dto.LogInUserId;
                            //dmoObj.HRMQM_UpdatedBy = dto.LogInUserId;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRMAL_CreatedBy = dto.LogInUserId;
                            dmoObj.HRMAL_UpdatedBy = dto.LogInUserId;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                            //var result = _HRMSContext.HR_Master_Allowance.Single(t => t.HRMAL_Id == dmoObj.HRMAL_Id);

                            //HR_Master_Quarter_Month dmoObjs = Mapper.Map<HR_Master_Quarter_Month>(dto);
                            //dmoObjs.HRMQ_Id = result.HRMQ_Id;
                            //dmoObjs.HRMQM_ActiveFlg = result.HRMQ_ActiveFlg;
                            //dmoObjs.HRMQM_CreatedBy = dto.LogInUserId;
                            //dmoObjs.HRMQM_UpdatedBy = dto.LogInUserId;
                            //_HRMSContext.Add(dmoObjs);
                            //var flags = _HRMSContext.SaveChanges();
                            //if (flags == 1)
                            //{
                            //    dto.retrunMsg = "Add";
                            //}
                            //else
                            //{
                            //    dto.retrunMsg = "false";
                            //}
                        }



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

        public MasterAllowanceDTO editData(int id)
        {

            MasterAllowanceDTO dto = new MasterAllowanceDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_Allowance> lorg = new List<HR_Master_Allowance>();
                lorg = _HRMSContext.HR_Master_Allowance.AsNoTracking().Where(t => t.HRMAL_Id.Equals(id)).ToList();
                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public MasterAllowanceDTO deactivate(MasterAllowanceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMAL_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_Allowance.Single(t => t.HRMAL_Id == dto.HRMAL_Id);

                    if (result.HRMAL_ActiveFlg == true)
                    {
                        result.HRMAL_ActiveFlg = false;
                    }
                    else if (result.HRMAL_ActiveFlg == false)
                    {
                        result.HRMAL_ActiveFlg = true;
                    }
                   // result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMAL_ActiveFlg == true)
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

        public MasterAllowanceDTO GetAllDropdownAndDatatableDetails(MasterAllowanceDTO dto)
        {
            List<HR_Master_Allowance> datalist = new List<HR_Master_Allowance>();
            try
            {               
                datalist = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
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
