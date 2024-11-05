(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentAddressBook2Controller', StudentAddressBook2Controller)

    StudentAddressBook2Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function StudentAddressBook2Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {


        $scope.grid_flag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.printdatatable = [];

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
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

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 === true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 || (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 || (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 || (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
        };



        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.items.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("grid_print").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet"  href="css/print/Admission/Student Book/StudentBook2Pdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.yeardis = true;
        $scope.classdis = true;
        $scope.secdis = true;
        $scope.stdnamedis = true;

        $scope.BindData = function () {
            apiService.get("StudentAddressBook2/getinitialdata/2").then(function (promise) {
                if (promise !== null) {
                    $scope.yeardropdown = promise.accyear;
                    $scope.classdropdown = promise.classlist;
                    $scope.sectiondropdown = promise.sectionlist;
                    // $scope.studentdropdown = promise.studentlist;
                }
            });
        };



        $scope.classchange = function (amcl_id) {
            apiService.get("StudentAddressBook2/classchange/", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectiondropdown = promise.sectionlist;
                    $scope.studentdropdown = promise.studentlist;
                }
            });
        };

        $scope.yearchange = function (asmaY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "flag": $scope.flag
            };

            apiService.get("StudentAddressBook2/yearchange/", data).then(function (promise) {
                if (promise !== null) {

                    $scope.studentdropdown = promise.studentlist;
                }
            });
        };

        $scope.sectionchange = function (asmC_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMC_Id": $scope.asmC_Id,
                "flag": $scope.flag
            };
            apiService.create("StudentAddressBook2/sectionchange/", data).then(function (promise) {
                if (promise !== null) {

                    $scope.studentdropdown = promise.studentlist;
                }
            });
        };

        $scope.ShowReport = function (flag, asmaY_Id, asmcL_Id, asmC_Id, amsT_Id, sall, dob, email, admno, tc, ph) {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.all2 = "";
            var data = {
                "asmaY_Id": $scope.asmaY_Id,
                "asmcL_Id": $scope.asmcL_Id,
                "asmC_Id": $scope.asmC_Id,
                "amsT_Id": $scope.amsT_Id,
                "flag": $scope.flag,
                "sall": $scope.sall
            };
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                apiService.create("StudentAddressBook2/getdetails", data).
                    then(function (promise) {

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


        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || angular.lowercase(obj.AMST_Photoname).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_RegistrationNo).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_AdmNo).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_FirstName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.ASMCL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_Sex).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_FatherName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_MotherName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.AMST_emailId).indexOf(angular.lowercase($scope.searchValue)) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0;
        };        

        $scope.checkemail = false;
        $scope.checkph = false;
        $scope.checkdob = false;
        $scope.checkadmno = false;
        $scope.checkper = false;
        $scope.checktem = false;
        $scope.checktc = false;

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
                    // clean the print section before adding new content
                    printSection.innerHTML = '';
                };
            }

            function printElement(elem) {
                // clones the element you want to print
                var domClone = elem.cloneNode(true);
                printSection.appendChild(domClone);
            }

            return {
                link: link,
                restrict: 'A'
            };
        }

        $scope.radio_but_switch = function () {
            if ($scope.sall == 1) {
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
            if ($scope.sall == 0) {
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
            if ($scope.sall == 0) {
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
            if ($scope.sall == 1) {
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

    }
})();
