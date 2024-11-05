using System;
using CollegeFeeService.com.vaps.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using AutoMapper;
using DomainModel.Model.com.vapstech.College.Fees;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CommonLibrary;


namespace CollegeFeeService.com.vaps.Implementation
{
    public class Clg_StudentFeeGroupMappingImpl : Clg_StudentFeeGroupMappingInterface
    {
        private static ConcurrentDictionary<string, Clg_StudentFeeGroupMapping_DTO> _login =
             new ConcurrentDictionary<string, Clg_StudentFeeGroupMapping_DTO>();

        public CollFeeGroupContext CollFeeGroupContext;

        public Clg_StudentFeeGroupMappingImpl(CollFeeGroupContext objDbcontext)
        {
            CollFeeGroupContext = objDbcontext;
        }

        public Clg_StudentFeeGroupMapping_DTO GetYearList(int id)
        {
            Clg_StudentFeeGroupMapping_DTO data = new Clg_StudentFeeGroupMapping_DTO();
            try
            {
                data.yearlist = CollFeeGroupContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active).Distinct().ToArray();


                data.schemeType = CollFeeGroupContext.AdmCollegeSchemeTypeDMO.Where(d => d.MI_Id == id && d.ACST_ActiveFlg == true).Distinct().ToArray();
                data.termList = CollFeeGroupContext.Clg_Adm_College_QuotaDMO.Where(d => d.MI_Id == id && d.ACQ_ActiveFlg == true).Distinct().ToArray();
                

            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO get_courses(Clg_StudentFeeGroupMapping_DTO data)
        {

            try
            {

                data.courselist = (from a in CollFeeGroupContext.MasterCourseDMO
                                   from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true
                                   && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                   && b.ACAYC_ActiveFlag == true && b.AMCO_Id == a.AMCO_Id)
                                   select new Clg_StudentFeeGroupMapping_DTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName
                                   }).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO get_branches(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                var branchlist = (from a in CollFeeGroupContext.ClgMasterBranchDMO
                                  from b in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag == true
                                  && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                  && b.ACAYC_ActiveFlag == true && data.AMCO_Ids.Contains(b.AMCO_Id)
                                  && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id
                                 && c.ACAYCB_ActiveFlag == true && a.AMB_Id==c.AMB_Id)
                                  select new Clg_StudentFeeGroupMapping_DTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_Order = a.AMB_Order,
                                      //AMB_BranchCode = a.AMB_BranchCode,
                                      //AMB_BranchInfo = a.AMB_BranchInfo,
                                      //AMB_BranchType = a.AMB_BranchType,
                                      //AMB_StudentCapacity = a.AMB_StudentCapacity,

                                      //AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO get_semisters(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                data.semesterlist = (from a in CollFeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                     from b in CollFeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from c in CollFeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from d in CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                     where (a.ACAYC_Id == b.ACAYC_Id && a.MI_Id == data.MI_Id && a.ACAYC_ActiveFlag == true
                                     && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Ids.Contains(a.AMCO_Id)
                                     && b.ACAYCB_Id == c.ACAYCB_Id && data.AMB_Ids.Contains(b.AMB_Id)
                                     && b.ACAYCB_ActiveFlag == true && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                    && d.MI_Id == data.MI_Id && d.AMSE_ActiveFlg == true)
                                     select new Clg_StudentFeeGroupMapping_DTO
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

        public Clg_StudentFeeGroupMapping_DTO get_report(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                data.studentlist = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                    from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                    from c in CollFeeGroupContext.AdmCollegeSchemeTypeDMO
                                    from d in CollFeeGroupContext.MasterCourseDMO
                                    from e in CollFeeGroupContext.ClgMasterBranchDMO
                                    from f in CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCST_Id == b.AMCST_Id && b.ACST_Id == c.ACST_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Ids.Contains(a.AMCO_Id) && data.AMB_Ids.Contains(a.AMB_Id) && a.ACYST_ActiveFlag == 1 && data.AMSE_Ids.Contains(a.AMSE_Id) && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag && c.ACST_Id == data.ACST_Id && b.ACQ_Id == data.FOPM_Id)
                                    select new Clg_StudentFeeGroupMapping_DTO
                                    {
                                        ACAYC_Id = a.ACYST_Id,
                                        AMCST_Id = a.AMCST_Id,
                                        ACYST_RollNo = a.ACYST_RollNo,
                                        AMCST_FirstName = b.AMCST_FirstName,
                                        AMCST_MiddleName = b.AMCST_MiddleName,
                                        AMCST_LastName = b.AMCST_LastName,
                                        AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                        AMCST_AdmNo = b.AMCST_AdmNo,
                                        AMCO_CourseName = d.AMCO_CourseName,
                                        AMB_BranchName = e.AMB_BranchName,
                                        AMSE_SEMName = f.AMSE_SEMName
                                    }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();

                data.fillmastergroup = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in CollFeeGroupContext.FeeGroupClgDMO
                                        from f in CollFeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO

                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == data.user_id && g.MI_Id == data.MI_Id)
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                data.fillmasterhead = (from a in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                       from b in CollFeeGroupContext.FeeGroupClgDMO
                                       from c in CollFeeGroupContext.FeeHeadClgDMO
                                       from g in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                       where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FMH_ActiveFlag == true && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == a.FMG_Id && g.FMH_Id == a.FMH_Id)
                                       select new Clg_StudentFeeGroupMapping_DTO
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
                                        select new Clg_StudentFeeGroupMapping_DTO
                                        {
                                            FMG_Id = b.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = e.FTI_Id,
                                            FTI_Name = e.FTI_Name
                                        }).Distinct().ToArray();

