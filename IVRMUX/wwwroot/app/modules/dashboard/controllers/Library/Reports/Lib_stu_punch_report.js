(function () {
    'use strict';
    angular
        .module('app')
        .controller('Lib_stu_punch_reportController', libriarypuchreport)

    libriarypuchreport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','$filter']
    function libriarypuchreport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {
        $scope.getstudentlist = [];
        $scope.sectionlist = [];
        $scope.studentlist = [];
        $scope.section_list = [];
        $scope.boimetricname = [];
        $scope.obj = {};
        $scope.BindData = function () {
            var pageid = 2;
            
            apiService.getURI("Lib_stu_punch_report/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
                $scope.boimetricname = promise.biometricdevice;
              
            });
        };
      


        $scope.onyearchange = function () {
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Lib_stu_punch_report/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
             
            });
        };

        $scope.onclasschange = function () {
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("Lib_stu_punch_report/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };
        $scope.onsectionchange = function () {
        
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("Lib_stu_punch_report/get_students_category_grade", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.all = true;

                angular.forEach($scope.getstudentlist, function (dd) {
                    dd.AMST_Ids = true;
                });
            });
        };
        $scope.OnClickAll = function () {
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.AMST_Ids = $scope.all;
            });
        };


        $scope.obj.fromdate = new Date();
        $scope.obj.todate = new Date();
        $scope.submitted = false;
        $scope.Punchreport = function () {
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");                var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                $scope.Temp_AmstId = [];
                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.AMST_Ids) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });

                var input = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "FOBD_Id": $scope.FOBD_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "Fromdate": fromdate1,
                    "Todate": todate1,
                }
                apiService.create("Lib_stu_punch_report/get_report", input).then(function (promise) {
                    if (promise.getstupunchreport != null && promise.getstupunchreport.length > 0) {
                        $scope.Punchreport = promise.getstupunchreport;
                        $scope.columnnames = promise.columnnames;
                        var temp_array = [];
                        var temp_array1 = [];
                        for (var x = 0; x < promise.getstupunchreport.length; x++) {

                            var newCol = { punchdate: promise.getstupunchreport[x].ASPU_PunchDate, punchtime: promise.getstupunchreport[x].Punch_Time, In_Out: promise.getstupunchreport[x].In_Out }
                            temp_array.push(newCol);
                            if (x < promise.getstupunchreport.length - 1) {
                                if (promise.getstupunchreport[x].AMST_Id == promise.getstupunchreport[x + 1].AMST_Id)
                                    continue;
                            }
                            var newCol1 = { pdate: temp_array, AMST_AdmNo: promise.getstupunchreport[x].AMST_AdmNo, Student_Name: promise.getstupunchreport[x].Student_Name }
                            temp_array1.push(newCol1);
                            temp_array = [];
                            $scope.puncharray = temp_array1;

                        }
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

         

        }
        

    }

})();