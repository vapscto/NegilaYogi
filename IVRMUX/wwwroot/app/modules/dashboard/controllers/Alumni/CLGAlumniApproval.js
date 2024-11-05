

(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGAlumniApprovalController', CLGAlumniApprovalController)

    CLGAlumniApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function CLGAlumniApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
        
        $scope.checkboxchcked = [];
        var pageid = 1;
        $scope.loaddata = function () {     
            apiService.getURI("CLGAlumniMembership/get_intial_data", pageid).
                then(function (promise) {
                   
                    $scope.almstudentDetails = promise.almdetails;                   
                })
        }
        $scope.changecheckmatch22 = function (user) {

            $scope.listdata = [];
            var count = 0;
            angular.forEach($scope.almstudentDetails, function (user) {
                if (user.isSelected22 == true) {
                    $scope.listdata.push(user);
                }
            })
            angular.forEach($scope.almstudentDetails, function (user) {
                user.isSelected22 = false;
            })
            
            angular.forEach($scope.almstudentDetails, function (dd) {
                angular.forEach($scope.listdata, function (ww) {
                    if (dd.alcsreG_Id == ww.alcsreG_Id) {
                        user.isSelected22 = true;
                    }
                });
            });
           
        };
        $scope.page = "page";
        $scope.reverse = true;

        $scope.page2 = "page2";
        $scope.reverse2 = true;

        $scope.currentPage = 1;
        $scope.itemsPerPages = 10;
          $scope.currentPage2 = 1;
        $scope.itemsPerPages2 = 10;

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
        //checking min and max age details
        $scope.onBranchchange = function (branchId) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {

                    if (promise.message == "MaxCapacity") {
                        swal("Sorry,Branch Capacity is Full");

                    }
                    else {

                        if (promise.semesters != null) {
                            $scope.semesters = promise.semesters;
                        }
                        else {
                            swal("No Semester Is Mapped To Selected Branch");
                            $scope.semesters = "";
                        }
                    }
                })

            }
        }

        $scope.onCourseChange = function (courseId) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": $scope.obj.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
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
                        $scope.branches = "";
                    }
                })
            }
        }

        $scope.checkstudent = function (asmyid, course, branch, sem) {

            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semlist = [];

            if (course === 0) {
                angular.forEach($scope.courses, function (uem) {
                    $scope.courselist.push(uem.amcO_Id);
                });
            }
            else {
                $scope.courselist.push(course);
            }
            //if (branch === 0) {
            //    angular.forEach($scope.branches, function (uem) {
            //        $scope.branchlist.push(uem.amB_Id);
            //    });
            //}
            //else {
            //    $scope.branchlist.push(branch);
            //}
            //if (sem === 0) {
            //    angular.forEach($scope.semesters, function (uem) {
            //        $scope.semlist.push(uem.amsE_Id);
            //    });
            //}
            //else {
            //    $scope.semlist.push(sem);
            //}


            var data = {
                "ASMAY_Id": asmyid,
                "AMCO_Id": course
                //"courselist": $scope.courselist,
                //"branchlist": $scope.branchlist,
                //"semlist": $scope.semlist
            };
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

        $scope.checkstudentalumni = function (studentid) {
            debugger;
            $scope.check_list = [];
            angular.forEach($scope.almstudentDetails, function (aa) {
                if (aa.isSelected22 == true) {
                    $scope.check_list.push({ alcsreG_Id: aa.ALCSREG_Id})
                }
            })
            var sssss = $scope.check_list[0].alcsreG_Id;
            var data = {
                "ALCSREG_Id": sssss
            }
            apiService.create("CLGAlumniMembership/checkstudent", data).
                then(function (promise) {
                 
                    if (promise.studentDetails.length > 0) {                        
                        $scope.studentDetailscheck = promise.studentDetails;                        
                    }
                    else {
                        $scope.studentDetailscheck = {};
                        swal('No Records Found!!');
                        $state.reload();
                    }
                })
        }
        $scope.cleardata = function () {
            $state.reload();
        }

        $scope.onstudchange = function () {
            $scope.secondgrid = false;
            $scope.firstgrid = false;

            $scope.searcgbtn = false;
        }

        $scope.oncatchange = function () {

            var listOfStu = {
                "FMCC_Id": $scope.FMCC_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            //var catid = $scope.FMCC_Id;
            apiService.create("CLGAlumniMembership/catchange", listOfStu).
                then(function (promise) {
                    $scope.studentdrp = promise.fillstudentlst
                    $scope.checkbutton = false;
                    $scope.searcgbtn = false;
                })
        }

        $scope.submitted = false;
        $scope.searchdata = function () {
            if ($scope.myForm.$valid) {
                $scope.firstgrid = false;

                var listOfStu = {
                    "FMCC_Id": $scope.FMCC_Id,
                    "PASR_Id": $scope.pasr_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGAlumniMembership/oncheck", listOfStu).
                    then(function (promise) {
                        if (promise.fillstudentlst.length > 0) {
                            $scope.firstgrid = true;
                            $scope.checkbutton = false;
                            $scope.secondgrid = false;

                            $scope.displaysiblingdet = promise.fillstudentlst;
                            //swal('Record Checked Successfully', '');
                        }
                        else {
                            swal('No Records');
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.changecheckmatch = function (user1) {
            debugger;
            $scope.listdataaa = [];
            var count = 0;
            angular.forEach($scope.studentDetailscheck, function (user1) {
                if (user1.isSelected == true) {
                    $scope.listdataaa.push(user1);
                }
            })
            angular.forEach($scope.studentDetailscheck, function (user1) {
                user1.isSelected = false;
            })

            angular.forEach($scope.studentDetailscheck, function (ss) {
                angular.forEach($scope.listdataaa, function (vv) {
                    if (ss.alcsreG_Id == vv.alcsreG_Id) {
                        user1.isSelected = true;
                    }
                });
            });

        };
        $scope.aproovedata = function (arraystudent) {
            debugger;
            $scope.checkedlist = [];
            angular.forEach($scope.studentDetailscheck, function (bb) {
                if (bb.isSelected == true) {
                    debugger;
                    $scope.checkedlist.push({ ALCSREG_Id: bb.alcsreG_Id })
                }
            })
            var sssss = $scope.checkedlist[0].ALCSREG_Id;
            var data = {
                "ALCSREG_Id": sssss,               
                "ALCMST_Id": arraystudent[0].alcmsT_Id
            }
            apiService.create("CLGAlumniMembership/aproovedata", data).
                then(function (promise) {
                    debugger;
                    if (promise.returnval == "True") {
                        swal("Alumni member successfully approved!!");
                        $state.reload();
                    }
                    else {
                        swal("Alumni member not approved!!");
                        $state.reload();
                    }
                })
        }
        //Onchange Of Academic Year
        $scope.onchangeacc = function (trmA_Id) {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CLGAlumniMembership/Getstudentlist", data).
                then(function (promise) {
                    $scope.studentlst = promise.fillstudent;
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

        //-----student name selection change
        $scope.onchange = function (studentlst) {

            var studid = studentlst.amcsT_ID;

            $scope.acc = true;
            $scope.accyer = $scope.ASMAY_Id

            var data = {
                "AMCST_Id": studid,
                "ASMAY_Id": $scope.accyer
            }
            apiService.create("AlumniMembership/getstudata", data).
                then(function (promise) {

                    $scope.ALCMST_AdmNo = promise.studentDetails[0].alcmsT_AdmNo;

                    $scope.AMST_JOIN_YEAR = promise.studentDetails[0].amsT_JOIN_YEAR;
                    $scope.AMST_JOIN_LEFT = promise.studentDetails[0].amsT_JOIN_LEFT;
                    $scope.ASMCL_Id_Join = promise.studentDetails[0].amsT_CLASS_JOIN;
                    $scope.ASMCL_Id_Left = promise.studentDetails[0].amsT_CLASS_LEFT;

                    $scope.ALCMST_PhoneNo = promise.studentDetails[0].alcmsT_PhoneNo;
                    $scope.ALCMST_PerStreet = promise.studentDetails[0].alcmsT_PerStreet;
                    $scope.ALCMST_PerArea = promise.studentDetails[0].alcmsT_PerArea;

                    $scope.arrlist = promise.countryDrpDown;

                    $scope.ALCMST_PerCountry = promise.ALCMST_PerCountry;

                    $scope.arrStatelist2 = promise.statedropdown;

                    $scope.ALCMST_PerState = promise.ALCMST_PerState;

                    $scope.ALCMST_FatherName = promise.studentDetails[0].alcmsT_FatherName;
                    $scope.ALCMST_DOB = promise.studentDetails[0].alcmsT_DOB;
                    $scope.ALCMST_MobileNo = promise.studentDetails[0].alcmsT_MobileNo;
                    $scope.ALCMST_emailId = promise.studentDetails[0].alcmsT_emailId;
                    //$scope.ALCMST_Remarks = promise.studentDetails[0].ALCMST_Remarks;
                    $scope.ALCMST_PerCity = promise.studentDetails[0].alcmsT_PerCity;
                    $scope.ALCMST_PerPincode = promise.studentDetails[0].alcmsT_PerPincode;

                    $scope.ALCMST_Marital_Status = promise.studentDetails[0].alcmsT_Marital_Status;

                    $scope.ALCMST_Id = promise.alcmsT_Id;


                    $scope.ALCMST_PUC_QS_DETAILS = promise.saveddata[0].alcmsT_PUC_QS_DETAILS;
                    $scope.ALCMST_PUC_INS_NAME = promise.saveddata[0].alcmsT_PUC_INS_NAME;
                    $scope.ALCMST_PUC_PASSED_OUT = promise.saveddata[0].ALCMST_PUC_PASSED_OUT;
                    $scope.ALCMST_PUC_PERCENTAGE = promise.saveddata[0].ALCMST_PUC_PERCENTAGE;
                    $scope.ALCMST_PUC_PLACE = promise.saveddata[0].ALCMST_PUC_PLACE;
                    $scope.ALCMST_PUC_STATE = promise.saveddata[0].ALCMST_PUC_STATE;

                    $scope.ALCMST_UG_QS_DETAILS = promise.saveddata[0].ALCMST_UG_QS_DETAILS;
                    $scope.ALCMST_UG_INS_NAME = promise.saveddata[0].ALCMST_UG_INS_NAME;
                    $scope.ALCMST_UG_PASSED_OUT = promise.saveddata[0].ALCMST_UG_PASSED_OUT;
                    $scope.ALCMST_UG_PERCENTAGE = promise.saveddata[0].ALCMST_UG_PERCENTAGE;

                    $scope.ALCMST_UG_PLACE = promise.saveddata[0].ALCMST_UG_PLACE;
                    $scope.ALCMST_UG_STATE = promise.saveddata[0].ALCMST_UG_STATE;
                    $scope.ALCMST_PG_QS_DETAILS = promise.saveddata[0].ALCMST_PG_QS_DETAILS;
                    $scope.ALCMST_PG_INS_NAME = promise.saveddata[0].ALCMST_PG_INS_NAME;

                    $scope.ALCMST_PG_PASSED_OUT = promise.saveddata[0].ALCMST_PG_PASSED_OUT;
                    $scope.ALCMST_PG_PERCENTAGE = promise.saveddata[0].ALCMST_PG_PERCENTAGE;
                    $scope.ALCMST_PG_PLACE = promise.saveddata[0].ALCMST_PG_PLACE;
                    $scope.ALCMST_PG_STATE = promise.saveddata[0].ALCMST_PG_STATE;

                    $scope.ALCMST_ACH_DET = promise.saveddata[0].ALCMST_ACH_DET;
                    $scope.ALCMST_ACH_REMARKS = promise.saveddata[0].ALCMST_ACH_REMARKS;
                    $scope.ALCMST_PRO_COMPANY_NAME = promise.saveddata[0].ALCMST_PRO_COMPANY_NAME;
                    $scope.ALCMST_PRO_DESIGNATION = promise.saveddata[0].ALCMST_PRO_DESIGNATION;

                    $scope.ALCMST_PRO_OFFICE_NO = promise.saveddata[0].ALCMST_PRO_OFFICE_NO;
                    $scope.ALCMST_PRO_ADDRESS = promise.saveddata[0].ALCMST_PRO_ADDRESS;
                    $scope.ALCMST_PRO_REMARKS = promise.saveddata[0].ALCMST_PRO_REMARKS;

                })
        }

       
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
       

    }
})();
