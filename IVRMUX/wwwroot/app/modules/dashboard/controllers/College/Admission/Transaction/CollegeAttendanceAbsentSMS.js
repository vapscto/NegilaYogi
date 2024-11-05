(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeAttendanceAbsentSMSController', CollegeAttendanceAbsentSMSController)

    CollegeAttendanceAbsentSMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function CollegeAttendanceAbsentSMSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {


   

        $scope.currentPage = 1;
        $scope.sms_flag = true;
        //load
        $scope.Loaddata = function () {
            debugger;
            var pageid = 2;
            apiService.getURI("CollegeAttendanceAbsentSMS/getdetails", pageid).then(function (promise) {

                $scope.fillacademiyear = promise.acdlist;
            });
        }
        $scope.fromdate = new Date();

        $scope.submitted = false;
     


        $scope.searchValue1 = '';
        $scope.filterValue11 = function (obj) {

            return (obj.classsection).indexOf($scope.searchValue1) >= 0 || (obj.studentname).indexOf($scope.searchValue1) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Cancel = function () {
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.fromdate = "";
            $scope.submitted = false;
            $scope.catreport = false;
            $scope.catreport_btn = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = '';
            $scope.searchValue1 = '';
            $scope.amsT_Date = '';
            $scope.details1 = '';
            $scope.studentreport = [];
            $scope.sms_flag = true;
        }
        

        $scope.onselectAcdYear = function () {
            debugger;
            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semisterlist = [];
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CollegeAttendanceAbsentSMS/onselectAcdYear", data).then(function (promise) {

                $scope.courselist = promise.courselist;
            });
        }

        $scope.onselectCourse = function () {
            debugger;
            $scope.branchlist = [];
            $scope.semisterlist = [];
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("CollegeAttendanceAbsentSMS/onselectCourse", data).then(function (promise) {

                $scope.branchlist = promise.branchlist;
            });
        }

        $scope.onselectBranch = function () {
            debugger;
            $scope.semisterlist = [];
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CollegeAttendanceAbsentSMS/onselectBranch", data).then(function (promise) {

                $scope.semisterlist = promise.semlist;
         
            });
        }
        
        $scope.onSelectsemm = function () {
            debugger;
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
            }
            apiService.create("CollegeAttendanceAbsentSMS/getsection", data).then(function (promise) {

                $scope.sectionlist = promise.seclist;

            });
        }

        $scope.minDatedof = new Date();
        $scope.maxDatedof = new Date();
        $scope.Todates = new Date();
        $scope.getAttendetails = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var datess = $scope.Todates == null ? "" : $filter('date')($scope.Todates, "yyyy-MM-dd");
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "Todates": datess,
                }
                apiService.create("CollegeAttendanceAbsentSMS/getAttendetails", data).then(function (promise) {

                    $scope.studentreport = promise.studentreport;
                    if ($scope.studentreport.length > 0) {
                        $scope.sms_flag = false;
                    }
                    else {
                        swal("Records are not Available!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Send_SMS = function () {
            $scope.studentreportlist = [];
            debugger;
            var datess = $scope.Todates == null ? "" : $filter('date')($scope.Todates, "yyyy-MM-dd");
            angular.forEach($scope.studentreport, function (cls) {
                if (cls.Selected1 == true) {
                    $scope.studentreportlist.push(cls);
                }
            });
            if ($scope.studentreportlist.length > 0) {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "Todates": datess,
                "absentlist": $scope.studentreportlist,
            }
            apiService.create("CollegeAttendanceAbsentSMS/absentsendsms", data).then(function (promise) {

                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        } else {
            swal("Select The Student List To Send The SMS");
        }
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.details1;
            angular.forEach($scope.studentreport, function (itm) {
                itm.Selected1 = toggleStatus;
            });
        }

        $scope.optionToggled = function () {
            $scope.details1 = $scope.studentreport.every(function (itm){
                return itm.Selected1;
            })
        }
       

    }
})();
