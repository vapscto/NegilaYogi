

(function () {
    'use strict';
    angular
        .module('app')
        .controller('YearlyProgramReport', SportHouseCommitteeReportController)

    SportHouseCommitteeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportHouseCommitteeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = '';
        $scope.Binddata = function () {
            var pageid = 2;
            apiService.getURI("OnlineProgramReport/getyearlyprogram", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.typelist = promise.typelist;
                $scope.activitylist = promise.activitylist;
                $scope.levellist = promise.levellist;
            })
        }
        $scope.showdetails = function () {
            $scope.selectedyearlist = [];
            $scope.selectedtypelist = [];
            $scope.selectedactivitylist = [];
            $scope.selectedlevellist = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.yearlist, function (year) {
                    if (year.select == true) {
                        $scope.selectedyearlist.push({ asmaY_Id: year.asmaY_Id });
                    }
                });
                angular.forEach($scope.typelist, function (type) {
                    if (type.select1 == true) {
                        $scope.selectedtypelist.push({ prmtY_Id: type.prmtY_Id });
                    }
                })
                angular.forEach($scope.activitylist, function (acti) {
                    if (acti.select2 == true) {
                        $scope.selectedactivitylist.push({ pryrA_Id: acti.pryrA_Id });
                    }
                })
                angular.forEach($scope.levellist, function (level) {
                    if(level.select3 == true) {
                        $scope.selectedlevellist.push({ prmtlE_Id: level.prmtlE_Id });
                    }
                })
                //var date1 = $scope.PRYR_StartDate == null ? "" : $filter('date')($scope.PRYR_StartDate, "yyyy-MM-dd");
                //var date2 = $scope.PRYR_EndDate == null ? "" : $filter('date')($scope.PRYR_EndDate, "yyyy-MM-dd");
                var date1 = new Date($scope.PRYR_StartDate).toDateString();
                var date2= new Date($scope.PRYR_EndDate).toDateString();
                var data = {
                    "PRYR_StartDate": date1,
                    "PRYR_EndDate": date2,
                    selectedyearlist: $scope.selectedyearlist,
                    selectedtypelist: $scope.selectedtypelist,
                    selectedactivitylist: $scope.selectedactivitylist,
                    selectedlevellist: $scope.selectedlevellist,
                   
                }
                apiService.create("OnlineProgramReport/getYearlyProgramReport", data).then(function (promise) {
                  
                    if (promise.reportlist != null && promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;
                    }
                    else {
                        swal("Record Not Found");
                        $state.reload();
                    }
                })
            }
        }
      
        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.searchchkbxtype = "";
        $scope.searchchkbxacti = "";
        $scope.searchchkbxlevel = "";
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.all_check = function () {
         
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.yearlist, function (itm) {
                itm.select = checkStatus;
            });
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
        $scope.all_checktype = function () {
           
            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.typelist, function (itmtype) {
                itmtype.select1 = checkStatus;
            })
        }

        $scope.all_checkacti = function () {
            var checkStatus = $scope.usercheckacti;
            angular.forEach($scope.activitylist, function (itmacti) {
                itmacti.select2 = checkStatus;
            })
        }
        $scope.all_checklevel = function () {
            var checkStatus = $scope.userchecklevel;
            angular.forEach($scope.levellist, function (itmlevel) {
                itmlevel.select3 = checkStatus;
            })
        }
        $scope.togchkbx = function () {
      
            $scope.usercheck = $scope.yearlist.every(function (options) {
                return options.select;
            });
        }
        $scope.togchkbxtype = function () {
          
            $scope.userchecktype = $scope.typelist.every(function (optionstype) {
                return optionstype.select1;
            })
        }
        $scope.togchkbxacti = function () {
            $scope.usercheckacti = $scope.activitylist.every(function (optionsacti) {
                return optionsacti.select2;
            })
        }
        $scope.togchkbxlevel = function () {
            $scope.userchecklevel = $scope.levellist.every(function (optionslevel) {
                return optionslevel.select3;
            })
        }
        $scope.isOptionsRequired = function () {
            return !$scope.yearlist.some(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequiredtype = function () {
            return !$scope.typelist.some(function (options) {
                return options.select1;
            })
        }
        $scope.isOptionsRequiredacti = function () {
            return !$scope.activitylist.some(function (options) {
                return options.select2;
            })
        }
        $scope.isOptionsRequiredlevel = function () {
            return !$scope.levellist.some(function (options) {
                return options.select3;
            })
        }
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.filterchkbxtype = function (obj) {
            return (angular.lowercase(obj.prmtY_ProgramType)).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;
        }
        $scope.filterchkbxacti = function (obj) {
            return (angular.lowercase(obj.pryrA_ActivityName)).indexOf(angular.lowercase($scope.searchchkbxacti)) >= 0;
        }
        $scope.filterchkbxlevel = function (obj) {
            return (angular.lowercase(obj.prmtlE_ProgramLevel)).indexOf(angular.lowercase($scope.searchchkbxlevel)) >= 0;
        }


    }

})();






