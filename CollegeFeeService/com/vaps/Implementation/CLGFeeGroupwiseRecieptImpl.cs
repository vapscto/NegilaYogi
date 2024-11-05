using System;
using System.Linq;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using System.Collections.Generic;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using System.Dynamic;
using System.IO;
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeGroupwiseRecieptImpl : interfaces.CLGFeeGroupwiseRecieptInterfaces
    {
        private static ConcurrentDictionary<string, CLGFeeGroupwiseRecieptDTO> _login =
            new ConcurrentDictionary<string, CLGFeeGroupwiseRecieptDTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<CLGFeeGroupwiseRecieptImpl> _logger;
        public CLGFeeGroupwiseRecieptImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<CLGFeeGroupwiseRecieptImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public CLGFeeGroupwiseRecieptDTO Getinitialformload(CLGFeeGroupwiseRecieptDTO fee)
        {
            try
            {
                CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);

                fee.academicdrp = clgcomm.Get_All_Academicyear(fee.MI_Id);

                fee.currfillyear = clgcomm.Get_Current_Academicyear(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillcourse = clgcomm.Get_Yearly_Course(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillgroup = clgcomm.Get_Yearly_groups(fee.MI_Id, fee.ASMAY_Id);

                fee.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == fee.user_id && g.MI_Id == fee.MI_Id && g.ASMAY_Id == fee.ASMAY_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                fee.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == fee.MI_Id && g.ASMAY_Id == fee.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new CLGFeeGroupwiseRecieptDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

                fee.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == fee.MI_Id && a.ASMAY_Id == fee.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == fee.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == fee.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && g.MI_Id == fee.MI_Id && g.ASMAY_Id == fee.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == e.FTI_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
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
            return fee;
        }

        public CLGFeeGroupwiseRecieptDTO getbranchdetails(CLGFeeGroupwiseRecieptDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {

                if (data.AMCO_Id == 0)
                {
                    var branchlist = (from a in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                  from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id.Equals(data.MI_Id) && a.AMB_ActiveFlag && b.MI_Id.Equals(data.MI_Id) && b.ASMAY_Id.Equals(data.ASMAY_Id) && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id.Equals(data.MI_Id) && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
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
                                  }).ToList();

                    data.fillbranch = branchlist.ToArray();

                }

                else
                {
                    data.fillbranch = clgcomm.Get_Yearly_Course_Branch(data.MI_Id, data.ASMAY_Id, data.AMCO_Id);
                }
               
            }




            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
          public CLGFeeGroupwiseRecieptDTO onsemselection(CLGFeeGroupwiseRecieptDTO data)
        {
            try
            {
                data.fillsection = _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).Distinct().OrderBy(t => t.ACMS_Order).ToArray() ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeGroupwiseRecieptDTO onselectsec(CLGFeeGroupwiseRecieptDTO data)
        {
            try
            {
                if (data.ACMS_Id > 0)
                {

                    if (data.AMCO_Id == 0 && data.AMB_Id == 0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true && a.ACMS_Id == data.ACMS_Id)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
                                                AMCST_Id = a.AMCST_Id,
                                                ACYST_RollNo = a.ACYST_RollNo,
                                                AMCST_FirstName = b.AMCST_FirstName,
                                                AMCST_MiddleName = b.AMCST_MiddleName,
                                                AMCST_LastName = b.AMCST_LastName,
                                                AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                                AMCST_AdmNo = b.AMCST_AdmNo
                                            }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                    }

                    else if (data.AMCO_Id != 0 && data.AMB_Id == 0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Id == a.AMCO_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true && a.ACMS_Id == data.ACMS_Id)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
                                                AMCST_Id = a.AMCST_Id,
                                                ACYST_RollNo = a.ACYST_RollNo,
                                                AMCST_FirstName = b.AMCST_FirstName,
                                                AMCST_MiddleName = b.AMCST_MiddleName,
                                                AMCST_LastName = b.AMCST_LastName,
                                                AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                                AMCST_AdmNo = b.AMCST_AdmNo
                                            }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Id == a.AMCO_Id && data.AMB_Id == a.AMB_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true && a.ACMS_Id == data.ACMS_Id)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
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
                else
                {


                    if (data.AMCO_Id == 0 && data.AMB_Id == 0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
                                                AMCST_Id = a.AMCST_Id,
                                                ACYST_RollNo = a.ACYST_RollNo,
                                                AMCST_FirstName = b.AMCST_FirstName,
                                                AMCST_MiddleName = b.AMCST_MiddleName,
                                                AMCST_LastName = b.AMCST_LastName,
                                                AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                                AMCST_AdmNo = b.AMCST_AdmNo
                                            }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                    }
                    else if (data.AMCO_Id != 0 && data.AMB_Id == 0)
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Id == a.AMCO_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
                                                AMCST_Id = a.AMCST_Id,
                                                ACYST_RollNo = a.ACYST_RollNo,
                                                AMCST_FirstName = b.AMCST_FirstName,
                                                AMCST_MiddleName = b.AMCST_MiddleName,
                                                AMCST_LastName = b.AMCST_LastName,
                                                AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                                AMCST_AdmNo = b.AMCST_AdmNo
                                            }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();
                    }
                    else
                    {
                        data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from b in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Id == a.AMCO_Id && data.AMB_Id == a.AMB_Id && a.ACYST_ActiveFlag == 1 && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == true)
                                            select new CLGFeeGroupwiseRecieptDTO
                                            {
                                                ACAYC_Id = a.ACYST_Id,
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeGroupwiseRecieptDTO getsemesterdetails(CLGFeeGroupwiseRecieptDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                if (data.AMCO_Id ==0 && data.AMB_Id==0)
                {
                    var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                        from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                        from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id  && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
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
                    data.fillsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }

                else if (data.AMCO_Id != 0 && data.AMB_Id == 0)
                {
                    var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                        from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                        from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && b.AMCO_Id == data.AMCO_Id)
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
                    data.fillsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }
                else if (data.AMCO_Id != 0 && data.AMB_Id != 0)
                {
                    var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                        from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                        from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id)
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
                    data.fillsemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
                }

                else
                {
                    data.fillsemester = clgcomm.Get_Yearly_Course_Branch_Semesters(data.MI_Id, data.ASMAY_Id, data.AMCO_Id, data.AMB_Id);
                }
             
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeGroupwiseRecieptDTO getcourdetails(CLGFeeGroupwiseRecieptDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {

                data.Fillcourse = clgcomm.Get_Yearly_Course(data.MI_Id, data.ASMAY_Id);





                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                        from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == data.user_id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                data.fillmasterhead = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new CLGFeeGroupwiseRecieptDTO
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = c.FMH_Id,
                                           FMH_FeeName = c.FMH_FeeName
                                       }).Distinct().ToArray();

                data.fillinstallment = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        from g in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMI_Id == d.FMI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYGHM_ActiveFlag == "1" && b.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMI_ActiceFlag == true && e.FMI_Id == a.FMI_Id && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == e.FTI_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
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

        public CLGFeeGroupwiseRecieptDTO getgroupmapped(CLGFeeGroupwiseRecieptDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                List<CLG_Fee_Yearly_Group_Head_Mapping> commamtflag = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                commamtflag = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id).ToList();
                data.commountamountflag = commamtflag.ToArray();


                data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
                                        {
                                            FMH_Id = a.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FMG_GroupName = b.FMG_GroupName,
                                            FMH_FeeName = c.FMH_FeeName,
                                            FMI_Name = e.FTI_Name,
                                            FCMAS_Amount = 0,
                                            FMH_Order = c.FMH_Order,
                                            FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                        }
  ).OrderByDescending(t => t.FMH_Order).ToArray();

                //feeamtentry.allgroupheaddata var p1
                data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                         from g in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                         where (f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && f.AMCO_Id == data.AMCO_Id && f.AMB_Id == data.AMB_Id && g.AMSE_Id == data.AMSE_Id && d.FMI_Id == e.FMI_Id && f.FCMA_Id == g.FCMA_Id && f.FCMA_Id == h.FCMAS_Id && a.ASMAY_Id == f.ASMAY_Id)
                                         select new CLGFeeGroupwiseRecieptDTO
                                         {
                                             FCMAS_Id = g.FCMAS_Id,
                                             FMH_Id = a.FMH_Id,
                                             FTI_Id = e.FTI_Id,
                                             FMG_GroupName = b.FMG_GroupName,
                                             FMH_FeeName = c.FMH_FeeName,
                                             FMI_Name = e.FTI_Name,
                                             FCMAS_Amount = g.FCMAS_Amount,
                                             FCTDD_Day = h.FCTDD_Day,
                                             FCTDD_Month = h.FCTDD_Month,
                                             FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                             //added praveen
                                             FMH_Order=c.FMH_Order
                                         }
      ).OrderBy(t => t.FMH_Order).ToArray();


                data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                        from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Fine_Slabs
                                        from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        where (a.FMFS_Id == b.FMFS_Id && b.FCMAS_Id == d.FCMAS_Id && c.FCMA_Id == d.FCMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id)
                                        select new CLGFeeGroupwiseRecieptDTO
                                        {
                                            FCMAS_Id = d.FCMAS_Id,
                                            FCTFS_Amount = b.FCTFS_Amount,
                                            FMFS_FineType = a.FMFS_FineType,
                                            FMFS_ECSFlag = a.FMFS_ECSFlag,
                                            FCTFS_FineType = b.FCTFS_FineType,
                                            FMG_Id = c.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = c.FTI_Id,
                                            FMFS_FromDay = a.FMFS_FromDay,
                                            FMFS_ToDay = a.FMFS_ToDay,

                                        }
                    ).ToArray();

                if (data.allgroupheaddata.Length <= 0)
                {
                    //var p2
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                             from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                             from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                             where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                             select new CLGFeeGroupwiseRecieptDTO
                                             {
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FCMAS_Amount = 0,
                                                 FMH_Order = c.FMH_Order,
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag
                                             }
     ).OrderBy(t => t.FMH_Order).ToArray();

                    List<MasterMonthDMO> mon1 = new List<MasterMonthDMO>();
                    mon1 = _YearlyFeeGroupMappingContext.IVRM_Month.ToList();
                    data.fillmonth = mon1.ToArray();
                }
                //added Praveen
                List<MasterMonthDMO> mon = new List<MasterMonthDMO>();
                mon = _YearlyFeeGroupMappingContext.IVRM_Month.ToList();
                data.fillmonth = mon.ToArray();
                var dat = _YearlyFeeGroupMappingContext.AcademicYear.Single(y => y.MI_Id == data.MI_Id && y.ASMAY_Id == data.ASMAY_Id);
                data.ASMAY_Year = dat.ASMAY_Year;
                //end Praveen
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeGroupwiseRecieptDTO getreceiptreport(CLGFeeGroupwiseRecieptDTO data)
        {
            try
            {
                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();
                List<long> std_id = new List<long>();
                foreach (var item in data.savegrplst)
                {

                    grpid.Add(item.FMG_Id);
                }
                foreach (var item in data.saveheadlst)
                {
                    HeadId.Add(item.FMH_Id);
                }

                foreach (var item in data.studentdata)
                {
                    std_id.Add(item.AMCST_Id);
                }
                string groupidss = "0";
                string headlist = "0";
                string stdlist = "0";
                string rcptlist = "0";



                List<long> recpt_id = new List<long>();
                foreach (var item in data.receiptlist)
                {
                    recpt_id.Add(item.FYP_Id);
                }

                for (int r = 0; r < grpid.Count(); r++)
                {
                    if (r == 0)
                    {
                        groupidss = grpid[r].ToString();
                    }
                    else
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                }
                for (int c = 0; c < HeadId.Count(); c++)
                {
                    if (c == 0)
                    {
                        headlist = HeadId[c].ToString();
                    }
                    else
                    {
                        headlist = headlist + ',' + HeadId[c];
                    }
                }

                for (int c = 0; c < std_id.Count(); c++)
                {

                    if (c == 0)
                    {
                        stdlist = std_id[c].ToString();
                    }
                    else
                    {
                        stdlist = stdlist + ',' + std_id[c];
                    }

                }


                for (int c = 0; c < recpt_id.Count(); c++)
                {

                    if (c == 0)
                    {
                        rcptlist = recpt_id[c].ToString();
                    }
                    else
                    {
                        rcptlist = rcptlist + ',' + recpt_id[c];
                    }
                }

                string frmdate = "";
                string todate = "";

                if (rcptlist=="0")
                {
                    DateTime fdate = Convert.ToDateTime(data.Fromdate);
                    frmdate = fdate.ToString("yyyy-MM-dd");

                    DateTime tdate = Convert.ToDateTime(data.Todate);
                    todate = tdate.ToString("yyyy-MM-dd");

                    stdlist = "0";
                }
                else
                {
                    frmdate = "";
                    todate = "";
                }

                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_Fee_BalRegister_studentlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.AMCO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.AMB_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.AMSE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.ACMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                          SqlDbType.NVarChar)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                        SqlDbType.NVarChar)
                        {
                            Value = headlist
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.NVarChar)
                        {
                            Value = stdlist
                        });
                        cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                      SqlDbType.NVarChar)
                        {
                            Value = rcptlist
                        });

                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                      SqlDbType.VarChar)
                        {
                            Value = frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate",
                        SqlDbType.VarChar)
                        {
                            Value = todate
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.studentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
              


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_Fee_BalRegister_29Mar2021";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ACMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                      SqlDbType.NVarChar)
                    {
                        Value = groupidss
                    });

                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = headlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = stdlist
                    });
                    cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                  SqlDbType.NVarChar)
                    {
                        Value = rcptlist
                    });

                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                  SqlDbType.VarChar)
                    {
                        Value = frmdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
                    SqlDbType.VarChar)
                    {
                        Value = todate
                    });

                    //  cmd.Parameters.Add(new SqlParameter("@FYP_Id",
                    //SqlDbType.BigInt)
                    //  {
                    //      Value = data.FYP_Id
                    //  });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.receiptdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        
        public CLGFeeGroupwiseRecieptDTO getreceipt(CLGFeeGroupwiseRecieptDTO data)
        {
            try
            {
               
                List<long> std_id = new List<long>();
                foreach (var item in data.studentdata)
                {
                    std_id.Add(item.AMCST_Id);
                }

                //var fmg_ids = "";
                //foreach (var x in data.AMCST_Ids)
                //{
                //    fmg_ids += x + ",";
                //}
                //fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));
                

                data.receiptcount = (    from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                         where (a.FYP_Id==b.FYP_Id && a.FYP_Id==c.FYP_Id && std_id.Contains(b.AMCST_Id) && a.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id)
                                         select new CLGFeeGroupwiseRecieptDTO
                                         {
                                            FYP_Id=b.FYP_Id,
                                            FYP_Receiptno=a.FYP_ReceiptNo
                                         }
                                      ).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

    }
}
