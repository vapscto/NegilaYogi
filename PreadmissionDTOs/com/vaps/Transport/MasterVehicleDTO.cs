using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterVehicleDTO
    {
        public long TRMV_Id { get; set; }
        public long TRMVDO_Id { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long TRMVT_Id { get; set; }
        public long TRMFT_Id { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public DateTime? TRMV_PurchaseDate { get; set; }
        public string TRMV_ChassisNo { get; set; }
        public string TRMV_EngineNo { get; set; }
        public string TRMV_PurchasedFrom { get; set; }
        public decimal TRMV_Cost { get; set; }
        public int TRMV_Capacity { get; set; }
        public string TRMV_Desc { get; set; }
        public string TRMV_VehicleImage { get; set; }
        public bool TRMV_ActiveFlag { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }
        public Array getloaddata { get; set; }
        public Array geteditdata { get; set; }
        public Array editfiles { get; set; }
        public Array getfuletype { get; set; }
        public Array getvehicletype { get; set; }
        public string TRMV_CompanyName { get; set; }

        public DateTime? TRMV_RegistrationDate { get; set; }
        public string TRMV_SWDOff { get; set; }
        public string TRMV_Address { get; set; }
        public string TRMV_Model { get; set; }
        public string TRMV_Fuel { get; set; }
        public string TRMV_Make { get; set; }
        public string TRMV_Manufacturer { get; set; }
        public DateTime? TRMV_ManufacturedDate { get; set; }
        public string TRMV_Class { get; set; }
        public string TRMV_Color { get; set; }
        public string TRMV_Body { get; set; }
        public long? TRMV_NoOfCylinder { get; set; }
        public string TRMV_WheelBase { get; set; }
        public decimal? TRMV_UnladenWeight { get; set; }
        public long? TRMV_Seating { get; set; }
        public string TRMV_CC { get; set; }
        public DateTime? TRMV_RegFCUpToDate { get; set; }
        public string TRMV_TaxUpTo { get; set; }
        public string TRMV_OwnersName { get; set; }
       public vehicleids[] vclids { get; set; }
       public Array rcreport { get; set; }
       public bool returnval { get; set; }

        public MasterVehicledocDTO[] filelist { get; set; }

    }

    public class vehicleids {
        public long TRMV_Id { get; set; }
    }

    public class MasterVehicledocDTO
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string name { get; set; }
    }
}
