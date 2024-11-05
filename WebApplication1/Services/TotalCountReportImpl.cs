using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Text;
using System.IO;

namespace WebApplication1.Services
{
    public class TotalCountReportImpl : Interfaces.TotalCountReportInterface
    {
        private static ConcurrentDictionary<string, StudentDetailsDTO> _login =
         new ConcurrentDictionary<string, StudentDetailsDTO>();

        private readonly TotalCountReportContext _TotalCountReportContext;
        private readonly UserManager<ApplicationUser> _UserManager;

        public TotalCountReportImpl(TotalCountReportContext TotalCountReportContext, UserManager<ApplicationUser> UserManager)
        {
            _TotalCountReportContext = TotalCountReportContext;
            _UserManager = UserManager;
        }

        //public WrittenTestMarksBindDataDTO GetData(int ID)//int IVRMM_Id
        //{
        //    WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO = new WrittenTestMarksBindDataDTO();

        //    Array[] showdata3 = new Array[1];
        //    List<MasterConfiguration> Allname3 = new List<MasterConfiguration>();
        //    Allname3 = _TotalCountReportContext.MasterConfiguration.Where(t => t.MI_Id.Equals(ID)).ToList().ToList();
        //    WrittenTestMarksBindDataDTO.MasterConfiguration = Allname3.ToArray();
        //    WrittenTestMarksBindDataDTO.WrittenTestScheduleAppFlag = Allname3[0].ISPAC_WrittenTestApplFlag;

        //    Array[] showdata2 = new Array[1];
        //    List<MasterSubjectDMO> Allname2 = new List<MasterSubjectDMO>();
        //    Allname2 = _TotalCountReportContext.MasterSubjectDMO.ToList();
        //    WrittenTestMarksBindDataDTO.SubjectNames = Allname2.ToArray();

        //    Array[] showdata1 = new Array[1];
        //    List<StudentDetailsDMO> Allname1 = new List<StudentDetailsDMO>();
        //    Allname1 = _TotalCountReportContext.StudentDetailsDMO.ToList();
        //    WrittenTestMarksBindDataDTO.studentDetails = Allname1.ToArray();

        //    Array[] showdata = new Array[50];
        //    List<ScheduleNameDMO> Allname = new List<ScheduleNameDMO>();
        //    Allname = _TotalCountReportContext.ScheduleNameDMO.ToList();
        //    WrittenTestMarksBindDataDTO.WrittenTestSchedule = Allname.ToArray();

        //    return WrittenTestMarksBindDataDTO;
        //}


        //public WrittenTestMarksBindDataDTO[] Getdetails(WrittenTestMarksBindDataDTO mas)
        //{


        //    List<WrittenTestMarksBindDataDTO> AllInOne = new List<WrittenTestMarksBindDataDTO>();


        //    List<ApplicationUser> Allname4 = new List<ApplicationUser>();
        //    if (Convert.ToString(mas.From_Date) != "00/00/0000 00:00:00" && Convert.ToString(mas.To_Date) != "00/00/0000 00:00:00")
        //    {
        //        ApplicationUser user = new ApplicationUser { Entry_Date = mas.From_Date };

        //     //   Allname4 = _TotalCountReportContext.ApplicationUser.Where(t => t.Entry_Date >= (mas.From_Date) && t.Entry_Date <= (mas.To_Date)).ToList().ToList();              
        //    }
        //    else if (Convert.ToString(mas.From_Date) == "00/00/0000 00:00:00")
        //    {
        //        Allname4 = _TotalCountReportContext.ApplicationUser.Where(t => t.Entry_Date >= (mas.From_Date)).ToList().ToList();
        //    }
        //    else if (Convert.ToString(mas.To_Date) == "00/00/0000 00:00:00")
        //    {
        //        Allname4 = _TotalCountReportContext.ApplicationUser.Where(t => t.Entry_Date <= (mas.To_Date)).ToList().ToList();
        //    }

        //    mas.studentDetails = Allname4.ToArray();

        //    int count1 = 0;

        //    if (Allname4.Count > 0)
        //    {


