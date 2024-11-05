(function () {
    'use strict';
    angular
.module('app')
.controller('HolidayReportController', HolidayReportController)

    HolidayReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function HolidayReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.GridviewDetails = [];
        $scope.grid_view = false;
        $scope.loaddata = function () {


            
            $scope.all_check();
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 1;
            apiService.getURI("HolidayReport/", pageid).
        then(function (promise) {
            
            $scope.yearlist = promise.yeardropdown;
            $scope.holidayType = promise.holidayType;
            $scope.days_list = promise.dayslist;
            //$scope.gridviewDetails = promise.report_list;
        });
            function ShowHide() {
                apiService.getURI("HolidayReport/", pageid).
                then(function (promise) {
                    lblShowHide.style.visibility = 'visible';
                })

            }

        }
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.group_check = "0";
        $scope.all_check = function () {
            
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.days_list, function (itm) {
                itm.dayy = toggleStatus;
            });

        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.days_list.every(function (options) {
                return options.dayy;
            });
        }
        $scope.isOptionsRequired = function () {
            if ($scope.group_check == "1") {
                return !$scope.days_list.some(function (options) {
                    return options.dayy;
                });
            }
            else if ($scope.group_check == "0") {
                return false;
            }

        }

        //option toggled
        $scope.sort = function (keyname) {
            
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa

        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.gridviewDetails.every(function (itm)
            { return itm.selected; });
            if ($scope.GridviewDetails.indexOf(SelectedStudentRecord) === -1) {
                $filter('date')(SelectedStudentRecord.fomhwdD_Date, "yyyy-MM-dd");
                $scope.GridviewDetails.push(SelectedStudentRecord);
            }
            else {
                $scope.GridviewDetails.splice($scope.GridviewDetails.indexOf(SelectedStudentRecord), 1);
            }
        };
        //all toggled
        $scope.toggleAll = function () {
            
            var toggleStatus = $scope.all;
            angular.forEach($scope.gridviewDetails, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.GridviewDetails.indexOf(itm) === -1) {
                        //
                        //itm.fomhwdD_Date = $filter('date')(itm.fomhwdD_Date, "dd-MM-yyyy");
                        $scope.GridviewDetails.push(itm);
                    }
                }
                else {
                    $scope.GridviewDetails.splice(itm);
                }
            });
        }
  
      

        $scope.submitted = false;
    
        $scope.HolidayReport = function () {
            if ($scope.myForm.$valid) {
                $scope.Refrencecheckboxchcked = [];
                var data = {

                    "HRMLY_Id": $scope.asmaY_Id,
                    "FOHWDT_Id": $scope.fohwdT_Id,
                    "Day_flag": $scope.dayflag,
                }
                
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HolidayReport/Report/", data).
            then(function (promise) {
                
                if (promise.count > 0) {
                    $scope.gridview1 = true;
                    $scope.gridviewDetails = promise.report_list;
                    $scope.gridflag = true;
                    $scope.print_flag = false;
                    $scope.excel_flag = false;
                    $scope.grid_view = true;
                } else {
                    swal("Record not found.");
                    $scope.gridflag = false;
                    $scope.print_flag = true;
                    $scope.excel_flag = true;
                    $scope.gridviewDetails = "";
                    $scope.grid_view = false;
                }
            })

            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.cancel = function () {           
              
                $scope.gridflag = false;


                $scope.excel_flag = true;
                $scope.print_flag = true;

                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();           
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";                
                $scope.fohwdT_Id = "";
                $scope.gridflag = false;
                $scope.excel_flag = true;
                $scope.print_flag = true;            

                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

                $scope.gridviewDetails = "";
                $scope.grid_view = false;

        };

        $scope.exportToExcel = function (tableId)
        {
            
            if ($scope.GridviewDetails !== null && $scope.GridviewDetails.length > 0)
            {
                var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');

                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select at least one checkbox");
            }



           
            
               
               
           
            
            //if ($scope.gridviewDetails !== null && $scope.gridviewDetails.length > 0)
            //{
            //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            //    $timeout(function () { location.href = exportHref; }, 100);
            //}
            //else {
            //    swal("Please select records to be Exported");
            //}

        }
        //$scope.printData = function () {
        //    if ($scope.gridviewDetails !== null && $scope.gridviewDetails.length > 0)
        //    {
        //        var divToPrint = document.getElementById("table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
                
        //    }
        //    else {
        //        swal("Please select records to be printed");
        //    }
        //}

        $scope.printData = function (print_id)
        {
            
            if ($scope.GridviewDetails !== null && $scope.GridviewDetails.length > 0) {
                var innerContents = document.getElementById("print_id").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                       '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


    }



})();

















