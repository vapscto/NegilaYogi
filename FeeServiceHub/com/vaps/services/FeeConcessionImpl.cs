using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
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

namespace FeeServiceHub.com.vaps.services
{
    public class FeeConcessionImpl : interfaces.FeeConcessionInterface
    {
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        public FeeConcessionImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _context = context;
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
        }

        public FeeConcessionDTO deleteconcess(FeeConcessionDTO data)
        {
            try
            {
                if(data.radiobtnvalue!="Staff" && data.radiobtnvalue != "Others")
                {
                    var amstid = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id && t.ASMAY_ID == data.ASMAY_Id && t.MI_Id == data.MI_Id).AMST_Id;
                    var fmgid = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id && t.ASMAY_ID == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;
                    var fmhid = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id && t.ASMAY_ID == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;
                    var ftiid = _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO.Single(t => t.FSCI_ID == data.FSCI_ID && t.FSCI_FSC_Id == data.FSC_Id).FTI_Id;
                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@fsc_id and FSCI_ID=@fsci_id
                    var paidamount = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == amstid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FSS_PaidAmount;
                    //AMST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("deleteconcession @p0,@p1,@p2,@p3,@p4", data.FSC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FSCI_ID);
                        data.returnval = "true";
                    }
                    else
                    {
                        var result = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id);

                        var resultinstallment = _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO.Single(t => t.FSCI_ID == data.FSCI_ID);

                        var status_stu = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == amstid && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid);
                       
                        if (status_stu.FSS_CurrentYrCharges - status_stu.FSS_ConcessionAmount - status_stu.FSS_PaidAmount < 0)
                        {
                            status_stu.FSS_ToBePaid = 0;
                        }
                        else
                        {
                            status_stu.FSS_ToBePaid = status_stu.FSS_CurrentYrCharges - status_stu.FSS_ConcessionAmount - status_stu.FSS_PaidAmount;
                        }

                        if (status_stu.FSS_CurrentYrCharges - status_stu.FSS_ConcessionAmount > 0)
                        {
                            status_stu.FSS_TotalToBePaid = status_stu.FSS_CurrentYrCharges - status_stu.FSS_ConcessionAmount;
                        }
                        else
                        {
                            status_stu.FSS_TotalToBePaid = 0;
                        }

                        status_stu.FSS_ConcessionAmount = status_stu.FSS_ConcessionAmount;

                        if (status_stu.FSS_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                        {
                            status_stu.FSS_ExcessPaidAmount = 0;
                        }
                        else
                        {
                            status_stu.FSS_ExcessPaidAmount = status_stu.FSS_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                        }


                        if (status_stu.FSS_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                        {
                            status_stu.FSS_RunningExcessAmount = 0;
                        }
                        else
                        {
                            status_stu.FSS_RunningExcessAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                        }

                        _YearlyFeeGroupMappingContext.Remove(result);
                        _YearlyFeeGroupMappingContext.Remove(resultinstallment);
                        _YearlyFeeGroupMappingContext.Update(status_stu);

                        _YearlyFeeGroupMappingContext.SaveChanges();
                        data.returnval = "paid";
                    }
                }

               else if(data.radiobtnvalue.Equals("Staff"))
                {
                    var hrmeid = _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO.Single(t => t.FEC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).HRME_Id;

                    var fmgid = _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO.Single(t => t.FEC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;

                    var fmhid = _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO.Single(t => t.FEC_Id == data.FEC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;

                    var ftiid = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO.Single(t => t.FECI_Id == data.FECI_Id && t.FECI_FEC_Id == data.FEC_Id).FTI_Id;

                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@fsc_id and FSCI_ID=@fsci_id

                    var paidamount = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Single(t => t.HRME_Id == hrmeid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FSSST_PaidAmount;
                    //AMST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("deletestaffconcession @p0,@p1,@p2,@p3,@p4", data.FEC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FECI_Id);
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
                    var hrmeid = _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO.Single(t => t.FOC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMOST_Id;

                    var fmgid = _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO.Single(t => t.FOC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMG_Id;

                    var fmhid = _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO.Single(t => t.FOC_Id == data.FOC_Id && t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).FMH_Id;

                    var ftiid = _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO.Single(t => t.FOCI_Id == data.FOCI_Id && t.FOC_Id == data.FOC_Id).FTI_Id;

                    //select @ftiid=FTI_Id,@amount= FSCI_ConcessionAmount from Fee_Student_Concession_Installments where FSCI_FSC_Id=@fsc_id and FSCI_ID=@fsci_id

                    var paidamount = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Single(t => t.FMOST_Id == hrmeid && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMG_Id == fmgid && t.FMH_Id == fmhid && t.FTI_Id == ftiid).FSSOST_PaidAmount;
                    //AMST_Id=@amstid and FMG_Id=@fmgid and FMH_Id=@fmhid and FTI_Id=@ftiid and ASMAY_Id=@asmay_id and MI_Id=@mi_id  

                    if (paidamount == 0)
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("deleteothersconcession @p0,@p1,@p2,@p3,@p4", data.FOC_Id, data.ASMAY_Id, data.MI_Id, data.userid, data.FOCI_Id);
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "paid";
                    }
                }
            }
            catch(Exception ex)
            {
                data.returnval = "false";
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public FeeConcessionDTO EditconcessionDetails(FeeConcessionDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _YearlyFeeGroupMappingContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.ToArray();

                if (data.configset.Equals("T"))
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                      from b in _YearlyFeeGroupMappingContext.feeTr
                                      where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_GroupName = b.FMT_Name,
                                          FMG_Id = a.FMT_Id,
                                      }
     ).Distinct().ToArray();
                }
                else if (data.configset.Equals("G"))
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeGroup
                                      from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                      where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                      select new FeeConcessionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                  ).ToArray();
                }

