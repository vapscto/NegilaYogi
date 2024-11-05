(function () {
    'use strict';
    angular
.module('app')
.controller('DueDateReportController', DueDateReportController)
    DueDateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function DueDateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {


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


        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.pagination = true;
        $scope.export_disable = true;
        $scope.printdatatable = [];
        $scope.sorting_disable = true;
        $scope.grid_flag = false;
        $scope.searchString = "";
        $scope.export_flag = false;
        $scope.print_flag = false;
       // $scope.showbutton = false;


        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
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


        $scope.onselectyear = function (obj) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("FeeDueDateReport/getdata", data).
               then(function (promise) {
                   $scope.class_Category_List = promise.class_Category_List;

               })
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.export_flag = true;
            }
            else {
                $scope.export_flag = false;
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.searchdatalist.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.export_flag = true;
            }
            else {
                $scope.export_flag = false;
            }

        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }

        $scope.loadbasicdata = function () {
            apiService.get("FeeDueDateReport/getinitialfeedata/").then(function (promise) {
                if (promise !== "") {
                    $scope.yearlist = promise.yearList;
                    $scope.class_Category_List = promise.class_Category_List;

                }
            });
        }

        //$scope.file_show = function () {
        //    if ($scope.asmaY_Id == null && $scope.asmaY_Id == undefined && $scope.fmcC_Id == null && $scope.fmcC_Id == undefined) {
        //        swal("Please Select Academic Year and Class Category");
        //        $scope.doc_sel = false;
        //    }
        //}

        $scope.submitted = false;
        $scope.showreport = function () {
           // swal("hi");
           
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "FMCC_Id": $scope.fmcC_Id
            }

            if ($scope.myForm.$valid) {
                apiService.create("FeeDueDateReport/", data).then(function (promise) {

                    if (promise.fhwR_searchdatalist !== null) {
                        //$scope.export_flag = true;
                        $scope.print_flag = true;
                        //angular.forEach(promise.fhwR_searchdatalist, function (itm) {
                        //    itm.due_Date = $filter('date')(new Date(itm.from_Date), 'dd/MM/yyyy');
                        //})


                        $scope.pagination = true;
                        for (var i = 0; i < promise.fhwR_searchdatalist.length; i++) {
                            if (promise.fhwR_searchdatalist[i].due_Date == null || promise.fhwR_searchdatalist[i].due_Date == "") {
                                promise.fhwR_searchdatalist[i].due_Date = "N/A";
                            }
                            if (promise.fhwR_searchdatalist[i].fine_Type == null || promise.fhwR_searchdatalist[i].fine_Type == "") {
                                promise.fhwR_searchdatalist[i].fine_Type = "N/A";
                            }
                        }
                        $scope.searchdatalist = promise.fhwR_searchdatalist;
                        $scope.totcountfirst = promise.fhwR_searchdatalist.length;
                        $scope.export_disable = false;

                        $scope.sorting_disable = false;
                    }
                    if (promise.fhwR_searchdatalist !== null && promise.fhwR_searchdatalist.length > 0) {
                        //swal("Records found");
                        $scope.pagination = true;
                        $scope.grid_flag = true;

                    }
                    else {
                        swal("No Records Found");
                        $scope.export_disable = true;
                        $scope.sorting_disable = true;
                        $scope.doc_sel = false;
                       // $scope.pagination = false;
                        $scope.grid_flag = false;
                       // $state.reload();
                    }

                   

                })
            }

            else {
                $scope.submitted = true;

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

       // $scope.propertyName = 'fee_Group';

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.clear_details = function () {
            $state.reload();
        }
        

        $("#tableExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#table1').html());
            e.preventDefault();
        });


    }

})();







