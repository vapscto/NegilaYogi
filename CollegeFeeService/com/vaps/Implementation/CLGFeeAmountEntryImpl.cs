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
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeAmountEntryImpl : interfaces.CLGFeeAmountEntryInterfaces
    {
        private static ConcurrentDictionary<string, CLGFeeAmountEntryDTO> _login =
            new ConcurrentDictionary<string, CLGFeeAmountEntryDTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<CLGFeeAmountEntryImpl> _logger;
        public CLGFeeAmountEntryImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<CLGFeeAmountEntryImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public CLGFeeAmountEntryDTO Getinitialformload(CLGFeeAmountEntryDTO fee)
        {
            try
            {
                CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);

                fee.academicdrp = clgcomm.Get_All_Academicyear(fee.MI_Id);

                fee.currfillyear = clgcomm.Get_Current_Academicyear(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillcourse = clgcomm.Get_Yearly_Course(fee.MI_Id, fee.ASMAY_Id);

                fee.Fillgroup = clgcomm.Get_Yearly_groups(fee.MI_Id, fee.ASMAY_Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return fee;
        }

        public CLGFeeAmountEntryDTO getbranchdetails(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                data.fillbranch = clgcomm.Get_Yearly_Course_Branch(data.MI_Id, data.ASMAY_Id, data.AMCO_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeAmountEntryDTO getsemesterdetails(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                data.fillsemester = clgcomm.Get_Yearly_Course_Branch_Semesters(data.MI_Id, data.ASMAY_Id, data.AMCO_Id, data.AMB_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGFeeAmountEntryDTO getcourdetails(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                data.Fillcourseyearwise = clgcomm.Get_Yearly_Course(data.MI_Id, data.ASMAY_Id);

                data.Fillgroup = clgcomm.Get_Yearly_groups(data.MI_Id, data.ASMAY_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeAmountEntryDTO getgroupmapped(CLGFeeAmountEntryDTO data)
        {
            CollegeCommonDetails clgcomm = new CollegeCommonDetails(_YearlyFeeGroupMappingContext);
            try
            {
                List<long> semids = new List<long>();
                
                var semisterlist = (from a in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                from b in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_CourseDMO
                                from c in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_BranchDMO
                                from d in _YearlyFeeGroupMappingContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                where (a.MI_Id==data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id==data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id==data.MI_Id && c.AMB_Id==data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id==data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                select new CLG_Adm_Master_SemesterDMO
                                {
                                    AMSE_Id = a.AMSE_Id,
                                    AMSE_SEMName = a.AMSE_SEMName
                                }).ToList();


                foreach (var x in semisterlist)
                {

                    semids.Add(x.AMSE_Id);
                }
                List<CLG_Fee_Yearly_Group_Head_Mapping> commamtflag = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
                commamtflag = _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.MI_Id == data.MI_Id && t.FYGHM_ActiveFlag == "1" && t.FYGHM_Common_AmountFlag == "Y" && t.FMG_Id == data.FMG_Id).ToList();
                data.commountamountflag = commamtflag.ToArray();


                data.newlyupdatedrec = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                        from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id)
                                        select new CLGFeeAmountEntryDTO
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
                data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                         from g in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                         from i in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                         where (i.MI_Id == data.MI_Id && f.FMH_Id == c.FMH_Id && f.FMG_Id == b.FMG_Id && f.FTI_Id == e.FTI_Id && a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && f.AMCO_Id == data.AMCO_Id && f.AMB_Id == data.AMB_Id && d.FMI_Id == e.FMI_Id && f.FCMA_Id == g.FCMA_Id
                                         && g.FCMAS_Id == h.FCMAS_Id && f.FMG_Id == data.FMG_Id && i.AMSE_Id == g.AMSE_Id && f.ASMAY_Id == data.ASMAY_Id && semids.Contains(i.AMSE_Id))
                                         select new CLGFeeAmountEntryDTO
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
                                             FCTDD_Year = h.FCTDD_Year,
                                             FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                             FMH_Order = c.FMH_Order,
                                             AMSE_SEMName = i.AMSE_SEMName,
                                             AMSE_Id = i.AMSE_Id,
                                             AMSE_SEMOrder = i.AMSE_SEMOrder,
                                             FCMAS_DueDate = g.FCMAS_DueDate,
                                             FCMA_Id = f.FCMA_Id,
                                         }
      ).OrderBy(t => t.AMSE_SEMOrder).ThenBy(t=>t.FMH_Order).ToArray();


                data.fineslabdetails = (from a in _YearlyFeeGroupMappingContext.feeFS
                                        from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Fine_Slabs
                                        from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        where (a.FMFS_Id == b.FMFS_Id && b.FCMAS_Id == d.FCMAS_Id && c.FCMA_Id == d.FCMA_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == data.FMG_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id)
                                        select new CLGFeeAmountEntryDTO
                                        {
                                            FCMAS_Id = d.FCMAS_Id,
                                            FTFS_Amount = b.FCTFS_Amount,
                                            FMFS_FineType = a.FMFS_FineType,
                                            FMFS_ECSFlag = a.FMFS_ECSFlag,
                                            FTFS_FineType = b.FCTFS_FineType,
                                            FMG_Id = c.FMG_Id,
                                            FMH_Id = c.FMH_Id,
                                            FTI_Id = c.FTI_Id,
                                            FMFS_FromDay = a.FMFS_FromDay,
                                            FMFS_ToDay = a.FMFS_ToDay,
                                            FCTFS_PercentageFlg = b.FCTFS_PercentageFlg,
                                            AMSE_Id = d.AMSE_Id
                                        }
                    ).ToArray();

                if (data.allgroupheaddata.Length <= 0)
                {
                    data.allgroupheaddata = (from a in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                             from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                             from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                             from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                             where (f.MI_Id == data.MI_Id &&  a.FMH_Id == c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMI_Id == d.FMI_Id && a.FMG_Id == data.FMG_Id && d.FMI_Id == e.FMI_Id && semids.Contains(f.AMSE_Id))
                                             select new CLGFeeAmountEntryDTO
                                             {
                                                 FMH_Id = a.FMH_Id,
                                                 FTI_Id = e.FTI_Id,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FMI_Name = e.FTI_Name,
                                                 FCMAS_Amount = 0,
                                                 FMH_Order = c.FMH_Order,
                                                 FYGHM_FineApplicableFlag = a.FYGHM_FineApplicableFlag,
                                                 AMSE_SEMName = f.AMSE_SEMName,
                                                 AMSE_Id = f.AMSE_Id,
                                                 AMSE_SEMOrder = f.AMSE_SEMOrder,
                                                 FCMAS_DueDate = DateTime.Now.Date,


                                             }
     ).OrderBy(t => t.AMSE_SEMOrder).ThenBy(t => t.FMH_Order).ToArray();


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

                data.fillallyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).ToArray();

                //end Praveen
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeAmountEntryDTO fillslabde(CLGFeeAmountEntryDTO data)
        {
            try
            {
                List<FeeFineSlabDMO> head = new List<FeeFineSlabDMO>();
                head = _YearlyFeeGroupMappingContext.feeFS.Where(t => t.MI_Id == data.MI_Id && t.FMFS_ActiveFlag == true).ToList();
                data.fillslab = head.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGFeeAmountEntryDTO savedata(CLGFeeAmountEntryDTO pgmod)
        {
            CLGFeeAmountEntryDTO someObj = new CLGFeeAmountEntryDTO();
            try
            {
                if (pgmod.savetmpdata[0].FCMAS_Id > 0)
                {
                    var a = "";
                    if (pgmod.savetmpdata != null)
                    {
                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            var feestutrans = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.ASMAY_Id == pgmod.ASMAY_Id && t.MI_Id == pgmod.MI_Id && t.FCMAS_Id == pgmod.savetmpdata[j].FCMAS_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id).ToList();
                            var feestutranscount = _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO.Where(t => t.FCMAS_Id == pgmod.savetmpdata[j].FCMAS_Id).ToList();


                            if (feestutrans.Count > 0 || feestutranscount.Count > 0)
                            {
                                pgmod.returnval = "used";
                            }
                            else
                            {

                                //var amt_entry = _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO.Where(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.AMCO_Id == pgmod.AMCO_Id && t.AMB_Id == pgmod.AMB_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id).ToList();

                                //List<CLG_Fee_College_Master_Amount_Semesterwise> sem_entry = new List<CLG_Fee_College_Master_Amount_Semesterwise>();

                                //foreach (var item5 in amt_entry)
                                //{
                                //    if (amt_entry.Count > 0)
                                //    {
                                //        sem_entry = _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise.Where(t => t.FCMA_Id == item5.FCMA_Id && t.MI_Id == pgmod.MI_Id).ToList();
                                //    }

                                //    if (sem_entry.Count > 0)
                                //    {
                                //        foreach (var item6 in sem_entry)
                                //        {

                                //            var feestutrans1 = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.ASMAY_Id == pgmod.ASMAY_Id && t.MI_Id == pgmod.MI_Id && t.FCMAS_Id == item6.FCMAS_Id && t.FMG_Id == pgmod.FMG_Id && t.FMH_Id == pgmod.savetmpdata[j].FMH_Id && t.FTI_Id == pgmod.savetmpdata[j].FTI_Id).ToList();
                                //            if (feestutrans1.Count == 0)
                                //            {

                                //                var fineslabentry = _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Fine_Slabs.Where(t => t.FCMAS_Id == item6.FCMAS_Id).ToList();

                                //                if (fineslabentry.Count > 0)
                                //                {
                                //                    foreach (var itemslab in fineslabentry)
                                //                    {
                                //                        _YearlyFeeGroupMappingContext.Remove(itemslab);
                                //                    }
                                //                    _YearlyFeeGroupMappingContext.SaveChanges();
                                //                }

                                //                var duedateentry = _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO.Where(t => t.FCMAS_Id == item6.FCMAS_Id).ToList();

                                //                if (duedateentry.Count > 0)
                                //                {
                                //                    foreach (var item in duedateentry)
                                //                    {
                                //                        _YearlyFeeGroupMappingContext.Remove(item);
                                //                    }
                                //                    _YearlyFeeGroupMappingContext.SaveChanges();
                                //                }

                                //                _YearlyFeeGroupMappingContext.Remove(item6);
                                //                _YearlyFeeGroupMappingContext.SaveChanges();
                                //            }

                                //        }

                                //    }
                                //    _YearlyFeeGroupMappingContext.Remove(item5);
                                //    _YearlyFeeGroupMappingContext.SaveChanges();

                                //}

                                //if (amt_entry.Count > 0)
                                //{
                                //    foreach (var item2 in amt_entry)
                                //    {
                                //        var sem_entrylatest = _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise.Where(t => t.FCMA_Id == item2.FCMA_Id && t.MI_Id == pgmod.MI_Id).ToList();
                                //        if (sem_entrylatest.Count == 0)
                                //        {
                                //            _YearlyFeeGroupMappingContext.Remove(item2);
                                //            _YearlyFeeGroupMappingContext.SaveChanges();
                                //        }

                                //        //var amountentrylst = _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO.Where(t => t.FCMA_Id == item2.FCMA_Id && t.MI_Id == pgmod.MI_Id).ToList();
                                //        //if (amountentrylst.Count == 0)
                                //        //{
                                //        //    _YearlyFeeGroupMappingContext.Remove(item2);
                                //        //    _YearlyFeeGroupMappingContext.SaveChanges();
                                //        //}
                                //    }

                                //}

                                Clg_Fee_AmountEntry_DMO pmm = new Clg_Fee_AmountEntry_DMO();

                                if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                                {
                                    pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                    pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                    pmm.AMB_Id = pgmod.AMB_Id;
                                    pmm.AMCO_Id = pgmod.AMCO_Id;
                                    pmm.FCMA_Id = pgmod.savetmpdata[j].FCMA_Id;
                                    pmm.MI_Id = pgmod.MI_Id;
                                    pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                    pmm.FMG_Id = pgmod.FMG_Id;
                                    pmm.FCMA_ActiveFlg = true;
                                    pmm.FCMA_Flag = "1";
                                    pmm.CreatedDate = DateTime.Now;
                                    pmm.UpdatedDate = DateTime.Now;

                                    CLG_Fee_College_Master_Amount_Semesterwise pmmsem = new CLG_Fee_College_Master_Amount_Semesterwise();
                                    pmmsem.MI_Id = pgmod.MI_Id;
                                    pmmsem.FCMA_Id = pmm.FCMA_Id;
                                    pmmsem.AMSE_Id = pgmod.savetmpdata[j].AMSE_Id;
                                   
                                    pmmsem.FCMAS_Amount = pgmod.savetmpdata[j].FCMAS_Amount;
                                    pmmsem.FCMAS_Currency = "1";
                                    pmmsem.FCMAS_ActiveFlg = true;
                                    pmmsem.CreatedDate = DateTime.Now;
                                    pmmsem.UpdatedDate = DateTime.Now;
                                    pmmsem.FCMAS_DueDate = pgmod.savetmpdata[j].FCMAS_DueDate;
                                    pmmsem.FCMAS_Id = pgmod.savetmpdata[j].FCMAS_Id;

                                    CLG_Fee_College_T_Due_DateDMO fctdd = new CLG_Fee_College_T_Due_DateDMO();
                                    fctdd.FCMAS_Id = pmmsem.FCMAS_Id;
                                    fctdd.FCMAS_Id = pgmod.savetmpdata[j].FCMAS_Id;
                                    if (pgmod.savetmpdata[j].FCTDD_Month == null)
                                    {
                                        fctdd.FCTDD_Month = pgmod.savetmpdata[j].FCTDD_Month = "0";
                                    }
                                    else
                                    {
                                        fctdd.FCTDD_Month = pgmod.savetmpdata[j].FCTDD_Month;
                                    }

                                    if (pgmod.savetmpdata[j].FCTDD_Day == null)
                                    {
                                        fctdd.FCTDD_Day = pgmod.savetmpdata[j].FCTDD_Day = "0";
                                    }
                                    else
                                    {
                                        fctdd.FCTDD_Day = pgmod.savetmpdata[j].FCTDD_Day;
                                    }

                                    if (pgmod.savetmpdata[j].FCTDD_Year == null)
                                    {
                                        fctdd.FCTDD_Year = pgmod.savetmpdata[j].FCTDD_Year = "0";
                                    }
                                    else
                                    {
                                        fctdd.FCTDD_Year = pgmod.savetmpdata[j].FCTDD_Year;
                                    }

                                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "CLG_Insert_Fee_Amount_Entry";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                           SqlDbType.BigInt)
                                        {
                                            Value = pmm.MI_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                          SqlDbType.BigInt)
                                        {
                                            Value = pmm.ASMAY_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                               SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMH_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                            SqlDbType.BigInt)
                                        {
                                            Value = pmm.FTI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                                    SqlDbType.BigInt)
                                        {
                                            Value = pmm.AMB_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                                          SqlDbType.BigInt)
                                        {
                                            Value = pmm.AMCO_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                       SqlDbType.BigInt)
                                        {
                                            Value = pmm.FMG_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FCMA_Id",
                                SqlDbType.BigInt)
                                        {
                                            Value = pmm.FCMA_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                                SqlDbType.BigInt)
                                        {
                                            Value = pmmsem.FCMAS_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FCMA_Flag",
                                       SqlDbType.VarChar)
                                        {
                                            Value = pmm.FCMA_Flag
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FCMA_ActiveFlg",
                                      SqlDbType.VarChar)
                                        {
                                            Value = pmm.FCMA_ActiveFlg
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                                      SqlDbType.BigInt)
                                        {
                                            Value = pmmsem.AMSE_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Amount",
                                    SqlDbType.VarChar)
                                        {
                                            Value = pmmsem.FCMAS_Amount
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Currency",
                             SqlDbType.VarChar)
                                        {
                                            Value = pmmsem.FCMAS_Currency
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCMAS_ActiveFlg",
                            SqlDbType.VarChar)
                                        {
                                            Value = pmmsem.FCMAS_ActiveFlg
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@FCTDD_Month",
                           SqlDbType.VarChar)
                                        {
                                            Value = fctdd.FCTDD_Month
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCTDD_Day",
                          SqlDbType.VarChar)
                                        {
                                            Value = fctdd.FCTDD_Day
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCTDD_Year",
                         SqlDbType.VarChar)
                                        {
                                            Value = fctdd.FCTDD_Year
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FCMAS_DueDate",
                        SqlDbType.DateTime)
                                        {
                                            Value = pmmsem.FCMAS_DueDate
                                        });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var data = cmd.ExecuteNonQuery();


                                        if (data >= 1)
                                        {
                                            pgmod.returnval = "true";
                                            a = "true";
                                        }
                                        else
                                        {
                                            pgmod.returnval = "false";
                                        }
                                    }
                                }
                            }

                            j++;
                        }
                    }
                    if (a == "true")
                    {
                        pgmod.returnval = "true";
                    }
                    pgmod.amtentrystatus = "Updated";
                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("CLG_Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.AMB_Id, pgmod.savefineslabreg[r].AMSE_Id, pgmod.AMCO_Id, pgmod.savefineslabreg[r].FCTFS_PercentageFlg);
                        }
                        r++;
                    }

                    r = 0;
                }
                else
                {
                    if (pgmod.savetmpdata != null)
                    {

                        int j = 0;

                        while (j < pgmod.savetmpdata.Count())
                        {
                            Clg_Fee_AmountEntry_DMO pmm = new Clg_Fee_AmountEntry_DMO();
                            if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                            {
                                pmm.FMH_Id = pgmod.savetmpdata[j].FMH_Id;
                                pmm.FTI_Id = pgmod.savetmpdata[j].FTI_Id;
                                pmm.AMB_Id = pgmod.AMB_Id;
                                pmm.AMCO_Id = pgmod.AMCO_Id;

                                pmm.MI_Id = pgmod.MI_Id;
                                pmm.ASMAY_Id = pgmod.ASMAY_Id;
                                pmm.FMG_Id = pgmod.FMG_Id;
                                pmm.FCMA_ActiveFlg = true;
                                pmm.FCMA_Flag = "1";
                                pmm.CreatedDate = DateTime.Now;
                                pmm.UpdatedDate = DateTime.Now;

                                CLG_Fee_College_Master_Amount_Semesterwise pmmsem = new CLG_Fee_College_Master_Amount_Semesterwise();
                                pmmsem.MI_Id = pgmod.MI_Id;
                                pmmsem.FCMA_Id = pmm.FCMA_Id;
                                pmmsem.AMSE_Id = pgmod.savetmpdata[j].AMSE_Id;
                                pmmsem.FCMAS_Amount = pgmod.savetmpdata[j].FCMAS_Amount;
                                pmmsem.FCMAS_Currency = "1";
                                pmmsem.FCMAS_ActiveFlg = true;
                                pmmsem.CreatedDate = DateTime.Now;
                                pmmsem.UpdatedDate = DateTime.Now;
                                pmmsem.FCMAS_DueDate = pgmod.savetmpdata[j].FCMAS_DueDate;
                                CLG_Fee_College_T_Due_DateDMO fctdd = new CLG_Fee_College_T_Due_DateDMO();
                                fctdd.FCMAS_Id = pmmsem.FCMAS_Id;
                                if (pgmod.savetmpdata[j].FCTDD_Month == null)
                                {
                                    fctdd.FCTDD_Month = pgmod.savetmpdata[j].FCTDD_Month = "0";
                                }
                                else
                                {
                                    fctdd.FCTDD_Month = pgmod.savetmpdata[j].FCTDD_Month;
                                }

                                if (pgmod.savetmpdata[j].FCTDD_Day == null)
                                {
                                    fctdd.FCTDD_Day = pgmod.savetmpdata[j].FCTDD_Day = "0";
                                }
                                else
                                {
                                    fctdd.FCTDD_Day = pgmod.savetmpdata[j].FCTDD_Day;
                                }

                                if (pgmod.savetmpdata[j].FCTDD_Year == null)
                                {
                                    fctdd.FCTDD_Year = pgmod.savetmpdata[j].FCTDD_Year = "0";
                                }
                                else
                                {
                                    fctdd.FCTDD_Year = pgmod.savetmpdata[j].FCTDD_Year;
                                }

                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "CLG_Insert_Fee_Amount_Entry";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                       SqlDbType.BigInt)
                                    {
                                        Value = pmm.MI_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pmm.ASMAY_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                                           SqlDbType.BigInt)
                                    {
                                        Value = pmm.FMH_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FTI_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = pmm.FTI_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                                SqlDbType.BigInt)
                                    {
                                        Value = pmm.AMB_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                                      SqlDbType.BigInt)
                                    {
                                        Value = pmm.AMCO_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = pmm.FMG_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FCMA_Id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                            SqlDbType.BigInt)
                                    {
                                        Value = pmmsem.FCMAS_Id
                                    });


                                    cmd.Parameters.Add(new SqlParameter("@FCMA_Flag",
                                   SqlDbType.VarChar)
                                    {
                                        Value = pmm.FCMA_Flag
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FCMA_ActiveFlg",
                                  SqlDbType.VarChar)
                                    {
                                        Value = pmm.FCMA_ActiveFlg
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                                  SqlDbType.BigInt)
                                    {
                                        Value = pmmsem.AMSE_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_Amount",
                                SqlDbType.VarChar)
                                    {
                                        Value = pmmsem.FCMAS_Amount
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_Currency",
                         SqlDbType.VarChar)
                                    {
                                        Value = pmmsem.FCMAS_Currency
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_ActiveFlg",
                        SqlDbType.VarChar)
                                    {
                                        Value = pmmsem.FCMAS_ActiveFlg
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FCTDD_Month",
                       SqlDbType.VarChar)
                                    {
                                        Value = fctdd.FCTDD_Month
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCTDD_Day",
                      SqlDbType.VarChar)
                                    {
                                        Value = fctdd.FCTDD_Day
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCTDD_Year",
                     SqlDbType.VarChar)
                                    {
                                        Value = fctdd.FCTDD_Year
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_DueDate",
                       SqlDbType.DateTime)
                                    {
                                        Value = pmmsem.FCMAS_DueDate
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data = cmd.ExecuteNonQuery();


                                    if (data >= 1)
                                    {
                                        pgmod.returnval = "true";
                                        pgmod.amtentrystatus = "Saved";
                                    }
                                    else
                                    {
                                        pgmod.returnval = "false";
                                    }
                                }
                            }
                            j++;
                        }
                    }

                    int r = 0;
                    while (r < pgmod.savefineslabreg.Count())
                    {
                        if (pgmod.MI_Id != 0 && pgmod.ASMAY_Id != 0)
                        {
                            _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("CLG_Insert_Fee_T_Fine_Slab @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11", pgmod.MI_Id, pgmod.ASMAY_Id, pgmod.savefineslabreg[r].FMH_ID, pgmod.savefineslabreg[r].FTI_ID, pgmod.FMG_Id, pgmod.savefineslabreg[r].FTFS_FineType, pgmod.savefineslabreg[r].FTFS_Amount, pgmod.savefineslabreg[r].FMFS_Id, pgmod.AMB_Id, pgmod.savefineslabreg[r].AMSE_Id, pgmod.AMCO_Id, pgmod.savefineslabreg[r].FCTFS_PercentageFlg);
                        }
                        r++;
                    }

                    r = 0;
                }

                pgmod.alldata = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                 from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == d.FTI_Id && a.MI_Id == pgmod.MI_Id && a.ASMAY_Id == pgmod.ASMAY_Id && a.FCMA_Id == e.FCMA_Id)
                                 select new CLGFeeAmountEntryDTO
                                 {
                                     FMG_GroupName = b.FMG_GroupName,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FCMAS_Amount = e.FCMAS_Amount
                                 }
                    ).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
                pgmod.returnval = "false";
            }

            return pgmod;
        }
    }
}
