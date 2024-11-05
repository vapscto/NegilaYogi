using AutoMapper;
using CollegeFeeService.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeOpeningBalanceImpl : CLGFeeOpeningBalanceInterface
    {
        public CollFeeGroupContext _CollFeeGroupContext;
        ILogger<CLGFeeOpeningBalanceImpl> _logbranch;
        public CLGFeeOpeningBalanceImpl(CollFeeGroupContext CollFeeGroupContext, ILogger<CLGFeeOpeningBalanceImpl> log)
        {
            _CollFeeGroupContext = CollFeeGroupContext;
            _logbranch = log;
        }
        public CLGFeeOpeningBalanceDTO getalldetails(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                data.yearlist = _CollFeeGroupContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();
                data.courselist = _CollFeeGroupContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _CollFeeGroupContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  getalldetails :" + ex.Message);
            }
            return data;
        }

        public CLGFeeOpeningBalanceDTO get_courses(CLGFeeOpeningBalanceDTO data)
        {
            try
            {

                data.courselist = (from a in _CollFeeGroupContext.MasterCourseDMO
                                   from b in _CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                //data.grouplist = (from a in _CollFeeGroupContext.FeeGroupClgDMO
                //                  from b in _CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                //                  from c in _CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                //                      //   from d in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                //                  where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id && c.User_Id == data.UserId && c.MI_ID == data.MI_Id) /* && d.AMCST_Id == data.AMCST_Id && a.fmg_id.Contains(data.fmg_id)*/
                //                  select new CLGFeeOpeningBalanceDTO
                //                  {
                //                      FMG_GroupName = a.FMG_GroupName,
                //                      FMG_Id = a.FMG_Id,
                //                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_courses :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO get_branches(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                var branchlist = (from a in _CollFeeGroupContext.ClgMasterBranchDMO
                                  from b in _CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
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
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_branches :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO get_semisters(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                var semisterlist = (from a in _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                    from b in _CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _CollFeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_semisters :" + ex.Message);
            }
            return data;
        }

        public CLGFeeOpeningBalanceDTO get_groups(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                //data.grouplist = (from a in _CollFeeGroupContext.FeeGroupClgDMO
                //                  from b in _CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                //                  from c in _CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                //                  from d in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                //                  from e in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                //                      //   from d in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                //                  where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id && c.User_Id == data.UserId && c.MI_ID == data.MI_Id && d.MI_Id==data.MI_Id && d.ASMAY_Id==data.ASMAY_Id && d.AMCO_Id==data.AMCO_Id && d.AMB_Id==data.AMB_Id && d.FCMA_Id==e.FCMA_Id && e.AMSE_Id==data.AMSE_Id && d.FMG_Id==a.FMG_Id) /* && d.AMCST_Id == data.AMCST_Id && a.fmg_id.Contains(data.fmg_id)*/
                //                  select new CLGFeeOpeningBalanceDTO
                //                  {
                //                      FMG_GroupName = a.FMG_GroupName,
                //                      FMG_Id = a.FMG_Id,
                //                  }).Distinct().ToArray();

                data.grouplist = (from a in _CollFeeGroupContext.FeeGroupClgDMO                                  from b in _CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                      //from c in _CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                  from d in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO                                  from e in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                      //   from d in _CollFeeGroupContext.Fee_College_Student_StatusDMO
                                  where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.FCMA_Id == e.FCMA_Id && e.AMSE_Id == data.AMSE_Id && d.FMG_Id == a.FMG_Id) /* && d.AMCST_Id == data.AMCST_Id && a.fmg_id.Contains(data.fmg_id)*/
                                  select new CLGFeeOpeningBalanceDTO                                  {                                      FMG_GroupName = a.FMG_GroupName,                                      FMG_Id = a.FMG_Id,                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_courses :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO get_heads(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                data.headlist = (from a in _CollFeeGroupContext.FeeHeadClgDMO
                                 from b in _CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                 from c in _CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                 from d in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                 from e in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 where (a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.FMG_Id == data.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMH_ActiveFlag == true && c.FMG_ID == b.FMG_Id && c.User_Id == data.UserId && c.MI_ID == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.FCMA_Id == e.FCMA_Id && e.AMSE_Id == data.AMSE_Id && d.FMG_Id == data.FMG_Id && d.FMH_Id == a.FMH_Id)
                                 select new CLGFeeOpeningBalanceDTO
                                 {
                                     FMH_Id = a.FMH_Id,
                                     FMH_FeeName = a.FMH_FeeName
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_heads :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO get_installments(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                data.installmentlist = (from a in _CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                        from b in _CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from c in _CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        from d in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                        from e in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        where (a.MI_ID == data.MI_Id && a.FMI_Id == b.FMI_Id && b.FMH_Id == data.FMH_Id && b.FMG_Id == data.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_ID == b.FMG_Id && c.User_Id == data.UserId && c.MI_ID == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.FCMA_Id == e.FCMA_Id && e.AMSE_Id == data.AMSE_Id && d.FMG_Id == data.FMG_Id && d.FMH_Id == data.FMH_Id && d.FTI_Id == a.FTI_Id)
                                        select new CLGFeeOpeningBalanceDTO
                                        {
                                            FTI_Id = a.FTI_Id,
                                            FTI_Name = a.FTI_Name
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_installments :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO get_students(CLGFeeOpeningBalanceDTO data)
        {
            try
            {

                var students_mapped = _CollFeeGroupContext.Fee_College_Master_Opening_BalanceDMO.Where(b => b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == data.FMG_Id && b.FMH_Id == data.FMH_Id && b.FTI_Id == data.FTI_Id).Distinct().ToList();
                data.saveddata = students_mapped.ToArray();
                //data.subjectlist = _CollFeeGroupContext.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_AttendanceFlag).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();


                var fillstudent = (from a in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                   from b in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from c in _CollFeeGroupContext.MasterCourseDMO
                                   from d in _CollFeeGroupContext.ClgMasterBranchDMO
                                   from e in _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                   from f in _CollFeeGroupContext.Adm_College_Master_SectionDMO
                                   where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag == 1)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                       AMCST_FirstName = a.AMCST_FirstName,
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
                _logbranch.LogInformation("CLGFeeOpeningBalance  get_students :" + ex.Message);
            }
            return data;
        }
        public CLGFeeOpeningBalanceDTO savedata(CLGFeeOpeningBalanceDTO data)        {            try            {                foreach (var x in data.sub_data)                {                    var result321 = _CollFeeGroupContext.Fee_College_Master_Opening_BalanceDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == x.AMCST_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id).ToList();                    if (result321.Count > 0)                    {                        var result = _CollFeeGroupContext.Fee_College_Master_Opening_BalanceDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCST_Id == x.AMCST_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id);                        result.FCMOB_EntryDate = DateTime.Now;                        result.FCMOB_Student_Due = x.FCMOB_Student_Due;                        result.FCMOB_Institution_Due = x.FCMOB_Institution_Due;                        result.FCMOB_RefundandableAmount = 0;                        result.FCMOB_ActiveFlg = true;                        result.User_Id = data.UserId;                        _CollFeeGroupContext.Update(result);                        var status_list = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == x.AMCST_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id).ToList();                        if (status_list.Count > 0)                        {                            var stu_status_obj = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == x.AMCST_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id).FirstOrDefault();     
                            if (Convert.ToInt64(x.FCMOB_Institution_Due) > 0)                            {                                if (stu_status_obj.FCSS_TotalCharges > 0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges - stu_status_obj.FCSS_OBExcessAmount - Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                else                                {                                    stu_status_obj.FCSS_TotalCharges = Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                if (stu_status_obj.FCSS_ToBePaid > 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid - stu_status_obj.FCSS_OBExcessAmount - Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                else                                {                                    stu_status_obj.FCSS_ToBePaid = Convert.ToInt64(x.FCMOB_Institution_Due);                                }


                            }                            if (Convert.ToInt64(x.FCMOB_Student_Due) > 0)                            {                                if (stu_status_obj.FCSS_TotalCharges > 0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges - stu_status_obj.FCSS_OBArrearAmount + Convert.ToInt64(x.FCMOB_Student_Due);                                }                                else                                {                                    stu_status_obj.FCSS_TotalCharges = Convert.ToInt64(x.FCMOB_Student_Due);                                }                                if (stu_status_obj.FCSS_ToBePaid > 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid - stu_status_obj.FCSS_OBArrearAmount + Convert.ToInt64(x.FCMOB_Student_Due);                                }                                else                                {                                    stu_status_obj.FCSS_ToBePaid = Convert.ToInt64(x.FCMOB_Student_Due);                                }                            }
                            if (Convert.ToInt64(x.FCMOB_Student_Due) == 0)
                            {
                                if (stu_status_obj.FCSS_TotalCharges > 0 && stu_status_obj.FCSS_TotalCharges== stu_status_obj.FCSS_OBArrearAmount && stu_status_obj.FCSS_PaidAmount==0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges - stu_status_obj.FCSS_OBArrearAmount;                                }

                                if (stu_status_obj.FCSS_ToBePaid > 0 && stu_status_obj.FCSS_ToBePaid == stu_status_obj.FCSS_OBArrearAmount && stu_status_obj.FCSS_PaidAmount == 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid - stu_status_obj.FCSS_OBArrearAmount;                                }
                            }

                            if (Convert.ToInt64(x.FCMOB_Institution_Due) == 0)
                            {
                                if (stu_status_obj.FCSS_TotalCharges > 0 && stu_status_obj.FCSS_TotalCharges == stu_status_obj.FCSS_OBExcessAmount && stu_status_obj.FCSS_PaidAmount == 0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges - stu_status_obj.FCSS_OBExcessAmount;                                }

                                if (stu_status_obj.FCSS_ToBePaid > 0 && stu_status_obj.FCSS_ToBePaid == stu_status_obj.FCSS_OBExcessAmount && stu_status_obj.FCSS_PaidAmount == 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid - stu_status_obj.FCSS_OBExcessAmount;                                }
                            }

                            stu_status_obj.FCSS_OBArrearAmount = Convert.ToInt64(x.FCMOB_Student_Due);                            stu_status_obj.FCSS_OBExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);
                            // stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid + Convert.ToInt64(x.FCMOB_Student_Due);
                            // stu_status_obj.FCSS_RunningExcessAmount = stu_status_obj.FCSS_RunningExcessAmount + Convert.ToInt64(x.FCMOB_Institution_Due);
                            _CollFeeGroupContext.Update(stu_status_obj);                        }                        else if (status_list.Count == 0)                        {                            var FCMAS_Id = (from a in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO                                            from b in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise                                            from c in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO                                            where (c.AMCST_Id == x.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && a.FTI_Id == data.FTI_Id)                                            select b).ToList();                            Fee_College_Student_StatusDMO status_obj = new Fee_College_Student_StatusDMO();                            status_obj.MI_Id = data.MI_Id;                            status_obj.ASMAY_Id = data.ASMAY_Id;                            status_obj.AMCST_Id = x.AMCST_Id;                            status_obj.FMG_Id = data.FMG_Id;                            status_obj.FMH_Id = data.FMH_Id;                            status_obj.FTI_Id = data.FTI_Id;                            status_obj.FCMAS_Id = FCMAS_Id[0].FCMAS_Id;                            status_obj.FCSS_OBArrearAmount = Convert.ToInt64(x.FCMOB_Student_Due);                            status_obj.FCSS_OBExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);                            status_obj.FCSS_CurrentYrCharges = 0;                            status_obj.FCSS_TotalCharges = 0;                            status_obj.FCSS_ConcessionAmount = 0;                            status_obj.FCSS_WaivedAmount = 0;
                            // status_obj.FCSS_ToBePaid = 0;
                            status_obj.FCSS_ToBePaid = Convert.ToInt64(x.FCMOB_Student_Due);                            status_obj.FCSS_PaidAmount = 0;                            status_obj.FCSS_RefundableAmount = 0;                            status_obj.FCSS_ExcessPaidAmount = 0;                            status_obj.FCSS_ExcessAmountAdjusted = 0;                            status_obj.FCSS_RunningExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);                            status_obj.FCSS_AdjustedAmount = 0;                            status_obj.FCSS_RebateAmount = 0;                            status_obj.FCSS_FineAmount = 0;                            status_obj.FCSS_RefundAmount = 0;                            status_obj.FCSS_RefundAmountAdjusted = 0;                            status_obj.FCSS_NetAmount = 0;
                            //status_obj.FCSS_ChequeBounceAmount = 0;
                            status_obj.FCSS_ChequeBounceFlg = false;                            status_obj.FCSS_ArrearFlag = false;                            status_obj.FCSS_RefundOverFlag = false;                            status_obj.FCSS_ActiveFlag = true;                            status_obj.User_Id = data.UserId;                            _CollFeeGroupContext.Add(status_obj);                        }                    }                    else                    {                        Fee_College_Master_Opening_BalanceDMO objpge1 = new Fee_College_Master_Opening_BalanceDMO();                        objpge1.MI_Id = data.MI_Id;                        objpge1.AMCST_Id = x.AMCST_Id;                        objpge1.ASMAY_Id = data.ASMAY_Id;                        objpge1.FMH_Id = data.FMH_Id;                        objpge1.FMG_Id = data.FMG_Id;                        objpge1.FTI_Id = data.FTI_Id;                        objpge1.FCMOB_EntryDate = DateTime.Now;                        objpge1.FCMOB_Student_Due = x.FCMOB_Student_Due;                        objpge1.FCMOB_Institution_Due = x.FCMOB_Institution_Due;                        objpge1.FCMOB_RefundandableAmount = 0;                        objpge1.FCMOB_ActiveFlg = true;                        objpge1.User_Id = data.UserId;                        _CollFeeGroupContext.Add(objpge1);

                        //var FCMAS_Idnew = (from a in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                        //                from b in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                        //                from c in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                        //                where (c.AMCST_Id == x.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && a.FTI_Id == data.FTI_Id && b.AMSE_Id == data.AMSE_Id)
                        //                select b).ToList();

                        var status_list = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == x.AMCST_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id).ToList();                        if (status_list.Count > 0)                        {                            var stu_status_obj = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == x.AMCST_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.FTI_Id == data.FTI_Id).FirstOrDefault();                            stu_status_obj.FCSS_OBArrearAmount = Convert.ToInt64(x.FCMOB_Student_Due);
                            ////
                            //if(Convert.ToInt64(x.FCMOB_Student_Due)>0)
                            //{
                            //    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges + Convert.ToInt64(x.FCMOB_Student_Due);
                            //    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid + Convert.ToInt64(x.FCMOB_Student_Due);

                            //}
                            if (Convert.ToInt64(x.FCMOB_Institution_Due) > 0)                            {                                if (stu_status_obj.FCSS_TotalCharges > 0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges - Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                else                                {                                    stu_status_obj.FCSS_TotalCharges = Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                if (stu_status_obj.FCSS_ToBePaid > 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid - Convert.ToInt64(x.FCMOB_Institution_Due);                                }                                else                                {                                    stu_status_obj.FCSS_ToBePaid = Convert.ToInt64(x.FCMOB_Institution_Due);                                }


                            }                            if (Convert.ToInt64(x.FCMOB_Student_Due) > 0)                            {                                if (stu_status_obj.FCSS_TotalCharges > 0)                                {                                    stu_status_obj.FCSS_TotalCharges = stu_status_obj.FCSS_TotalCharges  + Convert.ToInt64(x.FCMOB_Student_Due);                                }                                else                                {                                    stu_status_obj.FCSS_TotalCharges = Convert.ToInt64(x.FCMOB_Student_Due);                                }                                if (stu_status_obj.FCSS_ToBePaid > 0)                                {                                    stu_status_obj.FCSS_ToBePaid = stu_status_obj.FCSS_ToBePaid  + Convert.ToInt64(x.FCMOB_Student_Due);                                }                                else                                {                                    stu_status_obj.FCSS_ToBePaid = Convert.ToInt64(x.FCMOB_Student_Due);                                }                            }                            stu_status_obj.FCSS_OBExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);                            stu_status_obj.FCSS_RunningExcessAmount = stu_status_obj.FCSS_RunningExcessAmount + Convert.ToInt64(x.FCMOB_Institution_Due);                            _CollFeeGroupContext.Update(stu_status_obj);                        }                        else if (status_list.Count == 0)                        {                            var FCMAS_Id = (from a in _CollFeeGroupContext.Clg_Fee_AmountEntry_DMO                                            from b in _CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise                                            from c in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO                                            where (c.AMCST_Id == x.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && a.FTI_Id == data.FTI_Id)                                            select b).ToList();                            Fee_College_Student_StatusDMO status_obj = new Fee_College_Student_StatusDMO();                            status_obj.MI_Id = data.MI_Id;                            status_obj.ASMAY_Id = data.ASMAY_Id;                            status_obj.AMCST_Id = x.AMCST_Id;                            status_obj.FMG_Id = data.FMG_Id;                            status_obj.FMH_Id = data.FMH_Id;                            status_obj.FTI_Id = data.FTI_Id;                            status_obj.FCMAS_Id = FCMAS_Id[0].FCMAS_Id;                            status_obj.FCSS_OBArrearAmount = Convert.ToInt64(x.FCMOB_Student_Due);                            status_obj.FCSS_OBExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);                            status_obj.FCSS_CurrentYrCharges = 0;                            status_obj.FCSS_TotalCharges = 0;                            status_obj.FCSS_ConcessionAmount = 0;                            status_obj.FCSS_WaivedAmount = 0;                            status_obj.FCSS_ToBePaid = 0;                            status_obj.FCSS_PaidAmount = 0;                            status_obj.FCSS_RefundableAmount = 0;                            status_obj.FCSS_ExcessPaidAmount = 0;                            status_obj.FCSS_ExcessAmountAdjusted = 0;                            status_obj.FCSS_RunningExcessAmount = Convert.ToInt64(x.FCMOB_Institution_Due);                            status_obj.FCSS_AdjustedAmount = 0;                            status_obj.FCSS_RebateAmount = 0;                            status_obj.FCSS_FineAmount = 0;                            status_obj.FCSS_RefundAmount = 0;                            status_obj.FCSS_RefundAmountAdjusted = 0;                            status_obj.FCSS_NetAmount = 0;
                            //status_obj.FCSS_ChequeBounceAmount = 0;
                            status_obj.FCSS_ChequeBounceFlg = false;                            status_obj.FCSS_ArrearFlag = false;                            status_obj.FCSS_RefundOverFlag = false;                            status_obj.FCSS_ActiveFlag = true;                            status_obj.User_Id = data.UserId;                            _CollFeeGroupContext.Add(status_obj);                        }                    }                }                var contactExists = _CollFeeGroupContext.SaveChanges();                if (contactExists >= 1)                {                    data.returnval = true;                }                else                {                    data.returnval = false;                }            }            catch (Exception ex)            {                _logbranch.LogInformation("CLGFeeOpeningBalance savedata :" + ex.Message);            }            return data;        }
       
        public CLGFeeOpeningBalanceDTO Deletedetails(CLGFeeOpeningBalanceDTO data)
        {
            try
            {
                // var result = _CollFeeGroupContext.Adm_College_CLGFeeOpeningBalanceDMO.Single(t => t.ACASMP_Id == data.ACASMP_Id);
                //if (result.ACASMP_ActiveFlag)
                //{
                //    result.ACASMP_ActiveFlag = false;
                //    result.UpdatedDate = DateTime.Now;
                //}
                //else if (!result.ACASMP_ActiveFlag)
                //{
                //    result.ACASMP_ActiveFlag = true;
                //    result.UpdatedDate = DateTime.Now;
                //}
                //_CollFeeGroupContext.Update(result);
                var contactExists = _CollFeeGroupContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeOpeningBalance Deletedetails :" + ex.Message);
            }

            return data;
        }
    }
}
