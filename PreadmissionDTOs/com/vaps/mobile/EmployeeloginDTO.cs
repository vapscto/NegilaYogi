﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class EmployeeloginDTO
    {
        public class input
        {
           public string IVRMSTAUL_UserName { get; set; }
            public string IVRMSTAUL_Password { get; set; }
            public long MI_Id { get; set; }
        }
        
        public Array Employee { get; set; }
        public class output
        {
            //public Array EmpDetails { get; set; }
            public long HRME_Id { get; set; }
            public long MI_Id { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
            public string HRME_EmployeeMiddleName { get; set; }
            public string HRME_EmployeeLastName { get; set; }
            public string HRME_EmployeeCode { get; set; }
            public DateTime? HRME_DOB { get; set; }
            public string HRMEM_EmailId { get; set; }
            public long Emp_MobileNo { get; set; }
            //     public long AMST_MobileNo { get; set; }
            public DateTime? HRME_DOJ { get; set; }
            public string HRME_Photo { get; set; }


            public string Hrme_designation { get; set; }

            public string hrme_deptname { get; set; }

        }
       
    }
}

