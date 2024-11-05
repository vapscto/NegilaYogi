(function () {
    'use strict';

    angular
        .module('app')
        .controller('BirthdayReport', BirthdayReport)
    BirthdayReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function BirthdayReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.submitted = false;
        $scope.table_flag = false;
        $scope.staffDetPrint = false;
        $scope.changeRadioVal = function () {
                $scope.hidegv1 = false;
                $scope.hidegv2 = false;
                $scope.studentlist = "";
                $scope.staffList = "";
                $scope.Start_Date = "";
                $scope.End_Date = "";
                $scope.details = "";
                $scope.printdatatable.length = 0;
                $scope.staffPrintList.length = 0;
        }
        $scope.validatetodate = function (data1) {
            $scope.toDate = data1;
            $scope.minDatet = new Date(
           $scope.toDate.getFullYear(),
           $scope.toDate.getMonth(),
          $scope.toDate.getDate());

            $scope.maxDatet = new Date(
          $scope.toDate="",
          $scope.toDate.getMonth(),
         $scope.toDate.getDate());
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.getReport = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.hidegv2 = false;
            $scope.hidegv1 = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "rdbbutton": $scope.radioption,
                    "start_date": new Date($scope.Start_Date).toDateString(),
                    "end_date": new Date($scope.End_Date).toDateString()
                }

                apiService.create("BirthDay/getReport/", data).
            then(function (promise) {
                if ($scope.radioption == "student") {
                    if (promise.count > 0) {
                        $scope.hidegv1 = false;
                        $scope.hidegv2 = true;
                        $scope.studentlist = promise.studentlist;
                    }
                    else {
                        swal("No Records Found");
                        $scope.hidegv2 = false;
                        $scope.hidegv1 = false;
                    }
                }
                else if ($scope.radioption == "Staff") {
                    if (promise.count > 0) {
                        $scope.hidegv1 = true;
                        $scope.hidegv2 = false;
                        $scope.staffList = promise.staffList;
                    }
                    else {
                        swal("No Records Found");
                        $scope.hidegv1 = false;
                        $scope.hidegv2 = false;
                    }

                }

            });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.printdatatable = [];
        $scope.staffPrintList = [];
        $scope.toggleAll = function () {
           
            if ($scope.radioption == "student") {

                var toggleStatus = $scope.details;
                angular.forEach($scope.studentlist, function (itm) {
                    itm.Selected = toggleStatus;
                    if ($scope.details == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
            else if ($scope.radioption == "Staff") {
                var toggleStatus = $scope.checkall;
                angular.forEach($scope.staffList, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.checkall == true) {
                        $scope.staffPrintList.push(itm);
                    }
                    else {
                        $scope.staffPrintList.splice(itm);
                    }
                });

            }
          
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            if ($scope.radioption == "student") {
                $scope.details = $scope.studentlist.every(function (itm)
                { return itm.Selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            else if ($scope.radioption == "Staff") {
                $scope.checkall = $scope.staffList.every(function (itm)
                { return itm.checked; });
                if ($scope.staffPrintList.indexOf(SelectedStudentRecord) === -1) {
                    $scope.staffPrintList.push(SelectedStudentRecord);
                }
                else {
                    $scope.staffPrintList.splice($scope.staffPrintList.indexOf(SelectedStudentRecord), 1);
                }
            }
        }
        $scope.Print = function () {
            if ($scope.radioption == "student") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var innerContents = document.getElementById("printSectionId").innerHTML;
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
                else {
                    swal("Please Select Records to be Printed");
                }

            }
            else if ($scope.radioption == "Staff") {
                if ($scope.staffPrintList !== null && $scope.staffPrintList.length > 0) {
                    var innerContents = document.getElementById("printstaffDet").innerHTML;
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
                else {
                    swal("Please Select Records to be Printed");
                }
            }
         
        }

        $scope.ExportToExcel = function (tableId) {
            
            if ($scope.radioption == "student") {
                if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
            else if ($scope.radioption == "Staff") {
                if ($scope.staffPrintList !== null && $scope.staffPrintList.length > 0) {
                    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                    $timeout(function () { location.href = exportHref; }, 100);
                }
                else {
                    swal("Please Select Records to be Exported");
                }
            }
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
    
})();
