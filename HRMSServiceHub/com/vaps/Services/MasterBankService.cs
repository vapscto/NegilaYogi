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
    public class MasterBankService : Interfaces.MasterBankInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterBankService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_BankDeatilsDTO getBasicData(HR_Master_BankDeatilsDTO dto)
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

        public HR_Master_BankDeatilsDTO SaveUpdate(HR_Master_BankDeatilsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_BankDeatils dmoObj = Mapper.Map<HR_Master_BankDeatils>(dto);

                var alldata = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BankName.Equals(dto.HRMBD_BankName) && t.HRMBD_BankAccountNo.Equals(dto.HRMBD_BankAccountNo) && t.HRMBD_BranchName.Equals(dto.HRMBD_BranchName) && t.HRMBD_IFSCCode == dto.HRMBD_IFSCCode && t.HRMBD_BankAddress == dto.HRMBD_BankAddress).Count();
                if (alldata == 0)
                {

                    if (dmoObj.HRMBD_Id > 0)
                    {

                            var duplicateHRMBD_BankName = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BankName.Equals(dto.HRMBD_BankName) && t.HRMBD_Id != dmoObj.HRMBD_Id).Count();
                            var duplicateaccno = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BankAccountNo.Equals(dto.HRMBD_BankAccountNo) && t.HRMBD_Id != dmoObj.HRMBD_Id).Count();
                            var duplicatebranch = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BranchName.Equals(dto.HRMBD_BranchName)).Count();
                            var duplicateifsc = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_IFSCCode.Equals(dto.HRMBD_IFSCCode)).Count();

                        if (duplicateaccno == 0 && duplicatebranch == 0 && duplicateifsc == 0)
                            {
                                var result = _HRMSContext.HR_Master_BankDeatils.Single(t => t.HRMBD_Id == dmoObj.HRMBD_Id);
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
                        else if (duplicateaccno > 0 ||  duplicatebranch > 0 || duplicateifsc > 0)
                        {
                            if (duplicateaccno > 0)
                            {
                                dto.retrunMsg = "acc";
                                return dto;
                            }
                           
                            else if (duplicatebranch > 0)
                            {
                                dto.retrunMsg = "branch";
                                return dto;
                            }
                            else if (duplicateifsc > 0)
                            {
                                dto.retrunMsg = "ifsc";
                                return dto;
                            }
                        }
                                    

                    }
                    else
                        {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BankName.Equals(dto.HRMBD_BankName)).Count();
                        var duplicateaccno = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BankAccountNo.Equals(dto.HRMBD_BankAccountNo)).Count();
                        var duplicatebranch = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_BranchName.Equals(dto.HRMBD_BranchName)).Count();
                        var duplicateifsc = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_IFSCCode.Equals(dto.HRMBD_IFSCCode)).Count();

                        if (duplicateaccno == 0 && duplicatebranch == 0 && duplicateifsc == 0)
                        {
                            dmoObj.HRMBD_ActiveFlag = true;
                            dmoObj.HRMBD_BankName = dto.HRMBD_BankName;
                            dmoObj.HRMBD_BankAccountNo = dto.HRMBD_BankAccountNo;
                            dmoObj.HRMBD_BankAddress = dto.HRMBD_BankAddress;
                            dmoObj.HRMBD_BranchName = dto.HRMBD_BranchName;
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.UpdatedDate = DateTime.Now;
                            dmoObj.CreatedDate = DateTime.Now;
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

                        else if (duplicateaccno > 0 ||  duplicatebranch > 0 || duplicateifsc > 0)
                        {
                            if (duplicateaccno > 0)
                            {
                                dto.retrunMsg = "acc";
                                return dto;
                            }

                            else if (duplicatebranch > 0)
                            {
                                dto.retrunMsg = "branch";
                                return dto;
                            }
                            else if (duplicateifsc > 0)
                            {
                                dto.retrunMsg = "ifsc";
                                return dto;
                            }
                        }

                    }

                }else
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

        public HR_Master_BankDeatilsDTO editData(int id)
        {

            HR_Master_BankDeatilsDTO dto = new HR_Master_BankDeatilsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_BankDeatils> lorg = new List<HR_Master_BankDeatils>();
                lorg = _HRMSContext.HR_Master_BankDeatils.AsNoTracking().Where(t => t.HRMBD_Id.Equals(id)).ToList();
                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_BankDeatilsDTO deactivate(HR_Master_BankDeatilsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMBD_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_BankDeatils.Single(t => t.HRMBD_Id == dto.HRMBD_Id);

                    if (result.HRMBD_ActiveFlag == true)
                    {
                        result.HRMBD_ActiveFlag = false;
                    }
                    else if (result.HRMBD_ActiveFlag == false)
                    {
                        result.HRMBD_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMBD_ActiveFlag == true)
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

        public HR_Master_BankDeatilsDTO GetAllDropdownAndDatatableDetails(HR_Master_BankDeatilsDTO dto)
        {
            List<HR_Master_BankDeatils> datalist = new List<HR_Master_BankDeatils>();
            try
            {
               
                    datalist = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
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
