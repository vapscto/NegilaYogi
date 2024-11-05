using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportServiceHub.com.vaps.Services
{
    public class SportMasterDivisionImpl:Interfaces.SportMasterDivisionInterface
    {
        private static ConcurrentDictionary<string, SportMasterDivisionDTO> _login =
       new ConcurrentDictionary<string, SportMasterDivisionDTO>();

        private readonly SportsContext _sportcontext;
        private readonly castecategoryContext _castecategoryContext;
        private readonly ILogger<SportMasterDivisionImpl> _log;


        public SportMasterDivisionImpl(SportsContext sportcontext, ILogger<SportMasterDivisionImpl> log)
        {
            _sportcontext = sportcontext;
            _log = log;
        }

        public SportMasterDivisionDTO GetmastercasteData(SportMasterDivisionDTO data)//int IVRMM_Id
        {

            List<SportMasterDivisionDMO> Allname = new List<SportMasterDivisionDMO>();

            try
            {
                Allname = _sportcontext.SportMasterDivisionDMO.ToList();
                data.mastercastename = Allname.ToArray();


                data.GridviewDetails = (from a in _sportcontext.SportMasterDivisionDMO
                                        where (a.MI_Id == data.MI_Id )
                                        select new SportMasterDivisionDTO
                                        {
                                                    SPCCMD_Id = a.SPCCMD_Id,
                                                     SPCCMD_DivisionName = a.SPCCMD_DivisionName,
                                                     SPCCMD_DivisionDescription=a.SPCCMD_DivisionDescription,
                                                     SPCCMD_ActiveFlag=a.SPCCMD_ActiveFlag,
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
                _log.LogInformation("Master Divison form error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public SportMasterDivisionDTO GetSelectedRowDetails(int ID)
        {
            SportMasterDivisionDTO SportMasterDivisionDTO = new SportMasterDivisionDTO();

            try
            {
                List<SportMasterDivisionDMO> lorg = new List<SportMasterDivisionDMO>();
                lorg = _sportcontext.SportMasterDivisionDMO.Where(t => t.SPCCMD_Id == ID).ToList();
                SportMasterDivisionDTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Divison form error");
                _log.LogDebug(ex.Message);
            }

            return SportMasterDivisionDTO;
        }

        //public SportMasterDivisionDTO MasterDeleteModulesData(int ID)
        //{
        //    SportMasterDivisionDTO SportMasterDivisionDTO = new SportMasterDivisionDTO();
        //    List<SportMasterDivisionDMO> masters = new List<SportMasterDivisionDMO>();
        //    try
        //    {
        //        masters = _sportcontext.SportMasterDivisionDMO.Where(t => t.SPCCMD_Id == ID).ToList();
        //        if (masters.Any())
        //        {
        //            var check_casteassign = (from a in _sportcontext.SportMasterDivisionDMO
        //                                    where ( a.SPCCMD_Id == ID)
        //                                     select new SportMasterDivisionDTO
        //                                     {
        //                                         SPCCMD_Id = a.SPCCMD_Id
        //                                     }
        //                                    ).ToList();

        //            if (check_casteassign.Count > 0)
        //            {
        //                SportMasterDivisionDTO.msg = "You Can Not Delete The Divison As It Is Already Assigned To Student";
        //            }

        //            else
        //            {
        //                _sportcontext.Remove(masters.ElementAt(0));
        //                var flag = _sportcontext.SaveChanges();
        //                if (flag > 0)
        //                {
        //                    SportMasterDivisionDTO.returnVal = true;
        //                }
        //                else
        //                {
        //                    SportMasterDivisionDTO.returnVal = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogInformation("Master Divison form error");
        //        _log.LogDebug(ex.Message);
        //    }


        //    return SportMasterDivisionDTO;
        //}

        public SportMasterDivisionDTO deactivate(SportMasterDivisionDTO dto)
        {
            try
            {
                SportMasterDivisionDTO enq = Mapper.Map<SportMasterDivisionDTO>(dto);

                if (enq.SPCCMD_Id > 0)
                {
                    var check_religinassign = (from a in _sportcontext.SportMasterDivisionDMO
                                               where (a.SPCCMD_Id == dto.SPCCMD_Id)
                                               select new SportMasterDivisionDTO
                                               {
                                                   SPCCMD_Id = a.SPCCMD_Id
                                               }
                                            ).ToList();

                    //if (check_religinassign.Count > 0)
                    //{
                    //    dto.returnVal = true;

                    //    dto.msg = "You Can Not Deactivate This Record It Is Already Mapped With Student";
                    //}
                    //else
                    //{
                        var result = _sportcontext.SportMasterDivisionDMO.Single(t => t.SPCCMD_Id == enq.SPCCMD_Id);
                        if (result.SPCCMD_ActiveFlag == true)
                        {
                            result.SPCCMD_ActiveFlag = false;
                        }
                        else
                        {
                            result.SPCCMD_ActiveFlag = true;
                        }
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _sportcontext.Update(result);
                        var flag = _sportcontext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.returnVal = true;

                            if (result.SPCCMD_ActiveFlag == true)
                            {
                                dto.msg = " Divison Activated Successfully.";
                            }
                            else if (result.SPCCMD_ActiveFlag == false)
                            {
                                dto.msg = " Divison Deactivated Successfully.";
                            }
                        }
                        else
                        {
                            dto.returnVal = false;
                        }
                        List<SportMasterDivisionDMO> Allname = new List<SportMasterDivisionDMO>();
                        Allname = _sportcontext.SportMasterDivisionDMO.ToList();
                        dto.mastercastename = Allname.ToArray();
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

        public SportMasterDivisionDTO mastercasteData(SportMasterDivisionDTO mas)
        {

            try
            {
                SportMasterDivisionDMO MM = Mapper.Map<SportMasterDivisionDMO>(mas);
                if (mas.SPCCMD_Id != 0)
                {
                   
                    var duplicate = _sportcontext.SportMasterDivisionDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMD_DivisionName == mas.SPCCMD_DivisionName && t.SPCCMD_Id != mas.SPCCMD_Id).ToList();

                   

                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {
                        
                        var result = _sportcontext.SportMasterDivisionDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMD_Id == mas.SPCCMD_Id);

                        result.SPCCMD_DivisionName = mas.SPCCMD_DivisionName;
                        result.SPCCMD_DivisionDescription = mas.SPCCMD_DivisionDescription;
                        result.SPCCMD_Id = mas.SPCCMD_Id;
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

                    var duplicate_caste_name = _sportcontext.SportMasterDivisionDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMD_DivisionName == mas.SPCCMD_DivisionName && t.SPCCMD_Id != mas.SPCCMD_Id).ToList();

                    var duplicatecountresult = _sportcontext.SportMasterDivisionDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMD_DivisionName == mas.SPCCMD_DivisionName && t.SPCCMD_Id != mas.SPCCMD_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_caste_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        SportMasterDivisionDMO MM3 = new SportMasterDivisionDMO();
                        MM3.SPCCMD_DivisionName = mas.SPCCMD_DivisionName;
                        MM3.SPCCMD_DivisionDescription = mas.SPCCMD_DivisionDescription;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMD_ActiveFlag = true;
                        MM3.SPCCMD_Id = mas.SPCCMD_Id;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMD_Id = MM3.SPCCMD_Id;

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
                _log.LogInformation("Master  Divison form error");
                _log.LogDebug(ex.Message);
            }
            return mas;
        }
    }
}
