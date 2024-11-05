using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegemastercasteImpl :Interface.CollegemastercasteInterface
    {
        private static ConcurrentDictionary<string, CollegemastercasteDTO> _login =
         new ConcurrentDictionary<string, CollegemastercasteDTO>();

        public ClgAdmissionContext _mastercasteContext;        
        private readonly ILogger<CollegemastercasteImpl> _log;


        public CollegemastercasteImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<CollegemastercasteImpl> log)
        {
            _mastercasteContext = ClgAdmissionContext;
            _log = log;
        }

        public CollegemastercasteDTO GetmastercasteData(CollegemastercasteDTO CollegemastercasteDTO)//int IVRMM_Id
        {

            List<CollegecastecaegoryDMO> Allname = new List<CollegecastecaegoryDMO>();

            try
            {
                Allname = _mastercasteContext.CasteCategory.ToList();
                CollegemastercasteDTO.mastercastename = Allname.ToArray();


                CollegemastercasteDTO.GridviewDetails = (from sp in _mastercasteContext.Caste
                                                  from cp in _mastercasteContext.CasteCategory
                                                  where (sp.IMCC_Id == cp.IMCC_Id && sp.MI_Id == CollegemastercasteDTO.MI_Id)
                                                  select new CollegemastercasteDTO
                                                  {
                                                      IC_Id = sp.IMC_Id,
                                                      IC_CasteName = sp.IMC_CasteName,
                                                      IC_CasteDesc = sp.IMC_CasteDesc,
                                                      CategoryName = cp.IMCC_CategoryName,
                                                      IMCC_Id = sp.IMCC_Id,
                                                      MI_Id = sp.MI_Id
                                                  }).OrderByDescending(a => a.IC_Id).ToArray();
                if (CollegemastercasteDTO.GridviewDetails.Length > 0)
                {
                    CollegemastercasteDTO.count = CollegemastercasteDTO.GridviewDetails.Length;
                }
                else
                {
                    CollegemastercasteDTO.count = 0;
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }
            return CollegemastercasteDTO;
        }

        public CollegemastercasteDTO GetSelectedRowDetails(int ID)
        {
            CollegemastercasteDTO CollegemastercasteDTO = new CollegemastercasteDTO();

            try
            {
                List<CollegemastercasteDMO> lorg = new List<CollegemastercasteDMO>();
                lorg = _mastercasteContext.Caste.Where(t => t.IMC_Id == ID).ToList();
                CollegemastercasteDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }

            return CollegemastercasteDTO;
        }

        public CollegemastercasteDTO MasterDeleteModulesData(int ID)
        {
            CollegemastercasteDTO CollegemastercasteDTO = new CollegemastercasteDTO();
            List<CollegemastercasteDMO> masters = new List<CollegemastercasteDMO>();
            try
            {
                masters = _mastercasteContext.Caste.Where(t => t.IMC_Id == ID).ToList();
                if (masters.Any())
                {
                    var check_casteassign = (from a in _mastercasteContext.Adm_Master_College_StudentDMO
                                             from b in _mastercasteContext.Caste
                                             where (a.IMC_Id == b.IMC_Id && a.IMC_Id == ID)
                                             select new CollegemastercasteDTO
                                             {
                                                 IMC_ID = b.IMC_Id
                                             }
                                            ).ToList();

                    if (check_casteassign.Count > 0)
                    {
                        CollegemastercasteDTO.msg = "You Can Not Delete The Caste As It Is Already Assigned To Student";
                    }

                    else
                    {
                        _mastercasteContext.Remove(masters.ElementAt(0));
                        var flag = _mastercasteContext.SaveChanges();
                        if (flag > 0)
                        {
                            CollegemastercasteDTO.returnVal = true;
                        }
                        else
                        {
                            CollegemastercasteDTO.returnVal = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
               // CollegemastercasteDTO.msg = "You Can Not Delete The Caste As It Is Already Assigned To Student";
                _log.LogDebug(ex.Message);
            }

            return CollegemastercasteDTO;
        }

        public CollegemastercasteDTO mastercasteData(CollegemastercasteDTO mas)
        {
            try
            {
                CollegemastercasteDMO MM = Mapper.Map<CollegemastercasteDMO>(mas);
                if (mas.IC_Id != 0)
                {                 
                    var duplicate = _mastercasteContext.Caste.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName && t.IMC_Id != mas.IC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {                        
                        var result = _mastercasteContext.Caste.Single(t => t.MI_Id == mas.MI_Id && t.IMC_Id == mas.IC_Id);

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
                    }
                   
                }
                else
                {
                    var duplicate_caste_name = _mastercasteContext.Caste.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName && t.IMCC_Id != mas.IMCC_Id).ToList();

                    var duplicatecountresult = _mastercasteContext.Caste.Where(t => t.MI_Id == mas.MI_Id && t.IMC_CasteName == mas.IC_CasteName && t.IMCC_Id == mas.IMCC_Id).Count();

                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_caste_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        CollegemastercasteDMO MM3 = new CollegemastercasteDMO();
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
