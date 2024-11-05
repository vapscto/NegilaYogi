(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeactivedeactivestudentsController', CollegeactivedeactivestudentsController)

    CollegeactivedeactivestudentsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeactivedeactivestudentsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.pagesrecord = {};
        $scope.students = [];
        $scope.adtable = true;
        $scope.searchValue = '';

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
                copty = "";
            }
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.students11 = [];

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;


        $scope.getpagesname = function () {
            $scope.currentPage = 1;
            var pageid = 1;
            apiService.getURI("CollegeActiveDeactiveStudents/getdata", pageid).
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                });
        };



        $scope.radiobtnchange = function () {
            $scope.vals = [];
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.students = [];   
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.onacademicyearchange = function () {
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onacademicyearchange", data).

                then(function (promise) {
                    $scope.courselist = promise.courselist;
                });
        };

        $scope.oncoursechange = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/oncoursechange", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onbranchchange = function () {
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onbranchchange", data).then(function (promise) {
                $scope.semesterlist = promise.semesterlist;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeActiveDeactiveStudents/onchangesemester", data).then(function (promise) {
                $scope.sectionlist = promise.sectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];
        };


        $scope.search = function () {
            $scope.adtable = true;
            $scope.students = [];
            $scope.students11 = [];

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMCST_SOL": $scope.AMCST_SOL
                };

                apiService.create("CollegeActiveDeactiveStudents/search", data).then(function (promise) {
                    $scope.students11 = promise.studentlist;
                    if ($scope.students11.length > 0) {
                        $scope.students = promise.studentlist;
                        $scope.adtable = false;
                    } else {
                        $scope.adtable = true;
                        swal("No Record Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;

        $scope.Clearid_up = function () {
            $state.reload();
        };


        var studclear = [];

        $scope.Clearid = function () {
            $state.reload();
            $scope.AMST_SOL = 'S';
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMC_Id = '';
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.isOptionsRequired = function () {
            return !$scope.students.some(function (options) {
                return options.checkedvalue;
            });
        };

        $scope.students1 = [];
        $scope.submitted = false;

        $scope.savedata = function (pagesrecord) {

            if ($scope.myForm.$valid) {

                if ($scope.all == true) {
                    angular.forEach(pagesrecord, function (student) {
                        $scope.students1.push(student);
                    });
                } else {
                    angular.forEach(pagesrecord, function (student) {
                        if (student.checkedvalue == true) {
                            $scope.students1.push(student);
                        }
                    });
                }

                if ($scope.AMCST_SOL == 'S') {
                    $scope.AMCST_SOL_activate = 'D';
                }
                else {
                    $scope.AMCST_SOL_activate = 'S';
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "savetmpdata": $scope.students1,
                    "AMCST_SOL_activate": $scope.AMCST_SOL_activate
                };

                apiService.create("CollegeActiveDeactiveStudents/savedata/", data).

                    then(function (promise) {

                        if (promise.returnval == true) {

                            if ($scope.AMCST_SOL == 'S') {
                                swal('Record Deactivated Successfully');
                                $scope.Clearid();

                            }
                            else if ($scope.AMCST_SOL == 'D') {
                                swal('Record Activated Successfully');
                                $scope.Clearid();
                            }
                        }
                        else {
                            swal('Record Not Activated/Deactivated Successfully');
                            $scope.Clearid();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.propertyName = 'regno';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        };
    }
})();

