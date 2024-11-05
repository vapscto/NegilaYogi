using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTExistingToNewImpl : TTExistingToNewInterface
    {

        private readonly TTContext _ttcontext;

        public TTExistingToNewImpl(TTContext obj)
        {
            _ttcontext = obj;
        }

        public TTExistingToNewDTO getdetails(TTExistingToNewDTO data)
        {
            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.Acdlist = year.Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
