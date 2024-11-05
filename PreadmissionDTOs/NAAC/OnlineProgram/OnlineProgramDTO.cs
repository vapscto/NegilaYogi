using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.OnlineProgram
{
    public class OnlineProgramDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string returnvaledit { get; set; }
        public string PRYRG_GuestProfileFileName { get; set; }
        public string PRYRG_GuestProfileFilePath { get; set; }
        public string PRYRG_GuestPhotoVideo { get; set; }
        public string PRYRG_GuestPhotoVideoPath { get; set; }
        public string PRYRG_GuestSpeechFilePath { get; set; }
        public string PRYR_ProgramDescription { get; set; }
        public string PRYRA_Duration { get; set; }
        public string n1 { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address3 { get; set; }
        public string type3 { get; set; }
        public string address { get; set; }
        public string number { get; set; }
        public string type { get; set; }

        public string PRYR_WListPath { get; set; }
        public string PRYR_ProgramName { get; set; }
        public long UserId { get; set; }
        public long? PRYR_ProgramTypeId { get; set; }
        public Array programlist { get; set; }
        public Array guest { get; set; }
        public Array rmv { get; set; }
        public Array fillyear { get; set; }
        public Array fillActivities1 { get; set; }
        public Array guestgrid { get; set; }
        public Array listg { get; set; }

        public string testpath { get; set; }
        public Array uploadwinnerfiles { get; set; }
        public Array uploadfiles11 { get; set; }
        public Array uploadfiles22 { get; set; }
        public Array uploadfiles33 { get; set; }
        public Array uploadfiles44 { get; set; }
        public Array uploadfiles55 { get; set; }
        public Array uploadfiles66 { get; set; }
        public Array uploadfiles77 { get; set; }
        public Array uploadfiles88 { get; set; }
        public Array uploadfiles99 { get; set; }

        public string programname { get; set; }
        public bool returnresult { get; set; }
        public string returndt { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string description { get; set; }
        public long PRYR_Id { get; set; }
        public Array uploadfiles { get; set; }
        public Array uploadfiles1 { get; set; }
        public Array uploadfiles2 { get; set; }
        public string message { get; set; }
        public string[] images_list { get; set; }
        public string[] videos_list { get; set; }
        public Array alllist { get; set; }
        public Array Photolist { get; set; }
        public Array Videolist { get; set; }
        public string PRYRG_GuestType { get; set; }
        public string PRYRG_GuestName { get; set; }
        public string e1 { get; set; }
        public string a1 { get; set; }
        public string p1 { get; set; }
        public string t1 { get; set; }
        public string msg22 { get; set; }
        public string PRYRG_GuestPhoneNo { get; set; }
        public string PRYRG_GuestEmailId { get; set; }
        public string PRYRG_GuestProfile { get; set; }
        public string PRYRG_GuestPhoto { get; set; }
        public string PRYRG_GuestVideo { get; set; }
        public string PRYRG_GuestSpeech { get; set; }
        public string PRYRG_GuestAddress { get; set; }
        public string LPMTR_ResourceType { get; set; }
        public string LPMTR_FileName { get; set; }
        public string LPMTR_Filetype { get; set; }
        public string club { get; set; }
        public string Org_name { get; set; }
        public long PRYRG_Id { get; set; }
        public long PRYRF_Id { get; set; }
        public long PRYRA_Id { get; set; }
        public string Eventname { get; set; }
        public bool PRYRA_ActiveFlag { get; set; }
        public string PDFfile { get; set; }
        public string Imagefile { get; set; }
        public string Videofile { get; set; }
        public string level { get; set; }
        public long? strength { get; set; }
        public string GuestName { get; set; }
        public string gno { get; set; }
        public string gaddress { get; set; }
        public string gtype { get; set; }
        public string emailid { get; set; }
        public Array testarray { get; set; }
        public pgTempDTO[] pgTempDTO { get; set; }
        public pgTempDTO1[] pgTempDTO1 { get; set; }
        public pgTempDTO2[] pgTempDTO2 { get; set; }
        public pgTempDTO3[] pgTempDTO3 { get; set; }
        public pgTempDTO4[] pgTempDTO4 { get; set; }
        public pgTempDTO5[] pgTempDTO5 { get; set; }
        public pgTempDTO6[] pgTempDTO6 { get; set; }
        public pgTempDTO7[] pgTempDTO7 { get; set; }
        public pgTempDTO8[] pgTempDTO8 { get; set; }
        public pgTempDTO99[] pgTempDTO99 { get; set; }
        public long participants { get; set; }
        public long Facty { get; set; }
        public long Stud_oth { get; set; }
        public long Nat_part { get; set; }
        public long Int_part { get; set; }
        public long Rch_schl { get; set; }
        public long Oral { get; set; }
        public long Lecture { get; set; }
        public long traning { get; set; }
        public long Poster_p { get; set; }
        public long? PRYR_TotalParticipants { get; set; }
        public long? PRYR_OurCollStudents { get; set; }
        public long? PRYR_Faculty { get; set; }
        public long? PRYR_OthCollStudents { get; set; }
        public long? PRYR_NatParticipants { get; set; }
        public long? PRYR_IntParticipants { get; set; }
        public long? PRYR_ResearchScholars { get; set; }
        public long? PRYR_OralPresentation { get; set; }
        public long PRYR_LecturesNo { get; set; }
        public long? PRYR_PosterPresentation { get; set; }
        public long PRYR_TrainingNo { get; set; }
        public long? PRYR_PrgramLevel { get; set; }
        public bool Oral_1 { get; set; }
        public bool? PRYR_OralPresentationFlg { get; set; }
        public bool PRYR_LecturesFlg { get; set; }
        public bool PRYR_PosterPresentationFlg { get; set; }
        public bool PRYR_TrainingFlg { get; set; }
        public DateTime PRYR_StartDate { get; set; }
        public Array sss { get; set; }
        public string name3 { get; set; }
        public bool Lecture_1 { get; set; }
        public bool traning_1 { get; set; }
        public bool Poster_p_1 { get; set; }
        public string PRYR_SponsorAgency { get; set; }
        public string PRYR_StartTime { get; set; }
        public string PRYR_PrgramConvenor { get; set; }
        public string programtype { get; set; }
        public string departmentname { get; set; }
        public string returnval { get; set; }
        public Array filldepartment { get; set; }
        public Array Typelist { get; set; }
        public Array levellist { get; set; }
        public Array fillActivities { get; set; }
        public long? PRMTY_Id { get; set; }
        public long? PRMTLE_Id { get; set; }
        public string PRMTLE_IdDesc { get; set; }
        public string PRMTY_Iddesc { get; set; }
        public string PRMTLE_ProgramLevel { get; set; }
        public string PRMTY_ProgramType { get; set; }
        public long? HRMD_Id { get; set; }
        //public guestidsDTO[] guestidsDTO { get; set; }


    }   
    public class pgTempDTO
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }

    }
    public class pgTempDTO1
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO2
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    //public class guestidsDTO
    //{
    //    public long PRYRG_Id { get; set; }
    //}
    public class pgTempDTO3
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO4
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO5
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO6
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO7
    {
        public long PRYR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class Speech
    {
        public long PRYRG_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class Programgst
    {
        public long PRYRG_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    //public class details
    //{
    //    public string GuestName { get; set; }
    //    public string gno { get; set; }
    //    public string gaddress { get; set; }
    //public long PRYRG_Id { get; set; }
    //public string LPMTR_Resources { get; set; }
    //public string file_name { get; set; }
    //public string filetype { get; set; }
    // }
    public class profile
    {
        public long PRYRG_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO99
    {
        public long PRYRG_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string file_name { get; set; }
        public string filetype { get; set; }
    }
    public class pgTempDTO8
    {
        public long PRYRG_Id { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string type { get; set; }
        public Speech[] Speech { get; set; }
        public Programgst[] Programgst { get; set; }
        // public details[] details { get; set; }
        public profile[] profile { get; set; }
    }
   
}
