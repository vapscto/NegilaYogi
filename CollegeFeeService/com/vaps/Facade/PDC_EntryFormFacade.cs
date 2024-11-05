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
    public class PDC_EntryFormFacade : Controller
    {
        public PDC_EntryFormInterface _feegrouppage;

        public PDC_EntryFormFacade(PDC_EntryFormInterface maspag)
        {
            _feegrouppage = maspag;
        }

        [HttpGet]
        public PDC_EntryFormDTO Get(PDC_EntryFormDTO mas)
        {
            return _feegrouppage.GetGroupSearchData(mas);
        }
        [Route("getpagedetails/{id:int}")]

        public PDC_EntryFormDTO getpagedetails(int id)
        {

            return _feegrouppage.getpageedit(id);
        }


        [HttpPost]
        public PDC_EntryFormDTO Post([FromBody] PDC_EntryFormDTO org)
        {

            //for (int i = 0; i < org.savetmpdata.Length; i++)
            //{
            //    int Id = Convert.ToInt32(org.TempararyArrayList[i].fmG_Id);


            //    org.FMG_Id = Id;


            _feegrouppage.SaveGroupData(org);
            //  }

            return org;
            //return _feegrouppage.SaveGroupData(org);
        }
        [Route("showdata")]
        public PDC_EntryFormDTO showdata([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppage.showdata(data);
        }

        [Route("getdetails")]
        public PDC_EntryFormDTO getorgdet([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppage.getdetails(data);
        }

        [Route("deactivate")]
        public PDC_EntryFormDTO deactivateAcdmYear([FromBody] PDC_EntryFormDTO id)
        {
            // id = 12;
            return _feegrouppage.deactivate(id);
        }

        [Route("PDCRemainder")]
        public PDC_EntryFormDTO PDCRemainder([FromBody] PDC_EntryFormDTO id)
        {
            // id = 12;
            return _feegrouppage.PDCRemainder(id);
        }

        [Route("Settlement_data")]
        public College_Student_SettlementDTO Settlement_data([FromBody] College_Student_SettlementDTO data)
        {
            // id = 12;
            return _feegrouppage.Settlement_data(data);
        }
        [Route("getbranchdetails")]
        public PDC_EntryFormDTO getbranch([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppage.getbranchdetails(data);
        }


        [Route("getsemesterdetails")]
        public PDC_EntryFormDTO getsemester([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppage.getsemesterdetails(data);
        }


        [Route("selectstudent")]
        public PDC_EntryFormDTO selectsem([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppage.selectstudent(data);
        }
    }
}
