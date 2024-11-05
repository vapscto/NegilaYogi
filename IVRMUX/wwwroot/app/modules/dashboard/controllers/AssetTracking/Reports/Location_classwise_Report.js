(function () {
    'use strict';
    angular
        .module('app')
        .controller('Location_classwise_ReportController', location_classwise_ReportController)
    location_classwise_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function location_classwise_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


        $scope.cancel = function () {
            $state.reload();
        }


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.routetypelist = [];
        $scope.vh = false;
        $scope.FMG_Id = 0;
        //===============================search check box

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.invmlO_LocationRoomName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }
        //============================
        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.location_list.every(function (options) {
                return options.select1;
            });
        }

        //============================check  box

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.location_list, function (itm) {
                itm.select1 = checkStatus;
            });
        }
        //======================
        $scope.onLoadGetData = function () {
            var pageid = 2
            apiService.getURI("SiteLocationsReport/get_all_data_LCR", pageid).then(function (promise) {
                if (promise.location_list != null) {
                    $scope.location_list = promise.location_list;


                }
                else {
                    swal("No Records Found")
                }
            })
        }


        $scope.getreport = function () {
            $scope.selecteloc_list = [];

            angular.forEach($scope.location_list, function (des) {
                if (des.select1 == true) {
                    $scope.selecteloc_list.push({ INVMLO_Id: des.invmlO_Id });
                }
            });

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {


                    selecteloc_list: $scope.selecteloc_list,


                }

                apiService.create("SiteLocationsReport/getreport_LCR", data).
                    then(function (promise) {

                        if (promise.location_print_list.length > 0) {


                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;
                            $scope.header_list = [];
                            $scope.header_list1 = [];
                            var count = 0;
                            $scope.location_print_list = promise.location_print_list;

                               if ($scope.location_print_list.length > 0) {
                                for (var i = 0; i < $scope.location_print_list.length; i++) {

                                    if (i === 0) {
                                        angular.forEach($scope.location_print_list[i], function (key, r) {

                                            $scope.header_list.push({ colmn: r, head: key });


                                        });
                                    }
                                }
                               
                                
                            }

                            $scope.header_list1 = [];
                            angular.forEach($scope.header_list, function (rr) {
                                var cnnt = 0;
                                for (var i = 0; i < $scope.location_print_list.length; i++) {

                                    angular.forEach($scope.location_print_list[i], function (key, r) {
                                        if (r == rr.colmn && rr.colmn != 'INVMLO_LocationRoomName') {
                                            cnnt += parseInt(key);
                                        }



                                    });

                                }

                                $scope.header_list1.push({ head: rr.colmn, cntt: cnnt });

                            })

                            
                            
                        }
                        
                        else {
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            swal("No Records Found!");
                            $state.reload();
                        }
                    })
            
             

                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;

          
        };




        $scope.isOptionsRequired1 = function () {

            return !$scope.location_list.some(function (sec) {
                return sec.select1;
            });
        }
        $scope.searchValue = '';
        $scope.filterValue1 = function (obj) {

            return (angular.lowercase(obj.employeename3)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var checkStatus2 = $scope.checkall;
            angular.forEach($scope.exi_employee_print_list, function (itm) {
                if (itm.checked = checkStatus2) {
                    $scope.printdatatable.push(itm);

                }
            });
        };

        $scope.optionToggled = function () {
            $scope.printdatatable = [];
            angular.forEach($scope.exi_employee_print_list, function (itm) {
                if (itm.checked === true) {
                    $scope.printdatatable.push(itm);
                }

            });
        }

        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'printSectionId');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


        $scope.Print = function () {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //================
       
    };

})();