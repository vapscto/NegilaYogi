(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGAlumniHomepageController1', CLGAlumniHomepageController1)

    CLGAlumniHomepageController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function CLGAlumniHomepageController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        pageid = 1;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.currentPage = 1;
        $scope.itemsPerPages = 10;
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //======================================================================
        $scope.qualificationDetails = [{ id: 'qualification1' }];
        $scope.addNewQualification = function () {
            var newItemNo = $scope.qualificationDetails.length + 1;

            if (newItemNo <= 3) {
                $scope.qualificationDetails.push({ 'id': 'qualification' + newItemNo });
            }
        };

        $scope.removeNewQualification = function (index, data) {
            var newItemNo = $scope.qualificationDetails.length - 1;
            $scope.qualificationDetails.splice(index, 1);
            if ($scope.qualificationDetails.length == 0)
                var data = {
                    "ALCSQU_Id": index.alcsqU_Id
                }           
        };       
        //======================Achievements
        $scope.achievementsDetails = [{ id: 'achievements1' }];
        $scope.addNewAchievements = function () {
            var newItemNo = $scope.achievementsDetails.length + 1;

            if (newItemNo <= 3) {
                $scope.achievementsDetails.push({ 'id': 'achievements' + newItemNo });
            }
        };

        $scope.removeNewAchievements = function (index, data) {
            var newItemNo = $scope.achievementsDetails.length - 1;
            $scope.achievementsDetails.splice(index, 1);
            if ($scope.achievementsDetails.length == 0)
                var data = {
                    "ALCSAC_Id": index.alcsaC_Id
                }
        };

       
        //=================

        $scope.professionalDetails = [{ id: 'professional1' }];
        $scope.addNewProfessional = function () {
            var newItemNo = $scope.professionalDetails.length + 1;

            if (newItemNo <= 3) {
                $scope.professionalDetails.push({ 'id': 'professional' + newItemNo });
            }
        };

        $scope.removeNewProfessional = function (index, data) {
            var newItemNo = $scope.professionalDetails.length - 1;
            $scope.professionalDetails.splice(index, 1);
            if ($scope.professionalDetails.length == 0)
                var data = {
                    "ALCSPR_Id": index.alcspR_Id
                }
        };

        $scope.onYearCahnge = function (acdYId) {
            apiService.getURI("ApplicationForm/getCourse/", acdYId).then(function (promise) {

                if (promise.courses != null) {
                    $scope.courses = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.courses = "";
                }
            });
        }

        //Left 
        $scope.onYearCahngeLeft = function (acdYId) {
            apiService.getURI("ApplicationForm/getCourse/", acdYId).then(function (promise) {

                if (promise.courses != null) {
                    $scope.coursesleft = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.coursesleft = "";
                }
            });
        }
        $scope.onCourseChange = function (courseId, asmyid) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": asmyid,
                    "ACAYC_Id": selectedData[0].acayC_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                     
                        if (promise.studentCategory != null) {
                            //$scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branches = "";
                    }
                })
            }
        }

        $scope.onBranchchange = function (branchId, asmyid) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": asmyid,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {

                    if (promise.semesters != null) {
                        $scope.semesters = promise.semesters;
                    }
                    else {
                        swal("No Semester Is Mapped To Selected Branch");
                        $scope.semesters = "";
                    }
                })

            }
        }

        $scope.onCourseChangeLeft = function (courseId, asmyid) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": asmyid,
                    "ACAYC_Id": selectedData[0].acayC_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branchesleft = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branchesleft = "";
                    }
                })
            }
        }

        $scope.onBranchchangeLeft = function (branchId, asmyid) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": asmyid,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {

                    if (promise.semesters != null) {
                        $scope.semestersleft = promise.semesters;
                    }
                    else {
                        swal("No Semester Is Mapped To Selected Branch");
                        $scope.semestersleft = "";
                    }
                })

            }
        }

        $scope.onchangecountry = function (ALMST_PerCountry) {
            var data = {
                "ALMST_PerCountry": ALMST_PerCountry
            }
            apiService.create("AlumniMembership/onchangecountry", data).
                then(function (promise) {
                    $scope.arrStatelist2 = promise.statedropdown;
                })
        }

        //===================================================================
        //Loading the Intial Data Function
        $scope.loaddata = function () {
        
            apiService.getURI("CLGAlumniMembership/get_intial_data", pageid).
                then(function (promise) {
                   
                    if (promise.alumnitrue == false) {
                        $scope.adminslumni = true;                        
                        $scope.classlist = promise.fillclass;
                        $scope.arrlist = promise.countryDrpDown;
                        $scope.yearlst = promise.fillyear;
                    }
                    else {
                        $scope.adminslumni = false;
                        $scope.ALCMST_AdmNo = promise.studentDetails[0].alcmsT_AdmNo;
                        $scope.studentname = promise.studentDetails[0].amcsT_FirstName;
                        $scope.ASMAY_Id_Join = promise.studentDetails[0].yeardmitted;
                        $scope.ASMAY_Id_Left = promise.studentDetails[0].yearleft;
                        $scope.AMCO_JOIN_Id = promise.studentDetails[0].courseadmitted;
                        $scope.AMCO_Left_Id = promise.studentDetails[0].courseleft;
                        $scope.AMB_JOIN_Id = promise.studentDetails[0].branchadmitted;
                        $scope.AMB_Id_Left = promise.studentDetails[0].branchleft;
                        $scope.ALCMST_PhoneNo = promise.studentDetails[0].alcmsT_PhoneNo;
                        $scope.ALCMST_PerStreet = promise.studentDetails[0].alcmsT_PerStreet;
                        $scope.ALCMST_PerArea = promise.studentDetails[0].alcmsT_PerArea;
                        $scope.arrlist = promise.countryDrpDown;
                        $scope.ALCMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                        $scope.arrStatelist2 = promise.statedropdown;
                        $scope.ALCMST_PerState = promise.studentDetails[0].alcmsT_PerState;
                        $scope.ALCMST_PerAdd3 = promise.studentDetails[0].alcmsT_PerAdd3;
                        $scope.ALCMST_FatherName = promise.studentDetails[0].alcmsT_FatherName;
                        $scope.ALCMST_DOB = promise.studentDetails[0].alcmsT_DOB;
                        $scope.ALCMST_MobileNo = promise.studentDetails[0].alcmsT_MobileNo;
                        $scope.ALCMST_emailId = promise.studentDetails[0].alcmsT_emailId;
                        //$scope.ALCMST_Remarks = promise.studentDetails[0].ALCMST_Remarks;
                        $scope.ALCMST_PerCity = promise.studentDetails[0].alcmsT_PerCity;
                        $scope.ALCMST_PerPincode = promise.studentDetails[0].alcmsT_PerPincode;
                        $scope.ALCMST_Id = promise.alcmsT_Id;
                        $scope.studentqualification = promise.studentqualification;
                        $scope.studentproffession = promise.studentproffession;
                        $scope.studentachievement = promise.studentachievement;

                        if ($scope.studentqualification.length > 0)
                        {
                                $scope.ALCSQU_Qulification = promise.studentqualification[0].alcsqU_Qulification;
                                $scope.ALCSQU_YearOfPassing = promise.studentqualification[0].alcsqU_YearOfPassing;
                                $scope.ALCSQU_University = promise.studentqualification.alcsqU_University;
                                $scope.ALCSQU_OtherDetails = promise.studentqualification.alcsqU_OtherDetails;                           
                        }
                        if ($scope.studentachievement.length > 0) {
                            $scope.ALCSAC_Achievement = promise.studentachievement[0].alcsaC_Achievement;
                            $scope.ALCSAC_Remarks = promise.studentachievement[0].alcsaC_Remarks;
                        }
                        if ($scope.studentproffession.length > 0) {
                            $scope.ALCSPR_CompanyName = promise.studentproffession[0].alcspR_CompanyName;
                            $scope.ALCSPR_CompanyAddress = promise.studentproffession[0].alcspR_CompanyAddress;
                            $scope.ALCSPR_Designation = promise.studentproffession[0].alcspR_Designation;
                            $scope.ALCSPR_EmailId = promise.studentproffession[0].alcspR_EmailId;
                            $scope.ALCSPR_Phone = promise.studentproffession[0].alcspR_Phone;
                            $scope.ALCSPR_WorkingSince = promise.studentproffession[0].alcspR_WorkingSince;
                            $scope.ALCSPR_OtherDetails = promise.studentproffession[0].alcspR_OtherDetails;
                        } 
                    }                   
                })
        }
        $scope.cleardata = function () {
            $state.reload();
        }
        $scope.clear = function () {
            $scope.ALMST_AdmNo = "";
            $scope.AMST_JOIN_YEAR = "";
            $scope.AMST_JOIN_LEFT = "";
            $scope.ASMCL_Id_Join = "";
            $scope.ASMCL_Id_Left = "";
            $scope.ALMST_PhoneNo = "";
            $scope.ALMST_PerStreet = "";
            $scope.ALMST_PerArea = "";
            $scope.arrlist = "";
            $scope.ALMST_PerCountry = "";
            $scope.arrStatelist2 = "";
            $scope.ALMST_PerState = "";
            $scope.ALMST_FatherName = "";
            $scope.ALMST_DOB = "";
            $scope.ALMST_MobileNo = "";
            $scope.ALMST_emailId = "";
            $scope.ALMST_Remarks = "";
            $scope.ALMST_PerCity = "";
            $scope.ALMST_PerPincode = "";
            $scope.ALMST_Marital_Status = "";
            $scope.ALMST_Id = "";
            $scope.ALMST_PUC_QS_DETAILS = "";
            $scope.ALMST_PUC_INS_NAME = "";
            $scope.ALMST_PUC_PASSED_OUT = "";
            $scope.ALMST_PUC_PERCENTAGE = "";
            $scope.ALMST_PUC_PLACE = "";
            $scope.ALMST_PUC_STATE = "";
            $scope.ALMST_UG_QS_DETAILS = "";
            $scope.ALMST_UG_INS_NAME = "";
            $scope.ALMST_UG_PASSED_OUT = "";
            $scope.ALMST_UG_PERCENTAGE = "";
            $scope.ALMST_UG_PLACE = "";
            $scope.ALMST_UG_STATE = "";
            $scope.ALMST_PG_QS_DETAILS = "";
            $scope.ALMST_PG_INS_NAME = "";
            $scope.ALMST_PG_PASSED_OUT = "";
            $scope.ALMST_PG_PERCENTAGE = "";
            $scope.ALMST_PG_PLACE = "";
            $scope.ALMST_PG_STATE = "";
            $scope.ALMST_ACH_DET = "";
            $scope.ALMST_ACH_REMARKS = "";
            $scope.ALMST_PRO_COMPANY_NAME = "";
            $scope.ALMST_PRO_DESIGNATION = "";
            $scope.ALMST_PRO_OFFICE_NO = "";
            $scope.ALMST_PRO_ADDRESS = "";
            $scope.ALMST_PRO_REMARKS = "";
        }
        //Onchange Of Academic Year
        $scope.onchangeacc = function (trmA_Id) {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CLGAlumniMembership/Getstudentlist", data).
                then(function (promise) {
                    $scope.studentlst = promise.stu_name;
                    if (promise.stu_name.length > 0) {
                        $scope.accchange = false;
                        $scope.stu_name = promise.stu_name;
                        $scope.clear();
                    }
                    else {
                        swal('No Records Found!!');
                        $scope.clear();
                    }
                })
        }
        //Onchange Of Academic Year for Approval
        $scope.onchangeaccapp = function (trmA_Id) {
            $scope.stu_name = {};
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CLGAlumniMembership/Getstudentlistapp", data).
                then(function (promise) {                   
                    if (promise.stu_name.length > 0) {
                        //$scope.accchange = false;
                        $scope.stu_name = promise.stu_name;
                        $scope.ALSREG_Id = $scope.stu_name[0].alsreG_Id;
                    }
                    else {
                        $scope.stu_name = {};
                        $scope.ALSREG_Id = "";
                        swal('No Records Found!!');
                    }
                })
        }
        $scope.onchangecountry = function (ALMST_PerCountry) {
            var data = {
                "ALMST_PerCountry": ALMST_PerCountry
            }
            apiService.create("CLGAlumniMembership/onchangecountry", data).
                then(function (promise) {
                    $scope.arrStatelist2 = promise.statedropdown;
                })
        }
        $scope.checkstudent = function (studentid) {           
            var data = {
                "ALSREG_Id": studentid
            }
            apiService.create("CLGAlumniMembership/checkstudent", data).
                then(function (promise) {
                    if (promise.studentDetails.length > 0) {
                        //$scope.accchange = false;
                        $scope.studentDetailscheck = promise.studentDetails;                      
                    }
                    else {
                        $scope.studentDetailscheck = {};
                        swal('No Records Found!!');
                    }

                })
        }
        //Student Search Dropdown
        $scope.aproovedata = function (ALSREG_Id, arraystudent) {
                var data = {
                    "ALSREG_Id": ALSREG_Id,
                    "ALMST_Id": arraystudent[0].almsT_Id
                }
                apiService.create("CLGAlumniMembership/aproovedata", data).
                    then(function (promise) {
                        if (promise.returnval == "True") {
                            swal("Alumni member successfully approved!!");
                            $state.reload();
                        }
                        else{
                            swal("Alumni member not approved!!");
                        }
                    })
        }
        $scope.searchfilter = function (objj) {
           
            if (objj.search.length >= '2') {
                var data = {
                    "searchfilter": objj.search,
                    "ASMAY_Id": $scope.ASMAY_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGAlumniMembership/searchfilter", data).
                    then(function (promise) {
                       
                        $scope.studentlst = promise.fillstudent;
                        angular.forEach($scope.studentlst, function (objectt) {
                            if (objectt.amcsT_FirstName.length > 0) {
                                var string = objectt.amcsT_FirstName;
                                objectt.amcsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }
        }
        $scope.onSelectGetState = function (countryidd) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist2 = promise.stateDrpDown;
            })
        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;
        //save
        $scope.saveData = function () {
            if ($scope.adminslumni == false) {
                $scope.amcsT_ID = $scope.ALCMST_Id;
            }
            else {
                $scope.amcsT_ID = $scope.Amst_Id.amcsT_ID;
            }
            var data = {
                "ALCMST_Id": $scope.amcsT_ID,
                "ALCMST_DOB": $scope.ALCMST_DOB,
                "ALCMST_MobileNo": $scope.ALCMST_MobileNo,
                "ALCMST_emailId": $scope.ALCMST_emailId,
                "ALCMST_PerArea": $scope.ALCMST_PerArea,
                "ALCMST_PerCountry": $scope.ALCMST_PerCountry,
                "ALCMST_PerState": $scope.ALCMST_PerState,
                "ALCMST_PerCity": $scope.ALCMST_PerCity,
                "ALCMST_PerPincode": $scope.ALCMST_PerPincode,
                "ALCMST_PerAdd3": $scope.ALCMST_PerAdd3,
                "qualificationDetails": $scope.qualificationDetails,
                "professionalDetails": $scope.professionalDetails,
                "achievementsDetails": $scope.achievementsDetails
            };
            apiService.create("CLGAlumniMembership/savedata", data).
                then(function (promise) {
                    swal(promise.returnval);
                    $state.reload();
                });
        };
        //save
        $scope.search = "";
        $scope.savenewData = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "ALCMST_FirstName": $scope.ALCMST_FirstName,
                    "ALCMST_AdmNo": $scope.ALCMST_AdmNo,
                    "ALCMST_FatherName": $scope.ALCMST_FatherName,
                    "ALCMST_emailId": $scope.ALCMST_emailId,
                    "AMCO_JOIN_Id": $scope.AMCO_JOIN_Id,
                    "AMCO_Left_Id": $scope.AMCO_Left_Id,
                    "ASMAY_Id_Join": $scope.ASMAY_Id_Join,
                    "ASMAY_Id_Left": $scope.ASMAY_Id_Left,
                    "AMB_JOIN_Id": $scope.AMB_JOIN_Id,
                    "AMB_Id_Left": $scope.AMB_Id_Left,
                    "AMSE_JOIN_Id": $scope.AMSE_JOIN_Id,
                    "AMSE_Id_Left": $scope.AMSE_Id_Left,
                    "ALCMST_MobileNo": $scope.ALCMST_MobileNo,
                    "ALCMST_PerStreet": $scope.ALCMST_PerStreet,
                    "ALCMST_PerCity": $scope.ALCMST_PerCity,
                    "ALCMST_PerPincode": $scope.ALCMST_PerPincode,
                    "ALCMST_PerState": $scope.ALCMST_PerState,
                    "ALCMST_PerArea": $scope.ALCMST_PerArea,
                    "ALCMST_PerAdd3": $scope.ALCMST_PerAdd3,
                    "ALCMST_DOB": new Date($scope.ALCMST_DOB),
                    "ALCMST_PerCountry": $scope.ALCMST_PerCountry,
                    "qualificationDetails": $scope.qualificationDetails,
                    "professionalDetails": $scope.professionalDetails,
                    "achievementsDetails": $scope.achievementsDetails
                };
                apiService.create("CLGAlumniMembership/savedata", data).
                    then(function (promise) {
                        swal(promise.returnval);
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        //-----student name selection change
        $scope.onchange = function (studentlst) {
           
            var studid = studentlst.amcsT_ID;
            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id
            var data = {
                "ALCMST_Id": studid,
                "ASMAY_Id": $scope.accyer
            }
            apiService.create("CLGAlumniMembership/getstudata", data).
                then(function (promise) {
                 
                        $scope.ALCMST_AdmNo = promise.studentDetails[0].alcmsT_AdmNo;
                        $scope.ASMAY_Id_Join = promise.studentDetails[0].yeardmitted;
                        $scope.ASMAY_Id_Left = promise.studentDetails[0].yearleft;
                        $scope.AMCO_JOIN_Id = promise.studentDetails[0].courseadmitted;
                        $scope.AMCO_Left_Id = promise.studentDetails[0].courseleft;
                        $scope.AMB_JOIN_Id = promise.studentDetails[0].branchadmitted;
                        $scope.AMB_Id_Left = promise.studentDetails[0].branchleft;
                        $scope.ALCMST_PhoneNo = promise.studentDetails[0].alcmsT_PhoneNo;
                        $scope.ALCMST_PerStreet = promise.studentDetails[0].alcmsT_PerStreet;
                        $scope.ALCMST_PerArea = promise.studentDetails[0].alcmsT_PerArea;
                        $scope.arrlist = promise.countryDrpDown;
                        $scope.ALCMST_PerCountry = promise.studentDetails[0].ivrmmC_Id;
                        $scope.arrStatelist2 = promise.statedropdown;
                        $scope.ALCMST_PerState = promise.studentDetails[0].alcmsT_PerState;
                        $scope.ALCMST_PerAdd3 = promise.studentDetails[0].alcmsT_PerAdd3;
                        $scope.ALCMST_FatherName = promise.studentDetails[0].alcmsT_FatherName;
                        $scope.ALCMST_DOB = promise.studentDetails[0].alcmsT_DOB;
                        $scope.ALCMST_MobileNo = promise.studentDetails[0].alcmsT_MobileNo;
                        $scope.ALCMST_emailId = promise.studentDetails[0].alcmsT_emailId;
                        //$scope.ALCMST_Remarks = promise.studentDetails[0].ALCMST_Remarks;
                        $scope.ALCMST_PerCity = promise.studentDetails[0].alcmsT_PerCity;
                        $scope.ALCMST_PerPincode = promise.studentDetails[0].alcmsT_PerPincode;
                        $scope.ALCMST_Id = promise.alcmsT_Id;
                        $scope.studentqualification = promise.studentqualification;
                        $scope.studentproffession = promise.studentproffession;
                        $scope.studentachievement = promise.studentachievement;

                    if ($scope.studentqualification.length > 0) {
                        $scope.qualificationDetails = $scope.studentqualification;
                    }
                    else {
                        $scope.qualificationDetails = [{ id: 'qualification1' }];
                    }
                  
                    if ($scope.studentachievement.length > 0) {
                        $scope.achievementsDetails = $scope.studentachievement;
                    }
                    else {
                        $scope.achievementsDetails = [{ id: 'achievements1' }];
                    }
                       
                    
                    if ($scope.studentproffession.length > 0) {
                        $scope.professionalDetails = $scope.studentproffession;
                        
                    }
                    else {
                        $scope.professionalDetails = [{ id: 'professional1' }];
                    }
        })
        }
        
    }
})();