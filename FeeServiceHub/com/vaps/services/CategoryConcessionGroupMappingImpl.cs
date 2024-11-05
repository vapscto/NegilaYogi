using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class CategoryConcessionGroupMappingImpl:interfaces.CategoryConcessionGroupMappingInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public CollFeeGroupContext _feecollgroup;
        public CategoryConcessionGroupMappingImpl(FeeGroupContext p, CollFeeGroupContext o)
        {
            _FeeGroupContext = p;
            _feecollgroup = o;
        }

        public CategoryConcessionGroupMappingDTO loaddata(CategoryConcessionGroupMappingDTO data)
        
            {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();
                data.ASMAY_Id = data.ASMAY_Id;

                data.concession = _FeeGroupContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).Distinct().ToArray();

                data.group = (from a in _FeeGroupContext.FeeGroupDMO
                              from b in _FeeGroupContext.Yearlygroups
                              from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                              where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_Id && c.FYGHM_ActiveFlag == "1")
                              select new FeeAmountEntryDTO
                              {
                                  FMG_Id = a.FMG_Id,
                                  FMG_GroupName = a.FMG_GroupName,
                              }
         ).Distinct().ToArray();

                data.alldata = (from a in _FeeGroupContext.AcademicYear
                                from b in _FeeGroupContext.Fee_Master_ConcessionDMO
                                from c in _FeeGroupContext.FeeGroupDMO
                                from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupContext.FeeHeadDMO
                                from f in _FeeGroupContext.Fee_Master_Concession_Group
                                where (a.ASMAY_Id == f.ASMAY_Id && a.Is_Active == true && b.FMCC_Id == f.FMCC_Id && b.FMCC_ActiveFlag == true && c.FMG_Id == f.FMG_Id && c.FMG_ActiceFlag == true && d.FTI_Id == f.FTI_Id && e.FMH_Id == f.FMH_Id && e.FMH_ActiveFlag == true)
                                select new CategoryConcessionGroupMappingDTO
                                {
                                    FMCCG_Id = f.FMCCG_Id,
                                    ASMAY_Year = a.ASMAY_Year,
                                    ASMAY_Id = f.ASMAY_Id,
                                    FMCC_ConcessionName = b.FMCC_ConcessionName,
                                    FMCC_Id = b.FMCC_Id,
                                    FMG_Id = c.FMG_Id,
                                    FMG_GroupName = c.FMG_GroupName,
                                    FTI_Id = d.FTI_Id,
                                    FTI_Name = d.FTI_Name,
                                    FMH_Id = e.FMH_Id,
                                    FMH_FeeName = e.FMH_FeeName,
                                    FMCCG_ActiveFlag = f.FMCCG_ActiveFlag,

                                }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CategoryConcessionGroupMappingDTO getgroup(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                data.group = (from a in _FeeGroupContext.FeeGroupDMO
                              from b in _FeeGroupContext.Yearlygroups
                              from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                              where (b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && a.FMG_Id == b.FMG_Id && b.FMG_Id == c.FMG_Id && c.FYGHM_ActiveFlag == "1")
                              select new FeeAmountEntryDTO
                              {
                                  FMG_Id = a.FMG_Id,
                                  FMG_GroupName = a.FMG_GroupName,
                              }
                    ).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CategoryConcessionGroupMappingDTO gethead(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                data.head = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                             from b in _FeeGroupContext.FeeHeadDMO
                             from c in _FeeGroupContext.FeeInstallmentsyearlyDMO

                             where (a.FMG_Id == data.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.user_id == data.user_Id && c.FTI_Id == data.FTI_Id && a.FMI_Id == c.FMI_Id && b.FMH_Flag != "F" && b.FMH_Flag != "E")
                             select new CategoryConcessionGroupMappingDTO
                             {

                                 FMH_Id = b.FMH_Id,
                                 FMH_FeeName = b.FMH_FeeName,

                             }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CategoryConcessionGroupMappingDTO getconcession(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                data.conce = (from b in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                              from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                              from c in _FeeGroupContext.FeeInstallmentsyearlyDMO
                              where (a.FMG_Id == b.FMG_ID && c.FMI_Id == a.FMI_Id && a.FMH_Id == b.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && b.User_Id == data.user_Id && a.MI_Id == data.MI_Id && a.FMG_Id == data.FMG_Id && b.MI_ID == data.MI_Id && c.MI_ID == data.MI_Id && c.MI_ID == b.MI_ID && b.User_Id == data.user_Id)
                              select new CategoryConcessionGroupMappingDTO
                              {
                                  FTI_Id = c.FTI_Id,
                                  FTI_Name = c.FTI_Name,

                              }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;

        }

        public CategoryConcessionGroupMappingDTO deactiveStudent(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                var u = _FeeGroupContext.Fee_Master_Concession_Group.Where(t => t.FMCCG_Id == data.FMCCG_Id).SingleOrDefault();
                if (u.FMCCG_ActiveFlag == true)
                {
                    u.FMCCG_ActiveFlag = false;
                }
                else if (u.FMCCG_ActiveFlag == false)
                {
                    u.FMCCG_ActiveFlag = true;
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


        public CategoryConcessionGroupMappingDTO save(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                if (data.FMCCG_Id == 0)
                {
                    for (int i = 0; i < data.headlistdata.Length; i++)
                    {
                        var tempdata = data.headlistdata[i].FMH_Id;

                        var duplicate = _FeeGroupContext.Fee_Master_Concession_Group.Where(t => t.MI_Id == data.MI_Id && t.FMCC_Id == data.FMCC_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == tempdata&&t.FTI_Id==data.FTI_Id&&t.ASMAY_Id==data.ASMAY_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            data.count1 += 1;
                            Fee_Master_Concession_Group obj = new Fee_Master_Concession_Group();
                            obj.MI_Id = data.MI_Id;
                            obj.FMCC_Id = data.FMCC_Id;
                            obj.FMG_Id = data.FMG_Id;
                            obj.FMH_Id = tempdata;
                            obj.FMCCG_ActiveFlag = true;
                            obj.ASMAY_Id = data.ASMAY_Id;
                            obj.FTI_Id = data.FTI_Id;
                            _FeeGroupContext.Add(obj);
                            int s = _FeeGroupContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "saved";

                            }
                            else
                            {
                                data.msg = "savingFailed";
                            }
                        }
                    }

                }
                else if (data.FMCCG_Id > 0)
                {
                    for (int i = 0; i < data.headlistdata.Length; i++)
                    {
                        var tempdata = data.headlistdata[i].FMH_Id;

                        var duplicate = _FeeGroupContext.Fee_Master_Concession_Group.Where(t => t.MI_Id == data.MI_Id && t.FMCCG_Id != data.FMCCG_Id && t.FMCC_Id == data.FMCC_Id && t.FMG_Id == data.FMG_Id && t.FMH_Id == tempdata && t.ASMAY_Id == data.ASMAY_Id&&t.FTI_Id==data.FTI_Id).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            data.count1 += 1;

                            var update = _FeeGroupContext.Fee_Master_Concession_Group.Where(t => t.MI_Id == data.MI_Id && t.FMCCG_Id == data.FMCCG_Id).SingleOrDefault();
                            update.FMCC_Id = data.FMCC_Id;
                            update.FMG_Id = data.FMG_Id;
                            update.FMH_Id = tempdata;
                            update.ASMAY_Id = data.ASMAY_Id;
                            update.FTI_Id = data.FTI_Id;
                            _FeeGroupContext.Update(update);
                            int s = _FeeGroupContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "update";
                            }
                            else
                            {
                                data.msg = "updateFailed";
                            }
                        }
                       
                    }
                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CategoryConcessionGroupMappingDTO EditData(CategoryConcessionGroupMappingDTO data)
        {
            try
            {
                var editlist = (from a in _FeeGroupContext.AcademicYear
                                from b in _FeeGroupContext.Fee_Master_Concession_Group
                                where (b.FMCCG_Id == data.FMCCG_Id && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.ASMAY_Id == a.ASMAY_Id )
                                select new CategoryConcessionGroupMappingDTO
                                {
                                    FMCCG_Id = b.FMCCG_Id,
                                    FMH_Id = b.FMH_Id,
                                    ASMAY_Year = a.ASMAY_Year,
                                    ASMAY_Id = b.ASMAY_Id,
                                }).Distinct().ToList();


                data.editlist = editlist.ToArray();
                if (editlist[0].FMH_Id != 0)
                {
                    var ee = (from a in _FeeGroupContext.Fee_Master_Concession_Group
                              where (a.FMH_Id == editlist[0].FMH_Id && a.ASMAY_Id == editlist[0].ASMAY_Id)
                              select new CategoryConcessionGroupMappingDTO
                              {
                                  ASMAY_Id = a.ASMAY_Id,
                                  FMCC_Id = a.FMCC_Id,
                                  FMG_Id = a.FMG_Id,
                                  FTI_Id = a.FTI_Id,
                              }).Distinct().ToList();

                    data.FMCC_Id = ee[0].FMCC_Id;
                    data.FMG_Id = ee[0].FMG_Id;
                    data.FTI_Id = ee[0].FTI_Id;
                    data.ASMAY_Id = ee[0].ASMAY_Id;

                    List<MasterAcademic> year = new List<MasterAcademic>();
                    year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                    data.acayear = year.Distinct().ToArray();

                    data.concession = _FeeGroupContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).Distinct().ToArray();
                    data.group = _FeeGroupContext.FeeGroupDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_ActiceFlag == true && t.user_id == data.user_Id).Distinct().OrderByDescending(t => t.FMG_Order).ToArray();
                    
                    data.alldata = (from a in _FeeGroupContext.AcademicYear
                                    from b in _FeeGroupContext.Fee_Master_ConcessionDMO
                                    from c in _FeeGroupContext.FeeGroupDMO
                                    from d in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                    from e in _FeeGroupContext.FeeHeadDMO
                                    from f in _FeeGroupContext.Fee_Master_Concession_Group
                                    where (a.ASMAY_Id == f.ASMAY_Id && a.Is_Active == true && b.FMCC_Id == f.FMCC_Id && b.FMCC_ActiveFlag == true && c.FMG_Id == f.FMG_Id && c.FMG_ActiceFlag == true && d.FTI_Id == f.FTI_Id && e.FMH_Id == f.FMH_Id && e.FMH_ActiveFlag == true)
                                    select new CategoryConcessionGroupMappingDTO
                                    {

                                        ASMAY_Year = a.ASMAY_Year,
                                        ASMAY_Id = f.ASMAY_Id,
                                        FMCC_ConcessionName = b.FMCC_ConcessionName,
                                        FMCC_Id = b.FMCC_Id,
                                        FMG_Id = c.FMG_Id,
                                        FMG_GroupName = c.FMG_GroupName,
                                        FTI_Id = d.FTI_Id,
                                        FTI_Name = d.FTI_Name,
                                        FMH_Id = e.FMH_Id,
                                        FMH_FeeName = e.FMH_FeeName,


                                    }).Distinct().ToArray();
                    data.conce = (from b in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                  from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FeeInstallmentsyearlyDMO
                                  where (a.FMG_Id == b.FMG_ID && c.FMI_Id == a.FMI_Id && a.FMH_Id == b.FMH_Id && a.ASMAY_Id == data.ASMAY_Id && b.User_Id == data.user_Id && a.MI_Id == data.MI_Id && a.FMG_Id == data.FMG_Id && b.MI_ID == data.MI_Id && c.MI_ID == data.MI_Id && c.MI_ID == b.MI_ID && b.User_Id == data.user_Id)
                                  select new CategoryConcessionGroupMappingDTO
                                  {
                                      FTI_Id = c.FTI_Id,
                                      FTI_Name = c.FTI_Name,

                                  }).Distinct().ToArray();


                    data.head = (from a in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                 from b in _FeeGroupContext.FeeHeadDMO
                                 from c in _FeeGroupContext.FeeInstallmentsyearlyDMO

                                 where (a.FMG_Id == data.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.user_id == data.user_Id && c.FTI_Id == data.FTI_Id && a.FMI_Id == c.FMI_Id)
                                 select new CategoryConcessionGroupMappingDTO
                                 {
                                     FMH_Id = b.FMH_Id,
                                     FMH_FeeName = b.FMH_FeeName,

                                 }).Distinct().ToArray();
                    data.FMCC_Id = ee[0].FMCC_Id;
                    data.FMG_Id = ee[0].FMG_Id;
                    data.FTI_Id = ee[0].FTI_Id;

                    data.ASMAY_Id = ee[0].ASMAY_Id;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