        //        for (int p = 0; p < count1; p++)
        //        {
        //            List<MasterSubjectDMO> Allname3 = new List<MasterSubjectDMO>();
        //            if (mas.PAMS_Id == 0)
        //            {
        //                Allname3 = _TotalCountReportContext.MasterSubjectDMO.ToList();
        //            }
        //            else
        //            {
        //                Allname3 = _TotalCountReportContext.MasterSubjectDMO.Where(t => t.PAMS_Id.Equals(mas.PAMS_Id)).ToList().ToList();
        //            }
        //            mas.SelectedSubjectNames = Allname3.ToArray();

        //            List<WIrttenTestSubjectWiseMarksDMO> Allname2 = new List<WIrttenTestSubjectWiseMarksDMO>();
        //            if (mas.PAMS_Id == 0)
        //            {
        //                Allname2 = _TotalCountReportContext.WIrttenTestSubjectWiseMarksDMO.ToList();
        //            }
        //            else
        //            {
        //                Allname2 = _TotalCountReportContext.WIrttenTestSubjectWiseMarksDMO.Where(t => t.PAMS_Id.Equals(mas.PAMS_Id) && t.MI_Id.Equals(mas.MI_Id) && t.ASMAY_Id.Equals(mas.ASMAY_Id)).ToList().ToList();
        //            }


        //            List<WrittenTestStudentSubjectWiseMarksDMO> Allname1 = new List<WrittenTestStudentSubjectWiseMarksDMO>();
        //            if (mas.PAMS_Id == 0)
        //            {
        //                Allname1 = _TotalCountReportContext.WrittenTestStudentSubjectWiseMarksDMO.Where(t => t.PASR_Id.Equals(mas.PASR_Id)).ToList().ToList();
        //            }
        //            else
        //            {
        //                Allname1 = _TotalCountReportContext.WrittenTestStudentSubjectWiseMarksDMO.Where(t => t.PASWM_Id.Equals(Allname2[0].PASWM_Id) && t.PASR_Id.Equals(mas.PASR_Id)).ToList().ToList();
        //            }
        //            mas.WirettenTestSubjectWiseStudentMarks = Allname1.ToArray();




        //            int count = 0;

        //            if (Allname3.Count > Allname1.Count)
        //            {
        //                count = Allname3.Count;
        //            }
        //            else
        //            {
        //                count = Allname1.Count;
        //            }

        //            for (int i = 0; i < count; i++)
        //            {
        //                WrittenTestMarksBindDataDTO temp = new WrittenTestMarksBindDataDTO();

        //                List<StudentApplication> Allname10 = new List<StudentApplication>();
        //                Allname10 = _TotalCountReportContext.StudentApplication.Where(t => t.Id.Equals(Allname4[p].Id)).ToList().ToList();
        //                mas.studentDetails = Allname10.ToArray();


        //                temp.PASR_FirstName = Allname10[0].PASR_FirstName;
        //                temp.PASR_MiddleName = Allname10[0].PASR_MiddleName;
        //                temp.PASR_LastName = Allname10[0].PASR_LastName;
        //                //temp.PASR_Id = Allname10[0].PASR_Id;

        //                if (i < Allname1.Count)
        //                {

        //                    temp.ObtMarks = Allname1[i].PASWMS_MarksScored;
        //                }
        //                else
        //                {
        //                    temp.ObtMarks = 0;
        //                }

        //                if (i < Allname3.Count)
        //                {
        //                    temp.PAMS_SubjectName = Allname3[i].PAMS_SubjectName;
        //                    temp.PAMS_MaxMarks = Allname3[i].PAMS_MaxMarks;
        //                    temp.PAMS_Id = Allname3[i].PAMS_Id;
        //                }
        //                else
        //                {
        //                    temp.PAMS_SubjectName = "";
        //                    temp.PAMS_MaxMarks = 0;
        //                    temp.PAMS_Id = 0;
        //                }

        //                AllInOne.Add(temp);
        //            }



        //        }
        //    }


        //    return AllInOne.ToArray();

        //}

    }
}
