using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.Extensions.Logging;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class mastercasteImpl : Interfaces.mastercasteInterface
    {
        private static ConcurrentDictionary<string, mastercasteDTO> _login =
        new ConcurrentDictionary<string, mastercasteDTO>();

        private readonly mastercasteContext _mastercasteContext; 
        private readonly ILogger<mastercasteImpl> _log;
        public mastercasteImpl(mastercasteContext mastercasteContext, ILogger<mastercasteImpl> log)
        {
            _mastercasteContext = mastercasteContext;
            _log = log;
        }

        public mastercasteDTO GetmastercasteData(mastercasteDTO mastercasteDTO)//int IVRMM_Id
        {

            List<castecategoryDMO> Allname = new List<castecategoryDMO>();

            try
            {
                Allname = _mastercasteContext.castecategoryDMO.ToList();
                mastercasteDTO.mastercastename = Allname.ToArray();


                mastercasteDTO.GridviewDetails = (from sp in _mastercasteContext.mastercasteDMO
                                                  from cp in _mastercasteContext.castecategoryDMO
                                                  where (sp.IMCC_Id == cp.IMCC_Id && sp.MI_Id == mastercasteDTO.MI_Id)
                                                  select new mastercasteDTO
                                                  {
                                                      IC_Id = sp.IMC_Id,
                                                      IC_CasteName = sp.IMC_CasteName,
                                                      IC_CasteDesc = sp.IMC_CasteDesc,
                                                      CategoryName = cp.IMCC_CategoryName,
                                                      IMCC_Id = sp.IMCC_Id,
                                                      MI_Id = sp.MI_Id
                                                  }).OrderByDescending(a=>a.IC_Id).ToArray();
                if (mastercasteDTO.GridviewDetails.Length > 0)
                {
                    mastercasteDTO.count = mastercasteDTO.GridviewDetails.Length;
                }
                else
                {
                    mastercasteDTO.count = 0;
                }

                mastercasteDTO.getcastelist = _mastercasteContext.mastercasteDMO.Where(a => a.MI_Id == mastercasteDTO.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }
            return mastercasteDTO;
        }

        public mastercasteDTO GetSelectedRowDetails(int ID)
        {
            mastercasteDTO mastercasteDTO = new mastercasteDTO();

            try
            {
                List<mastercasteDMO> lorg = new List<mastercasteDMO>();
                lorg = _mastercasteContext.mastercasteDMO.Where(t => t.IMC_Id == ID).ToList();
                mastercasteDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }

            return mastercasteDTO;
        }

        public mastercasteDTO MasterDeleteModulesData(int ID)
        {
            mastercasteDTO mastercasteDTO = new mastercasteDTO();
            List<mastercasteDMO> masters = new List<mastercasteDMO>();
            try
            {
                masters = _mastercasteContext.mastercasteDMO.Where(t => t.IMC_Id == ID).ToList();
                if (masters.Any())
                {
                    var check_casteassign = (from a in _mastercasteContext.adm_m_student
                                             from b in _mastercasteContext.mastercasteDMO
                                             where (a.IC_Id == b.IMC_Id && a.IC_Id == ID)
                                             select new mastercasteDTO
                                             {
                                                 IC_Id = b.IMC_Id
                                             }
                                            ).ToList();

                    if (check_casteassign.Count > 0)
                    {
                        mastercasteDTO.msg = "You Can Not Delete The Caste As It Is Already Assigned To Student";
                    }

                    else
                    {
                        _mastercasteContext.Remove(masters.ElementAt(0));
                        var flag = _mastercasteContext.SaveChanges();
                        if (flag > 0)
                        {
                            mastercasteDTO.returnVal = true;
                        }
                        else
                        {
                            mastercasteDTO.returnVal = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }


            return mastercasteDTO;
        }

        public mastercasteDTO mastercasteData(mastercasteDTO mas)
        {

            try
            {
                mastercasteDMO MM = Mapper.Map<mastercasteDMO>(mas);
                if (mas.IC_Id != 0)
                {
                    //&& t.IMCC_Id != mas.IMCC_Id
                    var duplicate = _mastercasteContext.mastercasteDMO.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName  && t.IMC_Id != mas.IC_Id).ToList();

                    // var result_update = _mastercasteContext.mastercasteDMO.Where(t => t.MI_Id == mas.MI_Id || t.IMC_CasteName != mas.IC_CasteName||t.IMC_CasteDesc!=mas.IC_CasteDesc||t.IMCC_Id!=mas.IMCC_Id).Count();

                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {
                        //if (duplicate.FirstOrDefault().IMC_CasteDesc != mas.IC_CasteDesc)
                        //{
                            var result = _mastercasteContext.mastercasteDMO.Single(t => t.MI_Id == mas.MI_Id && t.IMC_Id == mas.IC_Id);

                            result.IMC_CasteName = mas.IC_CasteName;
                            result.IMC_CasteDesc = mas.IC_CasteDesc;
                            result.IMCC_Id = mas.IMCC_Id;
                            result.MI_Id = mas.MI_Id;

                            result.UpdatedDate = DateTime.Now;
                            result.CreatedDate = result.CreatedDate;
                            _mastercasteContext.Update(result);
                            var flag = _mastercasteContext.SaveChanges();
                            if (flag > 0)
                            {
                                mas.returnVal_update = true;
                            }
                            else
                            {
                                mas.returnVal_update = false;
                            }

                        //}
                        //else
                        //{
                        //    mas.msg = "Record Already Exist";
                        //}
                    }


                    //if (duplicate.Count > 0)
                    //{
                    //    if (duplicate.FirstOrDefault().IMC_CasteDesc != mas.IC_CasteDesc)
                    //    {
                    //        var result = _mastercasteContext.mastercasteDMO.Single(t => t.MI_Id == mas.MI_Id && t.IMC_Id == mas.IC_Id);

                    //        result.IMC_CasteName = mas.IC_CasteName;
                    //        result.IMC_CasteDesc = mas.IC_CasteDesc;
                    //        result.IMCC_Id = mas.IMCC_Id;
                    //        result.MI_Id = mas.MI_Id;

                    //        result.UpdatedDate = DateTime.Now;
                    //        result.CreatedDate = result.CreatedDate;
                    //        _mastercasteContext.Update(result);
                    //        var flag = _mastercasteContext.SaveChanges();
                    //        if (flag > 0)
                    //        {
                    //            mas.returnVal_update = true;
                    //        }
                    //        else
                    //        {
                    //            mas.returnVal_update = false;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        mas.msg = "Same Record Already Exists";
                    //    }
                    //}
                    //else
                    //{
                    //    var result = _mastercasteContext.mastercasteDMO.Single(t => t.MI_Id == mas.MI_Id && t.IMC_Id == mas.IC_Id);

                    //    result.IMC_CasteName = mas.IC_CasteName;
                    //    result.IMC_CasteDesc = mas.IC_CasteDesc;
                    //    result.IMCC_Id = mas.IMCC_Id;
                    //    result.MI_Id = mas.MI_Id;
                    //    result.CreatedDate = result.CreatedDate;
                    //    result.UpdatedDate = DateTime.Now;

                    //    _mastercasteContext.Update(result);

                    //    var flag = _mastercasteContext.SaveChanges();

                    //    if (flag > 0)
                    //    {
                    //        mas.returnVal_update = true;
                    //    }
                    //    else
                    //    {
                    //        mas.returnVal_update = false;
                    //    }
                    //}
                }
                else
                {

                    var duplicate_caste_name = _mastercasteContext.mastercasteDMO.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName && t.IMCC_Id != mas.IMCC_Id).ToList();

                    var duplicatecountresult = _mastercasteContext.mastercasteDMO.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName && t.IMCC_Id == mas.IMCC_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_caste_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        mastercasteDMO MM3 = new mastercasteDMO();
                        MM3.IMC_CasteName = mas.IC_CasteName;
                        MM3.IMC_CasteDesc = mas.IC_CasteDesc;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;

                        MM3.IMCC_Id = mas.IMCC_Id;
                        MM3.MI_Id = mas.MI_Id;
                        _mastercasteContext.Add(MM3);
                        var flag = _mastercasteContext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.IC_Id = MM3.IMC_Id;

                            mas.returnVal = true;
                        }
                        else
                        {
                            mas.returnVal = false;
                        }

                    }



                    else
                    {
                        mas.msg = "Record Already Exist";
                    }


                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }
            return mas;
        }
    }
}