                    data.EditfeeDetails = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                 from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                 from e in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                 from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                 from g in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                 from h in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                                 from i in _YearlyFeeGroupMappingContext.feeTr
                                 from j in _YearlyFeeGroupMappingContext.School_M_Class
                                 where (a.AMST_Id==b.AMST_Id && b.AMST_Id==c.AMST_Id && b.ASMAY_Id==c.ASMAY_Id && c.AMST_Id==d.AMST_Id && c.ASMAY_Id==d.ASMAY_ID && c.FMH_Id==d.FMH_Id && c.FMG_Id==d.FMG_Id && d.FSC_Id==e.FSCI_FSC_Id && c.FTI_Id==e.FTI_Id && c.FMH_Id==f.FMH_Id && c.FTI_Id==g.FTI_Id && d.FMH_Id==h.FMH_Id && h.FTI_Id==e.FTI_Id && h.FMT_Id==i.FMT_Id && b.ASMCL_Id==j.ASMCL_Id && a.MI_Id==data.MI_Id && e.FSCI_ID==data.FSCI_ID
                                 && b.ASMAY_Id == data.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                 {
                                    ASMAY_Id=c.ASMAY_Id,
                                    ASMCL_ID=b.ASMCL_Id,
                                    classname=j.ASMCL_ClassName,
                                    termname=i.FMT_Name,
                                    FMT_Id=i.FMT_Id,
                                    AMST_RegistrationNo=a.AMST_RegistrationNo,
                                    AMST_AdmNo=a.AMST_AdmNo,
                                    AMST_FirstName=a.AMST_FirstName,
                                    FMH_Id=f.FMH_Id,
                                    FMG_Id=c.FMG_Id,
                                    FTI_Id=c.FTI_Id,
                                    FMA_Id=c.FMA_Id,
                                    FSCI_ID = e.FSCI_ID,
                                    FSC_Id=d.FSC_Id,
                                    FMH_FeeName =f.FMH_FeeName,
                                    FTI_Name=g.FTI_Name,
                                    FMA_Amount = c.FSS_TotalToBePaid,
                                    FSCI_ConcessionAmount = c.FSS_ConcessionAmount,
                                    FSC_ConcessionType=d.FSC_ConcessionType,
                                    FSC_ConcessionReason = d.FSC_ConcessionReason,
                                    Amst_Id=a.AMST_Id
                                 }).Distinct().ToArray();


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO fillamount(FeeConcessionDTO data)
        {
            try
            {
                var saved_fma = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 where (a.FMA_Id == b.FMA_Id && b.FYP_Id == c.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id  && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id )
                                 select b.FMA_Id
).Distinct().ToList();
                var fetchclass = (from a in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                  select new FeeStudentTransactionDTO
                                  {
                                      ASMCL_ID = a.ASMCL_Id,
                                      ASMAY_Id = a.ASMAY_Id
                                  }
).Distinct().ToArray();

                string classid = "0", academicyearid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    classid = fetchclass[s].ASMCL_ID.ToString();
                    academicyearid = fetchclass[s].ASMAY_Id.ToString();
                }

