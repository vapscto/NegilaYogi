﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class MasterDriverDTO
    {
        public long TRMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMD_DriverName { get; set; }
        public string TRMD_DriverCode { get; set; }
        public string TRMD_DLNo { get; set; }
        public string TRMD_RTOName { get; set; }
        public DateTime TRMD_DLExpiryDate { get; set; }
        public DateTime? TRMD_MTExpiryDate { get; set; }
        public DateTime? TRMD_SDExpiryDate { get; set; }
        public string TRMD_DriverBadgeNo { get; set; }
        public string TRMD_LicenseType { get; set; }
        public bool TRMD_SpareDriverFlg { get; set; }
        public bool TRMD_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array getdatamaster { get; set; }
        public Array getdatamasteredit { get; set; }
        public long? TRMD_MobileNo { get; set; }
        public string TRMD_EmailId { get; set; }
    }
}