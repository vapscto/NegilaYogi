using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
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
    public class TT_Master_Subject_AbbreviationImpl:Interfaces.TT_Master_Subject_AbbreviationInterface
    {
        private static ConcurrentDictionary<string, TT_Master_Subject_AbbreviationDTO> _login =
       new ConcurrentDictionary<string, TT_Master_Subject_AbbreviationDTO>();


        public TTContext _ttcategorycontext;
        public TT_Master_Subject_AbbreviationImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }

        public TT_Master_Subject_AbbreviationDTO savedetail(TT_Master_Subject_AbbreviationDTO _category)
        {
            TT_Master_Subject_AbbreviationDMO objpge = Mapper.Map<TT_Master_Subject_AbbreviationDMO>(_category);
            try
            {
                if (objpge.TTMSUAB_Id > 0)
                {
                    var result1 = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Where(t => t.TTMSUAB_Abbreviation.Equals(objpge.TTMSUAB_Abbreviation) && t.MI_Id.Equals(objpge.MI_Id)
                    //&& t.ASMAY_Id.Equals(objpge.ASMAY_Id) 
                    && t.TTMSUAB_Id!= objpge.TTMSUAB_Id);
                    if (result1.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Single(t => t.TTMSUAB_Id.Equals(objpge.TTMSUAB_Id) && t.MI_Id.Equals(objpge.MI_Id) 
                        //&& t.ASMAY_Id.Equals(objpge.ASMAY_Id)
                        );
                        result.TTMSUAB_Abbreviation = objpge.TTMSUAB_Abbreviation;
                        result.ISMS_Id = objpge.ISMS_Id;
                        objpge.TTMSUAB_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _ttcategorycontext.Update(result);
                        var contactExists = _ttcategorycontext.SaveChanges();
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
                    var result = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Where(t => t.TTMSUAB_Abbreviation.Equals(objpge.TTMSUAB_Abbreviation) && t.MI_Id.Equals(objpge.MI_Id) 
                  //  && t.ASMAY_Id.Equals(objpge.ASMAY_Id)
                    );
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result1 = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) 
                        //&& t.ASMAY_Id.Equals(objpge.ASMAY_Id)
                        );
                        if (result1.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTMSUAB_ActiveFlag = true;
                            _ttcategorycontext.Add(objpge);
                            var contactExists = _ttcategorycontext.SaveChanges();
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TT_Master_Subject_AbbreviationDTO getdetails(int id)
        {
            TT_Master_Subject_AbbreviationDTO TTMC = new TT_Master_Subject_AbbreviationDTO();
            try
            {
                List<IVRM_School_Master_SubjectsDMO> feegrp = new List<IVRM_School_Master_SubjectsDMO>();
                feegrp = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id.Equals(id)).ToList(); //  && t.TimeTable_flag.Equals("Y")
                TTMC.sujectslist = feegrp.ToArray();

                 TTMC.ttsujectslist = (from a in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from b in _ttcategorycontext.TT_Master_Subject_AbbreviationDMO
                                     //  from a in _ttcategorycontext.AcademicYear
                                       where (a.ISMS_Id.Equals(b.ISMS_Id) && a.MI_Id.Equals(id) 
                                       //&& a.ASMAY_Id.Equals(TT_Master_Subject_AbbreviationDMO.ASMAY_Id)
                                       )
                                        select new TT_Master_Subject_AbbreviationDTO
                                        {
                                            //academicyear=a.ASMAY_Year,
                                            subjectName= a.ISMS_SubjectName,
                                            TTMSUAB_Id= b.TTMSUAB_Id,
                                            TTMSUAB_Abbreviation = b.TTMSUAB_Abbreviation,
                                            TTMSUAB_ActiveFlag= b.TTMSUAB_ActiveFlag,
                                            ISMS_Id=a.ISMS_Id
                                        }
                                        ).Distinct().ToArray();               

            }
            catch (Exception ee)
            {
               Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public TT_Master_Subject_AbbreviationDTO getpageedit(int id)
        {
            TT_Master_Subject_AbbreviationDTO page = new TT_Master_Subject_AbbreviationDTO();
            try
            {
                page.sujectslistedit = (from Adm_M_Subject in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                        from TT_Master_Subject_AbbreviationDMO in _ttcategorycontext.TT_Master_Subject_AbbreviationDMO
                                        where (Adm_M_Subject.ISMS_Id.Equals(TT_Master_Subject_AbbreviationDMO.ISMS_Id) && TT_Master_Subject_AbbreviationDMO.TTMSUAB_Id.Equals(id))
                                      select new TT_Master_Subject_AbbreviationDTO
                                      {
                                          ISMS_Id = Adm_M_Subject.ISMS_Id,
                                          TTMSUAB_Id = TT_Master_Subject_AbbreviationDMO.TTMSUAB_Id,
                                          TTMSUAB_Abbreviation = TT_Master_Subject_AbbreviationDMO.TTMSUAB_Abbreviation
                                      }
                                       ).ToArray();


            }
            catch (Exception ee)
            {
              Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_Master_Subject_AbbreviationDTO deleterec(int id)
        {
            TT_Master_Subject_AbbreviationDTO page = new TT_Master_Subject_AbbreviationDTO();
            try
            {
                List<TT_Master_Subject_AbbreviationDMO> lorg = new List<TT_Master_Subject_AbbreviationDMO>();
                lorg = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Where(t => t.TTMSUAB_Id.Equals(id)).ToList();
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
             Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TT_Master_Subject_AbbreviationDTO deactivate(TT_Master_Subject_AbbreviationDTO acd)
        {
            try
            {
                TT_Master_Subject_AbbreviationDMO pge = Mapper.Map<TT_Master_Subject_AbbreviationDMO>(acd);
                if (pge.TTMSUAB_Id > 0)
                {
                    var result = _ttcategorycontext.TT_Master_Subject_AbbreviationDMO.Single(t => t.TTMSUAB_Id.Equals(pge.TTMSUAB_Id));
                    if (result.TTMSUAB_ActiveFlag.Equals(true))
                    {
                        result.TTMSUAB_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMSUAB_ActiveFlag = true;
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
               Console.WriteLine(e.InnerException);
            }
            return acd;
        }

    }
}