                var myArray = data.multiplegroups.Split(',');
                List<long> terms_groups = new List<long>();
                for (int i = 0; i < myArray.Length; i++)
                {
                    terms_groups.Add(Convert.ToInt64(myArray[i]));
                }
                data.terms_groups = terms_groups.ToArray();
                if (data.configset.Equals("T"))
                {
                    if(data.radiobtnvalue!= "Staff" && data.radiobtnvalue != "Others")
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstudentlistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from d in _YearlyFeeGroupMappingContext.feeMIY
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              from f in _YearlyFeeGroupMappingContext.feeYCCC
                                              from g in _YearlyFeeGroupMappingContext.feeYCC
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }
                    else if(data.radiobtnvalue.Equals("Staff"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstafflistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillotherslistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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
                else if(data.configset.Equals("G"))
                {

                    if (data.radiobtnvalue != "Staff")
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstudentlistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                              from b in _YearlyFeeGroupMappingContext.feeMIY
                                              from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }

                    else if(data.radiobtnvalue.Equals("Staff"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillstafflistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("fillotherslistforconcession_term_all @p0,@p1,@p2,@p3,@p4,@p5", data.AMST_Id, data.ASMAY_Id, data.MI_Id, data.multiplegroups, data.userid, data.configset);

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


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeConcessionDTO fillheaddetailsss(FeeConcessionDTO data)
        {
            try
            {
                if (data.configset.Equals("T"))
                {
                    data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Yearlygroups
                                         from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                         from f in _YearlyFeeGroupMappingContext.feeMTH
                                         where (f.FMH_Id==b.FMH_Id && f.FTI_Id==d.FTI_Id && a.FMG_Id == b.FMG_Id && b.FMH_Id == c.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && data.FMG_Ids.Contains(f.FMT_Id) && d.FMI_Id == b.FMI_Id && e.FMI_Id == d.FMI_Id) //&& f.FMT_Id == data.FMG_Id
                                         select new FeeConcessionDTO
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
                    data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Yearlygroups
                                         from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                         from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                         from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                         from e in _YearlyFeeGroupMappingContext.FeeInstallmentDMO
                                         where (a.FMG_Id == b.FMG_Id && b.FMH_Id == c.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && data.FMG_Ids.Contains(a.FMG_Id) && d.FMI_Id == b.FMI_Id && e.FMI_Id == d.FMI_Id)//&& a.FMG_Id == data.FMG_Id
                                         select new FeeConcessionDTO
                                         {
                                             FMH_FeeName = c.FMH_FeeName,
                                             FTI_Name = d.FTI_Name,
                                             FTI_Id = d.FTI_Id,
                                             FMH_Id = c.FMH_Id,
                                         }
     ).Distinct().ToArray();
                }

                // con checking

                if(data.AMST_Id!=0)
                {
                    if (data.configset.Equals("T"))
                    {
                        data.savedcondatalist = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                             from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                             from d in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                             from e in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                             from f in _YearlyFeeGroupMappingContext.feeMTH
                                             where (f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && data.FMG_Ids.Contains(f.FMT_Id) && b.FSCI_FSC_Id == a.FSC_Id && c.FMH_Id == a.FMH_Id && d.FTI_Id == b.FTI_Id && e.AMST_Id == a.AMST_Id && e.FMH_Id == c.FMH_Id && e.FTI_Id == b.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == e.ASMAY_Id && a.ASMAY_ID == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)//&& f.FMT_Id == data.FMG_Id
                                                 select new FeeConcessionDTO
                                             {
                                                 FMH_FeeName = c.FMH_FeeName,
                                                 FTI_Name = d.FTI_Name,
                                                 FTI_Id = d.FTI_Id,
                                                 FMH_Id = a.FMH_Id,
                                                 FMA_Amount = e.FSS_ToBePaid,
                                                 FMA_Id = e.FMA_Id,
                                                 FMG_Id = a.FMG_Id,
                                                 FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                                 FSC_ConcessionType = a.FSC_ConcessionType,
                                                 FSC_ConcessionReason = a.FSC_ConcessionReason,
                                             }
      ).Distinct().ToArray();

                       
                            data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 from d in _YearlyFeeGroupMappingContext.feeMTH
                                                 where (a.FMH_Id == d.FMH_Id && a.FTI_Id == d.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && b.FMH_Id == a.FMH_Id && c.FTI_Id == a.FTI_Id && data.FMG_Ids.Contains(d.FMT_Id))
                                                 select new FeeConcessionDTO//&& d.FMT_Id == data.FMG_Id
                                                 {
                                                     FMH_FeeName = b.FMH_FeeName,
                                                     FTI_Name = c.FTI_Name,
                                                     FTI_Id = a.FTI_Id,
                                                     FMH_Id = a.FMH_Id,
                                                     //FMA_Amount = a.FSS_NetAmount,
                                                     FMA_Amount = a.FSS_ToBePaid,
                                                     FMA_Id = a.FMA_Id,
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

        public FeeConcessionDTO filstaff(FeeConcessionDTO data)
        {
            try
            {
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
                                 }).ToList().Distinct().OrderBy(t => t.HRME_Id).Take(5).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO getdata(FeeConcessionDTO data)

        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

                List<FeeClassCategoryDMO> category = new List<FeeClassCategoryDMO>();
                category = _YearlyFeeGroupMappingContext.FeeClassCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.fillcategory = category.ToArray();

                List<Fee_Master_ConcessionDMO> feecategory = new List<Fee_Master_ConcessionDMO>();
                feecategory = _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.fillfeecategory = feecategory.ToArray();

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

                data.otherlist = (from a in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                  where (a.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMOST_ActiveFlag == true)
                                  select new Fee_Master_OtherStudentsDTO
                                  {
                                      FMOST_Id = a.FMOST_Id,
                                      FMOST_StudentName=a.FMOST_StudentName.Trim(),
                                      FMOST_StudentMobileNo = a.FMOST_StudentMobileNo,
                                      FMOST_StudentEmailId = a.FMOST_StudentEmailId,
                                  }).ToList().Distinct().OrderBy(t => t.FMOST_Id).ToArray();


                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _YearlyFeeGroupMappingContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag==true).ToList();
                data.fillclass = allclas.ToArray();
                
                if (data.configset.Equals("T"))
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                            from b in _YearlyFeeGroupMappingContext.feeTr
                                            where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMG_GroupName = b.FMT_Name,
                                                FMG_Id = a.FMT_Id,
                                            }
     ).Distinct().ToArray();

                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                        from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from e in _YearlyFeeGroupMappingContext.admissioncls
                                        from f in _YearlyFeeGroupMappingContext.feeGroup
                                        from g in _YearlyFeeGroupMappingContext.feehead
                                        from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        where (d.ASMAY_Id == a.ASMAY_ID && a.FSC_Id == b.FSCI_FSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMST_Id == c.AMST_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                        select new FeeConcessionDTO
                                        {
                                            studentname = c.AMST_FirstName + ' ' + c.AMST_MiddleName + ' ' + c.AMST_LastName,
                                            ASMCL_ClassName = e.ASMCL_ClassName,
                                            FMG_GroupName = f.FMG_GroupName,
                                            FMH_FeeName = g.FMH_FeeName,
                                            FTI_Name = h.FTI_Name,
                                            FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                            FSC_Id = a.FSC_Id,
                                            FTI_Id = b.FTI_Id,
                                            FSCI_ID = b.FSCI_ID,
                                            AMST_AdmNo = c.AMST_AdmNo

                                        }
                                ).Distinct().ToArray();


                    data.staffdata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO
                                        from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO
                                        from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                        from d in _YearlyFeeGroupMappingContext.HR_Master_Department
                                        from e in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                        from f in _YearlyFeeGroupMappingContext.feeGroup
                                        from g in _YearlyFeeGroupMappingContext.feehead
                                        from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                        where (a.HRME_Id==c.HRME_Id && a.FEC_Id==b.FECI_FEC_Id && c.HRMD_Id==d.HRMD_Id && c.HRMDES_Id==e.HRMDES_Id && c.MI_Id==data.MI_Id && a.FMH_Id==g.FMH_Id && b.FTI_Id==h.FTI_Id && a.FMG_Id==f.FMG_Id && a.ASMAY_Id==data.ASMAY_Id)
                                        select new FeeConcessionDTO
                                        {
                                            HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + '-' + c.HRME_EmployeeMiddleName + '-' +                     c.HRME_EmployeeLastName,
                                            HRMDES_DesignationName = e.HRMDES_DesignationName,
                                            HRMD_DepartmentName = d.HRMD_DepartmentName,
                                            FMG_GroupName = f.FMG_GroupName,
                                            FMH_FeeName = g.FMH_FeeName,
                                            FTI_Name = h.FTI_Name,
                                            FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                            FEC_Id = a.FEC_Id,
                                            FTI_Id = b.FTI_Id,
                                            FECI_Id = b.FECI_Id,

                                        }
                               ).Distinct().ToArray();

                    data.othersdata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO
                                      from c in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                      from f in _YearlyFeeGroupMappingContext.feeGroup
                                      from g in _YearlyFeeGroupMappingContext.feehead
                                      from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                      where (a.FMOST_Id == c.FMOST_Id && a.FOC_Id == b.FOC_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeConcessionDTO
                                      {
                                          FMOST_StudentName = c.FMOST_StudentName,
                                          FMOST_StudentMobileNo = c.FMOST_StudentMobileNo,
                                          FMOST_StudentEmailId = c.FMOST_StudentEmailId,
                                          FMG_GroupName = f.FMG_GroupName,
                                          FMH_FeeName = g.FMH_FeeName,
                                          FTI_Name = h.FTI_Name,
                                          FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                          FOC_Id = a.FOC_Id,
                                          FTI_Id = b.FTI_Id,
                                          FOCI_Id = b.FOCI_Id,

                                      }
                              ).Distinct().ToArray();

                }
                else
                {
                    data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeGroup
                                      from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                      where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                      select new FeeConcessionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName
                                      }
                                  ).ToArray();

                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                        from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                        from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from e in _YearlyFeeGroupMappingContext.admissioncls
                                        from f in _YearlyFeeGroupMappingContext.feeGroup
                                        from g in _YearlyFeeGroupMappingContext.feehead
                                        from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO

                                        where (a.FSC_Id == b.FSCI_FSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMST_Id == c.AMST_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                        select new FeeConcessionDTO
                                        {
                                            studentname = c.AMST_FirstName + ' ' + c.AMST_MiddleName + ' ' + c.AMST_LastName,
                                            ASMCL_ClassName = e.ASMCL_ClassName,
                                            FMG_GroupName = f.FMG_GroupName,
                                            FMH_FeeName = g.FMH_FeeName,
                                            FTI_Name = h.FTI_Name,
                                            FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                            FSC_Id=b.FSCI_FSC_Id,
                                            FTI_Id=b.FTI_Id,
                                            FSCI_ID=b.FSCI_ID
                                        }
                               ).Distinct().ToArray();

                    data.staffdata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO
                                      from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                      from d in _YearlyFeeGroupMappingContext.HR_Master_Department
                                      from e in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                      from f in _YearlyFeeGroupMappingContext.feeGroup
                                      from g in _YearlyFeeGroupMappingContext.feehead
                                      from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                      where (a.HRME_Id == c.HRME_Id && a.FEC_Id == b.FECI_FEC_Id && c.HRMD_Id == d.HRMD_Id && c.HRMDES_Id == e.HRMDES_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeConcessionDTO
                                      {
                                          HRME_EmployeeFirstName = c.HRME_EmployeeFirstName + '-' + c.HRME_EmployeeMiddleName + '-' + c.HRME_EmployeeLastName,
                                          HRMDES_DesignationName = e.HRMDES_DesignationName,
                                          HRMD_DepartmentName = d.HRMD_DepartmentName,
                                          FMG_GroupName = f.FMG_GroupName,
                                          FMH_FeeName = g.FMH_FeeName,
                                          FTI_Name = h.FTI_Name,
                                          FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                          FEC_Id = a.FEC_Id,
                                          FTI_Id = b.FTI_Id,
                                          FECI_Id = b.FECI_Id,

                                      }
                              ).Distinct().ToArray();

                    data.othersdata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO
                                      from c in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                      from f in _YearlyFeeGroupMappingContext.feeGroup
                                      from g in _YearlyFeeGroupMappingContext.feehead
                                      from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                      where (a.FMOST_Id == c.FMOST_Id && a.FOC_Id == b.FOC_Id && c.MI_Id == data.MI_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.FMG_Id == f.FMG_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeConcessionDTO
                                      {
                                          FMOST_StudentName = c.FMOST_StudentName ,
                                          FMOST_StudentEmailId = c.FMOST_StudentEmailId,
                                          FMOST_StudentMobileNo = c.FMOST_StudentMobileNo,
                                          FMG_GroupName = f.FMG_GroupName,
                                          FMH_FeeName = g.FMH_FeeName,
                                          FTI_Name = h.FTI_Name,
                                          FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                          FOC_Id = a.FOC_Id,
                                          FTI_Id = b.FTI_Id,
                                          FOCI_Id = b.FOCI_Id,
                                      }
                              ).Distinct().ToArray();
                }

                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && a.FMSFH_ConcessionFlag=="1")//&& a.IVRMSTAUL_Id==data.User_Id
                                           select new FeeSpecialFeeGroupDTO
                                           {
                                               FMSFH_Id = a.FMSFH_Id,
                                               FMSFH_Name = a.FMSFH_Name,
                                               FMSFHFH_Id = b.FMSFHFH_Id,
                                               FMH_ID = b.FMH_Id,
                                               FMH_Name = c.FMH_FeeName
                                           }).Distinct().ToArray();

                var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true && t.FMSFH_ConcessionFlag=="1").Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();

                        }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        //public FeeConcessionDTO savedatadelegateold(FeeConcessionDTO data)
        //{
        //   try
        //    {
        //        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
        //        {
        //            if (data.savetmpdata != null)
        //            {
        //                int j = 0, k = 0;
        //                while (j < data.savetmpdata.Count())
        //                {
        //                    FeeConcessionDMO pmm = new FeeConcessionDMO();
        //                    if (data.FMG_Id != 0)
        //                    {
        //                        pmm.AMST_Id = data.savetmpdata[j].AMST_Id;
        //                        pmm.FMG_Id = data.FMG_Id;
        //                        pmm.MI_Id = data.MI_Id;
        //                        pmm.FMC_Id = 1;
        //                        pmm.ASMAY_ID = data.ASMAY_ID;
        //                        pmm.FMH_Id = data.savetmpdata1[j].FMH_Id;
        //                        pmm.FSC_ConcessionReason = data.savetmpdata1[j].FSC_ConcessionReason;
        //                        pmm.FSC_ConcessionType = data.savetmpdata1[j].FSC_ConcessionType;
        //                        pmm.FMSG_ActiveFlag = "1";
        //                        pmm.CreatedDate = DateTime.Now;
        //                        pmm.UpdatedDate = DateTime.Now;

        //                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
        //                                             from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
        //                                             where (a.ASMAY_ID == data.ASMAY_ID && a.MI_Id== data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id==data.AMST_Id && a.FMG_Id==data.FMG_Id && a.FMH_Id== data.savetmpdata1[j].FMH_Id)
        //                                             select new FeeConcessionDTO
        //                                             {
        //                                                 FSC_Id=a.FSC_Id
        //                                             }
        //        ).Distinct().ToArray();

        //                        if(data.fillheaddata.Length<=0)
        //                        {
        //                            _YearlyFeeGroupMappingContext.Add(pmm);
        //                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
        //                        }

        //                        //head value 

        //                        if (data.savetmpdata1 != null)
        //                        {
        //                            while (k < data.savetmpdata1.Count())
        //                            {
        //                                FeeConcessionInstallmentsDMO pmmins = new FeeConcessionInstallmentsDMO();
        //                                if (data.savetmpdata1[k].FTI_Id != 0)
        //                                {
        //                                    if (data.fillheaddata.Length <= 0)
        //                                    {
        //                                        pmmins.FSCI_FSC_Id = pmm.FSC_Id;
        //                                        pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
        //                                        pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
        //                                        pmmins.CreatedDate = DateTime.Now;
        //                                        pmmins.UpdatedDate = DateTime.Now;

        //                                        _YearlyFeeGroupMappingContext.Add(pmmins);
        //                                        var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
        //                                    }
        //                                    else
        //                                    {
        //                                        var resultt = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
        //                                                       from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
        //                                                       where ( a.MI_Id== data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
        //                                                       select new FeeConcessionInstallmentsDMO
        //                                                       {
        //                                                           FSCI_FSC_Id = a.FSC_Id
        //                                                       });

        //                                        if (resultt.Count()>0)
        //                                        {
        //                                            pmmins.FSCI_ConcessionAmount= data.savetmpdata1[k].FSCI_ConcessionAmount;
        //                                         //   pmmins.FSCI_FSC_Id = resultt.FSCI_FSC_Id;

        //                                            _YearlyFeeGroupMappingContext.Update(pmmins);
        //                                            _YearlyFeeGroupMappingContext.SaveChanges();
        //                                        }
        //                                        //else
        //                                        //{
        //                                        //    pmmins.FSCI_FSC_Id = pmm.FSC_Id;
        //                                        //    pmmins.FTI_Id = data.savetmpdata1[j].FTI_Id;
        //                                        //    pmmins.FSCI_ConcessionAmount = data.savetmpdata1[j].FSCI_ConcessionAmount;
        //                                        //    pmmins.CreatedDate = DateTime.Now;
        //                                        //    pmmins.UpdatedDate = DateTime.Now;

        //                                        //    _YearlyFeeGroupMappingContext.Add(pmmins);
        //                                        //    _YearlyFeeGroupMappingContext.SaveChanges();
        //                                        //}
                                              
        //                                    }
        //                                }
        //                                k++;
        //                            }
        //                        }
        //                        j++;
        //                    }

        //                }
        //            }

        //            transaction.Commit();
        //        }

        //        //                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
        //        //                                     from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
        //        //                                     from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
        //        //                                     from d in _YearlyFeeGroupMappingContext.feehead
        //        //                                     from e in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
        //        //                                     where (a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[0].AMST_Id && b.FTI_Id == c.FTI_Id && d.FMH_Id == a.FMH_Id && a.AMST_Id == e.Amst_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_ID)
        //        //                                     select new FeeConcessionDTO
        //        //                                     {
        //        //                                         FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
        //        //                                         FMH_Id = a.FMH_Id,
        //        //                                         FMG_Id = a.FMG_Id,
        //        //                                         FTI_Id = b.FTI_Id,
        //        //                                         FSC_ConcessionType = a.FSC_ConcessionType,
        //        //                                         FSC_ConcessionReason = a.FSC_ConcessionReason,
        //        //                                         FMH_FeeName = d.FMH_FeeName,
        //        //                                         FTI_Name = c.FTI_Name,
        //        //                                         FMA_Amount = e.Net_amount
        //        //                                     }
        //        //).Distinct().ToArray();

        //        //                if (data.fillheaddata.Length <= 0)
        //        //                {
        //        //                    using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
        //        //                    {
        //        //                        if (data.savetmpdata != null)
        //        //                        {
        //        //                            int j = 0, k = 0;
        //        //                            while (j < data.savetmpdata.Count())
        //        //                            {
        //        //                                FeeConcessionDMO pmm = new FeeConcessionDMO();
        //        //                                if (data.FMG_Id != 0)
        //        //                                {
        //        //                                    pmm.AMST_Id = data.savetmpdata[j].AMST_Id;
        //        //                                    pmm.FMG_Id = data.FMG_Id;
        //        //                                    pmm.MI_Id = data.MI_Id;
        //        //                                    pmm.FMC_Id = 1;
        //        //                                    pmm.ASMAY_ID = data.ASMAY_ID;
        //        //                                    pmm.FMH_Id = data.savetmpdata1[j].FMH_Id;
        //        //                                    pmm.FSC_ConcessionReason = data.savetmpdata1[j].FSC_ConcessionReason;
        //        //                                    pmm.FSC_ConcessionType = data.savetmpdata1[j].FSC_ConcessionType;
        //        //                                    pmm.FMSG_ActiveFlag = "1";
        //        //                                    pmm.CreatedDate = DateTime.Now;
        //        //                                    pmm.UpdatedDate = DateTime.Now;

        //        //                                    _YearlyFeeGroupMappingContext.Add(pmm);
        //        //                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

        //        //                                    //head value 

        //        //                                    if (data.savetmpdata1 != null)
        //        //                                    {
        //        //                                        while (k < data.savetmpdata1.Count())
        //        //                                        {
        //        //                                            FeeConcessionInstallmentsDMO pmmins = new FeeConcessionInstallmentsDMO();
        //        //                                            if (data.savetmpdata1[k].FTI_Id != 0)
        //        //                                            {

        //        //                                                pmmins.FSCI_FSC_Id = pmm.FSC_Id;
        //        //                                                pmmins.FTI_Id = data.savetmpdata1[j].FTI_Id;
        //        //                                                pmmins.FSCI_ConcessionAmount = data.savetmpdata1[j].FSCI_ConcessionAmount;
        //        //                                                pmmins.CreatedDate = DateTime.Now;
        //        //                                                pmmins.UpdatedDate = DateTime.Now;

        //        //                                                _YearlyFeeGroupMappingContext.Add(pmmins);
        //        //                                                _YearlyFeeGroupMappingContext.SaveChanges();

        //        //                                            }
        //        //                                            k++;
        //        //                                        }
        //        //                                    }
        //        //                                    j++;
        //        //                                }

        //        //                            }
        //        //                        }

        //        //                        transaction.Commit();
        //        //                    }
        //        //                }
        //        //                else
        //        //                {
        //        //                    using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
        //        //                    {
        //        //                        if (data.savetmpdata != null)
        //        //                        {
        //        //                            int j = 0, k = 0;
        //        //                            while (j < data.savetmpdata.Count())
        //        //                            {
        //        //                                FeeConcessionDMO pmm = new FeeConcessionDMO();
        //        //                                if (data.FMG_Id != 0)
        //        //                                {

        //        //                                    pmm.AMST_Id = data.savetmpdata[j].AMST_Id;
        //        //                                    pmm.FMG_Id = data.FMG_Id;
        //        //                                    pmm.MI_Id = data.MI_Id;
        //        //                                    pmm.FMC_Id = 1;
        //        //                                    pmm.ASMAY_ID = data.ASMAY_ID;
        //        //                                    pmm.FMH_Id = data.savetmpdata1[j].FMH_Id;
        //        //                                    pmm.FSC_ConcessionReason = data.savetmpdata1[j].FSC_ConcessionReason;
        //        //                                    pmm.FSC_ConcessionType = data.savetmpdata1[j].FSC_ConcessionType;
        //        //                                    pmm.FMSG_ActiveFlag = "1";
        //        //                                    pmm.CreatedDate = DateTime.Now;
        //        //                                    pmm.UpdatedDate = DateTime.Now;

        //        //                                    //_YearlyFeeGroupMappingContext.Add(pmm);
        //        //                                   // var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();

        //        //                                    //head value 

        //        //                                    if (data.savetmpdata1 != null)
        //        //                                    {
        //        //                                        while (k < data.savetmpdata1.Count())
        //        //                                        {
        //        //                                            FeeConcessionInstallmentsDMO pmmins = new FeeConcessionInstallmentsDMO();
        //        //                                            if (data.savetmpdata1[k].FTI_Id != 0)
        //        //                                            {
        //        //                                                var result = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
        //        //                                                              from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
        //        //                                                              where (a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_ID && a.AMST_Id == data.AMST_Id && a.FMH_Id == data.savetmpdata1[j].FMH_Id && a.FMG_Id == data.FMG_Id && b.FTI_Id == data.savetmpdata1[j].FTI_Id)
        //        //                                                              select new FeeConcessionDTO
        //        //                                                              {
        //        //                                                                  FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
        //        //                                                                  FMH_Id = a.FMH_Id,
        //        //                                                                  FMG_Id = a.FMG_Id,
        //        //                                                                  FTI_Id = b.FTI_Id,
        //        //                                                                  FSC_ConcessionType = a.FSC_ConcessionType,
        //        //                                                                  FSC_ConcessionReason = a.FSC_ConcessionReason,
        //        //                                                              }
        //        //).Distinct().ToArray();

        //        //                                                pmmins.FSCI_FSC_Id = pmm.FSC_Id;
        //        //                                                pmmins.FTI_Id = data.savetmpdata1[j].FTI_Id;
        //        //                                                pmmins.FSCI_ConcessionAmount = data.savetmpdata1[j].FSCI_ConcessionAmount;
        //        //                                                pmmins.CreatedDate = DateTime.Now;
        //        //                                                pmmins.UpdatedDate = DateTime.Now;

        //        //                                                _YearlyFeeGroupMappingContext.Update(pmmins);
        //        //                                                _YearlyFeeGroupMappingContext.SaveChanges();

        //        //                                            }
        //        //                                            k++;
        //        //                                        }
        //        //                                    }
        //        //                                    j++;
        //        //                                }

        //        //                            }
        //        //                        }

        //        //                        transaction.Commit();
        //        //                    }
        //        //                }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return data;
        //}


        public FeeConcessionDTO savedatadelegate(FeeConcessionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.radiobtnvalue!="Staff" && data.radiobtnvalue != "Others")
                {
                    if(data.FSCI_ID>0 && data.FSC_Id>0)
                    {
                        using (var transactionupdate = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            var result = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id);

                            var resultinstallment = _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO.Single(t => t.FSCI_ID == data.FSCI_ID);

                            var status_stu = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[0].AMST_Id && t.FMG_Id == data.savetmpdata1[0].FMG_Id && t.FMH_Id == data.savetmpdata1[0].FMH_Id && t.FTI_Id == data.savetmpdata1[0].FTI_Id);

                            result.FSC_ConcessionReason = data.savetmpdata1[0].FSC_ConcessionReason;

                            _YearlyFeeGroupMappingContext.Update(result);

                            resultinstallment.FSCI_ConcessionAmount = data.savetmpdata1[0].FSCI_ConcessionAmount;
                            resultinstallment.UpdatedDate = indianTime;
                            resultinstallment.FSCI_UpdatedBy = data.userid;

                            _YearlyFeeGroupMappingContext.Update(resultinstallment);

                            if (status_stu.FSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount < 0)
                            {
                                status_stu.FSS_ToBePaid = 0;
                            }
                            else
                            {
                                status_stu.FSS_ToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount;
                            }

                            if (status_stu.FSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount > 0)
                            {
                                status_stu.FSS_TotalToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[0].FSCI_ConcessionAmount;
                            }
                            else
                            {
                                status_stu.FSS_TotalToBePaid = 0;
                            }

                            status_stu.FSS_ConcessionAmount = data.savetmpdata1[0].FSCI_ConcessionAmount;

                            if (data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                            {
                                status_stu.FSS_ExcessPaidAmount = 0;
                            }
                            else
                            {
                                status_stu.FSS_ExcessPaidAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                            }


                            if (data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                            {
                                status_stu.FSS_RunningExcessAmount = 0;
                            }
                            else
                            {
                                status_stu.FSS_RunningExcessAmount = data.savetmpdata1[0].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
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
                                        data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                             from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                             where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                             select new FeeConcessionDTO
                                                             {
                                                                 FSC_Id = a.FSC_Id
                                                             }
                       ).Distinct().ToArray();

                                        FeeConcessionDMO pmm = new FeeConcessionDMO();
                                        if (data.fillheaddata.Length <= 0)
                                        {
                                            var FMC_Id = _context.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id).Select(a => a.AMST_Concession_Type).FirstOrDefault();

                                            pmm.AMST_Id = data.savetmpdata[j].AMST_Id;
                                            //pmm.FMG_Id = data.FMG_Id;
                                            pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                            pmm.MI_Id = data.MI_Id;
                                            pmm.FMC_Id = Convert.ToInt64(FMC_Id);
                                            pmm.ASMAY_ID = data.ASMAY_Id;
                                            pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                            pmm.FSC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                            pmm.FSC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                            pmm.FMSG_ActiveFlag = "1";
                                            pmm.CreatedDate = indianTime;
                                            pmm.UpdatedDate = indianTime;

                                            pmm.FSC_CreatedBy = data.userid;
                                            pmm.FSC_UpdatedBy = data.userid;

                                            _YearlyFeeGroupMappingContext.Add(pmm);
                                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                        }
                                        else if (data.fillheaddata.Length >= 1)
                                        {
                                            var fetchfscid = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                              from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                              where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                              select new FeeConcessionDTO
                                                              {
                                                                  FSC_Id = a.FSC_Id
                                                              }
                      ).Distinct();

                                            var result1 = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FSC_Id == fetchfscid.FirstOrDefault().FSC_Id).FirstOrDefault();
                                            result1.FSC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                            _YearlyFeeGroupMappingContext.Update(result1);
                                            _YearlyFeeGroupMappingContext.SaveChanges();

                                            pmm.FSC_Id = fetchfscid.FirstOrDefault().FSC_Id;

                                        }

                                        FeeConcessionInstallmentsDMO pmmins = new FeeConcessionInstallmentsDMO();
                                        var resultt = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                       from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                       where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                                       select new FeeConcessionInstallmentsDMO
                                                       {
                                                           FSCI_ID = b.FSCI_FSC_Id
                                                       }).SingleOrDefault();

                                        if (resultt == null)
                                        {

                                            pmmins.FSCI_FSC_Id = pmm.FSC_Id;
                                            pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                            pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                            pmmins.CreatedDate = DateTime.Now;
                                            pmmins.UpdatedDate = DateTime.Now;
                                            pmmins.FSCI_UpdatedBy = data.userid;

                                            _YearlyFeeGroupMappingContext.Add(pmmins);


                                            var status_stu = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[j].AMST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            if (status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount < 0)
                                            {
                                                status_stu.FSS_ToBePaid = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_ToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount;
                                            }

                                            if (status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount > 0)
                                            {
                                                status_stu.FSS_TotalToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                            }
                                            else
                                            {
                                                status_stu.FSS_TotalToBePaid = 0;
                                            }

                                            status_stu.FSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FSS_ExcessPaidAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                                            }


                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FSS_RunningExcessAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                                            }


                                            _YearlyFeeGroupMappingContext.Update(status_stu);

                                            _YearlyFeeGroupMappingContext.SaveChanges();
                                        }
                                        else
                                        {
                                            var resultupdate = _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO.Single(t => t.FSCI_FSC_Id == resultt.FSCI_ID && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            resultupdate.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                            _YearlyFeeGroupMappingContext.Update(resultupdate);

                                            var status_stu = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[j].AMST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                            if (status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount < 0)
                                            {
                                                status_stu.FSS_ToBePaid = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_ToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSS_PaidAmount;
                                            }

                                            status_stu.FSS_TotalToBePaid = status_stu.FSS_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                            status_stu.FSS_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FSS_ExcessPaidAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
                                            }

                                            if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges < 0)
                                            {
                                                status_stu.FSS_RunningExcessAmount = 0;
                                            }
                                            else
                                            {
                                                status_stu.FSS_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSS_PaidAmount - status_stu.FSS_CurrentYrCharges;
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
                else if(data.radiobtnvalue.Equals("Staff"))
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

        public FeeConcessionDTO Insert_Others_concession(FeeConcessionDTO data)
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
                                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO
                                                     where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOC_Id == b.FOC_Id && a.FMOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                     select new FeeConcessionDTO
                                                     {
                                                         FOC_Id = a.FOC_Id
                                                     }
               ).Distinct().ToArray();

                                Fee_Others_ConcessionDMO pmm = new Fee_Others_ConcessionDMO();
                                if (data.fillheaddata.Length <= 0)
                                {
                                    pmm.FMOST_Id = data.savetmpdata[j].FMOST_Id;
                                    pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                    pmm.MI_Id = data.MI_Id;
                                    // pmm.FEC_Id = 1;
                                    pmm.ASMAY_Id = data.ASMAY_Id;
                                    pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                    pmm.FOC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                    pmm.FOC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                    pmm.FOC_ActiveFlag = true;
                                    pmm.CreatedDate = DateTime.Now;
                                    pmm.UpdatedDate = DateTime.Now;
                                    //pmm.FMCC_Id = 1;
                                    _YearlyFeeGroupMappingContext.Add(pmm);
                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else if (data.fillheaddata.Length == 1)
                                {
                                    var fetchfscid = (from a in _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO
                                                      from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO
                                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOC_Id == b.FOC_Id && a.FMOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                      select new FeeConcessionDTO
                                                      {
                                                          FOC_Id = a.FOC_Id
                                                      }
              ).Distinct().SingleOrDefault();

                                    pmm.FOC_Id = fetchfscid.FSC_Id;
                                }

                                Fee_Others_Concession_InstallmentsDMO pmmins = new Fee_Others_Concession_InstallmentsDMO();
                                var resultt = (from a in _YearlyFeeGroupMappingContext.Fee_Others_ConcessionDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO
                                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FOC_Id == b.FOC_Id && a.FMOST_Id == data.savetmpdata[j].FMOST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                               select new FeeConcessionInstallmentsDMO
                                               {
                                                   FSCI_ID = b.FOC_Id
                                               }).SingleOrDefault();

                                if (resultt == null)
                                {

                                    pmmins.FOC_Id = pmm.FOC_Id;
                                    pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                    pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    pmmins.CreatedDate = DateTime.Now;
                                    pmmins.UpdatedDate = DateTime.Now;

                                    _YearlyFeeGroupMappingContext.Add(pmmins);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMOST_Id == data.savetmpdata[j].FMOST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSOST_PaidAmount < 0)
                                    {
                                        status_stu.FSSOST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_ToBePaid = status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSOST_PaidAmount;
                                    }


                                    status_stu.FSSOST_TotalCharges = status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FSSOST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;



                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSOST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_ToBePaid - status_stu.FSSOST_CurrentYrCharges;
                                    }


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSOST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges;
                                    }


                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FSSST_ToBePaid = (status_stu.FSSST_ToBePaid + status_stu.FSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FSSST_ToBePaid = status_stu.FSSST_ToBePaid;
                                    //status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    //_YearlyFeeGroupMappingContext.Update(status_stu);

                                    //_YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else
                                {
                                    var resultupdate = _YearlyFeeGroupMappingContext.Fee_Others_Concession_InstallmentsDMO.Single(t => t.FOC_Id == resultt.FSCI_ID && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    resultupdate.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    _YearlyFeeGroupMappingContext.Update(resultupdate);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FMOST_Id == data.savetmpdata[j].FMOST_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSOST_PaidAmount < 0)
                                    {
                                        status_stu.FSSOST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_ToBePaid = status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSOST_PaidAmount;
                                    }

                                    if(status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount>0)
                                    {
                                        status_stu.FSSOST_TotalCharges = status_stu.FSSOST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_TotalCharges = 0;
                                    }

                                    status_stu.FSSOST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSOST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges;
                                    }

                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSOST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSOST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSOST_PaidAmount - status_stu.FSSOST_CurrentYrCharges;
                                    }

                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FSSST_ToBePaid = (status_stu.FSSST_ToBePaid + status_stu.FSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FSSST_ToBePaid = status_stu.FSSST_ToBePaid;
                                    //status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

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

        public  FeeConcessionDTO Insert_staff_concession(FeeConcessionDTO data)
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
                                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO
                                                     where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FEC_Id == b.FECI_FEC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                     select new FeeConcessionDTO
                                                     {
                                                         FSC_Id = a.FEC_Id
                                                     }
               ).Distinct().ToArray();

                                Fee_Employee_ConcessionDMO pmm = new Fee_Employee_ConcessionDMO();
                                if (data.fillheaddata.Length <= 0)
                                {
                                    pmm.HRME_Id = data.savetmpdata[j].HRME_Id;
                                    pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                    pmm.MI_Id = data.MI_Id;
                                   // pmm.FEC_Id = 1;
                                    pmm.ASMAY_Id = data.ASMAY_Id;
                                    pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                    pmm.FEC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                    pmm.FEC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                    pmm.FEC_ActiveFlag = true;
                                    pmm.CreatedDate = DateTime.Now;
                                    pmm.UpdatedDate = DateTime.Now;
                                    pmm.FMCC_Id = 1;
                                    _YearlyFeeGroupMappingContext.Add(pmm);
                                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else if (data.fillheaddata.Length == 1)
                                {
                                    var fetchfscid = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO
                                                      from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO
                                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FEC_Id == b.FECI_FEC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                      select new FeeConcessionDTO
                                                      {
                                                          FSC_Id = a.FEC_Id
                                                      }
              ).Distinct().SingleOrDefault();

                                    pmm.FEC_Id = fetchfscid.FSC_Id;
                                }

                                Fee_Employee_Concession_InstallmentsDMO pmmins = new Fee_Employee_Concession_InstallmentsDMO();
                                var resultt = (from a in _YearlyFeeGroupMappingContext.Fee_Employee_ConcessionDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO
                                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FEC_Id == b.FECI_FEC_Id && a.HRME_Id == data.savetmpdata[j].HRME_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id && b.FTI_Id == data.savetmpdata1[k].FTI_Id)
                                               select new FeeConcessionInstallmentsDMO
                                               {
                                                   FSCI_ID = b.FECI_FEC_Id
                                               }).SingleOrDefault();

                                if (resultt == null)
                                {

                                    pmmins.FECI_FEC_Id = pmm.FEC_Id;
                                    pmmins.FTI_Id = data.savetmpdata1[k].FTI_Id;
                                    pmmins.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    pmmins.CreatedDate = DateTime.Now;
                                    pmmins.UpdatedDate = DateTime.Now;

                                    _YearlyFeeGroupMappingContext.Add(pmmins);


                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.savetmpdata[j].HRME_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);


                                    if (status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSST_PaidAmount < 0)
                                    {
                                        status_stu.FSSST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_ToBePaid = status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSST_PaidAmount;
                                    }

                                    if (status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount > 0)
                                    {
                                        status_stu.FSSST_TotalCharges = status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_TotalCharges = 0;
                                    }


                                    //status_stu.FSSST_TotalCharges = status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;



                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges;
                                    }


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges;
                                    }


                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FSSST_ToBePaid = (status_stu.FSSST_ToBePaid + status_stu.FSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FSSST_ToBePaid = status_stu.FSSST_ToBePaid;
                                    //status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    //_YearlyFeeGroupMappingContext.Update(status_stu);

                                    //_YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                else
                                {
                                    var resultupdate = _YearlyFeeGroupMappingContext.Fee_Employee_Concession_InstallmentsDMO.Single(t => t.FECI_FEC_Id == resultt.FSCI_ID && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    resultupdate.FSCI_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    _YearlyFeeGroupMappingContext.Update(resultupdate);

                                    var status_stu = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRME_Id == data.savetmpdata[j].HRME_Id && t.FMG_Id == data.savetmpdata1[k].FMG_Id && t.FMH_Id == data.savetmpdata1[k].FMH_Id && t.FTI_Id == data.savetmpdata1[k].FTI_Id);

                                    if (status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSST_PaidAmount < 0)
                                    {
                                        status_stu.FSSST_ToBePaid = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_ToBePaid = status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount - status_stu.FSSST_PaidAmount;
                                    }

                                    status_stu.FSSST_TotalCharges = status_stu.FSSST_CurrentYrCharges - data.savetmpdata1[k].FSCI_ConcessionAmount;

                                    status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;


                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSST_ExcessPaidAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_ExcessPaidAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges;
                                    }

                                    if (data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges < 0)
                                    {
                                        status_stu.FSSST_RunningExcessAmount = 0;
                                    }
                                    else
                                    {
                                        status_stu.FSSST_RunningExcessAmount = data.savetmpdata1[k].FSCI_ConcessionAmount + status_stu.FSSST_PaidAmount - status_stu.FSSST_CurrentYrCharges;
                                    }

                                    _YearlyFeeGroupMappingContext.Update(status_stu);

                                    _YearlyFeeGroupMappingContext.SaveChanges();


                                    //status_stu.FSSST_ToBePaid = (status_stu.FSSST_ToBePaid + status_stu.FSSST_ConcessionAmount) - data.savetmpdata1[k].FSCI_ConcessionAmount;
                                    //status_stu.FSSST_ToBePaid = status_stu.FSSST_ToBePaid;
                                    //status_stu.FSSST_ConcessionAmount = data.savetmpdata1[k].FSCI_ConcessionAmount;

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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO selectcatorclass(FeeConcessionDTO data)
        {
            try
            {
                if(data.radiobtnvalue== "categorywise")
                {

                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.feeYCC
                                        from e in _YearlyFeeGroupMappingContext.feeYCCC
                                        from f in _YearlyFeeGroupMappingContext.Class_Category
                                        from d in _YearlyFeeGroupMappingContext.School_M_Class
                                        where (f.FMCC_Id==data.AMC_Id && c.FMCC_Id==f.FMCC_Id && c.FYCC_Id==e.FYCC_Id && a.AMST_Id==b.AMST_Id && b.ASMCL_Id==e.ASMCL_Id && a.MI_Id==data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id==d.ASMCL_Id && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag==1 && b.AMAY_ActiveFlag==1)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            ASMCL_ClassName = d.ASMCL_ClassName
                                        }
      ).Distinct().ToArray();

                }
                
                else if (data.radiobtnvalue == "Classwise")
                {
                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_M_Class
                                        where (c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && c.ASMCL_Id==b.ASMCL_Id && c.ASMCL_Id==data.ASMCL_Id && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag==1 && c.AMAY_ActiveFlag==1) 
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMAY_RollNo = c.AMAY_RollNo,
                                            ASMCL_ClassName = b.ASMCL_ClassName

                                        }
    ).Distinct().ToArray();
                }
                else if (data.radiobtnvalue == "leftstudent")
                {
                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_M_Class
                                        where (c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && c.ASMCL_Id == b.ASMCL_Id && c.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL.Equals("L") && a.AMST_ActiveFlag == 0 && c.AMAY_ActiveFlag == 1)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMAY_RollNo = c.AMAY_RollNo,
                                            ASMCL_ClassName = b.ASMCL_ClassName

                                        }
    ).Distinct().ToArray();
                }
                else if(data.radiobtnvalue.Equals("feecategorywise"))
                {
                    data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                        from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.School_M_Class
                                        from d in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                        where (c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && c.ASMCL_Id == b.ASMCL_Id && d.FMCC_Id==a.AMST_Concession_Type && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && c.AMAY_ActiveFlag == 1 && d.FMCC_Id==data.FMCC_Id)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMST_RegistrationNo = a.AMST_RegistrationNo,
                                            AMAY_RollNo = c.AMAY_RollNo,
                                            ASMCL_ClassName = b.ASMCL_ClassName
                                        }
    ).Distinct().ToArray();
                }
                 
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public FeeConcessionDTO getacademir(FeeConcessionDTO data)
        {
           try
            {
                data.studentdata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from e in _YearlyFeeGroupMappingContext.admissioncls
                                    from f in _YearlyFeeGroupMappingContext.feeGroup
                                    from g in _YearlyFeeGroupMappingContext.feehead
                                    from h in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                    where (d.ASMAY_Id == a.ASMAY_ID && a.FSC_Id == b.FSCI_FSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMST_Id == c.AMST_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                    select new FeeConcessionDTO
                                    {
                                        studentname = c.AMST_FirstName + '-' + c.AMST_MiddleName + '-' + c.AMST_LastName,
                                        ASMCL_ClassName = e.ASMCL_ClassName,
                                        FMG_GroupName = f.FMG_GroupName,
                                        FMH_FeeName = g.FMH_FeeName,
                                        FTI_Name = h.FTI_Name,
                                        FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                        FSC_Id = a.FSC_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSCI_ID = b.FSCI_ID,
                                    }
                             ).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO checkpaiddetails(FeeConcessionDTO data)
        {
            try
            {
                if(data.radiobtnvalue=="")
                data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from c in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from d in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                    from e in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                    from f in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    where (c.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && d.FMCC_Id == a.AMST_Concession_Type && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && c.AMAY_ActiveFlag == 1 && c.AMST_Id==e.AMST_Id && c.ASMAY_Id==e.ASMAY_Id && (e.FSS_PaidAmount>0 || e.FSS_ConcessionAmount>0) && e.FMG_Id==f.FMG_Id && f.FMG_CompulsoryFlag=="F" && d.FMCC_Id == data.FMCC_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = c.AMAY_RollNo,
                                    }
   ).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO searchfilter(FeeConcessionDTO data)
        {
            try
            {

                data.searchfilter = data.searchfilter.ToUpper();


                data.fillstudent = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_AdmNo.StartsWith(data.searchfilter) )
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_AdmNo,
                                        AMST_MiddleName = "",
                                        AMST_LastName = ""

                                    }).ToArray();

                data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.feeYCC
                                    from e in _YearlyFeeGroupMappingContext.feeYCCC
                                    from f in _YearlyFeeGroupMappingContext.Class_Category
                                    from d in _YearlyFeeGroupMappingContext.School_M_Class
                                    where (c.FMCC_Id == f.FMCC_Id && c.FYCC_Id == e.FYCC_Id && a.AMST_Id == b.AMST_Id && b.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id   && a.AMST_AdmNo.StartsWith(data.searchfilter))
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        ASMCL_ClassName = d.ASMCL_ClassName
                                    }
     ).Distinct().ToArray();


            }
            catch (Exception e)
            {

            }
            return data;
        }




    }
}


