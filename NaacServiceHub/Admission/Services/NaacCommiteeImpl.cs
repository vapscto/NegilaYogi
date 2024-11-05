using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NaacCommiteeImpl : Interface.NaacCommiteeInterface
    {
        public GeneralContext _context;
        public NaacCommiteeImpl(GeneralContext i)
        {
            _context = i;
        }
        public NAAC_AC_Committee_DTO loaddata(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }

                data.yearlist = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Year).ToArray();
                data.filldepartment = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_AC_Committee_DMO
                                     //from c in _context.NAAC_AC__Committee_Members_DMO

                                 where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCACCOMM_Year)
                                 select new NAAC_AC_Committee_DTO
                                 {
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCACCOMM_Id = b.NCACCOMM_Id,
                                     NCACCOMM_ActiveFlg = b.NCACCOMM_ActiveFlg,
                                     NCACCOMM_CommitteeName = b.NCACCOMM_CommitteeName,
                                     NCACCOMM_Flg = b.NCACCOMM_Flg,
                                     NCACCOMM_StaffFlg = b.NCACCOMM_StaffFlg,
                                     MI_Id = data.MI_Id,
                                     NCACCOMM_StatusFlg = b.NCACCOMM_StatusFlg,
                                     //  NCACCOMMM_MemberName =c.HRME_Id=="" c.NCACCOMMM_MemberName,
                                     // NCACCOMMM_MemberDetails = c.NCACCOMMM_MemberDetails,
                                     // =c.NCACCOMMM_MemberEmailId,
                                     //NCACCOMMM_Role = c.NCACCOMMM_Role,
                                     //NCACCOMMM_MemberName= c.HRME_Id != 0 ? _context.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == c.HRME_Id).FirstOrDefault().HRME_EmployeeFirstName : c.NCACCOMMM_MemberName,

                                     //NCACCOMMM_MemberEmailId=c.HRME_Id!=0? _context.Multiple_Email_DMO.Where(t=>t.HRME_Id==c.HRME_Id).FirstOrDefault().HRMEM_EmailId :c.NCACCOMMM_MemberEmailId,
                                 }).Distinct().ToArray();

                //(a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO saverecord(NAAC_AC_Committee_DTO data)
        {
            try
            {
                if (data.NCACCOMM_Id == 0)
                {
                    NAAC_AC_Committee_DMO obj1 = new NAAC_AC_Committee_DMO();
                    obj1.MI_Id = data.MI_Id;
                    obj1.NCACCOMM_ActiveFlg = true;
                    obj1.NCACCOMM_CreatedBy = data.UserId;
                    obj1.NCACCOMM_UpdatedBy = data.UserId;
                    obj1.NCACCOMM_CreatedDate = DateTime.Now;
                    obj1.NCACCOMM_UpdatedDate = DateTime.Now;
                    obj1.NCACCOMM_Year = data.ASMAY_Id;
                    obj1.NCACCOMM_Flg = data.NCACCOMM_Flg;
                    obj1.NCACCOMM_StatusFlg = "";
                    obj1.MI_Id = data.MI_Id;
                    obj1.NCACCOMM_CommitteeName = data.NCACCOMM_CommitteeName;
                    if (data.all1 == "11")
                    {
                        obj1.NCACCOMM_StaffFlg = "EXStaf";
                    }
                    else
                    {
                        obj1.NCACCOMM_StaffFlg = "NEWStaf";
                    }
                    _context.Add(obj1);
                    if (data.filelist.Length > 0)
                    {
                        for (int i = 0; i < data.filelist.Length; i++)
                        {
                            if (data.filelist[i].cfilepath != null)
                            {


                                NAAC_AC_Committee_Files_DMO objfiles = new NAAC_AC_Committee_Files_DMO();
                                objfiles.NCACCOMM_Id = obj1.NCACCOMM_Id;
                                objfiles.NCACCOMMF_FileName = data.filelist[i].cfilename;
                                objfiles.NCACCOMMF_FileDesc = data.filelist[i].cfiledesc;
                                objfiles.NCACCOMMF_FilePath = data.filelist[i].cfilepath;
                                objfiles.NCACCOMMF_StatusFlg = "";
                                objfiles.NCACCOMMF_ActiveFlg = true;
                                _context.Add(objfiles);
                            }
                        }
                    }

                    if (data.all1 == "11")
                    {
                        for (int e = 0; e < data.selectdStafflist.Length; e++)
                        {
                            NAAC_AC__Committee_Members_DMO obj2 = new NAAC_AC__Committee_Members_DMO();
                            obj2.NCACCOMM_Id = obj1.NCACCOMM_Id;
                            obj2.NCACCOMMM_Role = data.NCACCOMMM_Role;
                            obj2.NCACCOMMM_ActiveFlg = true;
                            obj2.NCACCOMMM_CreatedBy = data.UserId;
                            obj2.NCACCOMMM_UpdatedBy = data.UserId;
                            obj2.NCACCOMMM_CreatedDate = DateTime.Now;
                            obj2.NCACCOMMM_UpdatedDate = DateTime.Now;
                            obj2.NCACCOMMM_StatusFlg = "";


                            obj2.HRME_Id = data.selectdStafflist[e].HRME_Id;
                            _context.Add(obj2);
                            if (data.filelist_staff.Length > 0)
                            {
                                for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                                {
                                    if (data.filelist_staff[s1].cfilepath != null)
                                    {

                                        NAAC_AC_Committee_Members_files_DMO objfiles3 = new NAAC_AC_Committee_Members_files_DMO();
                                        objfiles3.NCACCOMMM_Id = obj2.NCACCOMMM_Id;
                                        objfiles3.NCACCOMMMF_FileName = data.filelist_staff[s1].cfilename;
                                        objfiles3.NCACCOMMMF_FileDesc = data.filelist_staff[s1].cfiledesc;
                                        objfiles3.NCACCOMMMF_FilePath = data.filelist_staff[s1].cfilepath;
                                        objfiles3.NCACCOMMMF_StatusFlg = "";
                                        objfiles3.NCACCOMMMF_ActiveFlg = true;

                                        _context.Add(objfiles3);
                                    }
                                }
                            }
                        }
                    }
                    else if (data.all1 == "12")
                    {
                        NAAC_AC__Committee_Members_DMO obj2 = new NAAC_AC__Committee_Members_DMO();
                        obj2.NCACCOMM_Id = obj1.NCACCOMM_Id;
                        obj2.NCACCOMMM_Role = data.NCACCOMMM_Role;
                        obj2.NCACCOMMM_ActiveFlg = true;
                        obj2.NCACCOMMM_CreatedBy = data.UserId;
                        obj2.NCACCOMMM_UpdatedBy = data.UserId;
                        obj2.NCACCOMMM_CreatedDate = DateTime.Now;
                        obj2.NCACCOMMM_UpdatedDate = DateTime.Now;
                        obj2.HRME_Id = 0;
                        obj2.NCACCOMMM_MemberName = data.NCACCOMMM_MemberName;
                        obj2.NCACCOMMM_MemberDetails = data.NCACCOMMM_MemberDetails;
                        obj2.NCACCOMMM_MemberPhoneNo = data.NCACCOMMM_MemberPhoneNo;
                        obj2.NCACCOMMM_MemberEmailId = data.NCACCOMMM_MemberEmailId;
                        obj2.NCACCOMMM_StatusFlg = "";

                        if (data.all1 == "11")
                        {
                            obj1.NCACCOMM_StaffFlg = "EXStaf";
                        }
                        else
                        {
                            obj1.NCACCOMM_StaffFlg = "NEWStaf";
                        }
                        _context.Add(obj2);
                        if (data.filelist_staff.Length > 0)
                        {
                            for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                            {
                                if (data.filelist_staff[s1].cfilepath != null)
                                {


                                    NAAC_AC_Committee_Members_files_DMO objfiles3 = new NAAC_AC_Committee_Members_files_DMO();
                                    objfiles3.NCACCOMMM_Id = obj2.NCACCOMMM_Id;
                                    objfiles3.NCACCOMMMF_FileName = data.filelist_staff[s1].cfilename;
                                    objfiles3.NCACCOMMMF_FileDesc = data.filelist_staff[s1].cfiledesc;
                                    objfiles3.NCACCOMMMF_FilePath = data.filelist_staff[s1].cfilepath;
                                    objfiles3.NCACCOMMMF_StatusFlg = "";
                                    objfiles3.NCACCOMMMF_ActiveFlg = true;
                                    _context.Add(objfiles3);
                                }
                            }
                        }
                    }
                    int f = _context.SaveChanges();
                    if (f > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "notsaved";
                    }
                }
                else if (data.NCACCOMM_Id > 0)
                {
                    var update = _context.NAAC_AC_Committee_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACCOMM_Id == data.NCACCOMM_Id).SingleOrDefault();
                    update.NCACCOMM_UpdatedBy = data.UserId;
                    update.NCACCOMM_UpdatedDate = DateTime.Now;
                    update.NCACCOMM_Year = data.ASMAY_Id;

                    update.NCACCOMM_CommitteeName = data.NCACCOMM_CommitteeName;
                    update.NCACCOMM_Flg = data.NCACCOMM_Flg;
                    _context.Update(update);
                    if (data.filelist.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.filelist)
                        {
                            Fid.Add(item.NCACCOMMF_Id);
                        }
                        var removefile11 = _context.NAAC_AC_Committee_Files_DMO.Where(t => t.NCACCOMM_Id == data.NCACCOMM_Id && !Fid.Contains(t.NCACCOMMF_Id)).Distinct().ToList();

                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _context.NAAC_AC_Committee_Files_DMO.Single(t => t.NCACCOMM_Id == data.NCACCOMM_Id && t.NCACCOMMF_Id == item2.NCACCOMMF_Id);
                                deactfile.NCACCOMMF_ActiveFlg = false;
                                _context.Update(deactfile);

                            }

                        }

                        foreach (NAAC_AC_Committee_DTO DocumentsDTO in data.filelist)
                        {

                            if (DocumentsDTO.NCACCOMMF_Id > 0 && DocumentsDTO.NCACCOMMF_StatusFlg != "approved")
                            {
                                if (DocumentsDTO.cfilepath != null)
                                {

                                    var filesdata = _context.NAAC_AC_Committee_Files_DMO.Where(t => t.NCACCOMMF_Id == DocumentsDTO.NCACCOMMF_Id).SingleOrDefault();
                                    filesdata.NCACCOMMF_FileDesc = DocumentsDTO.cfiledesc;
                                    filesdata.NCACCOMMF_FileName = DocumentsDTO.cfilename;
                                    filesdata.NCACCOMMF_FilePath = DocumentsDTO.cfilepath;


                                    _context.Update(filesdata);
                                    //int flag = _context.SaveChanges();
                                    //if (flag > 0)
                                    //{
                                    //    data.returnval = true;
                                    //}
                                    //else
                                    //{
                                    //    data.returnval = false;
                                    //}
                                }
                            }
                            else
                            {

                                if (DocumentsDTO.NCACCOMMF_Id == 0)
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {
                                        NAAC_AC_Committee_Files_DMO obj2 = new NAAC_AC_Committee_Files_DMO();
                                        obj2.NCACCOMMF_FileName = DocumentsDTO.cfilename;
                                        obj2.NCACCOMMF_FileDesc = DocumentsDTO.cfiledesc;
                                        obj2.NCACCOMMF_FilePath = DocumentsDTO.cfilepath;
                                        obj2.NCACCOMMF_StatusFlg = "";
                                        obj2.NCACCOMMF_ActiveFlg = true;
                                        obj2.NCACCOMM_Id = data.NCACCOMM_Id;
                                        _context.Add(obj2);
                                        //int flag = _context.SaveChanges();
                                        //if (flag > 0)
                                        //{
                                        //    data.returnval = true;
                                        //}
                                        //else
                                        //{
                                        //    data.returnval = false;
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    if (data.all1 == "11")
                    {
                        for (int e = 0; e < data.selectdStafflist.Length; e++)
                        {
                            var stfflist = _context.NAAC_AC__Committee_Members_DMO.Where(t => t.HRME_Id == data.selectdStafflist[e].HRME_Id && t.NCACCOMM_Id == data.NCACCOMM_Id).Single();
                            stfflist.NCACCOMMM_Role = data.NCACCOMMM_Role;
                            stfflist.NCACCOMMM_UpdatedBy = data.UserId;
                            stfflist.NCACCOMMM_UpdatedDate = DateTime.Now;
                            _context.Update(stfflist);
                            if (data.filelist_staff.Length > 0)
                            {

                                if (data.filelist_staff.Length > 0)
                                {
                                    List<long> Fid = new List<long>();
                                    foreach (var item in data.filelist_staff)
                                    {
                                        if(item.NCACCOMMM_Id== stfflist.NCACCOMMM_Id)
                                        {
                                            Fid.Add(item.NCACCOMMMF_Id);
                                        }                                        
                                    }
                                    var removefile11 = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMM_Id == stfflist.NCACCOMMM_Id && !Fid.Contains(t.NCACCOMMMF_Id)).Distinct().ToList();
                                    if (removefile11.Count > 0)
                                    {
                                        foreach (var item2 in removefile11)
                                        {
                                            var deactfile = _context.NAAC_AC_Committee_Members_files_DMO.Single(t => t.NCACCOMMM_Id == stfflist.NCACCOMMM_Id && t.NCACCOMMMF_Id == item2.NCACCOMMMF_Id);
                                            deactfile.NCACCOMMMF_ActiveFlg = false;
                                            _context.Update(deactfile);
                                            _context.SaveChanges();
                                        }

                                    }
                                }
                                // var removefile11 = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMM_Id == data.filelist_staff[0].NCACCOMMM_Id && !Fid.Contains(t.NCACCOMMMF_Id)).Distinct().ToList();
                               








                                foreach (NAAC_AC_Committee_DTO DocumentsDTO in data.filelist_staff)
                                {

                                    if (DocumentsDTO.NCACCOMMMF_Id > 0 && DocumentsDTO.NCACCOMMMF_StatusFlg != "approved")
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {

                                            var filesdata = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMMF_Id == DocumentsDTO.NCACCOMMMF_Id).FirstOrDefault();
                                            filesdata.NCACCOMMMF_FileDesc = DocumentsDTO.cfiledesc;
                                            filesdata.NCACCOMMMF_FileName = DocumentsDTO.cfilename;
                                            filesdata.NCACCOMMMF_FilePath = DocumentsDTO.cfilepath;



                                            _context.Update(filesdata);
                                            //int flag = _context.SaveChanges();
                                            //if (flag > 0)
                                            //{
                                            //    data.returnval = true;
                                            //}
                                            //else
                                            //{
                                            //    data.returnval = false;
                                            //}
                                        }
                                    }
                                    else
                                    {

                                        if (DocumentsDTO.NCACCOMMMF_Id == 0)
                                        {
                                            if (DocumentsDTO.cfilepath != null)
                                            {
                                                NAAC_AC_Committee_Members_files_DMO obj23 = new NAAC_AC_Committee_Members_files_DMO();
                                                obj23.NCACCOMMMF_FileName = DocumentsDTO.cfilename;
                                                obj23.NCACCOMMMF_FileDesc = DocumentsDTO.cfiledesc;
                                                obj23.NCACCOMMMF_FilePath = DocumentsDTO.cfilepath;
                                                obj23.NCACCOMMMF_StatusFlg = "";
                                                obj23.NCACCOMMMF_ActiveFlg = true;
                                                obj23.NCACCOMMM_Id = stfflist.NCACCOMMM_Id;
                                                _context.Add(obj23);
                                                //int flag = _context.SaveChanges();
                                                //if (flag > 0)
                                                //{
                                                //    data.returnval = true;
                                                //}
                                                //else
                                                //{
                                                //    data.returnval = false;
                                                //}
                                            }
                                        }
                                    }
                                }

                                //for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                                //{
                                //    if (data.filelist_staff[s1].cfilepath != null)
                                //    {
                                //        NAAC_AC_Committee_Members_files_DMO objfiles3 = new NAAC_AC_Committee_Members_files_DMO();
                                //        objfiles3.NCACCOMMM_Id = data.NCACCOMMM_Id;
                                //        objfiles3.NCACCOMMMF_FileName = data.filelist_staff[s1].cfilename;
                                //        objfiles3.NCACCOMMMF_FileDesc = data.filelist_staff[s1].cfiledesc;
                                //        objfiles3.NCACCOMMMF_FilePath = data.filelist_staff[s1].cfilepath;
                                //        _context.Add(objfiles3);
                                //    }
                                //}
                            }
                            //NAAC_AC__Committee_Members_DMO obj2 = new NAAC_AC__Committee_Members_DMO();
                            //obj2.NCACCOMM_Id = update.NCACCOMM_Id;
                            //obj2.HRME_Id = data.selectdStafflist[e].HRME_Id;
                            //obj2.NCACCOMMM_Role = data.NCACCOMMM_Role; 
                            //obj2.NCACCOMMM_UpdatedBy = data.UserId;
                            //obj2.NCACCOMMM_UpdatedDate = DateTime.Now;
                            //_context.Update(obj2);
                            //if (data.filelist_staff.Length > 0)
                            //{
                            //    for (int s1 = 0; s1 < data.filelist_staff.Length; s1++)
                            //    {
                            //        if (data.filelist_staff[0].cfilepath != null)
                            //        {


                            //            NAAC_AC_Committee_Members_files_DMO objfiles3 = new NAAC_AC_Committee_Members_files_DMO();
                            //            objfiles3.NCACCOMMM_Id = obj2.NCACCOMMM_Id;
                            //            objfiles3.NCACCOMMMF_FileName = data.filelist_staff[s1].cfilename;
                            //            objfiles3.NCACCOMMMF_FileDesc = data.filelist_staff[s1].cfiledesc;
                            //            objfiles3.NCACCOMMMF_FilePath = data.filelist_staff[s1].cfilepath;
                            //            _context.Add(objfiles3);
                            //        }
                            //    }
                            //}
                        }
                    }
                    if (data.all1 == "12")
                    {
                        for (int e = 0; e < data.selectdStafflist.Length; e++)
                        {
                            var stfflist = _context.NAAC_AC__Committee_Members_DMO.Where(t => t.HRME_Id == data.selectdStafflist[e].HRME_Id && t.NCACCOMMM_Id == data.NCACCOMMM_Id).Single();
                            stfflist.NCACCOMMM_Role = data.NCACCOMMM_Role;
                            stfflist.NCACCOMMM_UpdatedBy = data.UserId;
                            stfflist.NCACCOMMM_UpdatedDate = DateTime.Now;
                            _context.Update(stfflist);
                            if (data.filelist_staff.Length > 0)
                            {

                                if (data.filelist_staff.Length > 0)
                                {
                                    List<long> Fid = new List<long>();
                                    foreach (var item in data.filelist_staff)
                                    {
                                        if (item.NCACCOMMM_Id == stfflist.NCACCOMMM_Id)
                                        {
                                            Fid.Add(item.NCACCOMMMF_Id);
                                        }
                                    }
                                    var removefile11 = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMM_Id == stfflist.NCACCOMMM_Id && !Fid.Contains(t.NCACCOMMMF_Id)).Distinct().ToList();
                                    if (removefile11.Count > 0)
                                    {
                                        foreach (var item2 in removefile11)
                                        {
                                            var deactfile = _context.NAAC_AC_Committee_Members_files_DMO.Single(t => t.NCACCOMMM_Id == stfflist.NCACCOMMM_Id && t.NCACCOMMMF_Id == item2.NCACCOMMMF_Id);
                                            deactfile.NCACCOMMMF_ActiveFlg = false;
                                            _context.Update(deactfile);
                                            _context.SaveChanges();
                                        }

                                    }
                                }



                                foreach (NAAC_AC_Committee_DTO DocumentsDTO in data.filelist_staff)
                                {

                                    if (DocumentsDTO.NCACCOMMMF_Id > 0 && DocumentsDTO.NCACCOMMMF_StatusFlg != "approved")
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {

                                            var filesdata = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMMF_Id == DocumentsDTO.NCACCOMMMF_Id).FirstOrDefault();
                                            filesdata.NCACCOMMMF_FileDesc = DocumentsDTO.cfiledesc;
                                            filesdata.NCACCOMMMF_FileName = DocumentsDTO.cfilename;
                                            filesdata.NCACCOMMMF_FilePath = DocumentsDTO.cfilepath;


                                            _context.Update(filesdata);
                                            //int flag = _context.SaveChanges();
                                            //if (flag > 0)
                                            //{
                                            //    data.returnval = true;
                                            //}
                                            //else
                                            //{
                                            //    data.returnval = false;
                                            //}
                                        }
                                    }
                                    else
                                    {

                                        if (DocumentsDTO.NCACCOMMMF_Id == 0)
                                        {
                                            if (DocumentsDTO.cfilepath != null)
                                            {
                                                NAAC_AC_Committee_Members_files_DMO obj23 = new NAAC_AC_Committee_Members_files_DMO();
                                                obj23.NCACCOMMMF_FileName = DocumentsDTO.cfilename;
                                                obj23.NCACCOMMMF_FileDesc = DocumentsDTO.cfiledesc;
                                                obj23.NCACCOMMMF_FilePath = DocumentsDTO.cfilepath;
                                                obj23.NCACCOMMMF_StatusFlg = "";
                                                obj23.NCACCOMMMF_ActiveFlg = true;
                                                obj23.NCACCOMMM_Id = data.NCACCOMMM_Id;
                                                _context.Add(obj23);
                                                //int flag = _context.SaveChanges();
                                                //if (flag > 0)
                                                //{
                                                //    data.returnval = true;
                                                //}
                                                //else
                                                //{
                                                //    data.returnval = false;
                                                //}
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    int ff = _context.SaveChanges();
                    if (ff > 0)
                    {
                        data.msg = "update";
                    }
                    else
                    {
                        data.msg = "noupdate";
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_Committee_DTO savefilewisecomments(NAAC_AC_Committee_DTO data)
        {
            try
            {
                NAAC_AC_Committee_File_Comments_DMO obj1 = new NAAC_AC_Committee_File_Comments_DMO();
                obj1.NCACCOMMFC_Remarks = data.Remarks;
                obj1.NCACCOMMFC_RemarksBy = data.UserId;
                obj1.NCACCOMMFC_StatusFlg = "";
                obj1.NCACCOMMF_Id = data.filefkid;
                obj1.NCACCOMMFC_ActiveFlag = true;
                obj1.NCACCOMMFC_CreatedBy = data.UserId;
                obj1.NCACCOMMFC_UpdatedBy = data.UserId;
                obj1.NCACCOMMFC_UpdatedDate = DateTime.Now;
                obj1.NCACCOMMFC_CreatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        // member file
        public NAAC_AC_Committee_DTO savefilewisecommentsmember(NAAC_AC_Committee_DTO data)
        {
            try
            {
                NAAC_AC_Committee_Members_File_Comments_DMO obj1 = new NAAC_AC_Committee_Members_File_Comments_DMO();
                obj1.NCACCOMMMFC_Remarks = data.Remarks;
                obj1.NCACCOMMMFC_RemarksBy = data.UserId;
                obj1.NCACCOMMMFC_StatusFlg = "";
                obj1.NCACCOMMMF_Id = data.filefkid;
                obj1.NCACCOMMMFC_ActiveFlag = true;
                obj1.NCACCOMMMFC_CreatedBy = data.UserId;
                obj1.NCACCOMMMFC_UpdatedBy = data.UserId;
                obj1.NCACCOMMMFC_UpdatedDate = DateTime.Now;
                obj1.NCACCOMMMFC_CreatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO getfilecommentmember(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.commentlist1member = (from a in _context.NAAC_AC_Committee_Members_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCACCOMMMFC_RemarksBy == b.Id && a.NCACCOMMMF_Id == data.NCACCOMMMF_Id)
                                     select new NAAC_AC_Committee_DTO
                                     {
                                         NCACCOMMMF_Id = a.NCACCOMMMF_Id,
                                         NCACCOMMMFC_Remarks = a.NCACCOMMMFC_Remarks,
                                         NCACCOMMMFC_Id = a.NCACCOMMMFC_Id,
                                         NCACCOMMMFC_RemarksBy = a.NCACCOMMMFC_RemarksBy,
                                         NCACCOMMMFC_StatusFlg = a.NCACCOMMMFC_StatusFlg,
                                         NCACCOMMMFC_ActiveFlag = a.NCACCOMMMFC_ActiveFlag,
                                         NCACCOMMMFC_CreatedBy = a.NCACCOMMMFC_CreatedBy,
                                         NCACCOMMMFC_CreatedDate = a.NCACCOMMMFC_CreatedDate,
                                         NCACCOMMMFC_UpdatedBy = a.NCACCOMMMFC_UpdatedBy,
                                         NCACCOMMMFC_UpdatedDate = a.NCACCOMMMFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACCOMMMFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_Committee_DTO getfilecomment(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_AC_Committee_File_Comments_DMO
                                     from b in _context.ApplUser
                                     where (a.NCACCOMMFC_RemarksBy == b.Id && a.NCACCOMMF_Id == data.NCACCOMMF_Id)
                                     select new NAAC_AC_Committee_DTO
                                     {
                                         NCACCOMMF_Id = a.NCACCOMMF_Id,
                                         NCACCOMMFC_Remarks = a.NCACCOMMFC_Remarks,
                                         NCACCOMMFC_Id = a.NCACCOMMFC_Id,
                                         NCACCOMMFC_RemarksBy = a.NCACCOMMFC_RemarksBy,
                                         NCACCOMMFC_StatusFlg = a.NCACCOMMFC_StatusFlg,
                                         NCACCOMMFC_ActiveFlag = a.NCACCOMMFC_ActiveFlag,
                                         NCACCOMMFC_CreatedBy = a.NCACCOMMFC_CreatedBy,
                                         NCACCOMMFC_CreatedDate = a.NCACCOMMFC_CreatedDate,
                                         NCACCOMMFC_UpdatedBy = a.NCACCOMMFC_UpdatedBy,
                                         NCACCOMMFC_UpdatedDate = a.NCACCOMMFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACCOMMFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO getcomment(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_AC_Committee_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCACCOMMC_RemarksBy == b.Id && a.NCACCOMM_Id == data.NCACCOMM_Id)
                                    select new NAAC_AC_Committee_DTO
                                    {
                                        NCACCOMMC_Remarks = a.NCACCOMMC_Remarks,
                                        NCACCOMMC_Id = a.NCACCOMMC_Id,
                                        NCACCOMMC_RemarksBy = a.NCACCOMMC_RemarksBy,
                                        NCACCOMMC_StatusFlg = a.NCACCOMMC_StatusFlg,
                                        NCACCOMMC_ActiveFlag = a.NCACCOMMC_ActiveFlag,
                                        NCACCOMMC_CreatedBy = a.NCACCOMMC_CreatedBy,
                                        NCACCOMMC_CreatedDate = a.NCACCOMMC_CreatedDate,
                                        NCACCOMMC_UpdatedBy = a.NCACCOMMC_UpdatedBy,
                                        NCACCOMMC_UpdatedDate = a.NCACCOMMC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACCOMMC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO savemedicaldatawisecomments(NAAC_AC_Committee_DTO data)
        {
            try
            {
                NAAC_AC_Committee_Comments_DMO obj1 = new NAAC_AC_Committee_Comments_DMO();
                obj1.NCACCOMMC_Remarks = data.Remarks;
                obj1.NCACCOMMC_RemarksBy = data.UserId;
                obj1.NCACCOMMC_StatusFlg = "";
                obj1.NCACCOMM_Id = data.filefkid;
                obj1.NCACCOMMC_ActiveFlag = true;
                obj1.NCACCOMMC_CreatedBy = data.UserId;
                obj1.NCACCOMMC_UpdatedBy = data.UserId;
                obj1.NCACCOMMC_CreatedDate = DateTime.Now;
                obj1.NCACCOMMC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO delete_satff_activity(Int64 id)
        {
            NAAC_AC_Committee_DTO data = new NAAC_AC_Committee_DTO();
            try
            {
                var count_staff = _context.NAAC_AC__Committee_Members_DMO.Where(t => t.NCACCOMM_Id == id).ToList();

                List<long> count_staff_Activity_ids = new List<long>();
                if (count_staff.Count > 0)
                {
                    foreach (var item in count_staff)
                    {
                        count_staff_Activity_ids.Add(item.NCACCOMMM_Id);
                    }
                }
                var count_staff_activity_files = (from a in _context.NAAC_AC_Committee_Members_files_DMO
                                                  where (count_staff_Activity_ids.Contains(a.NCACCOMMM_Id))
                                                  select a).ToList();
                if (count_staff_activity_files.Count > 0)
                {
                    foreach (var item in count_staff_activity_files)
                    {
                        _context.Remove(item);
                    }
                }
                foreach (var item2 in count_staff)
                {
                    _context.Remove(item2);
                }
                int row = _context.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public NAAC_AC_Committee_DTO get_Designation(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.filldesignation = (from a in _context.HR_Master_Employee_DMO
                                        from b in _context.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO get_Employee(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.stafftlist = (from a in _context.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                   && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id && a.HRME_LeftFlag != null)
                                   select new NAAC_AC_Committee_DTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       MI_Id = a.MI_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                   }).Distinct().OrderBy(t => t.empname).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO deactiveStudent(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var w = _context.NAAC_AC_Committee_DMO.Where(t => t.NCACCOMM_Id == data.NCACCOMM_Id).SingleOrDefault();
                if (w.NCACCOMM_ActiveFlg == true)
                {
                    w.NCACCOMM_ActiveFlg = false;
                }
                else
                {
                    w.NCACCOMM_ActiveFlg = true;
                }
                w.NCACCOMM_UpdatedBy = data.UserId;
                w.NCACCOMM_UpdatedDate = DateTime.Now;
                w.MI_Id = data.MI_Id;
                _context.Update(w);
                int k = _context.SaveChanges();
                if (k > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_AC_Committee_DTO getcommentmember(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.commentlistmember = (from a in _context.NAAC_AC_Committee_Members_Comments_DMO
                                    from b in _context.ApplUser
                                    where (a.NCACCOMMMC_RemarksBy == b.Id && a.NCACCOMMM_Id == data.NCACCOMMM_Id)
                                    select new NAAC_AC_Committee_DTO
                                    {
                                        NCACCOMMMC_Remarks = a.NCACCOMMMC_Remarks,
                                        NCACCOMMMC_Id = a.NCACCOMMMC_Id,
                                        NCACCOMMMC_RemarksBy = a.NCACCOMMMC_RemarksBy,
                                        NCACCOMMMC_StatusFlg = a.NCACCOMMMC_StatusFlg,
                                        NCACCOMMMC_ActiveFlag = a.NCACCOMMMC_ActiveFlag,
                                        NCACCOMMMC_CreatedBy = a.NCACCOMMMC_CreatedBy,
                                        NCACCOMMMC_CreatedDate = a.NCACCOMMMC_CreatedDate,
                                        NCACCOMMMC_UpdatedBy = a.NCACCOMMMC_UpdatedBy,
                                        NCACCOMMMC_UpdatedDate = a.NCACCOMMMC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACCOMMMC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO savemedicaldatawisecommentsmember(NAAC_AC_Committee_DTO data)
        {
            try
            {
                NAAC_AC_Committee_Members_Comments_DMO obj1 = new NAAC_AC_Committee_Members_Comments_DMO();
                obj1.NCACCOMMMC_Remarks = data.Remarks;
                obj1.NCACCOMMMC_RemarksBy = data.UserId;
                obj1.NCACCOMMMC_StatusFlg = "";
                obj1.NCACCOMMM_Id = data.filefkid;
                obj1.NCACCOMMMC_ActiveFlag = true;
                obj1.NCACCOMMMC_CreatedBy = data.UserId;
                obj1.NCACCOMMMC_UpdatedBy = data.UserId;
                obj1.NCACCOMMMC_CreatedDate = DateTime.Now;
                obj1.NCACCOMMMC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO EditData(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var editlist = (from a in _context.Academic
                                from b in _context.NAAC_AC_Committee_DMO
                                from c in _context.NAAC_AC__Committee_Members_DMO
                                where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.NCACCOMM_Year == a.ASMAY_Id && b.NCACCOMM_Id == data.NCACCOMM_Id && c.NCACCOMM_Id == b.NCACCOMM_Id)
                                select new NAAC_AC_Committee_DTO
                                {
                                    NCACCOMM_Id = b.NCACCOMM_Id,
                                    ASMAY_Year = a.ASMAY_Year,
                                    NCACCOMM_CommitteeName = b.NCACCOMM_CommitteeName,
                                    NCACCOMMM_MemberName = c.NCACCOMMM_MemberName,
                                    NCACCOMMM_MemberDetails = c.NCACCOMMM_MemberDetails,
                                    NCACCOMMM_MemberEmailId = c.NCACCOMMM_MemberEmailId,
                                    NCACCOMMM_MemberPhoneNo = c.NCACCOMMM_MemberPhoneNo,
                                    NCACCOMMM_Role = c.NCACCOMMM_Role,
                                    NCACCOMM_Flg = b.NCACCOMM_Flg,
                                    MI_Id = data.MI_Id,
                                    NCACCOMM_StatusFlg = b.NCACCOMM_StatusFlg,
                                    NCACCOMMM_StatusFlg = c.NCACCOMMM_StatusFlg,
                                    HRME_Id = c.HRME_Id,
                                    NCACCOMM_Year = b.NCACCOMM_Year,
                                }).Distinct().ToList();
                data.editlist = editlist.ToArray();

                if (editlist[0].HRME_Id != 0)
                {
                    var ee = (from a in _context.HR_Master_Employee_DMO
                              where (a.HRME_Id == editlist[0].HRME_Id)
                              select new NAAC_AC_Committee_DTO
                              {
                                  HRMD_Id = a.HRMD_Id,
                                  HRMDES_Id = a.HRMDES_Id,

                              }).Distinct().ToList();
                    data.filldesignation = (from a in _context.HR_Master_Employee_DMO
                                            from b in _context.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == ee[0].HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

                    data.stafftlist = (from a in _context.HR_Master_Employee_DMO.Where(a => a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true
                                       && a.HRMDES_Id == ee[0].HRMDES_Id && a.HRMD_Id == ee[0].HRMD_Id)
                                       select new NAAC_AC_Committee_DTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           MI_Id = a.MI_Id,
                                           empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                       }).Distinct().OrderBy(t => t.empname).ToArray();
                    data.HRMD_Id = ee[0].HRMD_Id;
                    data.HRMDES_Id = ee[0].HRMDES_Id;
                }
                data.editMainSActFileslist = (from t in _context.NAAC_AC_Committee_Files_DMO
                                              from b in _context.NAAC_AC_Committee_DMO
                                              where (t.NCACCOMM_Id == data.NCACCOMM_Id && t.NCACCOMM_Id == b.NCACCOMM_Id && b.MI_Id == data.MI_Id&&t.NCACCOMMF_ActiveFlg==true)
                                              select new NAAC_AC_Committee_DTO
                                              {
                                                  cfilename = t.NCACCOMMF_FileName,
                                                  cfilepath = t.NCACCOMMF_FilePath,
                                                  cfiledesc = t.NCACCOMMF_FileDesc,
                                                  NCACCOMMF_StatusFlg = t.NCACCOMMF_StatusFlg,
                                                  NCACCOMMF_Id = t.NCACCOMMF_Id,
                                                  NCACCOMM_Id = t.NCACCOMM_Id,
                                              }).Distinct().ToArray();
                List<long> stafft_tableid = new List<long>();
                var filter_stafftableid = (from a in _context.NAAC_AC_Committee_DMO
                                           from b in _context.NAAC_AC__Committee_Members_DMO
                                           where (a.NCACCOMM_Id == data.NCACCOMM_Id && a.MI_Id == data.MI_Id && a.NCACCOMM_Id == b.NCACCOMM_Id)
                                           select b).ToList();
                if (filter_stafftableid.Count > 0)
                {
                    foreach (var item in filter_stafftableid)
                    {
                        stafft_tableid.Add(item.NCACCOMMM_Id);
                    }
                }
                data.editStaffActFileslist = (from t in _context.NAAC_AC_Committee_Members_files_DMO
                                              from b in _context.NAAC_AC__Committee_Members_DMO
                                              where (stafft_tableid.Contains(t.NCACCOMMM_Id) && t.NCACCOMMM_Id == b.NCACCOMMM_Id&&t.NCACCOMMMF_ActiveFlg==true)
                                              select new NAAC_AC_Committee_DTO
                                              {
                                                  cfilename = t.NCACCOMMMF_FileName,
                                                  cfilepath = t.NCACCOMMMF_FilePath,
                                                  cfiledesc = t.NCACCOMMMF_FileDesc,
                                                  NCACCOMMMF_StatusFlg = t.NCACCOMMMF_StatusFlg,
                                                  NCACCOMMMF_Id = t.NCACCOMMMF_Id,
                                                  NCACCOMMM_Id = t.NCACCOMMM_Id,
                                              }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<NAAC_AC_Committee_DTO> get_MappedStaff(NAAC_AC_Committee_DTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_GET_MODAL_STAFF_Members";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACCOMM_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.NCACCOMM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@NCACCOMM_StaffFlg",
                  SqlDbType.VarChar)
                    {
                        Value = data.NCACCOMM_StaffFlg
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.mappedstafflist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO deactive_staff(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var result = _context.NAAC_AC__Committee_Members_DMO.Where(t => t.NCACCOMMM_Id == data.NCACCOMMM_Id).Single();

                if (result.NCACCOMMM_ActiveFlg == true)
                {
                    result.NCACCOMMM_ActiveFlg = false;
                }
                else if (result.NCACCOMMM_ActiveFlg == false)
                {
                    result.NCACCOMMM_ActiveFlg = true;
                }

                result.NCACCOMMM_UpdatedDate = DateTime.Now;
                result.NCACCOMMM_UpdatedBy = data.UserId;
                
                _context.Update(result);

                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO viewdocument_MainActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.viewdocument_MainActUploadFiles = (from t in _context.NAAC_AC_Committee_Files_DMO

                                                        where (t.NCACCOMM_Id == data.NCACCOMM_Id&&t.NCACCOMMF_ActiveFlg==true)
                                                        select new NAAC_AC_Committee_DTO
                                                        {
                                                            cfilename = t.NCACCOMMF_FileName,
                                                            cfilepath = t.NCACCOMMF_FilePath,
                                                            cfiledesc = t.NCACCOMMF_FileDesc,
                                                            NCACCOMMF_Id = t.NCACCOMMF_Id,
                                                            NCACCOMM_Id = t.NCACCOMM_Id,
                                                            NCACCOMMF_StatusFlg = t.NCACCOMMF_StatusFlg,

                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO delete_MainActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var result = _context.NAAC_AC_Committee_Files_DMO.Where(t => t.NCACCOMMF_Id == data.NCACCOMMF_Id).SingleOrDefault();
                result.NCACCOMMF_ActiveFlg = false;
                _context.Update(result);
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _context.Remove(resultid);
                //    }
                //}
                int row = _context.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_MainActUploadFiles = (from t in _context.NAAC_AC_Committee_Files_DMO

                                                        where (t.NCACCOMM_Id == data.NCACCOMM_Id&&t.NCACCOMMF_ActiveFlg==true)
                                                        select new NAAC_AC_Committee_DTO
                                                        {
                                                            cfilename = t.NCACCOMMF_FileName,
                                                            cfilepath = t.NCACCOMMF_FilePath,
                                                            cfiledesc = t.NCACCOMMF_FileDesc,
                                                            NCACCOMMF_Id = t.NCACCOMMF_Id,
                                                            NCACCOMM_Id = t.NCACCOMM_Id,
                                                            NCACCOMMF_StatusFlg = t.NCACCOMMF_StatusFlg,

                                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO viewdocument_StaffActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            try
            {
                data.viewdocument_StaffActUploadFiles = (from t in _context.NAAC_AC_Committee_Members_files_DMO

                                                         where (t.NCACCOMMM_Id == data.NCACCOMMM_Id && t.NCACCOMMMF_ActiveFlg==true)
                                                         select new NAAC_AC_Committee_DTO
                                                         {
                                                             cfilename = t.NCACCOMMMF_FileName,
                                                             cfilepath = t.NCACCOMMMF_FilePath,
                                                             cfiledesc = t.NCACCOMMMF_FileDesc,
                                                             NCACCOMMMF_Id = t.NCACCOMMMF_Id,
                                                             NCACCOMMM_Id = t.NCACCOMMM_Id,
                                                             NCACCOMMMF_StatusFlg = t.NCACCOMMMF_StatusFlg,

                                                         }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_Committee_DTO delete_StaffActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            try
            {
                var result = _context.NAAC_AC_Committee_Members_files_DMO.Where(t => t.NCACCOMMMF_Id == data.NCACCOMMMF_Id).SingleOrDefault();

                result.NCACCOMMMF_ActiveFlg = false;
                _context.Update(result);
                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _context.Remove(resultid);
                //    }
                //}
                int row = _context.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewdocument_StaffActUploadFiles = (from t in _context.NAAC_AC_Committee_Members_files_DMO

                                                         where (t.NCACCOMMM_Id == data.NCACCOMMM_Id&&t.NCACCOMMMF_ActiveFlg==true)
                                                         select new NAAC_AC_Committee_DTO
                                                         {
                                                             cfilename = t.NCACCOMMMF_FileName,
                                                             cfilepath = t.NCACCOMMMF_FilePath,
                                                             cfiledesc = t.NCACCOMMMF_FileDesc,
                                                             NCACCOMMMF_Id = t.NCACCOMMMF_Id,
                                                             NCACCOMMM_Id = t.NCACCOMMM_Id,
                                                             NCACCOMMMF_StatusFlg = t.NCACCOMMMF_StatusFlg,

                                                         }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
