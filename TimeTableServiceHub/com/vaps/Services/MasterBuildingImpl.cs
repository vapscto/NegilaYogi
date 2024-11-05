using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class MasterBuildingImpl : Interfaces.MasterBuildingInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;


        public MasterBuildingImpl(TTContext ttcntx, DomainModelMsSqlServerContext db)
        {
            _ttcontext = ttcntx;
            _db = db;
        }

        public TT_Master_BuildingDTO savedetail(TT_Master_BuildingDTO _category)
        {
            TT_Master_BuildingDTO objpge = Mapper.Map<TT_Master_BuildingDTO>(_category);
            try
            {
                if (objpge.TTMB_Id > 0)
                {
                    var res = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == objpge.MI_Id && t.TTMB_BuildingName == objpge.TTMB_BuildingName && t.TTMB_Id!=objpge.TTMB_Id).ToList();
                    if (res.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcontext.TT_Master_BuildingDMO.Single(t => t.MI_Id == objpge.MI_Id && t.TTMB_Id == objpge.TTMB_Id);
                        result.TTMB_BuildingName = objpge.TTMB_BuildingName;
                        _ttcontext.Update(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                else
                {
                    var res = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == objpge.MI_Id && t.TTMB_BuildingName == objpge.TTMB_BuildingName).ToList();
                    if (res.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        TT_Master_BuildingDMO result = new TT_Master_BuildingDMO();
                        result.MI_Id = objpge.MI_Id;
                        result.TTMB_BuildingName = objpge.TTMB_BuildingName;
                        result.TTMB_ActiveFlag = true;
                        _ttcontext.Add(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TT_Master_BuildingDTO getdetails(int id)
        {

            TT_Master_BuildingDTO TTMB = new TT_Master_BuildingDTO();
            try
            {
                List<TT_Master_BuildingDMO> name = new List<TT_Master_BuildingDMO>();
                name = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == id).ToList();
                TTMB.bnamedrp = name.ToArray();

                List<TT_Master_BuildingDMO> master = new List<TT_Master_BuildingDMO>();
                master = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == id && t.TTMB_ActiveFlag == true).ToList();
                TTMB.masterbuilding = master.ToArray();

                List<School_M_Section> section = new List<School_M_Section>();
                section = _ttcontext.School_M_Section.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                TTMB.secdrp = section.ToArray();

                List<School_M_Class> cls = new List<School_M_Class>();
                cls = _db.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                TTMB.clsdrp = cls.ToArray();

                TTMB.csmap = (from s in _ttcontext.TT_Master_BuildingDMO
                              from p in _ttcontext.TT_Master_Building_Class_SectionDMO
                              from q in _ttcontext.School_M_Section
                              from r in _ttcontext.School_M_Class
                              where (s.TTMB_Id == p.TTMB_Id && p.ASMCL_Id == r.ASMCL_Id && p.ASMS_Id == q.ASMS_Id && s.MI_Id == id)
                              select new TT_Master_BuildingDTO
                              {
                                  TTMBCS_Id = p.TTMBCS_Id,
                                  TTMBCS_ActiveFlag = p.TTMBCS_ActiveFlag,
                                  TTMB_BuildingName = s.TTMB_BuildingName,
                                  ASMCL_ClassName = r.ASMCL_ClassName,
                                  ASMC_SectionName = q.ASMC_SectionName
                              }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMB;

        }

        public TT_Master_BuildingDTO savedetail1(TT_Master_BuildingDTO _category)
        {
            TT_Master_BuildingDTO objpge = Mapper.Map<TT_Master_BuildingDTO>(_category);
            try
            {
                if (objpge.TTMBCS_Id > 0)
                {
                    for (int i = 0; i < objpge.classarray.Count(); i++)
                    {
                        if (objpge.classarray != null && objpge.classarray.Count() > 0)
                        {
                            for (int j = 0; j < objpge.sectionarray.Count(); j++)
                            {
                                var res = _ttcontext.TT_Master_Building_Class_SectionDMO.Where(t => t.TTMB_Id == objpge.TTMB_Id && t.ASMCL_Id == objpge.classarray[i].ASMCL_Id && t.ASMS_Id == objpge.sectionarray[j].ASMS_Id && t.TTMBCS_Id!= objpge.TTMBCS_Id).ToList();
                                if (res.Count() > 0)
                                {
                                    _category.returnduplicatestatus = "Duplicate";
                                }
                                else
                                {
                                    var result = _ttcontext.TT_Master_Building_Class_SectionDMO.Single(t => t.TTMBCS_Id == objpge.TTMBCS_Id);
                                    result.TTMB_Id = objpge.TTMB_Id;
                                    result.ASMCL_Id = objpge.classarray[i].ASMCL_Id;
                                    result.ASMS_Id = objpge.sectionarray[j].ASMS_Id;
                                    _ttcontext.Update(result);
                                    var contactExists = _ttcontext.SaveChanges();
                                    if (contactExists == 1)
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < objpge.classarray.Count(); i++)
                    {
                        if (objpge.classarray != null && objpge.classarray.Count() > 0)
                        {

                            for (int j = 0; j < objpge.sectionarray.Count(); j++)
                            {
                                var result = _ttcontext.TT_Master_Building_Class_SectionDMO.Where(t => t.TTMB_Id == objpge.TTMB_Id && t.ASMCL_Id == objpge.classarray[i].ASMCL_Id && t.ASMS_Id == objpge.sectionarray[j].ASMS_Id).ToList();
                                if (result.Count() > 0)
                                {
                                    _category.returnduplicatestatus = "Duplicate";
                                }
                                else
                                {
                                    TT_Master_Building_Class_SectionDMO obj = new TT_Master_Building_Class_SectionDMO();
                                    obj.TTMB_Id = objpge.TTMB_Id;
                                    obj.ASMCL_Id = objpge.classarray[i].ASMCL_Id;
                                    obj.ASMS_Id = objpge.sectionarray[j].ASMS_Id;
                                    obj.TTMBCS_ActiveFlag = true;
                                    _ttcontext.Add(obj);
                                    var contactExists = _ttcontext.SaveChanges();
                                    if (contactExists == 1)
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TT_Master_BuildingDTO getpagedetails(long id)
        {
            TT_Master_BuildingDTO page = new TT_Master_BuildingDTO();
            try
            {
                List<TT_Master_BuildingDMO> lorg = new List<TT_Master_BuildingDMO>();
                lorg = _ttcontext.TT_Master_BuildingDMO.AsNoTracking().Where(t => t.TTMB_Id.Equals(id)).ToList();
                page.masterbuild = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;

        }

        public TT_Master_BuildingDTO getpagedetails1(long id)
        {
            TT_Master_BuildingDTO page = new TT_Master_BuildingDTO();
            try
            {
                List<TT_Master_Building_Class_SectionDMO> lorg = new List<TT_Master_Building_Class_SectionDMO>();
                lorg = _ttcontext.TT_Master_Building_Class_SectionDMO.AsNoTracking().Where(t => t.TTMBCS_Id == id).ToList();
                page.mastersection = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;

        }

        public TT_Master_BuildingDTO deactivate(TT_Master_BuildingDTO acd)
        {
            try
            {
                TT_Master_BuildingDMO master = Mapper.Map<TT_Master_BuildingDMO>(acd);
                if (master.TTMB_Id > 0)
                {
                    var result = _ttcontext.TT_Master_BuildingDMO.Single(t => t.TTMB_Id == master.TTMB_Id);
                    if (result.TTMB_ActiveFlag == true)
                    {
                        result.TTMB_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMB_ActiveFlag = true;
                    }

                    _ttcontext.Update(result);
                    var flag = _ttcontext.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public TT_Master_BuildingDTO deactivate1(TT_Master_BuildingDTO acd)
        {
            try
            {
                TT_Master_Building_Class_SectionDMO mastersection = Mapper.Map<TT_Master_Building_Class_SectionDMO>(acd);
                if (mastersection.TTMBCS_Id > 0)
                {
                    var result = _ttcontext.TT_Master_Building_Class_SectionDMO.Single(t => t.TTMBCS_Id == mastersection.TTMBCS_Id);
                    if (result.TTMBCS_ActiveFlag == true)
                    {
                        result.TTMBCS_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMBCS_ActiveFlag = true;
                    }

                    _ttcontext.Update(result);
                    var flag = _ttcontext.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
