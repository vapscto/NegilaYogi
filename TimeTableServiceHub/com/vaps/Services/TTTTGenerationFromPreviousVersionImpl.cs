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
    public class TTTTGenerationFromPreviousVersionImpl:Interfaces.TTGenerationFromPreviousVersionInterface
    {
        private static ConcurrentDictionary<string, TTTTGenerationFromPreviousVersionDTO> _login =
       new ConcurrentDictionary<string, TTTTGenerationFromPreviousVersionDTO>();


        public TTContext _ttcategorycontext;
        public TTTTGenerationFromPreviousVersionImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }

        public TTTTGenerationFromPreviousVersionDTO savedetail_(TTTTGenerationFromPreviousVersionDTO _category)
        {
            try
            {

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTTTGenerationFromPreviousVersionDTO getdetails(int id)
        {
            TTTTGenerationFromPreviousVersionDTO TTMC = new TTTTGenerationFromPreviousVersionDTO();
            try
            {

                List<TT_Final_GenerationDMO> versions = new List<TT_Final_GenerationDMO>();
                versions = _ttcategorycontext.TT_Final_GenerationDMO.AsNoTracking().Where(v => v.MI_Id == id && v.TTFG_ActiveFlag == true).ToList();
                TTMC.versionlist = versions.Distinct().GroupBy(x=>x.TTFG_VersionNo).Select(x=>x.First()).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTTTGenerationFromPreviousVersionDTO getpageedit(int id)
        {
            TTTTGenerationFromPreviousVersionDTO page = new TTTTGenerationFromPreviousVersionDTO();
            try
            {

                List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
                lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.AsNoTracking().Where(t => t.TTMSAB_Id.Equals(id)).ToList();
                page.sujectslistedit = lorg.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTTTGenerationFromPreviousVersionDTO deleterec(int id)
        {
            TTTTGenerationFromPreviousVersionDTO page = new TTTTGenerationFromPreviousVersionDTO();
            try
            {
                List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
                lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Id.Equals(id)).ToList();
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

        //active deactive 
        public TTTTGenerationFromPreviousVersionDTO deactivate(TTTTGenerationFromPreviousVersionDTO data)
        {
            TT_Master_Staff_AbbreviationDMO pge = Mapper.Map<TT_Master_Staff_AbbreviationDMO>(data);
            if (pge.TTMSAB_Id > 0)
            {
                var result = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Single(t => t.TTMSAB_Id == pge.TTMSAB_Id);
                if (result.TTMSAB_ActiveFlag == true)
                {
                    result.TTMSAB_ActiveFlag = false;
                }
                else
                {
                    result.TTMSAB_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ttcategorycontext.Update(result);
                var flag = _ttcategorycontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }



    }
}
