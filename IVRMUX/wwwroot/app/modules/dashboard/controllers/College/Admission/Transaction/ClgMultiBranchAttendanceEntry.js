(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgMultiBranchAttendanceEntryController', ClgMultiBranchAttendanceEntryController)
    ClgMultiBranchAttendanceEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgMultiBranchAttendanceEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.obj = {};

        $scope.searc_button = true;
        $scope.sortReverse = true;
        $scope.searchValue = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.amsT_Date = $scope.ddate;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //----------------------------------Page Load------------------------------------------------


        $scope.loaddata = function () {
            var pageid = 2;
            var dataf, datat;
            apiService.getURI("ClgAttendanceEntry/getalldetails", pageid).
                then(function (promise) {
                    $scope.getYear = promise.getYear; 
                    for (var i = 0; i < $scope.getYear.length; i++) {
                        name = $scope.getYear[i].asmaY_Id;
                        for (var j = 0; j < $scope.getYear.length; j++) {
                            if (name == $scope.getYear[j].asmaY_Id) {
                                $scope.getYear[0].Selected = true;
                                $scope.asmaY_Id = $scope.getYear[0].asmaY_Id;

                                dataf = new Date($scope.getYear[j].asmaY_From_Date);
                                datat = new Date($scope.getYear[j].asmaY_To_Date);
                                $scope.fDate = dataf;
                                $scope.tDate = datat;
                                $scope.dd = new Date();

                                $scope.minDatedof = new Date(
                                    $scope.fDate.getFullYear(),
                                    $scope.fDate.getMonth(),
                                    $scope.fDate.getDate());

                                $scope.maxDatedof = new Date(
                                    $scope.dd.getFullYear(),
                                    $scope.dd.getMonth(),
                                    $scope.dd.getDate());

                                $scope.minDatedtf = new Date(
                                    $scope.fDate.getFullYear(),
                                    $scope.fDate.getMonth(),
                                    $scope.fDate.getDate());

                                $scope.maxDatedtf = new Date(
                                    $scope.dd.getFullYear(),
                                    $scope.dd.getMonth(),
                                    $scope.dd.getDate());

                                $scope.minDatemf = new Date(
                                    $scope.fDate.getFullYear(),
                                    $scope.fDate.getMonth(),
                                    $scope.fDate.getDate());

                                $scope.maxDatemf = new Date(
                                    $scope.dd.getFullYear(),
                                    $scope.dd.getMonth(),
                                    $scope.dd.getDate());

                                $scope.minDatedf = new Date(
                                    $scope.fDate.getFullYear(),
                                    $scope.fDate.getMonth(),
                                    $scope.fDate.getDate());

                                $scope.maxDatedf = new Date(
                                    $scope.dd.getFullYear(),
                                    $scope.dd.getMonth(),
                                    $scope.dd.getDate());                             
                            }
                        }
                    }

                    
                    if (promise.message != null) {
                        swal(promise.message);
                    } else {
                        $scope.getCourse = promise.getCourse;
                        $scope.getPeriod = promise.getPeriod;
                    }

                })
        };


        //-----------Course Change
        $scope.onCoursechange = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "Multibranch": $scope.multibranch
            };

            apiService.create("ClgAttendanceEntry/getBranchdata", data).
                then(function (promise) {

                    if ($scope.multibranch == 1) {
                        $scope.getSemester = promise.getSemester;
                    } else {
                        $scope.getBranch = promise.getBranch;
                    }
                });
        };

        //-----------Branch Change
        $scope.onBranchchange = function () {
            var ambid = 0;
            if ($scope.multibranch == 1) {
                ambid = $scope.amB_Id;
            } else {
                ambid = 0;
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": ambid,
                "Multibranch": $scope.multibranch
            }
            apiService.create("ClgAttendanceEntry/getSemesterdata", data).
                then(function (promise) {
                    if ($scope.multibranch == 1) {
                        $scope.getSection = promise.getSection;
                    } else {
                        $scope.getSemester = promise.getSemester;
                    }
                });
        };

        //---- Semester Change
        $scope.onSemchange = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id,
            }
            apiService.create("ClgAttendanceEntry/getSectiondata", data).
                then(function (promise) {
                    $scope.getSection = promise.getSection;
                })
        }

        //--------Section Change
        $scope.onSectionchange = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id,
                "ACMS_Id": $scope.acmS_Id,
            }
            apiService.create("ClgAttendanceEntry/getSubjdata", data).
                then(function (promise) {
                    $scope.ismS_Id = "";
                    $scope.acaB_Id = "";
                    $scope.getSubject = promise.getSubject;

                })
        }

        //--------Subject Change
        $scope.onSubjectchange = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id,
                "ACMS_Id": $scope.acmS_Id,
                "ISMS_Id": $scope.ismS_Id,
            }
            apiService.create("ClgAttendanceEntry/getBatchdata", data).
                then(function (promise) {
                    $scope.getBatch = promise.getBatch;
                })
        }


        //----------------------- category Grid   
        $scope.get_Studentdetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var pageid = 2;
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "ACSA_Regular_Extra": $scope.period,
                    "ACSA_AttendanceDate": new Date($scope.amsT_Date).toDateString(),
                    "TTMP_Id": $scope.ttmP_Id,
                }

                apiService.create("ClgAttendanceEntry/getStudentdata", data).then(function (promise) {

                    $scope.getStudentdetails = promise.getStudentdetails;
                    if ($scope.getStudentdetails.length > 0) {

                        if (promise.check_attendance_entrytype == "A") {

                            angular.forEach($scope.getStudentdetails, function (dd) {
                                if (dd.pdays == 0) {
                                    dd.Selected = true;
                                } else {
                                    dd.Selected = false;
                                }                                
                            })

                        } else if (promise.check_attendance_entrytype == "P") {

                            angular.forEach($scope.getStudentdetails, function (dd) {
                                if (dd.pdays == 0) {
                                    dd.Selected = false;
                                } else {
                                    dd.Selected = true;
                                }
                            })

                        } else {
                            angular.forEach($scope.getStudentdetails, function (dd) {
                                dd.Selected = false;
                            })
                        }

                        

                    } else {
                        swal("No Records Found")
                    }                   
                })
            } else {
                $scope.submitted = true;
            }
        };


        $scope.toggleAll = function () {
            angular.forEach($scope.getStudentdetails, function (subj) {
                subj.xyz = $scope.all;
            })
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.getStudentdetails.every(function (itm) { return itm.xyz; });
        };


        //------------------------------Save
        $scope.saveatt = function (objas) {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "TTMP_Id": $scope.ttmP_Id,
                    "ClgAttendanceEntryTempDTO": $scope.getStudentdetails,
                    "ACSA_Regular_Extra": $scope.period,
                    "ACSA_AttendanceDate": new Date($scope.amsT_Date).toDateString(),
                }
                apiService.create("ClgAttendanceEntry/saveatt", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.message == "Add") {
                            swal("Record Saved Successfully")
                        } else {
                            swal("Record Update Successfully")
                        }
                    }
                    else if (promise.returnval == false) {
                        if (promise.message == "Add") {
                            swal("Failed To Save Record")
                        } else {
                            swal("Failed To Update Record")
                        }
                    }
                    else {
                        swal("Something Went Wrong.. Contact With Administrator");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        }


        //--------------------------Grid
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }

        //disable the sundays in calender
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;

        };

        $scope.isOptionsRequired = function () {
            return !$scope.getBranch.some(function (options) {
                return options.ambid;
            });
        }

    }
})();