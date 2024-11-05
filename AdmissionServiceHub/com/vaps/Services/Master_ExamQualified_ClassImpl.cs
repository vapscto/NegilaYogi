using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class Master_ExamQualified_ClassImpl : Interfaces.Master_ExamQualified_ClassInterface
    {

        private static ConcurrentDictionary<string, Master_ExamQualified_ClassDTO> _login =
      new ConcurrentDictionary<string, Master_ExamQualified_ClassDTO>();
        public DomainModelMsSqlServerContext _db;
        ILogger<Master_ExamQualified_ClassImpl> _mstimpl;

        public Master_ExamQualified_ClassImpl(DomainModelMsSqlServerContext db, ILogger<Master_ExamQualified_ClassImpl> mstimpl)
        {
            _db = db;
            _mstimpl = mstimpl;
        }

        public Master_ExamQualified_ClassDTO getalldata(Master_ExamQualified_ClassDTO data)
        {
            try
            {

                data.ExamQualifiedClass = (from a in _db.Master_ExamQualified_ClassDMO.Where(t => t.MI_ID == data.MI_Id)
                                          select new Master_ExamQualified_ClassDTO
                                          {
                                             IMQC_ExamName=a.IMQC_ExamName,
                                             IMQC_Id=a.IMQC_Id,
                                              IMQC_ActiveFlag=a.IMQC_ActiveFlag,
                                             

                                          }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Master_ExamQualified_ClassDTO SaveClass(Master_ExamQualified_ClassDTO data)
        {
            try
            {

                var duplicate = _db.Master_ExamQualified_ClassDMO.Where(t => t.MI_ID == data.MI_Id && t.IMQC_ExamName == data.IMQC_ExamName).ToList();

                if (duplicate.Count > 0)
                {
                    data.message = "Duplicate";
                }
                else
                {
                    if (data.IMQC_Id > 0)
                    {
                        Master_ExamQualified_ClassDMO Qualified = new Master_ExamQualified_ClassDMO();
                        Qualified.IMQC_Id = data.IMQC_Id;
                        Qualified.IMQC_ExamName = data.IMQC_ExamName;
                        Qualified.MI_ID = data.MI_Id;
                        Qualified.IMQC_ActiveFlag = true;
                        Qualified.IMQC_CreatedBy = data.User_id;
                        Qualified.IMQC_CreatedDate = DateTime.Now;
                        Qualified.IMQC_UpdatedBy = data.User_id;
                        Qualified.IMQC_UpdatedDate = DateTime.Now;
                        _db.Update(Qualified);
                    }
                    else
                    {
                        Master_ExamQualified_ClassDMO Qualified = new Master_ExamQualified_ClassDMO();
                        Qualified.IMQC_Id = data.IMQC_Id;
                        Qualified.IMQC_ExamName = data.IMQC_ExamName;
                        Qualified.MI_ID = data.MI_Id;
                        Qualified.IMQC_ActiveFlag = true;
                        Qualified.IMQC_CreatedBy = data.User_id;
                        Qualified.IMQC_CreatedDate = DateTime.Now;
                        Qualified.IMQC_UpdatedBy = data.User_id;
                        Qualified.IMQC_UpdatedDate = DateTime.Now;
                        _db.Add(Qualified);
                    }

                        int returnval = _db.SaveChanges();

                        if (returnval > 0)
                        {
                            data.message = "saved";
                        }
                    else
                    {
                        data.message = "Notsaved";
                    }
                   
                   

                }

            }
            catch (Exception ee)
            {
                data.message = "admin";
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public Master_ExamQualified_ClassDTO Editdetails(int id)
        {
            Master_ExamQualified_ClassDTO mast = new Master_ExamQualified_ClassDTO();
            try
            {
                List<Master_ExamQualified_ClassDMO> mastersec = new List<Master_ExamQualified_ClassDMO>();
                //  mastersec = _db.Master_ExamQualified_ClassDMO.AsNoTracking().Where(t => t.IMQC_Id==(id)).ToList();
                mastersec = _db.Master_ExamQualified_ClassDMO.Where(t => t.IMQC_Id == id).ToList();
                mast.EditExamQualifiedClass = mastersec.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                mast.message = ee.Message;
            }
            return mast;
        }


        public Master_ExamQualified_ClassDTO deactiveCat(Master_ExamQualified_ClassDTO user)
        {

            try
            {
                var result = _db.Master_ExamQualified_ClassDMO.Single(t => t.MI_ID == user.MI_Id && t.IMQC_Id == user.IMQC_Id);

                if (result.IMQC_ActiveFlag == true)
                {
                    result.IMQC_ActiveFlag = false;
                }
                else if (result.IMQC_ActiveFlag == false)
                {
                    result.IMQC_ActiveFlag = true;
                }
                //result.IMECM_UpdateDate = DateTime.Now;
                _db.Update(result);
                int rowAffected = _db.SaveChanges();
                if (rowAffected > 0)
                {
                    user.returnval = "true";
                }
                else
                {
                    user.returnval = "false";
                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return user;
        }

      
    }
}