                data.Studentreport = (from a in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                      from b in CollFeeGroupContext.Adm_Master_College_StudentDMO
                                      from c in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                      from d in CollFeeGroupContext.FeeGroupClgDMO
                                      from e in CollFeeGroupContext.AcademicYear
                                      from f in CollFeeGroupContext.AdmCollegeSchemeTypeDMO
                                      where (a.AMCST_Id == b.AMCST_Id && b.ACST_Id==f.ACST_Id && a.ASMAY_Id == data.ASMAY_Id && data.AMCO_Ids.Contains(a.AMCO_Id) && data.AMB_Ids.Contains(a.AMB_Id) && data.AMSE_Ids.Contains(a.AMSE_Id) && a.AMCST_Id == c.AMCST_Id && a.ACYST_ActiveFlag == 1 && c.FMG_Id == d.FMG_Id && c.MI_Id == d.MI_Id && d.FMG_ActiceFlag == true && a.ASMAY_Id == e.ASMAY_Id && e.MI_Id == data.MI_Id && b.ACST_Id==data.ACST_Id && c.ASMAY_Id==data.ASMAY_Id)
                                      select new Clg_StudentFeeGroupMapping_DTO
                                      {
                                          ACAYC_Id = a.ACYST_Id,
                                          AMCST_Id = a.AMCST_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          ASMAY_Year = e.ASMAY_Year,
                                          FMG_Id = c.FMG_Id,
                                          FCMSGH_Id = c.FCMSGH_Id,
                                          ACYST_RollNo = a.ACYST_RollNo,
                                          AMCST_FirstName = b.AMCST_FirstName,
                                          AMCST_MiddleName = b.AMCST_MiddleName,
                                          AMCST_LastName = b.AMCST_LastName,
                                          AMCST_RegistrationNo = b.AMCST_RegistrationNo,
                                          AMCST_AdmNo = b.AMCST_AdmNo,
                                          FMG_GroupName = d.FMG_GroupName

                                      }).Distinct().OrderBy(t => t.ACYST_RollNo).ToArray();

            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO DeleteRecord(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {

                var remlve = CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO.Single(t => t.FCMSGH_Id == data.FCMSGH_Id);
                var already_cnt = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == remlve.ASMAY_Id && t.AMCST_Id == remlve.AMCST_Id && t.FMG_Id == remlve.FMG_Id && t.FCSS_PaidAmount > 0).ToList().Count;
                if (already_cnt == 0)
                {

                    var remlve1 = CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO.Where(t => t.FCMSGH_Id == data.FCMSGH_Id).ToList();
                    if (remlve1.Any())
                    {
                        for (int y = 0; remlve1.Count > y; y++)
                        {
                            CollFeeGroupContext.Remove(remlve1.ElementAt(y));
                        }
                    }
                    var remlve2 = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == remlve.ASMAY_Id && t.AMCST_Id == remlve.AMCST_Id && t.FMG_Id == remlve.FMG_Id).ToList();
                    if (remlve2.Any())
                    {
                        for (int z = 0; remlve2.Count > z; z++)
                        {
                            CollFeeGroupContext.Remove(remlve2.ElementAt(z));
                        }
                    }

                    CollFeeGroupContext.Remove(remlve);

                    var exists = CollFeeGroupContext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    data.returnval = "Depend";
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Clg_StudentFeeGroupMapping_DTO savedata(Clg_StudentFeeGroupMapping_DTO data)        {            try            {                var checkconfiguration = CollFeeGroupContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();                if (checkconfiguration[0].FMC_BtachwiseFeeGlg == true)                {                    foreach (var a1 in data.unselectedlist)                    {                        var remlve2 = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.FMG_Id == a1.FMG_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.studentdata[0].AMCST_Id && t.FCSS_PaidAmount == 0).ToList();                        if (remlve2.Any())                        {                            for (int Z = 0; Z < remlve2.Count; Z++)                            {                                CollFeeGroupContext.Remove(remlve2.ElementAt(Z));                                var exists = CollFeeGroupContext.SaveChanges();                                if (exists >= 1)                                {                                    data.returnval = "true";                                }                                else                                {                                    data.returnval = "false";                                }                            }

                        }                    }

                    foreach (var a1 in data.studentdata)                    {                        var semid = CollFeeGroupContext.Adm_College_Yearly_StudentDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id && t.ACYST_ActiveFlag == 1).Select(t => t.AMSE_Id).ToList();                        foreach (var a2 in data.savegrplst)                        {
                            var exists = CollFeeGroupContext.Database.ExecuteSqlCommand("Batchwise_Student_feeGroup_Mapping_Regular @p0,@p1,@p2,@p3,@p4", data.MI_Id, a1.AMCST_Id, data.ASMAY_Id, semid[0], a2.FMG_Id);                            if (exists >= 1)                            {                                data.returnval = "true";                            }                            else                            {                                data.returnval = "false";                            }                        }                    }


                }                else                {                    foreach (var a1 in data.studentdata)                    {                        var semid = CollFeeGroupContext.Adm_College_Yearly_StudentDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id && t.ACYST_ActiveFlag == 1).OrderByDescending(t => t.AMSE_Id).Select(t => t.AMSE_Id).FirstOrDefault();                        var remlve = CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id).ToList();

                        //Existing groups should not be removed.
                        //if (remlve.Any())
                        //{
                        //    for (int x = 0; remlve.Count > x; x++)
                        //    {
                        //        var remlve1 = CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO.Where(t => t.FCMSGH_Id == remlve[x].FCMSGH_Id).ToList();
                        //        if (remlve1.Any())
                        //        {
                        //            for (int y = 0; remlve1.Count > y; y++)
                        //            {
                        //                var remlve2 = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.FMG_Id == remlve[x].FMG_Id && t.FMH_Id == remlve1[x].FMH_Id && t.FTI_Id == remlve1[x].FTI_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.studentdata[0].AMCST_Id).ToList();
                        //                if (remlve2.Any())
                        //                {
                        //                    for (int Z = 0; remlve2.Count > Z; Z++)
                        //                    {
                        //                        CollFeeGroupContext.Remove(remlve2.ElementAt(Z));
                        //                    }
                        //                }
                        //                CollFeeGroupContext.Remove(remlve1.ElementAt(y));
                        //            }
                        //        }
                        //        CollFeeGroupContext.Remove(remlve.ElementAt(x));
                        //    }
                        //}
                        //Existing groups should not be removed.

                        List<Clg_StudentFeeGroupMapping_DTO> saved_list = new List<Clg_StudentFeeGroupMapping_DTO>();                        foreach (var a2 in data.savegrplst)                        {                            Fee_College_Master_Student_GroupHeadDMO obj1 = new Fee_College_Master_Student_GroupHeadDMO();                            obj1.MI_Id = data.MI_Id;                            obj1.ASMAY_Id = data.ASMAY_Id;                            obj1.AMCST_Id = a1.AMCST_Id;                            obj1.FMG_Id = a2.FMG_Id;                            obj1.FCMSGH_ActiveFlag = true;                            CollFeeGroupContext.Add(obj1);                            foreach (var a3 in data.saveheadlst)                            {                                if (a3.FMG_Id == a2.FMG_Id)                                {                                    foreach (var a4 in data.saveftilst)                                    {                                        if (a4.FMH_Id == a3.FMH_Id && a4.FMG_Id == a2.FMG_Id)                                        {                                            Fee_C_Master_Student_GroupHead_InstallmentsDMO obj2 = new Fee_C_Master_Student_GroupHead_InstallmentsDMO();                                            obj2.FCMSGH_Id = obj1.FCMSGH_Id;                                            obj2.FMH_Id = a3.FMH_Id;                                            obj2.FTI_Id = a4.FTI_Id;                                            CollFeeGroupContext.Add(obj2);                                            Clg_StudentFeeGroupMapping_DTO obj_saved = new Clg_StudentFeeGroupMapping_DTO();                                            obj_saved.FMG_Id = a2.FMG_Id;                                            obj_saved.FMH_Id = a3.FMH_Id;                                            obj_saved.FTI_Id = a4.FTI_Id;                                            saved_list.Add(obj_saved);                                            var FCMAS_Idnew = (from a in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                                               from b in CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                               from c in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                                               where (c.AMCST_Id == a1.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == a2.FMG_Id && a.FMH_Id == a3.FMH_Id && a.FTI_Id == a4.FTI_Id && b.AMSE_Id == semid)
                                                               select b).ToList();                                            var status_list = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id && t.FMG_Id == a2.FMG_Id && t.FMH_Id == a3.FMH_Id && t.FTI_Id == a4.FTI_Id && t.FCMAS_Id == FCMAS_Idnew[0].FCMAS_Id).ToList();                                            if (status_list.Count == 0)                                            {                                                var FCMAS_Id = (from a in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO                                                                from b in CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise                                                                from c in CollFeeGroupContext.Adm_College_Yearly_StudentDMO                                                                where (c.AMCST_Id == a1.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == a2.FMG_Id && a.FMH_Id == a3.FMH_Id && a.FTI_Id == a4.FTI_Id && b.AMSE_Id == semid)                                                                select b).ToList();                                                Fee_College_Student_StatusDMO status_obj = new Fee_College_Student_StatusDMO();                                                if (FCMAS_Id.Count() > 0)
                                                {
                                                    status_obj.MI_Id = data.MI_Id;
                                                    status_obj.ASMAY_Id = data.ASMAY_Id;
                                                    status_obj.AMCST_Id = a1.AMCST_Id;
                                                    status_obj.FMG_Id = a2.FMG_Id;
                                                    status_obj.FMH_Id = a3.FMH_Id;
                                                    status_obj.FTI_Id = a4.FTI_Id;
                                                    status_obj.FCMAS_Id = FCMAS_Id[0].FCMAS_Id;
                                                    status_obj.FCSS_OBArrearAmount = 0;
                                                    status_obj.FCSS_OBExcessAmount = 0;
                                                    status_obj.FCSS_CurrentYrCharges = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                    status_obj.FCSS_TotalCharges = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                    status_obj.FCSS_ConcessionAmount = 0;
                                                    status_obj.FCSS_WaivedAmount = 0;
                                                    status_obj.FCSS_ToBePaid = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                    status_obj.FCSS_PaidAmount = 0;
                                                    status_obj.FCSS_RefundableAmount = 0;
                                                    status_obj.FCSS_ExcessPaidAmount = 0;
                                                    status_obj.FCSS_ExcessAmountAdjusted = 0;
                                                    status_obj.FCSS_RunningExcessAmount = 0;
                                                    status_obj.FCSS_AdjustedAmount = 0;
                                                    status_obj.FCSS_RebateAmount = 0;
                                                    status_obj.FCSS_FineAmount = 0;
                                                    status_obj.FCSS_RefundAmount = 0;
                                                    status_obj.FCSS_RefundAmountAdjusted = 0;
                                                    status_obj.FCSS_NetAmount = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                    status_obj.FCSS_ChequeBounceFlg = false;
                                                    status_obj.FCSS_ArrearFlag = false;
                                                    status_obj.FCSS_RefundOverFlag = false;
                                                    status_obj.FCSS_ActiveFlag = true;
                                                    status_obj.User_Id = data.user_id;
                                                    CollFeeGroupContext.Add(status_obj);
                                                }                                            }                                            else if (status_list.Count == 1)                                            {                                                var stu_status_obj = CollFeeGroupContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id && t.FMG_Id == a2.FMG_Id && t.FMH_Id == a3.FMH_Id && t.FTI_Id == a4.FTI_Id);                                                if ((stu_status_obj.FCSS_OBArrearAmount > 0 || stu_status_obj.FCSS_OBExcessAmount > 0) && stu_status_obj.FCSS_NetAmount == 0)                                                {                                                    var FCMAS_Id = (from a in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO                                                                    from b in CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise                                                                    from c in CollFeeGroupContext.Adm_College_Yearly_StudentDMO                                                                    where (c.AMCST_Id == a1.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id && a.FMG_Id == a2.FMG_Id && a.FMH_Id == a3.FMH_Id && a.FTI_Id == a4.FTI_Id)                                                                    select b).ToList();
                                                    if (FCMAS_Id.Count() > 0)
                                                    {
                                                        //stu_status_obj.MI_Id = data.MI_Id;
                                                        //stu_status_obj.ASMAY_Id = data.ASMAY_Id;
                                                        //stu_status_obj.AMCST_Id = a1.AMCST_Id;
                                                        //stu_status_obj.FMG_Id = a2.FMG_Id;
                                                        //stu_status_obj.FMH_Id = a3.FMH_Id;
                                                        //stu_status_obj.FTI_Id = a4.FTI_Id;
                                                        stu_status_obj.FCMAS_Id = FCMAS_Id[0].FCMAS_Id;
                                                        //stu_status_obj.FCSS_OBArrearAmount = 0;
                                                        //stu_status_obj.FCSS_OBExcessAmount = 0;
                                                        stu_status_obj.FCSS_CurrentYrCharges = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                        stu_status_obj.FCSS_TotalCharges = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                        stu_status_obj.FCSS_ConcessionAmount = 0;
                                                        stu_status_obj.FCSS_WaivedAmount = 0;
                                                        stu_status_obj.FCSS_ToBePaid = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                        stu_status_obj.FCSS_PaidAmount = 0;
                                                        stu_status_obj.FCSS_RefundableAmount = 0;
                                                        stu_status_obj.FCSS_ExcessPaidAmount = 0;
                                                        stu_status_obj.FCSS_ExcessAmountAdjusted = 0;
                                                        //stu_status_obj.FCSS_RunningExcessAmount = 0;
                                                        stu_status_obj.FCSS_AdjustedAmount = 0;
                                                        stu_status_obj.FCSS_RebateAmount = 0;
                                                        stu_status_obj.FCSS_FineAmount = 0;
                                                        stu_status_obj.FCSS_RefundAmount = 0;
                                                        stu_status_obj.FCSS_RefundAmountAdjusted = 0;
                                                        stu_status_obj.FCSS_NetAmount = Convert.ToInt64(FCMAS_Id[0].FCMAS_Amount);
                                                        //status_obj.FCSS_ChequeBounceAmount = 0;
                                                        stu_status_obj.FCSS_ChequeBounceFlg = false;
                                                        stu_status_obj.FCSS_ArrearFlag = false;
                                                        stu_status_obj.FCSS_RefundOverFlag = false;
                                                        stu_status_obj.FCSS_ActiveFlag = true;
                                                        stu_status_obj.User_Id = data.user_id;
                                                        CollFeeGroupContext.Update(stu_status_obj);
                                                    }                                                }                                            }                                        }                                    }                                }                            }                        }

                        //var status_del_lst = (from a in CollFeeGroupContext.Fee_College_Student_StatusDMO
                        //                      from b in saved_list
                        //                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == a1.AMCST_Id && a.FMG_Id != b.FMG_Id && a.FMH_Id != b.FMH_Id && a.FTI_Id != b.FTI_Id)
                        //                      select a).ToList();

                        //commented on 13-04-2019
                        //var status_all = CollFeeGroupContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == a1.AMCST_Id).Distinct().ToList();
                        //var status_saved_lst = (from a in CollFeeGroupContext.Fee_College_Student_StatusDMO
                        //                        from b in saved_list
                        //                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == a1.AMCST_Id && a.FMG_Id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id)
                        //                        select a).Distinct().ToList();
                        //var status_del_lst = (from a in status_all
                        //                      from b in status_saved_lst
                        //                      where (a.FCSS_Id != b.FCSS_Id)
                        //                      select a).ToList();

                        //if (status_del_lst.Any())
                        //{
                        //    for (int p = 0; status_del_lst.Count > p; p++)
                        //    {
                        //        CollFeeGroupContext.Remove(status_del_lst.ElementAt(p));
                        //    }
                        //}

                        //commented on 13-04-2019

                    }                    var exists = CollFeeGroupContext.SaveChanges();                    if (exists >= 1)                    {                        data.returnval = "true";                    }                    else                    {                        data.returnval = "false";                    }                }            }            catch (Exception ee)            {                data.returnval = "Error";                Console.WriteLine(ee.Message);            }            return data;        }


        public Clg_StudentFeeGroupMapping_DTO editdata(Clg_StudentFeeGroupMapping_DTO data)
        {
            try
            {
                //data.editdatalist= (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                //                    from b in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                //                    where (a.FCMSGH_Id == b.FCMSGH_Id && a.ASMAY_Id == data.ASMAY_Id
                //                    && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id 
                //                    && a.FCMSGH_ActiveFlag == true)

                //                    select new Clg_StudentFeeGroupMapping_DTO
                //                    {

                //                        AMSE_Id=a.ASMAY_Id,
                //                        MI_Id = a.MI_Id,
                //                        AMCST_Id =a.AMCST_Id,
                //                        FMG_Id =a.FMG_Id,
                //                        FCMSGHI_Id=b.FCMSGHI_Id,
                //                        FMH_Id=b.FMH_Id,
                //                        FTI_Id=b.FTI_Id
                //                    }).Distinct().ToArray();

                data.editdatalist = (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                     from b in CollFeeGroupContext.FeeGroupClgDMO
                                     from c in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                                     from g in CollFeeGroupContext.FeeHeadClgDMO
                                     from d in CollFeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                     from e in CollFeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping
                                     from f in CollFeeGroupContext.Fee_College_Student_StatusDMO
                                     where (f.AMCST_Id == a.AMCST_Id && f.ASMAY_Id == a.ASMAY_Id && f.MI_Id == a.MI_Id && a.FMG_Id == f.FMG_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == f.FTI_Id && a.FCMSGH_Id == c.FCMSGH_Id && a.FMG_Id == b.FMG_Id && c.FMH_Id == g.FMH_Id && c.FTI_Id == d.FTI_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FMG_Id == a.FMG_Id && e.FMH_Id == c.FMH_Id && e.FMI_Id == d.FMI_Id)

                                     select new Clg_StudentFeeGroupMapping_DTO
                                     {
                                         AMCST_Id = a.AMCST_Id,
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = b.FMG_GroupName,
                                         FMH_Id = g.FMH_Id,
                                         FMH_FeeName = g.FMH_FeeName,
                                         FCMSGH_Id = a.FCMSGH_Id,
                                         FTI_Id = d.FTI_Id,
                                         FTI_Name = d.FTI_Name,
                                         FCSS_PaidAmount = f.FCSS_PaidAmount
                                     }
                ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //saveeditdata
        public Clg_StudentFeeGroupMapping_DTO saveeditdata(Clg_StudentFeeGroupMapping_DTO pgmod)
        {
            try
            {
                string returntxt = "";
                if (pgmod.AMCST_Id != 0)
                {
                    int G = 0, H = 0, I = 0;
                    var FCMA_Idclg = (from a in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                      from b in CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                      from c in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                      where (a.FCMA_Id == b.FCMA_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && c.AMCST_Id == pgmod.AMCST_Id && c.ASMAY_Id == pgmod.ASMAY_Id)
                                      select a.FCMA_Id).FirstOrDefault();
                    if (FCMA_Idclg == 0)
                    {
                        returntxt = "a";
                    }
                    else
                    {
                        while (G < pgmod.savegrplst.Count())
                        {

                            while (H < pgmod.saveheadlst.Count())
                            {
                                while (I < pgmod.saveftilst.Count())
                                {

                                    if (pgmod.saveftilst[I].disableins == false)
                                    {
                                        if (pgmod.saveftilst[I].FMH_Id == pgmod.saveheadlst[H].FMH_Id && pgmod.saveftilst[I].FMG_Id == pgmod.savegrplst[G].FMG_Id && pgmod.savegrplst[G].FMG_Id == pgmod.saveheadlst[H].FMG_Id)
                                        {
                                            if (pgmod.savegrplst[G].checkedgrplstedit == true && pgmod.saveheadlst[H].checkedheadlstedit == true && pgmod.saveftilst[I].checkedinstallmentlstedit == true)
                                            {
                                                var checkforduplicates1 = (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                                                           from b in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                                                                           from c in CollFeeGroupContext.Fee_College_Student_StatusDMO
                                                                           where (a.FCMSGH_Id == b.FCMSGH_Id && a.FMG_Id == c.FMG_Id && a.AMCST_Id == c.AMCST_Id && a.FMG_Id == c.FMG_Id && b.FTI_Id == c.FTI_Id && c.AMCST_Id == pgmod.AMCST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                           select b.FCMSGH_Id).Distinct().ToList();
                                                if (checkforduplicates1.Count().Equals(0))
                                                {
                                                    var FMAlist = (from a in CollFeeGroupContext.Clg_Fee_AmountEntry_DMO
                                                                   from b in CollFeeGroupContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                                   from c in CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                                                   where (a.FCMA_Id == b.FCMA_Id && c.ASMAY_Id == pgmod.ASMAY_Id && c.ACYST_ActiveFlag == 1 && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == c.AMB_Id && b.FCMA_Id == a.FCMA_Id && b.AMSE_Id == c.AMSE_Id)
                                                                   select a.FCMA_Id).ToList();

                                                    if (FMAlist.Distinct().Count().Equals(0))
                                                    {
                                                        returntxt = "a";
                                                    }
                                                    else
                                                    {
                                                        using (var cmd = CollFeeGroupContext.Database.GetDbConnection().CreateCommand())
                                                        {
                                                            cmd.CommandText = "Insert_Fee_Student_Mapnew_Clg";
                                                            cmd.CommandType = CommandType.StoredProcedure;
                                                            cmd.Parameters.Add(new SqlParameter("@fmg_id",
                                                                SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMG_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                                               SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.AMCST_Id
                                                            });
                                                            cmd.Parameters.Add(new SqlParameter("@MI_ID",
                                                           SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.MI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@fti_id_new",
                                                        SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FTI_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@FMH_ID_new",
                                                              SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.saveftilst[I].FMH_Id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@userid",
                                                          SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.user_id
                                                            });

                                                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                                             SqlDbType.BigInt)
                                                            {
                                                                Value = pgmod.ASMAY_Id
                                                            });

                                                            if (cmd.Connection.State != ConnectionState.Open)
                                                                cmd.Connection.Open();
                                                            var data = cmd.ExecuteNonQuery();

                                                            if (data >= 1)
                                                            {
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                var checkforduplicatesdel = (from a in CollFeeGroupContext.Fee_College_Master_Student_GroupHeadDMO
                                                                             from b in CollFeeGroupContext.Fee_C_Master_Student_GroupHead_InstallmentsDMO
                                                                             from c in CollFeeGroupContext.Fee_College_Student_StatusDMO
                                                                             where (a.FCMSGH_Id == b.FCMSGH_Id && a.FMG_Id == c.FMG_Id && a.AMCST_Id == c.AMCST_Id && a.FMG_Id == c.FMG_Id && b.FTI_Id == c.FTI_Id && c.AMCST_Id == pgmod.AMCST_Id && c.FMG_Id == pgmod.saveftilst[I].FMG_Id && c.FMH_Id == pgmod.saveftilst[I].FMH_Id && c.FTI_Id == pgmod.saveftilst[I].FTI_Id && a.ASMAY_Id == pgmod.ASMAY_Id)
                                                                             select new { a.ASMAY_Id, a.MI_Id, a.FMG_Id, a.AMCST_Id, b.FCMSGH_Id, b.FMH_Id, b.FTI_Id }).Distinct().ToList();
                                                if (checkforduplicatesdel.Count > 0)
                                                {
                                                    foreach (var a in checkforduplicatesdel)
                                                    {
                                                        using (var transaction = CollFeeGroupContext.Database.BeginTransaction())
                                                        {

                                                            var outputval = CollFeeGroupContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMappingnew_Clg @p0,@p1,@p2,@p3,@p4,@p5,@p6", a.MI_Id, a.AMCST_Id, a.ASMAY_Id, a.FMG_Id, a.FCMSGH_Id, a.FMH_Id, a.FTI_Id);

                                                            if (outputval >= 1)
                                                            {
                                                                transaction.Commit();
                                                                pgmod.returnval = "true";
                                                            }
                                                            else
                                                            {
                                                                pgmod.returnval = "false";
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    I++;
                                }
                                I = 0;
                                H++;
                            }
                            H = 0;
                            G++;
                        }
                    }


                }
                if (returntxt != "")
                {
                    pgmod.returnval = "false";
                }               
            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return pgmod;
        }

    }
}



