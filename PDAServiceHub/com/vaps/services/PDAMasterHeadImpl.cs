using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.PDA;
using DataAccessMsSqlServerProvider.com.vapstech.PDA;
using DomainModel.Model.com.vapstech.PDA;

namespace PDAServiceHub.com.vaps.services
{
    public class PDAMasterHeadImpl : interfaces.PDAMasterHeadInterface
    {


        private static ConcurrentDictionary<string, PdaDTO> _login =
       new ConcurrentDictionary<string, PdaDTO>();

        public PDAContext _PdaheadContext;
        readonly ILogger<PDAMasterHeadImpl> _logger;
        public PDAMasterHeadImpl(PDAContext frgContext, ILogger<PDAMasterHeadImpl> log)
        {
            _logger = log;
            _PdaheadContext = frgContext;

        }

        public PdaDTO getdetails(PdaDTO data)
        {
            PdaDTO FGRDT = new PdaDTO();

            try
            {
                List<FeeHeadDMO> feegrp = new List<FeeHeadDMO>();
                feegrp = _PdaheadContext.feehead.Where(t => t.MI_Id == data.mi_id && t.FMH_PDAFlag==true).OrderBy(t => t.FMH_Order).ToList();
                FGRDT.headdata = feegrp.ToArray();

                List<PDA_Master_HeadDMO> pdahead = new List<PDA_Master_HeadDMO>();
                pdahead = _PdaheadContext.pdahead.Where(t => t.MI_Id == data.mi_id).OrderBy(t => t.PDAMH_Id).ToList();
                FGRDT.pdadata = pdahead.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }

        public PdaDTO savedetails(PdaDTO data)
        {
            bool returnresult = false;
            PDA_Master_HeadDMO feepge1 = Mapper.Map<PDA_Master_HeadDMO>(data);
            string retval = "";
            try
            {
                if (data.PDAMH_Id > 0)
                {
                    var result1 = _PdaheadContext.pdahead.Where(t => t.PDAMH_HeadName == data.PDAMH_HeadName && t.MI_Id == data.mi_id && t.FMH_ID == data.fmh_id).ToList();
                    if (result1.Count() > 0)
                    {
                        retval = "Duplicate";
                        data.returnduplicatestatus = retval;
                    }
                    else
                    {
                        var result = _PdaheadContext.pdahead.Single(t => t.PDAMH_Id == data.PDAMH_Id);
                        result.MI_Id = data.mi_id;
                        result.PDAMH_HeadName = data.PDAMH_HeadName;
                        result.PDAMH_ActiveFlag = true;
                        result.FMH_ID = data.fmh_id;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _PdaheadContext.Update(result);
                        var contactExists = _PdaheadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            data.returnval = returnresult;
                            data.message = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            data.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    
                    var result = _PdaheadContext.pdahead.Where(t => t.PDAMH_HeadName == data.PDAMH_HeadName && t.MI_Id == data.mi_id && t.FMH_ID==data.fmh_id).ToList();

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        data.returnduplicatestatus = retval;
                    }
                    else
                    {
                        PDA_Master_HeadDMO feepge = new PDA_Master_HeadDMO();
                        feepge.MI_Id = data.mi_id;
                        feepge.PDAMH_HeadName = data.PDAMH_HeadName;
                        feepge.PDAMH_ActiveFlag = true;
                        feepge.FMH_ID = data.fmh_id;
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;
                        _PdaheadContext.Add(feepge);
                        var contactExists = _PdaheadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            PdaDTO dto = Mapper.Map<PdaDTO>(feepge);
                            var res = _PdaheadContext.pdahead.Single(t => t.PDAMH_Id == dto.PDAMH_Id);
                            Mapper.Map(dto, res);
                            _PdaheadContext.Update(res);
                            _PdaheadContext.SaveChanges();

                            returnresult = true;
                            data.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            data.returnval = returnresult;
                        }
                    }
                }
                

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public PdaDTO getpageedit(PdaDTO data)
        {
            PdaDTO page = new PdaDTO();
            try
            {
                List<PDA_Master_HeadDMO> lorg = new List<PDA_Master_HeadDMO>();
                lorg = _PdaheadContext.pdahead.AsNoTracking().Where(t => t.PDAMH_Id.Equals(data.PDAMH_Id)).ToList();
                page.pdadata = lorg.ToArray();

                List<FeeHeadDMO> feegrp = new List<FeeHeadDMO>();
                feegrp = _PdaheadContext.feehead.Where(t => t.MI_Id == data.mi_id && t.FMH_Flag == "G" && t.FMH_PDAFlag == true).OrderBy(t => t.FMH_Order).ToList();
                page.headdata = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public PdaDTO deactivate(PdaDTO data)
        {

            PDA_Master_HeadDMO feepge = Mapper.Map<PDA_Master_HeadDMO>(data);
            try
            {
                
                if (data.PDAMH_Id > 0)
                {

                    var result = _PdaheadContext.pdahead.Single(t => t.PDAMH_Id == data.PDAMH_Id);
                    var feestutrans = _PdaheadContext.pdaexpenditurehead.Where(t => t.PDAMH_Id == data.PDAMH_Id).ToList();
                    if (feestutrans.Count > 0)
                    {
                        data.message = "used";
                        return data;
                    }
                    else
                    {
                        if (result.PDAMH_ActiveFlag == true)
                        {
                            result.PDAMH_ActiveFlag = false;
                        }
                        else
                        {
                            result.PDAMH_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _PdaheadContext.Update(result);
                        var flag = _PdaheadContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                    //List<PDA_Master_HeadDMO> pdahead = new List<PDA_Master_HeadDMO>();
                    //pdahead = _PdaheadContext.pdahead.Where(t => t.MI_Id == data.mi_id && t.PDAMH_ActiveFlag == true).OrderBy(t => t.PDAMH_Id).ToList();
                    //data.pdadata = pdahead.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return data;
        }


    }
}
