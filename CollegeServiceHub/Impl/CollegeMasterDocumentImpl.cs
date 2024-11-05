using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeMasterDocumentImpl : Interface.CollegeMasterDocumentInterface
    {
        public ClgAdmissionContext _clgadmctxt;

        public CollegeMasterDocumentImpl(ClgAdmissionContext clgadmctxt)
        {
            _clgadmctxt = clgadmctxt;
        }
        public CollegeDocumentMasterDTO Getdetails(CollegeDocumentMasterDTO data)
        {
            try
            {
                data.getdetails = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.documentlist = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id && a.ACQ_ActiveFlg == true).ToArray();

                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).ToArray();

                data.getmappeddetails = (from a in _clgadmctxt.MasterDocumentDMO
                                         from b in _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO
                                         from c in _clgadmctxt.MasterCourseDMO
                                         from d in _clgadmctxt.ClgMasterBranchDMO
                                         from e in _clgadmctxt.Clg_Adm_College_QuotaDMO
                                         where (a.AMSMD_Id == b.AMSMD_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && e.ACQ_Id == b.ACQ_Id
                                         && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                         select new CollegeDocumentMasterDTO
                                         {
                                             AMCO_CourseName = c.AMCO_CourseName,
                                             AMB_BranchName = d.AMB_BranchName,
                                             ACQ_QuotaName = e.ACQ_QuotaName,
                                             AMCO_Id = b.AMCO_Id,
                                             AMB_Id = b.AMB_Id,
                                             ACQ_Id = b.ACQ_Id
                                         }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO savedata(CollegeDocumentMasterDTO data)
        {
            try
            {

                //string image = "";
                //if (data.images_paths != null)
                //{
                //    foreach (var ig in data.images_paths)
                //    {
                //        image = ig;
                //    }
                //}
                string image = "", name = "";
                if (data.FilePath_Array.Length > 0)
                {
                    foreach (var fl in data.FilePath_Array)
                    {
                    //    for (int i= 0;i<= data.FilePath_Array.Length;i++)
                    //{
                        image = fl.AMSMD_FilePath;
                        name = fl.FileName;
                    }
                }
                    if (data.AMSMD_Id > 0)
                {
                    var checkdocname = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSMD_DocumentName == data.AMSMD_DocumentName && a.AMSMD_Id != a.AMSMD_Id).ToList();
                    if (checkdocname.Count() > 0)
                    {
                        data.message = "Already Exists";
                    }
                    else
                    {
                        var result = _clgadmctxt.MasterDocumentDMO.Single(a => a.MI_Id == data.MI_Id && a.AMSMD_Id == data.AMSMD_Id);
                        result.AMSMD_DocumentName = data.AMSMD_DocumentName;
                        result.AMSMD_Description = data.AMSMD_Description;
                        result.AMSMD_FLAG = data.AMSMD_FLAG;
                        result.AMSMD_FilePath = image;
                    //  result.AMSMD_FilePath = true;
                          result.AMSMD_FileName = name;
                        result.UpdatedDate = DateTime.Now;
                        _clgadmctxt.Update(result);
                        int i = _clgadmctxt.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Fail To Update";
                        }
                    }
                }
                else
                {
                    var checkdocname = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSMD_DocumentName == data.AMSMD_DocumentName).ToList();
                    if (checkdocname.Count() > 0)
                    {
                        data.message = "Already Exists";
                    }
                    else
                    {
                        CollegeDocumentMasterDMO doc = new CollegeDocumentMasterDMO();
                        doc.AMSMD_DocumentName = data.AMSMD_DocumentName;
                        doc.AMSMD_Description = data.AMSMD_Description;
                        doc.AMSMD_FLAG = data.AMSMD_FLAG;
                        doc.MI_Id = data.MI_Id;
                        doc.AMSMD_FLAG = data.AMSMD_FLAG;
                        doc.AMSMD_FilePath = image;
                        doc.AMSMD_FileName = name;
                        //   doc.AMSMD_ToUploadFlg = true;
                        doc.CreatedDate = DateTime.Now;
                        doc.UpdatedDate = DateTime.Now;
                        _clgadmctxt.Add(doc);
                        int i = _clgadmctxt.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Something";
            }
            return data;
        }
        public CollegeDocumentMasterDTO Editdata(CollegeDocumentMasterDTO data)
        {
            try
            {
                data.selectedRowDetails = _clgadmctxt.MasterDocumentDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSMD_Id == data.AMSMD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO DeleteData(CollegeDocumentMasterDTO data)
        {
            try
            {
                var checkdocused = _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSMD_Id == data.AMSMD_Id && a.ACQCD_ActiveFlg == true).ToArray();

                var checkdocused1 = _clgadmctxt.AdmCollegeStudentDocumentsDMO.Where(a => a.ACSMD_Id == data.AMSMD_Id).ToArray();

                if (checkdocused.Count() == 0 && checkdocused1.Count() == 0)
                {
                    var checkdocused12 = _clgadmctxt.MasterDocumentDMO.Where(a => a.AMSMD_Id == data.AMSMD_Id).ToList();
                    _clgadmctxt.Remove(checkdocused12.ElementAt(0));
                    int i = _clgadmctxt.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Deleted";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Deleted";
                    }
                }
                else
                {
                    data.message = "Delete";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO onchangecourse(CollegeDocumentMasterDTO data)
        {
            try
            {
                data.branchlist = (from a in _clgadmctxt.Adm_Course_Branch_MappingDMO
                                   from c in _clgadmctxt.MasterCourseDMO
                                   from d in _clgadmctxt.ClgMasterBranchDMO
                                   where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMCOBM_ActiveFlg == true && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true
                                   && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                   select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();



                data.getsavedbranchlist = (from a in _clgadmctxt.MasterDocumentDMO
                                           from b in _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO
                                           from c in _clgadmctxt.MasterCourseDMO
                                           from d in _clgadmctxt.ClgMasterBranchDMO
                                           from e in _clgadmctxt.Clg_Adm_College_QuotaDMO
                                           where (a.AMSMD_Id == b.AMSMD_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && e.ACQ_Id == b.ACQ_Id
                                           && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id && a.AMSMD_Id == data.AMSMD_Id
                                           && b.ACQ_Id == data.ACQ_Id)
                                           select new CollegeDocumentMasterDTO
                                           {
                                               AMCO_CourseName = c.AMCO_CourseName,
                                               AMB_BranchName = d.AMB_BranchName,
                                               ACQ_QuotaName = e.ACQ_QuotaName,
                                               AMCO_Id = b.AMCO_Id,
                                               AMB_Id = b.AMB_Id,
                                               ACQ_Id = b.ACQ_Id
                                           }).Distinct().ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO savedata1(CollegeDocumentMasterDTO data)
        {
            try
            {

                if (data.temp_branchDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (temp_branchDTO ph in data.temp_branchDTO)
                    {
                        temparr.Add(ph.AMB_Id);
                    }

                    //removing mobile number 
                    Array Phone_Noresultremove = _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO.Where(t => !temparr.Contains(t.AMB_Id) && t.AMCO_Id == data.AMCO_Id
                    && t.ACQ_Id == data.ACQ_Id && t.AMSMD_Id == data.AMSMD_Id).ToArray();
                    foreach (temp_branchDTO ph1 in Phone_Noresultremove)
                    {
                        _clgadmctxt.Remove(ph1);
                    }

                    for (int i = 0; i < data.temp_branchDTO.Count(); i++)
                    {
                        var checkidu = _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO.Where(t => t.AMCO_Id == data.AMCO_Id
                     && t.ACQ_Id == data.ACQ_Id && t.AMSMD_Id == data.AMSMD_Id && t.AMB_Id == data.temp_branchDTO[i].AMB_Id).ToArray();

                        if (checkidu.Count() > 0)
                        {
                            var checkidue = _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO.Single(t => t.AMCO_Id == data.AMCO_Id && t.ACQ_Id == data.ACQ_Id && t.AMSMD_Id == data.AMSMD_Id && t.AMB_Id == data.temp_branchDTO[i].AMB_Id);
                            checkidue.UpdatedDate = DateTime.Now;
                            _clgadmctxt.Update(checkidue);
                        }
                        else
                        {
                            CollegeQuotaCourseBranchDocumentMappingDMO dmo = new CollegeQuotaCourseBranchDocumentMappingDMO();
                            dmo.ACQ_Id = data.ACQ_Id;
                            dmo.AMCO_Id = data.AMCO_Id;
                            dmo.MI_Id = data.MI_Id;
                            dmo.AMSMD_Id = data.AMSMD_Id;
                            dmo.AMB_Id = data.temp_branchDTO[i].AMB_Id;
                            dmo.ACQCD_CompulsoryFlg = data.ACQCD_CompulsoryFlg;
                            dmo.ACQCD_ActiveFlg = true;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            _clgadmctxt.Add(dmo);
                        }
                    }
                    var i1 = _clgadmctxt.SaveChanges();
                    if (i1 > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO getdoc(CollegeDocumentMasterDTO data)
        {
            try
            {
                data.doclist = (from a in _clgadmctxt.MasterDocumentDMO
                                from b in _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO
                                from c in _clgadmctxt.MasterCourseDMO
                                from d in _clgadmctxt.ClgMasterBranchDMO
                                from e in _clgadmctxt.Clg_Adm_College_QuotaDMO
                                where (a.AMSMD_Id == b.AMSMD_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && e.ACQ_Id == b.ACQ_Id
                                && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.ACQ_Id == data.ACQ_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id)
                                select new CollegeDocumentMasterDTO
                                {
                                    AMCO_CourseName = c.AMCO_CourseName,
                                    AMB_BranchName = d.AMB_BranchName,
                                    ACQ_QuotaName = e.ACQ_QuotaName,
                                    AMCO_Id = b.AMCO_Id,
                                    AMB_Id = b.AMB_Id,
                                    ACQ_Id = b.ACQ_Id,
                                    AMSMD_DocumentName = a.AMSMD_DocumentName,
                                    ACQCD_CompulsoryFlg = b.ACQCD_CompulsoryFlg,
                                    ACQCD_ActiveFlg = b.ACQCD_ActiveFlg,
                                    ACQCD_Id = b.ACQCD_Id
                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeDocumentMasterDTO deactive_sub(CollegeDocumentMasterDTO data)
        {
            try
            {
                var checkid = _clgadmctxt.CollegeQuotaCourseBranchDocumentMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.ACQCD_Id == data.ACQCD_Id);
                if (checkid.ACQCD_ActiveFlg == true)
                {
                    checkid.ACQCD_ActiveFlg = false;
                }
                else
                {
                    checkid.ACQCD_ActiveFlg = true;
                }
                checkid.UpdatedDate = DateTime.Now;
                _clgadmctxt.Update(checkid);
                int i = _clgadmctxt.SaveChanges();
                if (i > 0)
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

    }
}
