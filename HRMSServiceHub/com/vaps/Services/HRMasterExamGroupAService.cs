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
    public class HRMasterExamGroupAService : Interfaces.HRMasterExamGroupAInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRMasterExamGroupAService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_MasterExam_GroupADTO getBasicData(HR_MasterExam_GroupADTO dto)
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

        public HR_MasterExam_GroupADTO SaveUpdate(HR_MasterExam_GroupADTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_MasterExam_GroupADMO dmoObj = Mapper.Map<HR_MasterExam_GroupADMO>(dto);

                var alldata = _HRMSContext.HR_MasterExam_GroupADMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGA_GroupAExamName.Equals(dto.HRMEGA_GroupAExamName)).Count();
                if (alldata == 0)
                {
                    if (dmoObj.HRMEGA_Id > 0)
                    {
                        var duplicateExamName = _HRMSContext.HR_MasterExam_GroupADMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGA_GroupAExamName.Equals(dto.HRMEGA_GroupAExamName) && t.HRMEGA_Id != dmoObj.HRMEGA_Id).Count();

                        if (duplicateExamName == 0)
                            {
                                var result = _HRMSContext.HR_MasterExam_GroupADMO.Single(t => t.HRMEGA_Id == dmoObj.HRMEGA_Id);
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
                        else if (duplicateExamName > 0)
                        {
                            dto.retrunMsg = "bank";
                            return dto;
                        }
                    }
                    else
                        {
                        var duplicateExamName = _HRMSContext.HR_MasterExam_GroupADMO.Where(t => t.MI_Id == dto.MI_Id && t.HRMEGA_GroupAExamName.Equals(dto.HRMEGA_GroupAExamName)).Count();

                        if (duplicateExamName == 0)
                        {
                            dmoObj.HRMEGA_ActiveFlg = true;
                            dmoObj.HRMEGA_GroupAExamName = dto.HRMEGA_GroupAExamName;
                            dmoObj.HRMEGA_CreatedBy = dto.HRMEGA_CreatedBy;
                            dmoObj.HRMEGA_UpdatedBy = dto.HRMEGA_UpdatedBy;
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

                        else if (duplicateExamName > 0)
                        {
                            dto.retrunMsg = "bank";
                            return dto;
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

        public HR_MasterExam_GroupADTO editData(int id)
        {
            HR_MasterExam_GroupADTO dto = new HR_MasterExam_GroupADTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_MasterExam_GroupADMO> lorg = new List<HR_MasterExam_GroupADMO>();
                lorg = _HRMSContext.HR_MasterExam_GroupADMO.AsNoTracking().Where(t => t.HRMEGA_Id.Equals(id)).ToList();
                dto.examdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MasterExam_GroupADTO deactivate(HR_MasterExam_GroupADTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMEGA_Id > 0)
                {
                    var result = _HRMSContext.HR_MasterExam_GroupADMO.Single(t => t.HRMEGA_Id == dto.HRMEGA_Id);

                    if (result.HRMEGA_ActiveFlg == true)
                    {
                        result.HRMEGA_ActiveFlg = false;
                    }
                    else if (result.HRMEGA_ActiveFlg == false)
                    {
                        result.HRMEGA_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMEGA_ActiveFlg == true)
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

        public HR_MasterExam_GroupADTO GetAllDropdownAndDatatableDetails(HR_MasterExam_GroupADTO dto)
        {
            List<HR_MasterExam_GroupADMO> datalist = new List<HR_MasterExam_GroupADMO>();
            try
            {
                datalist = _HRMSContext.HR_MasterExam_GroupADMO.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.examdetailList = datalist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
