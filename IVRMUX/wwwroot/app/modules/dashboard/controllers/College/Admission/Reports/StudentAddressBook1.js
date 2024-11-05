(function () {
    'use strict';
    angular.module('app').controller('StudentAddressBookController', StudentAddressBookController)

    StudentAddressBookController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function StudentAddressBookController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.print_flag = true;
        $scope.submitted = false;

        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var copty;
        $scope.searchValue = '';

        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];
        $scope.coptyright = copty;
        $scope.ddate = new Date();

        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("StudentAddressBook/loaddata", pageid).then(function (promise) {
                $scope.academiclist = promise.academiclist;
            });
        };

        $scope.getcourse = function () {
            $scope.AMCO_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";     
            $scope.catreport = true;
            $scope.detail_checked = false;       
            $scope.branchlist = [];
            $scope.alldata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("StudentAddressBook/getcourse", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        };

        $scope.getbranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.catreport = true;
            $scope.detail_checked = false;
            $scope.branchlist = [];
            $scope.alldata = [];       
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("StudentAddressBook/getbranch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.albumNameArraycolumn1 = [];
            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn1.push(role);
            });

            if ($scope.albumNameArraycolumn1.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "Temp_branchDTOreport": $scope.albumNameArraycolumn1
                };
                apiService.create("StudentAddressBook/onselectBranch", data).then(function (promise) {
                    $scope.semlist = promise.semlist;
                });
            }
        };

        $scope.getsection = function () {
            $scope.ACMS_Id = "";
            $scope.obj = {};       
            $scope.albumNameArraycolumn3 = [];
            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn3.push(role);
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "Temp_branchDTOreport": $scope.albumNameArraycolumn3,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("StudentAddressBook/getsection", data).then(function (promise) {
                $scope.seclist23 = promise.seclist;
                if ($scope.seclist23.length > 0) {
                    $scope.seclist = promise.seclist;
                } else {
                    swal("No Section Is Found For Selected List");
                }
            });
        };


        $scope.print_flag = true;

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.alldata = [];
                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.branchlist, function (role) {
                    if (!!role.ambid) $scope.albumNameArraycolumn3.push(role);
                });

                if ($scope.ACMS_Id === undefined || $scope.ACMS_Id === null || $scope.ACMS_Id === "") {
                    swal("Selection Section");
                    return;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "Temp_branchDTOreport": $scope.albumNameArraycolumn3
                };

                apiService.create("StudentAddressBook/Report", data).then(function (promise) {
                    if (promise.reportdata !== null && promise.reportdata.length !== 0) {
                        $scope.print_flag = false;
                        $scope.alldata = promise.reportdata;

                        angular.forEach($scope.academiclist, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.courselist, function (dd) {
                            if (dd.amcO_Id === parseInt($scope.amcO_Id)) {
                                $scope.coursename = dd.amcO_CourseName;
                            }
                        });

                        angular.forEach($scope.semlist, function (dd) {
                            if (dd.amsE_Id === parseInt($scope.AMSE_Id)) {
                                $scope.semname = dd.amsE_SEMName;
                            }
                        });
                    }
                    else {
                        swal("Record Not Found");
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.branchlist, function (itm) {
                itm.ambid = toggleStatus;
            });

            $scope.albumNameArraycolumn1 = [];
            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn1.push(role);
            });

            $scope.onselectBranch(1, 2, 3);
        };
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest1 = [];

        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.branchlist.every(function (itm) { return itm.selected; });
            if (role1.selected === true) {
                $scope.albumNameArraycolumn.push(role1);
                var newCol = { AMB_Id: role1.amB_Id, checked: true, AMB_BranchName: role1.amB_BranchName };
                $scope.columnsTest1.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest1.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
            }
            $scope.onselectBranch(1, 2, 3);
        };

        $scope.isOptionsRequired = function () {
            return !$scope.branchlist.some(function (options) {
                return options.ambid;
            });
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Student Address Book');
            var excelname = "Student Address Book.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }
})();

