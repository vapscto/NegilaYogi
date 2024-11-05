(function () {
    'use strict';
    angular.module('app')
        .controller('InteractionReportController', InteractionReportController)
    InteractionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function InteractionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        //for (var i = 0; i < privlgs.length; i++) {
        //    if (privlgs[i].pageId == pageid) {
        //        $scope.userPrivileges = privlgs[i];
        //    }
        //}

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
    

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.loaddata = function () {
            $scope.screport = false;
          
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("Interaction_Delete_Report/loadreportdata", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                    $scope.classlist = promise.fillclass;
                })
        };

   
        $scope.submitted = false;
        $scope.flag = "STAFF";
        $scope.showreport = function () {
           
            if ($scope.myForm.$valid) {
                
                $scope.fromdate = new Date($scope.fromdate).toDateString();
                $scope.todate = new Date($scope.todate).toDateString();
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "flag": $scope.flag,
                }
                apiService.create("Interaction_Delete_Report/getintreport/", data).
                    then(function (promise) {
                  
                        if (promise.deletemsglist != null && promise.deletemsglist.length > 0) {



                            $scope.deletemsglist = promise.deletemsglist;

                            $scope.mainintlist = [];

                            angular.forEach($scope.deletemsglist, function (eps) {
                                if ($scope.mainintlist.length == 0) {
                                    $scope.mainintlist.push({ ISMINT_Id: eps.ISMINT_Id, ISMINT_InteractionId: eps.ISMINT_InteractionId, ISMINT_Subject: eps.ISMINT_Subject, ISMINT_ComposedByFlg: eps.ISMINT_ComposedByFlg, ISMINT_DateTime: eps.ISMINT_DateTime });
                                }
                                else if ($scope.mainintlist.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.mainintlist, function (exm) {
                                        if (exm.ISMINT_Id == eps.ISMINT_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    })
                                    if (al_exm_cnt == 0) {
                                        $scope.mainintlist.push({ ISMINT_Id: eps.ISMINT_Id, ISMINT_InteractionId: eps.ISMINT_InteractionId, ISMINT_Subject: eps.ISMINT_Subject, ISMINT_ComposedByFlg: eps.ISMINT_ComposedByFlg, ISMINT_DateTime: eps.ISMINT_DateTime });
                                    }
                                }
                            })



                            angular.forEach($scope.mainintlist, function (zz) {
                                var LIST = [];
                                angular.forEach($scope.deletemsglist, function (nn) {

                                    if (nn.ISMINT_Id == zz.ISMINT_Id) {
                                        LIST.push(nn);
                                    }

                                })
                                zz.LIST = LIST;
                            })



                            console.log($scope.mainintlist);
                        }
                        else {
                            swal('No Data Found!!!')
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.mainintlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                      
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                   
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.mainintlist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
                
               
            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
                
            }

        }
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        
        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();