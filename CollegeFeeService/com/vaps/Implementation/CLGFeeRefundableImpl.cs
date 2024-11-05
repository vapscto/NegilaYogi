using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using CollegeFeeService.com.vaps.Interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.College.Fees;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeRefundableImpl : CLGFeeRefundableInterface
    {
        private static ConcurrentDictionary<string, CLGFeeRefundableDTO> _login =
        new ConcurrentDictionary<string, CLGFeeRefundableDTO>();
        public DomainModelMsSqlServerContext _context;
        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        public CLGFeeRefundableImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
        }
        public CLGFeeRefundableDTO getdata(CLGFeeRefundableDTO id)
        {
            try
            {

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == id.MI_Id && t.IMN_Flag == "Refund").ToList();
                // id.transnumbconfigurationsettingsss = masnum.ToArray();
               

                foreach (var ab in masnum)
                {
                    if (ab.IMN_AutoManualFlag == "Auto")
                    {
                        Master_NumberingDTO transnumbconfigurationsettingsss = Mapper.Map<Master_NumberingDTO>(ab);

                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        transnumbconfigurationsettingsss.MI_Id = id.MI_Id;
                        transnumbconfigurationsettingsss.ASMAY_Id = id.ASMAY_Id;
                        id.FCR_RefundNo = a.GenerateNumber(transnumbconfigurationsettingsss);
                      
                    }
                    id.IMN_AutoManualFlag = ab.IMN_AutoManualFlag;
                }
                List<MasterAcademic> year = new List<MasterAcademic>();

                year = _YearlyFeeGroupMappingContext.AcademicYear.Where(t=>t.MI_Id== id.MI_Id && t.Is_Active==true).ToList();
                id.fillyear = year.Distinct().ToArray();

                List<MasterAcademic> currentYear = new List<MasterAcademic>();
                currentYear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true && t.ASMAY_Id==id.ASMAY_Id).ToList();
                id.currentYear = currentYear.ToArray();


                id.courselist = _YearlyFeeGroupMappingContext.MasterCourseDMO.Where(a => a.MI_Id == id.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                id.branchlist = _YearlyFeeGroupMappingContext.ClgMasterBranchDMO.Where(a => a.MI_Id == id.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                id.semisterlist = _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == id.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                id.sectionlist = _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == id.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                id.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_RefundDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.FMH_Id == c.FMH_Id && b.AMCST_Id == d.AMCST_Id && d.ASMAY_Id == id.ASMAY_Id && a.MI_Id == id.MI_Id && b.AMCST_SOL == "S"
                                        && b.AMCST_ActiveFlag == true && d.ACYST_ActiveFlag == 1 && a.FCR_RefundFlag.Equals("true") && a.User_Id == id.userid)
                                        select new CLGFeeRefundableDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = b.AMCST_FirstName,
                                            AMCST_MiddleName = b.AMCST_MiddleName,
                                            AMCST_LastName = b.AMCST_LastName,
                                            FMH_FeeName = c.FMH_FeeName,
                                            FCR_RefundAmount = a.FCR_RefundAmount,
                                            FCR_RefundNo=a.FCR_RefundNo,
                                            FCR_Date = a.FCR_Date,
                                            FCR_Id = a.FCR_Id
                                        }).OrderByDescending(t => t.FCR_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return id;
        }

        public CLGFeeRefundableDTO deleterec(CLGFeeRefundableDTO id)
        {
            var lorg = _YearlyFeeGroupMappingContext.Fee_College_RefundDMO.Single(t => t.FCR_Id == id.FCR_Id);

            try
            {
                if (lorg != null)
                {
                    var lorg1 = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.AMCST_Id == lorg.AMCST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.User_Id == lorg.User_Id);

                    var updateStudentStatus = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.FCSS_Id == lorg1.FCSS_Id);

                    if (updateStudentStatus.FCSS_RefundAmount >= Convert.ToInt64(lorg.FCR_RefundAmount))
                    {
                        var gethead = _YearlyFeeGroupMappingContext.FeeHeadClgDMO.Single(t => t.FMH_Id == lorg1.FMH_Id);
                        if (gethead.FMH_RefundFlag == true)
                        {
                            updateStudentStatus.FCSS_RefundableAmount = updateStudentStatus.FCSS_RefundableAmount + Convert.ToInt64(lorg.FCR_RefundAmount);

                        }
                        else
                        {
                            updateStudentStatus.FCSS_RunningExcessAmount = updateStudentStatus.FCSS_RunningExcessAmount + Convert.ToInt64(lorg.FCR_RefundAmount);

                        }
                        updateStudentStatus.FCSS_RefundAmount = updateStudentStatus.FCSS_RefundAmount - Convert.ToInt64(lorg.FCR_RefundAmount);
                        _YearlyFeeGroupMappingContext.Update(updateStudentStatus);

                        lorg.FCR_RefundFlag = "False";
                        _YearlyFeeGroupMappingContext.Update(lorg);
                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                        if (contactExists > 1)
                        {

                            id.returnVal = true;
                        }
                        else
                        {
                            id.returnVal = false;

                        }
                    }
                    else
                    {
                        id.returnVal = false;
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        public CLGFeeRefundableDTO getdatastuacad(CLGFeeRefundableDTO data)
        {
            try
            {

              //  data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
              //                      from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
              //                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag==1 && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag==1)
              //                      select new CollegeFeeTransactionDTO
              //                      {
              //                          AMCST_Id = a.AMST_Id,
              //                          AMCST_FirstName = a.AMST_FirstName,
              //                          AMCST_MiddleName = a.AMST_MiddleName,
              //                          AMCST_LastName = a.AMST_LastName,
              //                      }
              //).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

            // throw new NotImplementedException();
        }

        public CLGFeeRefundableDTO GetStudentListByYear(CLGFeeRefundableDTO data)
        {

          //  CLGFeeRefundableDTO stu = new CLGFeeRefundableDTO();
            try
            {

                //List<School_M_Class> clslst = new List<School_M_Class>();
                //clslst = _YearlyFeeGroupMappingContext.admissioncls.Where(m => m.MI_Id == data.MI_Id && m.ASMCL_ActiveFlag == true).ToList();
                //stu.fillclass = clslst.ToArray();
                data.courselist = (from a in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                   from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

            // throw new NotImplementedException();
        }

        public CLGFeeRefundableDTO GetSection(CLGFeeRefundableDTO data)
        {
            try
            {


                //data.fillsection = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                    from b in _YearlyFeeGroupMappingContext.school_M_Section
                //                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_ID && b.ASMC_ActiveFlag == 1)
                //                    select b).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                //new FeeStudentAdjustmentDTO
                //{
                //    ASMS_Id = a.ASMS_Id,
                //    ASMC_SectionName = b.ASMC_SectionName,
                //}
                var branchlist = (from a in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                  from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
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
                Console.Write(ex.Message);
            }
            return data;
        }

        public CLGFeeRefundableDTO get_semisters(CLGFeeRefundableDTO data)
        {
            try
            {
                var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                    from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
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
                Console.Write(ex.Message);
            }
            return data;
        }
        public CLGFeeRefundableDTO GetStudent(CLGFeeRefundableDTO stu)
        {
            try
            {



                //  stu.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                     from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                     where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == stu.ASMCL_ID &&  b.ASMS_Id == stu.ASMS_Id && a.MI_Id == stu.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.ASMAY_Id == stu.ASMAY_Id && b.AMAY_ActiveFlag == 1)
                //                     select new CollegeFeeTransactionDTO
                //                     {
                //                         AMCST_Id = a.AMST_Id,
                //                         AMCST_FirstName = a.AMST_FirstName,
                //                         AMCST_MiddleName = a.AMST_MiddleName,
                //                         AMCST_LastName = a.AMST_LastName,
                //                     }
                //).ToArray();
                var fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                   from d in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                   from f in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                   where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && b.AMCO_Id == stu.AMCO_Id && b.AMB_Id == stu.AMB_Id && b.AMSE_Id == stu.AMSE_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag==1)
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
                stu.studentlist = fillstudent.ToArray();


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }
        public CLGFeeRefundableDTO GetStudentListByamst(CLGFeeRefundableDTO stu)
        {
            try
            {
                bool refundflag = false;
                if (stu.filterrefund == "Refunable")
                    refundflag = true;
                stu.fillgroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 where (c.FMH_Id == b.FMH_Id && a.FMG_Id == b.FMG_Id && b.AMCST_Id == stu.AMCST_Id && a.MI_Id == stu.MI_Id && b.ASMAY_Id == stu.ASMAY_Id && a.user_id == b.User_Id && a.user_id == stu.userid && c.FMH_RefundFlag == refundflag && a.FMG_ActiceFlag == true && c.FMH_ActiveFlag==true)                                 
                                 select new CLGFeeRefundableDTO
                                 {
                                     FMG_Id = a.FMG_Id,
                                     FMG_GroupName = a.FMG_GroupName
                                 }

             ).Distinct().ToArray();

                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                             from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            // from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                             where (a.FYP_Id == b.FYP_Id && a.MI_Id == stu.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == stu.ASMAY_Id && a.User_Id == stu.userid && b.AMCST_Id == stu.AMCST_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                             select new CollegeFeeTransactionDTO
                                             {
                                                 FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                 FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                 FYP_TotalPaidAmount = a.FYP_TotalPaidAmount
                                             }
                    ).ToList();
                stu.showstaticticsdetails = showstaticticsdetails.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }

        public CLGFeeRefundableDTO getdatastuacadgrp(CLGFeeRefundableDTO id)
        {
            throw new NotImplementedException();
        }

        public CLGFeeRefundableDTO geteditdet(CLGFeeRefundableDTO data)
        {
            try
            {
               // var query = _YearlyFeeGroupMappingContext.FeeMasterRefundDMO.Where(d => d.FR_ID == data.FCR_Id).ToArray();
              
                //data.ASMCL_ID  = _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO.Single(c => c.AMST_Id == query.FirstOrDefault().AMST_ID).ASMCL_Id;
                //data.FMH_FeeName = _YearlyFeeGroupMappingContext.FeeHeadDMO.Single(d => d.FMH_Id == query.FirstOrDefault().FMH_ID).FMH_FeeName;
               
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGFeeRefundableDTO getstuddet(CLGFeeRefundableDTO data)
        {
            throw new NotImplementedException();
        }

        public CLGFeeRefundableDTO savedetails(CLGFeeRefundableDTO data)
        {
            int j = 0;

            CLGFeeRefundableDTO pmm = new CLGFeeRefundableDTO();
            try
            {
                if (data.FCR_Id > 0)
                {
                    if (data.savetmpdata.Length > 0)
                    {
                        if (data.ASMAY_Id > 0 && data.MI_Id > 0)
                        {

                            while (j < data.savetmpdata.Count())
                            {
                                var updateStudentStatus = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.FCSS_Id == data.savetmpdata[j].FCSS_Id);
                                var gethead = _YearlyFeeGroupMappingContext.FeeHeadClgDMO.Single(t => t.FMH_Id == updateStudentStatus.FMH_Id);
                                if (gethead.FMH_RefundFlag == true)
                                {
                                    if (updateStudentStatus.FCSS_RefundableAmount >= Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount))
                                    {
                                        updateStudentStatus.FCSS_RefundableAmount = updateStudentStatus.FCSS_RefundableAmount - Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount);

                                    }
                                }
                                else
                                {
                                    if (updateStudentStatus.FCSS_RunningExcessAmount >= Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount))
                                    {
                                        updateStudentStatus.FCSS_RunningExcessAmount = updateStudentStatus.FCSS_RunningExcessAmount - Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount);

                                    }
                                }

                                if (data.selectedRefundOverList.Length > 0)
                                {
                                    if (data.selectedRefundOverList[j].FCSS_Id == updateStudentStatus.FCSS_Id)
                                    {
                                        updateStudentStatus.FCSS_RefundOverFlag = true;
                                    }
                                }
                                _YearlyFeeGroupMappingContext.Update(updateStudentStatus);
                                var fmr = _YearlyFeeGroupMappingContext.Fee_College_RefundDMO.Single(d => d.FCR_Id == data.FCR_Id);
                                fmr.FMH_Id = data.savetmpdata[j].FMH_Id;
                                fmr.AMCST_Id = data.AMCST_Id;
                                fmr.FCR_Date = data.FCR_Date;
                                fmr.FCR_RefundAmount = fmr.FCR_RefundAmount + data.savetmpdata[j].FCR_RefundAmount;
                                fmr.FCR_RefundRemarks = data.FR_RefundRemarks;
                                fmr.FCR_RefundNo = data.FCR_RefundNo;
                                fmr.FCR_ModeOfPayment = data.FCR_ModeOfPayment;
                                fmr.FCR_ChequeDDDate = data.FCR_ChequeDDDate;
                                fmr.FCR_ChequeDDNo = data.FCR_ChequeDDNo;
                                fmr.FMG_Id = data.FMG_Id;
                                fmr.FCR_Bank = data.FCR_Bank;
                                _YearlyFeeGroupMappingContext.Update(fmr);
                                var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                if (contactExists > 0)
                                {
                                    data.returnVal = true;
                                    data.returntxt = "Updated";
                                }
                                else
                                {
                                    data.returnVal = false;
                                    data.returntxt = "not Updated";
                                }
                                j++;
                            }
                            //}

                        }
                    }
                }
                else
                {
                    if (data.savetmpdata.Length > 0)
                    {
                        if (data.ASMAY_Id > 0 && data.MI_Id > 0)
                        {

                            while (j < data.savetmpdata.Count())
                            {
                                //Updating Columns in Fee_Student_Status table
                                var updateStudentStatus = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.FCSS_Id == data.savetmpdata[j].FCSS_Id);
                                var gethead = _YearlyFeeGroupMappingContext.FeeHeadClgDMO.Single(t => t.FMH_Id == updateStudentStatus.FMH_Id);
                                if (gethead.FMH_RefundFlag == true)
                                {
                                    if (updateStudentStatus.FCSS_RefundableAmount >= Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount))
                                    {
                                        updateStudentStatus.FCSS_RefundableAmount = updateStudentStatus.FCSS_RefundableAmount - Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount);

                                    }
                                }
                                else
                                {
                                    if (updateStudentStatus.FCSS_RunningExcessAmount >= Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount))
                                    {
                                        updateStudentStatus.FCSS_RunningExcessAmount = updateStudentStatus.FCSS_RunningExcessAmount - Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount);

                                    }
                                }
                                for (int i = 0; i < data.selectedRefundOverList.Length; i++)
                                {
                                    if (data.selectedRefundOverList[i].FCSS_Id == updateStudentStatus.FCSS_Id)
                                    {
                                        updateStudentStatus.FCSS_RefundOverFlag = true;
                                    }
                                }
                                //for (int i = 0; i < data.savetmpdata.Length; i++)
                                //{
                                    updateStudentStatus.FCSS_RefundAmount = Convert.ToInt32(updateStudentStatus.FCSS_RefundAmount) + Convert.ToInt64(data.savetmpdata[j].FCR_RefundAmount);
                               // }
                                _YearlyFeeGroupMappingContext.Update(updateStudentStatus);
                                //**********************************************************

                                //Saving data in Fee_Refund table
                                //Fee_College_RefundDMO pgmodule = Mapper.Map<Fee_College_RefundDMO>(data);
                                //pgmodule.FMH_Id = data.savetmpdata[j].FMH_Id;
                                //pgmodule.FR_RefundAmount = data.savetmpdata[j].FCR_RefundAmount;
                                //pgmodule.FTI_Id = data.savetmpdata[j].FTI_Id;
                                //pgmodule.FMG_Id = data.savetmpdata[j].FMG_Id;
                                //pgmodule.FR_RefundFlag = "true";
                                Fee_College_RefundDMO pgmodule = new  Fee_College_RefundDMO();
                                pgmodule.MI_Id = data.MI_Id;
                                pgmodule.AMCST_Id = data.AMCST_Id;
                                pgmodule.ASMAY_Id = data.ASMAY_Id;
                                pgmodule.FMG_Id = data.savetmpdata[j].FMG_Id;
                                pgmodule.FCMAS_Id = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.FCSS_Id == data.savetmpdata[j].FCSS_Id).FCMAS_Id;
                                pgmodule.FMH_Id= data.savetmpdata[j].FMH_Id;
                                pgmodule.FTI_Id= data.savetmpdata[j].FTI_Id;
                                pgmodule.FCR_Date = data.FCR_Date;
                                pgmodule.FCR_RefundNo = data.FCR_RefundNo;
                                pgmodule.FCR_RefundAmount = data.savetmpdata[j].FCR_RefundAmount;
                                pgmodule.FCR_ModeOfPayment = data.FCR_ModeOfPayment;
                                pgmodule.FCR_ChequeDDNo = data.FCR_ChequeDDNo;
                                pgmodule.FCR_RefundRemarks = data.FCR_RefundRemarks;
                                pgmodule.FCR_ChequeDDDate = data.FCR_ModeOfPayment=="C" ? data.FCR_Date : data.FCR_ChequeDDDate;
                                pgmodule.FCR_Bank = data.FCR_ModeOfPayment == "C" ? "" :data.FCR_Bank;
                                pgmodule.FCR_OPReferenceNo = 0;
                                pgmodule.FCR_RefundFlag = "true";
                                pgmodule.User_Id = data.userid;                              
                                _YearlyFeeGroupMappingContext.Add(pgmodule);
                                var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

                                if (contactExists > 0)
                                {
                                    data.returnVal = true;
                                    data.returntxt = "Saved";
                                }
                                else
                                {
                                    data.returnVal = false;
                                    data.returntxt = "not Saved";
                                }
                                j++;
                            }
                            // }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                data.returntxt = "Failed";
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGFeeRefundableDTO getdataclawisestude(CLGFeeRefundableDTO clsid)
        {
            try
            {
              //  clsid.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
              //                      from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO 
              //                      where (a.AMST_Id == b.AMST_Id && a.MI_Id == clsid.MI_Id && a.AMST_SOL == "S" && b.ASMAY_Id == clsid.ASMAY_Id && b.ASMCL_Id==clsid.ASMCL_ID)
              //                      select new CollegeFeeTransactionDTO
              //                      {
              //                          AMCST_Id = a.AMST_Id,
              //                          AMCST_FirstName = a.AMST_FirstName,
              //                          AMCST_MiddleName = a.AMST_MiddleName,
              //                          AMCST_LastName = a.AMST_LastName,
              //                      }
              //).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return clsid;
        }

        public CLGFeeRefundableDTO getgroupheaddetails(CLGFeeRefundableDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.fillhead = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from c in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                 where (a.FMH_Id == b.FMH_Id && b.ASMAY_Id == data.ASMAY_Id &&
                                 b.MI_Id == data.MI_Id && b.AMCST_Id == data.AMCST_Id
                                 && data.multiplegroupF.Contains(Convert.ToString(b.FMG_Id)) && c.FTI_Id == b.FTI_Id && a.user_id == data.userid && a.FMH_RefundFlag == refundflag && (a.FMH_Flag != "F" && (b.FCSS_RunningExcessAmount + b.FCSS_RefundableAmount) > 0) && b.FCSS_RefundOverFlag == false) //a.FMH_Flag == "E" ||  
                                 select new CLGFeeRefundableDTO
                                 {
                                     FCSS_Id = b.FCSS_Id,
                                     FMH_Id = a.FMH_Id,
                                     FMH_FeeName = a.FMH_FeeName,
                                     FTI_Id = b.FTI_Id,
                                     FTI_Name = c.FTI_Name,
                                     FCSS_RunningExcessAmount = b.FCSS_RunningExcessAmount,
                                     FCSS_RefundableAmount = b.FCSS_RefundableAmount,
                                     FCSS_RefundAmount = b.FCSS_RefundAmount,
                                     FMG_Id = b.FMG_Id,
                                     FCSS_ToBePaid=b.FCSS_ToBePaid
                                 }
              ).ToArray();


            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGFeeRefundableDTO getmodeofpaymentdata(CLGFeeRefundableDTO data)
        {
            try
            {
                if (data.FCR_ModeOfPayment == "C")
                {
             //       data.fillacclst = (from a in _YearlyFeeGroupMappingContext.AccLedgerDMO
             //                          where (a.MG_CodeInt == 15)
             //                          select new FeeStudentTransactionDTO
             //                          {
             //                              L_Code = a.L_Code,
             //                              L_Name = a.L_Name,
             //                          }
             //).ToArray();

                }
                else if (data.FCR_ModeOfPayment == "B" || data.FCR_ModeOfPayment == "R" || data.FCR_ModeOfPayment == "S")
                {
             //       data.fillacclst = (from a in _YearlyFeeGroupMappingContext.AccLedgerDMO
             //                          where (a.MG_CodeInt == 14)
             //                          select new FeeStudentTransactionDTO
             //                          {
             //                              L_Code = a.L_Code,
             //                              L_Name = a.L_Name,
             //                          }
             //).ToArray();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGFeeRefundableDTO searching(CLGFeeRefundableDTO data)
        {

            try
            {

                switch (data.searchType)
                {
                    case "0":
                        string str = "";
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_RefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                  from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.FMH_Id == c.FMH_Id && b.AMCST_Id == d.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_SOL == "S"
                                                  && b.AMCST_ActiveFlag == true && d.ACYST_ActiveFlag == 1 && a.FCR_RefundFlag.Equals("true") && (((b.AMCST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(b.AMCST_MiddleName.Trim()) == true ? str : b.AMCST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(b.AMCST_LastName.Trim()) == true ? str : b.AMCST_LastName.Trim())).Trim().Contains(data.searchtext) || b.AMCST_FirstName.StartsWith(data.searchtext) || b.AMCST_MiddleName.StartsWith(data.searchtext) || b.AMCST_LastName.StartsWith(data.searchtext)) && a.User_Id == data.userid)
                                                  select new CLGFeeRefundableDTO
                                                  {
                                                      AMCST_Id = a.AMCST_Id,
                                                      AMCST_FirstName = b.AMCST_FirstName,
                                                      AMCST_MiddleName = b.AMCST_MiddleName,
                                                      AMCST_LastName = b.AMCST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FCR_RefundAmount = a.FCR_RefundAmount,
                                                      FCR_RefundNo = a.FCR_RefundNo,
                                                      FCR_Date = a.FCR_Date,
                                                      FCR_Id = a.FCR_Id
                                                  }).OrderBy(t => t.AMCST_FirstName).ToList().ToArray();


                        break;
                    case "1":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_RefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                  from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.FMH_Id == c.FMH_Id && b.AMCST_Id == d.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_SOL == "S"
                                                  && b.AMCST_ActiveFlag == true && d.ACYST_ActiveFlag == 1 && a.FCR_RefundFlag.Equals("true") && c.FMH_FeeName.Contains(data.searchtext)) && a.User_Id == data.userid
                                                  select new CLGFeeRefundableDTO
                                                  {
                                                      AMCST_Id = a.AMCST_Id,
                                                      AMCST_FirstName = b.AMCST_FirstName,
                                                      AMCST_MiddleName = b.AMCST_MiddleName,
                                                      AMCST_LastName = b.AMCST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FCR_RefundAmount = a.FCR_RefundAmount,
                                                      FCR_RefundNo = a.FCR_RefundNo,
                                                      FCR_Date = a.FCR_Date,
                                                      FCR_Id = a.FCR_Id
                                                  }).OrderBy(t => t.FMH_FeeName).ToList().ToArray();


                        break;
                    case "2":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_RefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                  from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.FMH_Id == c.FMH_Id && b.AMCST_Id == d.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_SOL == "S"
                                                  && b.AMCST_ActiveFlag == true && d.ACYST_ActiveFlag == 1 && a.FCR_RefundFlag.Equals("true") && a.FCR_RefundAmount.ToString().Contains(data.searchnumber) && a.User_Id == data.userid)
                                                  select new CLGFeeRefundableDTO
                                                  {
                                                      AMCST_Id = a.AMCST_Id,
                                                      AMCST_FirstName = b.AMCST_FirstName,
                                                      AMCST_MiddleName = b.AMCST_MiddleName,
                                                      AMCST_LastName = b.AMCST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FCR_RefundAmount = a.FCR_RefundAmount,
                                                      FCR_RefundNo = a.FCR_RefundNo,
                                                      FCR_Date = a.FCR_Date,
                                                      FCR_Id = a.FCR_Id
                                                  }).OrderBy(t => t.FCR_RefundAmount).ToList().ToArray();
                        break;
                    case "3":
                        data.fillthirdgriddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_RefundDMO
                                                  from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                  from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                                  where (a.AMCST_Id == b.AMCST_Id && a.FMH_Id == c.FMH_Id && b.AMCST_Id == d.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_SOL == "S"
                                                  && b.AMCST_ActiveFlag == true && d.ACYST_ActiveFlag == 1 && a.FCR_RefundFlag.Equals("true") && Convert.ToDateTime(a.FCR_Date).ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.User_Id == data.userid)
                                                  select new CLGFeeRefundableDTO
                                                  {
                                                      AMCST_Id = a.AMCST_Id,
                                                      AMCST_FirstName = b.AMCST_FirstName,
                                                      AMCST_MiddleName = b.AMCST_MiddleName,
                                                      AMCST_LastName = b.AMCST_LastName,
                                                      FMH_FeeName = c.FMH_FeeName,
                                                      FCR_RefundAmount = a.FCR_RefundAmount,
                                                      FCR_RefundNo = a.FCR_RefundNo,
                                                      FCR_Date = a.FCR_Date,
                                                      FCR_Id = a.FCR_Id
                                                  }).OrderBy(t => t.FCR_Date).ToList().ToArray();
                        break;
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
