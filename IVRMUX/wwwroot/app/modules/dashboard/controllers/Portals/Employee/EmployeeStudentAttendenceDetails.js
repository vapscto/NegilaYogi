

(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeStudentAttendenceDetailsController', EmployeeStudentAttendenceDetailsController);

    EmployeeStudentAttendenceDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];
    function EmployeeStudentAttendenceDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.searchValue = "";
        $scope.totalregstudent = 0;

        $scope.totalnewstudent = 0;
        $scope.sms = 0;
        $scope.email = 0;
        $scope.regular = [];

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.searchValue = "";
        $scope.itemsPerPage = 15;
        
        $scope.fields = function () {
            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];
            $scope.Todaydate = new Date();
        };
        $scope.studentdrp = false;

        $scope.Binddata = function () {
            $scope.fields();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.amst_Id = "";
            apiService.getDATA("EmployeeStudentAttendenceDetails/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                });
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.amst_Id = "";
            var a = $scope.asmaY_Id;            
            $scope.fields();
            apiService.getURI("EmployeeStudentAttendenceDetails/getclass", asmaY_Id).
                then(function (promise) {
                    $scope.classarray = promise.classlist;
                });
        };

        $scope.OnClass = function (asmcL_Id) {
            $scope.asmcL_Id = asmcL_Id;
            $scope.fields();
            $scope.asmS_Id = "";
            $scope.amst_Id = "";
            var data = {
                "asmcL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("EmployeeStudentAttendenceDetails/Getsection", data).
                then(function (promise) {

                    $scope.section = promise.sectionList;
                });
        };

        $scope.OnSection = function (asmS_Id) {
            $scope.asmS_Id = asmS_Id;
            $scope.fields();
            $scope.amst_Id = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": asmS_Id,                
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("EmployeeStudentAttendenceDetails/GetAttendence", data).
                then(function (promise) {

                    $scope.fillstudents = promise.studentList;
                });
        };
       
        //======================= Student Selection        
        $scope.togchkbx = function () {
            $scope.stuall = $scope.fillstudents.every(function (itm) {
                return itm.stuckd;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.fillstudents.some(function (options) {
                return options.stuckd;
            });
        };
        $scope.all_check = function (stual) {
            $scope.stuall = stual;
            var toggleStatus = $scope.stuall;
            angular.forEach($scope.fillstudents, function (st) {
                st.stuckd = toggleStatus;
            });
        };

        //============================ Attendance Details
        $scope.showreport = function () {
            $scope.studentArray = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.fillstudents, function (stu) {
                    if (stu.stuckd === true) {
                        $scope.studentArray.push(stu);
                    }
                });
                var data = {
                    "asmcL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "asmS_Id": $scope.asmS_Id,
                    "studentArray": $scope.studentArray                
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("EmployeeStudentAttendenceDetails/GetIndividualAttendence", data).
                    then(function (promise) {
                        if (promise.attendencelist.length > 0) {
                            $scope.indattendance = true;
                            $scope.attdnclst = promise.attendencelist;    
                            
                        }
                        else {
                            swal("No Record Found");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }
})();