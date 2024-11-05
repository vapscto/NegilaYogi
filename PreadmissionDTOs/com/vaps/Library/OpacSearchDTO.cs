using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class OpacSearchDTO
    {
        public Array subjectlist { get; set; }
        public Array publisherlist { get; set; }
        public Array authorlist { get; set; }
        public Array reportlist { get; set; }
        public long LMS_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LMAU_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string authorname { get; set; }
        public string LMP_PublisherName { get; set; }
        public long MI_Id { get; set; }
        public long ExactMatch { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string SubjectId { get; set; }
        public string PublisherId { get; set; }
        public string AccessionNo { get; set; }
        public string CallNo { get; set; }
        public string TYPE { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string ClassNo { get; set; }
        public string ISBNNO { get; set; }



    }
}
