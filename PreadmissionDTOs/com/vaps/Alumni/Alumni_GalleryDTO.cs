using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class Alumni_GalleryDTO
    {
        public long ALGA_Id { get; set; }
        public long ALGAP_Id { get; set; }
        public long ALGAV_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string Role_flag { get; set; }
        public string ALGA_GalleryName { get; set; }
        public DateTime ALGA_Date { get; set; }
        public string ALGA_Time { get; set; }
        public string ALGA_CommonGalleryFlg { get; set; }
        public long ALGA_CreatedBy { get; set; }
        public long ALGA_UpdatedBy { get; set; }
        public bool ALGA_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string mediatype { get; set; }
        public string roleflg { get; set; }
        public images_list1[] images_list { get; set; }
        public Array roletype { get; set; }
        public Array get_galleryimg { get; set; }
        public Array view_galleryimg { get; set; }
        public class images_list1
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
        }
    }
}
