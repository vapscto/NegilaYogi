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
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegemasterstudentconcessionImpl: Interfaces.CollegemasterstudentconcessionInterface
    {
        public CollFeeGroupContext _ClgAdmissionContext;
        readonly ILogger<CollegemasterstudentconcessionImpl> _logger;
        public CollegemasterstudentconcessionImpl(CollFeeGroupContext _ClgAdmissionCon, ILogger<CollegemasterstudentconcessionImpl> log)
        {
            _logger = log;
            _ClgAdmissionContext = _ClgAdmissionCon;

        }
        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)

        {
            try
            {

                //List<ClgMasterAcademicYearDMO> allyear = new List<ClgMasterAcademicYearDMO>();
                //allyear = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).ToList();
                //data.yearlst = allyear.Distinct().ToArray();

               // List<MasterAcademic> year = new List<MasterAcademic>();
               var  year = _ClgAdmissionContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).ToList();
                data.yearlst = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();


                data.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                  from b in _ClgAdmissionContext.FeeYearGroupDMO
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id==data.ASMAY_Id)
                                  select new FeeGroupDMO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                     ).Distinct().ToArray();

                data.savedrecord = (from a in _ClgAdmissionContext.Fee_College_Student_StatusDMO
                                    from b in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                    from c in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                    from e in _ClgAdmissionContext.CollegeConcessionDMO
                                    from f in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                    from g in _ClgAdmissionContext.MasterCourseDMO
                                    from h in _ClgAdmissionContext.ClgMasterBranchDMO
                                    from i in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from j in _ClgAdmissionContext.FeeGroupClgDMO
                                    from k in _ClgAdmissionContext.FeeHeadClgDMO
                                    from l in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                    where (a.AMCST_Id==b.AMCST_Id && a.AMCST_Id==c.AMCST_Id && b.AMCST_Id==c.AMCST_Id && a.ASMAY_Id==c.ASMAY_Id && a.AMCST_Id==e.AMCST_Id && a.ASMAY_Id==e.ASMAY_Id && e.FCSC_Id==f.FCSC_Id && c.AMCO_Id==g.AMCO_Id && c.AMB_Id==h.AMB_Id && c.AMSE_Id==i.AMSE_Id && a.FMG_Id==j.FMG_Id && e.FMG_Id==j.FMG_Id && a.FMH_Id==k.FMH_Id && a.FMH_Id==e.FMH_Id && a.FTI_Id==l.FTI_Id && f.FTI_Id==l.FTI_Id && a.MI_Id==data.MI_Id && e.ASMAY_Id==data.ASMAY_Id)
                                    select new CollegeConcessionDTO
                                    {

                                        AMCST_FirstName = b.AMCST_FirstName,
                                        AMCST_MiddleName = b.AMCST_MiddleName,
                                        AMCST_LastName = b.AMCST_LastName,
                                        ACYST_RollNo = c.ACYST_RollNo,
                                        AMCST_AdmNo = b.AMCST_AdmNo,
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        FMH_FeeName = k.FMH_FeeName,
                                        FTI_Name = l.FTI_Name,
                                        FTI_Id = a.FTI_Id,
                                        FMH_Id = a.FMH_Id,
                                        FSCI_ConcessionAmount = f.FSCI_ConcessionAmount,
                                        AMB_BranchName = h.AMB_BranchName,
                                        AMSE_SEMName = i.AMSE_SEMName,
                                        AMCO_CourseName = g.AMCO_CourseName,
                                        FMG_GroupName = j.FMG_GroupName,
                                        FSCI_ID = f.FSCI_Id
                                    }
   ).Distinct().ToArray();

             
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            try
            {

                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();


                data.savedrecord = (from a in _ClgAdmissionContext.Fee_College_Student_StatusDMO
                                    from b in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                    from c in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                    from e in _ClgAdmissionContext.CollegeConcessionDMO
                                    from f in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                    from g in _ClgAdmissionContext.MasterCourseDMO
                                    from h in _ClgAdmissionContext.ClgMasterBranchDMO
                                    from i in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from j in _ClgAdmissionContext.FeeGroupClgDMO
                                    from k in _ClgAdmissionContext.FeeHeadClgDMO
                                    from l in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == c.AMCST_Id && b.AMCST_Id == c.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCST_Id == e.AMCST_Id && a.ASMAY_Id == e.ASMAY_Id && e.FCSC_Id == f.FCSC_Id && c.AMCO_Id == g.AMCO_Id && c.AMB_Id == h.AMB_Id && c.AMSE_Id == i.AMSE_Id && a.FMG_Id == j.FMG_Id && e.FMG_Id == j.FMG_Id && a.FMH_Id == k.FMH_Id && a.FMH_Id == e.FMH_Id && a.FTI_Id == l.FTI_Id && f.FTI_Id == l.FTI_Id && a.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id)
                                    select new CollegeConcessionDTO
                                    {

                                        AMCST_FirstName = b.AMCST_FirstName,
                                        AMCST_MiddleName = b.AMCST_MiddleName,
                                        AMCST_LastName = b.AMCST_LastName,
                                        ACYST_RollNo = c.ACYST_RollNo,
                                        AMCST_AdmNo = b.AMCST_AdmNo,
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        FMH_FeeName = k.FMH_FeeName,
                                        FTI_Name = l.FTI_Name,
                                        FTI_Id = a.FTI_Id,
                                        FMH_Id = a.FMH_Id,
                                        FSCI_ConcessionAmount = f.FSCI_ConcessionAmount,
                                        AMB_BranchName = h.AMB_BranchName,
                                        AMSE_SEMName = i.AMSE_SEMName,
                                        AMCO_CourseName = g.AMCO_CourseName,
                                        FMG_GroupName = j.FMG_GroupName,
                                        FSCI_ID = f.FSCI_Id
                                    }
  ).Distinct().ToArray();

                data.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                  from b in _ClgAdmissionContext.FeeYearGroupDMO
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                  select new FeeGroupDMO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                     ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
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
                _logger.LogInformation("Atten_Batch_Mapping  get_branches :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            try
            {
                var semisterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
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
                _logger.LogInformation("Atten_Batch_Mapping  get_semisters :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_student(CollegeConcessionDTO data)
        {
            var AMSE_ID = 0;
            var AMSE_IDs = "";
            foreach (var x in data.AMS_Id)
            {
                AMSE_IDs += x + ",";
            }
            AMSE_IDs = AMSE_IDs.Substring(0, (AMSE_IDs.Length));
           // AMSE_ID = Convert.ToInt32(AMSE_IDs);
            
            try
            {
                var studentlst = (from a in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                    from b in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.ACYST_ActiveFlag == 1 && AMSE_IDs.Contains(a.AMSE_Id.ToString()) && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag)
                                    select new CollegeConcessionDTO
                                    {
                                        AMCST_Id = a.AMCST_Id,
                                        ACYST_RollNo = a.ACYST_RollNo,
                                        AMCST_FirstName = b.AMCST_FirstName,
                                        AMCST_MiddleName = b.AMCST_MiddleName,
                                        AMCST_LastName = b.AMCST_LastName,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        AMCST_AdmNo = b.AMCST_AdmNo
                                    }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();

                //var studentlst = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                //                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                //                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                //                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                //                    from e in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                //                    from f in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                //                  where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag && f.AMCST_Id==e.AMCST_Id && e.AMSE_Id==d.AMSE_Id && e.ACYST_ActiveFlag==1 && AMSE_IDs.Contains(e.AMSE_Id.ToString()))
                //                    select new CollegeConcessionDTO
                //                    {
                //                        AMCST_FirstName = f.AMCST_FirstName,
                //                        AMCST_MiddleName = f.AMCST_MiddleName,
                //                        AMCST_LastName = f.AMCST_LastName,
                //                        ACYST_RollNo = e.ACYST_RollNo,
                //                        AMCST_AdmNo = f.AMCST_AdmNo,
                //                        AMCST_Id=f.AMCST_Id,
                //                        AMCST_RegistrationNo = f.AMCST_RegistrationNo,
                //                    }).Distinct().ToList();
                data.studentlist = studentlst.ToList().Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegeConcessionDTO fillamount(CollegeConcessionDTO data)
        {
            try
            {

                //              _ClgAdmissionContext.Database.ExecuteSqlCommand("fillstudentlistforconcession_term_all_College @p0,@p1,@p2,@p3,@p4", data.AMCST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid);
                //              //_ClgAdmissionContext.Database.ExecuteSqlCommand("fillstudentlistforconcession_term_all @p0,@p1,@p2,@p3,@p4", data.AMST_Id, data.ASMAY_ID, data.MI_Id, data.multiplegroups, data.userid);

                //              data.savedcondatalist = (from a in _ClgAdmissionContext.v_studentPendingsavedconcessionDMO
                //                                       where (a.mi_id == data.MI_Id)
                //                                       select new FeeConcessionDTO
                //                                       {
                //                                           FMH_FeeName = a.FMH_FeeName,
                //                                           FTI_Name = a.FTI_Name,
                //                                           FTI_Id = a.FTI_Id,
                //                                           FMH_Id = a.fmh_id,
                //                                           FMA_Amount = a.FMA_Amount,
                //                                           FMA_Id = a.fma_id,
                //                                           FMG_Id = a.FMG_Id,
                //                                           FSCI_ConcessionAmount = a.FSCI_ConcessionAmount,
                //                                           FSC_ConcessionType = a.FSC_ConcessionType,
                //                                           FSC_ConcessionReason = a.FSC_ConcessionReason,
                //                                           FSC_Id = a.fsc_id
                //                                       }
                //).Distinct().ToArray();






                //                   data.fillheaddata = (from a in _ClgAdmissionContext.v_studentPendingconcessionDMO
                //                                        where (a.mi_id == data.MI_Id)
                //                                        select new FeeConcessionDTO
                //                                        {
                //                                            FMH_FeeName = a.FMH_FeeName,
                //                                            FTI_Name = a.FTI_Name,
                //                                            FTI_Id = a.FTI_Id,
                //                                            FMH_Id = a.fmh_id,
                //                                            FMA_Amount = a.FMA_Amount,
                //                                            FMA_Id = a.fma_id,
                //                                            FMG_Id = a.FMG_Id
                //                                        }
                //).Distinct().ToArray();



                data.fillheaddata = (from a in _ClgAdmissionContext.Fee_College_Student_StatusDMO
                                     from b in _ClgAdmissionContext.FeeHeadClgDMO
                                     from c in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                     from d in _ClgAdmissionContext.CLG_Fee_Yearly_Group_Head_Mapping
                                     from e in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                     where (a.FMH_Id == a.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && b.FMH_Id == a.FMH_Id && c.FTI_Id == a.FTI_Id && d.FMG_Id == data.FMG_Id && e.AMCST_Id == a.AMCST_Id && d.FMG_Id == a.FMG_Id && d.FMH_Id == a.FMH_Id)
                                     select new CollegeConcessionDTO//&& d.FMT_Id == data.FMG_Id
                                     {
                                         FMH_FeeName = b.FMH_FeeName,
                                         FTI_Name = c.FTI_Name,
                                         FTI_Id = a.FTI_Id,
                                         FMH_Id = a.FMH_Id,
                                         //FMA_Amount = a.FSS_NetAmount,
                                         FMA_Amount = a.FCSS_ToBePaid,
                                         FCMAS_Id = a.FCMAS_Id,
                                         FMG_Id = a.FMG_Id
                                     }
   ).Distinct().ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegeConcessionDTO fillheaddetailsss(CollegeConcessionDTO data)
        {
            try
            {
                data.fillheaddata = (from a in _ClgAdmissionContext.FeeYearGroupDMO
                                         from b in _ClgAdmissionContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from c in _ClgAdmissionContext.FeeHeadClgDMO
                                         from d in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                         from e in _ClgAdmissionContext.Clg_Fee_Installment_DMO
                                         where (a.FMG_Id == b.FMG_Id && b.FMH_Id == c.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && data.FMG_Ids.Contains(a.FMG_Id) && d.FMI_Id == b.FMI_Id && e.FMI_Id == d.FMI_Id)//&& a.FMG_Id == data.FMG_Id
                                         select new CollegeConcessionDTO
                                         {
                                             FMH_FeeName = c.FMH_FeeName,
                                             FTI_Name = d.FTI_Name,
                                             FTI_Id = d.FTI_Id,
                                             FMH_Id = c.FMH_Id,
                                         }
     ).Distinct().ToArray();
                

                // con checking

                if (data.AMCST_Id != 0)
                {
                   data.savedcondatalist = (from a in _ClgAdmissionContext.CollegeConcessionDMO
                                            from b in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                            from c in _ClgAdmissionContext.FeeHeadClgDMO
                                            from d in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                            from e in _ClgAdmissionContext.Fee_College_Student_StatusDMO
                                            where (b.FCSC_Id == a.FCSC_Id && c.FMH_Id == a.FMH_Id && d.FTI_Id == b.FTI_Id && e.AMCST_Id == a.AMCST_Id && e.FMH_Id == c.FMH_Id && e.FTI_Id == b.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)
                                                 select new CollegeConcessionDTO
                                                 {
                                                     FMH_FeeName = c.FMH_FeeName,
                                                     FTI_Name = d.FTI_Name,
                                                     FTI_Id = d.FTI_Id,
                                                     FMH_Id = a.FMH_Id,
                                                     FMA_Amount = e.FCSS_ToBePaid,
                                                     FCMAS_Id = e.FCMAS_Id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                                     FCSC_ConcessionType = a.FCSC_ConcessionType,
                                                     FCSC_ConcessionReason = a.FCSC_ConcessionReason,
                                                 }
      ).Distinct().ToArray();


                        data.fillheaddata = (from a in _ClgAdmissionContext.Fee_College_Student_StatusDMO
                                             from b in _ClgAdmissionContext.FeeHeadClgDMO
                                             from c in _ClgAdmissionContext.Clg_Fee_Installments_Yearly_DMO
                                             from d in _ClgAdmissionContext.CLG_Fee_Yearly_Group_Head_Mapping
                                             from e in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                             where (a.FMH_Id == a.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && b.FMH_Id == a.FMH_Id && c.FTI_Id == a.FTI_Id && d.FMG_Id==data.FMG_Id && e.AMCST_Id==a.AMCST_Id && d.FMG_Id==a.FMG_Id && d.FMH_Id==a.FMH_Id)
                                             select new CollegeConcessionDTO//&& d.FMT_Id == data.FMG_Id
                                             {
                                                 FMH_FeeName = b.FMH_FeeName,
                                                 FTI_Name = c.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 //FMA_Amount = a.FSS_NetAmount,
                                                 FMA_Amount = a.FCSS_ToBePaid,
                                                 FCMAS_Id = a.FCMAS_Id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();

                    
                }
                //con checking


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public CollegeConcessionDTO savedata(CollegeConcessionDTO data)
        {
            try
            {

                using (var transaction = _ClgAdmissionContext.Database.BeginTransaction())
                {
                    if (data.savetmpdata != null || data.savetmpdata1 != null)
                    {
                        int j = 0, k = 0;

                        while (j < data.savetmpdata.Count())
                        {
                            while (k < data.savetmpdata1.Count())
                            {
                                data.fillheaddata = (from a in _ClgAdmissionContext.CollegeConcessionDMO
                                                     from b in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                                     where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id==data.savetmpdata1[k].FTI_Id)
                                                     select new CollegeConcessionDTO
                                                     {
                                                         FCSC_Id = a.FCSC_Id
                                                     }
               ).Distinct().ToArray();

                                CollegeConcessionDMO pmm = new CollegeConcessionDMO();
                                if (data.fillheaddata.Length <= 0)
                                {
                                    pmm.AMCST_Id = data.savetmpdata[j].AMCST_Id;
                                    pmm.FCMAS_Id = data.savetmpdata1[k].FCMAS_Id;
                                    pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                    pmm.MI_Id = data.MI_Id;
                                    pmm.FMCC_Id = 2;
                                    pmm.ASMAY_Id = data.ASMAY_Id;
                                    pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                    pmm.FCSC_ConcessionReason = data.savetmpdata1[k].FCSC_ConcessionReason;
                                    pmm.FCSC_ConcessionType = data.savetmpdata1[k].FCSC_ConcessionType;
                                    pmm.FCSC_ActiveFlag = true;
                                    pmm.CreatedDate = DateTime.Now;
                                    pmm.UpdatedDate = DateTime.Now;

                                    _ClgAdmissionContext.Add(pmm);
                                    var contactExists = _ClgAdmissionContext.SaveChanges();
                                }
                                else if (data.fillheaddata.Length == 1)
                                {
                                    var fetchfscid = (from a in _ClgAdmissionContext.CollegeConcessionDMO
                                                      from b in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                      select new CollegeConcessionDTO
                                                      {
                                                          FCSC_Id = a.FCSC_Id
                                                      }
              ).Distinct().SingleOrDefault();

                                    pmm.FCSC_Id = fetchfscid.FCSC_Id;
                                }

                                CollegeConcessionInstallmentDMO pmmins = new CollegeConcessionInstallmentDMO();
                                var resultt = (from a in _ClgAdmissionContext.CollegeConcessionDMO
                                               from b in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                               select new CollegeConcessionInstallmentDMO
                                               {
                                                   FSCI_Id = b.FSCI_Id
                                               }).SingleOrDefault();

                                if (resultt == null)
                                {


                                    pmmins.FCSC_Id = pmm.FCSC_Id;
                                    pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                    pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    pmmins.CreatedDate = DateTime.Now;
                                    pmmins.UpdatedDate = DateTime.Now;

                                    _ClgAdmissionContext.Add(pmmins);


                                    var status_stu = _ClgAdmissionContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.savetmpdata[j].AMCST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id && t.FCMAS_Id == data.savetmpdata1[k].FCMAS_Id);

                                    status_stu.FCSS_ToBePaid = (status_stu.FCSS_ToBePaid + status_stu.FCSS_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    status_stu.FCSS_TotalCharges = status_stu.FCSS_TotalCharges;
                                    status_stu.FCSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    _ClgAdmissionContext.Update(status_stu);

                                    _ClgAdmissionContext.SaveChanges();




                                }
                                else
                                {
                                    var resultupdate = _ClgAdmissionContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == resultt.FSCI_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    resultupdate.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    _ClgAdmissionContext.Update(resultupdate);

                                    var status_stu = _ClgAdmissionContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.savetmpdata[j].AMCST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id && t.FCMAS_Id == data.savetmpdata1[k].FCMAS_Id);

                                    status_stu.FCSS_ToBePaid = (status_stu.FCSS_ToBePaid + status_stu.FCSS_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    status_stu.FCSS_TotalCharges = status_stu.FCSS_TotalCharges;
                                    status_stu.FCSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    _ClgAdmissionContext.Update(status_stu);

                                    _ClgAdmissionContext.SaveChanges();
                                }
                                    k++;
                                }
                                j++;
                            }
                            transaction.Commit();
                        }
                    }
                }
            
            catch (Exception e)
            {
                data.returnval = "false";
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CollegeConcessionDTO DeletRecord(CollegeConcessionDTO data)
        {
            try
            {
                var FCSC_Id = _ClgAdmissionContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == data.FSCI_ID).FCSC_Id;
                var AMCST_Id = _ClgAdmissionContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).AMCST_Id;
                var fmgid = _ClgAdmissionContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;
                var fmhid = _ClgAdmissionContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;
                var ftiid = _ClgAdmissionContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == data.FSCI_ID && t.FCSC_Id == FCSC_Id).FTI_Id;
                var paidamount = _ClgAdmissionContext.Fee_College_Student_StatusDMO.Single(t => t.AMCST_Id == AMCST_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FCSS_PaidAmount;

                //AMST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  


                //data.result = (from a in _ClgAdmissionContext.CollegeConcessionDMO
                //               from b in _ClgAdmissionContext.CollegeConcessionInstallmentDMO
                //               where (a.FCSC_Id == b.FCSC_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //               select new CollegeConcessionDTO
                //               {
                //                   AMCST_Id = a.AMCST_Id,
                //                   FSCI_ID = b.FSCI_Id,
                //                   FCSC_Id = b.FSCI_Id,

                //               }).).Distinct().ToArray();




                if (paidamount == 0)
                    {
                        _ClgAdmissionContext.Database.ExecuteSqlCommand("Collegedeleteconcession @p0,@p1,@p2,@p3,@p4", data.FSCI_ID, data.ASMAY_Id, data.MI_Id, data.userid, FCSC_Id);
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "paid";
                    }
                
              

            }
            catch (Exception ex)
            {
                data.returnval = "false";
                Console.WriteLine(ex.Message);
            }

            return data;
        }

    }
}


