(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgAttendanceSMSDetailsReport', ClgAttendanceSMSDetailsReport)

    ClgAttendanceSMSDetailsReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ClgAttendanceSMSDetailsReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ClgAttendanceSMSDetailsReport/loaddata", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.sectionlist = promise.sectionlist;
            })
        }

        $scope.getcourse = function () {        
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("ClgAttendanceSMSDetailsReport/getcourse", data).then(function (promise) {
                $scope.courselist = promise.getcourse;
            })
        }
        $scope.getbranch = function () {         
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            }
            apiService.create("ClgAttendanceSMSDetailsReport/getbranch", data).then(function (promise) {
                $scope.branch = promise.getbranch;
            })
        }
        $scope.getsemester = function () {        
            $scope.selectedbranchlist = [];
            angular.forEach($scope.branch, function (branch) {      
                if (branch.cls == true) {
                    $scope.selectedbranchlist.push({ AMB_Id: branch.amB_Id });
                }
            })
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,              
                selectedbranchlist: $scope.selectedbranchlist,
            }
            apiService.create("ClgAttendanceSMSDetailsReport/getsemester", data).then(function (promise) {
                $scope.semesterlist = promise.getsemester;
            })
        }
        $scope.search = "";
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.showdetails = function () {
            if ($scope.myForm.$valid) {
                $scope.selectedbranchlist = [];
                $scope.selectedsemesterlist = [];
                angular.forEach($scope.branch, function (branch) {
                    if (branch.cls == true) {
                        $scope.selectedbranchlist.push({ AMB_Id: branch.amB_Id });
                    }
                })
                angular.forEach($scope.semesterlist, function (type) {
                    if (type.select1 == true) {
                        $scope.selectedsemesterlist.push({ AMSE_Id: type.amsE_Id });
                    }
                })               
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    selectedbranchlist: $scope.selectedbranchlist,
                    selectedsemesterlist: $scope.selectedsemesterlist,
                }
                apiService.create("ClgAttendanceSMSDetailsReport/showdetails", data).then(function (promise) {
                    if (promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;
                    }
                    else {
                        swal("No Record Found");
                    }                    
                })
            }
            else {
                $scope.Submitted = true;
            }
        }
        $scope.searchchkbx3 = "";
        $scope.searchchkbxtype = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.all_check3 = function () {
            var toggleStatus = $scope.usercheck3;
            angular.forEach($scope.branch, function (itm) {
                itm.cls = toggleStatus;
            });
            $scope.getsemester();
        };
        $scope.all_checktype = function () {
            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.semesterlist, function (itmtype) {
                itmtype.select1 = checkStatus;
            })
        }
        $scope.togchkbx3 = function () {
            $scope.usercheck3 = $scope.branch.every(function (options) {
                return options.cls;
            });
            $scope.getsemester();
        };
        $scope.togchkbxtype = function () {
            $scope.userchecktype = $scope.semesterlist.every(function (optionstype) {
                return optionstype.select1;
            })
        }
        $scope.isOptionsRequired3 = function () {
            return !$scope.branch.some(function (options) {
                return options.cls;
            })
        }
        $scope.isOptionsRequiredtype = function () {
            return !$scope.semesterlist.some(function (options) {
                return options.select1;
            })
        }
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.filterchkbxtype = function (obj) {
            return (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;
        }
        $scope.Submitted = false;
        $scope.interacted = function () {
            return $scope.Submitted;
        }
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
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
        }
        $scope.cancel = function () {
            $state.reload();
        }       
    }
})();
