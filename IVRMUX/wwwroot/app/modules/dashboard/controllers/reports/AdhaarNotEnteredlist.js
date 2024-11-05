(function () {
    'use strict';
    angular.module('app').controller('AdharNotEnteredListController', AdharNotEnteredListController)

    AdharNotEnteredListController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function AdharNotEnteredListController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.AdhaarNotEnteredList = {};
        $scope.usrname = localStorage.getItem('username');
        
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

       

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters = 0;
        var copty = "";

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
        }

        $scope.imgname = logopath;

        $scope.StuAttRptDropdownList = function () {
            $scope.classname = false;
            $scope.sectionname = false;

            apiService.get("AdharNotEnteredList/getdetails/").then(function (promise) {

                $scope.all = true;
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;

                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }
                $scope.imgname = promise.photopathname;
                $scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
                $scope.monthDropdown = promise.monthList;
            });
        };

        $scope.getDataByType = function (type) {

            if (type == 1) {
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.classname = false;
                $scope.sectionname = false;
                $scope.gridflag = false;
            }
            if (type == 2) {
                $scope.classname = true;
                $scope.sectionname = true;
                $scope.gridflag = false;
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
            }
        };

        $scope.getclass = function () {
            $scope.gridflag = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("AdharNotEnteredList/getclass", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;
            });

        };

        $scope.getsection = function () {
            $scope.gridflag = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("AdharNotEnteredList/getsection", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            });
        };

        $scope.savetmpldata = function () {
          
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.vals = [];
            if ($scope.type23 == 1) {
                $scope.asmcL_Id = 0;
                $scope.asmC_Id = 0;
            }

            var data = {
                "type": $scope.type23,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMC_Id": $scope.asmC_Id
            };


            if ($scope.myForm.$valid) {

                if ($scope.type23 == 1) {
                    $scope.vals = [];
                    if ($scope.asmaY_Id === undefined || $scope.asmaY_Id === null) {

                        swal("Select The Fields !");
                    }
                    else {
                        apiService.create("AdharNotEnteredList/getAttendetails", data)
                            .then(function (promise) {

                                $scope.date = new Date();
                                if (promise.adhaarNotEnteredList !== null) {

                                    if (promise.adhaarNotEnteredList.length === 0) {
                                        swal("No Record Found");
                                        $scope.gridflag = false;
                                        $scope.print_flag = true;
                                        $scope.excel_flag = true;
                                    }
                                    else {
                                        $scope.students = promise.adhaarNotEnteredList;
                                        $scope.presentCountgrid = $scope.students.length;
                                        for (var i = 0; i < promise.adhaarNotEnteredList.length; i++) {
                                            var name = promise.adhaarNotEnteredList[i].AMST_FirstName;
                                            if (promise.adhaarNotEnteredList[i].AMST_MiddleName !== null) {
                                                name += " " + promise.adhaarNotEnteredList[i].AMST_MiddleName;
                                            }
                                            if (promise.adhaarNotEnteredList[i].Amst_LastName !== null) {
                                                name += " " + promise.adhaarNotEnteredList[i].Amst_LastName;
                                            }
                                            $scope.vals.push(name);
                                        }
                                        angular.forEach($scope.vals, function (v, k) {
                                            $scope.angularData.nameList.push({
                                                'fullname': v
                                            });
                                        });

                                        var j = 0;
                                        angular.forEach($scope.students, function (obj) {
                                            //Using bracket notation
                                            obj["fullname"] = $scope.angularData.nameList[j].fullname;
                                            j++;
                                        });

                                        angular.forEach($scope.students, function (objectt) {
                                            if (objectt.fullname.length > 0) {
                                                var string = objectt.fullname;
                                                objectt.namme = string.replace(/  +/g, ' ');
                                            }
                                        });
                                        $scope.gridflag = true;
                                        $scope.print_flag = false;
                                        $scope.excel_flag = false;

                                        angular.forEach($scope.yearDropdown, function (y) {
                                            if (parseInt(y.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                                $scope.yearname = y.asmaY_Year;
                                            }
                                        });
                                    }
                                }
                                else {

                                    swal("No record Found !");
                                    $scope.gridflag = false;
                                    $scope.print_flag = true;
                                    $scope.excel_flag = true;
                                }
                            });
                    }
                }

                if ($scope.type23 == 2) {
                    $scope.vals = [];
                    if ($scope.asmaY_Id === undefined || $scope.asmaY_Id === null || $scope.asmcL_Id === undefined || $scope.asmcL_Id === null || $scope.asmC_Id === undefined || $scope.asmC_Id === null) {

                        swal("Select The Fields !");
                    }
                    else {
                        apiService.create("AdharNotEnteredList/getAttendetails", data)
                            .then(function (promise) {
                                if (promise.adhaarNotEnteredList !== null) {

                                    if (promise.adhaarNotEnteredList.length == 0) {
                                        swal("No record Found !");
                                        $scope.gridflag = false;
                                        $scope.print_flag = true;
                                        $scope.excel_flag = true;
                                    } else {
                                        $scope.students = promise.adhaarNotEnteredList;
                                        $scope.presentCountgrid = $scope.students.length;
                                        for (var i = 0; i < promise.adhaarNotEnteredList.length; i++) {

                                            var name = promise.adhaarNotEnteredList[i].AMST_FirstName;
                                            if (promise.adhaarNotEnteredList[i].AMST_MiddleName !== null) {
                                                name += " " + promise.adhaarNotEnteredList[i].AMST_MiddleName;
                                            }
                                            if (promise.adhaarNotEnteredList[i].Amst_LastName !== null) {
                                                name += " " + promise.adhaarNotEnteredList[i].Amst_LastName;
                                            }
                                            $scope.vals.push(name);
                                        }
                                        angular.forEach($scope.vals, function (v, k) {
                                            $scope.angularData.nameList.push({
                                                'fullname': v
                                            });
                                        });

                                        var j = 0;
                                        angular.forEach($scope.students, function (obj) {
                                            //Using bracket notation
                                            obj["fullname"] = $scope.angularData.nameList[j].fullname;
                                            j++;
                                        });

                                        angular.forEach($scope.students, function (objectt) {
                                            if (objectt.fullname.length > 0) {
                                                var string = objectt.fullname;
                                                objectt.namme = string.replace(/  +/g, ' ');
                                            }
                                        });
                                        $scope.gridflag = true;
                                        $scope.print_flag = false;
                                        $scope.excel_flag = false;
                                        angular.forEach($scope.yearDropdown, function (y) {
                                            if (parseInt(y.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                                $scope.yearname = y.asmaY_Year;
                                            }
                                        });
                                    }
                                }
                                else {
                                    swal("No record Found !");
                                    $scope.gridflag = false;
                                    $scope.print_flag = true;
                                    $scope.excel_flag = true;
                                }
                            });
                    }
                }
            }

            else {
                $scope.submitted = true;
            }
        };

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_AdmNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_RegistrationNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMCL_ClassName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMC_SectionName)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.printData = function () {

            //apiService.get("AttendanceReport/getdetails/").then(function (promise) {
            //    
            //    if (promise.photopathname != null) {
            //        $scope.imgname = promise.photopathname;
            //    }
            //});


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

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.submitted = false;

        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };





        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.hide = function () {
            $scope.gridflag = false;
        };

    }
})();
