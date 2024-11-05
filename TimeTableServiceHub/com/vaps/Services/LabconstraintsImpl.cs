using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class LabconstraintsImpl : Interfaces.LabconstraintsInterface
    {
        private static ConcurrentDictionary<string, TT_LABLIB_DTO> _login =
               new ConcurrentDictionary<string, TT_LABLIB_DTO>();


        public TTContext _ttcategorycontext;
        public LabconstraintsImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public TT_LABLIB_DTO savedetail(TT_LABLIB_DTO _category)
        {
            TT_LABLIB_DMO objpge = Mapper.Map<TT_LABLIB_DMO>(_category);
            try
            {
                if (objpge.TTLAB_Id > 0)
                {
                    var result = _ttcategorycontext.TT_LABLIB_DMO.Single(t => t.TTLAB_Id.Equals(objpge.TTLAB_Id) && t.MI_Id.Equals(objpge.MI_Id));
                    result.ASMAY_Id = objpge.ASMAY_Id;
                    result.TTMC_Id = objpge.TTMC_Id;
                    result.TTLAB_LABLIBName = objpge.TTLAB_LABLIBName;
                    result.TTLAB_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _ttcategorycontext.Update(result);
                    var contactExists = _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                        {
                            TT_LABLIB_DetailsDTO ttlldto = new TT_LABLIB_DetailsDTO();
                            TT_LABLIB_DetailsDMO ttllddmo = Mapper.Map<TT_LABLIB_DetailsDMO>(ttlldto);

                            var result0 = _ttcategorycontext.TT_LABLIB_DetailsDMO.Where(t => t.TTLAB_Id.Equals(objpge.TTLAB_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMCL_Id.Equals(Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id)) && t.ASMS_Id.Equals(Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id)) && t.ISMS_Id.Equals(Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id)));

                            if (result0.Count() == 0)
                            {
                                ttllddmo.TTLAB_Id = objpge.TTLAB_Id;
                                ttllddmo.ASMAY_Id = objpge.ASMAY_Id;
                                ttllddmo.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                ttllddmo.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                ttllddmo.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                ttllddmo.CreatedDate = DateTime.Now;
                                ttllddmo.UpdatedDate = DateTime.Now;
                                ttllddmo.TTLABD_ActiveFlag = true;
                                _ttcategorycontext.Add(ttllddmo);
                                var contactExists1 = _ttcategorycontext.SaveChanges();
                                if (contactExists1 == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                            else
                            {
                                List<TT_LABLIB_DetailsDMO> lorg1 = new List<TT_LABLIB_DetailsDMO>();
                                lorg1 = _ttcategorycontext.TT_LABLIB_DetailsDMO.Where(t => t.TTLAB_Id.Equals(objpge.TTLAB_Id)).ToList();
                                if (lorg1.Any())
                                {
                                    for (int l = 0; l < lorg1.Count; l++)
                                    {
                                        _ttcategorycontext.Remove(lorg1.ElementAt(l));
                                        var fff = _ttcategorycontext.SaveChanges();
                                        if (fff == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
                                }

                                TT_LABLIB_DetailsDTO ttlldto12 = new TT_LABLIB_DetailsDTO();
                                TT_LABLIB_DetailsDMO result10 = Mapper.Map<TT_LABLIB_DetailsDMO>(ttlldto12);
                                result10.TTLAB_Id = objpge.TTLAB_Id;
                                result10.ASMAY_Id = objpge.ASMAY_Id;
                                result10.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                result10.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                result10.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                result10.CreatedDate = DateTime.Now;
                                result10.UpdatedDate = DateTime.Now;
                                result10.TTLABD_ActiveFlag = true;
                                _ttcategorycontext.Add(result10);
                                var contactExists1 = _ttcategorycontext.SaveChanges();
                                if (contactExists1 == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }

                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }

                }
                else
                {
                    var result = _ttcategorycontext.TT_LABLIB_DMO.Where(t => t.TTLAB_LABLIBName.Equals(objpge.TTLAB_LABLIBName) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.TTLAB_ActiveFlag = true;
                        _ttcategorycontext.Add(objpge);
                        var contactExists = _ttcategorycontext.SaveChanges();

                        var result123 = _ttcategorycontext.TT_LABLIB_DMO.Max(t => t.TTLAB_Id);
                        if (contactExists == 1)
                        {
                            for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                            {
                                TT_LABLIB_DetailsDTO ttlldto = new TT_LABLIB_DetailsDTO();
                                TT_LABLIB_DetailsDMO ttllddmo = Mapper.Map<TT_LABLIB_DetailsDMO>(ttlldto);
                                ttllddmo.TTLAB_Id = result123;
                                ttllddmo.ASMAY_Id = objpge.ASMAY_Id;
                                ttllddmo.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                ttllddmo.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                ttllddmo.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                ttllddmo.CreatedDate = DateTime.Now;
                                ttllddmo.UpdatedDate = DateTime.Now;
                                ttllddmo.TTLABD_ActiveFlag = true;
                                _ttcategorycontext.Add(ttllddmo);
                                var contactExists1 = _ttcategorycontext.SaveChanges();
                                if (contactExists1 == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _ttcategorycontext.Remove(_ttcategorycontext.TT_LABLIB_DMO.Where(t => t.TTLAB_Id.Equals(result123)));
                                    _category.returnval = false;
                                }
                            }
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
                _category.returnval = false;
            }
            return _category;
        }

        public TT_LABLIB_DTO getdetails(int id)
        {
            TT_LABLIB_DTO TTMC = new TT_LABLIB_DTO();
            try
            {

                TTMC.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active.Equals(true)).OrderByDescending(y=>y.ASMAY_Order).ToList().ToArray();
                TTMC.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();

                TTMC.classDrpDwn = _ttcategorycontext.School_M_Class.Where(c => c.ASMCL_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList().ToArray();

                TTMC.sectDrpDwn = _ttcategorycontext.School_M_Section.Where(c => c.ASMC_ActiveFlag.Equals(1) && c.MI_Id.Equals(id)).ToList().ToArray();

                TTMC.subjDrpDwn = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id.Equals(id)).ToList().ToArray();

                TTMC.labdetailsarray = (from TT_Master_Category in _ttcategorycontext.TTMasterCategoryDMO
                                        from Adm_School_M_Academic_Year in _ttcategorycontext.AcademicYear
                                        from TT_LABLIB in _ttcategorycontext.TT_LABLIB_DMO
                                        where (TT_LABLIB.MI_Id.Equals(id) && TT_LABLIB.ASMAY_Id.Equals(Adm_School_M_Academic_Year.ASMAY_Id) && TT_Master_Category.TTMC_Id.Equals(TT_LABLIB.TTMC_Id))
                                        select new TT_LABLIB_DTO
                                        {
                                            TTLAB_Id = TT_LABLIB.TTLAB_Id,
                                            ASMAYYear = Adm_School_M_Academic_Year.ASMAY_Year,
                                            CategoryName = TT_Master_Category.TTMC_CategoryName,
                                            TTLAB_LABLIBName = TT_LABLIB.TTLAB_LABLIBName,
                                            TTLAB_ActiveFlag = TT_LABLIB.TTLAB_ActiveFlag
                                        }
                                      ).ToArray();


            }
            catch (Exception ee)
            {
                TTMC.returnval = false;
            }
            return TTMC;

        }

        public TT_LABLIB_DTO getpageedit(int id)
        {
            TT_LABLIB_DTO page = new TT_LABLIB_DTO();
            try
            {
                page.labconsedit = _ttcategorycontext.TT_LABLIB_DMO.AsNoTracking().Where(t => t.TTLAB_Id.Equals(id)).ToList().ToArray();
                page.labconsdetailsedit = _ttcategorycontext.TT_LABLIB_DetailsDMO.AsNoTracking().Where(t => t.TTLAB_Id.Equals(id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                page.returnval = false;
            }
            return page;
        }
        public TT_LABLIB_DTO deleterec(int id)
        {
            TT_LABLIB_DTO page = new TT_LABLIB_DTO();
            try
            {
                List<TT_LABLIB_DetailsDMO> lorg1 = new List<TT_LABLIB_DetailsDMO>();
                lorg1 = _ttcategorycontext.TT_LABLIB_DetailsDMO.Where(t => t.TTLAB_Id.Equals(id)).ToList();
                if (lorg1.Any())
                {
                    for (int i = 0; i < lorg1.Count; i++)
                    {
                        _ttcategorycontext.Remove(lorg1.ElementAt(i));
                        var contactExists1 = _ttcategorycontext.SaveChanges();
                        if (contactExists1 == 1)
                        {
                            page.returnval = true;
                        }
                        else
                        {
                            page.returnval = false;
                        }
                    }
                }

                List<TT_LABLIB_DMO> lorg = new List<TT_LABLIB_DMO>();
                lorg = _ttcategorycontext.TT_LABLIB_DMO.Where(t => t.TTLAB_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _ttcategorycontext.Remove(lorg.ElementAt(0));
                    var contactExists = _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                page.returnval = false;
            }
            return page;
        }
        public TT_LABLIB_DTO deletepagesRightgrid(long id)
        {
            TT_LABLIB_DTO pagert = new TT_LABLIB_DTO();
            TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {
                List<TT_LABLIB_DetailsDMO> lorg = new List<TT_LABLIB_DetailsDMO>();
                lorg = _ttcategorycontext.TT_LABLIB_DetailsDMO.Where(t => t.TTLAB_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ttcategorycontext.Remove(lorg.ElementAt(i));
                        var contactExists = _ttcategorycontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                pagert.returnval = false;
            }
            return pagert;
        }
        public TT_LABLIB_DTO getalldetailsviewrecords(int id)
        {
            TT_LABLIB_DTO TTMC = new TT_LABLIB_DTO();
            try
            {
                TTMC.labdetilspopuparray = (from TTLABLIB in _ttcategorycontext.TT_LABLIB_DMO
                                            from Adm_School_M_Class in _ttcategorycontext.School_M_Class
                                            from School_M_Section in _ttcategorycontext.School_M_Section
                                            from Adm_M_Subject in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                            from TT_LABLIBDetails in _ttcategorycontext.TT_LABLIB_DetailsDMO
                                            where (TTLABLIB.TTLAB_Id.Equals(TT_LABLIBDetails.TTLAB_Id) && TT_LABLIBDetails.TTLAB_Id.Equals(id) && TT_LABLIBDetails.ASMCL_Id.Equals(Adm_School_M_Class.ASMCL_Id) &&
                                            TT_LABLIBDetails.ASMS_Id.Equals(School_M_Section.ASMS_Id) && TT_LABLIBDetails.ISMS_Id.Equals(Adm_M_Subject.ISMS_Id))
                                            select new TT_LABLIB_DTO
                                            {
                                                TTLAB_Id = Convert.ToInt64(id),
                                                TTLAB_LABLIBName = TTLABLIB.TTLAB_LABLIBName,
                                                classname = Adm_School_M_Class.ASMCL_ClassName,
                                                sectionname = School_M_Section.ASMC_SectionName,
                                                subjectname = Adm_M_Subject.ISMS_SubjectName
                                            }
                                      ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TT_LABLIB_DTO getclass_catg(TT_LABLIB_DTO data)
        {

            try
            {
                data.classbycategory = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                        from b in _ttcategorycontext.TT_Category_Class_DMO
                                        from c in _ttcategorycontext.School_M_Class
                                        where (a.MI_Id.Equals(b.MI_Id) && a.MI_Id.Equals(c.MI_Id) && a.MI_Id.Equals(data.MI_Id) && a.TTMC_Id.Equals(b.TTMC_Id) && b.ASMCL_Id.Equals(c.ASMCL_Id) && a.TTMC_Id.Equals(data.TTMC_Id))
                                        select new TT_LABLIB_DTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            TTMC_Id = a.TTMC_Id,
                                            TTMC_CategoryName = a.TTMC_CategoryName,
                                        }
      ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
        public TT_LABLIB_DTO deactivate(TT_LABLIB_DTO acd)
        {
            try
            {
                TT_LABLIB_DMO pge = Mapper.Map<TT_LABLIB_DMO>(acd);
                if (pge.TTLAB_Id > 0)
                {
                    var result = _ttcategorycontext.TT_LABLIB_DMO.Single(t => t.TTLAB_Id.Equals(pge.TTLAB_Id));
                    if (result.TTLAB_ActiveFlag.Equals(true))
                    {
                        result.TTLAB_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTLAB_ActiveFlag = true;
                    }
                    _ttcategorycontext.Update(result);
                    var flag = _ttcategorycontext.SaveChanges();
                    if (flag.Equals(1))
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
                acd.returnval = false;
            }
            return acd;
        }
        public TT_LABLIB_DTO get_catg(TT_LABLIB_DTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TT_LABLIB_DTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }
    }
}
