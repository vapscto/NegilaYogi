﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_HolidayWorkingDay_Type", Schema = "FO")]
    public class HolidayWorkingDayTypeDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FOHWDT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOHTWD_HolidayWDType { get; set; }
        public string FOHTWD_HolidayWDTypeFlag { get; set; }

        public bool FOHWDT_ActiveFlg { get; set; }

        public bool FOHTWD_HolidayFlag { get; set; }
    }
}
