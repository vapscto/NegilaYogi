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
    public class SportMasterHouseImpl : Interfaces.SportsMasterHouseInterface
    {
     
        private readonly SportsContext _sportcontext;
       


        public SportMasterHouseImpl(SportsContext sportcontext)
        {
            _sportcontext = sportcontext;
          
        }

        public SportsMasterHouse_DTO getdetails(SportsMasterHouse_DTO data)//int IVRMM_Id
        {
            
            try
            {
              
                data.gridviewDetails = (from a in _sportcontext.SportMasterHouseDMO
                                        where (a.MI_Id == data.MI_Id)
                                        select new SportsMasterHouse_DTO
                                        {
                                            SPCCMH_Id = a.SPCCMH_Id,
                                            SPCCMH_HouseName = a.SPCCMH_HouseName,
                                            SPCCMH_HouseDescription = a.SPCCMH_HouseDescription,
                                            SPCCMH_ActiveFlag = a.SPCCMH_ActiveFlag,
                                            SPCCMH_Flag = a.SPCCMH_Flag,
                                        }).Distinct().OrderBy(t=>t.SPCCMH_Id).ToArray();

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SportsMasterHouse_DTO savedata(SportsMasterHouse_DTO mas)
        {

            try
            {
             
                if (mas.SPCCMH_Id != 0)
                {

                    var duplicate = _sportcontext.SportMasterHouseDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMH_HouseName == mas.SPCCMH_HouseName && t.SPCCMH_Id != mas.SPCCMH_Id).ToList();



                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {

                        var result = _sportcontext.SportMasterHouseDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMH_Id == mas.SPCCMH_Id);

                        result.SPCCMH_HouseName = mas.SPCCMH_HouseName;
                        result.SPCCMH_HouseDescription = mas.SPCCMH_HouseDescription;
                        result.SPCCMH_Id = mas.SPCCMH_Id;
                        result.MI_Id = mas.MI_Id;
                        result.SPCCMH_Flag = mas.SPCCMH_Flag;
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

                    var duplicate_caste_name = _sportcontext.SportMasterHouseDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMH_HouseName == mas.SPCCMH_HouseName && t.SPCCMH_Id != mas.SPCCMH_Id).ToList();

                    var duplicatecountresult = _sportcontext.SportMasterHouseDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMH_HouseName == mas.SPCCMH_HouseName && t.SPCCMH_Id != mas.SPCCMH_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_caste_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        SportMasterHouseDMO MM3 = new SportMasterHouseDMO();
                        MM3.SPCCMH_HouseName = mas.SPCCMH_HouseName;
                        MM3.SPCCMH_HouseDescription = mas.SPCCMH_HouseDescription;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMH_ActiveFlag = true;
                        MM3.SPCCMH_Id = mas.SPCCMH_Id;
                        MM3.SPCCMH_Flag = mas.SPCCMH_Flag;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMH_Id = MM3.SPCCMH_Id;

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
                Console.WriteLine(ex.Message);
            }
            return mas;
        }

        public SportsMasterHouse_DTO editdata(SportsMasterHouse_DTO data)
        {
            

            try
            {
               data.editdetails=_sportcontext.SportMasterHouseDMO.Where(t=>t.MI_Id==data.MI_Id && t.SPCCMH_Id==data.SPCCMH_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public SportsMasterHouse_DTO deactivate(SportsMasterHouse_DTO dto)
        {
            try
            {
                if (dto.SPCCMH_Id > 0)
                {
                    var check_religinassign = (from a in _sportcontext.SportMasterHouseDMO
                                               where (a.SPCCMH_Id == dto.SPCCMH_Id && a.MI_Id==dto.MI_Id)
                                               select new SportsMasterHouse_DTO
                                               {
                                                   SPCCMH_Id = a.SPCCMH_Id
                                               }
                                            ).ToList();                  
                    var result = _sportcontext.SportMasterHouseDMO.Single(t => t.SPCCMH_Id == dto.SPCCMH_Id && t.MI_Id == dto.MI_Id);
                    if (result.SPCCMH_ActiveFlag == true)
                    {
                        result.SPCCMH_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCMH_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _sportcontext.Update(result);
                    var flag = _sportcontext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.returnVal = true;

                        if (result.SPCCMH_ActiveFlag == true)
                        {
                            dto.msg = "House Activated Successfully.";
                        }
                        else if (result.SPCCMH_ActiveFlag == false)
                        {
                            dto.msg = "House Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        dto.returnVal = false;
                    }
                   
                }
           
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

     


    }
}
