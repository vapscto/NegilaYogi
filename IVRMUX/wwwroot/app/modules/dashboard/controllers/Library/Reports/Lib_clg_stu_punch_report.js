(function () {
    'use strict';
    angular.module('app').controller('Lib_clg_stu_punch_reportController', Lib_clg_stu_punch_reportController)

    Lib_clg_stu_punch_reportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function Lib_clg_stu_punch_reportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {
        $scope.obj = {};
        $scope.yearlist = [];
        $scope.courselist = [];
        $scope.branchlist = [];
        $scope.sectionlist = [];
        $scope.semisterlist = [];
        //load academic year,
        $scope.Onloadpage = function () {
            var pageid = 2;
            apiService.getURI("Lib_stu_punch_report/onloadpage", pageid).then(function (promise) {
                $scope.yearlist = promise.clg_academicyear;
                $scope.courselist = [];
                $scope.branchlist = [];
                $scope.sectionlist = [];
                $scope.semisterlist = [];

            });
        }
        //to get course 
        $scope.loadcourse = function () {
            var input = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("Lib_stu_punch_report/loadcourse", input).then(function (response) {
                $scope.courselist = response.courselist;

            })
        }
        //load branch
        $scope.loadbranch = function () {
            var input = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            }
            apiService.create("Lib_stu_punch_report/loadbranch", input).then(function (promise) {
                $scope.branchlist = promise.branchlist;

            });
        }
        //load semester
        $scope.loadsemester = function () {
            var input = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            }
            apiService.create("Lib_stu_punch_report/loadsemester", input).then(function (promise) {
                $scope.semisterlist = promise.semisterlist;
                $scope.sectionlist = [];
            });
        }
        //load section
        $scope.loadsection = function () {
            var input = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("Lib_stu_punch_report/loaadsection", input).then(function (promise) {
                $scope.sectionlist = promise.getsection;
            });
        }
        //load students
        $scope.loadstudents = function () {
            var input = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("Lib_stu_punch_report/loadstudents", input).then(function (promise) {
                $scope.studentlist = promise.studentlist;
                $scope.all = true;
                angular.forEach($scope.studentlist, function (dd) {
                    dd.AMCST_Id = true;
                });
            });
        }
        $scope.OnClickAll = function () {
            $scope.studentdetails = [];
            angular.forEach($scope.studentlist, function (dd) {
                dd.AMCST_Id = $scope.all;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.AMCST_Id;
            });
        };
        //punchreport
        $scope.fromdate = '';
        $scope.submitted = false;
        $scope.clgpunchreport = function () {
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
                var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                $scope.Temp_AmstId = [];
                angular.forEach($scope.studentlist, function (dd) {
                    if (dd.AMCST_Id) {
                        $scope.Temp_AmstId.push({ AMCST_Id: dd.amcsT_Id });
                    }
                });

                var input = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "Temp_AmcstIds": $scope.Temp_AmstId,
                    "Fromdate": fromdate1,
                    "Todate": todate1,
                }
                apiService.create("Lib_stu_punch_report/clgpunchreport", input).then(function (promise) {

                    if (promise.clgstupunchreport != null && promise.clgstupunchreport.length > 0) {
                        $scope.clgstupunchreport = promise.clgstupunchreport;
                        $scope.columnnames = promise.columnnames;
                        var temp_array = [];
                        var temp_array1 = [];
                        for (var x = 0; x < promise.clgstupunchreport.length; x++) {
                            var newCol = { punchdate: promise.clgstupunchreport[x].ASPU_PunchDate, punchtime: promise.clgstupunchreport[x].Punch_Time, In_Out: promise.clgstupunchreport[x].In_Out }
                            temp_array.push(newCol);
                            if (x < promise.clgstupunchreport.length - 1) {
                                if (promise.clgstupunchreport[x].AMST_Id == promise.clgstupunchreport[x + 1].AMST_Id)
                                    continue;
                            }
                            var newCol1 = { pdate: temp_array, AMCST_AdmNo: promise.clgstupunchreport[x].AMCST_AdmNo, Student_Name: promise.clgstupunchreport[x].Student_Name }
                            temp_array1.push(newCol1);
                            temp_array = [];
                            $scope.puncharray = temp_array1;
                        }
                    }
                    else {
                        swal("No Data Found", "", "error");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
            $scope.interacted = function (field) {
                return $scope.submitted;
            };
        }
    }
})();