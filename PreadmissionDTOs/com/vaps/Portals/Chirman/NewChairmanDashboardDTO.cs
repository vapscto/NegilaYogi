using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
   public  class NewChairmanDashboardDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        //public Array studentCount { get; set; }
        //public Array BooksCount { get; set; }
        public Array get_levl { get; set; }
        public Array getReport { get; set; }
        public string flag { get; set; }
    }
}
