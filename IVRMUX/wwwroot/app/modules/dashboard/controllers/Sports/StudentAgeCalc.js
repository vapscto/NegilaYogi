(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentAgeCalcController', StudentAgeCalcController);

    StudentAgeCalcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function StudentAgeCalcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("StudentAgeCalc/loadgrid/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                $scope.studentDropdown = promise.studentList;
                $scope.datareport = promise.datareport;

                if (promise.count > 0) {
                    $scope.eventsStudentRecordList = promise.eventsStudentRecordList;
                    $scope.presentCountgrid = $scope.eventsStudentRecordList.length;
                }
                $scope.cancel();
            });
        }
        $scope.getStudent = function () {
            $scope.studentDropdown = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            }
            apiService.create("StudentAgeCalc/getStudents", data).then(function (promise) {
                if (promise.studentList != null) {
                    $scope.studentDropdown = promise.studentList;
                }

            })
        }



        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                debugger
                if ($scope.studentDropdown.length > 0) {
                    $scope.selectedStdList = [];
                    for (var i = 0; i < $scope.studentDropdown.length; i++) {
                        if ($scope.studentDropdown[i].stud == true) {
                            $scope.selectedStdList.push($scope.studentDropdown[i]);
                        }
                    }
                }

                var obj = {
                    "SPCCAC_Id": $scope.SPCCAC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    student: $scope.selectedStdList,
                    "Till_Date": new Date($scope.Till_Date).toDateString()
                }
                apiService.create("StudentAgeCalc/saveRecord", obj).
                    then(function (promise) {
                        if (promise.returnVal == 'saved') {
                            swal("Record saved Successfully");
                            $scope.datareport = promise.datareport;
                            $scope.cancel();
                           // $scope.Cumureport = true;
                          //  $scope.screport = true;
                        }
                        else if (promise.returnVal == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Records already exists");
                            $scope.loadgrid();

                        }
                        
                    });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Print = function () {
            var innerContents = '';
            innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        $scope.cancel = function () {

            $scope.SPCCAC_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.Till_Date = "";
            $scope.studentDropdown = "";
            $scope.Cumureport = false;
            $scope.screport = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

            angular.forEach($scope.studentList, function (option9) {
                option9.selected = false;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired3 = function () {

            return !$scope.studentList.some(function (options) {
                return options.selected;
            });

        }

        $scope.addColumn4 = function () {
            $scope.selected = $scope.studentList.every(function (itm) { return itm.selected; });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentDropdown.every(function (options) {
                return options.stud;
            });
        }
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.studentDropdown, function (itm) {
                itm.stud = checkStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.studentDropdown.some(function (options) {
                return options.stud;
            });

        }

    }
})();