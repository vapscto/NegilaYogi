(function () {
    'use strict';
    angular.module('app').controller('BloodGroupWiseStudentDetailsReport', BloodGroupWiseStudentDetailsReport)

    BloodGroupWiseStudentDetailsReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', 'Excel', '$http', 'superCache', '$filter', '$q', '$timeout']
    function BloodGroupWiseStudentDetailsReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, Excel, $http, superCache, $filter, $q, $timeout) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;

        $scope.submitted = false;
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.search = "";

        $scope.bloodgrouplist = {};
        $scope.bloodgrouplist = [
            { id: 2, name: 'A+' },
            { id: 3, name: 'A-' },
            { id: 4, name: 'B+' },
            { id: 5, name: 'B-' },
            { id: 6, name: 'AB+' },
            { id: 7, name: 'AB-' },
            { id: 8, name: 'O+' },
            { id: 9, name: 'O-' },
            { id: 10, name: 'A1+ve' },
            { id: 11, name: 'A1-ve' },
            { id: 12, name: 'B1+ve' },
            { id: 13, name: 'B1-ve' },
            { id: 14, name: 'A2+ve' },
            { id: 15, name: 'A1B+ve' },
            { id: 16, name: 'A2B+ve' },
            { id: 17, name: 'A1B2' },
            { id: 18, name: 'A1B-ve' }
        ];

        //=============================page load
        $scope.loaddata = function () {
            $scope.usercheckB = true;
            angular.forEach($scope.bloodgrouplist, function (itm) {
                itm.blood = true;
            });
            var pageid = 2;
            apiService.getURI("BloodGroupWiseStudentDetailsReport/loaddata", pageid).then(function (promise) {
                $scope.allacademicyear = promise.allacademicyear;
                $scope.sectionlist = promise.sectionlist;
            });
        };

        //================================get course list
        $scope.getcourse = function () {
            angular.forEach($scope.branchlist, function (itm) {
                if (itm.selected) {
                    //dd
                }
                $scope.AMSE_Id = '';
                $scope.semesterlist = [];
                $scope.branchlist = [];
                $scope.usercheckC = '';
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.studentDetails = [];
                $scope.ACMS_Id = '';
                $scope.AMCO_Id = '';
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("BloodGroupWiseStudentDetailsReport/getcourse", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        };

        $scope.getbranch = function () {
            angular.forEach($scope.branchlist, function (itm) {
                if (itm.selected) {
                    //dd
                }
                $scope.AMSE_Id = '';
                $scope.semesterlist = [];
                $scope.branchlist = [];
                $scope.usercheckC = '';
                $scope.selected = '';
                $scope.searchchkbx1 = '';
                $scope.studentDetails = [];
                $scope.ACMS_Id = '';
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_ID": $scope.AMCO_Id
            };
            apiService.create("BloodGroupWiseStudentDetailsReport/getbranch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };


        $scope.getsemester = function () {
            $scope.studentDetails = [];
            $scope.branchess = [];
            angular.forEach($scope.branchlist, function (cls) {
                if (cls.selected === true) {
                    $scope.branchess.push(cls);
                }
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_ID": $scope.AMCO_Id,
                branchess: $scope.branchess
            };
            apiService.create("BloodGroupWiseStudentDetailsReport/getsemester", data).then(function (promise) {
                $scope.semesterlist = promise.semesterlist;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.branchlist.some(function (item) {
                return item.selected;
            });
        };

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
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



        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.branchlist.every(function (role) {
                return role.selected;
            });
        };

        //---------all checkbox Select...
        $scope.all_checkC = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.branchlist, function (role) {
                role.selected = toggleStatus;
            });
            $scope.getsemester();
        };

        $scope.isOptionsRequiredB = function () {
            return !$scope.bloodgrouplist.some(function (item) {
                return item.blood;
            });
        };

        $scope.reportdata = true;
        //=======selection of checkbox....
        $scope.togchkbxB = function () {
            $scope.usercheckB = $scope.bloodgrouplist.every(function (role) {
                return role.blood;
            });
        };
        //---------all checkbox Select...
        $scope.all_checkB = function (all) {
            $scope.usercheckB = all;
            var toggleStatus = $scope.usercheckB;
            angular.forEach($scope.bloodgrouplist, function (role) {
                role.blood = toggleStatus;
            });
        };

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.newclsist = [];
                angular.forEach($scope.sectionlist, function (cc) {
                    if (parseInt($scope.ACMS_Id) === 0) {
                        $scope.newclsist.push(cc);
                    }
                    else if (parseInt($scope.ACMS_Id) === cc.acmS_Id) {
                        $scope.newclsist.push(cc);
                    }
                });

                $scope.branchess = [];
                angular.forEach($scope.branchlist, function (cls) {
                    if (cls.selected === true) {
                        $scope.branchess.push(cls);
                    }
                });

                $scope.blood1 = [];
                angular.forEach($scope.bloodgrouplist, function (cls) {
                    if (cls.blood === true) {
                        $scope.blood1.push(cls);
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    branchess: $scope.branchess,
                    "clsidlist": $scope.newclsist,
                    "blood1": $scope.blood1,
                    "AMSE_Id": $scope.AMSE_Id
                };

                apiService.create("BloodGroupWiseStudentDetailsReport/Report", data).then(function (promise) {
                    $scope.year = promise.year;
                    $scope.sem = promise.sem;

                    $scope.year1 = promise.year[0].asmaY_Year;
                    $scope.sem1 = promise.sem[0].amsE_SEMName;
                    if (promise.studentDetails.length > 0) {
                        $scope.reportdata = false;
                        $scope.studentDetails = promise.studentDetails;
                        $scope.imgname = $scope.studentDetails[0].logo;
                    }
                    else {
                        swal("No Students Are Available");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
    }
})();