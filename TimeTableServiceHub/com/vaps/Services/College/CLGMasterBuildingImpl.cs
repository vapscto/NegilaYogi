using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CLGMasterBuildingImpl:Interfaces.CLGMasterBuildingInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;
        private object _category;

        public CLGMasterBuildingImpl(TTContext ttcntx, DomainModelMsSqlServerContext db)
        {
            _ttcontext = ttcntx;
            _db = db;
        }
        public CLGMasterBuilding_DTO getdetails(CLGMasterBuilding_DTO data)
        {


            try
            {
                data.academic = _ttcontext.AcademicYear.Where(w => w.MI_Id == data.MI_Id && w.Is_Active == true).Distinct().OrderByDescending(e => e.ASMAY_Order).ToArray();

                List<TT_Master_BuildingDMO> name = new List<TT_Master_BuildingDMO>();
                name = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.bnamedrp = name.ToArray();

                List<TT_Master_BuildingDMO> master = new List<TT_Master_BuildingDMO>();
                master = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMB_ActiveFlag == true).ToList();
                data.masterbuilding = master.ToArray();

                List<Adm_College_Master_SectionDMO> section = new List<Adm_College_Master_SectionDMO>();
                section = _ttcontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToList();
                data.secdrp = section.ToArray();

             
                data.courselist = _ttcontext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().ToArray();
                data.csmap = (from a in _ttcontext.MasterCourseDMO
                              from b in _ttcontext.Adm_College_Master_SectionDMO
                              from c in _ttcontext.TT_Master_BuildingDMO
                              from d in _ttcontext.ClgMasterBranchDMO
                              from e in _ttcontext.CLG_Adm_Master_SemesterDMO
                              from f in _ttcontext.CLGMasterBuilding_DMO
                              from g in _ttcontext.AcademicYear
                              where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && c.MI_Id == d.MI_Id && d.MI_Id == e.MI_Id && a.MI_Id==data.MI_Id && a.AMCO_Id == f.AMCO_Id && b.ACMS_Id == f.ACMS_Id && c.TTMB_Id == f.TTMB_Id && d.AMB_Id == f.AMB_Id && e.AMSE_Id == f.AMSE_Id && a.AMCO_ActiveFlag == true && b.ACMS_ActiveFlag == true && c.TTMB_ActiveFlag == true && d.AMB_ActiveFlag == true && e.AMSE_ActiveFlg == true && f.ASMAY_Id == g.ASMAY_Id)
                              select new CLGMasterBuilding_DTO
                              {
                                  TTMBCS_Id = f.TTMBCS_Id,
                                  AMCO_CourseName = a.AMCO_CourseName,
                                  ACMS_SectionName = b.ACMS_SectionName,
                                  TTMB_BuildingName = c.TTMB_BuildingName,
                                  AMB_BranchName = d.AMB_BranchName,
                                  AMSE_SEMName = e.AMSE_SEMName,
                                  TTMBCS_ActiveFlag = f.TTMBCS_ActiveFlag,
                                  ASMAY_Year = g.ASMAY_Year,
                                  ASMAY_Order = g.ASMAY_Order
                              }).Distinct().OrderByDescending(e=>e.ASMAY_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGMasterBuilding_DTO savedetail(CLGMasterBuilding_DTO _category)
        {
            CLGMasterBuilding_DTO objpge = Mapper.Map<CLGMasterBuilding_DTO>(_category);
            try
            {
                if (objpge.TTMB_Id > 0)
                {
                    var res = _ttcontext.TT_Master_BuildingDMO.Where(t => t.MI_Id == objpge.MI_Id && t.TTMB_BuildingName == objpge.TTMB_BuildingName && t.TTMB_Id != objpge.TTMB_Id).ToList();
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
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
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

        public CLGMasterBuilding_DTO savedetail1(CLGMasterBuilding_DTO _category)
        {
            CLGMasterBuilding_DTO obj = Mapper.Map<CLGMasterBuilding_DTO>(_category);
            try
            {
                if (obj.TTMBCS_Id > 0)
                {
                    for (int i = 0; i < obj.sectionarray.Count(); i++)
                    {
                        var res = _ttcontext.CLGMasterBuilding_DMO.Where(t => t.TTMB_Id == obj.TTMB_Id &&t.AMCO_Id==obj.AMCO_Id&&t.AMB_Id==obj.AMB_Id&&t.AMSE_Id==obj.AMSE_Id&& /* t.ACMS_Id == obj.sectionarray[i].ACMS_Id &&*/ t.TTMB_Id != obj.TTMB_Id && t.ASMAY_Id == obj.ASMAY_Id).ToList();
                        if (res.Count > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }

                        else
                        {
                            var result = _ttcontext.CLGMasterBuilding_DMO.Single(t => t.TTMBCS_Id == obj.TTMBCS_Id);
                            result.TTMB_Id = obj.TTMB_Id;
                            result.ASMAY_Id = obj.ASMAY_Id;
                            result.ACMS_Id = obj.sectionarray[i].ACMS_Id;
                            result.AMCO_Id = obj.AMCO_Id;
                            result.AMB_Id = obj.AMB_Id;
                            result.AMSE_Id = obj.AMSE_Id;
                            result.CreatedDate = DateTime.Now;
                            result.UpdatedDate = DateTime.Now;
                            result.TTMBCS_ActiveFlag = true;
                            _ttcontext.Update(result);
                            var contactexists = _ttcontext.SaveChanges();
                            if (contactexists > 0)
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
                else
                {
              for(int j=0;j<obj.sectionarray.Count();j++)
                    {
                        var res = _ttcontext.CLGMasterBuilding_DMO.Where(t => t.TTMB_Id == obj.TTMB_Id &&t.AMB_Id==obj.AMB_Id&&t.AMCO_Id==obj.AMCO_Id&&t.AMSE_Id==obj.AMSE_Id&& t.ACMS_Id == obj.sectionarray[j].ACMS_Id && t.ASMAY_Id==obj.ASMAY_Id).ToList();
                        if(res.Count>0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            CLGMasterBuilding_DMO r = new CLGMasterBuilding_DMO();
                            r.ASMAY_Id = obj.ASMAY_Id;
                            r.TTMB_Id = obj.TTMB_Id;
                            r.AMB_Id = obj.AMB_Id;
                            r.AMCO_Id = obj.AMCO_Id;
                            r.AMSE_Id = obj.AMSE_Id;
                            r.ACMS_Id = obj.sectionarray[j].ACMS_Id;
                            r.CreatedDate = DateTime.Now;
                            r.UpdatedDate = DateTime.Now;
                            r.TTMBCS_ActiveFlag = true;
                            _ttcontext.Add(r);
                            var d = _ttcontext.SaveChanges();
                            if(d>0)
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return _category;
        }

        public CLGMasterBuilding_DTO getpagedetails1(int id)
        {
            CLGMasterBuilding_DTO obj = new CLGMasterBuilding_DTO();
            try
            {
                List<CLGMasterBuilding_DMO> ss = new List<CLGMasterBuilding_DMO>();
                ss = _ttcontext.CLGMasterBuilding_DMO.Where(t => t.TTMBCS_Id == id).ToList();
                obj.mastersection = ss.ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return obj;
        }
        public CLGMasterBuilding_DTO deactive1(CLGMasterBuilding_DTO abcd)
        {
            try
            {
                if (abcd.TTMBCS_Id > 0)
                {
                    var d = _ttcontext.CLGMasterBuilding_DMO.Single(t => t.TTMBCS_Id == abcd.TTMBCS_Id);
                    if (d.TTMBCS_ActiveFlag == true)
                    {
                        d.TTMBCS_ActiveFlag = false;
                    }
                    else 
                    {
                        d.TTMBCS_ActiveFlag = true;
                    }
                    _ttcontext.Update(d);
                    var o = _ttcontext.SaveChanges();
                    if (o > 0)
                    {
                        abcd.returnval = true;
                    }
                    else
                    {
                        abcd.returnval = false;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return abcd;
        }


    }
}



//CLGMasterBuilding_DTO objpge = Mapper.Map<CLGMasterBuilding_DTO>(_category);

//if (objpge.TTMBCS_Id > 0)
//{
//    for (int i = 0; i < objpge.classarray.Count(); i++)
//    {
//        if (objpge.classarray != null && objpge.classarray.Count() > 0)
//        {
//            for (int j = 0; j < objpge.sectionarray.Count(); j++)
//            {
//                var res = _ttcontext.TT_Master_Building_Class_SectionDMO.Where(t => t.TTMB_Id == objpge.TTMB_Id && t.ASMCL_Id == objpge.classarray[i].ASMCL_Id && t.ASMS_Id == objpge.sectionarray[j].ASMS_Id && t.TTMBCS_Id != objpge.TTMBCS_Id).ToList();
//                if (res.Count() > 0)
//                {
//                    _category.returnduplicatestatus = "Duplicate";
//                }
//                else
//                {
//                    var result = _ttcontext.TT_Master_Building_Class_SectionDMO.Single(t => t.TTMBCS_Id == objpge.TTMBCS_Id);
//                    result.TTMB_Id = objpge.TTMB_Id;
//                    result.ASMCL_Id = objpge.classarray[i].ASMCL_Id;
//                    result.ASMS_Id = objpge.sectionarray[j].ASMS_Id;
//                    _ttcontext.Update(result);
//                    var contactExists = _ttcontext.SaveChanges();
//                    if (contactExists == 1)
//                    {
//                        _category.returnval = true;
//                    }
//                    else
//                    {
//                        _category.returnval = false;
//                    }
//                }
//            }
//        }
//    }
//}
//else
//{
//    for (int i = 0; i < objpge.classarray.Count(); i++)
//    {
//        if (objpge.classarray != null && objpge.classarray.Count() > 0)
//        {

//            for (int j = 0; j < objpge.sectionarray.Count(); j++)
//            {
//                var result = _ttcontext.TT_Master_Building_Class_SectionDMO.Where(t => t.TTMB_Id == objpge.TTMB_Id && t.ASMCL_Id == objpge.classarray[i].ASMCL_Id && t.ASMS_Id == objpge.sectionarray[j].ASMS_Id).ToList();
//                if (result.Count() > 0)
//                {
//                    _category.returnduplicatestatus = "Duplicate";
//                }
//                else
//                {
//                    TT_Master_Building_Class_SectionDMO obj = new TT_Master_Building_Class_SectionDMO();
//                    obj.TTMB_Id = objpge.TTMB_Id;
//                    obj.ASMCL_Id = objpge.classarray[i].ASMCL_Id;
//                    obj.ASMS_Id = objpge.sectionarray[j].ASMS_Id;
//                    obj.TTMBCS_ActiveFlag = true;
//                    _ttcontext.Add(obj);
//                    var contactExists = _ttcontext.SaveChanges();
//                    if (contactExists == 1)
//                    {
//                        _category.returnval = true;
//                    }
//                    else
//                    {
//                        _category.returnval = false;
//                    }
//                }
//            }
//        }

//    }
//}


