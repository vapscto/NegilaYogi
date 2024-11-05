

(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdharNotEnteredListController', AdharNotEnteredListController)

    AdharNotEnteredListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function AdharNotEnteredListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;

        //userid = configsettings[0].user;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "User_Name";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.obj = {};

        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.pagination = false;
        $scope.validate = false;
        $scope.sms_flag = false;
        $scope.mail_flag = false;
        $scope.export_flag = false;
        $scope.print_flag = false;
        $scope.errMessage_From_Date = 'Select From Date from the Calendar';
        $scope.errMessage_To_Date = 'Select To Date from the Calendar';
        $scope.errMessage_Year = 'Select Academic Year';
        $scope.propertyName = 'pasP_FirstName';

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.Created_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (obj.Email_Id).indexOf($scope.searchValue) >= 0 || (obj.Mob_No).indexOf($scope.searchValue) >= 0;
        };

        $scope.OnChangeYear = function () {
            $scope.reportdetails = [];
        };

        $scope.OnChangeCountry = function () {
            $scope.reportdetails = [];
        };

        $scope.OnChangeState = function () {
            $scope.reportdetails = [];
        };

        $scope.exportToExcel = function (tableId) {

            var excelname = " CountryStateReport";
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = tableId;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, ' CountryStateReport');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.printData = function () {
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


        $scope.toggleAll = function () {

            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
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
            if ($scope.searchValue == '') {
                $scope.all = $scope.reportdetails.every(function (itm) { return itm.selected; });
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

        //Sorting function
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sortBydropdown = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //Loading the Intial Data Function
        $scope.loaddata = function () {
            apiService.get("AdharNotEnteredList/getdetails", pageid).then(function (promise) {

                $scope.academicList = promise.academicList;
                $scope.countrylst = promise.countrylist;
                $scope.statelst = promise.statelist;
            });
        };

        //Collapse function
        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        //Radio button switching function
        $scope.onclickloaddata = function () {
            $scope.reportdetails = [];
        };

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }



        //Date Compare function
        $scope.checkErr = function (FromDate, ToDate) {

            $scope.errMessage = '';
            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.IsHiddendown = false;
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }

        };

        //date field Validation
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };






        //Report showing function
        $scope.ShowReport = function (enq) {

            $scope.submitted = false;
            //Form validation
            if ($scope.myForm.$valid) {
                //$scope.state = 0;
                //$scope.country = 0;
                var state = 0;
                var country = 0;
                if ($scope.enq === "country") {
                    country = $scope.obj.ivrmmC_Id.ivrmmC_Id;
                }

                else if ($scope.enq === "state") {
                    state = $scope.obj.ivrmmS_Id.ivrmmS_Id;
                }

                var data = {
                    "countrytype": country,
                    "statetype": state,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "flag_type": $scope.enq
                };

                apiService.create("AdharNotEnteredList/Getcountrystatedata", data).then(function (promise) {

                    if (promise.studatalist != null && promise.studatalist.length > 0) {
                        $scope.reportdetails = promise.studatalist;
                        $scope.presentCountgrid = $scope.reportdetails.length;
                        $scope.IsHiddendown = true;
                        $scope.pagination = true;
                        $scope.print_flag = true;
                        $scope.export_flag = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.pagination = false;
                        $scope.IsHiddendown = false;
                        $scope.print_flag = false;
                        $scope.sms_flag = false;
                        $scope.mail_flag = false;
                        $scope.export_flag = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        //Cancel function
        $scope.cancel = function () {
            $state.reload();
        };
    }
})();