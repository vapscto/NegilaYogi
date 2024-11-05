
(function () {
    'use strict';
    angular.module('app').controller('sectionallotmentController', sectionallotmentController)
    sectionallotmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function sectionallotmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {
        $scope.student = {};

        $rootScope.refresh = function () {
            $log.info("fired");
            eventService.events();
        }

        //declaration part 
        $scope.studentList = [];
        $scope.resultData = [];
        $scope.resultData_CS = [];
        $scope.resultData_PS = [];
        $scope.resultData_YS = [];
        $scope.studentList1 = [];
        $scope.studentList2 = [];
        $scope.studentList3 = [];
        $scope.studentList4 = [];
        $scope.studentList5 = [];
        $scope.studentList6 = [];

        $scope.ASMCL_Id_PS1dis = true;
        $scope.AMAY_Id_PS1dis = true;
        $scope.AMAY_Id_YLS1dis = true;

        $scope.sortKey100 = 'amaY_RollNo';
        $scope.sortReverse = false;

        $scope.searchValuenew = "";

        $scope.ddate = {};
        $scope.objj = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;

        // On load page get all dropdown list
        $scope.LoadData = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("SectionAllotment/getalldetails", pageid).then(function (promise) {

                $scope.yearList = promise.yearList;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                $scope.studentList = promise.studentList;
                $scope.studentList1 = promise.studentList;
                $scope.studentList2 = promise.studentList;
                $scope.studentList3 = promise.studentList;
                $scope.studentList4 = promise.studentList;
                $scope.studentList5 = promise.studentList;
                $scope.studentList6 = promise.studentList;
                $scope.AMAY_Id = promise.yearList[0].asmaY_Id;
                $scope.AMAY_Id_CS = promise.yearList[0].asmaY_Id;
                $scope.AMAY_Id_DS = promise.yearList[0].asmaY_Id;
                $scope.AMAY_Id_PS = promise.yearList[0].asmaY_Id;
                $scope.AMAY_Id_YLS = promise.yearList[0].asmaY_Id;
                $scope.classListps = promise.classList;
                $scope.yearList1ps = promise.yearList;

                $scope.yearList2ys = promise.yearList;
                $scope.classListys = promise.classList;
                // $scope.NoOfYears = 1;
                // $scope.NoOfYearsYLS = 1;

            });
        };


        $scope.OnChangeAcademicYearns = function () {

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.studentList = [];
            $scope.resultData = [];
            $scope.finaldataNS = [];
            $scope.chckedIndexs = [];
            $scope.chckedIndexs1 = [];
            $scope.selectedAll1 = false;
            $scope.selectedAll = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.GetStudentListByYearAndCLass_NS = function () {
            $scope.ASMS_Id = "";
            $scope.resultData = [];
            $scope.selectedAll1 = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

            var data = {
                "ASMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "SectionAllotmentType": "New"
            }

            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {

                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList = promise.sectionAllotedStudentList;
                    $scope.count = promise.sectionAllotedStudentList.length;
                    $scope.presentCountgridnew = $scope.sectionAllotedStudentList.length;
                }
                else {

                    $scope.count = 0;
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.presentCountgridnew = "";
                }

                if (promise.studentlistCount > 0) {
                    $scope.studentList = promise.studentList;
                } else {
                    $scope.studentList = "";
                    //$scope.ASMCL_Id = "";
                    //$scope.ASMS_Id = "";
                    //$scope.myForm.$setPristine();
                    //$scope.myForm.$setUntouched();
                    swal('No Students Are Found To Allot Section');
                }
            });
        };

        //table data new section 
        $scope.chckedIndexs = [];
        $scope.chckedIndexs1 = [];
        $scope.checkAll = function () {
            if ($scope.studentList.length > 0) {
                var toggleStatus = $scope.selectedAll;
                angular.forEach($scope.studentList, function (itm) {
                    itm.Selected = toggleStatus;
                    if ($scope.chckedIndexs.indexOf(itm) === -1) {
                        $scope.chckedIndexs.push(itm);
                    }
                    else {
                        $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll = false;
            }
        };

        $scope.checkAll1 = function () {
            if ($scope.resultData.length > 0) {
                $scope.selectedAll1 = true;
                var toggleStatus = $scope.selectedAll1;
                angular.forEach($scope.resultData, function (itm) {
                    itm.Selected1 = toggleStatus;
                    if ($scope.chckedIndexs1.indexOf(itm) === -1) {
                        $scope.chckedIndexs1.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1.splice($scope.chckedIndexs1.indexOf(itm), 1);
                    }
                });

            }
            else {
                $scope.selectedAll1 = false;
            }
        };
        $scope.test = function (data) {
            $scope.selectedAll = $scope.studentList.every(function (itm) {
                return itm.Selected;
            })
        };

        $scope.GetFirtTableData = function () {

            if ($scope.selectedAll == true) {
                angular.forEach($scope.studentList, function (student) {
                    $scope.resultData.push(student);
                });
            } else {
                angular.forEach($scope.studentList, function (student) {
                    if (student.Selected == true) {
                        $scope.resultData.push(student);
                    }
                });
            }
            $scope.studentList = $scope.studentList.filter(function (student) {
                return !student.Selected
            })

            $scope.checkAll1();
            $scope.checkAll();
        }

        $scope.RemoveSecondTableData = function () {
            if ($scope.selectedAll1 == true) {
                angular.forEach($scope.resultData, function (student) {
                    $scope.studentList.push(student);
                });
            } else {
                angular.forEach($scope.resultData, function (student) {
                    if (student.Selected1 == true) {
                        $scope.studentList.push(student);
                    }
                });
            }
            $scope.resultData = $scope.resultData.filter(function (student) {
                return !student.Selected1;
            })
            $scope.checkAll();
            $scope.checkAll1();
        }
        $scope.test1 = function (data) {
            $scope.selectedAll1 = $scope.resultData.every(function (itm) { return itm.Selected1; })
        };

        //save new section data

        $scope.submitted = false;
        $scope.saveSectionNew = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.finaldataNS = [];
                angular.forEach($scope.resultData, function (student) {
                    if (student.Selected1 == true) {
                        $scope.finaldataNS.push(student);
                    }
                });

                if ($scope.finaldataNS.length > 0) {

                    var plg = $scope.finaldataNS;

                    var data = {
                        "SectionAllotmentType": "New",
                        "ASMAY_Id": $scope.AMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        resultData: plg
                    }
                    apiService.create("SectionAllotment/", data).then(function (promise) {
                        $scope.submitted = false;
                        if (promise.returnMsg == "" || promise.returnMsg == null) {
                            if (promise.returnVal == true) {
                                swal('Record Saved Successfully');
                            }
                        }
                        else {
                            swal(promise.returnMsg);
                        }
                        $state.reload();
                    });
                }
                else {
                    swal('Kindly add atleast one record to second grid');
                    return;
                }
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interactedCS = function (field) {
            return $scope.submittedCS || field.$dirty;
        };

        $scope.interactedDS = function (field) {
            return $scope.submittedDS || field.$dirty;
        };

        $scope.interactedPS = function (field) {
            return $scope.submittedPS || field.$dirty;
        };

        $scope.interactedYS = function (field) {
            return $scope.submittedYS || field.$dirty;
        };

        //--New Section--// Clearid
        $scope.clr = false;
        $scope.Clearid = function () {
            $scope.clr = true;
            // $state.reload();
            $scope.AMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.searchValuenew = "";
            $scope.studentList = [];
            $scope.sectionAllotedStudentList = "";
            $scope.resultData = [];
            $scope.finaldataNS = [];
            $scope.chckedIndexs = [];
            $scope.chckedIndexs1 = [];
            // $scope.LoadData();
            $scope.selectedAll1 = false;
            $scope.selectedAll = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.presentCountgridnew = "";
        }

        //-----ChangeSection....//

        $scope.OnChangeAcademicYearcs = function () {
            $scope.ASMCL_Id_CS = "";
            $scope.onClasschangeCS();
        }

        $scope.checkSection1 = function (s1, s2) {


            if (s1 == s2) {
                swal("Selected Section Is Already Assigned.Please Select Different One");
                $scope.sameSection = true;
                return;
            }
            else {
                $scope.sameSection = false;
            }
            var data = {
                "ASMAY_Id": $scope.AMAY_Id_CS,
                "ASMCL_Id": $scope.ASMCL_Id_CS,
                "ASMS_Id": $scope.ASMS_Id_CS1,
                "SectionAllotmentType": "Change"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList1 = promise.sectionAllotedStudentList;
                    $scope.presentCountgridchange = $scope.sectionAllotedStudentList1.length;
                }
                else {
                    swal("No Students Are Allotted For Selected Section");
                    $scope.sectionAllotedStudentList1 = "";
                }
            });
        };

        $scope.checkSection2 = function (s1, s2) {
            var data = {
                "ASMAY_Id": $scope.AMAY_Id_PS1,
                "ASMCL_Id": $scope.ASMCL_Id_PS1,
                "ASMS_Id": $scope.ASMS_Id_PS1,
                "SectionAllotmentType": "Promotion"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList3 = promise.sectionAllotedStudentList;
                    $scope.presentCountgridpromotion = $scope.sectionAllotedStudentList3.length;
                }
                else {
                    swal("No Students Are Allotted For Selected Section");
                    $scope.sectionAllotedStudentList3 = "";
                    $scope.presentCountgridpromotion = "";
                }
            });
        };

        $scope.checkSection3 = function (s1, s2) {
            var data = {
                "ASMAY_Id": $scope.AMAY_Id_YLS1,
                "ASMCL_Id": $scope.ASMCL_Id_YLS1,
                "ASMS_Id": $scope.ASMS_Id_YLS1,
                "SectionAllotmentType": "YearLoss"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList4 = promise.sectionAllotedStudentList;
                    $scope.presentCountgridyearloss = $scope.sectionAllotedStudentList4.length;
                }
                else {
                    swal("No Students Are Allotted For Selected Section");
                    $scope.sectionAllotedStudentList4 = "";
                    $scope.presentCountgridyearloss = "";
                }
            });
        };

        //DDYear
        $scope.GetStudentListByYearAndCLass_CS = function () {
            $scope.studentList1 = [];
            $scope.resultData_CS = [];
            $scope.selectedAll1_CS = false;
            $scope.chckedIndexs_cs = [];
            $scope.CSData = "";

            if ($scope.ASMCL_Id_CS == null || $scope.ASMCL_Id_CS == undefined || $scope.ASMCL_Id_CS == "") {
                swal("Kindly select class");
                $scope.ASMS_Id_CS = "";
                $scope.ASMS_Id_CS1 = "";
                $scope.myForm1.$setPristine();
                $scope.myForm1.$setUntouched();
                return;
            }
            var data = {
                "ASMAY_Id": $scope.AMAY_Id_CS,
                "ASMCL_Id": $scope.ASMCL_Id_CS,
                "ASMS_Id": $scope.ASMS_Id_CS,
                "SectionAllotmentType": "Change"
            }

            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList1 = promise.sectionAllotedStudentList;
                    $scope.count1 = promise.sectionAllotedStudentList.length;
                    $scope.presentCountgridchange = $scope.sectionAllotedStudentList1.length;
                }
                else {
                    $scope.count1 = 0;
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.presentCountgridchange = "";
                }
                if (promise.studentListYearCount > 0) {
                    $scope.studentList1 = promise.studentListYear;
                } else {
                    $scope.ASMS_Id_CS = "";
                    $scope.ASMS_Id_CS1 = "";
                    $scope.myForm1.$setPristine();
                    $scope.myForm1.$setUntouched();
                    swal('No Students Are Found To Change Section');
                }
            })
        };

        $scope.chckedIndexs_CS = [];
        $scope.chckedIndexs1_CS = [];
        $scope.checkAll_CS = function () {
            if ($scope.studentList1.length > 0) {
                var toggleStatus_CS = $scope.selectedAll_CS;
                angular.forEach($scope.studentList1, function (itm) {
                    itm.Selected_CS = toggleStatus_CS;
                    if ($scope.chckedIndexs_CS.indexOf(itm) === -1) {
                        $scope.chckedIndexs_CS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_CS.splice($scope.chckedIndexs_CS.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_CS = false;
            }
        };

        $scope.checkAll1_CS = function () {

            if ($scope.resultData_CS.length > 0) {
                $scope.selectedAll1_CS = true;
                var toggleStatus_CS = $scope.selectedAll1_CS;
                angular.forEach($scope.resultData_CS, function (itm) {
                    itm.Selected1_CS = toggleStatus_CS;
                    if ($scope.chckedIndexs1_CS.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_CS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_CS.splice($scope.chckedIndexs1_CS.indexOf(itm), 1);
                    }
                });

            }
            else {
                $scope.selectedAll1_CS = false;
            }
        };

        $scope.test_CS = function (data) {
            $scope.selectedAll_CS = $scope.studentList1.every(function (itm) {
                return itm.Selected_CS;
            })
        };

        $scope.GetFirstTableCS = function () {

            if ($scope.selectedAll_CS == true) {
                angular.forEach($scope.studentList1, function (student) {
                    $scope.resultData_CS.push(student);
                });
            } else {
                angular.forEach($scope.studentList1, function (student) {
                    if (student.Selected_CS == true) {
                        $scope.resultData_CS.push(student);
                    }
                });
            }
            $scope.studentList1 = $scope.studentList1.filter(function (student) {
                return !student.Selected_CS
            })

            $scope.checkAll1_CS();
            $scope.checkAll_CS();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        };
        $scope.sort2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        };
        $scope.sort3 = function (keyname) {
            $scope.sortKey3 = keyname;   //set the sortKey to the param passed
            $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
        };
        $scope.sort4 = function (keyname) {
            $scope.sortKey4 = keyname;   //set the sortKey to the param passed
            $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
        };
        $scope.sort5 = function (keyname) {
            $scope.sortKey5 = keyname;   //set the sortKey to the param passed
            $scope.reverse5 = !$scope.reverse5; //if true make it false and vice versa
        };
        $scope.sort6 = function (keyname) {
            $scope.sortKey6 = keyname;   //set the sortKey to the param passed
            $scope.reverse6 = !$scope.reverse6; //if true make it false and vice versa
        };
        $scope.sort7 = function (keyname) {
            $scope.sortKey7 = keyname;   //set the sortKey to the param passed
            $scope.reverse7 = !$scope.reverse7; //if true make it false and vice versa
        };
        $scope.sort8 = function (keyname) {
            $scope.sortKey8 = keyname;   //set the sortKey to the param passed
            $scope.reverse8 = !$scope.reverse8; //if true make it false and vice versa
        };
        $scope.sort9 = function (keyname) {
            $scope.sortKey9 = keyname;   //set the sortKey to the param passed
            $scope.reverse9 = !$scope.reverse9; //if true make it false and vice versa
        };

        $scope.RemoveSecondTableCS = function () {
            if ($scope.selectedAll1_CS == true) {
                angular.forEach($scope.resultData_CS, function (student) {
                    $scope.studentList1.push(student);
                });
            } else {
                angular.forEach($scope.resultData_CS, function (student) {
                    if (student.Selected1_CS == true) {
                        $scope.studentList1.push(student);
                    }
                });
            }
            $scope.resultData_CS = $scope.resultData_CS.filter(function (student) {
                return !student.Selected1_CS;
            })
            $scope.checkAll_CS();
            $scope.checkAll1_CS();
        };

        $scope.test1_CS = function (data) {
            $scope.selectedAll1_CS = $scope.resultData_CS.every(function (itm) { return itm.Selected1_CS; })
        };


        //saveSection_CS

        $scope.submittedCS = false;
        $scope.saveSection_CS = function () {

            $scope.submittedCS = true;
            if ($scope.sameSection == true) {
                swal("Selected Section Is Already Assigned.Please Select Different One");
            }
            else {
                if ($scope.myForm1.$valid) {

                    $scope.finaldataCS = [];
                    angular.forEach($scope.resultData_CS, function (student) {
                        if (student.Selected1_CS == true) {
                            $scope.finaldataCS.push(student);
                        }
                    });

                    if ($scope.finaldataCS.length > 0) {
                        var plg = $scope.finaldataCS;
                        var data = {
                            "ASMAY_Id": $scope.AMAY_Id_CS,
                            "ASMCL_Id": $scope.ASMCL_Id_CS,
                            "ASMS_Id": $scope.ASMS_Id_CS1,
                            "SectionAllotmentType": "Change",
                            resultData: plg
                        }
                        apiService.create("SectionAllotment/", data).then(function (promise) {
                            if (promise.returnVal == true) {
                                if (promise.returnMsg != null && promise.returnMsg != "") {
                                    swal('Record Saved Successfully', promise.returnMsg);
                                }
                                else {
                                    swal('Record Saved Successfully');
                                }
                            }
                            else {
                                swal('Failed To Saved Record');
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal('Kindly add atleast one record to second grid');
                        return;
                    }
                }
            }
        };

        //--ChangeSection Clear--//
        $scope.clrcs = false;
        $scope.Clearid_CS = function () {
            // $state.reload();
            $scope.clrcs = true;
            $scope.ASMCL_Id_CS = "";
            $scope.ASMS_Id_CS1 = "";
            $scope.AMAY_Id_CS = "";
            $scope.ASMS_Id_CS = "";
            $scope.searchValuechange = "";
            $scope.selectedAll_CS = false;
            $scope.selectedAll1_CS = false;
            $scope.studentList1 = [];
            $scope.sectionAllotedStudentList1 = "";
            $scope.resultData_CS = [];
            $scope.finaldataCS = [];
            $scope.chckedIndexs_CS = [];
            $scope.chckedIndexs1_CS = [];
            // $scope.LoadData();
            $scope.submittedCS = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.presentCountgridchange = "";
        };

        $scope.onClasschangeCS = function () {

            $scope.ASMS_Id_CS = "";
            $scope.ASMS_Id_CS1 = "";
            $scope.selectedAll_CS = false;
            $scope.selectedAll1_CS = false;
            $scope.chckedIndexs1_CS = [];
            $scope.studentList1 = [];
            $scope.resultData_CS = [];
            $scope.chckedIndexs_CS = [];
            $scope.finaldataCS = [];
            $scope.submittedCS = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };

        //--Delete Section--//
        $scope.GetStudentListByYear_DS = function () {
            $scope.chckedIndexs_ds = [];
            $scope.finaldataDS = [];
            $scope.studentList2 = [];
            $scope.selectedAll_ds = false;
            if ($scope.ASMCL_Id_DS == null || $scope.ASMCL_Id_DS == undefined || $scope.ASMCL_Id_DS == "") {
                swal("Kindly select class");
                $scope.ASMS_Id_DS = "";
                $scope.myForm2.$setPristine();
                $scope.myForm2.$setUntouched();
                return;
            }

            var data = {
                "ASMAY_Id": $scope.AMAY_Id_DS,
                "ASMCL_Id": $scope.ASMCL_Id_DS,
                "ASMS_Id": $scope.ASMS_Id_DS,
                "SectionAllotmentType": "Delete"
            }

            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList2 = promise.sectionAllotedStudentList;
                    $scope.count2 = promise.sectionAllotedStudentList.length;
                    $scope.presentCountgriddelete = $scope.sectionAllotedStudentList2.length;
                }
                else {
                    $scope.count2 = 0;
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.presentCountgriddelete = "";
                }
                if (promise.studentListYearCount > 0) {
                    $scope.studentList2 = promise.studentListYear;
                }
                else {
                    $scope.ASMS_Id_DS = "";
                    $scope.myForm2.$setPristine();
                    $scope.myForm2.$setUntouched();
                    swal('No Students Are Found To Delete Section');
                }
            });
        };

        $scope.OnChangeAcademicYeards = function () {
            $scope.ASMCL_Id_DS = "";
            $scope.onClasschangeDS();
        }

        $scope.Clearid_ds = function () {
            //  $state.reload();
            $scope.AMAY_Id_DS = "";
            $scope.ASMCL_Id_DS = "";
            $scope.ASMS_Id_DS = "";
            $scope.searchValuedelete = "";
            $scope.chckedIndexs_ds = [];
            $scope.finaldataDS = [];
            $scope.studentList2 = [];
            $scope.sectionAllotedStudentList2 = "";
            // $scope.LoadData();
            $scope.submittedDS = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.presentCountgriddelete = "";
        };

        $scope.onClasschangeDS = function () {
            $scope.ASMS_Id_DS = "";
            $scope.chckedIndexs_ds = [];
            $scope.finaldataDS = [];
            $scope.studentList2 = [];
            $scope.selectedAll_ds = false;
            $scope.submittedDS = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };


        $scope.chckedIndexs_ds = [];

        $scope.checkAll_ds = function () {
            var toggleStatus_ds = $scope.selectedAll_ds;
            angular.forEach($scope.studentList2, function (student) {
                student.Selected = toggleStatus_ds;

                if ($scope.chckedIndexs_ds.indexOf(student) === -1) {
                    $scope.chckedIndexs_ds.push(student);
                }
            });
        };

        $scope.test_ds = function (data) {
            $scope.selectedAll_ds = $scope.studentList2.every(function (itm) {
                return itm.Selected;
            })
        };

        $scope.submittedDS = false;
        $scope.DeleteSection_DS = function () {
            $scope.submittedDS = true;
            if ($scope.myForm2.$valid) {
                $scope.finaldataDS = [];
                angular.forEach($scope.studentList2, function (student) {
                    if (student.Selected == true) {
                        $scope.finaldataDS.push(student);
                    }
                });

                if ($scope.finaldataDS.length > 0) {
                    var plg = $scope.finaldataDS;
                    var data = {
                        "ASMAY_Id": $scope.AMAY_Id_DS,
                        "ASMCL_Id": $scope.ASMCL_Id_DS,
                        "ASMS_Id": $scope.ASMS_Id_DS,
                        "SectionAllotmentType": "Delete",
                        resultData: plg
                    }
                    apiService.create("SectionAllotment/", data).then(function (promise) {

                        if (promise.returnVal == true) {
                            if (promise.returnMsg != null) {
                                //  alert(promise.returnMsg);
                                swal('Selected Record Deleted Successfully', promise.returnMsg);
                            }
                            else {
                                swal('Selected Record Deleted Successfully');
                            }

                            $state.reload();
                        }
                        else if (promise.returnVal == false) {
                            if (promise.returnMsg != null) {
                                //  alert(promise.returnMsg);
                                swal(promise.returnMsg);
                            }
                            else {
                                swal('Failed To Delete Section');
                            }
                        }
                        $state.reload();
                    });
                }
                else {
                    swal('Kindly select atleast one record');
                    $state.reload();
                }
            }
        };

        //---Promotion Section---//



        $scope.GetStudentListByYear_Promotion1 = function () {

            $scope.ASMCL_Id_PS1 = "";
            $scope.AMAY_Id_PS1 = "";
            $scope.ASMS_Id_PS1 = "";
            $scope.selectedAll_PS = false;
            $scope.selectedAll1_PS = false;
            $scope.chckedIndexs1_PS = [];
            $scope.studentList4 = [];
            $scope.resultData_PS = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];
            //  $scope.yearList1 = [];
            if ($scope.NoOfYears == null || $scope.NoOfYears == undefined || $scope.NoOfYears == "") {
                swal("Kindly Enter No. Of Years To Be Promoted");
                $scope.ASMS_Id_PS = "";
                $scope.myForm4.$setPristine();
                $scope.myForm4.$setUntouched();
                return;
            }
            if ($scope.ASMCL_Id_PS == null || $scope.ASMCL_Id_PS == undefined || $scope.ASMCL_Id_PS == "") {
                swal("Kindly select class");
                $scope.ASMS_Id_PS = "";
                $scope.myForm4.$setPristine();
                $scope.myForm4.$setUntouched();
                return;
            }


            var data = {
                "ASMAY_Id": $scope.AMAY_Id_PS,
                "ASMCL_Id": $scope.ASMCL_Id_PS,
                "ASMS_Id": $scope.ASMS_Id_PS,
                "NoOfYears": $scope.NoOfYears,
                "SectionAllotmentType": "Promotion"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList3 = promise.sectionAllotedStudentList;
                    $scope.count3 = promise.sectionAllotedStudentList.length;
                    $scope.presentCountgridpromotion = $scope.sectionAllotedStudentList3.length;
                }
                else {
                    $scope.count3 = 0;
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.presentCountgridpromotion = "";
                }
                if (promise.yearList.length > 0) {
                    if (promise.asmcL_Id > 0) {
                        if (promise.studentListYearCount > 0) {

                            $scope.studentList4 = promise.studentListYear;
                            if ($scope.studentList4.length > 0) {
                                $scope.studentList4 = promise.studentListYear;
                            } else {
                                swal('No Students Are Found To Promote Section');
                            }

                            $scope.ASMCL_Id_PS1 = promise.asmcL_Id;
                            //$scope.yearList1ps = promise.yearList;
                            $scope.AMAY_Id_PS1 = promise.yearList[0].asmaY_Id;

                        } else {
                            swal('No Students Are Found To Promote Section');
                        }
                    }
                    else {
                        swal("Generated Class not present in database");
                    }
                }
                else {
                    swal('Generated Academic Year not present in database');
                }
            });
        };

        $scope.OnChangeAcademicYearps = function () {
            $scope.NoOfYears = "";
            $scope.OnChangeNoOfYears();
        };

        $scope.OnChangeNoOfYears = function () {
            $scope.ASMCL_Id_PS = "";
            $scope.AMAY_Id_PS1 = "";
            $scope.onClasschangePS();
        };

        $scope.onClasschangePS = function () {
            $scope.ASMS_Id_PS = "";
            $scope.selectedAll_PS = false;
            $scope.selectedAll1_PS = false;
            $scope.chckedIndexs1_PS = [];
            $scope.studentList4 = [];
            $scope.resultData_PS = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];
            $scope.ASMCL_Id_PS1 = "";
            $scope.submittedPS = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();

            if ($scope.NoOfYears == null || $scope.NoOfYears == undefined || $scope.NoOfYears == "") {
                swal("Kindly Enter No. Of Years To Be Promoted");
                return;
            }
        };

        $scope.chckedIndexs_PS = [];
        $scope.chckedIndexs1_PS = [];
        $scope.checkAll_PS = function () {
            if ($scope.studentList4.length > 0) {
                var toggleStatus_PS = $scope.selectedAll_PS;
                angular.forEach($scope.studentList4, function (itm) {
                    itm.Selected_PS = toggleStatus_PS;
                    if ($scope.chckedIndexs_PS.indexOf(itm) === -1) {
                        $scope.chckedIndexs_PS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_PS.splice($scope.chckedIndexs_PS.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_PS = false;
            }
        };

        $scope.checkAll1_PS = function () {

            if ($scope.resultData_PS.length > 0) {
                $scope.selectedAll1_PS = true;
                var toggleStatus_PS = $scope.selectedAll1_PS;
                angular.forEach($scope.resultData_PS, function (itm) {
                    itm.Selected1_PS = toggleStatus_PS;
                    if ($scope.chckedIndexs1_PS.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_PS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_PS.splice($scope.chckedIndexs1_PS.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_PS = false;
            }
        };

        $scope.test_PS = function (data) {
            $scope.selectedAll_PS = $scope.studentList4.every(function (itm) {
                return itm.Selected_PS;
            })
        };

        $scope.GetFirstTablePS = function () {
            $scope.classname = "";
            $scope.sectionname = "";

            if ($scope.ASMS_Id_PS1 > 0) {
                if ($scope.selectedAll_PS == true) {
                    angular.forEach($scope.studentList4, function (student) {
                        $scope.resultData_PS.push(student);
                    });
                }
                else {
                    angular.forEach($scope.studentList4, function (student) {
                        if (student.Selected_PS == true) {
                            $scope.resultData_PS.push(student);
                        }
                    });
                }
                angular.forEach($scope.classListps, function (student) {
                    if ($scope.ASMCL_Id_PS1 == student.asmcL_Id ) {
                        $scope.classname = student.asmcL_ClassName;

                    }
                });
                angular.forEach($scope.sectionList, function (student) {
                    if ($scope.ASMS_Id_PS1 == student.asmS_Id) {
                        $scope.sectionname = student.asmC_SectionName;

                    }
                });
            }
            else {
                swal("Please select section")
            }
           
            $scope.studentList4 = $scope.studentList4.filter(function (student) {
                return !student.Selected_PS
            })

            $scope.checkAll1_PS();
            $scope.checkAll_PS();
        };

        $scope.RemoveSecondTablePS = function () {
            if ($scope.selectedAll1_PS == true) {
                angular.forEach($scope.resultData_PS, function (student) {
                    $scope.studentList4.push(student);
                });
            } else {
                angular.forEach($scope.resultData_PS, function (student) {
                    if (student.Selected1_PS == true) {
                        $scope.studentList4.push(student);
                    }
                });
            }
            $scope.resultData_PS = $scope.resultData_PS.filter(function (student) {
                return !student.Selected1_PS;
            })
            $scope.checkAll_PS();
            $scope.checkAll1_PS();
        };

        $scope.test1_PS = function (data) {
            $scope.selectedAll1_PS = $scope.resultData_PS.every(function (itm) { return itm.Selected1_PS; })
        };

        $scope.SaveClassFeeChange = function () {
            if ($scope.myForm7.$valid) {

                swal({
                    title: "Are You Sure?",
                    text: "You Want To Abjust the Amount for Selected Class For This Student",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Change It!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id_CLS,
                                "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
                                "ASMCL_Id_CLS_New": $scope.ASMCL_Id_CLS_New,
                                "ASMS_Id": $scope.ASMS_Id_CLS,
                                "AMST_Id": $scope.AMST_Id.amsT_Id,
                                "Remarks": $scope.Remarks,
                                "ASYST_Id": $scope.ASYST_Id
                            };

                            apiService.create("SectionAllotment/SaveClassFeeChange", data).then(function (promise) {

                                if (promise !== null) {
                                    if (promise.returnVal === true) {
                                        swal("Fee Amount Adjusted Successfully");
                                    } else {
                                        swal("Failed To Updated Records");
                                    }

                                }
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            } else {
                $scope.submittedcls = true;
            }
        };
        //SavePromotion
        $scope.submittedPS = false;
        $scope.SavePromotion = function () {
            $scope.submittedPS = true;

            if ($scope.myForm4.$valid) {

                $scope.finaldataPS = [];
                angular.forEach($scope.resultData_PS, function (student) {
                    if (student.Selected1_PS == true) {
                        $scope.finaldataPS.push(student);
                    }
                });
                if ($scope.finaldataPS.length > 0) {


                    $scope.finaldataPS = [];
                    angular.forEach($scope.resultData_PS, function (student) {
                        if (student.Selected1_PS == true) {
                            $scope.finaldataPS.push(student);
                        }
                    });
                    if ($scope.finaldataPS.length > 0) {

                        var plg = $scope.finaldataPS;
                        var data = {
                            "ASMAY_Id": $scope.AMAY_Id_PS1,
                            "ASMCL_Id": $scope.ASMCL_Id_PS1,
                            "ASMS_Id": $scope.ASMS_Id_PS1,
                            "SectionAllotmentType": "Promotion",
                            resultData: plg
                        }
                        apiService.create("SectionAllotment/", data).then(function (promise) {
                            if (promise.returnMsg == "" || promise.returnMsg == null) {
                                if (promise.returnVal == true) {
                                    swal('Selected Record Promoted Successfully');
                                }
                            }
                            else {
                                swal(promise.returnMsg);
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal('Kindly add atleast one record to second grid');
                        return;
                    }
                }
            }
        };

        $scope.clrps = false;
        $scope.Clearid_ps = function () {
            // $state.reload();
            $scope.clrps = true;
            $scope.ASMCL_Id_PS = "";
            $scope.ASMS_Id_PS = "";
            $scope.AMAY_Id_PS = "";
            $scope.NoOfYears = "";
            $scope.searchValuepromotion = "";
            $scope.selectedAll_PS = false;
            $scope.selectedAll1_PS = false;
            $scope.NoOfYears = "";
            $scope.chckedIndexs1_PS = [];
            $scope.studentList4 = [];
            $scope.sectionAllotedStudentList3 = "";
            $scope.resultData_PS = [];
            $scope.chckedIndexs_ps = [];
            $scope.finaldataPS = [];
            // $scope.LoadData();
            $scope.submittedPS = false;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.presentCountgridpromotion = "";

        };

        //---Year Loss Section---//

        $scope.onClasschangeYS = function () {

            $scope.ASMS_Id_YLS = "";
            $scope.AMAY_Id_YLS1 = "";
            $scope.ASMCL_Id_YLS1 = "";
            $scope.ASMS_Id_YLS1 = "";
            $scope.selectedAll_YS = false;
            $scope.selectedAll1_YS = false;
            $scope.chckedIndexs1_YS = [];
            $scope.studentList5 = [];
            $scope.resultData_YS = [];
            $scope.chckedIndexs_ys = [];
            $scope.finaldataYS = [];

            $scope.submittedYS = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();

        };

        $scope.OnChangeAcademicYearys = function () {
            $scope.ASMCL_Id_YLS = "";
            $scope.onClasschangeYS();
        };

        $scope.GetStudentListByYear_YearLoss = function () {

            $scope.AMAY_Id_YLS1 = "";
            $scope.ASMCL_Id_YLS1 = "";
            $scope.ASMS_Id_YLS1 = "";

            $scope.selectedAll_YS = false;
            $scope.selectedAll1_YS = false;
            $scope.chckedIndexs1_YS = [];
            $scope.studentList5 = [];
            $scope.resultData_YS = [];
            $scope.chckedIndexs_ys = [];
            $scope.finaldataYS = [];

            if ($scope.ASMCL_Id_YLS == null || $scope.ASMCL_Id_YLS == undefined || $scope.ASMCL_Id_YLS == "") {
                swal("Kindly select class");
                $scope.ASMS_Id_YLS = "";
                $scope.myForm5.$setPristine();
                $scope.myForm5.$setUntouched();
                return;
            }

            var data = {
                "ASMAY_Id": $scope.AMAY_Id_YLS,
                "ASMCL_Id": $scope.ASMCL_Id_YLS,
                "ASMS_Id": $scope.ASMS_Id_YLS,
                "NoOfYears": 1,
                "SectionAllotmentType": "YearLoss"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList4 = promise.sectionAllotedStudentList;
                    $scope.count4 = promise.sectionAllotedStudentList.length;
                    $scope.presentCountgridyearloss = $scope.sectionAllotedStudentList4.length;
                }
                else {
                    $scope.count4 = 0;
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.presentCountgridyearloss = "";
                }
                if (promise.yearList.length > 0) {

                    if (promise.studentListYearCount > 0) {
                        // $scope.yearList2ys = promise.yearList;
                        $scope.AMAY_Id_YLS1 = promise.yearList[0].asmaY_Id;
                        $scope.ASMCL_Id_YLS1 = $scope.ASMCL_Id_YLS;
                        $scope.ASMS_Id_YLS1 = $scope.ASMS_Id_YLS;
                        $scope.studentList5 = promise.studentListYear;

                    } else {
                        swal('No Students Are Found To Make YearLoss');
                    }
                }
                else {
                    swal('Generated Academic Year not present in database');
                }
            })
        };
        $scope.chckedIndexs_YS = [];
        $scope.chckedIndexs1_YS = [];
        $scope.checkAll_YS = function () {
            if ($scope.studentList5.length > 0) {
                var toggleStatus_YS = $scope.selectedAll_YS;
                angular.forEach($scope.studentList5, function (itm) {
                    itm.Selected_YS = toggleStatus_YS;
                    if ($scope.chckedIndexs_YS.indexOf(itm) === -1) {
                        $scope.chckedIndexs_YS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs_YS.splice($scope.chckedIndexs_YS.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll_YS = false;
            }
        };

        $scope.checkAll1_YS = function () {
            if ($scope.resultData_YS.length > 0) {
                $scope.selectedAll1_YS = true;
                var toggleStatus_YS = $scope.selectedAll1_YS;
                angular.forEach($scope.resultData_YS, function (itm) {
                    itm.Selected1_YS = toggleStatus_YS;
                    if ($scope.chckedIndexs1_YS.indexOf(itm) === -1) {
                        $scope.chckedIndexs1_YS.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1_YS.splice($scope.chckedIndexs1_YS.indexOf(itm), 1);
                    }
                });
            }
            else {
                $scope.selectedAll1_YS = false;
            }
        };
        $scope.test_YS = function (data) {
            $scope.selectedAll_YS = $scope.studentList5.every(function (itm) {
                return itm.Selected_YS;
            })
        };
        $scope.GetFirstTableYS = function () {
            if ($scope.selectedAll_YS == true) {
                angular.forEach($scope.studentList5, function (student) {
                    $scope.resultData_YS.push(student);
                });
            } else {
                angular.forEach($scope.studentList5, function (student) {
                    if (student.Selected_YS == true) {
                        $scope.resultData_YS.push(student);
                    }
                });
            }
            $scope.studentList5 = $scope.studentList5.filter(function (student) {
                return !student.Selected_YS
            })
            $scope.checkAll1_YS();
            $scope.checkAll_YS();
        };
        $scope.RemoveSecondTableYS = function () {
            if ($scope.selectedAll1_YS == true) {
                angular.forEach($scope.resultData_YS, function (student) {
                    $scope.studentList5.push(student);
                });
            } else {
                angular.forEach($scope.resultData_YS, function (student) {
                    if (student.Selected1_YS == true) {
                        $scope.studentList5.push(student);
                    }
                });
            }
            $scope.resultData_YS = $scope.resultData_YS.filter(function (student) {
                return !student.Selected1_YS;
            })
            $scope.checkAll_YS();
            $scope.checkAll1_YS();
        };
        $scope.test1_YS = function (data) {
            $scope.selectedAll1_YS = $scope.resultData_YS.every(function (itm) { return itm.Selected1_YS; })
        };
        //SaveYearLossSection
        $scope.submittedYS = false;
        $scope.SaveYearLossSection = function () {
            $scope.submittedYS = true;
            if ($scope.myForm5.$valid) {
                $scope.finaldataYS = [];
                angular.forEach($scope.resultData_YS, function (student) {
                    if (student.Selected1_YS == true) {
                        $scope.finaldataYS.push(student);
                    }
                });
                if ($scope.finaldataYS.length > 0) {
                    var plg = $scope.finaldataYS;
                    var data = {
                        "ASMAY_Id": $scope.AMAY_Id_YLS1,
                        "ASMCL_Id": $scope.ASMCL_Id_YLS1,
                        "ASMS_Id": $scope.ASMS_Id_YLS1,
                        "SectionAllotmentType": "YearLoss",
                        resultData: plg,
                    }
                    apiService.create("SectionAllotment/", data).then(function (promise) {
                        //if (promise.returnMsg == "" || promise.returnMsg == null) {
                        //    if (promise.returnVal == true) {
                        //        swal('Selected Record Year Loss Updated Successfully');
                        //    }
                        //}
                        //else {
                        //    swal(promise.returnMsg);
                        //}
                        //$state.reload();

                        if (promise.returnMsg == "" || promise.returnMsg == null) {
                            if (promise.returnVal == true) {
                                //  alert(promise.returnMsg);
                                swal('Selected Record Year Loss Updated Successfully', promise.returnMsg);
                            }
                            else if (promise.returnVal == false) {
                                swal('Selected Record Year Loss Updated Successfully');
                            }
                            else {
                                swal('Failed To Year Loss Section');
                            }

                            $state.reload();
                        }
                        else {
                            swal(promise.returnMsg);
                        }
                    });
                }
                else {
                    swal('Kindly add atleast one record to second grid');
                    return;
                }
            };
        };
        //$scope.SaveYearLossSection = function () {

        //    $scope.submittedYS = true;
        //    if ($scope.myForm5.$valid) {

        //        $scope.finaldataYS = [];
        //        angular.forEach($scope.resultData_YS, function (student) {
        //            if (student.Selected1_YS == true) {
        //                $scope.finaldataYS.push(student);
        //            }
        //        });

        //        if ($scope.finaldataYS.length > 0) {
        //            var plg = $scope.finaldataYS;
        //            var data = {

        //                "ASMAY_Id": $scope.AMAY_Id_YLS1,
        //                "ASMCL_Id": $scope.ASMCL_Id_YLS1,
        //                "ASMS_Id": $scope.ASMS_Id_YLS1,
        //                "SectionAllotmentType": "YearLoss",
        //                resultData: plg,
        //            }
        //            apiService.create("SectionAllotment/", data).then(function (promise) {
        //                if (promise.returnMsg == "" || promise.returnMsg == null) {
        //                    if (promise.returnVal == true) {
        //                        swal('Selected Record Year Loss Updated Successfully');
        //                    }
        //                }
        //                else {
        //                    swal(promise.returnMsg);
        //                }
        //                $state.reload();
        //            });
        //        }
        //        else {
        //            swal('Kindly add atleast one record to second grid');
        //            return;
        //        }
        //    };
        //};

        $scope.clryls = false;
        $scope.Clearid_yls = function () {
            //$state.reload();
            $scope.clryls = true;
            $scope.AMAY_Id_YLS = "";
            $scope.AMAY_Id_YLS1 = "";
            $scope.ASMCL_Id_YLS1 = "";
            $scope.ASMCL_Id_YLS = "";
            $scope.ASMS_Id_YLS1 = "";
            $scope.ASMS_Id_YLS = "";
            $scope.searchValueyearloss = "";
            $scope.selectedAll_YS = false;
            $scope.selectedAll1_YS = false;
            $scope.chckedIndexs_YS = [];
            $scope.chckedIndexs1_YS = [];
            $scope.studentList5 = [];
            $scope.sectionAllotedStudentList4 = "";
            $scope.resultData_YS = [];
            $scope.chckedIndexs_ys = [];
            $scope.finaldataYS = [];
            $scope.submittedYS = false;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
            $scope.presentCountgridyearloss = "";

            // $scope.LoadData();
        };

        $scope.onSectionChange = function () {
            var data = {
                "ASMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "SectionAllotmentType": "New"
            }
            apiService.create("SectionAllotment/GetStudentListByYearAndCLass", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sectionAllotedStudentList = promise.sectionAllotedStudentList;
                    $scope.presentCountgridnew = $scope.sectionAllotedStudentList.length;
                }
                else {
                    swal("No Students Are Allotted For Selected Section");
                    $scope.sectionAllotedStudentList = "";
                }
            });
        };

        //Search sort functionality

        $scope.currentPageRightNS = 1;
        $scope.itemsPerPageRightNS = 5;
        $scope.paginateRightNS = "paginateRightNS";
        $scope.reverseRightNS = true;

        $scope.orderRightNS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseRightNS = !$scope.reverseRightNS; //if true make it false and vice versa
        };

        $scope.searchRightNS = "";
        $scope.filterRightNS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchRightNS)) >= 0;
        };

        //
        //new section Left
        $scope.currentPageLeftNS = 1;
        $scope.itemsPerPageLeftNS = 5;
        $scope.paginateLeftNS = "paginateLeftNS";
        $scope.reverseLeftNS = true;
        $scope.orderLeftNS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseLeftNS = !$scope.reverseLeftNS; //if true make it false and vice versa
        };

        $scope.searchLeftNS = "";
        $scope.filterLeftNS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchLeftNS)) >= 0;
        };

        //Change section
        //Change section right
        $scope.currentPageRightCS = 1;
        $scope.itemsPerPageRightCS = 5;
        $scope.paginateRightCS = "paginateRightCS";
        $scope.reverseRightCS = true;

        $scope.orderRightCS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseRightCS = !$scope.reverseRightCS; //if true make it false and vice versa
        };

        $scope.searchRightCS = "";
        $scope.filterRightCS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchRightCS)) >= 0;
        };

        //
        //Change section Left
        $scope.currentPageLeftCS = 1;
        $scope.itemsPerPageLeftCS = 5;
        $scope.paginateLeftCS = "paginateLeftCS";
        $scope.reverseLeftCS = true;
        $scope.orderLeftCS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseLeftCS = !$scope.reverseLeftCS; //if true make it false and vice versa
        };

        $scope.searchLeftCS = "";
        $scope.filterLeftCS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchLeftCS)) >= 0;
        };

        //Delete section

        //Delete section
        $scope.currentPageRightDS = 1;
        $scope.itemsPerPageRightDS = 5;
        $scope.paginateRightDS = "paginateRightDS";
        $scope.reverseRightDS = true;

        $scope.orderRightDS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseRightDS = !$scope.reverseRightDS; //if true make it false and vice versa
        };

        $scope.searchRightDS = "";
        $scope.filterRightDS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchRightDS)) >= 0;
        };


        //Promostion section
        //Promostion section right
        $scope.currentPageRightPS = 1;
        $scope.itemsPerPageRightPS = 5;
        $scope.paginateRightPS = "paginateRightPS";
        $scope.reverseRightPS = true;

        $scope.orderRightPS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseRightPS = !$scope.reverseRightPS; //if true make it false and vice versa
        };

        $scope.searchRightPS = "";
        $scope.filterRightPS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchRightPS)) >= 0;
        };

        //
        //Promostion section Left
        $scope.currentPageLeftPS = 1;
        $scope.itemsPerPageLeftPS = 5;
        $scope.paginateLeftPS = "paginateLeftPS";
        $scope.reverseLeftPS = true;
        $scope.orderLeftPS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseLeftPS = !$scope.reverseLeftPS; //if true make it false and vice versa
        };

        $scope.searchLeftPS = "";
        $scope.filterLeftPS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchLeftPS)) >= 0;
        };



        //Year Lose section
        //Year Lose section right
        $scope.currentPageRightYS = 1;
        $scope.itemsPerPageRightYS = 5;
        $scope.paginateRightYS = "paginateRightYS";
        $scope.reverseRightYS = true;

        $scope.orderRightYS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseRightYS = !$scope.reverseRightYS; //if true make it false and vice versa
        };

        $scope.searchRightYS = "";
        $scope.filterRightYS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchRightYS)) >= 0;
        };

        //
        //Year section Left
        $scope.currentPageLeftYS = 1;
        $scope.itemsPerPageLeftYS = 5;
        $scope.paginateLeftYS = "paginateLeftYS";
        $scope.reverseLeftYS = true;
        $scope.orderLeftYS = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverseLeftYS = !$scope.reverseLeftYS; //if true make it false and vice versa
        };

        $scope.searchLeftYS = "";
        $scope.filterLeftYS = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchLeftYS)) >= 0;
        };

        $scope.searchValuechange = '';
        $scope.changeSectionFilter = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.searchValuechange)) >= 0;
        };

        $scope.searchValuedelete = '';
        $scope.deleteFilter = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.searchValuedelete)) >= 0;
        };
        $scope.searchValuepromotion = '';
        $scope.promotionFilter = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.searchValuepromotion)) >= 0;
        };
        $scope.searchValueyearloss = '';
        $scope.yearlossFilter = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.searchValueyearloss)) >= 0;
        };

        //update roll number    

        $scope.interactedURN = function (field) {
            return $scope.submittedURN;
        };


        $scope.clrurn = false;
        $scope.Clearid_URN = function () {
            $scope.search100 = "";
            $scope.clrurn = true;
            $scope.ASMCL_Id_URN = "";
            $scope.ASMS_Id_URN = "";
            $scope.AMAY_Id_URN = "";
            $scope.studentList6 = "";
            $scope.studentList7 = "";
            $scope.presentCountgridupdate = "";
        };

        $scope.OnChangeAcademicYearys = function () {
            $scope.ASMCL_Id_URN = "";
            $scope.onClasschangeYS();
        };
        $scope.onClasschangeurn = function () {
            $scope.ASMS_Id_URN = "";
        };

        $scope.sort100 = function (key) {
            $scope.reverse100 = ($scope.sortKey100 == key) ? !$scope.reverse100 : $scope.reverse100;
            $scope.sortKey100 = key;
        };

        $scope.search100 = '';
        $scope.updateno = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.search100)) >= 0;
        };

        $scope.search1001 = '';
        $scope.updateno = function (list) {
            return (angular.lowercase(list.amsT_FirstName + list.amsT_MiddleName + list.amsT_LastName)).indexOf(angular.lowercase($scope.search1001)) >= 0;
        };

        $scope.updateroll = false;
        $scope.updateroll1 = false;
        $scope.GetStudentListByYear_URN = function () {
            if ($scope.ASMCL_Id_URN == null || $scope.AMAY_Id_URN == "" || $scope.ASMS_Id_URN == "") {
                swal("Kindly select class");
                return;
            }
            var data = {
                "ASMAY_Id": $scope.AMAY_Id_URN,
                "ASMCL_Id": $scope.ASMCL_Id_URN,
                "ASMS_Id": $scope.ASMS_Id_URN,
            }
            apiService.create("SectionAllotment/GetStudentListByURN", data).then(function (promise) {
                if (promise.count > 0) {
                    $scope.studentList6 = promise.studentlisturn;
                    $scope.studentList7 = promise.studentlisturn1;
                    $scope.presentCountgridupdate = $scope.studentList7.length;
                    $scope.updateroll = true;
                    $scope.updateroll1 = true;
                }
                else {
                    swal("No Students Are Allotted For Selected Class/Section");
                    $scope.studentList6 = "";
                    $scope.studentList7 = "";
                }
            });
        };

        $scope.submittedURN = false;

        $scope.updatesave = function () {
            $scope.submittedURN = true;
            if ($scope.myForm6.$valid) {
                var data = {
                    studentlisturn: $scope.studentList6,
                    "SectionAllotmentType": "updateroll",
                    "ASMAY_Id": $scope.AMAY_Id_URN,
                    "ASMCL_Id": $scope.ASMCL_Id_URN,
                    "ASMS_Id": $scope.ASMS_Id_URN,
                };
                apiService.create("SectionAllotment/GetStudentListByURNsave", data).then(function (promise) {
                    if (promise.returnal == true) {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else {
                        swal("Failed To Update");
                        $state.reload();
                    }
                });
            }
        };

        $scope.addtopdays = function (user, index) {
            for (var k = 0; k < $scope.studentList6.length; k++) {
                var roll = parseInt(user.pdays);
                var arryind = $scope.studentList6.indexOf($scope.studentList6[k]);
                console.log(arryind);
                if (arryind != index) {
                    if ($scope.studentList6[k].pdays == roll) {

                        swal("Already Exist");
                        $scope.studentList6[index].pdays = "";
                        //angular.forEach($scope.studentList6, function (obj) {
                        //    if (roll == obj.pdays) {
                        //        obj.pdays = "";
                        //    }
                        //})
                        $scope.SaveDis = true;
                        break;
                    }
                    else {
                        $scope.SaveDis = false;
                    }
                }
            }
        };


        // Change Class Details

        $scope.GetChangeClassDetails = function () {
            $scope.AMAY_Id_CLS = "";
            $scope.ASMCL_Id_CLS_Old = "";
            $scope.ASMS_Id_CLS = "";
            $scope.AMST_Id = "";
            $scope.show = false;
            $scope.ASMCL_Id_CLS_New = "";
            $scope.stdname = [];
            $scope.classListCLS = [];
            $scope.yearListCLS = [];
            $scope.sectionListCLS = [];
            $scope.NewclassList = [];
            $scope.stdname = [];

            var pageid = 2;
            apiService.getURI("SectionAllotment/GetChangeClassDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.classListCLS = promise.classList;
                    $scope.sectionListCLS = promise.sectionList;
                    $scope.yearListCLS = promise.yearList;
                    $scope.ASMAY_Id_CLS = promise.asmaY_Id;
                }
            });
        };

        $scope.onClasschangeCLS = function () {
            $scope.ASMS_Id_CLS = "";
            $scope.AMST_Id = "";
            $scope.ASMCL_Id_CLS_New = "";
            $scope.stdname = [];
            $scope.show = false;
        };

        $scope.GetStudentListByYearCLS = function () {
            $scope.AMST_Id = "";
            $scope.ASMCL_Id_CLS_New = "";
            $scope.stdname = [];
            $scope.show = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id_CLS,
                "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
                "ASMS_Id": $scope.ASMS_Id_CLS
            };

            apiService.create("SectionAllotment/GetStudentListByYearCLS", data).then(function (promise) {

                if (promise !== null) {
                    $scope.NewclassList = promise.newclasslist;
                    $scope.stdname = promise.sectionAllotedStudentList;
                }
            });
        };

        $scope.OnChangeNewClassCLS = function () {
            $scope.Remarks = "";
            $scope.FeeCategoryDetails = [];
            $scope.show = false;
            $scope.AMST_Id = "";
        };

        $scope.onstudentnamechange = function () {
              
            $scope.Remarks = "";
            $scope.studentname = "";
            $scope.AMST_AdmNo = "";
            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.AMAY_RollNo = "";
            $scope.FeeCategoryDetails = [];
            $scope.show = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id_CLS,
                "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
                "ASMCL_Id_CLS_New": $scope.ASMCL_Id_CLS_New,
                "ASMS_Id": $scope.ASMS_Id_CLS,
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };

            apiService.create("SectionAllotment/onstudentnamechange", data).then(function (promise) {
                if (promise !== null) {
                    $scope.studentdetails = promise.studentdetails;
                    $scope.Attendancecount = promise.attendancecount;
                    $scope.Examcount = promise.examcount;
                    $scope.Feecount = promise.feecount;
                    
                    $scope.studentname = $scope.studentdetails[0].studentName;
                    $scope.AMST_AdmNo = $scope.studentdetails[0].amsT_AdmNo;
                    $scope.ASMCL_ClassName = $scope.studentdetails[0].asmcL_ClassName;
                    $scope.ASMC_SectionName = $scope.studentdetails[0].asmC_SectionName;
                    $scope.AMAY_RollNo = $scope.studentdetails[0].amaY_RollNo;
                    $scope.ASYST_Id = $scope.studentdetails[0].asysT_Id;
                    $scope.FeeCategoryDetails = promise.feeCategoryDetails;
                    console.log($scope.FeeCategoryDetails);

                    $scope.studentOptions = [];

                    $scope.studentOptions = $scope.FeeCategoryDetails.filter((item, i, arr) => arr.findIndex((t) => t.fmcC_ClassCategoryName === item.fmcC_ClassCategoryName) === i);

                    console.log($scope.studentOptions);
                    $scope.Feecount = $scope.studentOptions.length;
                    $scope.show = true;

                    if (promise.attendanceDone == true && promise.examMarksDone == false) {

                        $scope.objj.AttendanceCheck = promise.attendanceDone;

                    }
                    else if (promise.examMarksDone == true && promise.attendanceDone == false) {

                    }
                    else if (promise.examMarksDone == true && promise.attendanceDone == true) {
                        $scope.objj.AttendanceCheck = promise.attendanceDone;
                    }
                }

            });
        };
        //added By Kavita 
        $scope.DeleteFeeMapping = function () {

            swal({
                title: "Are You Sure?",
                text: "You Want To Delete Fee Maping For This Student",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "ASMAY_Id": $scope.ASMAY_Id_CLS,
                            "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
                            "ASMCL_Id_CLS_New": $scope.ASMCL_Id_CLS_New,
                            "ASMS_Id": $scope.ASMS_Id_CLS,
                            "AMST_Id": $scope.AMST_Id.amsT_Id
                        };

                        apiService.create("SectionAllotment/DeleteFeeMapping", data).then(function (promise) {
                            if (promise !== null) {

                                if (promise.flag == 'true') {
                                    if (promise.returnVal === true) {
                                        swal("Fee Mapping Deleted Successfully");
                                    } else {
                                        swal("Failed To Delete Fee Mapping");
                                    }
                                }
                                else if (promise.flag == 'false') {

                                    $scope.SaveClassFeeChange();

                                }
                                else {
                                    swal("Failed To Delete Fee Mapping");
                                }
                            }
                        });
                    }
                    else {
                        swal("Record Delection Cancelled");
                    }
                });
        };
        //$scope.DeleteFeeMapping = function () {

        //    swal({
        //        title: "Are You Sure?",
        //        text: "You Want To Delete Fee Maping For This Student",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
        //        cancelButtonText: "Cancel..!",
        //        closeOnConfirm: false,
        //        closeOnCancel: true
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                var data = {
        //                    "ASMAY_Id": $scope.ASMAY_Id_CLS,
        //                    "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
        //                    "ASMCL_Id_CLS_New": $scope.ASMCL_Id_CLS_New,
        //                    "ASMS_Id": $scope.ASMS_Id_CLS,
        //                    "AMST_Id": $scope.AMST_Id.amsT_Id
        //                };

        //                apiService.create("SectionAllotment/DeleteFeeMapping", data).then(function (promise) {
        //                    if (promise !== null) {
        //                        if (promise.returnVal === true) {
        //                            swal("Fee Mapping Deleted Successfully");
        //                        } else {
        //                            swal("Failed To Delete Fee Mapping");
        //                        }
        //                    }
        //                });
        //            }
        //            else {
        //                swal("Record Delection Cancelled");
        //            }
        //        });
        //};

        $scope.SaveClassChange = function () {
            if ($scope.myForm7.$valid) {

                swal({
                    title: "Are You Sure?",
                    text: "You Want To Change Class For This Student",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Change It!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id_CLS,
                                "ASMCL_Id_CLS_Old": $scope.ASMCL_Id_CLS_Old,
                                "ASMCL_Id_CLS_New": $scope.ASMCL_Id_CLS_New,
                                "ASMS_Id": $scope.ASMS_Id_CLS,
                                "AMST_Id": $scope.AMST_Id.amsT_Id,
                                "Remarks": $scope.Remarks,
                                "ASYST_Id": $scope.ASYST_Id,
                                "attendanceDone": $scope.objj.AttendanceCheck
                            };

                            apiService.create("SectionAllotment/SaveClassChange", data).then(function (promise) {

                                if (promise !== null) {
                                    if (promise.returnVal === true || promise.returnValattendance === false) {
                                        swal("Records Updated Successfully");
                                    }
                                    else if (promise.returnVal === true && promise.returnValattendance === true) {
                                        swal("Records Updated Successfully with Attendance Update");
                                    }
                                    else {
                                        swal("Failed To Updated Records");
                                    }
                                    $scope.GetChangeClassDetails();
                                }
                            });
                        }
                        else {
                            swal("Cancelled");
                        }
                    });

            } else {
                $scope.submittedcls = true;
            }
        };

       

        $scope.interactedCLS = function (field) {
            return $scope.submittedcls;
        };

        $scope.Clearid_CLS = function () {
            $scope.GetChangeClassDetails();
        };
    }
})();