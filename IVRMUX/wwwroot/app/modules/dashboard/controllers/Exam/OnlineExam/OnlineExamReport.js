

(function () {
    'use strict';
    angular
.module('app')
        .controller('OnlineExamConfigController', OnlineExamConfigController)

    OnlineExamConfigController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamConfigController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
     
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;
       
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        //------------------1st Tab 
        $scope.savedata = function () {
            $scope.submitted1 = true;
            debugger;
            if ($scope.myForm.$valid) {

                $scope.fromdate = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.FMCB_toDATE).toDateString();

                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("OnlineExamConfig/getreport", data).
                    then(function (promise) {
                        $scope.currentPage = 1;
                        $scope.itemsPerPage = paginationformasters;

                        if (promise.result.length > 0) {
                            $scope.export_flag = true;
                            $scope.print_flag = true;
                                     $scope.result = promise.result;
                                 }
                                 else {
                                     swal('No Records Found');
                                 }

                                 //$state.reload();
                             })
            }
            else {
                $scope.submitted1 = true;
            }
        };
        

        $scope.cancel = function () {
            $state.reload();
        }
        
        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        
        $scope.interacted = function (field) {
            return $scope.submitted1;
        };
        
        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }


        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            debugger;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');

                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }


        $scope.printData = function (printSectionId) {
            debugger;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.result, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }

  
    }
})();