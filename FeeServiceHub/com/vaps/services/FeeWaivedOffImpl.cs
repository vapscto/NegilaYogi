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
using Microsoft.SqlServer.Server;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services 
{
  
    public class FeeWaivedOffImpl : interfaces.FeeWaivedOffInterface
    {
       
        private static ConcurrentDictionary<string, FeeStudentWaiveOffDTO> _login =
        new ConcurrentDictionary<string, FeeStudentWaiveOffDTO>();

        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _context;
       // readonly ILogger<FeeAdjustmentImpl> _logger;
        public FeeWaivedOffImpl(FeeGroupContext FeeGroupContext, DomainModelMsSqlServerContext context)
        {
            _FeeGroupContext = FeeGroupContext;
            _context = context;
           // _logger = log;
        }

        public FeeStudentWaiveOffDTO getdata(FeeStudentWaiveOffDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.fillyear = year.Distinct().ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.Distinct().ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.fillsection = allsetion.Distinct().ToArray();

                //data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                //                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                //                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //                    select new FeeStudentWaiveOffDTO
                //                    {
                //                        AMST_RegistrationNo=a.AMST_RegistrationNo,
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName = a.AMST_FirstName,
                //                        AMST_MiddleName = a.AMST_MiddleName,
                //                        AMST_LastName = a.AMST_LastName 

                //                    }
                //                    ).Distinct().ToArray();

                var resulta = _FeeGroupContext.AdmissionStudentDMO.FirstOrDefault(b => b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id);
                data.AMST_Id = resulta.AMST_Id;

                //data.fillgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                //                  from b in _FeeGroupContext.FeeGroupDMO
                //                  where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_ActiceFlag == true && a.FSS_ToBePaid > 0 && a.FSS_WaivedAmount == 0)
                //                  select new FeeStudentWaiveOffDTO
                //                  {
                //                      FMG_Id = b.FMG_Id,
                //                      FMG_GroupName = b.FMG_GroupName,
                //                  }
                //                ).Distinct().ToArray();

                data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                 from b in _FeeGroupContext.feehead
                                 from d in _FeeGroupContext.AdmissionStudentDMO
                                 from f in _FeeGroupContext.feeMIY
                                 where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id &&  a.ASMAY_Id==data.ASMAY_Id && a.MI_Id==data.MI_Id)

                                 select new FeeStudentWaiveOffDTO
                                 {
                                     FSWO_Id = a.FSWO_Id,
                                     AMST_FirstName = d.AMST_FirstName,
                                     AMST_MiddleName = d.AMST_MiddleName,
                                     AMST_LastName = d.AMST_LastName,
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Name = f.FTI_Name,
                                     FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                     FSWO_Date = a.FSWO_Date,
                                    

                                 }).Distinct().OrderByDescending(t => t.FSWO_Id).ToList().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
       
        public FeeStudentWaiveOffDTO getdatastudentdet(FeeStudentWaiveOffDTO data)
        {
            try
            {
                if(data.ASMCL_Id != 0 && data.ASMS_Id == 0)
                {
                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id)
                                        select new FeeStudentWaiveOffDTO
                                        {
                                            AMST_RegistrationNo = a.AMST_AdmNo,
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName

                                        }).OrderBy(t=>t.AMST_FirstName).Distinct().ToArray();
                }

                else if (data.ASMCL_Id == 0 && data.ASMS_Id == 0)
                {
                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                        select new FeeStudentWaiveOffDTO
                                        {
                                            AMST_RegistrationNo = a.AMST_AdmNo,
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName

                                        }).OrderBy(t => t.AMST_FirstName).Distinct().ToArray();
                }

                else if (data.ASMCL_Id != 0 && data.ASMS_Id != 0)
                {
                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                        select new FeeStudentWaiveOffDTO
                                        {
                                            AMST_RegistrationNo = a.AMST_AdmNo,
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName

                                        }).OrderBy(t => t.AMST_FirstName).Distinct().ToArray();
                }

                data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                 from b in _FeeGroupContext.feehead
                                 from d in _FeeGroupContext.AdmissionStudentDMO
                                 from f in _FeeGroupContext.feeMIY
                                 where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.User_Id == data.userid)

                                 select new FeeStudentWaiveOffDTO
                                 {
                                     FSWO_Id = a.FSWO_Id,
                                     AMST_FirstName = d.AMST_FirstName,
                                     AMST_MiddleName = d.AMST_MiddleName,
                                     AMST_LastName = d.AMST_LastName,
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Name = f.FTI_Name,
                                     FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                     FSWO_Date = a.FSWO_Date,


                                 }).Distinct().OrderByDescending(t => t.FSWO_Id).ToList().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentWaiveOffDTO getdatagroupdet(FeeStudentWaiveOffDTO data)
        {
            try
            {


                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.fillgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                      from b in _FeeGroupContext.FeeGroupDMO
                                  from c in _FeeGroupContext.feehead
                                  where (a.FMG_Id == b.FMG_Id && c.FMH_Id==a.FMH_Id && c.FMH_RefundFlag==refundflag && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.FSS_WaivedAmount == 0)
                                      select new FeeStudentWaiveOffDTO
                                      {
                                          FMG_Id = b.FMG_Id,
                                          FMG_GroupName = b.FMG_GroupName,
                                      }
                              ).Distinct().ToArray();
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentWaiveOffDTO getdataheaddet(FeeStudentWaiveOffDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;

                if(data.finewaiveoff==0)
                {
                    data.fillhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     from b in _FeeGroupContext.feehead
                                     from c in _FeeGroupContext.feeMIY
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && data.multiplegroup.ToString().Contains(Convert.ToString(a.FMG_Id))  && b.FMH_RefundFlag == refundflag && a.FSS_WaivedAmount == 0)
                                     select new FeeStudentWaiveOffDTO
                                     {
                                         FMH_Id = b.FMH_Id,
                                         FMH_FeeName = b.FMH_FeeName,
                                         FTI_Id = c.FTI_Id,
                                         FTI_Name = c.FTI_Name,
                                         FMG_Id = a.FMG_Id,
                                         FMA_Id = a.FMA_Id,
                                         TotalTobepaid = a.FSS_TotalToBePaid,
                                         Tobepaid = a.FSS_ToBePaid,
                                         FSS_PaidAmount = a.FSS_PaidAmount
                                     }).Distinct().ToArray();
                }
                else if(data.finewaiveoff == 1)
                {
                    //data.fillhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                    //                 from b in _FeeGroupContext.feehead
                    //                 from c in _FeeGroupContext.feeMIY
                    //                 where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && data.multiplegroup.ToString().Contains(Convert.ToString(a.FMG_Id)) && a.User_Id == b.user_id && a.User_Id == data.userid && a.FSS_WaivedAmount == 0 && b.FMH_Flag=="F")
                    //                 select new FeeStudentWaiveOffDTO
                    //                 {
                    //                     FMH_Id = b.FMH_Id,
                    //                     FMH_FeeName = b.FMH_FeeName,
                    //                     FTI_Id = c.FTI_Id,
                    //                     FTI_Name = c.FTI_Name,
                    //                     FMG_Id = a.FMG_Id,
                    //                     FMA_Id = a.FMA_Id,
                    //                     TotalTobepaid = a.FSS_TotalToBePaid,
                    //                     Tobepaid = a.FSS_ToBePaid,
                    //                     FSS_PaidAmount = a.FSS_PaidAmount
                    //                 }).Distinct().ToArray();

                    var savedfillhead = (from a in _FeeGroupContext.feeStudentWaivedOff
                                     where (a.AMST_Id==data.AMST_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.FSWO_FullFineWaiveOffFlg==1)
                                     select new FeeStudentWaiveOffDTO
                                     {
                                         FMA_Id = a.FMA_Id,
                                     }).Select(t=>t.FMA_Id).Distinct().ToList();

                    data.fillhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     from b in _FeeGroupContext.feehead
                                     from c in _FeeGroupContext.feeMIY
                                     from d in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && data.multiplegroup.ToString().Contains(Convert.ToString(a.FMG_Id)) && d.FYGHM_FineApplicableFlag == "Y" && d.ASMAY_Id==a.ASMAY_Id && d.FMH_Id==a.FMH_Id && b.FMH_Flag!="F" && !savedfillhead.Contains(a.FMA_Id) && a.FSS_ToBePaid>0)
                                     select new FeeStudentWaiveOffDTO
                                     {
                                         FMH_Id = b.FMH_Id,
                                         FMH_FeeName = b.FMH_FeeName,
                                         FTI_Id = c.FTI_Id,
                                         FTI_Name = c.FTI_Name,
                                         FMG_Id = a.FMG_Id,
                                         FMA_Id = a.FMA_Id,
                                         TotalTobepaid = a.FSS_TotalToBePaid,
                                         Tobepaid = a.FSS_ToBePaid,
                                         FSS_PaidAmount = a.FSS_PaidAmount
                                     }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        
        public FeeStudentWaiveOffDTO savedatadelegate(FeeStudentWaiveOffDTO data)
        {
            try
            {
                FeeStudentWaivedOffDMO feepge = Mapper.Map<FeeStudentWaivedOffDMO>(data);
                if (feepge.FSWO_Id > 0)
                {
                    var lorg = _FeeGroupContext.feeStudentWaivedOff.Single(t => t.FSWO_Id == Convert.ToInt64(feepge.FSWO_Id));
                    
                        if (lorg != null)
                        {
                            var lorg1 = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.ASMAY_Id == lorg.ASMAY_Id && t.MI_Id == lorg.MI_Id);
                            if (lorg1.FSS_ExcessPaidAmount != 0)
                            {
                            var gethead = _FeeGroupContext.feehead.Single(t => t.FMH_Id == lorg1.FMH_Id);
                            if (gethead.FMH_RefundFlag == true)
                            {
                                if (lorg1.FSS_RefundableAmount < lorg1.FSS_ExcessPaidAmount)
                                {
                                    data.returnduplicatestatus = "adjusted";
                                }
                            }
                            else
                            {
                                if (lorg1.FSS_RunningExcessAmount < lorg1.FSS_ExcessPaidAmount)
                                {
                                    data.returnduplicatestatus = "adjusted";
                                }
                            }
                        }
                            if (data.returnduplicatestatus != "adjusted")
                            {
                            var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Waived_Off_Edit @p0, @p1,@p2,@p3,@p4,@p5,@p6,@p7", data.FSWO_Id, data.headlist[0].FSWO_WaivedOffAmount,data.FSWO_Date,data.finewaiveoff,data.FSWO_WaivedOffRemarks, data.completefinewaiveoff,data.filepath,data.filename);
                            if (contactExists > 0)
                            {

                                data.returnduplicatestatus = "Updated";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Updated";
                            }
                        }

                    }
                }
                else {
                    int contact =0;
                    foreach (var headlst in data.headlist)
                    {
                            var contactAdd = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Waived_Off_insert @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9,@p10,@p11,@p12,@p13,@p14", data.MI_Id, data.ASMAY_Id, data.AMST_Id, headlst.FMG_Id, headlst.FMH_Id, headlst.FTI_Id, headlst.FMA_Id, headlst.FSWO_WaivedOffAmount, data.FSWO_Date, data.userid,data.finewaiveoff, data.FSWO_WaivedOffRemarks,data.completefinewaiveoff, data.filepath, data.filename);
                            contact = contact + contactAdd;
                    }
                    if (contact >=1)
                    {
                        data.returnduplicatestatus = "Saved";
                    }                        
                    else
                        data.returnduplicatestatus = "not Saved";
                }
               
            }
            catch (Exception ee)
            {
                //   _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
       
        public FeeStudentWaiveOffDTO getpageedit(int id)
        {
            FeeStudentWaiveOffDTO data = new FeeStudentWaiveOffDTO();
            try
            {
                //List<FeeStudentWaivedOffDMO> alldata = new List<FeeStudentWaivedOffDMO>();
                //alldata = _FeeGroupContext.feeStudentWaivedOff.Where(t => t.FSWO_Id.Equals(id)).ToList();
                //data.filldata = alldata.ToArray();

                var result = _FeeGroupContext.feeStudentWaivedOff.Single(t => t.FSWO_Id == id);
                data.FSWO_Id = result.FSWO_Id;
                data.MI_Id = result.MI_Id;
                data.AMST_Id = result.AMST_Id;
                data.ASMAY_Id = result.ASMAY_Id;
                data.FSWO_Date = result.FSWO_Date;
                data.FMG_Id = result.FMG_Id;
                data.FMH_Id = result.FMH_Id;
                data.FTI_Id = result.FTI_Id;
                data.FMA_Id = result.FMA_Id;
                data.userid = result.User_Id;

                data.finewaiveoff = result.FSWO_FineFlg;

                data.completefinewaiveoff = result.FSWO_FullFineWaiveOffFlg;

                data.filepath = result.FSWO_WaivedOfffilepath;
                data.filename = result.FSWO_WaivedOfffilename;

                data.FSWO_WaivedOffAmount = result.FSWO_WaivedOffAmount;
                data.FSWO_WaivedOffRemarks = result.FSWO_WaivedOffRemarks;

                var result1 = _FeeGroupContext.School_Adm_Y_StudentDMO.FirstOrDefault(t => t.AMST_Id == data.AMST_Id && t.ASMAY_Id==data.ASMAY_Id);
                data.ASMCL_Id = result1.ASMCL_Id;
                data.ASMS_Id = result1.ASMS_Id;

                data.editamount = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                    where (a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.FMG_Id==data.FMG_Id && a.FMH_Id==data.FMH_Id && a.FTI_Id==data.FTI_Id && a.AMST_Id== data.AMST_Id && a.FSS_PaidAmount>0 && a.FSS_RunningExcessAmount==0)
                                    select new FeeStudentWaiveOffDTO
                                    {
                                        FSS_PaidAmount = a.FSS_PaidAmount,
                                        AMST_Id = a.AMST_Id,

                                    }).Distinct().ToArray();

                //List<School_M_Class> allclas = new List<School_M_Class>();
                //allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                //data.fillclass = allclas.Distinct().ToArray();

                //List<School_M_Section> allsetion = new List<School_M_Section>();
                //allsetion = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id).ToList();
                //data.fillsection = allsetion.Distinct().ToArray();

                data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMST_Id== data.AMST_Id)
                                    select new FeeStudentWaiveOffDTO
                                    {
                                        AMST_RegistrationNo = a.AMST_AdmNo,
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName

                                    }).Distinct().ToArray();

                data.fillgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                  from b in _FeeGroupContext.FeeGroupDMO
                                  where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.FMG_Id==data.FMG_Id  )
                                  select new FeeStudentWaiveOffDTO
                                  {
                                      FMG_Id = b.FMG_Id,
                                      FMG_GroupName = b.FMG_GroupName,
                                  }
                                                ).Distinct().ToArray();
                data.fillhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                 from b in _FeeGroupContext.feehead
                                 from c in _FeeGroupContext.feeMIY
                                 where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.FMG_Id == data.FMG_Id && a.FMH_Id == data.FMH_Id && a.FTI_Id == data.FTI_Id && a.FMA_Id == data.FMA_Id)
                                 select new FeeStudentWaiveOffDTO
                                 {
                                     FMH_Id = b.FMH_Id,
                                     FMH_FeeName = b.FMH_FeeName,
                                     FTI_Id = c.FTI_Id,
                                     FTI_Name = c.FTI_Name,
                                     FMG_Id = a.FMG_Id,
                                     FMA_Id = a.FMA_Id,
                                     TotalTobepaid = a.FSS_TotalToBePaid,
                                     Tobepaid = a.FSS_ToBePaid+(a.FSS_WaivedAmount-a.FSS_ExcessPaidAmount),
                                     FSS_PaidAmount = a.FSS_PaidAmount

                                 }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                //_logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
      
        public FeeStudentWaiveOffDTO deleterec(int id)
        {
            FeeStudentWaiveOffDTO page = new FeeStudentWaiveOffDTO();
           // List<FeeStudentWaivedOffDMO> lorg = new List<FeeStudentWaivedOffDMO>();
            var lorg = _FeeGroupContext.feeStudentWaivedOff.Single(t => t.FSWO_Id==Convert.ToInt64(id));
            try
            {
                if (lorg != null)
                {
                    if(lorg.FSWO_FineFlg==1)
                    {
                        var lorg1 = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.ASMAY_Id == lorg.ASMAY_Id && t.MI_Id == lorg.MI_Id && t.User_Id == lorg.User_Id && t.FMA_Id==lorg.FMA_Id);
                        if(lorg1.FSS_PaidAmount==0)
                        {
                            if (lorg1.FSS_ExcessPaidAmount != 0)
                            {
                                var gethead = _FeeGroupContext.feehead.Single(t => t.FMH_Id == lorg1.FMH_Id);
                                if (gethead.FMH_RefundFlag == true)
                                {
                                    if (lorg1.FSS_RefundableAmount < lorg1.FSS_ExcessPaidAmount)
                                    {
                                        page.returnduplicatestatus = "adjusted";
                                    }
                                }
                                else
                                {
                                    if (lorg1.FSS_RunningExcessAmount < lorg1.FSS_ExcessPaidAmount)
                                    {
                                        page.returnduplicatestatus = "adjusted";
                                    }
                                }

                            }
                        }
                        else
                        {
                            page.returnduplicatestatus = "adjusted";
                        }
                     
                        if (page.returnduplicatestatus != "adjusted")
                        {
                            var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Waived_Off_Delete_online @p0", id);
                            if (contactExists > 0)
                            {

                                page.returnduplicatestatus = "success";
                            }
                            else
                            {
                                page.returnduplicatestatus = "fail";
                            }
                        }
                    }
                    else if (lorg.FSWO_FineFlg == 0)
                    {
                        var lorg1 = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.ASMAY_Id == lorg.ASMAY_Id && t.MI_Id == lorg.MI_Id && t.User_Id == lorg.User_Id && t.FMA_Id==lorg.FMA_Id);
                        if (lorg1.FSS_ExcessPaidAmount != 0)
                        {
                            var gethead = _FeeGroupContext.feehead.Single(t => t.FMH_Id == lorg1.FMH_Id);
                            if (gethead.FMH_RefundFlag == true)
                            {
                                if (lorg1.FSS_RefundableAmount < lorg1.FSS_ExcessPaidAmount)
                                {
                                    page.returnduplicatestatus = "adjusted";
                                }
                            }
                            else
                            {
                                if (lorg1.FSS_RunningExcessAmount < lorg1.FSS_ExcessPaidAmount)
                                {
                                    page.returnduplicatestatus = "adjusted";
                                }
                            }

                        }
                        if (page.returnduplicatestatus != "adjusted")
                        {
                            var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Waived_Off_Delete @p0", id);
                            if (contactExists > 0)
                            {

                                page.returnduplicatestatus = "success";
                            }
                            else
                            {
                                page.returnduplicatestatus = "fail";
                            }
                        }
                    }
                   
                    
                }                
            }
            catch (Exception ee)
            {
                // _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return page;
        }
        
        public FeeStudentWaiveOffDTO searching(FeeStudentWaiveOffDTO data)
        {

            try
            {

                switch (data.searchType)
                {
                    case "0":
                        string str = "";

                        List<FeeStudentWaiveOffDTO> result = new List<FeeStudentWaiveOffDTO>();
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FEE_WAIVEDOFF_NAME_SEARCH";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@searchtext",
                                         SqlDbType.VarChar)
                            {
                                Value = data.searchtext
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@USERID",
                           SqlDbType.BigInt)
                            {
                                Value = data.userid
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
                                        result.Add(new FeeStudentWaiveOffDTO
                                        {

                                            FSWO_Id = Convert.ToInt64(dataReader["FSWO_Id"].ToString()),
                                            AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                            AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                                            AMST_LastName = dataReader["AMST_LastName"].ToString(),
                                            FMH_FeeName = dataReader["FMH_FeeName"].ToString(),
                                            FTI_Name = dataReader["FTI_Name"].ToString(),
                                            FSWO_WaivedOffAmount = Convert.ToInt64(dataReader["FSWO_WaivedOffAmount"].ToString()),
                                            FSWO_Date = Convert.ToDateTime(dataReader["FSWO_Date"].ToString()),
                                           
                                        });
                                    }
                                }
                                data.filldata = result.ToArray();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        //data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                        //                 from b in _FeeGroupContext.feehead
                        //                 from d in _FeeGroupContext.AdmissionStudentDMO
                        //                 from f in _FeeGroupContext.feeMIY
                        //                 where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (((d.AMST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(d.AMST_MiddleName.Trim()) == true ? str : d.AMST_MiddleName.Trim())).Trim() + ' ' + (string.IsNullOrEmpty(d.AMST_LastName.Trim()) == true ? str : d.AMST_LastName.Trim())).Trim().Contains(data.searchtext) || d.AMST_FirstName.StartsWith(data.searchtext) || d.AMST_MiddleName.StartsWith(data.searchtext) || d.AMST_LastName.StartsWith(data.searchtext)) && a.User_Id == data.userid)

                        //                 select new FeeStudentWaiveOffDTO
                        //                 {
                        //                     FSWO_Id = a.FSWO_Id,
                        //                     AMST_FirstName = d.AMST_FirstName,
                        //                     AMST_MiddleName = d.AMST_MiddleName,
                        //                     AMST_LastName = d.AMST_LastName,
                        //                     FMH_FeeName = b.FMH_FeeName,
                        //                     FTI_Name = f.FTI_Name,
                        //                     FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                        //                     FSWO_Date = a.FSWO_Date,

                        //                 }).Distinct().OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();

                        break;
                    case "1":
                        data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                         from b in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_FeeName.Contains(data.searchtext) )

                                         select new FeeStudentWaiveOffDTO
                                         {
                                             FSWO_Id = a.FSWO_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeName = b.FMH_FeeName,
                                             FTI_Name = f.FTI_Name,
                                             FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                             FSWO_Date = a.FSWO_Date,

                                         }).Distinct().OrderByDescending(t => t.FMH_FeeName).ToList().ToArray();
                        break;
                    case "2":
                        data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                         from b in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.FTI_Name.Contains(data.searchtext) )

                                         select new FeeStudentWaiveOffDTO
                                         {
                                             FSWO_Id = a.FSWO_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeName = b.FMH_FeeName,
                                             FTI_Name = f.FTI_Name,
                                             FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                             FSWO_Date = a.FSWO_Date,

                                         }).Distinct().OrderByDescending(t => t.FTI_Name).ToList().ToArray();
                        break;
                    case "3":
                        data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                         from b in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FSWO_WaivedOffAmount.ToString().Contains(data.searchnumber) )

                                         select new FeeStudentWaiveOffDTO
                                         {
                                             FSWO_Id = a.FSWO_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeName = b.FMH_FeeName,
                                             FTI_Name = f.FTI_Name,
                                             FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                             FSWO_Date = a.FSWO_Date,

                                         }).Distinct().OrderByDescending(t => t.FSWO_WaivedOffAmount).ToList().ToArray();
                        break;
                    case "4":
                        //var str = Convert.ToDateTime(data.searchtext).ToString("yyyy-MM-dd");
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");
                        data.filldata = (from a in _FeeGroupContext.feeStudentWaivedOff
                                         from b in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FMH_Id == b.FMH_Id && a.AMST_Id == d.AMST_Id && a.FTI_Id == f.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id &&  a.FSWO_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") )
                                         select new FeeStudentWaiveOffDTO
                                         {
                                             FSWO_Id = a.FSWO_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeName = b.FMH_FeeName,
                                             FTI_Name = f.FTI_Name,
                                             FSWO_WaivedOffAmount = a.FSWO_WaivedOffAmount,
                                             FSWO_Date = a.FSWO_Date,
                                         }).Distinct().OrderByDescending(t => t.FSWO_Date).ToList().ToArray();

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


