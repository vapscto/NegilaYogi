

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ConferenceDetailsReport', ConferenceDetailsReport)

    ConferenceDetailsReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function ConferenceDetailsReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = '';
        $scope.Binddata = function () {

            var pageid = 2;
            apiService.getURI("OnlineProgramReport/getyearlyprogram", pageid).then(function (promise) {
               
                $scope.typelist = promise.typelist;
                $scope.departmentlist = promise.departmentlist;
                $scope.levellist = promise.levellist;
            })
        }

        $scope.Clearid = function () {
            $state.reload();
        };


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



        $scope.showdetails = function () {

           
            $scope.selectedtypelist = [];
            $scope.selecteddepartmentlist = [];
            $scope.selectedlevellist = [];
            if ($scope.myForm.$valid) {

              
                angular.forEach($scope.typelist, function (type) {
                    if (type.select1 == true) {
                        $scope.selectedtypelist.push({ prmtY_Id: type.prmtY_Id });
                    }
                })
                angular.forEach($scope.departmentlist, function (dept) {
                    if (dept.select == true) {
                        $scope.selecteddepartmentlist.push({ hrmD_Id: dept.hrmD_Id });
                    }
                })
                angular.forEach($scope.levellist, function (level) {
                    if (level.select3 == true) {
                        $scope.selectedlevellist.push({ prmtlE_Id: level.prmtlE_Id });
                    }
                })
                //var date1 = $scope.PRYR_StartDate == null ? "" : $filter('date')($scope.PRYR_StartDate, "yyyy-MM-dd");
                //var date2 = $scope.PRYR_EndDate == null ? "" : $filter('date')($scope.PRYR_EndDate, "yyyy-MM-dd");
                var date1 = new Date($scope.PRYR_StartDate).toDateString();
                var date2 = new Date($scope.PRYR_EndDate).toDateString();
                var data = {
                    "PRYR_StartDate": date1,
                    "PRYR_EndDate": date2,
                    
                    selectedtypelist: $scope.selectedtypelist,
                    selecteddepartmentlist: $scope.selecteddepartmentlist,
                    selectedlevellist: $scope.selectedlevellist,

                }
                apiService.create("OnlineProgramReport/ConferenceDetailsReport", data).then(function (promise) {
                  
                    if (promise.reportlist != null && promise.reportlist.length>0) {
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
        $scope.searchchkbxdept = "";
        $scope.searchchkbxlevel = "";
        $scope.cancel = function () {
            $scope.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

      
        $scope.all_checktype = function () {

            var checkStatus = $scope.userchecktype;
            angular.forEach($scope.typelist, function (itmtype) {
                itmtype.select1 = checkStatus;
            })
        }

        $scope.all_checkdept = function () {
            var checkStatus = $scope.usercheckdept;
            angular.forEach($scope.departmentlist, function (itmdept) {
                itmdept.select = checkStatus;
            })
        }
        $scope.all_checklevel = function () {
            var checkStatus = $scope.userchecklevel;
            angular.forEach($scope.levellist, function (itmlevel) {
                itmlevel.select3 = checkStatus;
            })
        }
       
        $scope.togchkbxtype = function () {

            $scope.userchecktype = $scope.typelist.every(function (optionstype) {
                return optionstype.select1;
            })
        }
        $scope.togchkbxdept = function () {
            $scope.usercheckdept = $scope.departmentlist.every(function (optionsdept) {
                return optionsdept.select;
            })
        }
        $scope.togchkbxlevel = function () {
            $scope.userchecklevel = $scope.levellist.every(function (optionslevel) {
                return optionslevel.select3;
            })
        }
       
        $scope.isOptionsRequiredtype = function () {
            return !$scope.typelist.some(function (options) {
                return options.select1;
            })
        }
        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentlist.some(function (options) {
                return options.select;
            })
        }
        $scope.isOptionsRequiredlevel = function () {
            return !$scope.levellist.some(function (options) {
                return options.select3;
            })
        }
      
        $scope.filterchkbxtype = function (obj) {
            return (angular.lowercase(obj.prmtY_ProgramType)).indexOf(angular.lowercase($scope.searchchkbxtype)) >= 0;
        }
        $scope.filterchkbxdept = function (obj) {
            return (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.searchchkbxdept)) >= 0;
        }
        $scope.filterchkbxlevel = function (obj) {
            return (angular.lowercase(obj.prmtlE_ProgramLevel)).indexOf(angular.lowercase($scope.searchchkbxlevel)) >= 0;
        }


    }

})();






