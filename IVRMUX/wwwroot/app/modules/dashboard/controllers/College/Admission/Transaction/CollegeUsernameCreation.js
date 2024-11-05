(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeUsernameCreationController', CollegeUsernameCreationController)
    CollegeUsernameCreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeUsernameCreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.obj = {};

        $scope.searc_button = true;
        $scope.sortReverse = true;
        $scope.searchValue = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.amsT_Date = $scope.ddate;

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //----------------------------------Page Load------------------------------------------------


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CollegeUsernameCreation/getalldetails", pageid).
                then(function (promise) {
                    $scope.getYear = promise.yearlist;
                });
        };

        $scope.onchangetype = function (stype) {
            if (stype === 'Alumni') {
                $scope.acmS_Id = 0;
            }
            else {
                $scope.acmS_Id = '';
            }

        };

        // on year change 

        $scope.onyearchange = function () {
            $scope.students = [];
            $scope.amcO_Id = "";
            $scope.amB_Id = "";
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("CollegeUsernameCreation/onyearchange", data).then(function (promise) {

                if (promise != null) {
                    $scope.getCourse = promise.courselist;
                    if ($scope.getCourse.length == 0) {
                        swal("No Course Details For This Year");
                    }
                } else {
                    swal("No Records Found");
                }

            });
        };


        //-----------Course Change
        $scope.onCoursechange = function () {

            $scope.students = [];
            $scope.amB_Id = "";
            $scope.amsE_Id = "";
            $scope.stdalu = "";
            $scope.acmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id
            };

            apiService.create("CollegeUsernameCreation/onCoursechange", data).
                then(function (promise) {
                    if (promise != null) {
                        $scope.getBranch = promise.branchlist;
                        if ($scope.getBatch.length == 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };

        //-----------Branch Change
        $scope.onBranchchange = function () {

            $scope.students = [];
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id
            };

            apiService.create("CollegeUsernameCreation/onBranchchange", data).
                then(function (promise) {
                    if (promise != null) {
                        $scope.getSemester = promise.semesterlist;
                        if ($scope.getSemester.length == 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };

        //---- Semester Change
        $scope.onSemchange = function () {

            $scope.students = [];
            $scope.acmS_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id
            };

            apiService.create("CollegeUsernameCreation/onSemchange", data).
                then(function (promise) {

                    if (promise != null) {
                        $scope.getSection = promise.sectionlist;
                        if ($scope.getSection.length == 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };


        //--------Section Change
        $scope.onSectionchange = function () {
            $scope.students = [];
        };



        $scope.get_Studentdetails = function () {
            $scope.submitted = true;

            if ($scope.stdalu == "Alumni") {
                $scope.acmS_Id = "0";
            }

            $scope.students = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                    "Studenttype": $scope.stdalu
                };

                apiService.create("CollegeUsernameCreation/get_Studentdetails", data).then(function (promise) {

                    $scope.students = promise.studentlist;

                    if ($scope.students.length == 0) {
                        swal("No Records Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
            });
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }



        //------------------------------Save
        $scope.saveatt = function (objas) {

            $scope.submitted = true;

            $scope.albumNameArray1 = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray1.push(user);
                }
            })

            if ($scope.albumNameArray1.length == 0) {
                swal("Please Select Alteast One Student Details");
                return;
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id,
                "ACMS_Id": $scope.acmS_Id,
                "Temp_Student": $scope.albumNameArray1,
                "Studenttype": $scope.stdalu

            }
            apiService.create("CollegeUsernameCreation/saveatt", data).then(function (promise) {
                if (promise != null) {
                    if (promise.msg != null) {
                        swal(promise.msg);
                    } else {
                        swal("Failed to create Username");
                    }
                } else {
                    swal("Something Went Wrong Please Contact Administrator");
                }

                $state.reload();
            });

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





    }
})();