using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.University;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Service
{
    public class HSU_341_EthicsImpl:Interface.HSU_341_EthicsInterface
    {
        public GeneralContext _context;
        public HSU_341_EthicsImpl(GeneralContext y)
        {
            _context = y;
        }
        public HSU_341_EthicsDTO loaddata(HSU_341_EthicsDTO data)
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
                data.yearlist = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();
                data.alldata1 = (from a in _context.HSU_341_EthicsDMO
                                 from y in _context.Academic
                                 where (a.MI_Id == data.MI_Id && a.NCMC331ES_Year == y.ASMAY_Id)
                                 select new HSU_341_EthicsDTO
                                 {
                                     ASMAY_Year = y.ASMAY_Year,
                                     NCMC331ES_Year = a.NCMC331ES_Year,
                                     NCMC331ES_Id = a.NCMC331ES_Id,
                                     NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag = a.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag,
                                     NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag = a.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag,
                                     NCMC331ES_InstPlagiarismSoftInstPolicyFlag = a.NCMC331ES_InstPlagiarismSoftInstPolicyFlag,
                                     NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag = a.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag,
                                     NCMC331ES_ActiveFlag = a.NCMC331ES_ActiveFlag,
                                     MI_Id = data.MI_Id,
                                 }).Distinct().OrderByDescending(t => t.NCMC331ES_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public HSU_341_EthicsDTO savedata(HSU_341_EthicsDTO data)
        {
            try
            {
                if (data.NCMC331ES_Id == 0)
                {
                    var duplicate = _context.HSU_341_EthicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC331ES_Year == data.ASMAY_Id && t.NCMC331ES_Id != 0
                    && t.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag == data.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag && t.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag == data.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag && t.NCMC331ES_InstPlagiarismSoftInstPolicyFlag == data.NCMC331ES_InstPlagiarismSoftInstPolicyFlag && t.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag == data.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HSU_341_EthicsDMO obj1 = new HSU_341_EthicsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC331ES_Year = data.ASMAY_Id;
                        obj1.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag = data.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag;
                        obj1.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag = data.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag;
                        obj1.NCMC331ES_InstPlagiarismSoftInstPolicyFlag = data.NCMC331ES_InstPlagiarismSoftInstPolicyFlag;
                        obj1.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag = data.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag;
                        obj1.NCMC331ES_ActiveFlag = true;
                        obj1.NCMC331ES_CreatedDate = DateTime.Now;
                        obj1.NCMC331ES_UpdatedDate = DateTime.Now;
                        obj1.NCMC331ES_CreatedBy = data.UserId;
                        obj1.NCMC331ES_UpdatedBy = data.UserId;
                        _context.Add(obj1);
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
                }
                else if (data.NCMC331ES_Id > 0)
                {
                    var duplicate = _context.HSU_341_EthicsDMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMC331ES_Id != data.NCMC331ES_Id && t.NCMC331ES_Year == data.ASMAY_Id
                    && t.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag == data.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag
                    && t.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag == data.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag
                    && t.NCMC331ES_InstPlagiarismSoftInstPolicyFlag == data.NCMC331ES_InstPlagiarismSoftInstPolicyFlag
                    && t.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag == data.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _context.HSU_341_EthicsDMO.Where(t => t.NCMC331ES_Id == data.NCMC331ES_Id && t.MI_Id == data.MI_Id).Single();
                        update.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag = data.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag;
                        update.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag = data.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag;
                        update.NCMC331ES_InstPlagiarismSoftInstPolicyFlag = data.NCMC331ES_InstPlagiarismSoftInstPolicyFlag;
                        update.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag = data.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag;
                        update.NCMC331ES_Year = data.ASMAY_Id;
                        update.MI_Id = data.MI_Id;
                        update.NCMC331ES_UpdatedDate = DateTime.Now;
                        update.NCMC331ES_UpdatedBy = data.UserId;
                        _context.Update(update);
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HSU_341_EthicsDTO editdata(HSU_341_EthicsDTO data)
        {
            try
            {
                data.editlist = (from a in _context.HSU_341_EthicsDMO
                                 from b in _context.Academic
                                 where (a.NCMC331ES_Id == data.NCMC331ES_Id && a.MI_Id == data.MI_Id && a.NCMC331ES_Year == b.ASMAY_Id && a.MI_Id == b.MI_Id)
                                 select new HSU_341_EthicsDTO
                                 {
                                     NCMC331ES_Id = a.NCMC331ES_Id,
                                     MI_Id = a.MI_Id,
                                     NCMC331ES_Year = a.NCMC331ES_Year,
                                     ASMAY_Year = b.ASMAY_Year,
                                     NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag = a.NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag,
                                     NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag = a.NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag,
                                     NCMC331ES_InstPlagiarismSoftInstPolicyFlag = a.NCMC331ES_InstPlagiarismSoftInstPolicyFlag,
                                     NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag = a.NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag,
                                 }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HSU_341_EthicsDTO deactive(HSU_341_EthicsDTO data)
        {
            try
            {
                var u = _context.HSU_341_EthicsDMO.Where(t => t.NCMC331ES_Id == data.NCMC331ES_Id).SingleOrDefault();
                if (u.NCMC331ES_ActiveFlag == true)
                {
                    u.NCMC331ES_ActiveFlag = false;
                }
                else if (u.NCMC331ES_ActiveFlag == false)
                {
                    u.NCMC331ES_ActiveFlag = true;
                }
                u.NCMC331ES_UpdatedDate = DateTime.Now;
                u.NCMC331ES_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
