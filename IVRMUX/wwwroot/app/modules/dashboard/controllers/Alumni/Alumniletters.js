//By prashant latest file
(function () {
    'use strict';
    angular.module('app').controller('Alumniletters', Alumniletters)
    Alumniletters.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function Alumniletters($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.showreport = true;

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.pagination = false;
        $scope.currentPage = 1;
        $scope.export_flag = true;
        $scope.Print_flag = false;
        $scope.printstudents = [];
        $scope.printData = function (letterdiv) {
            var divid = "";
            if (letterdiv === "OBA") {
                divid = "printSectionIdl";
            }
            else if (letterdiv === "ENR") {
                divid = "printSectionId2";
            }
            else if (letterdiv === "ENR3") {
                divid = "printSectionId3";
            }
            else if (letterdiv === "ADR") {
                divid = "printSectionId4";
            }
            else if (letterdiv === "PTRN") {
                divid = "printSectionId5";
            }
            else if (letterdiv === "LFMB") {
                divid = "printSectionId6";
            }

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById(divid).innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/Alumniletters.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        };
        $scope.showreportd = false;
        $scope.rep = false;
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("Alumniletters/BindData", pageid).then(function (promise) {
                $scope.newuser1 = promise.newuser1;
                $scope.newuser2 = promise.newuser2;
                $scope.logopath = promise.logopath[0].logoname;
                console.log($scope.logopath);
            });
        };
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian; 
        };


        $scope.ShowReport = function () {
            if ($scope.asmaY_Id === '' || $scope.asmaY_Id === null) {
                swal("Please Select Academic Year");
            }
            else {
                $scope.letter_list = '';
                $scope.studlistdata = [];
                $scope.all = '';
                $scope.printstudents = [];
                $scope.searchValue = "";
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "logoname": $scope.reporttype
                };
                apiService.create("Alumniletters/ShowReport", data).then(function (promise) {
                    if (promise.searchstudentDetails.length > 0) {
                        $scope.reportone = false;
                        $scope.searchResult = promise.searchstudentDetails;
                        $scope.showreportd = true;
                        angular.forEach($scope.searchResult, function (objectt) {
                            if (objectt.AMST_PerStreet !== null && objectt.AMST_PerStreet !== "" && objectt.AMST_PerStreet.length > 0) {
                                var string = objectt.AMST_PerStreet;
                                if (string[string.length - 1] == ',') {
                                    var n = string.lastIndexOf(",");
                                    objectt.AMST_PerStreet = string.substring(0, n);
                                }
                                var so = string.substr(0, 1);
                                if (so == ',') {
                                    objectt.AMST_PerStreet = string.substring(1);
                                }
                            }
                            if (objectt.AMST_PerArea !== null && objectt.AMST_PerArea !== "" && objectt.AMST_PerArea.length > 0) {
                                var string1 = objectt.AMST_PerArea;
                                if (string1[string1.length - 1] == ',') {
                                    var n1 = string1.lastIndexOf(",");
                                    objectt.AMST_PerArea = string1.substring(0, n1);
                                }
                                var so1 = string1.substr(0, 1);
                                if (so1 == ',') {
                                    objectt.AMST_PerArea = string1.substring(1);
                                }
                            }
                        });
                        $scope.presentCountgrid = promise.searchstudentDetails.length;
                    }
                    else {
                        $scope.searchResult = {};
                        $scope.reportone = true;
                        swal("Records Not Found");
                    }
                });
            }
        };

        $scope.onchangeyear = function () {
            $scope.asmcL_Id = '';
            $scope.letter_list = '';
            $scope.searchResult = [];
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.export_flag = true;
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.searchResult, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                        $scope.export_flag = false;
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                    $scope.export_flag = true;
                }
            });
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                $scope.letterReport();
            }
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
                $scope.export_flag = false;
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printstudents.length > 0) {
                $scope.export_flag = false;
            }
            else {
                $scope.export_flag = true;
            }
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                $scope.letterReport();
            }
        };

        $scope.searchValue = '';
        $scope.filterValue = function () {
            return ($scope.AMST_FirstName + ' ' + $scope.AMST_MiddleName + ' ' + $scope.Amst_LastName).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_AdmNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.asmcl_classname).indexOf($scope.searchValue) >= 0
                || ($scope.asmc_sectionname).indexOf($scope.searchValue) >= 0 ||
                ($scope.classes).indexOf($scope.searchValue) >= 0;
        };

        $scope.presentCountgrid = 0; $scope.AbsentCountgrid = 0;
        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.submitted = false;
        $scope.propertyName = 'AMST_FirstName';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.printDataadd = function () {
            var innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.CurrentDate = new Date();
        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.FromDate = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.reportdetails = "";
            $scope.submitted = false;
            $scope.searchResult = {};
            $scope.IsHiddendown = false;
            $scope.export_flag = true;
            $scope.myform.$setPristine();
            $scope.myform.$setUntouched();
            $scope.studlistdata = [];
            $scope.letter_list = '';
            $scope.all = '';
        };

        $scope.letterReport = function () {
            if ($scope.myform.$valid) {
                if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                    $scope.studlistdata = [];
                    angular.forEach($scope.searchResult, function (ty) {
                        if (ty.selected) {
                            $scope.studlistdata.push({
                                amsT_Id: ty.amsT_Id,
                            });
                        }
                    });

                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "letter_list": $scope.letter_list,
                        "logoname": $scope.reporttype,
                        studlistdata: $scope.studlistdata
                    };

                    apiService.create("Alumniletters/letterReport", data).then(function (promise) {
                        $scope.export_flag = false;
                        if (promise.searchstudentDetails2.length > 0) {
                            $scope.searchstudentDetails2 = promise.searchstudentDetails2;
                            $scope.amsT_FirstName = promise.searchstudentDetails2[0].amsT_FirstName;
                            $scope.AMST_PerStreet = promise.searchstudentDetails2[0].AMST_PerStreet;
                            $scope.AMST_PerArea = promise.searchstudentDetails2[0].AMST_PerArea;
                            $scope.AMST_PerCity = promise.searchstudentDetails2[0].AMST_PerCity;
                            $scope.AMST_PerPincode = promise.searchstudentDetails2[0].AMST_PerPincode;
                        }
                    });
                }
                else {
                    swal("Please Select Record To Show Report");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.onchangetype = function () {
            $scope.export_flag = true;
            $scope.showreportd = false;
            $scope.searchResult = [];
        };
    }
})();