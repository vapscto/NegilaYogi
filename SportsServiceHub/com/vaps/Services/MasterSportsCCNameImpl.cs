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
    public class MasterSportsCCNameImpl : Interfaces.MasterSportsCCNameInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public MasterSportsCCNameImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public MasterSportsCCNameDTO getDetails(MasterSportsCCNameDTO data)
        {
            try
            {
                var groupNameList = _context.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true &&  d.SPCCMSCCG_Under == 0).ToList();
                if (groupNameList.Count > 0)
                {
                    data.gropList = groupNameList.ToArray();
                }
                var sportsccName = (from m in _context.MasterSportsCCNameDMO
                                    from n in _context.MasterSportsCCGroupDMO
                                    where m.SPCCMSCCG_Id == n.SPCCMSCCG_Id && m.MI_Id == data.MI_Id
                                    select new MasterSportsCCNameDTO
                                    {
                                        SPCCMSCCG_Id = m.SPCCMSCCG_Id,
                                        SPCCMSCC_Id = m.SPCCMSCC_Id,
                                        SPCCMSCC_SportsCCName = m.SPCCMSCC_SportsCCName,
                                        SPCCMSCC_SportsCCDesc = m.SPCCMSCC_SportsCCDesc,
                                        SPCCMSCC_SGFlag = m.SPCCMSCC_SGFlag,
                                        SPCCMSCC_NoOfMembers = m.SPCCMSCC_NoOfMembers,
                                        SPCCMSCC_RecHighLowFlag = m.SPCCMSCC_RecHighLowFlag,
                                        SPCCMSCC_RecInfo = m.SPCCMSCC_RecInfo,
                                        SPCCMSCC_ActiveFlag = m.SPCCMSCC_ActiveFlag,
                                        groupName = n.SPCCMSCCG_SportsCCGroupName
                                    }
                                    ).OrderByDescending(r => r.SPCCMSCC_Id).ToList();
                if (sportsccName.Count > 0)
                {
                    data.sportsCCNameList = sportsccName.ToArray();
                    data.count = sportsccName.Count;
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
        public MasterSportsCCNameDTO saveRecord(MasterSportsCCNameDTO obj)
        {
            try
            {
                if (obj.SPCCMSCC_Id == 0)
                {
                    var checkduplicate = _context.MasterSportsCCNameDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && d.SPCCMSCC_SportsCCName.Equals(obj.SPCCMSCC_SportsCCName)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<MasterSportsCCNameDMO>(obj);
                        mapp.SPCCMSCC_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        mapp.SPCCMSCC_MultiAttemptFlg = obj.SPCCMSCC_MultiAttemptFlg;
                        mapp.SPCCMSCC_NoOfAttempts = obj.SPCCMSCC_NoOfAttempts;
                        _context.Add(mapp);
                      
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {

                            obj.returnVal = "saved";

                            if (obj.tempDatas != null && obj.tempDatas.Length > 0)
                            {
                                var MasterSportsCCName = _context.MasterSportsCCNameDMO.Where(R => R.MI_Id == obj.MI_Id).OrderByDescending(R => R.SPCCMSCC_Id).Distinct().ToList();
                                var sportEvent = _context.MasterSportsCCGroupDMO.Where(R => R.SPCCMSCCG_Id == obj.SPCCMSCCG_Id).FirstOrDefault();
                                var SPCCMSCCG_Level = _context.MasterSportsCCGroupDMO.Where(R => R.SPCCMSCCG_Under == obj.SPCCMSCCG_Id).ToList();
                                foreach (var d in obj.tempDatas)
                                {
                                    MasterSportsCCGroupDMO objp = new MasterSportsCCGroupDMO();
                                    objp.MI_Id = obj.MI_Id;
                                    objp.SPCCMSCCG_SportsCCGroupName = d.SPCCMSCCG_UnderEvent;
                                    objp.SPCCMSCCG_SportsCCGroupDesc = obj.SPCCMSCC_SportsCCName;
                                    objp.SPCCMSCCG_SCCFlag = sportEvent.SPCCMSCCG_SCCFlag;
                                    objp.SPCCMSCCG_ActiveFlag = true;
                                    objp.CreatedDate = DateTime.Now;
                                    objp.UpdatedDate = DateTime.Now;
                                    objp.SPCCMSCCG_Under = sportEvent.SPCCMSCCG_Id;
                                    // objp.SPCCMSCCG_Level = SPCCMSCCG_Level.Count + 1;
                                    objp.SPCCMSCCG_Level = MasterSportsCCName.FirstOrDefault().SPCCMSCC_Id;
                                    _context.Add(objp);
                                }
                                int sl = _context.SaveChanges();

                            }

                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }
                else if (obj.SPCCMSCC_Id > 0)
                {
                    var checkduplicate = _context.MasterSportsCCNameDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCMSCCG_Id == obj.SPCCMSCCG_Id && d.SPCCMSCC_SportsCCName.Equals(obj.SPCCMSCC_SportsCCName) && d.SPCCMSCC_Id != obj.SPCCMSCC_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.MasterSportsCCNameDMO.Where(d => d.SPCCMSCC_Id == obj.SPCCMSCC_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.MasterSportsCCNameDMO.Single(d => d.SPCCMSCC_Id == obj.SPCCMSCC_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCMSCCG_Id = obj.SPCCMSCCG_Id;
                            update.SPCCMSCC_NoOfMembers = obj.SPCCMSCC_NoOfMembers;
                            update.SPCCMSCC_RecHighLowFlag = obj.SPCCMSCC_RecHighLowFlag;
                            update.SPCCMSCC_RecInfo = obj.SPCCMSCC_RecInfo;
                            update.SPCCMSCC_SGFlag = obj.SPCCMSCC_SGFlag;
                            update.SPCCMSCC_SportsCCDesc = obj.SPCCMSCC_SportsCCDesc;
                            update.SPCCMSCC_SportsCCName = obj.SPCCMSCC_SportsCCName;
                            update.SPCCMSCC_MultiAttemptFlg = obj.SPCCMSCC_MultiAttemptFlg;
                            update.SPCCMSCC_NoOfAttempts = obj.SPCCMSCC_NoOfAttempts;
                            _context.Update(update);
                            int s = _context.SaveChanges();
                            if (s > 0)
                            {
                                obj.returnVal = "updated";
                                if (obj.tempDatas != null && obj.tempDatas.Length > 0)
                                {
                                    // long SPCCMSCCG_Level = 0;
                                    var sportEvent = _context.MasterSportsCCGroupDMO.Where(R => R.SPCCMSCCG_Id == obj.SPCCMSCCG_Id).FirstOrDefault();
                                    var SPCCMSCCG_Level = _context.MasterSportsCCGroupDMO.Where(R => R.SPCCMSCCG_Under == obj.SPCCMSCCG_Id).ToList();

                                    foreach (var d in obj.tempDatas)
                                    {                                        
                                        if (d.SPCCMSCCG_Id > 0)
                                        {
                                            var Updates = _context.MasterSportsCCGroupDMO.Where(R => R.SPCCMSCCG_Id == d.SPCCMSCCG_Id).FirstOrDefault();
                                            Updates.SPCCMSCCG_SportsCCGroupName = d.SPCCMSCCG_UnderEvent;
                                            Updates.UpdatedDate = DateTime.Now;
                                            _context.Update(Updates);
                                        }
                                        else
                                        {
                                            MasterSportsCCGroupDMO objp = new MasterSportsCCGroupDMO();
                                            objp.MI_Id = obj.MI_Id;
                                            objp.SPCCMSCCG_SportsCCGroupName = d.SPCCMSCCG_UnderEvent;
                                            objp.SPCCMSCCG_SportsCCGroupDesc = obj.SPCCMSCC_SportsCCName;
                                            objp.SPCCMSCCG_SCCFlag = sportEvent.SPCCMSCCG_SCCFlag;
                                            objp.SPCCMSCCG_ActiveFlag = true;
                                            objp.CreatedDate = DateTime.Now;
                                            objp.UpdatedDate = DateTime.Now;
                                            objp.SPCCMSCCG_Under = sportEvent.SPCCMSCCG_Id;
                                            // objp.SPCCMSCCG_Level = SPCCMSCCG_Level.Count + 1;
                                            objp.SPCCMSCCG_Level = obj.SPCCMSCC_Id;
                                            _context.Add(objp);
                                        }

                                    }
                                    int u = _context.SaveChanges();
                                }

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
        public MasterSportsCCNameDTO EditDetails(int id)
        {
            MasterSportsCCNameDTO resp = new MasterSportsCCNameDTO();
            try
            {
                resp.editDetails = _context.MasterSportsCCNameDMO.Where(d => d.SPCCMSCC_Id == id).Distinct().ToArray();
                var editDetails = _context.MasterSportsCCNameDMO.Where(d => d.SPCCMSCC_Id == id).Distinct().ToList();
                resp.gropList = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_Under == id && d.SPCCMSCCG_ActiveFlag == true).Distinct().ToArray();
                //SPCC_Master_SportsCCName
               // resp.editsubevent = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == editDetails.FirstOrDefault().SPCCMSCCG_Id).Distinct().ToArray();
                resp.editsubevent = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_Under == editDetails.FirstOrDefault().SPCCMSCCG_Id && d.SPCCMSCCG_Level== id).Distinct().ToArray();
                // resp.editsubevent = _context.MasterSportsCCGroupDMO.Where(d => d.SPCCMSCCG_Under == d.SPCCMSCCG_Id).Distinct().ToArray();         
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public MasterSportsCCNameDTO deactivate(MasterSportsCCNameDTO obj)
        {
            try
            {
                var query = _context.MasterSportsCCNameDMO.Single(d => d.SPCCMSCC_Id == obj.SPCCMSCC_Id);

                if (query.SPCCMSCC_ActiveFlag == true)
                {
                    query.SPCCMSCC_ActiveFlag = false;
                }
                else if (query.SPCCMSCC_ActiveFlag == false)
                {
                    query.SPCCMSCC_ActiveFlag = true;
                }

                query.UpdatedDate = DateTime.Now;
                _context.Update(query);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    obj.retval = true;
                }

                else
                {
                    obj.retval = false;

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
