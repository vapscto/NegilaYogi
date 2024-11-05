using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeMasterOtherStudentImpl:interfaces.FeeMasterOtherStudentInterface
    {
        public FeeGroupContext _FeeContext;

        public FeeMasterOtherStudentImpl(FeeGroupContext db)
        {
            _FeeContext = db;
        }

        public FeeMasterOtherStudentDTO getdetails(int id)
        {
            FeeMasterOtherStudentDTO obj = new FeeMasterOtherStudentDTO();
            try
            {
                obj.otherstudentList = _FeeContext.FeeMasterOtherStudentDMO.Where(d => d.MI_Id == id && d.FMOST_ActiveFlag == true).ToArray();
                if (obj.otherstudentList.Length > 0)
                {
                    obj.count = obj.otherstudentList.Length;
                }
                else
                {
                    obj.count = 0;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public FeeMasterOtherStudentDTO save(FeeMasterOtherStudentDTO data)
        {
            try
            {
                if(data.FMOST_Id > 0)
                {
                    var checkduplicate = _FeeContext.FeeMasterOtherStudentDMO.Where(d => d.MI_Id == data.MI_Id && d.FMOST_ActiveFlag == true &&d.FMOST_StudentEmailId.Equals(data.FMOST_StudentEmailId) && d.FMOST_StudentMobileNo==data.FMOST_StudentMobileNo && d.FMOST_StudentName.Equals(data.FMOST_StudentName) && d.FMOST_Id!=data.FMOST_Id).ToList();
                    if(checkduplicate.Count==0)
                    {
                        var result = _FeeContext.FeeMasterOtherStudentDMO.Single(d => d.FMOST_Id == data.FMOST_Id);
                        result.FMOST_StudentEmailId = data.FMOST_StudentEmailId;
                        result.FMOST_StudentMobileNo = data.FMOST_StudentMobileNo;
                        result.FMOST_StudentName = data.FMOST_StudentName;
                        result.UpdatedDate = DateTime.Now;
                        _FeeContext.Update(result);
                      var flag=  _FeeContext.SaveChanges();
                        if(flag > 0)
                        {
                            data.returnval = "updated";
                        }
                        else
                        {
                            data.returnval = "updatefailed";
                        }

                    }
                    else
                    {
                        data.returnval = "duplicate";
                    }

                }
                else
                {
                    var checkduplicate = _FeeContext.FeeMasterOtherStudentDMO.Where(d => d.MI_Id == data.MI_Id && d.FMOST_ActiveFlag == true && d.FMOST_StudentEmailId.Equals(data.FMOST_StudentEmailId) && d.FMOST_StudentMobileNo == data.FMOST_StudentMobileNo && d.FMOST_StudentName.Equals(data.FMOST_StudentName)).ToList();
                    if(checkduplicate.Count ==0)
                    {
                        FeeMasterOtherStudentDMO dmo = Mapper.Map<FeeMasterOtherStudentDMO>(data);
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.FMOST_ActiveFlag = true;
                        _FeeContext.Add(dmo);
                      var flag  = _FeeContext.SaveChanges();
                        if(flag > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "savefailed";
                        }
                    }
                    else
                    {
                        data.returnval = "duplicate";
                    }
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return data;
        }
        public FeeMasterOtherStudentDTO edit(int id)
        {
            FeeMasterOtherStudentDTO data = new FeeMasterOtherStudentDTO();
            try
            {
                data.otherstudentList = _FeeContext.FeeMasterOtherStudentDMO.Where(d => d.FMOST_Id == id).ToArray();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeMasterOtherStudentDTO delete(int id)
        {
            FeeMasterOtherStudentDTO data = new FeeMasterOtherStudentDTO();
            try
            {
                var query = _FeeContext.FeeMasterOtherStudentDMO.Where(d => d.FMOST_Id == id).ToList();
                if (query.Any())
                {
                    _FeeContext.Remove(query.ElementAt(0));
                    var del = _FeeContext.SaveChanges();
                    if (del > 0)
                    {
                        data.returnval = "deleted";
                    }
                    else
                    {
                        data.returnval = "deletefailed";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data.returnval = "failed";
            }
            return data;
        }

    }
}
