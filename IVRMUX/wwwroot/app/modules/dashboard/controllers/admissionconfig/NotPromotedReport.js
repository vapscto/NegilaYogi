(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdharNotEnteredListController', AdharNotEnteredListController)
    AdharNotEnteredListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function AdharNotEnteredListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.doc_flag = true;
        $scope.div_flag = false;
        $scope.searchString = "";
        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.loadbasicdata = function () {

            apiService.get("AdharNotEnteredList/getdetails", 2).then(function (promise) {
                if (promise !== "") {
                    $scope.yearlist = promise.academicList;

                }
            });
        }



        $scope.submitted = false;
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id

                }
                apiService.create("AdharNotEnteredList/gestudetails", data).then(function (promise) {
                    if (promise.searchdatalist != null && promise.searchdatalist != "") {
                        $scope.searchdatalist = promise.searchdatalist;
                        $scope.totcountfirst = promise.searchdatalist.length;
                        $scope.exportsheet = true;
                        $scope.doc_flag = false;
                        $scope.div_flag = true;
                    }

                    else {
                        swal("No Records Found");
                        $scope.exportsheet = false;
                        $scope.div_flag = false;
                        $scope.doc_flag = true;
                        $scope.doc_sel = "";
                    }
                })
            } else {
                $scope.submitted = true;

            }

        }

        $scope.printdatatable = [];
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {

                var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');

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
                $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printdatatable = [];
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.searchdatalist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }

        //pagination

        //search functionality and pagination
        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;
                $scope.searchtrust();   //calling Search functionality
            }
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.loadbasicdata();
        }

    }

})();







