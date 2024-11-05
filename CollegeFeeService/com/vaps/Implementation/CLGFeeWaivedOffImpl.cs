using CollegeFeeService.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeWaivedOffImpl : CLGFeeWaivedOffInterface
    {
        public CollFeeGroupContext _CollFeeGroupContext;
        ILogger<CLGFeeWaivedOffImpl> _logbranch;
        public CLGFeeWaivedOffImpl(CollFeeGroupContext CollFeeGroupContext, ILogger<CLGFeeWaivedOffImpl> log)
        {
            _CollFeeGroupContext = CollFeeGroupContext;
            _logbranch = log;
        }
        public CLGFeeWaivedOffDTO getalldetails(CLGFeeWaivedOffDTO data)
        {
            try
            {
                data.yearlist = _CollFeeGroupContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();
                data.courselist = _CollFeeGroupContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _CollFeeGroupContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.alldata = (from a in _CollFeeGroupContext.Fee_College_Student_WaivedOffDMO
                                 from b in _CollFeeGroupContext.FeeHeadClgDMO
                                 from d in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                 from f in _CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FMH_Id == b.FMH_Id && a.AMCST_Id == d.AMCST_Id && a.FTI_Id == f.FTI_Id  && a.MI_Id == data.MI_Id && a.User_Id == data.UserId)//&& a.ASMAY_Id == data.ASMAY_Id
                                select new CLGFeeWaivedOffDTO
                                 {
                                     FCSWO_Id = a.FCSWO_Id,
                                     AMCST_FirstName = ((d.AMCST_FirstName == null || d.AMCST_FirstName == "" ? "" : " " + d.AMCST_FirstName) + (d.AMCST_MiddleName == null || d.AMCST_MiddleName == "" || d.AMCST_MiddleName == "0" ? "" : " " + d.AMCST_MiddleName) + (d.AMCST_LastName == null || d.AMCST_LastName == "" || d.AMCST_LastName == "0" ? "" : " " + d.AMCST_LastName)).Trim(),                                     
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Name = f.FTI_Name,
                                     FCSWO_WaivedOffAmount = a.FCSWO_WaivedOffAmount,
                                     FCSWO_Date = a.FCSWO_Date,


                                 }).Distinct().OrderByDescending(t => t.FCSWO_Id).ToList().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeWaivedOff  getalldetails :" + ex.Message);
            }
            return data;
        }
        public CLGFeeWaivedOffDTO get_students(CLGFeeWaivedOffDTO data)
        {
            try
            {
                var fillstudent = (from a in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                   from b in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from c in _CollFeeGroupContext.MasterCourseDMO
                                   from d in _CollFeeGroupContext.ClgMasterBranchDMO
                                   from e in _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                   from f in _CollFeeGroupContext.Adm_College_Master_SectionDMO
                                   where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag==1)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       // AMCST_FirstName = a.AMCST_FirstName,
                                       AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName) + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                       AMCST_MiddleName = a.AMCST_MiddleName,
                                       AMCST_LastName = a.AMCST_LastName,
                                       AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                       AMCST_AdmNo = a.AMCST_AdmNo,
                                       ACYST_RollNo = b.ACYST_RollNo,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMSE_SEMName = e.AMSE_SEMName,
                                       ACMS_SectionName = f.ACMS_SectionName,
                                       AMCST_FatherName = a.AMCST_FatherName,
                                       AMCST_DOB = a.AMCST_DOB,
                                       AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo)
                                   }
               ).Distinct().OrderBy(t => t.ACYST_RollNo).ToList();
                data.studentlist = fillstudent.ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeWaivedOff  get_students :" + ex.Message);
            }

            return data;
        }

        public CLGFeeWaivedOffDTO get_groups(CLGFeeWaivedOffDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.grouplist = (from a in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                                  from b in _CollFeeGroupContext.FeeGroupClgDMO
                                  from c in _CollFeeGroupContext.FeeHeadClgDMO
                                  where (a.FMG_Id == b.FMG_Id && c.FMH_Id == a.FMH_Id && c.FMH_RefundFlag == refundflag && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.User_Id == b.user_id && a.User_Id == data.UserId && a.FCSS_WaivedAmount == 0)
                                  select new CLGFeeWaivedOffDTO
                                  {
                                      FMG_Id = b.FMG_Id,
                                      FMG_GroupName = b.FMG_GroupName,
                                  }
                              ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeWaivedOff  get_groups :" + ex.Message);
            }

            return data;
        }

        public CLGFeeWaivedOffDTO get_heads(CLGFeeWaivedOffDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.headlist = (from a in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                                 from b in _CollFeeGroupContext.FeeHeadClgDMO
                                 from c in _CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && data.multiplegroup.ToString().Contains(Convert.ToString(a.FMG_Id)) && a.User_Id == b.user_id && a.User_Id == data.UserId && b.FMH_RefundFlag == refundflag && a.FCSS_WaivedAmount == 0)
                                 select new CLGFeeWaivedOffDTO
                                 {
                                     FMH_Id = b.FMH_Id,
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Id = c.FTI_Id,
                                     FTI_Name = c.FTI_Name,
                                     FMG_Id = a.FMG_Id,
                                     FCMAS_Id = a.FCMAS_Id,
                                     FCSS_TotalCharges = a.FCSS_TotalCharges,
                                     FCSS_ToBePaid = a.FCSS_ToBePaid,
                                     FCSS_PaidAmount = a.FCSS_PaidAmount
                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeWaivedOffDTO savedata(CLGFeeWaivedOffDTO data)
        {
            try
            {
                // FeeStudentWaivedOffDMO feepge = Mapper.Map<FeeStudentWaivedOffDMO>(data);
                if (data.FCSWO_Id > 0)
                {
                    var lorg = _CollFeeGroupContext.Fee_College_Student_WaivedOffDMO.Single(t => t.FCSWO_Id == Convert.ToInt64(data.FCSWO_Id));

                    if (lorg != null)
                    {
                        var lorg1 = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Single(t => t.AMCST_Id == lorg.AMCST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.ASMAY_Id == lorg.ASMAY_Id && t.MI_Id == lorg.MI_Id && t.User_Id == lorg.User_Id);
                        if (lorg1.FCSS_ExcessPaidAmount != 0)
                        {
                            var gethead = _CollFeeGroupContext.FeeHeadClgDMO.Single(t => t.FMH_Id == lorg1.FMH_Id);
                            if (gethead.FMH_RefundFlag == true)
                            {
                                if (lorg1.FCSS_RefundableAmount < lorg1.FCSS_ExcessPaidAmount)
                                {
                                    data.returnduplicatestatus = "adjusted";
                                }
                            }
                            else
                            {
                                if (lorg1.FCSS_RunningExcessAmount < lorg1.FCSS_ExcessPaidAmount)
                                {
                                    data.returnduplicatestatus = "adjusted";
                                }
                            }
                        }
                        if (data.returnduplicatestatus != "adjusted")
                        {
                            var contactExists = _CollFeeGroupContext.Database.ExecuteSqlCommand("CLG_Fee_Student_Waived_Off_Edit @p0, @p1", data.FCSWO_Id, data.checkedlist[0].FCSWO_WaivedOffAmount);
                            if (contactExists > 0)
                            {

                                data.returnduplicatestatus = "Updated";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Updated";
                            }
                        }

                    }
                }
                else
                {
                    int contact = 0;
                    foreach (var headlst in data.checkedlist)
                    {

                        var contactAdd = _CollFeeGroupContext.Database.ExecuteSqlCommand("CLG_Fee_Student_Waived_Off_insert @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9", data.MI_Id, data.ASMAY_Id, data.AMCST_Id, headlst.FMG_Id, headlst.FMH_Id, headlst.FTI_Id, headlst.FCMAS_Id, headlst.FCSWO_WaivedOffAmount, data.FCSWO_Date, data.UserId);
                        contact = contact + contactAdd;
                        // }

                    }
                    if (contact >= 2)
                    {
                        data.returnduplicatestatus = "Saved";
                    }
                    else
                        data.returnduplicatestatus = "not Saved";

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CLGFeeWaivedOffDTO EditDetails(CLGFeeWaivedOffDTO data)
        {
           
            try
            {
               
                var result = _CollFeeGroupContext.Fee_College_Student_WaivedOffDMO.Single(t => t.FCSWO_Id == data.FCSWO_Id);
                data.FCSWO_Id = result.FCSWO_Id;
                data.MI_Id = result.MI_Id;
                data.AMCST_Id = result.AMCST_Id;
                data.ASMAY_Id = result.ASMAY_Id;
                data.FCSWO_Date = result.FCSWO_Date;
                data.FMG_Id = result.FMG_Id;
                data.FMH_Id = result.FMH_Id;
                data.FTI_Id = result.FTI_Id;
                data.FCMAS_Id = result.FCMAS_Id;
                data.UserId = result.User_Id;
                data.FCSWO_WaivedOffAmount = result.FCSWO_WaivedOffAmount;
                data.filterrefund = _CollFeeGroupContext.FeeHeadClgDMO.Single(t => t.FMH_Id == data.FMH_Id).FMH_RefundFlag ? "Refunable" : "NonRefunable";

                var result1 = _CollFeeGroupContext.Adm_College_Yearly_StudentDMO.FirstOrDefault(t => t.AMCST_Id == data.AMCST_Id && t.ACYST_ActiveFlag==1);
                data.AMCO_Id = result1.AMCO_Id;
                data.AMB_Id = result1.AMB_Id;
                data.AMSE_Id = result1.AMSE_Id;
              
                data.grouplist = (from a in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                                  from b in _CollFeeGroupContext.FeeGroupClgDMO
                                  where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.FMG_Id == data.FMG_Id && a.User_Id == b.user_id && a.User_Id == data.UserId)
                                  select new CLGFeeWaivedOffDTO
                                  {
                                      FMG_Id = b.FMG_Id,
                                      FMG_GroupName = b.FMG_GroupName,
                                  }).Distinct().ToArray();
                data.headlist = (from a in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                                 from b in _CollFeeGroupContext.FeeHeadClgDMO
                                 from c in _CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && a.FTI_Id == data.FTI_Id && a.FCMAS_Id == data.FCMAS_Id && a.User_Id == b.user_id && a.User_Id == data.UserId)
                                 select new CLGFeeWaivedOffDTO
                                 {
                                     FMH_Id = b.FMH_Id,
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Id = c.FTI_Id,
                                     FTI_Name = c.FTI_Name,
                                     FMG_Id = a.FMG_Id,
                                     FCMAS_Id = a.FCMAS_Id,
                                     FCSS_TotalCharges = a.FCSS_TotalCharges,
                                     FCSS_ToBePaid = a.FCSS_ToBePaid + (a.FCSS_WaivedAmount - a.FCSS_ExcessPaidAmount),
                                     FCSS_PaidAmount = a.FCSS_PaidAmount

                                 }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                //_logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CLGFeeWaivedOffDTO DeletRecord(CLGFeeWaivedOffDTO data)
        {
            try
            {
                var lorg = _CollFeeGroupContext.Fee_College_Student_WaivedOffDMO.Single(t => t.FCSWO_Id == Convert.ToInt64(data.FCSWO_Id));
                if (lorg != null)
                {
                    var lorg1 = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Single(t => t.AMCST_Id == lorg.AMCST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.ASMAY_Id == lorg.ASMAY_Id && t.MI_Id == lorg.MI_Id && t.User_Id == lorg.User_Id);
                    if (lorg1.FCSS_ExcessPaidAmount != 0)
                    {
                        var gethead = _CollFeeGroupContext.FeeHeadClgDMO.Single(t => t.FMH_Id == lorg1.FMH_Id);
                        if (gethead.FMH_RefundFlag == true)
                        {
                            if (lorg1.FCSS_RefundableAmount < lorg1.FCSS_ExcessPaidAmount)
                            {
                                data.returnduplicatestatus = "adjusted";
                            }
                        }
                        else
                        {
                            if (lorg1.FCSS_RunningExcessAmount < lorg1.FCSS_ExcessPaidAmount)
                            {
                                data.returnduplicatestatus = "adjusted";
                            }
                        }

                    }
                    if (data.returnduplicatestatus != "adjusted")
                    {
                        var contactExists = _CollFeeGroupContext.Database.ExecuteSqlCommand("CLG_Fee_Student_Waived_Off_Delete @p0", data.FCSWO_Id);
                        if (contactExists > 0)
                        {

                            data.returnduplicatestatus = "success";
                        }
                        else
                        {
                            data.returnduplicatestatus = "fail";
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
