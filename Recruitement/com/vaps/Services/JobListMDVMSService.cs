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
    public class JobListMDVMSService : Interfaces.JobListMDVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public JobListMDVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
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

        public HR_MRFRequisitionDTO getRecordById(int id)
        {
            HR_MRFRequisitionDTO dto = new HR_MRFRequisitionDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_MRFRequisitionDMO> lorg = new List<HR_MRFRequisitionDMO>();

                dto.VMSEditValue = (from a in _VMSContext.HR_MRFRequisitionDMO
                                    from b in _VMSContext.HR_Master_PositionDMO
                                    from c in _VMSContext.HR_Master_Department
                                    from d in _VMSContext.HR_Master_PriorityDMO
                                    from e in _VMSContext.HR_Master_PostionTypeDMO
                                    from g in _VMSContext.HR_Master_Course
                                    from h in _VMSContext.IVRM_Master_Gender
                                    from i in _VMSContext.ApplicationUserDMO
                                    where (a.HRMRFR_Id.Equals(id) && a.HRMP_Id == b.HRMP_Id && a.MI_Id == b.MI_Id && a.HRMD_Id == c.HRMD_Id && a.MI_Id == c.MI_Id && a.HRMPR_Id == d.HRMPR_Id && a.MI_Id == d.MI_Id && a.HRMPT_Id == e.HRMPT_Id && a.MI_Id == e.MI_Id && a.HRMC_Id == g.HRMC_Id && a.MI_Id == g.MI_Id && a.IVRMMG_Id == h.IVRMMG_Id && a.MI_Id == h.MI_Id && a.HRMRFR_CreatedBy == i.Id)
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
                                        UserName = i.UserName,
                                        CreatedDate = a.CreatedDate,
                                        HRMRFR_PayScaleFrom = a.HRMRFR_PayScaleFrom,
                                        HRMRFR_PayScaleTo = a.HRMRFR_PayScaleTo,
                                        HRMRFR_PositionFilled = a.HRMRFR_PositionFilled,
                                        HRMRFR_HRComment = a.HRMRFR_HRComment,
                                        HRMRFR_JobLocation = a.HRMRFR_JobLocation,
                                        HRMRFR_MDComment = a.HRMRFR_MDComment
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
                                  from e in _VMSContext.ApplicationUserDMO
                                  where (a.HRMP_Id == b.HRMP_Id && a.HRMPR_Id == d.HRMPR_Id && a.HRMRFR_ManagerFlag == true && a.HRMRFR_CreatedBy == e.Id)
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
                                      UserName = e.UserName,
                                      HRMRFR_JobLocation = a.HRMRFR_JobLocation
                                  };
                dto.VMSMRFList = mrfdatalist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
