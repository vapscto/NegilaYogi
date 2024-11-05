


(function () {
    'use strict';
    angular
        .module('app')
        .controller('ConsolidatesRankReportController', ConsolidatesRankReportController)

    ConsolidatesRankReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$compile', '$timeout']
    function ConsolidatesRankReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $compile, $timeout) {



        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.page1 = "pag1";
        $scope.reverse1 = true;

        $scope.vals1 = [];


        //$scope.toggleAll = function () {
        //    var toggleStatus = $scope.all;
        //    angular.forEach($scope.StudentName, function (itm) {
        //        itm.checked = toggleStatus;

        //    });
        //}

        //$scope.optionToggled = function () {
        //    $scope.all = $scope.StudentName.every(function (itm)
        //    { return itm.checked; });
        //}


        $scope.loaddata = function () {
            apiService.getDATA("ConsolidatesRankReport/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.fillyear;
                    $scope.classarray = promise.classlist;
                    $scope.arrlist9 = promise.admissioncatdrpall;
                });
        };

        $scope.OnAcdyear = function (asmaY_Id) {

            $scope.asmcL_Id = "";

            var a = $scope.asmaY_Id;


            apiService.getURI("ConsolidatesRankReport/getclass", asmaY_Id).
                then(function (promise) {
                    $scope.classarray = promise.classlist;
                })


        }

        $scope.showreport = function () {
            $scope.students = [];
            $scope.all = false;
            $scope.printdatatable = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "asmcL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "order_type": $scope.hrsrt,
                    "CasteCategory_Id": $scope.CasteCategory_Id
                }

                apiService.create("ConsolidatesRankReport/Getreport", data).
                    then(function (promise) {


                        if (promise.wirettenTestSubjectWiseStudentMarks.length > 0) {
                            //$scope.indattendance = true;

                            $scope.students = promise.wirettenTestSubjectWiseStudentMarks;
                            if ($scope.students != undefined && $scope.students != null && $scope.students.length > 0) {
                                $scope.showgriddata = true;
                                $scope.presentCountgrid = $scope.students.length;
                            }

                            if ($scope.hrsrt == 'Name') {
                                $scope.sort('PASR_FirstName');
                                $scope.sort('PASR_FirstName');
                            }
                            else if ($scope.hrsrt == 'Rank') {
                                $scope.sort('RNK');
                                $scope.sort('RNK');
                            }
                            else if ($scope.hrsrt == 'ApplNo') {
                                $scope.sort('PASR_RegistrationNo');
                                $scope.sort('PASR_RegistrationNo');
                            }

                        }
                        else {
                            $scope.showgriddata = false;
                            swal("No Record Found")
                        }

                    })


            }
        }

        $scope.submitted = false;
        $scope.BindGrid = function (details) {
            $scope.submitted = true;

        };


        $scope.Clearid = function () {
            $state.reload();
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //written test marks

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.fullname).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.students, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
        };

        $scope.selected = function (SelectedStudentRecord, index) {

            // $scope.printdatatable = [];

            if ($scope.searchValue == '') {
                $scope.all = $scope.students.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

        //for Print
        $scope.printData = function (print_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();

            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        //for Export excel
        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }

})();