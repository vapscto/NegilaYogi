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
    public class SportMasterCompitionLevelImpl:Interfaces.SportMasterCompitionLevelInterface
    {
        private static ConcurrentDictionary<string, SportMasterCompitionLevelDTO> _login =
      new ConcurrentDictionary<string, SportMasterCompitionLevelDTO>();

        private readonly SportsContext _sportcontext;
        private readonly castecategoryContext _castecategoryContext;
        private readonly ILogger<SportMasterCompitionLevelImpl> _log;


        public SportMasterCompitionLevelImpl(SportsContext sportcontext, ILogger<SportMasterCompitionLevelImpl> log)
        {
            _sportcontext = sportcontext;
            _log = log;
        }

        public SportMasterCompitionLevelDTO GetmastercasteData(SportMasterCompitionLevelDTO data)//int IVRMM_Id
        {

            List<SportMasterCompitionLevelDMO> Allname = new List<SportMasterCompitionLevelDMO>();

            try
            {
                Allname = _sportcontext.SportMasterCompitionLevelDMO.ToList();
                data.mastercompitionname = Allname.ToArray();


                data.GridviewDetails = (from a in _sportcontext.SportMasterCompitionLevelDMO
                                        where (a.MI_Id == data.MI_Id)
                                        select new SportMasterCompitionLevelDTO
                                        {
                                            SPCCMCL_Id = a.SPCCMCL_Id,
                                            SPCCMCL_CompitionLevel = a.SPCCMCL_CompitionLevel,
                                            SPCCMCL_CLDesc = a.SPCCMCL_CLDesc,
                                            SPCCMCL_ActiveFlag = a.SPCCMCL_ActiveFlag,
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
                _log.LogInformation("Master Compition Level form error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public SportMasterCompitionLevelDTO GetSelectedRowDetails(int ID)
        {
            SportMasterCompitionLevelDTO SportMasterCompitionLevelDTO = new SportMasterCompitionLevelDTO();

            try
            {
                List<SportMasterCompitionLevelDMO> lorg = new List<SportMasterCompitionLevelDMO>();
                lorg = _sportcontext.SportMasterCompitionLevelDMO.Where(t => t.SPCCMCL_Id == ID).ToList();
                SportMasterCompitionLevelDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Compition Level form error");
                _log.LogDebug(ex.Message);
            }

            return SportMasterCompitionLevelDTO;
        }

       

        public SportMasterCompitionLevelDTO deactivate(SportMasterCompitionLevelDTO dto)
        {
            try
            {
                SportMasterCompitionLevelDTO enq = Mapper.Map<SportMasterCompitionLevelDTO>(dto);

                if (enq.SPCCMCL_Id > 0)
                {
                    var check_religinassign = (from a in _sportcontext.SportMasterCompitionLevelDMO
                                               where (a.SPCCMCL_Id == dto.SPCCMCL_Id)
                                               select new SportMasterCompitionLevelDTO
                                               {
                                                   SPCCMCL_Id = a.SPCCMCL_Id
                                               }
                                            ).ToList();

                    //if (check_religinassign.Count > 0)
                    //{
                    //    dto.returnVal = true;

                    //    dto.msg = "You Can Not Deactivate This Record It Is Already Mapped With Student";
                    //}
                    //else
                    //{
                    var result = _sportcontext.SportMasterCompitionLevelDMO.Single(t => t.SPCCMCL_Id == enq.SPCCMCL_Id);
                    if (result.SPCCMCL_ActiveFlag == true)
                    {
                        result.SPCCMCL_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCMCL_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _sportcontext.Update(result);
                    var flag = _sportcontext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.returnVal = true;

                        if (result.SPCCMCL_ActiveFlag == true)
                        {
                            dto.msg = "Activated Successfully.";
                        }
                        else if (result.SPCCMCL_ActiveFlag == false)
                        {
                            dto.msg = "Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        dto.returnVal = false;
                    }
                    List<SportMasterCompitionLevelDMO> Allname = new List<SportMasterCompitionLevelDMO>();
                    Allname = _sportcontext.SportMasterCompitionLevelDMO.ToList();
                    dto.mastercompitionname = Allname.ToArray();
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

        public SportMasterCompitionLevelDTO mastercasteData(SportMasterCompitionLevelDTO mas)
        {

            try
            {
                SportMasterCompitionLevelDMO MM = Mapper.Map<SportMasterCompitionLevelDMO>(mas);
                if (mas.SPCCMCL_Id != 0)
                {

                    var duplicate = _sportcontext.SportMasterCompitionLevelDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMCL_CompitionLevel == mas.SPCCMCL_CompitionLevel && t.SPCCMCL_Id != mas.SPCCMCL_Id).ToList();



                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {

                        var result = _sportcontext.SportMasterCompitionLevelDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMCL_Id == mas.SPCCMCL_Id);

                        result.SPCCMCL_CompitionLevel = mas.SPCCMCL_CompitionLevel;
                        result.SPCCMCL_CLDesc = mas.SPCCMCL_CLDesc;
                        result.SPCCMCL_Id = mas.SPCCMCL_Id;
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

                    var duplicate_caste_name = _sportcontext.SportMasterCompitionLevelDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMCL_CompitionLevel == mas.SPCCMCL_CompitionLevel && t.SPCCMCL_Id != mas.SPCCMCL_Id).ToList();

                    var duplicatecountresult = _sportcontext.SportMasterCompitionLevelDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMCL_CompitionLevel == mas.SPCCMCL_CompitionLevel && t.SPCCMCL_Id != mas.SPCCMCL_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_compition_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        SportMasterCompitionLevelDMO MM3 = new SportMasterCompitionLevelDMO();
                        MM3.SPCCMCL_CompitionLevel = mas.SPCCMCL_CompitionLevel;
                        MM3.SPCCMCL_CLDesc = mas.SPCCMCL_CLDesc;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMCL_ActiveFlag = true;
                        MM3.SPCCMCL_Id = mas.SPCCMCL_Id;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMCL_Id = MM3.SPCCMCL_Id;

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
                _log.LogInformation("Master Compition Level form error");
                _log.LogDebug(ex.Message);
            }
            return mas;
        }
    }
}
