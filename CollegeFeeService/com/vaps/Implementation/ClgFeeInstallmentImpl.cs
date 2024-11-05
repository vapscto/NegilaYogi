using System;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fee;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class ClgFeeInstallmentImpl : Interfaces.ClgFeeInstallmentInterface
    {

        private static ConcurrentDictionary<string, Clg_Fee_Installment_DTO> _login =
         new ConcurrentDictionary<string, Clg_Fee_Installment_DTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<ClgFeeInstallmentImpl> _logger;
        public ClgFeeInstallmentImpl(CollFeeGroupContext frgContext, ILogger<ClgFeeInstallmentImpl> log)
        {
            _logger = log;
            _FeeGroupContext = frgContext;
        }
   

        public Clg_Fee_Installment_DTO SaveGroupData(Clg_Fee_Installment_DTO FGpage)
        {
            Clg_Fee_Installment_DMO feepge = Mapper.Map<Clg_Fee_Installment_DMO>(FGpage);
            Clg_Fee_Installments_Yearly_DTO te = new Clg_Fee_Installments_Yearly_DTO();

            string retval = "";
            try
            {
                if (feepge.FMI_Id > 0)
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name)
                           .Where(group => group.Count() > 1)
                           .Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg = "Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var res = _FeeGroupContext.Clg_Fee_Installment_DMO.Where(t => t.FMI_Id != feepge.FMI_Id && t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);
                        if (res.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                        }
                        else
                        {
                            var result = _FeeGroupContext.Clg_Fee_Installment_DMO.Single(t => t.FMI_Id == feepge.FMI_Id);
                            result.MI_Id = feepge.MI_Id;
                            result.FMI_Name = feepge.FMI_Name;
                            result.FMI_No_Of_Installments = feepge.FMI_No_Of_Installments;
                            result.FMI_Installment_Type = feepge.FMI_Installment_Type;
                            result.FMI_ActiceFlag = feepge.FMI_ActiceFlag;
                            result.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Update(result);
                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {
                                // Clg_Fee_Installments_Yearly_DMO feepgeY = Mapper.Map<Clg_Fee_Installments_Yearly_DMO>(FGpage.fydto[i]);
                                //  var resultnew = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.FirstOrDefault(t => t.FTI_Id == FGpage.fydto[i].FTI_Id);

                                if (FGpage.fydto[i].FTI_Id == 0)
                                {
                                    Clg_Fee_Installments_Yearly_DMO feepgeY = Mapper.Map<Clg_Fee_Installments_Yearly_DMO>(FGpage.fydto[i]);
                                    feepgeY.FMI_Id = feepge.FMI_Id;
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                    feepgeY.MI_ID = FGpage.MI_Id;
                                    feepgeY.CreatedDate = DateTime.Now;
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Add(feepgeY);
                                }
                                else
                                {
                                    Clg_Fee_Installments_Yearly_DMO feepgeY = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Single(t => t.FTI_Id == FGpage.fydto[i].FTI_Id);
                                    // feepgeY.FMI_Id = feepge.FMI_Id;
                                    feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                    // feepgeY.MI_ID = FGpage.MI_Id;
                                    // feepgeY.CreatedDate = DateTime.Now;
                                    feepgeY.UpdatedDate = DateTime.Now;
                                    _FeeGroupContext.Update(feepgeY);
                                }
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                retval = "Update";
                                FGpage.returnduplicatestatus = retval;

                            }
                            else
                            {
                                retval = "NotUpdate";
                                FGpage.returnduplicatestatus = retval;
                            }
                        }
                    }
                }

                else
                {
                    var duplicateKeys = FGpage.fydto.GroupBy(x => x.FTI_Name).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
                    if (duplicateKeys.Count > 0)
                    {
                        FGpage.msg = "Installment Name Contains Some Duplicate Values";
                        return FGpage;
                    }
                    else
                    {
                        var result = _FeeGroupContext.Clg_Fee_Installment_DMO.Where(t => t.FMI_Id == feepge.FMI_Id || t.FMI_Name == feepge.FMI_Name && t.MI_Id == feepge.MI_Id);

                        if (feepge.FMI_Installment_Type == "2")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (feepge.FMI_Installment_Type == "3")
                        {
                            feepge.FMI_No_Of_Installments = 12;
                        }
                        else if (feepge.FMI_Installment_Type == "4")
                        {
                            feepge.FMI_No_Of_Installments = 4;
                        }
                        else if (feepge.FMI_Installment_Type == "5")
                        {
                            feepge.FMI_No_Of_Installments = 1;
                        }
                        else if (feepge.FMI_Installment_Type == "6")
                        {
                            feepge.FMI_No_Of_Installments = 2;
                        }
                        else if (feepge.FMI_Installment_Type == "7")
                        {
                            feepge.FMI_No_Of_Installments = 24;
                        }

                        if (result.Count() > 0)
                        {
                            retval = "Duplicate";
                            FGpage.returnduplicatestatus = retval;
                            return FGpage;
                        }
                        else
                        {

                            feepge.CreatedDate = DateTime.Now;
                            feepge.UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Add(feepge);
                            for (int i = 0; i < FGpage.fydto.Count; i++)
                            {
                                Clg_Fee_Installments_Yearly_DMO feepgeY = Mapper.Map<Clg_Fee_Installments_Yearly_DMO>(FGpage.fydto[i]);
                                feepgeY.FMI_Id = feepge.FMI_Id;
                                feepgeY.FTI_Name = FGpage.fydto[i].FTI_Name;
                                feepgeY.MI_ID = FGpage.MI_Id;
                                feepgeY.CreatedDate = DateTime.Now;
                                feepgeY.UpdatedDate = DateTime.Now;
                                _FeeGroupContext.Add(feepgeY);
                            }
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists > 0)
                            {
                                retval = "Save";
                                FGpage.returnduplicatestatus = retval;

                            }
                            else
                            {
                                retval = "NotSave";
                                FGpage.returnduplicatestatus = retval;
                            }
                        }
                    }
                }

                List<Clg_Fee_Installment_DMO> allpages = new List<Clg_Fee_Installment_DMO>();
                allpages = _FeeGroupContext.Clg_Fee_Installment_DMO.OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.InstallmentData = allpages.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }

        public Clg_Fee_Installment_DTO getdetails(Clg_Fee_Installment_DTO FGRDT)
        {
            //Clg_Fee_Installment_DTO FGRDT = new Clg_Fee_Installment_DTO();
            try
            {
                List<Clg_Fee_Installment_DMO> feegrp = new List<Clg_Fee_Installment_DMO>();
                feegrp = _FeeGroupContext.Clg_Fee_Installment_DMO.Where(t => t.MI_Id == FGRDT.MI_Id).OrderBy(t => t.FMI_Name).ToList();
                FGRDT.InstallmentData = feegrp.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == FGRDT.MI_Id && t.Is_Active == true).ToList();
                FGRDT.academicdrp = allyear.Distinct().ToArray();


                List<Clg_Fee_Installment_DMO> allinstypes = new List<Clg_Fee_Installment_DMO>();
                allinstypes = _FeeGroupContext.Clg_Fee_Installment_DMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMI_ActiceFlag == true).ToList();
                FGRDT.instypesdrp = allinstypes.ToArray();


                FGRDT.datasendhtml = (from a in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                      from b in _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO
                                      where (a.FTI_Id == b.FTI_Id && a.MI_ID == FGRDT.MI_Id)
                                      select new Clg_Fee_Installment_Due_Date_DTO
                                      {
                                          FTIDD_Id = b.FTIDD_Id,
                                          fname = a.FTI_Name,
                                          FTIDD_FromDate = b.FTIDD_FromDate,
                                          FTIDD_ToDate = b.FTIDD_ToDate,
                                          FTIDD_ApplicableDate = b.FTIDD_ApplicableDate,
                                          FTIDD_DueDate = b.FTIDD_DueDate,
                                      }
              ).ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }

        public Clg_Fee_Installment_DTO EditgroupDetails(int id)
        {
            Clg_Fee_Installment_DTO FMG = new Clg_Fee_Installment_DTO();
            try
            {
                List<Clg_Fee_Installment_DMO> masterfeegroup = new List<Clg_Fee_Installment_DMO>();
                masterfeegroup = _FeeGroupContext.Clg_Fee_Installment_DMO.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                FMG.InstallmentData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public Clg_Fee_Installment_DTO GetGroupSearchData(Clg_Fee_Installment_DTO mas)
        {

            Clg_Fee_Installment_DTO FGRDT = new Clg_Fee_Installment_DTO();
            try
            {
                List<Clg_Fee_Installment_DMO> feegrp = new List<Clg_Fee_Installment_DMO>();
                feegrp = _FeeGroupContext.Clg_Fee_Installment_DMO.ToList();
                FGRDT.InstallmentData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public Clg_Fee_Installment_DTO getpageedit(int id)
        {
            Clg_Fee_Installment_DTO page = new Clg_Fee_Installment_DTO();
            try
            {
                List<Clg_Fee_Installment_DMO> lorg = new List<Clg_Fee_Installment_DMO>();
                lorg = _FeeGroupContext.Clg_Fee_Installment_DMO.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InstallmentData = lorg.ToArray();

                List<Clg_Fee_Installments_Yearly_DMO> lorg123 = new List<Clg_Fee_Installments_Yearly_DMO>();
                lorg123 = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.AsNoTracking().Where(t => t.FMI_Id.Equals(id)).ToList();
                page.InTData = lorg123.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public Clg_Fee_Installment_Due_Date_DTO getpageeditY(int id)
        {
            Clg_Fee_Installment_Due_Date_DTO page = new Clg_Fee_Installment_Due_Date_DTO();
            try
            {
                page.InstallmentDatad = (from a in _FeeGroupContext.Clg_Fee_Installment_DMO
                                         from b in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                         from c in _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO
                                         where (a.FMI_Id == b.FMI_Id && b.FTI_Id == c.FTI_Id && c.FTIDD_Id == id)
                                         select new Clg_Fee_Installment_Due_Date_DTO
                                         {

                                             FTIDD_Id = Convert.ToInt64(id),
                                             FTI_Id = b.FTI_Id,
                                             ftI_Name = b.FTI_Name,
                                             fmiid = a.FMI_Id,
                                             yrid = c.ASMAY_Id,
                                             FTIDD_FromDate = c.FTIDD_FromDate,
                                             FTIDD_ToDate = c.FTIDD_ToDate,
                                             FTIDD_ApplicableDate = c.FTIDD_ApplicableDate,
                                             FTIDD_DueDate = c.FTIDD_DueDate,
                                         }
              ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public Clg_Fee_Installment_DTO deleterec(int id)
        {
            bool returnresult = false;
            Clg_Fee_Installment_DTO page = new Clg_Fee_Installment_DTO();
            bool rtvalue = deleteforeighkeyrecord(id);     //  first delete foreigh key table 
            if (rtvalue == true)  // true refere delete
            {


                List<Clg_Fee_Installment_DMO> lorg = new List<Clg_Fee_Installment_DMO>();
                lorg = _FeeGroupContext.Clg_Fee_Installment_DMO.Where(t => t.FMI_Id.Equals(id)).ToList();
                try
                {
                    if (lorg.Any())
                    {
                        _FeeGroupContext.Remove(lorg.ElementAt(0));

                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }

                    List<Clg_Fee_Installment_DMO> allpages = new List<Clg_Fee_Installment_DMO>();
                    allpages = _FeeGroupContext.Clg_Fee_Installment_DMO.ToList();
                    page.InstallmentData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                returnresult = false;
                page.returnval = returnresult;
            }

            return page;

        }
        public Clg_Fee_Installment_Due_Date_DTO deleterecY(int id)
        {

            bool returnresult = false;

            Clg_Fee_Installment_Due_Date_DTO page = new Clg_Fee_Installment_Due_Date_DTO();
            List<Clg_Fee_Installment_Due_Date_DMO> lorg = new List<Clg_Fee_Installment_Due_Date_DMO>();


            var exists1 = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Where(t => t.FTIDD_Id == id).ToArray();

            var exists2 = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Where(t => t.FTI_Id == exists1.FirstOrDefault().FTI_Id).ToArray();
            var exists = _FeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FMI_Id == exists2.FirstOrDefault().FMI_Id).ToArray().Count();
            if (exists == 0)
            {
                lorg = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Where(t => t.FTIDD_Id.Equals(id)).ToList();

                try
                {
                    if (lorg.Any())
                    {
                        _FeeGroupContext.Remove(lorg.ElementAt(0));

                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }

                    List<Clg_Fee_Installment_Due_Date_DMO> allpages = new List<Clg_Fee_Installment_Due_Date_DMO>();
                    allpages = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.ToList();
                    page.InstallmentDatad = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {

                page.returnvalexist = "Exist";
            }
            return page;
        }
        public Clg_Fee_Installment_DTO deactivate(Clg_Fee_Installment_DTO acd)
        {
            try
            {
                Clg_Fee_Installment_DMO feepge = Mapper.Map<Clg_Fee_Installment_DMO>(acd);
                if (feepge.FMI_Id > 0)
                {
                    var result = _FeeGroupContext.Clg_Fee_Installment_DMO.Single(t => t.FMI_Id == feepge.FMI_Id);

                    var feestutrans = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Where(t => t.FMI_Id == feepge.FMI_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.returnvalue = "used";
                        return acd;
                    }
                    else
                    {
                        result.UpdatedDate = DateTime.Now;

                        if (result.FMI_ActiceFlag == true)
                        {
                            result.FMI_ActiceFlag = false;
                            result.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            result.FMI_ActiceFlag = true;
                            result.UpdatedDate = DateTime.Now;
                        }
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }



                    }
                    List<Clg_Fee_Installment_DMO> allorganisation = new List<Clg_Fee_Installment_DMO>();
                    allorganisation = _FeeGroupContext.Clg_Fee_Installment_DMO.ToList();
                    acd.InstallmentData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public Clg_Fee_Installments_Yearly_DTO[] GetWrittenTestMarks(Clg_Fee_Installments_Yearly_DTO mas)
        {
            List<Clg_Fee_Installments_Yearly_DTO> AllInOne = new List<Clg_Fee_Installments_Yearly_DTO>();
            //   instemp temp = new instemp();
            int count = Convert.ToInt32(mas.valueloop);
            for (int i = 0; i < count; i++)
            {
                Clg_Fee_Installments_Yearly_DTO temp = new Clg_Fee_Installments_Yearly_DTO();
                temp.fno1 = i + 1;
                temp.fname1 = "";
                AllInOne.Add(temp);
            }
            return AllInOne.ToArray();
        }
        [Route("years")]
        public async Task<Clg_Fee_Installment_DTO> getIndependentDropDowns(Clg_Fee_Installment_DTO yrs)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _FeeGroupContext.AcademicYear.ToListAsync();
                yrs.academicdrp = allyear.ToArray();


                List<Clg_Fee_Installment_DMO> allinstypes = new List<Clg_Fee_Installment_DMO>();
                allinstypes = await _FeeGroupContext.Clg_Fee_Installment_DMO.ToListAsync();
                yrs.instypesdrp = allinstypes.ToArray();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var str = ex.Message;
            }

            return yrs;
        }
        public Clg_Fee_Installments_Yearly_DTO[] Getduedates(Clg_Fee_Installments_Yearly_DTO mas)
        {
            //List<Clg_Fee_Installment_Due_Date_DMO> Allrows = new List<Clg_Fee_Installment_Due_Date_DMO>();
            //Allrows = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Where(t => t.ASMAY_Id==mas.ASMAY_Id && t.MI_Id==mas.MI_Id && t.FTI_Id==mas.FTI_Id).ToList();
            //mas.yrlData = Allrows.ToArray();

            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _FeeGroupContext.AcademicYear.Where(t=>t.ASMAY_Id==mas.ASMAY_Id).ToList();
            mas.academicdrp = allyear.ToArray();

            mas.fillsaveddata = (from a in _FeeGroupContext.Clg_Fee_Installment_DMO
                                 from b in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 from c in _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO
                                 where (a.FMI_Id == b.FMI_Id && b.MI_ID == mas.MI_Id && c.ASMAY_Id == mas.ASMAY_Id)
                                 select new Clg_Fee_Installments_Yearly_DTO
                                 {
                                     FTI_Id = b.FTI_Id,
                                     FTI_Name = b.FTI_Name,
                                     fdate = c.FTIDD_FromDate,
                                     tdate = c.FTIDD_ToDate,
                                     Aplc = c.FTIDD_ApplicableDate,
                                     ddate = c.FTIDD_DueDate
                                 }
                            ).ToArray();


            List<Clg_Fee_Installments_Yearly_DTO> AllInOne = new List<Clg_Fee_Installments_Yearly_DTO>();
            List<Clg_Fee_Installments_Yearly_DMO> Allrows = new List<Clg_Fee_Installments_Yearly_DMO>();
            //Allrows = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Where(t => t.FMI_Id.Equals(mas.FMI_Id)).ToList();
            //mas.yrlData = Allrows.ToArray();

            //for (int i = 0; i < Allrows.Count; i++)
            //{

            Clg_Fee_Installments_Yearly_DTO temp = new Clg_Fee_Installments_Yearly_DTO();
            List<Clg_Fee_Installments_Yearly_DTO> fillsaveddata = new List<Clg_Fee_Installments_Yearly_DTO>();
            List<Clg_Fee_Installments_Yearly_DMO> Allname10 = new List<Clg_Fee_Installments_Yearly_DMO>();
            List<long> Allname2 = new List<long>();

            fillsaveddata = (
                                 from b in _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO
                                 from c in _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO
                                 where (b.FTI_Id == c.FTI_Id && b.MI_ID == mas.MI_Id && c.ASMAY_Id == mas.ASMAY_Id && b.FMI_Id == mas.FMI_Id)
                                 select new Clg_Fee_Installments_Yearly_DTO
                                 {
                                     FTI_Id = b.FTI_Id,
                                     FTI_Name = b.FTI_Name,
                                     fdate = c.FTIDD_FromDate,
                                     tdate = c.FTIDD_ToDate,
                                     Aplc = c.FTIDD_ApplicableDate,
                                     ddate = c.FTIDD_DueDate
                                 }
                       ).ToList();
            foreach (var a in fillsaveddata)
            {
                Allname2.Add(a.FTI_Id);
            }

            Allname10 = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Where(t => t.FMI_Id.Equals(mas.FMI_Id) && !Allname2.Contains(t.FTI_Id)).ToList().ToList();


            if (Allname10.Count > 0)
            {
                for (int i = 0; i < Allname10.Count; i++)
                {
                    //mas.yrlData = Allname10.ToArray();
                    //temp.FTI_Id = Allname10[i].FTI_Id;
                    //temp.FTI_Name = Allname10[i].FTI_Name;
                    Clg_Fee_Installments_Yearly_DTO dto = Mapper.Map<Clg_Fee_Installments_Yearly_DTO>(Allname10[i]);
                    AllInOne.Add(dto);
                }
                mas.instlreturn = "Yes";
            }
            else
            {
                mas.instlreturn = "No";
            }



            //}

            // return mas;
            return AllInOne.ToArray();

        }
        public Clg_Fee_Installment_DTO savedetailDDD(Clg_Fee_Installment_DTO FGpage)
        {
            string retvalue = "false";
            for (int i = 0; i < FGpage.fidddto.Count; i++)
            {
                retvalue = savedetailDDDinDB(FGpage.MI_Id, FGpage.temyrid, FGpage.fidddto[i].FTI_Id, FGpage.fidddto[i].FTIDD_Id, FGpage.fidddto[i].FTIDD_FromDate, FGpage.fidddto[i].FTIDD_ToDate, FGpage.fidddto[i].FTIDD_ApplicableDate, FGpage.fidddto[i].FTIDD_DueDate);
            }
            FGpage.returnvalue = retvalue;
            return FGpage;
        }
        public string savedetailDDDinDB(long mid, long yrid, long id, long ftidd, DateTime fromdate, DateTime todate, DateTime apldate, DateTime ddudate)
        {
            string returnvalue = "false";
            Clg_Fee_Installment_Due_Date_DTO te = new Clg_Fee_Installment_Due_Date_DTO();
            Clg_Fee_Installment_Due_Date_DMO feepgeY = Mapper.Map<Clg_Fee_Installment_Due_Date_DMO>(te);
            try
            {
                if (ftidd > 0)
                {
                    var result = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Single(t => t.FTIDD_Id == ftidd);
                    result.MI_Id = mid;
                    result.FTI_Id = id;
                    result.ASMAY_Id = yrid;
                    result.FTIDD_FromDate = fromdate;
                    result.FTIDD_ToDate = todate;
                    result.FTIDD_ApplicableDate = apldate;
                    result.FTIDD_DueDate = ddudate;
                    result.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {

                        returnvalue = "Update";
                    }
                    else
                    {
                        returnvalue = "NotUpdate";
                    }
                }
                else
                {
                    var result = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Where(t => t.FTI_Id == id && t.ASMAY_Id == yrid && t.MI_Id == mid);
                    if (result.Count() > 0)
                    {
                        returnvalue = "Duplicate";
                    }
                    else
                    {
                        feepgeY.MI_Id = mid;
                        feepgeY.FTI_Id = id;
                        feepgeY.ASMAY_Id = yrid;
                        feepgeY.FTIDD_FromDate = fromdate;
                        feepgeY.FTIDD_ToDate = todate;
                        feepgeY.FTIDD_ApplicableDate = apldate;
                        feepgeY.FTIDD_DueDate = ddudate;
                        feepgeY.CreatedDate = DateTime.Now;
                        feepgeY.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Add(feepgeY);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnvalue = "Save";
                        }
                        else
                        {
                            returnvalue = "NotSave";
                        }
                    }
                }
                List<Clg_Fee_Installment_Due_Date_DMO> allpages = new List<Clg_Fee_Installment_Due_Date_DMO>();
                allpages = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.OrderBy(t => t.FTIDD_Id).ToList();
                te.retrivedata = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return returnvalue;
        }
        public bool deleteforeighkeyrecord(long id)
        {
            bool returnresult = false;
            List<CLG_Fee_Yearly_Group_Head_Mapping> pmm = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
            pmm = _FeeGroupContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FMI_Id.Equals(id)).ToList();
            if (pmm.Count == 0)
            {
                Clg_Fee_Installments_Yearly_DTO page = new Clg_Fee_Installments_Yearly_DTO();
                List<Clg_Fee_Installments_Yearly_DMO> lorg = new List<Clg_Fee_Installments_Yearly_DMO>();
                lorg = _FeeGroupContext.Clg_Fee_Installments_Yearly_DMO.Where(t => t.FMI_Id.Equals(id)).ToList();
                try
                {
                    if (lorg.Count() > 0)
                    {
                        string condition = "deletefalse";
                        for (int i = 0; lorg.Count > i; i++)
                        {
                            Clg_Fee_AmountEntry_DTO page4 = new Clg_Fee_AmountEntry_DTO();
                            List<Clg_Fee_AmountEntry_DMO> lorg4 = new List<Clg_Fee_AmountEntry_DMO>();
                            lorg4 = _FeeGroupContext.Clg_Fee_AmountEntry_DMO.Where(t => t.FTI_Id.Equals(lorg[i].FTI_Id)).ToList();
                            if (lorg4.Count > 0)
                            {
                                condition = "deletefalse";
                            }
                            else
                            {
                                condition = "deletetrue";
                            }

                        }

                        if (condition == "deletetrue")
                        {
                            for (int i = 0; lorg.Count > i; i++)
                            {
                                Clg_Fee_Installment_Due_Date_DTO page1 = new Clg_Fee_Installment_Due_Date_DTO();
                                List<Clg_Fee_Installment_Due_Date_DMO> lorg1 = new List<Clg_Fee_Installment_Due_Date_DMO>();
                                lorg1 = _FeeGroupContext.Clg_Fee_Installment_Due_Date_DMO.Where(t => t.FTI_Id.Equals(lorg[i].FTI_Id)).ToList();
                                try
                                {
                                    if (lorg1.Any())
                                    {
                                        _FeeGroupContext.Remove(lorg1.ElementAt(0));
                                        var contactExists1 = _FeeGroupContext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            returnresult = true;
                                            page.returnval = returnresult;
                                        }
                                        else
                                        {
                                            returnresult = false;
                                            page.returnval = returnresult;
                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }

                            }
                            for (int i = 0; lorg.Count > i; i++)
                            {
                                _FeeGroupContext.Remove(lorg.ElementAt(i));
                                var contactExists = _FeeGroupContext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    returnresult = true;
                                    page.returnval = returnresult;
                                }
                                else
                                {
                                    returnresult = false;
                                    page.returnval = returnresult;
                                }
                            }
                        }
                        else
                        {
                            returnresult = false;

                        }
                    }
                    else
                    {
                        returnresult = true;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                returnresult = false;
            }

            return returnresult;
        }

    }
}
