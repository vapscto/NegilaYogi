using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class Alumni_FriendRequestDTO
    {
        public long ALSFRND_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_id { get; set; }
        public long ALSFRND_FriendsId { get; set; }
        public DateTime ALSFRND_RequestDate { get; set; }
        public DateTime ALSFRND_AcceptedDate { get; set; }
        public bool ALSFRND_ActiveFlg { get; set; }
        public string ASMAY_Year { get; set; }
        public long? ASMAY_Id_Left { get; set; }
        public string ALMST_MiddleName { get; set; }
        public string ALMST_FirstName { get; set; }
        public string ALMST_LastName { get; set; }
        public string message { get; set; }
        public string Cancel { get; set; }
        public Array alumnilist { get; set; }
        public Array searchResult { get; set; }
        public Array friendrequest { get; set; }
        public Array friendrequestlist { get; set; }
        public Array almst_profile { get; set; }
        public int count { get; set; }
        public List<Object> condition = new List<Object>();
        public List<Object> field = new List<Object>();
        public List<Object> Operator = new List<Object>();
        public List<Object> value = new List<Object>();
        public string stuStatus { get; set; }
        public requestalumni1[] requestalumni { get; set; }
        public requestalumni12[] requestalumniaccept { get; set; }

        public class requestalumni1
        {
            public long ALMST_Id { get; set; }
           
        }
         public class requestalumni12
        {
            public long ALMST_Id { get; set; }
            public DateTime ALSFRND_RequestDate { get; set; }
        }

    }
}
