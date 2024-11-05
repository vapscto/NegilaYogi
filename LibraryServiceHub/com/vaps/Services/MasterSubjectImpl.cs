using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterSubjectImpl : Interfaces.MasterSubjectInterface
    {
        public LibraryContext _LibraryContext;

        public MasterSubjectImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }


        public MasterSubject_DTO getdetails(int id)
        {

            MasterSubject_DTO data = new MasterSubject_DTO();
            try
            {
                data.parentsublist = _LibraryContext.IVRM_Master_Subjects_DMO.Where(t => t.MI_Id == id && t.ISMS_ActiveFlag == 1).Distinct().ToArray();

                data.alldata = (from a in _LibraryContext.IVRM_Master_Subjects_DMO
                                from b in _LibraryContext.MasterSubject_DMO
                                where (a.MI_Id == b.MI_Id && a.ISMS_Id == b.LMS_ParentId && b.MI_Id == id && a.ISMS_ActiveFlag==1)
                                select new MasterSubject_DTO
                                {
                                    MI_Id = b.MI_Id,
                                    LMS_Id = b.LMS_Id,
                                    LMS_SubjectName = b.LMS_SubjectName,
                                    LMS_SubjectNo = b.LMS_SubjectNo,
                                    LMS_ParentId = b.LMS_ParentId,
                                    ISMS_Id = a.ISMS_Id,
                                    LMS_Level = b.LMS_Level,
                                    ISMS_SubjectName = a.ISMS_SubjectName,
                                    LMS_ClassNo=b.LMS_ClassNo,
                                    LMS_ActiveFlg = b.LMS_ActiveFlg

                                }).Distinct().ToArray();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterSubject_DTO Savedata(MasterSubject_DTO data)
        {
            try
            {
                if (data.LMS_Id > 0)
                {
                    var Dulicate = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_Id != data.LMS_Id && t.LMS_ParentId == data.LMS_ParentId && t.LMS_SubjectName==data.LMS_SubjectName).ToList();
                    if (Dulicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterSubject_DMO.Single(t => t.LMS_Id == data.LMS_Id && t.MI_Id == data.MI_Id);

                        update.LMS_SubjectNo = data.LMS_SubjectNo;
                        update.LMS_SubjectName = data.LMS_SubjectName;
                        update.LMS_ParentId = data.LMS_ParentId;                       
                        update.LMS_Level = data.LMS_Level;                       
                        update.LMS_ClassNo = data.LMS_ClassNo;
                        update.UpdatedDate = DateTime.Now;

                        _LibraryContext.Update(update);
                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var Dulicate = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_ParentId == data.LMS_ParentId && t.LMS_SubjectName==data.LMS_SubjectName).ToList();
                    if (Dulicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterSubject_DMO obj = new MasterSubject_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.LMS_SubjectNo = data.LMS_SubjectNo;
                        obj.LMS_ParentId = data.LMS_ParentId;
                        obj.LMS_SubjectName = data.LMS_SubjectName;                       
                        obj.LMS_Level = data.LMS_Level; 
                        obj.LMS_ClassNo = data.LMS_ClassNo;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.LMS_ActiveFlg = true;

                        _LibraryContext.Add(obj);

                        int rowAffected = _LibraryContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterSubject_DTO deactiveY(MasterSubject_DTO data)
        {
            try
            {
                var result = _LibraryContext.MasterSubject_DMO.Single(t => t.MI_Id == data.MI_Id && t.LMS_Id == data.LMS_Id);

                if (result.LMS_ActiveFlg == true)
                {
                    result.LMS_ActiveFlg = false;
                }
                else if (result.LMS_ActiveFlg == false)
                {
                    result.LMS_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
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
