(function () {
    'use strict';
    angular.module('app').controller('StudentAddressBook2Controller', StudentAddressBook2Controller)

    StudentAddressBook2Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function StudentAddressBook2Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {


        $scope.grid_flag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.printdatatable = [];

        $scope.checkemail = false;
        $scope.checkph = false;
        $scope.checkdob = false;
        $scope.checkadmno = false;
        $scope.checkper = false;
        $scope.checktem = false;
        $scope.checktc = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.yeardis = true;
        $scope.classdis = true;
        $scope.secdis = true;
        $scope.stdnamedis = true;

        $scope.BindData = function () {
            apiService.get("StudentAddressBook2/getinitialdata/2").then(function (promise) {
                if (promise !== null) {
                    $scope.yeardropdown = promise.accyear;
                }
            });
        };

        $scope.yearchange = function (asmaY_Id) {
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.studentdropdown = [];
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.items = [];
            $scope.all1 = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "flag": $scope.flag
            };

            apiService.create("StudentAddressBook2/yearchangenew", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classdropdown = promise.classlist;
                }
            });
        };

        $scope.classchange = function (amcl_id) {
            $scope.asmC_Id = "";
            $scope.studentdropdown = [];
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.items = [];
            $scope.all1 = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "flag": $scope.flag,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("StudentAddressBook2/classchangenew", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectiondropdown = promise.sectionlist;
                }
            });
        };

        $scope.sectionchange = function (asmC_Id) {
            $scope.studentdropdown = [];
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.items = [];
            $scope.all1 = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMC_Id": $scope.asmC_Id,
                "flag": $scope.flag
            };
            apiService.create("StudentAddressBook2/sectionchangenew", data).then(function (promise) {
                if (promise !== null) {
                    $scope.studentdropdown = promise.studentlist;
                    if ($scope.studentdropdown !== null && $scope.studentdropdown.length > 0) {
                        angular.forEach($scope.studentdropdown, function (dd) {
                            dd.amsT_IdSelected = true;
                        });

                        $scope.all1 = true;
                    } else {
                        swal("No Students Found");

                    }
                }
            });
        };

        $scope.ShowReport = function (flag, asmaY_Id, asmcL_Id, asmC_Id, amsT_Id, sall, dob, email, admno, tc, ph) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.printdatatable = [];
                $scope.searchValue = "";
                $scope.all2 = "";
                var selected_student = [];
                $scope.items = [];
                angular.forEach($scope.studentdropdown, function (stu3) {
                    if (stu3.amsT_IdSelected === true) {
                        selected_student.push(stu3);
                    }
                });

                var data = {
                    "asmaY_Id": $scope.asmaY_Id,
                    "asmcL_Id": $scope.asmcL_Id,
                    "asmC_Id": $scope.asmC_Id,
                    "amsT_Id": $scope.amsT_Id,
                    "flag": $scope.flag,
                    "sall": "1",
                    "studentlisttemp": selected_student
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("StudentAddressBook2/getdetailsnew", data).then(function (promise) {
                    $scope.items = [];
                    $scope.items = promise.getdetails;
                    if (promise.getdetails === null || promise.getdetails.length === 0 || promise.getdetails === undefined) {
                        swal('No Records Found.....!');
                        $scope.grid_flag = false;
                        $scope.print_flag = true;
                        $scope.excel_flag = true;
                        return;
                    }
                    else {
                        $scope.grid_flag = true;
                        $scope.print_flag = false;
                        $scope.excel_flag = false;
                        $scope.items = promise.getdetails;
                    }
                });
            }
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.emailfunction = function (email) {
            if (email === true) {
                $scope.checkemail = true;
            }
            else {
                $scope.checkemail = false;
            }
        };

        $scope.phfunction = function (ph) {
            if (ph === true) {
                $scope.checkph = true;
            }
            else {
                $scope.checkph = false;
            }
        };

        $scope.dobfunction = function (dob) {
            if (dob === true) {
                $scope.checkdob = true;
            }
            else {
                $scope.checkdob = false;
            }
        };

        $scope.admnofunction = function (admno) {
            if (admno === true) {
                $scope.checkadmno = true;
            }
            else {
                $scope.checkadmno = false;
            }
        };

        $scope.perfunction = function (per) {
            if (per === true) {
                $scope.checkper = true;
            }
            else {
                $scope.checkper = false;
            }
        };

        $scope.temfunction = function (tem) {
            if (tem === true) {
                $scope.checktem = true;
            }
            else {
                $scope.checktem = false;
            }
        };

        $scope.tcfunction = function (tc) {
            if (tc === true) {
                $scope.checktc = true;
            }
            else {
                $scope.checktc = false;
            }
        };

        $scope.radio_but_switch = function () {
            if (parseInt($scope.sall) === 1) {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";

                $scope.email = false;
                $scope.dob = false;
                $scope.ph = false;
                $scope.tc = false;

                $scope.admno = false;
                $scope.tem = false;
                $scope.grid_flag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();


            }
            if (parseInt($scope.sall) === 0) {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.amsT_Id = "";
                $scope.email = false;
                $scope.dob = false;
                $scope.ph = false;
                $scope.tc = false;

                $scope.admno = false;
                $scope.tem = false;
                $scope.grid_flag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.leftoractive = function () {
            if (parseInt($scope.sall) === 0) {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.amsT_Id = "";
                $scope.email = false;
                $scope.dob = false;
                $scope.ph = false;
                $scope.tc = false;

                $scope.admno = false;
                $scope.tem = false;
                $scope.grid_flag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            if (parseInt($scope.sall) === 1) {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";

                $scope.email = false;
                $scope.dob = false;
                $scope.ph = false;
                $scope.tc = false;

                $scope.admno = false;
                $scope.tem = false;
                $scope.grid_flag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

            }
        };

        $scope.Toggle_header1 = function () {
            $scope.gridlength = false;
            $scope.printdata = false;
            var toggleStatus1 = $scope.all1;
            angular.forEach($scope.studentdropdown, function (itm) {
                itm.amsT_IdSelected = toggleStatus1;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.addColumn1 = function (role) {
            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.all1 = $scope.studentdropdown.every(function (itm) { return itm.amsT_IdSelected; });
            if (role.amsT_IdSelected === true) {
                $scope.albumNameArraycolumn.push(role);
                $scope.columnsTest.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.studentdropdown.some(function (options) {
                return options.amsT_IdSelected;
            });
        };


        $scope.printData = function () {
            var data;
            var innerContents;
            var popupWinindow;
            if ($scope.type === "address") {
                data = 'SRKVSStudentAddressBook';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank', 'padding-top=0%;');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else if ($scope.type === "addressprint") {
                data = 'SRKVSStudentAddressBookprint';
                innerContents = document.getElementById(data).innerHTML;
                popupWinindow = window.open('_blank', 'padding-top=10%;');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;

        $scope.exportToExcel = function (table) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(table, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        function printDirective() {
            var printSection = document.getElementById('printSection');
            // if there is no printing section, create one
            if (!printSection) {
                printSection = document.createElement('div');
                printSection.id = 'printSection';
                document.body.appendChild(printSection);
            }

            function link(scope, element, attrs) {
                element.on('click', function () {
                    var elemToPrint = document.getElementById(attrs.printElementId);
                    if (elemToPrint) {
                        printElement(elemToPrint);
                        window.print();
                    }
                });

                window.onafterprint = function () {
                    printSection.innerHTML = '';
                };
            }

            function printElement(elem) {
                var domClone = elem.cloneNode(true);
                printSection.appendChild(domClone);
            }

            return {
                link: link,
                restrict: 'A'
            };
        }
    }
})();
