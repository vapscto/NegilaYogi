using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class AmountEntryReportImpl : AmountEntryReportInterface
    {
       
        public CollFeeGroupContext _clgfee;
        readonly ILogger<AmountEntryReportImpl> _logger;

        public AmountEntryReportImpl(CollFeeGroupContext _clgfeecon,ILogger<AmountEntryReportImpl> log)
        {
            _logger = log;
            _clgfee = _clgfeecon;
        }


        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {

            try
            {
                var year = _clgfee.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).ToList();
                data.yearlst = year.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.quota = _clgfee.Clg_Adm_College_QuotaDMO.Where(q => q.MI_Id == data.MI_Id && q.ACQ_ActiveFlg == true).Distinct().ToArray();

                data.category = _clgfee.mastercategory.Where(c => c.ACMC_ActiveFlag == true && c.MI_Id == data.MI_Id).Distinct().ToArray();

                data.semisterlist = (from a in _clgfee.CLG_Adm_Master_SemesterDMO
                                     from b in _clgfee.CLG_Fee_College_Master_Amount_Semesterwise
                                     where (a.AMSE_Id == b.AMSE_Id && a.AMSE_ActiveFlg == true && b.FCMAS_ActiveFlg == true && b.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id)
                                     select new CLG_Adm_Master_SemesterDMO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName
                                     }
                                   ).Distinct().ToArray();

                data.grouplist = (from a in _clgfee.FeeGroupClgDMO
                                from b in _clgfee.FeeYearGroupDMO
                                where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                select new FeeGroupDMO
                                {
                                    FMG_Id = a.FMG_Id,
                                    FMG_GroupName = a.FMG_GroupName
                                }
                    ).Distinct().ToArray();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return data;
        }

        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {

            try
            {
                var branchlist = (from a in _clgfee.ClgMasterBranchDMO
                                  from b in _clgfee.CLG_Adm_College_AY_CourseDMO
                                  from c in _clgfee.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && b.AMCO_Id==data.AMCO_Id)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_branches :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            try
            {

                //data.courselist = (from a in _clgfee.MasterCourseDMO
                //                   from b in _clgfee.CLG_Adm_College_AY_CourseDMO
                //                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                //                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                data.courselist = (from a in _clgfee.MasterCourseDMO
                                   join b in _clgfee.ClgMasterCourseCategoryMapDMO on a.AMCO_Id equals b.AMCO_Id
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.AMCOCM_ActiveFlg==true && a.AMCO_ActiveFlag==true && b.AMCOC_Id==data.AMCOC_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }


        public CollegeConcessionDTO radiobtndata(CollegeConcessionDTO data)
        {
            try
            {

                data.savedrecord = (from a in _clgfee.ClgMasterBranchDMO
                                    from b in _clgfee.Clg_Fee_AmountEntry_DMO
                                    from c in _clgfee.CLG_Fee_College_Master_Amount_Semesterwise
                                    from d in _clgfee.FeeHeadClgDMO
                                    from e in _clgfee.Clg_Fee_Installments_Yearly_DMO
                                    from f in _clgfee.CLG_Adm_Master_SemesterDMO
                                    where (a.AMB_Id == b.AMB_Id && b.FMH_Id == d.FMH_Id && e.FTI_Id == b.FTI_Id && b.FCMA_Id == c.FCMA_Id && c.AMSE_Id == f.AMSE_Id
                                    && data.AMB_Ids.Contains(b.AMB_Id) && b.AMCO_Id == data.AMCO_Id && b.FMG_Id == data.FMG_Id && data.AMSE_Ids.Contains(c.AMSE_Id) && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && c.FCMAS_Amount > 0)
                                    select new CollegeConcessionDTO
                                    {
                                        AMB_BranchName = a.AMB_BranchName,
                                        FMH_FeeName = d.FMH_FeeName,
                                        FTI_Name = e.FTI_Name,
                                        FCSA_Amount = c.FCMAS_Amount,
                                        AMSE_SEMName = f.AMSE_SEMName
                                    }
                                                  ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }


    }
}
