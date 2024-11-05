using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGStudentFeeEnablePartialPaymentImpl : Interfaces.CLGStudentFeeEnablePartialPaymentInterface
    {
        public CollFeeGroupContext CollFeeGroupContext;

        public CLGStudentFeeEnablePartialPaymentImpl(CollFeeGroupContext objDbcontext)
        {
            CollFeeGroupContext = objDbcontext;
        }
        public CollegeOverallFeeStatusDTO GetYearList(int id)
        {

            CollegeOverallFeeStatusDTO data = new CollegeOverallFeeStatusDTO();
            try
            {
                data.yearlist = CollFeeGroupContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).Distinct().ToArray();
                data.sectionlist = CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == id && t.ACMS_ActiveFlag == true).OrderBy(t => t.ACMS_Order).ToArray();
                data.alldata = (from a in CollFeeGroupContext.Fee_Student_EnablePartialPayment_CollegeDMO
                                from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                from c in CollFeeGroupContext.AcademicYear                             
                                where (a.MI_Id == id && a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id )
                                select new CollegeOverallFeeStatusDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FSEPPC_RemarksDate = a.FSEPPC_RemarksDate,
                                    FSEPPC_ActiveFlag=a.FSEPPC_ActiveFlag,
                                    AMCST_Id = a.AMCST_Id,
                                    AMCST_FirstName = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "" ? "" : " " + b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "" || b.AMCST_MiddleName == "0" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "" || b.AMCST_LastName == "0" ? "" : " " + b.AMCST_LastName)).Trim(),                                  
                                    FSEPPC_Id = a.FSEPPC_Id,
                                    FSEPPC_Remarks = a.FSEPPC_Remarks
                                }
       ).OrderByDescending(t => t.FSEPPC_Id).ToArray();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            return data;
        }
        public CollegeOverallFeeStatusDTO get_courses(CollegeOverallFeeStatusDTO data)
        {

            try
            {

                data.courselist = (from a in CollFeeGroupContext.MasterCourseDMO
                                   from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true
                                   && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag == true && b.AMCO_Id == a.AMCO_Id)
                                   select new CollegeOverallFeeStatusDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName
                                   }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                data.fillmastergroup = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from f in CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == data.user_id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id)
                                        select new CollegeYearlyStatusReportDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                data.fillmasterhead = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in CollFeeGroupContext.FeeGroupClgDMO
                                       from c in CollFeeGroupContext.FeeHeadClgDMO
                                       from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new CollegeYearlyStatusReportDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

                data.fillinstallment = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from c in CollFeeGroupContext.FeeHeadClgDMO
                                        from d in CollFeeGroupContext.Clg_Fee_Installment_DMO
                                        from e in CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == e.FTI_Id)
                                        select new CollegeYearlyStatusReportDTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeOverallFeeStatusDTO get_branches(CollegeOverallFeeStatusDTO data)
        {
            try
            {
                var branchlist = (from a in CollFeeGroupContext.ClgMasterBranchDMO
                                  from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.AMB_ActiveFlag == true && a.AMB_Id == c.AMB_Id && b.AMCO_Id == data.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && b.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                  select new CollegeOverallFeeStatusDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_Order = a.AMB_Order,                                 
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeOverallFeeStatusDTO get_semisters(CollegeOverallFeeStatusDTO data)
        {
            try
            {
                data.semesterlist = (from a in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                     from b in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from d in CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == c.AMSE_Id && c.ACAYCBS_ActiveFlag == true && d.AMSE_ActiveFlg == true)
                                     select new CollegeOverallFeeStatusDTO
                                     {
                                         AMSE_Id = d.AMSE_Id,
                                         AMSE_SEMName = d.AMSE_SEMName,
                                         AMSE_SEMOrder = d.AMSE_SEMOrder

                                     }).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeOverallFeeStatusDTO get_student(CollegeOverallFeeStatusDTO data)
        {
            try
            {
                if (data.ACMS_Id > 0)
                {
                    data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                        from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                                        && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            ACYST_RollNo = a.ACYST_RollNo,                                           
                                            AMCST_MiddleName = b.AMCST_MiddleName,
                                            AMCST_LastName = b.AMCST_LastName,
                                            AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                            AMCST_AdmNo = b.AMCST_AdmNo,
                                            AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                        }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                }
                else
                {
                    data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                        from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                        && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                        select new CollegeStudentLedgerDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            ACYST_RollNo = a.ACYST_RollNo,
                                            AMCST_FirstName = b.AMCST_FirstName,
                                            AMCST_MiddleName = b.AMCST_MiddleName,
                                            AMCST_LastName = b.AMCST_LastName,
                                            AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                            AMCST_AdmNo = b.AMCST_AdmNo
                                        }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeOverallFeeStatusDTO savedata(CollegeOverallFeeStatusDTO data)
        {
            try
            {  
                Fee_Student_EnablePartialPayment_CollegeDMO obj = new Fee_Student_EnablePartialPayment_CollegeDMO();

                obj.MI_Id = data.MI_Id;
                obj.FSEPPC_Id = data.FSEPPC_Id;
                obj.FSEPPC_RemarksDate = data.FSEPPC_RemarksDate;
                obj.AMCST_Id = data.AMCST_Id;
                obj.ASMAY_Id = data.ASMAY_Id;
                obj.FSEPPC_ActiveFlag = true;
                obj.FSEPPC_Remarks = data.FSEPPC_Remarks;
                obj.FSEPPC_CreatedDate = DateTime.Now;
                CollFeeGroupContext.Add(obj);           

                var contactExists = CollFeeGroupContext.SaveChanges();
                if (contactExists >= 1)
                {
                    data.retrunMsg = "Add";
                }
                else
                {
                    data.retrunMsg = "false";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CollegeOverallFeeStatusDTO deactivate(CollegeOverallFeeStatusDTO dto)
        {
            dto.retrunMsg = "";
            try
            {               
                if (dto.FSEPPC_Id > 0)
                {
                    var result = CollFeeGroupContext.Fee_Student_EnablePartialPayment_CollegeDMO.Single(t => t.FSEPPC_Id == dto.FSEPPC_Id);

                    if (result.FSEPPC_ActiveFlag == true)
                    {
                        result.FSEPPC_ActiveFlag = false;
                    }
                    else if (result.FSEPPC_ActiveFlag == false)
                    {
                        result.FSEPPC_ActiveFlag = true;
                    }
                    CollFeeGroupContext.Update(result);
                    var flag = CollFeeGroupContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.FSEPPC_ActiveFlag == true)
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
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
    }
}
