(function () {
    'use strict';
    angular.module('app')
        .controller('ClgNoticeboardConsolidatedController', ClgNoticeboardConsolidatedController)
    ClgNoticeboardConsolidatedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ClgNoticeboardConsolidatedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.FromDate = new Date();
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

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
        $scope.coursearray = [];
        $scope.search = "";
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10;
        $scope.loaddata = function () {
            $scope.screport = false;

           
            var pageid = 2;
            apiService.getURI("ClgNoticeBoard/Getdata_class", pageid).
                then(function (promise) {
                    $scope.courselist = promise.course_list;

                })
        };


        //=============== Check box
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.courselist.every(function (itm) { return itm.selected; });
            if ($scope.coursearray.indexOf(SelectedStudentRecord) === -1) {
                $scope.coursearray.push(SelectedStudentRecord);

            }
            else {
                $scope.coursearray.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }

        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.courselist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.coursearray.indexOf(itm) === -1) {
                        $scope.coursearray.push({ AMCO_Id: itm.AMCO_Id });

                    }
                }
                else {
                    $scope.coursearray.splice({ AMCO_Id: itm.AMCO_Id });

                }
            });
        }

        $scope.isOptionsRequired = function () {

            return !$scope.courselist.some(function (sec) {
                return sec.selected;
            });
        }

        $scope.submitted = false;

        $scope.onrdochange = function () {
            $scope.reportlist = [];
        }
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {
                $scope.coursearraynew = [];
                if ($scope.coursearray != null || $scope.coursearray > 0) {
                    angular.forEach($scope.courselist, function (qq) {
                        if (qq.selected == true) {
                            $scope.coursearraynew.push({ AMCO_Id: qq.AMCO_Id })
                        }

                    })
                }
                else {
                    $scope.classarraynew = undefined;
                }

                $scope.fromdate = $filter('date')($scope.fromdate, "yyyy-MM-dd");
                $scope.todate = $filter('date')($scope.todate, "yyyy-MM-dd");
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "coursearray": $scope.coursearraynew,
                    "flag": $scope.optionflag,
                    "type": $scope.typeflag
                   

                }
                apiService.create("ClgNoticeBoard/getreportnotice", data).
                    then(function (promise) {
                        if (promise.reportlist.length > 0 && promise.reportlist !== null) {
                            $scope.reportlist = promise.reportlist;
                         //   $scope.reportlist = promise.reportlist;
                            $scope.showGrafh = true;
                            //$scope.stud_graph1 = [];
                            //$scope.studgraph1 = false;
                            //for (var i = 0; i < $scope.reportlist.length; i++) {
                            //    $scope.stud_graph1.push({ label: $scope.reportlist[i].cou, "y": $scope.classreportlist[i].ISMS_SubjectName });
                            //}

                            //chart = new CanvasJS.Chart("linechart1", {
                            //    animationEnabled: true,
                            //    animationDuration: 3000,
                            //    axisX: {
                            //        labelFontSize: 12
                            //    },
                            //    axisY: {
                            //        labelFontSize: 12
                            //    },

                            //    toolTip: {
                            //        shared: true
                            //    },
                            //    data: [{
                            //        name: "Bank",
                            //        showInLegend: true,
                            //        type: "column",
                            //        color: "#2295ea",
                            //        dataPoints: $scope.stud_graph1
                            //    }
                            //    ]
                            //});
                            //chart.render();
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

        $scope.showdetails = function (user) {

            //stu_id = stu
            $scope.fromdate = $filter('date')($scope.fromdate, "yyyy-MM-dd");
            $scope.todate = $filter('date')($scope.todate, "yyyy-MM-dd");
            var data = {
                "fromdate": $scope.fromdate,
                "todate": $scope.todate,
                "AMCO_Id": user.AMCO_Id,
                "flag": "Detailed",
                "type": $scope.typeflag

            }

            apiService.create("ClgNoticeBoard/Getdataview", data).
                then(function (promise) {
                    $scope.view_array = promise.view_array;

                })
            // $('#popup111').modal('show');
            $('#myModall').modal('show');
        }
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.reportlist !== null && $scope.reportlist.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };
        //add by raj
        $scope.exportToExcel1 = function (tableId) {
            if ($scope.reportlist !== null && $scope.reportlist.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            //else {
            //    swal("Please select records to be Exported");
            //}

        };



        $scope.printData = function () {
            if ($scope.reportlist !== null && $scope.reportlist.length > 0) {
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

        //add by raj
        $scope.printData1 = function () {
            if ($scope.reportlist !== null && $scope.reportlist.length > 0) {
                var innerContents = document.getElementById("printSectionId1").innerHTML;
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
        //add by raj download
        //Pdf download
        $scope.download = function () {
            html2canvas(document.getElementById('myModal'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500,
                            height: 600
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("RECEIPT.pdf");
                }
            });
        };



        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //add by Raj


    }
})();