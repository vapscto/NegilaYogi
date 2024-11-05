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
    public class MasterotherIncomeService : Interfaces.MasterotherIncomeInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterotherIncomeService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_master_otherIncomeDTO getBasicData(HR_master_otherIncomeDTO dto)
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

        public HR_master_otherIncomeDTO SaveUpdate(HR_master_otherIncomeDTO dto)
        {
            dto.retrunMsg = "";
          

            try
            {
                HR_Master_OtherIncome dmoObj = Mapper.Map<HR_Master_OtherIncome>(dto);

                var alldata = _HRMSContext.HR_Master_OtherIncome.Where(t => t.MI_Id == dto.MI_Id && t.HRMOI_OtherIncomeName.Equals(dto.HRMOI_OtherIncomeName) && t.HRMOI_MaxLimit == dto.HRMOI_MaxLimit).Count();
                var alldata1 = _HRMSContext.HR_Master_OtherIncome.Where(t => t.MI_Id == dto.MI_Id && t.HRMOI_MaxLimit == dto.HRMOI_MaxLimit).Count();


                if (alldata == 0)
                {
                    if (dmoObj.HRMOI_Id > 0)
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_OtherIncome.Where(t => t.MI_Id == dto.MI_Id && t.HRMOI_OtherIncomeName.Equals(dto.HRMOI_OtherIncomeName) && t.HRMOI_MaxLimit != dmoObj.HRMOI_MaxLimit && t.HRMOI_Id != dmoObj.HRMOI_Id).Count();

                        if (duplicateHRMBD_BankName == 0)
                        {
                            var result = _HRMSContext.HR_Master_OtherIncome.Single(t => t.HRMOI_Id == dmoObj.HRMOI_Id);

                            dto.HRMOI_ActiveFlg = true;
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
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_OtherIncome.Where(t => t.MI_Id == dto.MI_Id && t.HRMOI_OtherIncomeName.Equals(dto.HRMOI_OtherIncomeName)).Count();
                        if (duplicateHRMBD_BankName == 0)
                        {


                            dmoObj.HRMOI_ActiveFlg = true;
                            dmoObj.HRMOI_OtherIncomeName = dto.HRMOI_OtherIncomeName;
                            dmoObj.HRMOI_OtherIncomeFlg = dto.HRMOI_OtherIncomeFlg;
                            dmoObj.HRMOI_MaxLimitAplFlg = dto.HRMOI_MaxLimitAplFlg;
                            dmoObj.HRMOI_MaxLimit = dto.HRMOI_MaxLimit;
                            dmoObj.MI_Id = dto.MI_Id;
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

        public HR_master_otherIncomeDTO editData(int id)
        {
            HR_master_otherIncomeDTO dto = new HR_master_otherIncomeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_OtherIncome> lorg = new List<HR_Master_OtherIncome>();
                lorg = _HRMSContext.HR_Master_OtherIncome.AsNoTracking().Where(t => t.HRMOI_Id.Equals(id)).ToList();
                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_master_otherIncomeDTO deactivate(HR_master_otherIncomeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMOI_Id> 0)
                {
                    var result = _HRMSContext.HR_Master_OtherIncome.Single(t => t.HRMOI_Id == dto.HRMOI_Id);
                    if (result.HRMOI_ActiveFlg == true)
                    {
                        result.HRMOI_ActiveFlg = false;
                    }
                    else if (result.HRMOI_ActiveFlg == false)
                    {
                        result.HRMOI_ActiveFlg = true;
                    }
                   // result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMOI_ActiveFlg == true)
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

        public HR_master_otherIncomeDTO GetAllDropdownAndDatatableDetails(HR_master_otherIncomeDTO dto)
        {
            List<HR_Master_OtherIncome> datalist = new List<HR_Master_OtherIncome>();
            try
            {
               
                    datalist = _HRMSContext.HR_Master_OtherIncome.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
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
