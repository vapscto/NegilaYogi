(function () {
    'use strict';
    angular
        .module('app')
        .controller('UsrPwdController', UsrPwdController)

    UsrPwdController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function UsrPwdController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
            else {
                paginationformasters = "";
                copty = "";
            }
        } else {
            paginationformasters = "";
            copty = "";
        }

        $scope.itemsPerPage = 25;
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            //dd
        }

        $scope.imgname = logopath;
        $scope.reporsmart = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.BindData = function () {
            apiService.getDATA("SMSEmailSend/getdata/2").then(function (promise) {
                if (promise !== null) {
                    $scope.YearList = promise.yearList;
                    $scope.classlist = promise.classlist;
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal("No Records Found");
                }
            });
        };
        

        $scope.onclickloaddata = function () {

            if ($scope.allorindiv === "All") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.allorindiv === "indi") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            $scope.students = [];
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }              
            });           
        };

        $scope.toggleAll1 = function () {
            var toggleStatus1 = $scope.all1;
            $scope.printstudents = [];
            angular.forEach($scope.countsts, function (itm) {
                itm.selected1 = toggleStatus1;
                if (itm.selected1 == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
            });
        };
        $scope.clearfields = function () {
            $state.reload();
        };

        $scope.searchValue = '';       
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.asmC_SectionName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.amsT_AdmNo).indexOf(angular.lowercase($scope.searchValue)) >= 0
            // || angular.lowercase(obj.trmL_DropLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.getreport = function (obj) {

            $scope.all = "";
            $scope.searchValue = '';

            if ($scope.myForm.$valid) {
                var data =
                {
                    //"validreport": $scope.regorname_map,
                    "asmaY_Id": $scope.asmaY_Id,
                    "asmcL_Id": $scope.asmcL_Id,
                    "asmS_Id": obj.asmS_Id,
                    "filterdata": $scope.filterdata
                };

                apiService.create("UsrPwd/Getreportdetails", data).then(function (promise) {

                    if (promise.messagelist.length === 0) {
                        $scope.reporsmart = false;
                        swal("Record Not Found !!");
                        $state.reload();
                    }
                    else {

                        $scope.allandfalse = true;
                        $scope.counttrue = false;
                        $scope.students = promise.messagelist;
                        $scope.presentCountgrid = $scope.students.length;
                        $scope.exp_excel_flag = false;
                        $scope.print_flag = false;
                    }

                });
            }

            else {
                $scope.submitted = true;
            }
        };

        $scope.createusername = function () {

            $scope.albumNameArray = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray.push(user);
                }
            });
            if ($scope.albumNameArray.length > 0) {
                var datalist =
                {
                    "filterdata": $scope.filterdata,
                    data_array: $scope.albumNameArray
                };
                apiService.create("UsrPwd/createusername", datalist).then(function (promise) {

                    if (promise.message !== null) {
                        swal(promise.message);
                    } else {
                        swal("Failed to create Username");
                    }
                    $state.reload();
                });
            }
        };



        $scope.emailsend = function () {

            $scope.albumNameArray1 = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) {
                    $scope.albumNameArray1.push(user);
                }
            });
            if ($scope.albumNameArray1.length > 0) {
                var datalist =
                {
                    data_array: $scope.albumNameArray1
                };
                apiService.create("SMSEmailSend/emailsend", datalist).then(function (promise) {

                    if (promise.success == "success") {
                        swal('Email Sent  Successfully...!', 'success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Send Email..!', 'Failure');
                        return;
                    }
                });
            }
        };

        $scope.printstudents = [];
        $scope.printData = function () {

            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = "";
                innerContents = document.getElementById("printareaId").innerHTML;
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
        };

        $scope.exportToExcel = function () {

            var printSectionId = '#table1';
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        };

        $scope.validreport = function () {
            if ($scope.regorname_map === "new") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "regular") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "both") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }

            $scope.students = [];

        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

    }

})();