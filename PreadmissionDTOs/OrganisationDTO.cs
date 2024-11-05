using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OrganisationDTO : CommonParamDTO
    {
        public long MO_Id { get; set; }
        public string IVRMMCT_Name { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public string MO_Name { get; set; }
        public string MO_Address1 { get; set; }
        public string MO_Address2 { get; set; }
        public string MO_Address3 { get; set; }
        public string MO_Landmark { get; set; }
        public int MO_Pincode { get; set; }
        public string MO_FaxNo { get; set; }
        public string MO_Website { get; set; }
        public string MO_OrganisationType { get; set; }
        public int MO_ActiveFlag { get; set; }

        public string MT_Currency { get; set; }

        public string returnduplicatestatus { get; set; }

        //public long MOE_Id { get; set; }
        //public string MOE_EmailId { get; set; }

        //public long MOMN_Id { get; set; }
        //public int MOMN_MobileNo { get; set; }

        //public long MOPN_Id { get; set; }
        //public int MOPN_PhoneNo { get; set; }

        public string defaultcurrency { get; set; }
        public string returnval { get; set; }

        public Organisation_Phone_NoDTO[] phones { get; set; }
        public Organisation_MobileDTO[] mobiles { get; set; }
        public Organisation_EmailIdDTO[] emails { get; set; }

        public Array organisationname { get; set; }

        public Array stateDrpDown { get; set; }
        public Array cityDrpDown { get; set; }

        public Array MobilearrayList { get; set; }
        public Array PhonearrayList { get; set; }
        public Array EmailarrayList { get; set; }

        public SortingPagingInfoDTO trustPagination { get; set; }

        public Array countryDrpDown { get; set; }

        public string MT_Domain_name { get; set; }

        public long RoleId { get; set; }

        public long sessionMI_Id { get; set; }

    }
}
