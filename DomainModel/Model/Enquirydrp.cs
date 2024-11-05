using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    public class Enquirydrp
    {
        public int IVRMMS_Id { get; set; }

        public string IVRMMS_Name { get; set; }
        public int IVRMMC_Id { get; set; }

        public string IVRMMC_CountryName { get; set; }

        public int IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }
    }
}
