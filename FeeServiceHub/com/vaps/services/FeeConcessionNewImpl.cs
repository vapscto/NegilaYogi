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
    public class FeeConcessionNewImpl : interfaces.FeeConcessionNewInterface
    {
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public FeeConcessionNewImpl(FeeGroupContext YearlyFeeGroupMappingContext)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
        }

        public FeeConcessionDTO deleteconcess(FeeConcessionDTO data)
        {
            try
            {
                //if (data.radiobtnvalue != "Staff" && data.radiobtnvalue != "Others")
                //{
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
                //}


            }
            catch (Exception ex)
            {
                data.returnval = "false";
                Console.WriteLine(ex.Message);
            }

            return data;
        }

    

   

        public FeeConcessionDTO getdata(FeeConcessionDTO data)

        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

                List<FeeClassCategoryDMO> category = new List<FeeClassCategoryDMO>();
                category = _YearlyFeeGroupMappingContext.FeeClassCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.fillclass = category.ToArray();

              


                data.fillgroup = (from a in _YearlyFeeGroupMappingContext.feeGroup
                                  from b in _YearlyFeeGroupMappingContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                                  select new FeeConcessionDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
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
                                    where (d.ASMAY_Id == a.ASMAY_ID && a.FSC_Id == b.FSCI_FSC_Id && a.FMG_Id == f.FMG_Id && a.FMH_Id == g.FMH_Id && b.FTI_Id == h.FTI_Id && a.AMST_Id == c.AMST_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && f.user_id == data.userid)
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

                                    }
                              ).Distinct().ToArray();

               
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }



   


        public FeeConcessionDTO getfeegroup(FeeConcessionDTO data)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO getfeehead(FeeConcessionDTO data)
        {
            try
            {


                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                     where (a.FMG_Id == c.FMG_Id && a.FMH_Id == b.FMH_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMG_Id == data.FMG_Id)
                                     select new FeeConcessionDTO
                                     {
                                         FMH_Id = b.FMH_Id,
                                         FMH_FeeName = b.FMH_FeeName
                                     }
                                   ).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO getterm(FeeConcessionDTO data)
        {
            try
            {
                data.fillterm = (from a in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                                 from b in _YearlyFeeGroupMappingContext.feeTr
                                 where (a.FMT_Id == b.FMT_Id && a.MI_Id == b.MI_Id && a.FMH_Id == data.FMH_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.FMT_ActiveFlag == true)
                                 select new FeeConcessionDTO
                                 {
                                     FMT_ID = b.FMT_Id,
                                     FMT_Name = b.FMT_Name
                                 }
                                 ).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeConcessionDTO getinstallment(FeeConcessionDTO data)
        {
            try
            {

                data.filinstallment = (from a in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                       from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       where (a.FTI_Id==b.FTI_Id && b.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id && b.FMG_Id==data.FMG_Id && b.FMH_Id==data.FMH_Id && b.FMCC_Id==data.FMCC_Id)
                                       select new FeeConcessionDTO
                                       {
                                           FTI_Id = a.FTI_Id,
                                           FTI_Name = a.FTI_Name
                                       }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeConcessionDTO studentdata(FeeConcessionDTO data)
        {
            try
            {


                data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.feeYCC
                                    from e in _YearlyFeeGroupMappingContext.feeYCCC
                                    from f in _YearlyFeeGroupMappingContext.Class_Category
                                    from d in _YearlyFeeGroupMappingContext.School_M_Class
                                    from g in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from h in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                    from i in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                    from j in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                                    where (c.FMCC_Id == f.FMCC_Id && c.FYCC_Id == e.FYCC_Id && a.AMST_Id == b.AMST_Id &&
                                    b.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id &&
                                    a.AMST_Id == i.AMST_Id && a.ASMAY_Id == i.ASMAY_Id && a.MI_Id == i.MI_Id && g.FMG_Id == i.FMG_Id && h.FMH_ID == i.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id  && i.FSS_ToBePaid > 0 && i.FSS_PaidAmount == 0 && i.FSS_ConcessionAmount == 0 &&
                                    a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && c.FMCC_Id==data.FMCC_Id && h.FMH_ID==j.FMH_Id && j.FMH_Id==data.FMH_Id && j.FMT_Id==data.FMT_ID && j.FMH_Id==h.FMH_ID && j.FTI_Id==i.FTI_Id
                                    && f.FMCC_Id == data.FMCC_Id && b.ASMAY_Id == g.ASMAY_Id && a.AMST_Id == g.AMST_Id && a.MI_Id == g.MI_Id && g.FMSG_Id == h.FMSG_Id && g.FMG_Id == data.FMG_Id && h.FMH_ID == data.FMH_Id )
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        ASMCL_ClassName = d.ASMCL_ClassName,
                                        FMG_Id = g.FMG_Id,
                                        FTI_Id = i.FTI_Id,
                                        FMH_Id = h.FMH_ID,
                                        FSS_Id = i.FSS_Id
                                    }
      ).Distinct().ToArray();

                //var amount = _YearlyFeeGroupMappingContext.FeeAmountEntryDMO.Where(a => a.FMCC_Id == data.FMCC_Id && a.FMH_Id == data.FMH_Id && a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id  && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault();

                var amount = (from a in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                              from b in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                              where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.FMCC_Id == data.FMCC_Id && a.FMH_Id == data.FMH_Id && a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMT_Id == data.FMT_ID)
                              select a).FirstOrDefault();
                data.FMA_Amount = Convert.ToInt64(amount.FMA_Amount);


                var fmc = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(b => b.MI_Id == data.MI_Id).FirstOrDefault();
                data.FMC_Id = fmc.FMC_Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeConcessionDTO studentdata1(FeeConcessionDTO data)
        {
            try
            {



                data.studentdata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.feeYCC
                                    from e in _YearlyFeeGroupMappingContext.feeYCCC
                                    from f in _YearlyFeeGroupMappingContext.Class_Category
                                    from d in _YearlyFeeGroupMappingContext.School_M_Class
                                    from g in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from h in _YearlyFeeGroupMappingContext.FeeStudentGroupInstallmentMappingDMO
                                    from i in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                    where (c.FMCC_Id == f.FMCC_Id && c.FYCC_Id == e.FYCC_Id && a.AMST_Id == b.AMST_Id &&
                                   b.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id &&
                                    a.AMST_Id == i.AMST_Id && a.ASMAY_Id == i.ASMAY_Id && a.MI_Id == i.MI_Id && g.FMG_Id == i.FMG_Id && h.FMH_ID == i.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && i.FTI_Id == data.FTI_Id && i.FSS_ToBePaid > 0 && i.FSS_PaidAmount == 0 && i.FSS_ConcessionAmount ==0 &&
                                    a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && f.FMCC_Id == data.FMCC_Id
                                   && b.ASMAY_Id == g.ASMAY_Id && a.AMST_Id == g.AMST_Id && a.MI_Id == g.MI_Id && g.FMSG_Id == h.FMSG_Id && g.FMG_Id == data.FMG_Id && h.FTI_ID == data.FTI_Id && h.FMH_ID == data.FMH_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        ASMCL_ClassName = d.ASMCL_ClassName,
                                        FMG_Id = g.FMG_Id,
                                        FTI_Id = h.FTI_ID,
                                        FMH_Id = h.FMH_ID,
                                        FSS_Id = i.FSS_Id
                                    }
      ).Distinct().ToArray();

                var amount = _YearlyFeeGroupMappingContext.FeeAmountEntryDMO.Where(a => a.FMCC_Id == data.FMCC_Id && a.FMH_Id == data.FMH_Id && a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id && a.FTI_Id == data.FTI_Id && a.ASMAY_Id == data.ASMAY_Id).FirstOrDefault();
                data.FMA_Amount = Convert.ToInt64(amount.FMA_Amount);


                var fmc = _YearlyFeeGroupMappingContext.FeeMasterConfiguration.Where(b => b.MI_Id == data.MI_Id).FirstOrDefault();
                data.FMC_Id = fmc.FMC_Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeConcessionDTO Save(FeeConcessionDTO data)
        {
            var contactExists = 0;

            try
            {
                if (data.savetmpdata != null)
                {
                    for (int i = 0; i < data.savetmpdata.Length; i++)
                    {
                        using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            FeeConcessionDMO pmm = new FeeConcessionDMO();
                            if (data.FMG_Id != 0)
                            {
                                pmm.AMST_Id = data.savetmpdata[i].AMST_Id;
                                pmm.FMG_Id = data.FMG_Id;
                                pmm.MI_Id = data.MI_Id;
                                pmm.FMC_Id = data.savetmpdata[i].FMC_Id;
                                pmm.ASMAY_ID = data.ASMAY_Id;
                                pmm.FMH_Id = data.FMH_Id;
                                //pmm.FMG_Id = data.FMG_Id;

                                pmm.FMSG_ActiveFlag = "1";
                                pmm.CreatedDate = DateTime.Now;
                                pmm.UpdatedDate = DateTime.Now;
                                pmm.FSC_CreatedBy = data.userid;
                                pmm.FSC_UpdatedBy = data.userid;


                                data.fillheaddata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                     from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                     where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[i].AMST_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && b.FTI_Id==data.savetmpdata[i].FTI_Id)
                                                     select new FeeConcessionDTO
                                                     {
                                                         FSC_Id = a.FSC_Id
                                                     }
                ).Distinct().ToArray();

                                if (data.fillheaddata.Length <= 0)
                                {
                                    _YearlyFeeGroupMappingContext.Add(pmm);
                                     contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                }
                                FeeConcessionInstallmentsDMO pmmins = new FeeConcessionInstallmentsDMO();
                                if (data.savetmpdata[i].FTI_Id != 0)
                                {
                                    if (data.fillheaddata.Length <= 0)
                                    {
                                        pmmins.FSCI_FSC_Id = pmm.FSC_Id;
                                        pmmins.FTI_Id = data.savetmpdata[i].FTI_Id;
                                        pmmins.FSCI_ConcessionAmount = data.FSCI_ConcessionAmount;
                                        pmmins.CreatedDate = DateTime.Now;
                                        pmmins.UpdatedDate = DateTime.Now;
                                        pmmins.FSCI_CreatedBy = data.userid;
                                        pmmins.FSCI_UpdatedBy = data.userid;

                                        _YearlyFeeGroupMappingContext.Add(pmmins);
                                         contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                    }
                                    else
                                    {
                                        var resultt = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                       from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                       where (a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[i].AMST_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && b.FTI_Id == data.savetmpdata[i].FTI_Id)
                                                       select new FeeConcessionInstallmentsDMO
                                                       {
                                                           FSCI_FSC_Id = a.FSC_Id
                                                       });

                                        if (resultt.Count() > 0)
                                        {
                                            pmmins.FSCI_ConcessionAmount = data.FSCI_ConcessionAmount;


                                            _YearlyFeeGroupMappingContext.Update(pmmins);
                                            _YearlyFeeGroupMappingContext.SaveChanges();
                                        }


                                    }
                                }


                                var resultfeest = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(a => a.FSS_Id == data.savetmpdata[i].FSS_Id).ToList();
                                if (resultfeest.Count > 0)
                                {
                                    var updatequery = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Where(a => a.FSS_Id == data.savetmpdata[i].FSS_Id).FirstOrDefault();
                                    updatequery.FSS_ConcessionAmount = data.FSCI_ConcessionAmount;
                                    updatequery.FSS_ToBePaid= Convert.ToInt64(updatequery.FSS_NetAmount) - data.FSCI_ConcessionAmount;
                                    //updatequery.FSS_UpdatedBy = data.userid;
                                    //updatequery.FSS_UpdatedDate= DateTime.Now;

                                    _YearlyFeeGroupMappingContext.Update(updatequery);


                                }

                            }
                           var ResultCount= _YearlyFeeGroupMappingContext.SaveChanges();
                            if (ResultCount >= 1 && contactExists>=1 )
                            {
                                data.message = "true";
                            }
                            else
                            {
                                data.message = "false";
                            }
                            transaction.Commit();
                        }
                    }
                }
                data.studentdata = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                     from d in _YearlyFeeGroupMappingContext.feehead
                                     from e in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     where (a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[0].AMST_Id && b.FTI_Id == c.FTI_Id && d.FMH_Id == a.FMH_Id && a.AMST_Id == e.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                     select new FeeConcessionDTO
                                     {
                                         FSCI_ConcessionAmount = b.FSCI_ConcessionAmount,
                                         FMH_Id = a.FMH_Id,
                                         FMG_Id = a.FMG_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSC_ConcessionType = a.FSC_ConcessionType,
                                         FSC_ConcessionReason = a.FSC_ConcessionReason,
                                         FMH_FeeName = d.FMH_FeeName,
                                         FTI_Name = c.FTI_Name,
                                         FMA_Amount = Convert.ToInt64(e.FSS_NetAmount)
                                     }
                ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                List<FeeClassCategoryDMO> category = new List<FeeClassCategoryDMO>();
                category = _YearlyFeeGroupMappingContext.FeeClassCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
                data.fillclass = category.ToArray();
             //   List<School_M_Class> allclas = new List<School_M_Class>();
             //   allclas = _YearlyFeeGroupMappingContext.feeCC.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).ToList();
             ////   allclas = _YearlyFeeGroupMappingContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
             //   data.fillclass = allclas.ToArray();

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
                                       where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && c.AMST_Id == d.AMST_Id && c.ASMAY_Id == d.ASMAY_ID && c.FMH_Id == d.FMH_Id && c.FMG_Id == d.FMG_Id && d.FSC_Id == e.FSCI_FSC_Id && c.FTI_Id == e.FTI_Id && c.FMH_Id == f.FMH_Id && c.FTI_Id == g.FTI_Id && d.FMH_Id == h.FMH_Id && h.FTI_Id == e.FTI_Id && h.FMT_Id == i.FMT_Id && b.ASMCL_Id == j.ASMCL_Id && a.MI_Id == data.MI_Id && e.FSCI_ID == data.FSCI_ID)
                                       select new FeeStudentTransactionDTO
                                       {
                                           ASMAY_Id = c.ASMAY_Id,
                                           ASMCL_ID = b.ASMCL_Id,
                                           classname = j.ASMCL_ClassName,
                                           termname = i.FMT_Name,
                                           FMT_Id = i.FMT_Id,
                                           AMST_RegistrationNo = a.AMST_RegistrationNo,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_FirstName = a.AMST_FirstName,
                                           FMH_Id = f.FMH_Id,
                                           FMG_Id = c.FMG_Id,
                                           FTI_Id = c.FTI_Id,
                                           FMA_Id = c.FMA_Id,
                                           FSCI_ID = e.FSCI_ID,
                                           FSC_Id = d.FSC_Id,
                                           FMH_FeeName = f.FMH_FeeName,
                                           FTI_Name = g.FTI_Name,
                                           FMA_Amount = c.FSS_TotalToBePaid,
                                           FSCI_ConcessionAmount = c.FSS_ConcessionAmount,
                                           FSC_ConcessionType = d.FSC_ConcessionType,
                                           FSC_ConcessionReason = d.FSC_ConcessionReason,
                                           Amst_Id = a.AMST_Id
                                       }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeConcessionDTO savedatadelegate(FeeConcessionDTO data)
        {
            try
            {
          
                    if (data.FSCI_ID > 0 && data.FSC_Id > 0)
                    {
                        using (var transactionupdate = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            var result = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Single(t => t.FSC_Id == data.FSC_Id);

                            var resultinstallment = _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO.Single(t => t.FSCI_ID == data.FSCI_ID);

                            var status_stu = _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.savetmpdata[0].AMST_Id && t.FMG_Id == data.savetmpdata1[0].FMG_Id && t.FMH_Id == data.savetmpdata1[0].FMH_Id && t.FTI_Id == data.savetmpdata1[0].FTI_Id);

                            result.FSC_ConcessionReason = data.savetmpdata1[0].FSC_ConcessionReason;

                            _YearlyFeeGroupMappingContext.Update(result);

                            resultinstallment.FSCI_ConcessionAmount = data.savetmpdata1[0].FSCI_ConcessionAmount;

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
                                            pmm.AMST_Id = data.savetmpdata[j].AMST_Id;
                                          
                                            pmm.FMG_Id = data.savetmpdata1[k].FMG_Id;
                                            pmm.MI_Id = data.MI_Id;
                                            pmm.FMC_Id = 1;
                                            pmm.ASMAY_ID = data.ASMAY_Id;
                                            pmm.FMH_Id = data.savetmpdata1[k].FMH_Id;
                                            pmm.FSC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                            pmm.FSC_ConcessionType = data.savetmpdata1[k].FSC_ConcessionType;
                                            pmm.FMSG_ActiveFlag = "1";
                                            pmm.CreatedDate = DateTime.Now;
                                            pmm.UpdatedDate = DateTime.Now;
                                        pmm.FSC_UpdatedBy = data.userid;
                                            _YearlyFeeGroupMappingContext.Add(pmm);
                                            var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                                        }
                                        else if (data.fillheaddata.Length == 1)
                                        {
                                            var fetchfscid = (from a in _YearlyFeeGroupMappingContext.FeeConcessionDMO
                                                              from b in _YearlyFeeGroupMappingContext.FeeConcessionInstallmentsDMO
                                                              where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FSC_Id == b.FSCI_FSC_Id && a.AMST_Id == data.savetmpdata[j].AMST_Id && a.FMG_Id == data.savetmpdata1[k].FMG_Id && a.FMH_Id == data.savetmpdata1[k].FMH_Id)
                                                              select new FeeConcessionDTO
                                                              {
                                                                  FSC_Id = a.FSC_Id
                                                              }
                      ).Distinct().SingleOrDefault();

                                            var result1 = _YearlyFeeGroupMappingContext.FeeConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.FSC_Id == fetchfscid.FSC_Id).SingleOrDefault();
                                            result1.FSC_ConcessionReason = data.savetmpdata1[k].FSC_ConcessionReason;
                                            _YearlyFeeGroupMappingContext.Update(result1);
                                            _YearlyFeeGroupMappingContext.SaveChanges();

                                            pmm.FSC_Id = fetchfscid.FSC_Id;

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

                                        //status_stu.FSS_UpdatedBy = data.userid;
                                        //status_stu.FSS_UpdatedDate = DateTime.Now;

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

                                            //_YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("update_concession_status @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.MI_Id, data.ASMAY_ID, data.savetmpdata[j].AMST_Id, data.savetmpdata1[k].FMG_Id, data.savetmpdata1[k].FMH_Id, data.savetmpdata1[k].FTI_Id, data.savetmpdata1[k].FSCI_ConcessionAmount);
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
            catch (Exception e)
            {
                data.returnval = "false";
                Console.WriteLine(e.Message);
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
                                 where (a.FMA_Id == b.FMA_Id && b.FYP_Id == c.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
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
               
                    //if (data.radiobtnvalue != "Staff" && data.radiobtnvalue != "Others")
                    //{
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
