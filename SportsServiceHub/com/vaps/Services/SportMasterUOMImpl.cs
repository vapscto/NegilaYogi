using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportMasterUOMImpl:Interfaces.SportMasterUOMInterface
    {
        private static ConcurrentDictionary<string, SportMasterUOMDTO> _login =
    new ConcurrentDictionary<string, SportMasterUOMDTO>();

        private readonly SportsContext _sportcontext;
        private readonly castecategoryContext _castecategoryContext;
        private readonly ILogger<SportMasterUOMImpl> _log;


        public SportMasterUOMImpl(SportsContext sportcontext, ILogger<SportMasterUOMImpl> log)
        {
            _sportcontext = sportcontext;
            _log = log;
        }

        public SportMasterUOMDTO GetmastercasteData(SportMasterUOMDTO data)//int IVRMM_Id
        {

            List<SportMasterUOMDMO> Allname = new List<SportMasterUOMDMO>();

            try
            {
                Allname = _sportcontext.SportMasterUOMDMO.ToList();
                data.masterUOMname = Allname.ToArray();


                data.GridviewDetails = (from a in _sportcontext.SportMasterUOMDMO
                                        where (a.MI_Id == data.MI_Id)
                                        select new SportMasterUOMDTO
                                        {
                                            SPCCMUOM_Id = a.SPCCMUOM_Id,
                                            SPCCMUOM_UOMName = a.SPCCMUOM_UOMName,
                                            SPCCMUOM_ActiveFlag = a.SPCCMUOM_ActiveFlag,
                                        }
                                                  ).Distinct().ToArray();

                if (data.GridviewDetails.Length > 0)
                {
                    data.count = data.GridviewDetails.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public SportMasterUOMDTO GetSelectedRowDetails(int ID)
        {
            SportMasterUOMDTO SportMasterUOMDTO = new SportMasterUOMDTO();

            try
            {
                List<SportMasterUOMDMO> lorg = new List<SportMasterUOMDMO>();
                lorg = _sportcontext.SportMasterUOMDMO.Where(t => t.SPCCMUOM_Id == ID).ToList();
                SportMasterUOMDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }

            return SportMasterUOMDTO;
        }

        public SportMasterUOMDTO deactivate(SportMasterUOMDTO dto)
        {
            try
            {
                SportMasterUOMDTO enq = Mapper.Map<SportMasterUOMDTO>(dto);

                if (enq.SPCCMUOM_Id > 0)
                {
                    var check_religinassign = (from a in _sportcontext.SportMasterUOMDMO
                                               where (a.SPCCMUOM_Id == dto.SPCCMUOM_Id)
                                               select new SportMasterUOMDTO
                                               {
                                                   SPCCMUOM_Id = a.SPCCMUOM_Id
                                               }
                                            ).ToList();

                    //if (check_religinassign.Count > 0)
                    //{
                    //    dto.returnVal = true;

                    //    dto.msg = "You Can Not Deactivate This Record It Is Already Mapped With Student";
                    //}
                    //else
                    //{
                    var result = _sportcontext.SportMasterUOMDMO.Single(t => t.SPCCMUOM_Id == enq.SPCCMUOM_Id);
                    if (result.SPCCMUOM_ActiveFlag == true)
                    {
                        result.SPCCMUOM_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCMUOM_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _sportcontext.Update(result);
                    var flag = _sportcontext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.returnVal = true;

                        if (result.SPCCMUOM_ActiveFlag == true)
                        {
                            dto.msg = "UOM Activated Successfully.";
                        }
                        else if (result.SPCCMUOM_ActiveFlag == false)
                        {
                            dto.msg = "UOM Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        dto.returnVal = false;
                    }
                    List<SportMasterUOMDMO> Allname = new List<SportMasterUOMDMO>();
                    Allname = _sportcontext.SportMasterUOMDMO.ToList();
                    dto.masterUOMname = Allname.ToArray();
                }
                //  }
            }
            catch (Exception ee)
            {
                _log.LogTrace(ee.Message);
                _log.LogDebug(ee.Message);
                _log.LogError(ee.Message);
            }
            return dto;
        }

        public SportMasterUOMDTO mastercasteData(SportMasterUOMDTO mas)
        {

            try
            {
                SportMasterUOMDMO MM = Mapper.Map<SportMasterUOMDMO>(mas);
                if (mas.SPCCMUOM_Id != 0)
                {

                    var duplicate = _sportcontext.SportMasterUOMDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMUOM_UOMName == mas.SPCCMUOM_UOMName && t.SPCCMUOM_Id != mas.SPCCMUOM_Id).ToList();



                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {

                        var result = _sportcontext.SportMasterUOMDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMUOM_Id == mas.SPCCMUOM_Id);

                        result.SPCCMUOM_UOMName = mas.SPCCMUOM_UOMName;
                        result.SPCCMUOM_Id = mas.SPCCMUOM_Id;
                        result.MI_Id = mas.MI_Id;

                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        _sportcontext.Update(result);
                        var flag = _sportcontext.SaveChanges();
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

                    var duplicate_caste_name = _sportcontext.SportMasterUOMDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMUOM_UOMName == mas.SPCCMUOM_UOMName && t.SPCCMUOM_Id != mas.SPCCMUOM_Id).ToList();

                    var duplicatecountresult = _sportcontext.SportMasterUOMDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMUOM_UOMName == mas.SPCCMUOM_UOMName && t.SPCCMUOM_Id != mas.SPCCMUOM_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_UOM_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        SportMasterUOMDMO MM3 = new SportMasterUOMDMO();
                        MM3.SPCCMUOM_UOMName = mas.SPCCMUOM_UOMName;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMUOM_ActiveFlag = true;
                        MM3.SPCCMUOM_Id = mas.SPCCMUOM_Id;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMUOM_Id = MM3.SPCCMUOM_Id;

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
                _log.LogInformation("Master UOM form error");
                _log.LogDebug(ex.Message);
            }
            return mas;
        }
    }
}
