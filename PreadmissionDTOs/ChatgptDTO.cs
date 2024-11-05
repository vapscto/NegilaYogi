using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class ChatgptDTO
    {
        public string message { get; set; }
        public string ChatCompletion { get; set; }

        public string MobileNumber { get; set; }
        public string BirthdayName { get; set; }
        public string InstituteNameB { get; set; }
        public string Attendance1 { get; set; }
        public string Attendance2 { get; set; }

        public string Attendance3 { get; set; }
        public string Attendance4 { get; set; }
        public string Attendance5 { get; set; }
        public string FeeDue { get; set; }
        public string Board { get; set; }

        public string FeeTransaction1 { get; set; }

        public string FeeTransaction2 { get; set; }
        public string FeeTransaction3 { get; set; }
        public string FeeTransaction4 { get; set; }
        public string FeeTransaction5 { get; set; }

        public string LoginCredentials1 { get; set; }

        public string LoginCredentials2 { get; set; }
        public string LoginCredentials3 { get; set; }
        public string LoginCredentials4 { get; set; }
        public string LoginCredentials5 { get; set; }
       
        public string Typedropdown { get; set; }

        public string id { get; set; }
        public string model { get; set; }
        public string created { get; set; }
        public string objectname { get; set; }
        public choiceslist[] choices { get; set; }
        public usagelist[] usage { get; set; }



    }

    public class choiceslist
    {
        public long index { get; set; }
        public messagelist[] message { get; set; }
        public string finish_reason { get; set; }
    }
    public class messagelist
    {
        public string role { get; set; }
        public string content { get; set; }
    }
    public class usagelist
    {
        public long prompt_tokens { get; set; }
        public long completion_tokens { get; set; }
        public long total_tokens { get; set; }
    }

}
