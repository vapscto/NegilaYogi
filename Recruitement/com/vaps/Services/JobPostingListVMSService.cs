using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class JobPostingListVMSService : Interfaces.JobPostingListVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public JobPostingListVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_MRFRequisitionDTO getBasicData(HR_MRFRequisitionDTO dto)
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

        public HR_MRFRequisitionDTO SaveUpdate(HR_MRFRequisitionDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_MRFRequisitionDMO dmoObj = Mapper.Map<HR_MRFRequisitionDMO>(dto);
                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO editData(int id)
        {
            HR_MRFRequisitionDTO dto = new HR_MRFRequisitionDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSEditValue = (from a in _VMSContext.HR_MRFRequisitionDMO
                                  from b in _VMSContext.HR_Master_PositionDMO
                                  from c in _VMSContext.HR_Master_Department
                                  from d in _VMSContext.HR_Master_PriorityDMO
                                  from e in _VMSContext.HR_Master_PostionTypeDMO
                                  from g in _VMSContext.HR_Master_Course
                                  from h in _VMSContext.IVRM_Master_Gender
                                  where (a.HRMRFR_Id.Equals(id) && a.HRMP_Id == b.HRMP_Id && a.MI_Id == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.MI_Id == c.MI_Id && a.HRMPR_Id == d.HRMPR_Id && a.MI_Id == d.MI_Id && a.HRMPT_Id == e.HRMPT_Id && a.MI_Id == e.MI_Id && a.HRMC_Id == g.HRMC_Id && a.MI_Id == g.MI_Id && a.IVRMMG_Id == h.IVRMMG_Id && a.MI_Id == h.MI_Id)
                                  select new HR_MRFRequisitionDTO
                                  {
                                      HRMRFR_Id = a.HRMRFR_Id,
                                      HRMP_Id = a.HRMP_Id,
                                      HRMP_Position = b.HRMP_Position,
                                      HRMD_Id = a.HRMD_Id,
                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                      HRMPR_Id = a.HRMPR_Id,
                                      HRMP_Name = d.HRMP_Name,
                                      HRMPT_Id = a.HRMPT_Id,
                                      HRMPT_Name = e.HRMPT_Name,
                                      HRMC_Id = a.HRMC_Id,
                                      HRMC_QulaificationName = g.HRMC_QulaificationName,
                                      HRMRFR_NoofPosition = a.HRMRFR_NoofPosition,
                                      HRMRFR_Skills = a.HRMRFR_Skills,
                                      HRMRFR_JobDesc = a.HRMRFR_JobDesc,
                                      HRMRFR_ExpFrom = a.HRMRFR_ExpFrom,
                                      HRMRFR_ExpTo = a.HRMRFR_ExpTo,
                                      HRMRFR_Age = a.HRMRFR_Age,
                                      HRMRFR_MRFNO = a.HRMRFR_MRFNO,
                                      IVRMMG_Id = a.IVRMMG_Id,
                                      IVRMMG_GenderName = h.IVRMMG_GenderName,
                                      HRMRFR_Reason = a.HRMRFR_Reason,
                                      HRMRFR_Remark = a.HRMRFR_Remark,
                                      HRMRFR_WrittenTestFlg = a.HRMRFR_WrittenTestFlg,
                                      HRMRFR_OnlineTestFlg = a.HRMRFR_OnlineTestFlg,
                                      HRMRFR_TechnicalInterviewFlg = a.HRMRFR_TechnicalInterviewFlg,
                                      HRMRFR_Attachment = a.HRMRFR_Attachment,
                                      HRMRFR_CreatedBy = a.HRMRFR_CreatedBy,
                                      CreatedDate = a.CreatedDate
                                  }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_MRFRequisitionDTO GetAllDropdownAndDatatableDetails(HR_MRFRequisitionDTO dto)
        {
            List<HR_MRFRequisitionDMO> datalist = new List<HR_MRFRequisitionDMO>();
            try
            {
                var mrfdatalist = from a in _VMSContext.HR_MRFRequisitionDMO
                                  from b in _VMSContext.HR_Master_PositionDMO
                                  from d in _VMSContext.HR_Master_PriorityDMO
                                  where (a.HRMP_Id == b.HRMP_Id && a.HRMPR_Id == d.HRMPR_Id && a.HRMRFR_Status != "Approved")
                                  select new HR_MRFRequisitionDTO
                                  {
                                      HRMRFR_Id = a.HRMRFR_Id,
                                      HRMP_Id = a.HRMP_Id,
                                      HRMP_Position = b.HRMP_Position,
                                      HRMPR_Id = a.HRMPR_Id,
                                      HRMP_Name = d.HRMP_Name,
                                      HRMRFR_MRFNO = a.HRMRFR_MRFNO,
                                      HRMRFR_NoofPosition = a.HRMRFR_NoofPosition,
                                      HRMRFR_Status = a.HRMRFR_Status,
                                      HRMRFR_JobLocation = a.HRMRFR_JobLocation
                                  };
                dto.VMSMRFList = mrfdatalist.ToArray();

                dto.MasterPositionList = (from emp in _VMSContext.HR_Master_PositionDMO
                                          where emp.HRMP_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                          select new HR_MRFRequisitionDTO
                                          {
                                              HRMP_Id = emp.HRMP_Id,
                                              HRMP_Position = emp.HRMP_Position
                                          }).ToArray();

                dto.MasterLocation = (from emp in _VMSContext.HR_Master_LocationDMO
                                      where (emp.HRMLO_ActiveFlg == true && emp.MI_Id == dto.MI_Id)
                                      select new HR_MRFRequisitionDTO
                                      {
                                          HRMLO_Id = emp.HRMLO_Id,
                                          HRMLO_LocationName = emp.HRMLO_LocationName
                                      }).ToArray();

                dto.MasterDepartmentList = (from emp in _VMSContext.HR_Master_Department
                                            where (emp.HRMD_ActiveFlag == true && emp.MI_Id == dto.MI_Id)
                                            select new HR_MRFRequisitionDTO
                                            {
                                                HRMD_Id = emp.HRMD_Id,
                                                HRMD_DepartmentName = emp.HRMD_DepartmentName
                                            }).ToArray();

                dto.MasterPriorityList = (from emp in _VMSContext.HR_Master_PriorityDMO
                                          where emp.HRMP_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                          select new HR_MRFRequisitionDTO
                                          {
                                              HRMPR_Id = emp.HRMPR_Id,
                                              HRMP_Name = emp.HRMP_Name
                                          }).ToArray();

                dto.MasterPosTypeList = (from emp in _VMSContext.HR_Master_PostionTypeDMO
                                         where emp.HRMPT_ActiveFlg == true && emp.MI_Id == dto.MI_Id
                                         select new HR_MRFRequisitionDTO
                                         {
                                             HRMPT_Id = emp.HRMPT_Id,
                                             HRMPT_Name = emp.HRMPT_Name
                                         }).ToArray();

                dto.MasterQualification = (from emp in _VMSContext.HR_Master_Course
                                           where emp.HRMC_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                           select new HR_MRFRequisitionDTO
                                           {
                                               HRMC_Id = emp.HRMC_Id,
                                               HRMC_QulaificationName = emp.HRMC_QulaificationName
                                           }).ToArray();

                dto.MasterGender = (from emp in _VMSContext.IVRM_Master_Gender
                                    where emp.IVRMMG_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                    select new HR_MRFRequisitionDTO
                                    {
                                        IVRMMG_Id = emp.IVRMMG_Id,
                                        IVRMMG_GenderName = emp.IVRMMG_GenderName
                                    }).ToArray();

                dto.clientlist = (from emp in _VMSContext.ISM_Master_Client_DMO
                                  where emp.ISMMCLT_ActiveFlag == true && emp.MI_Id == dto.MI_Id
                                  select new HR_MRFRequisitionDTO
                                  {
                                      ISMMCLT_Id = emp.ISMMCLT_Id,
                                      ISMMCLT_ClientName = emp.ISMMCLT_ClientName
                                  }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
