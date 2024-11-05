(function () {
    'use strict';
    angular.module('app').controller('StudentAddressBook2Controller', StudentAddressBook2Controller)

    StudentAddressBook2Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function StudentAddressBook2Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.print_flag = true;
        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.searchValue = '';

        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var copty;       

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
            $scope.courselist = [];
            $scope.AMB_Id = "";
            $scope.branchlist = [];
            $scope.AMSE_Id = "";
            $scope.semlist = [];
            $scope.ACMS_Id = "";
            $scope.seclist = [];
            $scope.studentlist = [];           
            $scope.all = false;
            $scope.alldata = [];
            $scope.searchchkbx = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("StudentAddressBook/getcourse", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        };

        $scope.getbranch = function () {
            $scope.AMB_Id = "";
            $scope.branchlist = [];
            $scope.AMSE_Id = "";
            $scope.semlist = [];
            $scope.ACMS_Id = "";
            $scope.seclist = [];
            $scope.studentlist = [];
            $scope.all = false;
            $scope.alldata = [];
            $scope.searchchkbx = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("StudentAddressBook/getbranch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.getsemester = function () {
            $scope.AMSE_Id = "";
            $scope.semlist = [];
            $scope.ACMS_Id = "";
            $scope.seclist = [];
            $scope.studentlist = [];
            $scope.all = false;
            $scope.alldata = [];
            $scope.searchchkbx = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("StudentAddressBook/getsemester", data).then(function (promise) {
                $scope.semlist = promise.semlist;
            });
        };

        $scope.getsection = function () {
            $scope.ACMS_Id = "";
            $scope.seclist = [];
            $scope.studentlist = [];
            $scope.all = false;
            $scope.alldata = [];
            $scope.searchchkbx = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
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

        $scope.getstudent = function () {
            $scope.studentlist = [];
            $scope.all = false;
            $scope.alldata = [];
            $scope.searchchkbx = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("StudentAddressBook/getstudent", data).then(function (promise) {
                if (promise.studentlist !== null && promise.studentlist.length > 0) {
                    $scope.studentlist = promise.studentlist;
                    $scope.all = true;
                    $timeout(function () { $scope.OnClickAll(); }, 200);
                } else {
                    swal("No Students Found For Selected List");
                }
            });
        };    

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.alldata = [];
                $scope.studentlist_temp = [];
                angular.forEach($scope.studentlist, function (role) {
                    if (role.checkedsub) {
                        $scope.studentlist_temp.push({ AMCST_Id: role.amcsT_Id });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,                    
                    "Temp_StudentListDto": $scope.studentlist_temp
                };

                apiService.create("StudentAddressBook/AddressBookFormat2", data).then(function (promise) {
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

        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlist, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlist.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
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

