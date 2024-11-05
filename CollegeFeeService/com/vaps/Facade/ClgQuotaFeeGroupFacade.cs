using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class ClgQuotaFeeGroupFacade : Controller
    {
       public ClgQuotaFeeGroupInterface _feegrouppage;

            public ClgQuotaFeeGroupFacade(ClgQuotaFeeGroupInterface maspag)
            {
                _feegrouppage = maspag;
            }

            [HttpGet]
            public ClgQuotaFeeGroupDTO Get(ClgQuotaFeeGroupDTO mas)
            {
                return _feegrouppage.GetGroupSearchData(mas);
            }
            [Route("getpagedetails/{id:int}")]
          
            public ClgQuotaFeeGroupDTO getpagedetails(int id)
            {
              
                return _feegrouppage.getpageedit(id);
            }

            
            [HttpPost]
            public ClgQuotaFeeGroupDTO Post([FromBody] ClgQuotaFeeGroupDTO org)
            {

            for (int i = 0; i < org.TempararyArrayList.Length; i++)
            {
                int Id = Convert.ToInt32(org.TempararyArrayList[i].fmG_Id);


                org.FMG_Id = Id;


                _feegrouppage.SaveGroupData(Id, org);
            }

            return org;
            //return _feegrouppage.SaveGroupData(org);
            }
            [Route("getdetails")]
            public ClgQuotaFeeGroupDTO getorgdet([FromBody] ClgQuotaFeeGroupDTO data)
            {
                return _feegrouppage.getdetails(data);
            }

            [Route("deactivate")]
            public ClgQuotaFeeGroupDTO deactivateAcdmYear([FromBody] ClgQuotaFeeGroupDTO id)
            {
                // id = 12;
                return _feegrouppage.deactivate(id);
            }
            //[Route("yearsbind")]
            //public Task<ClgQuotaFeeGroupDTO> Gets([FromBody] ClgQuotaFeeGroupDTO enqo)
            //{
            //    return _feegrouppage.getIndependentDropDowns(enqo);
            //}
      
            //[HttpDelete]
            //[Route("deletedetails/{id:int}")]
            //public ClgQuotaFeeGroupDTO Deleterec(int id)
            //{
            //    return _feegrouppage.deleterec(id);
            //}
           

        }
    }
