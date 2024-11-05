using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class MasterActivityGroupHeadImpl : interfaces.MasterActivityGroupHeadInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public MasterActivityGroupHeadImpl(FeeGroupContext hh)
        {
            _FeeGroupContext = hh;
        }
        //======================================loaddata=================================================//
        public Adm_Master_ActivitiesDTO loaddata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                data.group_list = _FeeGroupContext.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_ActiceFlag == true).Distinct().ToArray();

                data.get_masterlist = (from a in _FeeGroupContext.Adm_Master_Activities
                                       from b in _FeeGroupContext.FeeGroupDMO
                                       from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from d in _FeeGroupContext.FeeHeadDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && c.FMH_Id == d.FMH_Id && a.MI_Id == data.MI_Id)
                                       select new Adm_Master_ActivitiesDTO
                                       {
                                           AMA_Id = a.AMA_Id,
                                           AMA_ActivityName = a.AMA_ActivityName,
                                           AMA_ActivityDesc = a.AMA_ActivityDesc,
                                           AMA_ActiveFlg = a.AMA_ActiveFlg,
                                           FMG_GroupName = b.FMG_GroupName,
                                           FMG_Id = a.FMG_Id,
                                           FMH_Id = a.FMH_Id,
                                           FMH_FeeName = d.FMH_FeeName,
                                       }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //============================================gethead()======================================================================//
        public Adm_Master_ActivitiesDTO gethead(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                data.gethead = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                from b in _FeeGroupContext.FeeHeadDMO
                                where (a.FMH_Id == b.FMH_Id && a.MI_Id == data.MI_Id && b.FMH_ActiveFlag == true && a.FMG_Id == data.FMG_Id)
                                select new Adm_Master_ActivitiesDTO
                                {
                                    FMH_FeeName = b.FMH_FeeName,
                                    FMH_Id = b.FMH_Id,
                                }).Distinct().OrderBy(a => a.FMH_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //=================================================savedata()===============================================//
        public Adm_Master_ActivitiesDTO savedata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                if (data.AMA_Id == 0)
                {
                    for (int i = 0; i < data.headid.Length; i++)
                    {
                        var tempdata = data.headid[i].FMH_Id;
                        {
                            var duplicate = _FeeGroupContext.Adm_Master_Activities.Where(t => t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.AMA_ActivityName==data.AMA_ActivityName).ToArray();
                            //var duplicate = _FeeGroupContext.Adm_Master_Activities.Where(t => t.AMA_ActivityName == data.AMA_ActivityName).ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.count += 1;
                                data.duplicate = true;
                            }
                            else
                            {
                                data.count1 += 1;
                                Adm_Master_Activities rrr = new Adm_Master_Activities();
                                rrr.AMA_Id = data.AMA_Id;
                                rrr.MI_Id = data.MI_Id;
                                rrr.FMG_Id = data.FMG_Id;
                                rrr.AMA_ActivityName = data.AMA_ActivityName;
                                rrr.AMA_ActivityDesc = data.AMA_ActivityDesc;
                                 rrr.FMH_Id = tempdata;
                                rrr.AMA_CreatedDate = DateTime.Now;
                                rrr.AMA_UpdatedDate = DateTime.Now;
                                rrr.AMA_CreatedBy = data.UserId;
                                rrr.AMA_UpdatedBy = data.UserId;
                                rrr.AMA_ActiveFlg = true;
                                _FeeGroupContext.Add(rrr);
                                int y = _FeeGroupContext.SaveChanges();
                                if (y > 0)
                                {
                                    data.msg = "Saved";
                                }
                                else
                                {
                                    data.msg = "Failed";
                                }
                            }
                        }
                    }
                }
                else if (data.AMA_Id > 0)
                {
                    var duplicate = _FeeGroupContext.Adm_Master_Activities.Where(t => t.AMA_Id == data.AMA_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == data.FMH_Id && t.AMA_Id != data.AMA_Id && t.AMA_ActivityName==data.AMA_ActivityName).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _FeeGroupContext.Adm_Master_Activities.Where(t => t.AMA_Id == data.AMA_Id).SingleOrDefault();
                        yy.AMA_Id = data.AMA_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.FMG_Id = data.FMG_Id;
                        yy.FMH_Id = data.FMH_Id;
                        yy.AMA_ActivityName = data.AMA_ActivityName;
                        yy.AMA_ActivityDesc = data.AMA_ActivityDesc;
                        yy.AMA_CreatedDate = DateTime.Now;
                        yy.AMA_UpdatedDate = DateTime.Now;
                        yy.AMA_CreatedBy = data.UserId;
                        yy.AMA_UpdatedBy = data.UserId;
                        _FeeGroupContext.Update(yy);
                        int r = _FeeGroupContext.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "Updated";
                        }
                        else
                        {
                            data.msg = "Update Failed";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //===============================================active/deactive()=================================================//
        public Adm_Master_ActivitiesDTO masterDecative(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                var u = _FeeGroupContext.Adm_Master_Activities.Where(t => t.AMA_Id == data.AMA_Id).SingleOrDefault();
                if (u.AMA_ActiveFlg == true)
                {
                    u.AMA_ActiveFlg = false;
                }
                else if (u.AMA_ActiveFlg == false)
                {
                    u.AMA_ActiveFlg = true;
                }
                _FeeGroupContext.Update(u);
                int o = _FeeGroupContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        //=========================================delete data===========================================================//
        public Adm_Master_ActivitiesDTO deletedata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                for (int i = 0; i < data.listdata23.Length; i++)
                {
                    var temp_id = data.listdata23[i].AMA_Id;

                    var result = _FeeGroupContext.Adm_Master_Activities.Where(t => t.AMA_Id == temp_id).Single();

                    if (result.AMA_ActiveFlg==true)
                    {
                        result.AMA_ActiveFlg = false;
                    }
                    result.AMA_UpdatedBy = data.userid;
                    result.AMA_UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);
                }
               int row =_FeeGroupContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception s)
            {
                Console.WriteLine(s.Message);
            }
            return data;
        }
    }
}

