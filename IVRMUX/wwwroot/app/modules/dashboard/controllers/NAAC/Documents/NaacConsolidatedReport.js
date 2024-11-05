
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacConsolidatedReportController', NaacConsolidatedReportController)

    NaacConsolidatedReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel', '$window', 'uiGridGroupingConstants']
    function NaacConsolidatedReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel, $window, uiGridGroupingConstants) {

        $scope.obj = {};

        $scope.submitted = false;
        $scope.loaddata = function () {
            $scope.printflag = false;
            $scope.showflag = false;
            var pageid = 2;
            apiService.getURI("NaacConsolidatedReport/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                //$scope.cycleid = promise.getinstitutioncycle[0].cycleid;
            })
        }
        $scope.submitted = false;

        $scope.get_report = function () {
            $scope.submitted = true;
            debugger;
           // $scope.showflag = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("NaacConsolidatedReport/get_report", data).then(function (promise) {

                    $scope.getparentidzero = promise.getparentidzero;
                    $scope.getsavealldata = promise.getsavealldata;
                    $scope.get_Report = promise.get_Report;
                    

                    $scope.mainarry = [];
                    angular.forEach($scope.getparentidzero, function (ze) {
                        $scope.sublist = [];
                        angular.forEach($scope.getsavealldata, function (al) {
                            if (ze.naacsL_Id === al.naacsL_ParentId) {
                                $scope.sublist.push(al);
                            }
                        });
                        $scope.mainarry.push({ naacsL_ParentId: ze.naacsL_ParentId, naacsL_SLNoDescription: ze.naacsL_SLNoDescription, naacsL_SLNo: ze.naacsL_SLNo, sublist2: $scope.sublist })
                    });
                    $scope.gridOptions.data = $scope.getsavealldata;
                    console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    console.log($scope.mainarry);
                    console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

                    if (promise !== null) {
                        $scope.getparentidzero = promise.getparentidzero;
                       $scope.getalldata = promise.getalldata;
                        $scope.array = [];
                        $scope.getsavealldata = promise.getsavealldata;
                        angular.forEach($scope.getparentidzero, function (dd) {
                            $scope.temparra1d = [];
                            var toptotal = 0;
                            angular.forEach($scope.getalldata, function (ddd) {
                                if (ddd.naacsL_ParentId === dd.naacsL_Id) {
                                    $scope.temparra1d.push(ddd);
                                }
                            });

                            angular.forEach($scope.temparra1d, function (sdd) {
                                angular.forEach($scope.getsavealldata, function (dds) {
                                    if (sdd.naacsL_Id === dds.naacsL_Id) {
                                        sdd.NAACMSL_Status = dds.naacmsL_Status;
                                        sdd.NAACMSL_ConsultantRemarks = dds.naacmsL_ConsultantRemarks;
                                        sdd.coordinatorcomments = dds.naacmsL_ConsultantRemarks;
                                        sdd.usercomments = dds.naacmsL_Details;
                                    }
                                });
                            });
                            
                            $scope.temparray2d = [];
                            angular.forEach($scope.temparra1d, function (levelii) {
                                $scope.temparray2d = [];
                                angular.forEach($scope.getalldata, function (ddd) {
                                    if (ddd.naacsL_ParentId === levelii.naacsL_Id) {
                                        $scope.temparray2d.push(ddd);
                                    }
                                });
                                angular.forEach($scope.temparray2d, function (sddd) {
                                    //angular.forEach(promise.get_Report, function (ddds) {
                                    //    if (sddd.naacsL_SLNo === ddds.NAACSL_SLNO) {
                                    //        //sddd.naacsL_SLNo = ddds.NAACSL_SLNO;
                                    //        $scope.temparray2d.push(ddds)
                                    //    }
                                    //});
                                });
                                levelii.temparray2 = $scope.temparray2d;
                            });
                            $scope.array.push({ NAACSL_Id: dd.naacsL_Id, NAACSL_SLNo: dd.naacsL_SLNo, NAACSL_SLNoDescription: dd.naacsL_SLNoDescription, temparra1: $scope.temparra1d });
                            console.log($scope.array);
                        });
                        if ($scope.array !== null && $scope.array.length > 0) {
                            $timeout(function () {
                                $scope.getdata();
                            }, 500);
                            $scope.getdata();
                        }
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function () {
            return $scope.submitted;
        }
            //=================//
        $scope.gridOptions = {
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            enableColumnMenus: false,

            treeRowHeaderAlwaysVisible: false,
            columnDefs: [
                { name: 'naacsL_SLNoDescription', displayName: 'CRITERIA', grouping: { groupPriority: 0 }, width: '24%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'naacsL_SLNote', displayName: 'SUB CRITERIA', grouping: { groupPriority: 1 }, width: '25%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },
                { name: 'naacmsL_Status', displayName: 'STATUS', grouping: { groupPriority: 2 }, width: '20%', cellTemplate: '<div><div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div></div>' },

                //{
                //    name: 'naacmsL_CGPA', displayName: 'CGPA', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                //        aggregation.rendered = aggregation.value;
                //    }
                //},
            ],
            exporterMenuPdf: false,

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'class year status report',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'all', 'all');
                },
                order: 110
            },
            {
                title: 'Export visible data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'visible', 'visible');
                },
                order: 111
            }
            ]
        };

        $scope.exportToExcel = function () {
            var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

            var table = document.getElementById("printSectionId");
            var filters = $('.ng-table-filters').remove();
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML };
            $('.ng-table-sort-header').after(filters);
            var url = uri + base64(format(template, ctx));
            var a = document.createElement('a');
            a.href = url;
            a.download = 'Exported_Table.xls';
            a.click();
        };
    }

})();