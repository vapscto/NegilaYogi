(function () {
    'use strict';
    angular
        .module('app')
        .controller('LibraryMonthEndReportController', LibraryMonthEndReportController)

    LibraryMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function LibraryMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        //$scope.BindData = function () {
        //    apiService.getDATA("TrnsMonthEndReport/getdetails").
        //        then(function (promise) {

        //            $scope.acdlist = promise.acdlist;
        //            $scope.examlist = promise.examlist;
        //            $scope.monthlist = promise.monthlist;
        //        })
        //};

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("LibraryMonthEndReport/getdetails", pageid).then(function (promise) {
                if (promise != null) {

                    $scope.acayyearbind = promise.acdlist;

                    $scope.monthlist = promise.monthlist;


                    $scope.alldata = promise.alldata;

                }
                else {
                    swal("No Records Found")
                }
            })

        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.date_m = new Date();

        var temp = [];
        var year = "";

        $scope.onselectAcdYear = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        }

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            var LMAL_Id = 0;
            if ($scope.myForm.$valid) {
                if ($scope.LMAL_Id > 0) {
                    LMAL_Id = $scope.LMAL_Id;
                }
                var data = {
                    "ASMAY_Id": $scope.academicyr,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id,
                    "year": $scope.yearmodel,
                    "LMRA_Id": $scope.LMAL_Id,
                }

                apiService.create("LibraryMonthEndReport/savedata", data).
                    then(function (promise) {

                        for (var i = 0; i < $scope.monthlist.length; i++) {
                            if ($scope.monthlist[i].ivrM_Month_Id == $scope.IVRM_Month_Id) {
                                $scope.Month_Name = $scope.monthlist[i].ivrM_Month_Name;
                            }
                        }
                        for (var j = 0; j < $scope.acayyearbind.length; j++) {
                            if ($scope.acayyearbind[j].asmaY_Id == $scope.academicyr) {
                                $scope.Year_Name = $scope.acayyearbind[j].asmaY_Year;
                            }
                        }
                        $scope.main_list = promise.griddata;
                        debugger;
                        $scope.temparraylist = [];

                        angular.forEach($scope.main_list, function (ff) {
                            $scope.temparraylist = [];
                            //BOOK COUNT
                            if (ff.TOTALBOOK >= 0) {
                                $scope.temparraylist.push({ ENAME: 'TOTALBOOK', TNAME: 'TOTAL BOOKS', COUNT: ff.TOTALBOOK })
                            }
                            if (ff.AVAILBOOK >= 0) {
                                $scope.temparraylist.push({ ENAME: 'AVAILBOOK', TNAME: 'AVAILABLE BOOKS', COUNT: ff.AVAILBOOK })
                            }

                            if (ff.PURCHASECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'PURCHASECNT', TNAME: 'PURCHASED BOOKS', COUNT: ff.PURCHASECNT })
                            }

                            if (ff.DONATECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'DONATECNT', TNAME: 'DONATED BOOKS', COUNT: ff.DONATECNT })
                            }

                            //STUDENT BOOK COUNT
                            if (ff.STDISSUECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'STDISSUECNT', TNAME: 'STUDENT BOOK ISSUE', COUNT: ff.STDISSUECNT })
                            }
                            if (ff.STDRETURNCNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'STDRETURNCNT', TNAME: 'STUDENT BOOK RETURN', COUNT: ff.STDRETURNCNT })
                            }
                            //STAFF COUNT
                            if (ff.STFISSUECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'STFISSUECNT', TNAME: 'STAFF BOOK ISSUE', COUNT: ff.STFISSUECNT })
                            }
                            if (ff.STFRETURNCNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'STFRETURNCNT', TNAME: 'STAFF BOOK RETURN', COUNT: ff.STFRETURNCNT })
                            }
                            //DEPARTMENT COUNT
                            if (ff.DEPISSUECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'DEPISSUECNT', TNAME: 'DEPARTMENT BOOK ISSUE', COUNT: ff.DEPISSUECNT })
                            }
                            if (ff.DEPRETURNCNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'DEPRETURNCNT', TNAME: 'DEPARTMENT BOOK RETURN', COUNT: ff.DEPRETURNCNT })
                            }
                            //GUEST COUNT
                            if (ff.GSTISSUECNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'GSTISSUECNT', TNAME: 'GUEST BOOK ISSUE', COUNT: ff.GSTISSUECNT })
                            }
                            if (ff.GSTRETURNCNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'GSTRETURNCNT', TNAME: 'GUEST BOOK RETURN', COUNT: ff.GSTRETURNCNT })
                            }
                            //  FINE
                            if (ff.FINEAMOUNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'FINEAMOUNT', TNAME: 'TOTAL FINE', COUNT: ff.FINEAMOUNT })
                            }
                            //SMS/EMAIL
                            if (ff.SMSCOUNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'SMSCOUNT', TNAME: 'SMS', COUNT: ff.SMSCOUNT })
                            }
                            if (ff.EMAILCOUNT >= 0) {
                                $scope.temparraylist.push({ ENAME: 'EMAILCOUNT', TNAME: 'EMAIL', COUNT: ff.EMAILCOUNT })
                            }


                        })

                        console.log($scope.temparraylist);

                        console.log($scope.main_list);
                        $scope.stud_graph = [];
                        if ($scope.temparraylist != "0" && $scope.temparraylist != null) {
                            $scope.studgraph = false;
                            for (var i = 0; i < $scope.temparraylist.length; i++) {
                                $scope.stud_graph.push({ label: $scope.temparraylist[i].TNAME, "y": $scope.temparraylist[i].COUNT })
                            }


                            var chart = new CanvasJS.Chart("linechart", {
                                height: 350,
                                width: 1065,
                                axisX: {
                                    labelFontSize: 12,
                                    interval: 1,
                                    labelAngle: -20,
                                },
                                axisY: {
                                    labelFontSize: 12,
                                    //    maximum: 90
                                    //scaleBreaks: {
                                    //    customBreaks: [{
                                    //        startValue: 100,
                                    //        endValue: 400,
                                    //        color: "orange"
                                    //    }]
                                    //}

                                },

                                data: [
                                    {
                                        type: "column",
                                        showInLegend: true,
                                        color: "Blue",
                                        dataPoints: $scope.stud_graph
                                    }
                                ]
                            });

                            chart.render();

                            $scope.printData = function () {
                                if ($scope.temparraylist.length > 0 && $scope.temparraylist != null) {
                                    var base64Image = chart.canvas.toDataURL();
                                    document.getElementById('linechart').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;
                                    var innerContents = document.getElementById("table").innerHTML;

                                    var popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/LIbMonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');

                                    popupWinindow.document.close();


                                }
                                //  $state.reload();
                            };

                        }
                        else {
                            swal("No Record Found....");
                        }

                    })
            }
        };



        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.EME_Id = '';
            $scope.IVRM_Month_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



    }

})();