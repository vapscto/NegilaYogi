using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class MasterSportsCCGroupImpl:Interfaces.MasterSportsCCGroupInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterSportsCCGroupImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterSportsCCGroupDTO getDetails(MasterSportsCCGroupDTO data)
        {
            try
            {
                var category = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                if (category.Count > 0)
                {
                    data.groupNameList = category.ToArray();
                    data.count = category.Count;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public MasterSportsCCGroupDTO saveRecord(MasterSportsCCGroupDTO obj)
        {
            try
            {
                if (obj.SPCCMSCCG_Id == 0)
                {
                    var checkduplicate = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_SportsCCGroupName.Equals(obj.SPCCMSCCG_SportsCCGroupName)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterSportsCCGroupDMO>(obj);
                        mapp.SPCCMSCCG_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        _context.Add(mapp);
                        int s = _context.SaveChanges();
                        
                        if (s > 0)
                        {
                            obj.returnVal = "saved";
                            if (obj.tempDatas != null && obj.tempDatas.Length > 0)
                            {
                               //long SPCCMSCCG_Id = _context.MasterSportsCCGroupDMO.Where(R=>R.MI_Id==obj.MI_Id).Distinct().OrderByDescending(d => d.SPCCMSCCG_Id).Take(1).FirstOrDefault().SPCCMSCCG_Id;
                               // foreach ( var d  in obj.tempDatas)
                               // {
                               //     MasterSportsCCGroupDMO objp = new MasterSportsCCGroupDMO();
                               //     objp.MI_Id = obj.MI_Id;
                               //     objp.SPCCMSCCG_SportsCCGroupName = d.SPCCMSCCG_SportsCCGroupName;
                               //     objp.SPCCMSCCG_SportsCCGroupDesc = d.SPCCMSCCG_SportsCCGroupDesc;
                               //     objp.SPCCMSCCG_SCCFlag = obj.SPCCMSCCG_SCCFlag;
                               //     objp.SPCCMSCCG_ActiveFlag = true;
                               //     objp.CreatedDate = DateTime.Now;
                               //     objp.UpdatedDate = DateTime.Now;
                               //     objp.SPCCMSCCG_Under = SPCCMSCCG_Id;                                    
                               //     objp.SPCCMSCCG_Level = d.SPCCMSCCG_Level;                                    
                               //     _context.Add(objp);

                               // }

                               // int O = _context.SaveChanges();
                            }
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }
                else if (obj.SPCCMSCCG_Id > 0)
                {
                    var checkduplicate = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_SportsCCGroupName.Equals(obj.SPCCMSCCG_SportsCCGroupName) && d.SPCCMSCCG_Id != obj.SPCCMSCCG_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterSportsCCGroupDMO.Single(d => d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMSCCG_SCCFlag = obj.SPCCMSCCG_SCCFlag;
                            update.SPCCMSCCG_SportsCCGroupDesc = obj.SPCCMSCCG_SportsCCGroupDesc;
                            update.SPCCMSCCG_SportsCCGroupName = obj.SPCCMSCCG_SportsCCGroupName;
                            _context.Update(update);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "updated";
                            }
                            else
                            {
                                obj.returnVal = "updateFailed";
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public MasterSportsCCGroupDTO EditDetails(int id)
        {
            MasterSportsCCGroupDTO resp = new MasterSportsCCGroupDTO();
            try
            {
                resp.editDetails = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterSportsCCGroupDTO deactivate(MasterSportsCCGroupDTO obj)
        {
            try
            {
               


                var result = _context.MasterSportsCCGroupDMO.Single(t => t.MI_Id == obj.MI_Id && t.SPCCMSCCG_Id == obj.SPCCMSCCG_Id);

                if (result.SPCCMSCCG_ActiveFlag == true)
                {
                    result.SPCCMSCCG_ActiveFlag = false;
                }
                else if (result.SPCCMSCCG_ActiveFlag == false)
                {
                    result.SPCCMSCCG_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                int rowAffected = _context.SaveChanges();
                if (rowAffected > 0)
                {
                    obj.returnval = true;
                }
                else
                {
                    obj.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
