using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.Fees;
using AutoMapper;
using DomainModel.Model.com.vaps.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeCardDetailsEntryImpl : interfaces.FeeCardDetailsEntryInterface
    {
        private static ConcurrentDictionary<string, FeeCardDetailEntryDTO> _login =
        new ConcurrentDictionary<string, FeeCardDetailEntryDTO>();

        private static readonly Object obj = new Object();

        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _context;
     
        public FeeCardDetailsEntryImpl(FeeGroupContext FeeGroupContext, DomainModelMsSqlServerContext context)
        {
            _FeeGroupContext = FeeGroupContext;
            _context = context;
          
        }
        public FeeCardDetailEntryDTO getdata(FeeCardDetailEntryDTO data)
        {
            List<MasterAcademic> year = new List<MasterAcademic>();
            year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).ToList();
            data.fillyear = year.Distinct().ToArray();
            data.fillgrid = (from a in _FeeGroupContext.AdmissionStudentDMO
                             from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                             from c in _FeeGroupContext.feeCardDetailsEntry
                             from d in _FeeGroupContext.FeeGroupDMO
                             from e in _FeeGroupContext.FeeHeadDMO
                             from f in _FeeGroupContext.feeMIY
                             from g in _FeeGroupContext.AcademicYear
                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && g.MI_Id == data.MI_Id && g.ASMAY_Id == c.ASMAY_Id && g.ASMAY_ActiveFlag == 1 && f.MI_ID == data.MI_Id && f.FTI_Id == c.FTI_Id && e.MI_Id == data.MI_Id && e.FMH_Id == c.FMH_Id && e.FMH_ActiveFlag == true && d.MI_Id == data.MI_Id && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S")
                             select new FeeCardDetailEntryDTO
                             {
                                 //AMST_RegistrationNo = a.AMST_RegistrationNo,
                                 Amst_Id = a.AMST_Id,
                                 AMST_FirstName = a.AMST_FirstName,
                                 AMST_MiddleName = a.AMST_MiddleName,
                                 AMST_LastName = a.AMST_LastName,
                                 ASMAY_Year = g.ASMAY_Year,
                                 FMG_Id = c.FMG_Id,
                                 FMH_Id = c.FMH_Id,
                                 FTI_Id = c.FTI_Id,
                                 FMG_GroupName = d.FMG_GroupName,
                                 FMH_FeeName = e.FMH_FeeName,
                                 FTI_Name = f.FTI_Name,
                                 FSFM_Amount = c.FSFM_Amount,
                                 FSFM_PaidAmount = c.FSFM_PaidAmount,
                                 FSFM_Id = c.FSFM_Id                               

                             }).Distinct().OrderByDescending(t=>t.FSFM_Id).ToArray();



            return data;
        }
        public FeeCardDetailEntryDTO getsearchfilter(FeeCardDetailEntryDTO data)
        {
            try
            {
                data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && ((a.AMST_FirstName.Trim() + ' ' + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || (a.AMST_FirstName.Trim() + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).StartsWith(data.searchfilter) || a.AMST_FirstName.StartsWith(data.searchfilter) || a.AMST_MiddleName.StartsWith(data.searchfilter) || a.AMST_LastName.StartsWith(data.searchfilter)))
                                    select new FeeCardDetailEntryDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                        //AMST_FirstName = a.AMST_FirstName + a.AMST_MiddleName == null ? " " : a.AMST_MiddleName + " " + a.AMST_LastName == null ? " " : a.AMST_LastName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }).ToArray();



            }
            catch (Exception e)
            {

            }
            return data;
        }
        public FeeCardDetailEntryDTO getstudlistgroup(FeeCardDetailEntryDTO data)
        {
            try
            {
                data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupDMO
                                        from b in _FeeGroupContext.FeeStudentGroupMappingDMO
                                        from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        where (a.FMG_Id == b.FMG_Id && b.AMST_Id == data.Amst_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                        select new FeeCardDetailEntryDTO
                                        {
                                            FMG_GroupName = a.FMG_GroupName,
                                            FMG_Id = a.FMG_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeCardDetailEntryDTO getgroupmappedheads(FeeCardDetailEntryDTO data)
        {
            try
            {
                data.alldata = (from a in _FeeGroupContext.FeeHeadDMO
                                from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                where (a.FMH_Id ==b.FMH_Id && b.FMG_Id==data.FMG_Id && a.MI_Id==data.MI_Id && a.FMH_ActiveFlag==true && b.MI_Id==data.MI_Id && b.ASMAY_Id==data.ASMAY_Id) 
                                select new FeeCardDetailEntryDTO
                                {
                                    FMH_FeeName = a.FMH_FeeName,
                                    FMH_Id = a.FMH_Id,                                    
                                }).Distinct().OrderBy(t => t.FMH_Id).ToArray();

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                // data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public FeeCardDetailEntryDTO savedata(FeeCardDetailEntryDTO data)
        {
            FeeCardDetailsEntryDMO feepge = Mapper.Map<FeeCardDetailsEntryDMO>(data);
            feepge.FTI_Id= 27;
            try
            {
                if (feepge.FSFM_Id > 0)
                {
                    var result1 = _FeeGroupContext.feeCardDetailsEntry.Where(t => t.AMST_Id== feepge.AMST_Id && t.ASMAY_Id==feepge.ASMAY_Id && t.FMG_Id==feepge.FMG_Id && t.FMH_Id==feepge.FMH_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _FeeGroupContext.feeCardDetailsEntry.Single(t => t.FSFM_Id == feepge.FSFM_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.AMST_Id = feepge.AMST_Id;
                        result.ASMAY_Id = feepge.ASMAY_Id;
                        result.FMH_Id = feepge.FMH_Id;
                        result.FMG_Id = feepge.FMG_Id;
                        result.FTI_Id = feepge.FTI_Id;
                        result.FSFM_Amount= feepge.FSFM_Amount;                       
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Update(result);
                        var lorg = _FeeGroupContext.FeeStudentTransactionDMO.SingleOrDefault(t => t.AMST_Id == feepge.AMST_Id && t.FMG_Id == feepge.FMG_Id && t.FMH_Id == feepge.FMH_Id && t.FTI_Id == feepge.FTI_Id && t.MI_Id == feepge.MI_Id && t.ASMAY_Id == feepge.ASMAY_Id);
                        if (lorg != null)
                        {
                            var updateStudentStatus = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == lorg.FSS_Id);
                            updateStudentStatus.FSS_ToBePaid = updateStudentStatus.FSS_ToBePaid + Convert.ToInt64(feepge.FSFM_Amount);
                            _FeeGroupContext.Update(updateStudentStatus);
                        }
                        else
                        {
                            var FMCC_Idnew = (from a in _FeeGroupContext.feeYCC
                                              from b in _FeeGroupContext.feeYCCC
                                              from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                              where (a.FYCC_Id == b.FYCC_Id && a.ASMAY_Id == feepge.ASMAY_Id && a.MI_Id == feepge.MI_Id && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == feepge.AMST_Id && c.ASMAY_Id == feepge.ASMAY_Id)
                                              select a.FMCC_Id).FirstOrDefault();
                            if (FMCC_Idnew == 0)
                            {
                                data.returnval = "a";
                            }
                            else
                            {
                                var FMAlist = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                               from b in _FeeGroupContext.feeMIY
                                               where (a.FTI_Id == b.FTI_Id && a.FMCC_Id == FMCC_Idnew && a.MI_Id == feepge.MI_Id && a.ASMAY_Id == feepge.ASMAY_Id && a.FMG_Id == feepge.FMG_Id && a.FMH_Id == feepge.FMH_Id && a.FTI_Id == feepge.FTI_Id)//&& a.FMA_Amount > 0
                                               select a.FMA_Id).Distinct().FirstOrDefault();

                                if (FMAlist == 0)
                                {
                                    data.returnval = "a";
                                }
                                else
                                {
                                    FeeStudentTransactionDTO obj_status = new FeeStudentTransactionDTO();
                                    obj_status.MI_Id = feepge.MI_Id;
                                    obj_status.ASMAY_Id = feepge.ASMAY_Id;
                                    obj_status.Amst_Id = feepge.AMST_Id;
                                    obj_status.FMG_Id = feepge.FMG_Id;
                                    obj_status.FMH_Id = feepge.FMH_Id;
                                    obj_status.FTI_Id = feepge.FTI_Id;
                                    obj_status.FMA_Id = FMAlist;
                                    obj_status.FSS_OBArrearAmount = 0;
                                    obj_status.FSS_OBExcessAmount = 0;
                                    obj_status.FSS_CurrentYrCharges = 0;
                                    obj_status.FSS_TotalToBePaid = feepge.FSFM_Amount;
                                    obj_status.FSS_ToBePaid = feepge.FSFM_Amount;
                                    obj_status.FSS_PaidAmount = 0;
                                    obj_status.FSS_ExcessPaidAmount = 0;
                                    obj_status.FSS_ExcessAdjustedAmount = 0;
                                    obj_status.FSS_RunningExcessAmount = 0;
                                    obj_status.FSS_ConcessionAmount = 0;
                                    obj_status.FSS_AdjustedAmount = 0;
                                    obj_status.FSS_WaivedAmount = 0;
                                    obj_status.FSS_RebateAmount = 0;
                                    obj_status.FSS_FineAmount = 0;
                                    obj_status.FSS_RefundAmount = 0;
                                    obj_status.FSS_RefundAmountAdjusted = 0;
                                    obj_status.FSS_NetAmount = feepge.FSFM_Amount;
                                    obj_status.FSS_ChequeBounceFlag = false;
                                    obj_status.FSS_ArrearFlag = false;
                                    obj_status.FSS_RefundOverFlag = false;
                                    obj_status.FSS_ActiveFlag = true;
                                    obj_status.User_Id = data.userid;
                                    _FeeGroupContext.Add(obj_status);
                                }

                            }
                        }
                        if (data.returnval != "a")
                        {
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = "Update";
                            }
                            else
                            {
                                data.returnval = "not Update";
                            }
                        }
                            
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeCardDetailsEntry.Where(t => t.AMST_Id == feepge.AMST_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.FMG_Id == feepge.FMG_Id && t.FMH_Id == feepge.FMH_Id).ToList();
                    if (result.Count() > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        FeeCardDetailsEntryDMO feepge1 = new FeeCardDetailsEntryDMO();
                        feepge1.MI_Id = feepge.MI_Id;
                        feepge1.AMST_Id = feepge.AMST_Id;
                        feepge1.ASMAY_Id = feepge.ASMAY_Id;
                        feepge1.FMH_Id = feepge.FMH_Id;
                        feepge1.FMG_Id = feepge.FMG_Id;
                        feepge1.FTI_Id = feepge.FTI_Id;
                        feepge1.FSFM_Amount = feepge.FSFM_Amount;
                        feepge1.FSFM_PaidAmount= 0;
                        feepge1.CreatedDate = DateTime.Now;
                        feepge1.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Add(feepge1);
                        var lorg = _FeeGroupContext.FeeStudentTransactionDMO.SingleOrDefault(t => t.AMST_Id == feepge.AMST_Id && t.FMG_Id == feepge.FMG_Id && t.FMH_Id == feepge.FMH_Id && t.FTI_Id == feepge.FTI_Id && t.MI_Id == feepge.MI_Id && t.ASMAY_Id == feepge.ASMAY_Id);
                        if (lorg != null)
                        {
                            var updateStudentStatus = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == lorg.FSS_Id);
                            updateStudentStatus.FSS_ToBePaid = updateStudentStatus.FSS_ToBePaid + Convert.ToInt64(feepge.FSFM_Amount);
                            _FeeGroupContext.Update(updateStudentStatus);
                        }
                        else
                        {
                            var FMCC_Idnew = (from a in _FeeGroupContext.feeYCC
                                              from b in _FeeGroupContext.feeYCCC
                                              from c in _FeeGroupContext.School_Adm_Y_StudentDMO
                                              where (a.FYCC_Id == b.FYCC_Id && a.ASMAY_Id == feepge.ASMAY_Id && a.MI_Id == feepge.MI_Id && b.ASMCL_Id == c.ASMCL_Id && c.AMST_Id == feepge.AMST_Id && c.ASMAY_Id == feepge.ASMAY_Id)
                                              select a.FMCC_Id).FirstOrDefault();
                            if (FMCC_Idnew == 0)
                            {
                                data.returnval = "a";
                            }
                            else
                            {
                                var FMAlist = (from a in _FeeGroupContext.FeeAmountEntryDMO
                                               from b in _FeeGroupContext.feeMIY
                                               where (a.FTI_Id == b.FTI_Id && a.FMCC_Id == FMCC_Idnew && a.MI_Id == feepge.MI_Id && a.ASMAY_Id == feepge.ASMAY_Id && a.FMG_Id == feepge.FMG_Id && a.FMH_Id == feepge.FMH_Id && a.FTI_Id == feepge.FTI_Id)//&& a.FMA_Amount > 0
                                               select a.FMA_Id).Distinct().FirstOrDefault();

                                if (FMAlist==0)
                                {
                                    data.returnval = "a";
                                }
                                else
                                {
                                    FeeStudentTransactionDTO obj_status = new FeeStudentTransactionDTO();
                                    obj_status.MI_Id = feepge.MI_Id;
                                    obj_status.ASMAY_Id = feepge.ASMAY_Id;
                                    obj_status.Amst_Id = feepge.AMST_Id;
                                    obj_status.FMG_Id = feepge.FMG_Id;
                                    obj_status.FMH_Id = feepge.FMH_Id;
                                    obj_status.FTI_Id = feepge.FTI_Id;
                                    obj_status.FMA_Id = FMAlist;
                                    obj_status.FSS_OBArrearAmount = 0;
                                    obj_status.FSS_OBExcessAmount = 0;
                                    obj_status.FSS_CurrentYrCharges = 0;
                                    obj_status.FSS_TotalToBePaid = feepge.FSFM_Amount;
                                    obj_status.FSS_ToBePaid = feepge.FSFM_Amount;
                                    obj_status.FSS_PaidAmount = 0;
                                    obj_status.FSS_ExcessPaidAmount = 0;
                                    obj_status.FSS_ExcessAdjustedAmount = 0;
                                    obj_status.FSS_RunningExcessAmount = 0;
                                    obj_status.FSS_ConcessionAmount = 0;
                                    obj_status.FSS_AdjustedAmount = 0;
                                    obj_status.FSS_WaivedAmount = 0;
                                    obj_status.FSS_RebateAmount = 0;
                                    obj_status.FSS_FineAmount = 0;
                                    obj_status.FSS_RefundAmount = 0;
                                    obj_status.FSS_RefundAmountAdjusted = 0;
                                    obj_status.FSS_NetAmount = feepge.FSFM_Amount;
                                    obj_status.FSS_ChequeBounceFlag = false;
                                    obj_status.FSS_ArrearFlag = false;
                                    obj_status.FSS_RefundOverFlag = false;
                                    obj_status.FSS_ActiveFlag = true;
                                    obj_status.User_Id = data.userid;
                                    _FeeGroupContext.Add(obj_status);
                                }

                            }
                        }

                        if (data.returnval != "a")
                        {
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 1)
                            {
                                data.returnval = "Save";
                            }
                            else
                            {
                                data.returnval = "not Save";
                            }
                        }
                    }
                }
            
            }
            catch (Exception ee)
            {
              //  _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeCardDetailEntryDTO editdetails(int id)
        {
            FeeCardDetailEntryDTO data = new FeeCardDetailEntryDTO();
            var lorg = _FeeGroupContext.feeCardDetailsEntry.Single(t => t.FSFM_Id == id);

            try
            {
                if (lorg != null)
                {
                    var lorg1 = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.MI_Id == lorg.MI_Id && t.ASMAY_Id == lorg.ASMAY_Id);
                    if (lorg1.FSS_ToBePaid < lorg.FSFM_Amount || lorg.FSFM_PaidAmount > 0)
                    {
                        data.returnval = "Already Paid. Record cannot Edit";
                    }
                    else
                    {
                        data.fillgrid = (from a in _FeeGroupContext.AdmissionStudentDMO
                                         from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                         from c in _FeeGroupContext.feeCardDetailsEntry
                                         from d in _FeeGroupContext.FeeGroupDMO
                                         from e in _FeeGroupContext.FeeHeadDMO
                                         from f in _FeeGroupContext.feeMIY
                                         from g in _FeeGroupContext.AcademicYear
                                         where (c.FSFM_Id == lorg.FSFM_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == lorg.MI_Id && g.MI_Id == lorg.MI_Id && g.ASMAY_Id == c.ASMAY_Id && g.ASMAY_ActiveFlag == 1 && f.MI_ID == lorg.MI_Id && f.FTI_Id == c.FTI_Id && e.MI_Id == lorg.MI_Id && e.FMH_Id == c.FMH_Id && e.FMH_ActiveFlag == true && d.MI_Id == lorg.MI_Id && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && c.MI_Id == lorg.MI_Id && a.AMST_Id == c.AMST_Id && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S")
                                         select new FeeCardDetailEntryDTO
                                         {
                                             //AMST_RegistrationNo = a.AMST_RegistrationNo,
                                             Amst_Id = a.AMST_Id,
                                             AMST_FirstName = a.AMST_FirstName,
                                             AMST_MiddleName = a.AMST_MiddleName,
                                             AMST_LastName = a.AMST_LastName,
                                             ASMAY_Year = g.ASMAY_Year,
                                             FMG_Id = c.FMG_Id,
                                             FMH_Id = c.FMH_Id,
                                             FTI_Id = c.FTI_Id,
                                             FMG_GroupName = d.FMG_GroupName,
                                             FMH_FeeName = e.FMH_FeeName,
                                             FTI_Name = f.FTI_Name,
                                             FSFM_Amount = c.FSFM_Amount,
                                             FSFM_PaidAmount = c.FSFM_PaidAmount,
                                             FSFM_Id = c.FSFM_Id

                                         }).Distinct().ToArray();
                        data.fillmastergroup = (from a in _FeeGroupContext.FeeGroupDMO
                                                from b in _FeeGroupContext.FeeStudentGroupMappingDMO
                                                from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                                where (a.FMG_Id == b.FMG_Id && b.AMST_Id == lorg.AMST_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id)
                                                select new FeeCardDetailEntryDTO
                                                {
                                                    FMG_GroupName = a.FMG_GroupName,
                                                    FMG_Id = a.FMG_Id,
                                                }).Distinct().ToArray();
                        data.alldata = (from a in _FeeGroupContext.FeeHeadDMO
                                        from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                        where (a.FMH_Id == b.FMH_Id && b.FMG_Id == lorg.FMG_Id && a.MI_Id == lorg.MI_Id && a.FMH_ActiveFlag == true && b.MI_Id == lorg.MI_Id && b.ASMAY_Id == lorg.ASMAY_Id)
                                        select new FeeCardDetailEntryDTO
                                        {
                                            FMH_FeeName = a.FMH_FeeName,
                                            FMH_Id = a.FMH_Id,
                                        }).Distinct().OrderBy(t => t.FMH_Id).ToArray();
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;

        }
        public FeeCardDetailEntryDTO Deletedetails(int id)
        {
            FeeCardDetailEntryDTO del = new FeeCardDetailEntryDTO();
            var lorg = _FeeGroupContext.feeCardDetailsEntry.Single(t => t.FSFM_Id == id);

            try
            {
                if (lorg != null)
                {
                    var lorg1 = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.AMST_Id == lorg.AMST_Id && t.FMG_Id == lorg.FMG_Id && t.FMH_Id == lorg.FMH_Id && t.FTI_Id == lorg.FTI_Id && t.MI_Id == lorg.MI_Id && t.ASMAY_Id == lorg.ASMAY_Id);
                    if(lorg1.FSS_ToBePaid<lorg.FSFM_Amount || lorg.FSFM_PaidAmount>0)
                    {
                        del.returnval = "Already Paid. Record cannot delete";
                    }
                    else
                    {
                        var updateStudentStatus = _FeeGroupContext.FeeStudentTransactionDMO.Single(t => t.FSS_Id == lorg1.FSS_Id);
                        updateStudentStatus.FSS_ToBePaid = updateStudentStatus.FSS_ToBePaid - Convert.ToInt64(lorg.FSFM_Amount);
                        _FeeGroupContext.Update(updateStudentStatus);
                        _FeeGroupContext.feeCardDetailsEntry.Remove(lorg);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists > 1)
                        {
                            del.returnval = "true";
                        }
                        else
                        {
                            del.returnval = "false";
                        }
                    }
                   
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
          
           
            return del;

        }
    }
}



