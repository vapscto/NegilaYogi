using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model.com.vaps.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class FeeHeadClgImpl:Interfaces.FeeHeadClgInterface
    {
        private static ConcurrentDictionary<string, FeeHeadClgDTO> _login =
        new ConcurrentDictionary<string, FeeHeadClgDTO>();

        public CollFeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<FeeHeadClgImpl> _logger;
        public FeeHeadClgImpl(CollFeeGroupContext frgContext, ILogger<FeeHeadClgImpl> log)
        {
            _logger = log;
            _FeeGroupHeadContext = frgContext;

        }
        public FeeHeadClgDTO SaveGroupData(FeeHeadClgDTO FGpage)
        {
            bool returnresult = false;
            FeeHeadClgDMO feepge = Mapper.Map<FeeHeadClgDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMH_Id > 0)
                {
                    var result1 = _FeeGroupHeadContext.FeeHeadClgDMO.Where(t => t.FMH_Id != feepge.FMH_Id && t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _FeeGroupHeadContext.FeeHeadClgDMO.Single(t => t.FMH_Id == feepge.FMH_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMH_FeeName = feepge.FMH_FeeName;
                        result.FMH_Flag = feepge.FMH_Flag;
                        result.FMH_Order = feepge.FMH_Order;
                        result.FMH_PDAFlag = feepge.FMH_PDAFlag;
                        result.FMH_RefundFlag = feepge.FMH_RefundFlag;
                        result.FMH_SpecialFeeFlag = feepge.FMH_SpecialFeeFlag;
                        result.FMH_ActiveFlag = feepge.FMH_ActiveFlag;
                        result.user_id = feepge.user_id;
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Update(result);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.message = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    var result = _FeeGroupHeadContext.FeeHeadClgDMO.Where(t => t.FMH_FeeName == feepge.FMH_FeeName && t.MI_Id == feepge.MI_Id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Add(feepge);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            FeeHeadClgDTO dto = Mapper.Map<FeeHeadClgDTO>(feepge);
                            dto.FMH_Order = Convert.ToInt32(dto.FMH_Id);
                            var res = _FeeGroupHeadContext.FeeHeadClgDMO.Single(t => t.FMH_Id == dto.FMH_Id);
                            Mapper.Map(dto, res);
                            _FeeGroupHeadContext.Update(res);
                            _FeeGroupHeadContext.SaveChanges();

                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                List<FeeHeadClgDMO> allpages = new List<FeeHeadClgDMO>();
                allpages = _FeeGroupHeadContext.FeeHeadClgDMO.OrderBy(t => t.FMH_Order).ToList();
                FGpage.GroupHeadData = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }

        public FeeHeadClgDTO changeorderData(FeeHeadClgDTO dto)
        {
            bool returnresult = false;
            try
            {
                if (dto.CourseDTO.Count() > 0)
                {
                    foreach (FeeHeadClgDTO mob in dto.CourseDTO)
                    {
                        if (mob.FMH_Id > 0)
                        {
                            var result = _FeeGroupHeadContext.FeeHeadClgDMO.Single(t => t.FMH_Id.Equals(mob.FMH_Id));
                            Mapper.Map(mob, result);
                            _FeeGroupHeadContext.Update(result);
                            _FeeGroupHeadContext.SaveChanges();
                        }
                    }
                    returnresult = true;
                    dto.returnval = returnresult;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                returnresult = false;
                dto.returnval = returnresult;
            }
            return dto;
        }

        public FeeHeadClgDTO getdetails(int id)
        {
            FeeHeadClgDTO FGRDT = new FeeHeadClgDTO();

            try
            {
                List<FeeHeadClgDMO> feegrp = new List<FeeHeadClgDMO>();
                feegrp = _FeeGroupHeadContext.FeeHeadClgDMO.Where(t => t.MI_Id == id).OrderBy(t => t.FMH_Order).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeHeadClgDTO EditgroupDetails(int id)
        {
            FeeHeadClgDTO FMG = new FeeHeadClgDTO();
            try
            {
                List<FeeHeadClgDMO> masterfeegroup = new List<FeeHeadClgDMO>();
                masterfeegroup = _FeeGroupHeadContext.FeeHeadClgDMO.AsNoTracking().Where(t => t.FMH_Id.Equals(id)).ToList();
                FMG.GroupHeadData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeHeadClgDTO GetGroupSearchData(FeeHeadClgDTO mas)
        {

            FeeHeadClgDTO FGRDT = new FeeHeadClgDTO();
            try
            {
                List<FeeHeadClgDMO> feegrp = new List<FeeHeadClgDMO>();
                feegrp = _FeeGroupHeadContext.FeeHeadClgDMO.OrderBy(t => t.FMH_Order).ToList();
                FGRDT.GroupHeadData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeHeadClgDTO getpageedit(int id)
        {
            FeeHeadClgDTO page = new FeeHeadClgDTO();
            try
            {
                List<FeeHeadClgDMO> lorg = new List<FeeHeadClgDMO>();
                lorg = _FeeGroupHeadContext.FeeHeadClgDMO.AsNoTracking().Where(t => t.FMH_Id.Equals(id)).ToList();
                page.GroupHeadData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeHeadClgDTO deleterec(int id)
        {
            bool returnresult = false;
            bool dupl = false;
            FeeHeadClgDTO page = new FeeHeadClgDTO();
            List<CLG_Fee_Yearly_Group_Head_Mapping> lorgrecords = new List<CLG_Fee_Yearly_Group_Head_Mapping>();
            lorgrecords = _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping.Where(t => t.FMH_Id.Equals(id)).ToList();
            if (lorgrecords.Count == 0)
            {

                List<FeeHeadClgDMO> lorg = new List<FeeHeadClgDMO>();
                lorg = _FeeGroupHeadContext.FeeHeadClgDMO.Where(t => t.FMH_Id.Equals(id)).ToList();

                try
                {
                    if (lorg.Any())
                    {
                        _FeeGroupHeadContext.Remove(lorg.ElementAt(0));
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
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

                    List<FeeHeadClgDMO> allpages = new List<FeeHeadClgDMO>();
                    allpages = _FeeGroupHeadContext.FeeHeadClgDMO.OrderBy(t => t.FMH_Order).ToList();
                    page.GroupHeadData = allpages.ToArray();
                }
                catch (Exception ee)
                {
                    _logger.LogError(ee.Message);
                    Console.WriteLine(ee.Message);
                }
            }
            else
            {
                dupl = false;
                page.dupr = dupl;
            }
            return page;
        }
        public FeeHeadClgDTO deactivate(FeeHeadClgDTO acd)
        {
            try
            {
                FeeHeadClgDMO feepge = Mapper.Map<FeeHeadClgDMO>(acd);
                if (feepge.FMH_Id > 0)
                {

                    var result = _FeeGroupHeadContext.FeeHeadClgDMO.Single(t => t.FMH_Id == feepge.FMH_Id);
                    var feestutrans = _FeeGroupHeadContext.Fee_College_Student_StatusDMO.Where(t => t.FMH_Id == feepge.FMH_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        acd.message = "used";
                        return acd;
                    }
                    else
                    {
                        if (result.FMH_ActiveFlag == true)
                        {
                            result.FMH_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMH_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupHeadContext.Update(result);
                        var flag = _FeeGroupHeadContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }

                    List<FeeHeadClgDMO> allorganisation = new List<FeeHeadClgDMO>();
                    allorganisation = _FeeGroupHeadContext.FeeHeadClgDMO.OrderBy(t => t.FMH_Order).ToList();
                    acd.GroupHeadData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public FeeHeadClgDTO getallbankdetails(FeeHeadClgDTO data)
        {
            try
            {
                List<Fee_Master_BankDMO> feegrp = new List<Fee_Master_BankDMO>();
                feegrp = _FeeGroupHeadContext.Fee_Master_BankDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.getbankdetails = feegrp.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeHeadClgDTO savedata(FeeHeadClgDTO data)
        {
            try
            {
                if(data.FMBANK_Id>0)
                {
                    var result1 = _FeeGroupHeadContext.Fee_Master_BankDMO.Where(t => t.MI_Id==data.MI_Id && t.FMBANK_BankName==data.FMBANK_BankName && t.FMBANK_BankDescription == data.FMBANK_BankDescription).ToList();
                    if (result1.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _FeeGroupHeadContext.Fee_Master_BankDMO.Single(t => t.MI_Id == data.MI_Id && t.FMBANK_BankName==data.FMBANK_BankName);
                        result.MI_Id = data.MI_Id;
                        result.FMBANK_BankName = data.FMBANK_BankName;
                        result.FMBANK_BankDescription = data.FMBANK_BankDescription;
                        result.FMBANK_ActiveFlg = true;
                        result.FMBANK_UpdatedDate = DateTime.Now;
                        result.FMBANK_UpdatedBy = data.user_id;

                        _FeeGroupHeadContext.Update(result);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    Fee_Master_BankDMO feepge = new Fee_Master_BankDMO();
                    feepge.MI_Id = data.MI_Id;
                    feepge.FMBANK_BankName = data.FMBANK_BankName;
                    feepge.FMBANK_BankDescription = data.FMBANK_BankDescription;
                    feepge.FMBANK_ActiveFlg = true;
                    feepge.FMBANK_CreatedDate = DateTime.Now;
                    feepge.FMBANK_UpdatedDate = DateTime.Now;
                    feepge.FMBANK_CreatedBy = data.user_id;
                    feepge.FMBANK_UpdatedBy = data.user_id;
                    _FeeGroupHeadContext.Add(feepge);
                    var contactExists = _FeeGroupHeadContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                List<Fee_Master_BankDMO> feegrp = new List<Fee_Master_BankDMO>();
                feegrp = _FeeGroupHeadContext.Fee_Master_BankDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.getbankdetails = feegrp.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeHeadClgDTO edit(FeeHeadClgDTO data)
        {
            try
            {
                data.geteditdata = _FeeGroupHeadContext.Fee_Master_BankDMO.Where(a => a.MI_Id == data.MI_Id && a.FMBANK_Id == data.FMBANK_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeHeadClgDTO activedeactive(FeeHeadClgDTO data)
        {
            try
            {
                var result = _FeeGroupHeadContext.Fee_Master_BankDMO.Single(a => a.FMBANK_Id == data.FMBANK_Id);
                if (result.FMBANK_ActiveFlg == true)
                {
                    result.FMBANK_ActiveFlg = false;
                }
                else
                {
                    result.FMBANK_ActiveFlg = true;
                }
                result.FMBANK_UpdatedDate = DateTime.Now;
                _FeeGroupHeadContext.Update(result);
                int n = _FeeGroupHeadContext.SaveChanges();
                if (n > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
