using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace CollegeFeeService.com.vaps.Implementation
{
    public class StaffAndOtherConcessionImpl:Interfaces.StaffAndOtherConcessionInterface
    {
        private static ConcurrentDictionary<string, CollegeConcessionDTO> _login =
    new ConcurrentDictionary<string, CollegeConcessionDTO>();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StaffAndOtherConcessionImpl> _logger;
        public StaffAndOtherConcessionImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, ILogger<StaffAndOtherConcessionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public CollegeConcessionDTO deleteconcess(CollegeConcessionDTO data)
        {
            try
            {
                if (data.radiobtnvalue != "Staff" && data.radiobtnvalue != "Others")
                {
                    var amstid = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == data.FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).AMCST_Id;
                    var fmgid = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == data.FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;
                    var fmhid = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == data.FCSC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;
                    var ftiid = _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == data.FSCI_ID && t.FCSC_Id == data.FCSC_Id).FTI_Id;
                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@FCSC_Id and FSCI_ID=@fsci_id
                    var paidamount = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.AMCST_Id == amstid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FCSS_PaidAmount;
                    //AMCST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Collegedeleteconcession @p0,@p1,@p2,@p3,@p4", data.FCSC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FSCI_ID);
                        data.returnval = "true";
                    }
                    else
                    {
                        var result = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == data.FCSC_Id);

                        var resultinstallment = _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == data.FSCI_ID);

                        var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == amstid && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid);

                        if (status_stu.FCSS_CurrentYrCharges - status_stu.FCSS_ConcessionAmount - status_stu.FCSS_PaidAmount < 0)
                        {
                            status_stu.FCSS_ToBePaid = 0;
                        }
                        else
                        {
                            status_stu.FCSS_ToBePaid = status_stu.FCSS_CurrentYrCharges - status_stu.FCSS_ConcessionAmount - status_stu.FCSS_PaidAmount;
                        }

                        //if (status_stu.FCSS_CurrentYrCharges - status_stu.FCSS_ConcessionAmount > 0)
                        //{
                        //    status_stu.FCSS_TotalToBePaid = status_stu.FCSS_CurrentYrCharges - status_stu.FCSS_ConcessionAmount;
                        //}
                        //else
                        //{
                        //    status_stu.FCSS_TotalToBePaid = 0;
                        //}

                        status_stu.FCSS_ConcessionAmount = status_stu.FCSS_ConcessionAmount;

                        if (status_stu.FCSS_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                        {
                            status_stu.FCSS_ExcessPaidAmount = 0;
                        }
                        else
                        {
                            status_stu.FCSS_ExcessPaidAmount = status_stu.FCSS_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                        }


                        if (status_stu.FCSS_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                        {
                            status_stu.FCSS_RunningExcessAmount = 0;
                        }
                        else
                        {
                            status_stu.FCSS_RunningExcessAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                        }

                        _YearlyFeeGroupMappingContext.Remove(result);
                        _YearlyFeeGroupMappingContext.Remove(resultinstallment);
                        _YearlyFeeGroupMappingContext.Update(status_stu);

                        _YearlyFeeGroupMappingContext.SaveChanges();
                        data.returnval = "paid";
                    }
                }

                else if (data.radiobtnvalue.Equals("Staff"))
                {
                    var hrmeid = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO.Single(t => t.FECC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).HRME_Id;

                    var fmgid = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO.Single(t => t.FECC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;

                    var fmhid = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO.Single(t => t.FECC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;

                    var ftiid = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO.Single(t => t.FECIC_Id == data.FECI_Id && t.FECC_Id == data.FEC_Id).FTI_Id;

                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@FCSC_Id and FSCI_ID=@fsci_id

                    var paidamount = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Single(t => t.HRME_Id == hrmeid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FCSSST_PaidAmount;
                    //AMCST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("deletestaffconcessioncollege @p0,@p1,@p2,@p3,@p4", data.FEC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FECI_Id);
                        data.returnval = "true";
                    }
                    else if (paidamount > 0)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "paid";
                    }
                }

                else if (data.radiobtnvalue.Equals("Others"))
                {
                    var hrmeid = _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO.Single(t => t.FOCC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMCOST_Id;

                    var fmgid = _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO.Single(t => t.FOCC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;

                    var fmhid = _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO.Single(t => t.FOCC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;

                    var ftiid = _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO.Single(t => t.FOCIC_Id == data.FOCI_Id && t.FOCC_Id == data.FOC_Id).FTI_Id;

                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@FCSC_Id and FSCI_ID=@fsci_id

                    var paidamount = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Single(t => t.FMCOST_Id == hrmeid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FCSSOST_PaidAmount;
                    //AMCST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("deleteothersconcessioncollege @p0,@p1,@p2,@p3,@p4", data.FOC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FOCI_Id);
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "paid";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = "false";
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public CollegeConcessionDTO EditconcessionDetails(CollegeConcessionDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

   

                if (data.configset.Equals("G"))
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                      from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                      where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                      select new CollegeConcessionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                  ).ToArray();
                }

                data.EditfeeDetails = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                       from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                       from d in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                       from e in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                       from f in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from g in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       //from h in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                                       //from i in _YearlyFeeGroupMappingContext.feeTr
                                       //from j in _YearlyFeeGroupMappingContext.School_M_Class
                                       where (a.AMCST_Id == b.AMCST_Id && b.AMCST_Id == c.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && c.AMCST_Id == d.AMCST_Id && c.ASMAY_Id == d.ASMAY_Id && c.FMH_Id == d.FMH_Id && c.FMG_Id == d.FMG_Id && d.FCSC_Id == e.FCSC_Id && c.FTI_Id == e.FTI_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == g.FTI_Id && a.MI_Id == data.MI_Id && e.FSCI_Id == data.FSCI_ID)
                                       select new CollegeConcessionDTO
                                       {
                                           ASMAY_Id = c.ASMAY_Id,
                                           //ASMCL_ID = b.ASMCL_Id,
                                           //classname = j.ASMCL_ClassName,
                                           //termname = i.FMT_Name,
                                           //FMT_Id = i.FMT_Id,
                                           AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                           AMCST_AdmNo = a.AMCST_AdmNo,
                                           AMCST_FirstName = a.AMCST_FirstName,
                                           FMH_Id = f.FMH_Id,
                                           FMG_Id = c.FMG_Id,
                                           FTI_Id = c.FTI_Id,
                                           FMA_Id = c.FCMAS_Id,
                                           FSCI_ID = e.FSCI_Id,
                                           FCSC_Id = d.FCSC_Id,
                                           FMH_FeeName = f.FMH_FeeName,
                                           FTI_Name = g.FTI_Name,
                                          // FMA_Amount = c.FCSS_TotalToBePaid,
                                           FSCI_ConcessionAmount = c.FCSS_ConcessionAmount,
                                          // FSC_ConcessionType = d.FSC_ConcessionType,
                                          // FSC_ConcessionReason = d.FSC_ConcessionReason,
                                           AMCST_Id = a.AMCST_Id
                                       }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeConcessionDTO fillamount(CollegeConcessionDTO data)
        {
            try
            {


                var myArray = data.multiplegroups.Split(',');
                List<long> terms_groups = new List<long>();
                for (int i = 0; i < myArray.Length; i++)
                {
                    terms_groups.Add(Convert.ToInt64(myArray[i]));
                }
                data.terms_groups = terms_groups.ToArray();
                if (data.configset.Equals("T"))
                {
                
                     if (data.radiobtnvalue.Equals("Staff"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstafflistforconcessioncollege_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMCST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.v_studentPendingsavedconcessionDMO
                                                 where (a.mi_id == data.MI_Id)
                                                 select new FeeConcessionDTO
                                                 {
                                                     FMH_FeeName = a.FMH_FeeName,
                                                     FTI_Name = a.FTI_Name,
                                                     FTI_Id = a.FTI_Id,
                                                     FMH_Id = a.fmh_id,
                                                     FMA_Amount = a.FMA_Amount,
                                                     FMA_Id = a.fma_id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = a.FSCI_ConcessionAmount,
                                                     FSC_ConcessionType = a.FSC_ConcessionType,
                                                     FSC_ConcessionReason = a.FSC_ConcessionReason,
                                                     FSC_Id = a.fsc_id
                                                 }
          ).Distinct().ToArray();

                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.v_studentPendingconcessionDMO
                                             where (a.mi_id == data.MI_Id)
                                             select new FeeConcessionDTO
                                             {
                                                 FMH_FeeName = a.FMH_FeeName,
                                                 FTI_Name = a.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.fmh_id,
                                                 //FMA_Amount = a.FSS_NetAmount,
                                                 FMA_Amount = a.FMA_Amount,
                                                 FMA_Id = a.fma_id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();
                    }

                    else if (data.radiobtnvalue.Equals("Others"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillotherslistforconcessioncollege_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMCST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.v_studentPendingsavedconcessionDMO
                                                 where (a.mi_id == data.MI_Id)
                                                 select new FeeConcessionDTO
                                                 {
                                                     FMH_FeeName = a.FMH_FeeName,
                                                     FTI_Name = a.FTI_Name,
                                                     FTI_Id = a.FTI_Id,
                                                     FMH_Id = a.fmh_id,
                                                     FMA_Amount = a.FMA_Amount,
                                                     FMA_Id = a.fma_id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = a.FSCI_ConcessionAmount,
                                                     FSC_ConcessionType = a.FSC_ConcessionType,
                                                     FSC_ConcessionReason = a.FSC_ConcessionReason,
                                                     FSC_Id = a.fsc_id
                                                 }
          ).Distinct().ToArray();

                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.v_studentPendingconcessionDMO
                                             where (a.mi_id == data.MI_Id)
                                             select new FeeConcessionDTO
                                             {
                                                 FMH_FeeName = a.FMH_FeeName,
                                                 FTI_Name = a.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.fmh_id,
                                                 //FMA_Amount = a.FSS_NetAmount,
                                                 FMA_Amount = a.FMA_Amount,
                                                 FMA_Id = a.fma_id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();
                    }

                }
                else if (data.configset.Equals("G"))
                {

                

                     if (data.radiobtnvalue.Equals("Staff"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstafflistforconcessioncollege_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMCST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.v_studentPendingsavedconcessionDMO
                                                 where (a.mi_id == data.MI_Id)
                                                 select new FeeConcessionDTO
                                                 {
                                                     FMH_FeeName = a.FMH_FeeName,
                                                     FTI_Name = a.FTI_Name,
                                                     FTI_Id = a.FTI_Id,
                                                     FMH_Id = a.fmh_id,
                                                     FMA_Amount = a.FMA_Amount,
                                                     FMA_Id = a.fma_id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = a.FSCI_ConcessionAmount,
                                                     FSC_ConcessionType = a.FSC_ConcessionType,
                                                     FSC_ConcessionReason = a.FSC_ConcessionReason,
                                                     FSC_Id = a.fsc_id
                                                 }
          ).Distinct().ToArray();

                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.v_studentPendingconcessionDMO
                                             where (a.mi_id == data.MI_Id)
                                             select new FeeConcessionDTO
                                             {
                                                 FMH_FeeName = a.FMH_FeeName,
                                                 FTI_Name = a.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.fmh_id,
                                                 //FMA_Amount = a.FSS_NetAmount,
                                                 FMA_Amount = a.FMA_Amount,
                                                 FMA_Id = a.fma_id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();
                    }

                    else if (data.radiobtnvalue.Equals("Others"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillotherslistforconcessioncollege_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMCST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.v_studentPendingsavedconcessionDMO
                                                 where (a.mi_id == data.MI_Id)
                                                 select new FeeConcessionDTO
                                                 {
                                                     FMH_FeeName = a.FMH_FeeName,
                                                     FTI_Name = a.FTI_Name,
                                                     FTI_Id = a.FTI_Id,
                                                     FMH_Id = a.fmh_id,
                                                     FMA_Amount = a.FMA_Amount,
                                                     FMA_Id = a.fma_id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = a.FSCI_ConcessionAmount,
                                                     FSC_ConcessionType = a.FSC_ConcessionType,
                                                     FSC_ConcessionReason = a.FSC_ConcessionReason,
                                                     FSC_Id = a.fsc_id
                                                 }
          ).Distinct().ToArray();

                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.v_studentPendingconcessionDMO
                                             where (a.mi_id == data.MI_Id)
                                             select new FeeConcessionDTO
                                             {
                                                 FMH_FeeName = a.FMH_FeeName,
                                                 FTI_Name = a.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.fmh_id,
                                                 FMA_Amount = a.FMA_Amount,
                                                 FMA_Id = a.fma_id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();
                    }

                }


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
                if (data.configset.Equals("T"))
                {
                    data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                         from b in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         from f in _YearlyFeeGroupMappingContext.feeMTH
                                         where (f.FMH_Id == b.FMH_Id && f.FTI_Id == d.FTI_Id && a.FMG_Id == b.FMG_Id && b.FMH_Id == c.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && data.FMG_Ids.Contains(f.FMT_Id) && d.FMI_Id == b.FMI_Id && e.FMI_Id == d.FMI_Id) //&& f.FMT_Id == data.FMG_Id
                                         select new CollegeConcessionDTO
                                         {
                                             FMH_FeeName = c.FMH_FeeName,
                                             FTI_Name = d.FTI_Name,
                                             FTI_Id = d.FTI_Id,
                                             FMH_Id = c.FMH_Id,
                                         }
     ).Distinct().ToArray();
                }
                else
                {
                    data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                         from b in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installment_DMO
                                         where (a.FMG_Id == b.FMG_Id && b.FMH_Id == c.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && data.FMG_Ids.Contains(a.FMG_Id) && d.FMI_Id == b.FMI_Id && e.FMI_Id == d.FMI_Id)//&& a.FMG_Id == data.FMG_Id
                                         select new CollegeConcessionDTO
                                         {
                                             FMH_FeeName = c.FMH_FeeName,
                                             FTI_Name = d.FTI_Name,
                                             FTI_Id = d.FTI_Id,
                                             FMH_Id = c.FMH_Id,
                                         }
     ).Distinct().ToArray();
                }

                // con checking

                if (data.AMCST_Id != 0)
                {
                    if (data.configset.Equals("T"))
                    {
                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                                 from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                 from e in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                                 from f in _YearlyFeeGroupMappingContext.feeMTH
                                                 where (f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && data.FMG_Ids.Contains(f.FMT_Id) && b.FCSC_Id == a.FCSC_Id && c.FMH_Id == a.FMH_Id && d.FTI_Id == b.FTI_Id && e.AMCST_Id == a.AMCST_Id && e.FMH_Id == c.FMH_Id && e.FTI_Id == b.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id)//&& f.FMT_Id == data.FMG_Id
                                                 select new CollegeConcessionDTO
                                                 {
                                                     FMH_FeeName = c.FMH_FeeName,
                                                     FTI_Name = d.FTI_Name,
                                                     FTI_Id = d.FTI_Id,
                                                     FMH_Id = a.FMH_Id,
                                                     FMA_Amount = e.FCSS_ToBePaid,
                                                     FMA_Id = e.FCMAS_Id,
                                                     FMG_Id = a.FMG_Id,
                                                     FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                                    // FSC_ConcessionType = a.FSC_ConcessionType,
                                                    // FSC_ConcessionReason = a.FSC_ConcessionReason,
                                                 }
      ).Distinct().ToArray();


                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                             from c in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                             from d in _YearlyFeeGroupMappingContext.feeMTH
                                             where (a.FMH_Id == d.FMH_Id && a.FTI_Id == d.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && b.FMH_Id == a.FMH_Id && c.FTI_Id == a.FTI_Id && data.FMG_Ids.Contains(d.FMT_Id))
                                             select new CollegeConcessionDTO//&& d.FMT_Id == data.FMG_Id
                                             {
                                                 FMH_FeeName = b.FMH_FeeName,
                                                 FTI_Name = c.FTI_Name,
                                                 FTI_Id = a.FTI_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 //FMA_Amount = a.FCSS_NetAmount,
                                                 FMA_Amount = a.FCSS_ToBePaid,
                                                 FMA_Id = a.FCMAS_Id,
                                                 FMG_Id = a.FMG_Id
                                             }
     ).Distinct().ToArray();

                    }
                }
                //con checking


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public CollegeConcessionDTO filstaff(CollegeConcessionDTO data)
        {
            try
            {
                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                  from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                  select new PreadmissionDTOs.com.vaps.College.Fee.Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRMD_Id = a.HRMD_Id,
                                      HRMDES_Id = c.HRMDES_Id,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName
                                  }).ToList().Distinct().OrderBy(t => t.HRME_Id).Take(5).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeConcessionDTO getdata(CollegeConcessionDTO data)

        {
            try
            {
                // List<MasterAcademic> allyear = new List<MasterAcademic>();
                data.fillyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();
                //data.fillyear = allyear.Distinct().ToArray();


                //List<Fee_Master_ConcessionDMO> feecategory = new List<Fee_Master_ConcessionDMO>();
                data.fillfeecategory = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.FCSC_ActiveFlag == true).ToArray();
                //data.fillfeecategory = feecategory.ToArray();

                data.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_ID == data.ASMAY_Id).ToList().Distinct().ToArray();

                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                  from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRMD_Id = a.HRMD_Id,
                                      HRMDES_Id = c.HRMDES_Id,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName
                                  }).ToList().Distinct().OrderBy(t => t.HRME_Id).ToArray();

                data.otherlist = (from a in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                  where (a.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMCOST_ActiveFlag == true)
                                  select new Fee_Master_OtherStudentsDTO
                                  {
                                      FMOST_Id = a.FMCOST_Id,
                                      FMOST_StudentName = a.FMCOST_StudentName.Trim(),
                                      FMOST_StudentMobileNo = a.FMCOST_StudentMobileNo,
                                      FMOST_StudentEmailId = a.FMCOST_StudentEmailId,
                                  }).ToList().Distinct().OrderBy(t => t.FMOST_Id).ToArray();



                if (data.configset.Equals("T"))
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                      from b in _YearlyFeeGroupMappingContext.feeTr
                                      where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                      select new CollegeConcessionDTO
                                      {
                                          FMG_GroupName = b.FMT_Name,
                                          FMG_Id = a.FMT_Id,
                                      }
     ).Distinct().ToArray();

                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                        from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                      from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                     
                                        from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        where (d.ASMAY_Id == a.ASMAY_Id && a.FCSC_Id == b.FCSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMCST_Id == c.AMCST_Id && a.AMCST_Id == d.AMCST_Id  && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                        select new CollegeConcessionDTO
                                        {
                                            studentname = c.AMCST_FirstName + ' ' + c.AMCST_MiddleName + ' ' + c.AMCST_LastName,
                                         
                                            FMG_GroupName = f.FMG_GroupName,
                                            FMH_FeeName = g.FMH_FeeName,
                                            FTI_Name = h.FTI_Name,
                                            FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                            FCSC_Id = a.FCSC_Id,
                                            FTI_Id = b.FTI_Id,
                                            FSCI_ID = b.FSCI_Id,

                                        }
                                ).Distinct().ToArray();


                    data.staffdata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO
                                      from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                      from d in _YearlyFeeGroupMappingContext.HR_Master_Department
                                      from e in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                      from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                      from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                      from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                      where (a.HRME_Id == c.HRME_Id && a.FECC_Id == b.FECC_Id && c.HRMD_Id == d.HRMD_Id && c.HRMDES_Id == e.HRMDES_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new CollegeConcessionDTO
                                      {
                                          HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + '-' + c.HRME_EmployeeMiddleName + '-' + c.HRME_EmployeeLastName,
                                          HRMDES_DesignationName = e.HRMDES_DesignationName,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          FMG_GroupName = f.FMG_GroupName,
                                          FMH_FeeName = g.FMH_FeeName,
                                          FTI_Name = h.FTI_Name,
                                          FSCI_ConcessionAmount = b.FECIC_ConcessionAmount,
                                          FEC_Id = a.FECC_Id,
                                          FTI_Id = b.FTI_Id,
                                          FECI_Id = b.FECIC_Id,

                                      }
                               ).Distinct().ToArray();

                    data.othersdata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                       from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       where (a.FMCOST_Id == c.FMCOST_Id && a.FOCC_Id == b.FOCC_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new CollegeConcessionDTO
                                       {
                                           FMOST_StudentName = c.FMCOST_StudentName,
                                           FMOST_StudentMobileNo = c.FMCOST_StudentMobileNo,
                                           FMOST_StudentEmailId = c.FMCOST_StudentEmailId,
                                           FMG_GroupName = f.FMG_GroupName,
                                           FMH_FeeName = g.FMH_FeeName,
                                           FTI_Name = h.FTI_Name,
                                           FSCI_ConcessionAmount = b.FSCIC_ConcessionAmount,
                                           FOC_Id = a.FOCC_Id,
                                           FTI_Id = b.FTI_Id,
                                           FOCI_Id = b.FOCIC_Id,

                                       }
                              ).Distinct().ToArray();

                }
                else
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                      from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                      where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                      select new CollegeConcessionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                  ).ToArray();

                   

                    data.staffdata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO
                                      from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                      from d in _YearlyFeeGroupMappingContext.HR_Master_Department
                                      from e in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                      from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                      from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                      from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                      where (a.HRME_Id == c.HRME_Id && a.FECC_Id == b.FECC_Id && c.HRMD_Id == d.HRMD_Id && c.HRMDES_Id == e.HRMDES_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new CollegeConcessionDTO
                                      {
                                          HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + '-' + c.HRME_EmployeeMiddleName + '-' + c.HRME_EmployeeLastName,
                                          HRMDES_DesignationName = e.HRMDES_DesignationName,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          FMG_GroupName = f.FMG_GroupName,
                                          FMH_FeeName = g.FMH_FeeName,
                                          FTI_Name = h.FTI_Name,
                                          FSCI_ConcessionAmount = b.FECIC_ConcessionAmount,
                                          FEC_Id = a.FECC_Id,
                                          FTI_Id = b.FTI_Id,
                                          FECI_Id = b.FECIC_Id,

                                      }
                              ).Distinct().ToArray();

                    data.othersdata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_Master_College_OtherStudents
                                       from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       where (a.FMCOST_Id == c.FMCOST_Id && a.FOCC_Id == b.FOCC_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new CollegeConcessionDTO
                                       {
                                           FMOST_StudentName = c.FMCOST_StudentName,
                                           FMOST_StudentEmailId = c.FMCOST_StudentEmailId,
                                           FMOST_StudentMobileNo = c.FMCOST_StudentMobileNo,
                                           FMG_GroupName = f.FMG_GroupName,
                                           FMH_FeeName = g.FMH_FeeName,
                                           FTI_Name = h.FTI_Name,
                                           FSCI_ConcessionAmount = b.FSCIC_ConcessionAmount,
                                           FOC_Id = a.FOCC_Id,
                                           FTI_Id = b.FTI_Id,
                                           FOCI_Id = b.FOCIC_Id,
                                       }
                              ).Distinct().ToArray();
                }

      
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

      

        public CollegeConcessionDTO savedatadelegate(CollegeConcessionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.radiobtnvalue != "Staff" && data.radiobtnvalue != "Others")
                {
                    if (data.FSCI_ID > 0 && data.FCSC_Id > 0)
                    {
                        using (var transactionupdate = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            var result = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Single(t => t.FCSC_Id == data.FCSC_Id);

                            var resultinstallment = _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == data.FSCI_ID);

                            var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.savetmpdata[0].AMCST_Id && t.FMG_Id == data.savetmpdata1[0].FMG_Id && t.FMH_Id == data.savetmpdata1[0].FMH_Id && t.FTI_Id == data.savetmpdata1[0].FTI_Id);

                            result.FCSC_ConcessionReason = data.savetmpdata1[0].FCSC_ConcessionReason;

                            _YearlyFeeGroupMappingContext.Update(result);

                            resultinstallment.FSCI_ConcessionAmount = data.savetmpdata1[0].FSCI_ConcessionAmount;
                            resultinstallment.UpdatedDate = indianTime;
                            resultinstallment.FSCI_UpdatedBy = data.userid;

                            _YearlyFeeGroupMappingContext.Update(resultinstallment);

                            if (status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount < 0)
                            {
                                status_stu.FCSS_ToBePaid = 0;
                            }
                            else
                            {
                                status_stu.FCSS_ToBePaid = status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount;
                            }

                    


                            status_stu.FCSS_ConcessionAmount = data.savetmpdata1[0].FSCI_ConcessionAmount;

                            if (data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                            {
                                status_stu.FCSS_ExcessPaidAmount = 0;
                            }
                            else
                            {
                                status_stu.FCSS_ExcessPaidAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                            }


                            if (data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                            {
                                status_stu.FCSS_RunningExcessAmount = 0;
                            }
                            else
                            {
                                status_stu.FCSS_RunningExcessAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                            }

                            _YearlyFeeGroupMappingContext.Update(status_stu);

                            _YearlyFeeGroupMappingContext.SaveChanges();
                            transactionupdate.Commit();
                        }
                    }
                    else
                    {
                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            if (data.savetmpdata != null || data.savetmpdata1 != null)
                            {
                                int j = 0, k = 0;

                                while (j < data.savetmpdata.Count())
                                {
                                    while (k < data.savetmpdata1.Count())
                                    {
                                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                                             from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                                             where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                             select new CollegeConcessionDTO
                                                             {
                                                                 FCSC_Id = a.FCSC_Id
                                                             }
                       ).Distinct().ToArray();

                                        CollegeConcessionDMO pmm = new CollegeConcessionDMO();
                                        if (data.fillheaddata.Length <= 0)
                                        {
                                            pmm.AMCST_Id = data.savetmpdata[j].AMCST_Id;
                                         
                                            pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                            pmm.MI_Id = data.MI_Id;
                                        
                                            pmm.ASMAY_Id = data.ASMAY_Id;
                                            pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                            pmm.FCSC_ConcessionReason = data.savetmpdata1[k].FCSC_ConcessionReason;
                                            pmm.FCSC_ConcessionType = data.savetmpdata1[k].FCSC_ConcessionType;
                                            pmm.FCSC_ActiveFlag = true;
                                            pmm.CreatedDate = indianTime;
                                            pmm.UpdatedDate = indianTime;

                                  

                                            _YearlyFeeGroupMappingContext.Add(pmm);
                                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                        }
                                        else if (data.fillheaddata.Length >= 1)
                                        {
                                            var fetchfscid = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                                              from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                                              where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                              select new CollegeConcessionDTO
                                                              {
                                                                  FCSC_Id = a.FCSC_Id
                                                              }
                      ).Distinct();

                                            var result1 = _YearlyFeeGroupMappingContext.CollegeConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FCSC_Id == fetchfscid.FirstOrDefault().FCSC_Id).FirstOrDefault();
                                            result1.FCSC_ConcessionReason = data.savetmpdata1[k].FCSC_ConcessionReason;
                                            _YearlyFeeGroupMappingContext.Update(result1);
                                            _YearlyFeeGroupMappingContext.SaveChanges();

                                            pmm.FCSC_Id = fetchfscid.FirstOrDefault().FCSC_Id;

                                        }

                                        CollegeConcessionInstallmentDMO pmmins = new CollegeConcessionInstallmentDMO();
                                        var resultt = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                                       from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                                       where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FCSC_Id == b.FCSC_Id && a.AMCST_Id == data.savetmpdata[j].AMCST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                                       select new CollegeConcessionInstallmentDMO
                                                       {
                                                           FCSC_Id = b.FCSC_Id
                                                       }).SingleOrDefault();

                                        if (resultt == null)
                                        {

                                            pmmins.FCSC_Id = pmm.FCSC_Id;
                                            pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                            pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                            pmmins.CreatedDate = DateTime.Now;
                                            pmmins.UpdatedDate = DateTime.Now;
                                            pmmins.FSCI_UpdatedBy = data.userid;

                                            _YearlyFeeGroupMappingContext.Add(pmmins);


                                            var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.savetmpdata[j].AMCST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            if (status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount < 0)
                                            {
                                                status_stu.FCSS_ToBePaid = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_ToBePaid = status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount;
                                            }

                                           

                                            status_stu.FCSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FCSS_ExcessPaidAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                                            }


                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FCSS_RunningExcessAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                                            }


                                            _YearlyFeeGroupMappingContext.Update(status_stu);

                                            _YearlyFeeGroupMappingContext.SaveChanges();
                                        }
                                        else
                                        {
                                            var resultupdate = _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO.Single(t => t.FSCI_Id == resultt.FSCI_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            resultupdate.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                            _YearlyFeeGroupMappingContext.Update(resultupdate);

                                            var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.savetmpdata[j].AMCST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            if (status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount < 0)
                                            {
                                                status_stu.FCSS_ToBePaid = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_ToBePaid = status_stu.FCSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSS_PaidAmount;
                                            }

                                          
                                            status_stu.FCSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FCSS_ExcessPaidAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                                            }

                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FCSS_RunningExcessAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FCSS_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSS_PaidAmount - status_stu.FCSS_CurrentYrCharges;
                                            }

                                            _YearlyFeeGroupMappingContext.Update(status_stu);

                                            _YearlyFeeGroupMappingContext.SaveChanges();
                                        }

                                        k++;
                                    }
                                    j++;
                                }
                                transaction.Commit();
                            }
                        }
                    }
                }
                else if (data.radiobtnvalue.Equals("Staff"))
                {
                    Insert_staff_concession(data);
                }
                else if (data.radiobtnvalue.Equals("Others"))
                {
                    Insert_Others_concession(data);
                }

            }
            catch (Exception e)
            {
                data.returnval = "false";
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CollegeConcessionDTO Insert_Others_concession(CollegeConcessionDTO data)
        {
            try
            {
                using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    if (data.savetmpdata != null || data.savetmpdata1 != null)
                    {
                        int j = 0, k = 0;

                        while (j < data.savetmpdata.Count())
                        {
                            while (k < data.savetmpdata1.Count())
                            {
                                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO
                                                     where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOCC_Id == b.FOCC_Id && a.FMCOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                     select new CollegeConcessionDTO
                                                     {
                                                         FOC_Id = a.FOCC_Id
                                                     }
               ).Distinct().ToArray();

                                Fee_Others_Concession_CollegeDMO pmm = new Fee_Others_Concession_CollegeDMO();
                                if (data.fillheaddata.Length <= 0)
                                {
                                    pmm.FMCOST_Id = data.savetmpdata[j].FMOST_Id;
                                    pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                    pmm.MI_Id = data.MI_Id;
                               
                                    pmm.ASMAY_Id = data.ASMAY_Id;
                                    pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                    pmm.FOCC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                    pmm.FOCC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                    pmm.FOCC_ActiveFlag = true;
                                    pmm.FOCC_CreatedDate = DateTime.Now;
                                    pmm.FOCC_UpdatedDate = DateTime.Now;
                                    pmm.FOCC_CreatedBy = data.user_id;
                                    pmm.FOCC_UpdatedBy = data.user_id;
                                    pmm.FCMA_Id = 9;

                                    _YearlyFeeGroupMappingContext.Add(pmm);
                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else if (data.fillheaddata.Length == 1)
                                {
                                    var fetchfscid = (from a in _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO
                                                      from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO
                                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOCC_Id == b.FOCC_Id && a.FMCOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                      select new CollegeConcessionDTO
                                                      {
                                                          FOC_Id = a.FOCC_Id
                                                      }
              ).Distinct().SingleOrDefault();

                                    pmm.FOCC_Id = fetchfscid.FCSC_Id;
                                }

                                Fee_Others_Concession_Installments_CollegeDMO pmmins = new Fee_Others_Concession_Installments_CollegeDMO();
                                var resultt = (from a in _YearlyFeeGroupMappingContext.Fee_Others_Concession_CollegeDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO
                                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOCC_Id == b.FOCC_Id && a.FMCOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                               select new CollegeConcessionInstallmentDMO
                                               {
                                                   FSCI_Id = b.FOCC_Id
                                               }).SingleOrDefault();

                                if (resultt == null)
                                {

                                    pmmins.FOCC_Id = pmm.FOCC_Id;
                                    pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                    pmmins.FSCIC_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    pmmins.FSCIC_CreatedDate = DateTime.Now;
                                    pmmins.FSCIC_UpdatedDate = DateTime.Now;
                                    pmmins.FOCIC_CreatedBy = data.user_id;
                                    pmmins.FOCIC_UpdatedBy = data.user_id;
                                    _YearlyFeeGroupMappingContext.Add(pmmins);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMCOST_Id == data.savetmpdata[j].FMOST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSOST_PaidAmount < 0)
                                    {
                                        status_stu.FCSSOST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_ToBePaid = status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSOST_PaidAmount;
                                    }


                                    status_stu.FCSSOST_TotalCharges = status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FCSSOST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;



                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSOST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_ToBePaid - status_stu.FCSSOST_CurrentYrCharges;
                                    }


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSOST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges;
                                    }


                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();

                                }
                                else
                                {
                                    var resultupdate = _YearlyFeeGroupMappingContext.Fee_Others_Concession_Installments_CollegeDMO.Single(t => t.FOCC_Id == resultt.FSCI_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    resultupdate.FSCIC_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    _YearlyFeeGroupMappingContext.Update(resultupdate);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMCOST_Id == data.savetmpdata[j].FMOST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSOST_PaidAmount < 0)
                                    {
                                        status_stu.FCSSOST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_ToBePaid = status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSOST_PaidAmount;
                                    }

                                    if (status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount > 0)
                                    {
                                        status_stu.FCSSOST_TotalCharges = status_stu.FCSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_TotalCharges = 0;
                                    }

                                    status_stu.FCSSOST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSOST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges;
                                    }

                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSOST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSOST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSOST_PaidAmount - status_stu.FCSSOST_CurrentYrCharges;
                                    }

                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FCSSST_ToBePaid = (status_stu.FCSSST_ToBePaid + status_stu.FCSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FCSSST_ToBePaid = status_stu.FCSSST_ToBePaid;
                                    //status_stu.FCSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    //_YearlyFeeGroupMappingContext.Update(status_stu);

                                    //_YearlyFeeGroupMappingContext.SaveChanges();
                                }

                                k++;
                            }
                            j++;
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeConcessionDTO Insert_staff_concession(CollegeConcessionDTO data)
        {
            try
            {
                using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    if (data.savetmpdata != null || data.savetmpdata1 != null)
                    {
                        int j = 0, k = 0;

                        while (j < data.savetmpdata.Count())
                        {
                            while (k < data.savetmpdata1.Count())
                            {
                                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO
                                                     where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FECC_Id == b.FECC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                     select new CollegeConcessionDTO
                                                     {
                                                         FCSC_Id = a.FECC_Id
                                                     }
               ).Distinct().ToArray();

                                Fee_Employee_Concession_CollegeDMO pmm = new Fee_Employee_Concession_CollegeDMO();
                                if (data.fillheaddata.Length <= 0)
                                {
                                    pmm.HRME_Id = data.savetmpdata[j].HRME_Id;
                                    pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                    pmm.MI_Id = data.MI_Id;
                                    // pmm.FEC_Id = 1;
                                    pmm.ASMAY_Id = data.ASMAY_Id;
                                    pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                    pmm.FECC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                    pmm.FECC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                    pmm.FECC_ActiveFlag = true;
                                    pmm.FECC_CreatedDate = DateTime.Now;
                                    pmm.FECC_UpdatedDate = DateTime.Now;
                                    pmm.FECC_UpdatedBy = data.user_id;
                                    pmm.FECC_CreatedBy = data.user_id;
                                    pmm.FMCC_Id = 2;
                                    _YearlyFeeGroupMappingContext.Add(pmm);
                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else if (data.fillheaddata.Length == 1)
                                {
                                    var fetchfscid = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO
                                                      from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO
                                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FECC_Id == b.FECC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                      select new CollegeConcessionDTO
                                                      {
                                                          FCSC_Id = a.FECC_Id
                                                      }
              ).Distinct().SingleOrDefault();

                                    pmm.FECC_Id = fetchfscid.FCSC_Id;
                                }

                                Fee_Employee_Concession_Installments_CollegeDMO pmmins = new Fee_Employee_Concession_Installments_CollegeDMO();
                                var resultt = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_CollegeDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO
                                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FECC_Id == b.FECC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                               select new CollegeConcessionInstallmentDMO
                                               {
                                                   FSCI_Id = b.FECC_Id
                                               }).SingleOrDefault();

                                if (resultt == null)
                                {

                                    pmmins.FECC_Id = pmm.FECC_Id;
                                    pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                    pmmins.FECIC_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    pmmins.FECIC_CreatedDate = DateTime.Now;
                                    pmmins.FECIC_UpdatedDate = DateTime.Now;
                                    pmmins.FECIC_CreatedBy = data.user_id;
                                    pmmins.FECIC_UpdatedBy = data.user_id;
                                    _YearlyFeeGroupMappingContext.Add(pmmins);


                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.savetmpdata[j].HRME_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);


                                    if (status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSST_PaidAmount < 0)
                                    {
                                        status_stu.FCSSST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_ToBePaid = status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSST_PaidAmount;
                                    }

                                    if (status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount > 0)
                                    {
                                        status_stu.FCSSST_TotalCharges = status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_TotalCharges = 0;
                                    }


                                    //status_stu.FCSSST_TotalCharges = status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FCSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;



                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges;
                                    }


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges;
                                    }


                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FCSSST_ToBePaid = (status_stu.FCSSST_ToBePaid + status_stu.FCSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FCSSST_ToBePaid = status_stu.FCSSST_ToBePaid;
                                    //status_stu.FCSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    //_YearlyFeeGroupMappingContext.Update(status_stu);

                                    //_YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else
                                {
                                    var resultupdate = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_Installments_CollegeDMO.Single(t => t.FECC_Id == resultt.FSCI_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    resultupdate.FECIC_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    _YearlyFeeGroupMappingContext.Update(resultupdate);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.savetmpdata[j].HRME_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSST_PaidAmount < 0)
                                    {
                                        status_stu.FCSSST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_ToBePaid = status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FCSSST_PaidAmount;
                                    }

                                    status_stu.FCSSST_TotalCharges = status_stu.FCSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FCSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges;
                                    }

                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FCSSST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FCSSST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FCSSST_PaidAmount - status_stu.FCSSST_CurrentYrCharges;
                                    }

                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FCSSST_ToBePaid = (status_stu.FCSSST_ToBePaid + status_stu.FCSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FCSSST_ToBePaid = status_stu.FCSSST_ToBePaid;
                                    //status_stu.FCSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    //_YearlyFeeGroupMappingContext.Update(status_stu);

                                    //_YearlyFeeGroupMappingContext.SaveChanges();
                                }

                                k++;
                            }
                            j++;
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeConcessionDTO selectcatorclass(CollegeConcessionDTO data)
        {
            try
            {
    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public CollegeConcessionDTO getacademir(CollegeConcessionDTO data)
        {
            try
            {
                data.studentdata = (from a in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                    from b in _YearlyFeeGroupMappingContext.CollegeConcessionInstallmentDMO
                                    from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                    from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                  //  from e in _YearlyFeeGroupMappingContext.admissioncls
                                    from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                    from g in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                    from h in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                    where (d.ASMAY_Id == a.ASMAY_Id && a.FCSC_Id == b.FCSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMCST_Id == c.AMCST_Id && a.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new CollegeConcessionDTO
                                    {
                                        studentname = c.AMCST_FirstName + '-' + c.AMCST_MiddleName + '-' + c.AMCST_LastName,
                                      //  ASMCL_ClassName = e.ASMCL_ClassName,
                                        FMG_GroupName = f.FMG_GroupName,
                                        FMH_FeeName = g.FMH_FeeName,
                                        FTI_Name = h.FTI_Name,
                                        FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                        FCSC_Id = a.FCSC_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSCI_ID = b.FSCI_Id,
                                    }
                             ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeConcessionDTO checkpaiddetails(CollegeConcessionDTO data)
        {
            try
            {
                if (data.radiobtnvalue == "")
                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        from d in _YearlyFeeGroupMappingContext.CollegeConcessionDMO
                                        from e in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                        from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        where (c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == c.AMCST_Id  && a.AMCST_SOL.Equals("S") && a.AMCST_ActiveFlag == true && c.ACYST_ActiveFlag == 1 && c.AMCST_Id == e.AMCST_Id && c.ASMAY_Id == e.ASMAY_Id && (e.FCSS_PaidAmount > 0 || e.FCSS_ConcessionAmount > 0) && e.FMG_Id == f.FMG_Id && f.FMG_CompulsoryFlag == "F" )
                                        select new CollegeConcessionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                            AMCST_AdmNo = a.AMCST_AdmNo,
                                            AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                            ACYST_RollNo = c.ACYST_RollNo,
                                        }
       ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
