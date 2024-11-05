(function () {
    'use strict';
    angular.module('app').controller('NaacDocumentUploadReportController', NaacDocumentUploadReportController)

    NaacDocumentUploadReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel', '$window', 'uiGridGroupingConstants']
    function NaacDocumentUploadReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel, $window, uiGridGroupingConstants) {

        $scope.obj = {};
        $scope.valued = "1";

        $scope.addnewbtn = true;
        $scope.remflg = false;

        $scope.delete = function (data) {
            data.nodes = [];
        };

        $scope.add = function (data) {
            var post = data.nodes.length + 1;
            var newName = data.name + '-' + post;
            data.nodes.push({ name: newName, nodes: [] });
        };

        $scope.array = [];

        $scope.ddfd = true;
        $scope.onload = function () {

            var pageid = 1;

            apiService.getURI("NaacDocumentUploadReport/loaddata", pageid).then(function (promise) {


                $scope.getparentidzero = promise.getparentidzero;
                $scope.getsavealldata = promise.getsavealldata;

                $scope.mainarry = [];

                var grand_total = 0;
                angular.forEach($scope.getparentidzero, function (ze) {
                    $scope.sublist = [];   
                    var cgptotal = 0;
                    angular.forEach($scope.getsavealldata, function (al) {
                        if (ze.naacsL_Id === al.naacsL_ParentId) {
                            cgptotal += parseFloat(al.naacmsL_CGPA);                            
                             $scope.sublist.push(al);                                                     
                        }
                    });
                    $scope.cgpa_total = cgptotal;
                    grand_total += parseFloat(cgptotal);
                    $scope.mainarry.push({ naacsL_ParentId: ze.naacsL_ParentId, naacsL_SLNoDescription: ze.naacsL_SLNoDescription, naacsL_SLNo: ze.naacsL_SLNo, sublist2: $scope.sublist, cgpalist: $scope.cgpa_total, grnd_total: grand_total})

                });
                $scope.grnd_total = grand_total;

                $scope.gridOptions.data = $scope.getsavealldata;


                console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                console.log($scope.grnd_total);
                console.log(grand_total);
                console.log($scope.mainarry);
                console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");


                if (promise !== null) {
                    $scope.getparentidzero = promise.getparentidzero;
                    $scope.getalldata = promise.getalldata;

                    $scope.array = [];
                    $scope.getsavealldata = promise.getsavealldata;

                    $scope.per = promise.percentage;
                    var grandtoal = 0; 
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
                                    //sdd.document_Path = dds.naacmsL_Uploadpath;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                    sdd.NAACMSL_ConsultantRemarks = dds.naacmsL_ConsultantRemarks;
                                    sdd.coordinatorcomments = dds.naacmsL_ConsultantRemarks;
                                    sdd.usercomments = dds.naacmsL_Details;
                                    sdd.naacmsL_CGPA = dds.naacmsL_CGPA;
                                }
                            });
                        });


                        $scope.temparray2d = [];
                        angular.forEach($scope.temparra1d, function (levelii) {
                            var totalCGPA = 0;
                            $scope.temparray2d = [];
                            angular.forEach($scope.getalldata, function (ddd) {
                                if (ddd.naacsL_ParentId === levelii.naacsL_Id) {
                                    $scope.temparray2d.push(ddd);
                                }
                            });
                            
                            angular.forEach($scope.temparray2d, function (sddd) {                               
                                
                                angular.forEach($scope.getsavealldata, function (ddds) {
                                    if (sddd.naacsL_Id === ddds.naacsL_Id) {
                                        //sddd.document_Path = ddds.naacmsL_Uploadpath;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                        sddd.NAACMSL_ConsultantRemarks = ddds.naacmsL_ConsultantRemarks;
                                        sddd.coordinatorcomments = ddds.naacmsL_ConsultantRemarks;
                                        sddd.usercomments = ddds.naacmsL_Details;
                                        sddd.naacmsL_CGPA = ddds.naacmsL_CGPA;
                                       
                                        if (ddds.naacmsL_CGPA !== null && ddds.naacmsL_CGPA !== undefined && ddds.naacmsL_CGPA !== "") {
                                            totalCGPA += parseFloat(ddds.naacmsL_CGPA);
                                            toptotal += parseFloat(ddds.naacmsL_CGPA);
                                            grandtoal += parseFloat(ddds.naacmsL_CGPA);
                                        }

                                    }
                                });                               
                            });                          
                            
                            levelii.temparray2 = $scope.temparray2d;
                            levelii.totalte3 = totalCGPA;
                        });
                        
                        $scope.array.push({ NAACSL_Id: dd.naacsL_Id, NAACSL_SLNo: dd.naacsL_SLNo, NAACSL_SLNoDescription: dd.naacsL_SLNoDescription, temparra1: $scope.temparra1d, toptotal: toptotal });

                        console.log($scope.array);
                        $scope.finalgrandtoal = grandtoal;



                        var amt = 60;

                        $scope.countTo = amt;
                        $scope.countFrom = 0;

                    });

                    //$timeout(function () {
                    //    $scope.progressValue = amt;
                    //}, 10);

                    if ($scope.array !== null && $scope.array.length > 0) {
                        $timeout(function () {
                            $scope.getdata();
                        }, 500);
                        $scope.getdata();
                    }
                }
            });
        };
        $scope.getdata = function () {
            $scope.ddfd = true;
            console.log("==============================");
            console.log($scope.array);
            var tree = document.querySelectorAll('ul.tree a:not(:last-child)');
            for (var i = 0; i < tree.length; i++) {
                tree[i].addEventListener('click', function (e) {
                    var parent = e.target.parentElement;
                    var classList = parent.classList;
                    if (classList.contains("open")) {
                        classList.remove('open');
                        var opensubs = parent.querySelectorAll(':scope .open');
                        for (var i = 0; i < opensubs.length; i++) {
                            opensubs[i].classList.remove('open');
                        }
                    } else {
                        classList.add('open');
                    }
                });
            }
        };


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

                {
                    name: 'naacmsL_CGPA', displayName: 'CGPA', width: '15%', treeAggregationType: uiGridGroupingConstants.aggregation.SUM, customTreeAggregationFinalizerFn: function (aggregation) {
                        aggregation.rendered = aggregation.value;
                    }
                },
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


        //$scope.exportToExcel = function (tableId) {
        //    var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //}

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



