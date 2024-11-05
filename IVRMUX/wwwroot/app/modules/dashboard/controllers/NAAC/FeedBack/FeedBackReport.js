(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeedBackReportController', FeedBackReportController)

    FeedBackReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeedBackReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#34495E",
                "#85C1E9",
                "#DAF7A6",
                "#FFC300",
                "#FF5733"
            ]);
        $scope.report = false;
        $scope.catreport = false;
        $scope.table = false;
        $scope.graphs = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;

        $scope.BindData = function () {
            apiService.getDATA("FeedBackReport/getdetails").
                then(function (promise) {
                    $scope.yearlist = promise.getyear;
                });
        };


        $scope.onchangeradio = function () {
            var data = {
                "flag": $scope.FMTY_StakeHolderFlag
            };
            apiService.create("FeedBackReport/onchangeradio", data).then(function (promise) {

                if (promise !== null) {

                    $scope.feedbacktype1 = promise.feedbacktype;
                    if ($scope.feedbacktype1.length > 0) {
                        $scope.feedbacktype = promise.feedbacktype;

                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });
        };






        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';
        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        // For table represtation
        $scope.onreport = function (obj) {

            if ($scope.myForm.$valid) {

                $scope.table = false;
                $scope.graphs = false;

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "flag": $scope.FMTY_StakeHolderFlag,
                    "Type": $scope.FMTY_Id
                };

                apiService.create("FeedBackReport/getreport", data).
                    then(function (promise) {

                        if (promise !== null) {
                            $scope.report1 = promise.getreportdetails;
                            if ($scope.report1.length > 0) {

                                $scope.table = true;
                                $scope.graphs = false;

                                $scope.report = promise.getreportdetails;
                                $scope.temp123 = [];

                                $scope.getquestions = promise.getquestions;
                                $scope.getoptions = promise.getoptions;

                                angular.forEach($scope.getquestions, function (dd) {
                                    $scope.tempoption = [];
                                    angular.forEach($scope.report, function (ddd) {
                                        if (dd.fmqE_Id === ddd.qid) {
                                            angular.forEach($scope.getoptions, function (dddd) {
                                                if (dddd.fmoP_Id === ddd.opid) {
                                                    $scope.tempoption.push({ optionname: dddd.fmoP_FeedbackOptions, optionscore: ddd.total });
                                                }
                                            });
                                        }
                                    });
                                    $scope.temp123.push({ header: dd.fmqE_FeedbackQuestions, optiondetails: $scope.tempoption });
                                });

                                console.log($scope.temp123);

                                var k = 0;

                                angular.forEach($scope.temp123, function (dd) {
                                    dd.n = k = k + 1;
                                });

                                $scope.temp1 = $scope.temp123;

                                $scope.bar1 = [];

                                var i = 0;


                                $scope.reporttype = $scope.FMTY_StakeHolderFlag;
                                angular.forEach($scope.yearlist, function (dd) {
                                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                        $scope.yearname = dd.asmaY_Year;
                                    }
                                });

                                $scope.reportname = "Feedback Type :" + $scope.reporttype;
                                $scope.yearname = "Year :" + $scope.yearname;
                                //$scope.reportname = "Feedback Report For Year " + $scope.yearname + " Feedback Type " + $scope.reporttype;

                            } else {
                                swal("No Reocrd Found");
                                $scope.table = false;
                                $scope.graphs = false;
                            }

                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                            $scope.table = false;
                            $scope.graphs = false;
                        }


                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        // For Graphical Representaion
        $scope.onreport1 = function (obj) {
            $scope.table = false;
            $scope.graphs = false;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "flag": $scope.FMTY_StakeHolderFlag,
                    "Type": $scope.FMTY_Id
                };

                apiService.create("FeedBackReport/getreport", data).
                    then(function (promise) {

                        if (promise !== null) {
                            $scope.report1 = promise.getreportdetails;
                            if ($scope.report1.length > 0) {


                                $scope.report = promise.getreportdetails;
                                $scope.temp123 = [];

                                $scope.getquestions = promise.getquestions;
                                $scope.getoptions = promise.getoptions;

                                angular.forEach($scope.getquestions, function (dd) {
                                    $scope.tempoption = [];
                                    angular.forEach($scope.report, function (ddd) {
                                        if (dd.fmqE_Id === ddd.qid) {
                                            angular.forEach($scope.getoptions, function (dddd) {
                                                if (dddd.fmoP_Id === ddd.opid) {
                                                    $scope.tempoption.push({ optionname: dddd.fmoP_FeedbackOptions, optionscore: ddd.total });
                                                }
                                            });
                                        }
                                    });
                                    $scope.temp123.push({ header: dd.fmqE_FeedbackQuestions, optiondetails: $scope.tempoption });
                                });

                                console.log($scope.temp123);

                                var k = 0;

                                angular.forEach($scope.temp123, function (dd) {
                                    dd.n = k = k + 1;
                                });

                                $scope.temp1 = $scope.temp123;

                                $scope.bar1 = [];

                                var i = 0;


                                angular.forEach($scope.temp123, function (t) {

                                    setTimeout(function () {
                                        $scope.bar1 = [];

                                        angular.forEach(t.optiondetails, function (od) {
                                            $scope.bar1.push({ label: od.optionname, y: od.optionscore });
                                        });
                                        console.log($scope.bar1);
                                        // console.log(chart);

                                        var chart = "chartsContainer" + t.n;

                                        $scope.temp = $scope.temp123;

                                        chart = new CanvasJS.Chart("chartsContainer" + t.n, {

                                            animationEnabled: true,

                                            animationDuration: 50,

                                            height: 350,

                                            // colorSet: "graphcolor",

                                            axisX: {
                                                labelFontSize: 16,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black",
                                                labelAngle: -60,
                                                interval: 1

                                            },
                                            axisY: {
                                                labelFontSize: 16,
                                                labelFontWeight: "bold",
                                                labelFontColor: "black",
                                                gridThickness: 0
                                            },
                                            toolTip: {
                                                shared: true
                                            },
                                            data: [{
                                                type: "column",
                                                showInLegend: false,
                                                dataPoints: $scope.bar1
                                            }]
                                        });
                                        chart.render();

                                    }, 1000);
                                    $scope.table = false;
                                    $scope.graphs = true;
                                });

                                //$("#exportButton").click(function () {
                                //    html2canvas(document.querySelector("#chartsContainer")).then(canvas => {
                                //        var dataURL = canvas.toDataURL();

                                //        var printWindow = window.open("");
                                //        $(printWindow.document.body)
                                //            .html("<img id='Image' src=" + dataURL + "></img>")
                                //            .ready(function () {
                                //                printWindow.focus();                                             
                                //                printWindow.print();
                                //            });
                                //    });
                                //});


                                $scope.reporttype = $scope.FMTY_StakeHolderFlag;
                                angular.forEach($scope.yearlist, function (dd) {
                                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                        $scope.yearname = dd.asmaY_Year;
                                    }
                                });

                                $scope.reportname = "Feedback Report For_" + $scope.yearname + " " + $scope.reporttype;

                                $("#exportButton").click(function () {
                                    html2canvas(document.querySelector("#chartsContainer")).then(canvas => {
                                        var dataURL = canvas.toDataURL();
                                        var imgWidth = 200;
                                        var pageHeight = 1000;
                                        var imgHeight = canvas.height * imgWidth / canvas.width;
                                        var heightLeft = imgHeight;
                                        var pdf = new jsPDF();
                                        pdf.addImage(dataURL, 'JPEG', 0, 3, imgWidth, pageHeight); //addImage(image, format, x-coordinate, y-coordinate, width, height)
                                        //  heightLeft -= pageHeight;
                                        pdf.save("CanvasJS Charts.pdf");
                                    });
                                });


                                //$("#exportButton").click(function () {
                                //    html2canvas(document.querySelector("#chartsContainer")).then(canvas => {
                                //        var imgData = canvas.toDataURL();
                                //        var imgWidth = 200;
                                //        var pageHeight = 250;
                                //        var imgHeight = canvas.height * imgWidth / canvas.width;
                                //        var heightLeft = imgHeight;
                                //        var doc = new jsPDF('p', 'mm', 'a4');
                                //        var position = 5;
                                //        doc.addImage(imgData, 'PNG', 0, 3, imgWidth, imgHeight);
                                //        heightLeft -= pageHeight;

                                //        while (heightLeft >= 0) {

                                //            position = heightLeft - imgHeight;
                                //            doc.addPage();
                                //            doc.addImage(imgData, 'PNG', 0, 3, imgWidth, imgHeight);
                                //            heightLeft -= pageHeight;
                                //        }

                                //        doc.save($scope.reportname + '.pdf');
                                //    });
                                //});

                            } else {
                                swal("No Reocrd Found");
                                $scope.table = false;
                                $scope.graphs = false;
                            }

                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                            $scope.table = false;
                            $scope.graphs = false;
                        }


                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.PrintChart = function () {
            //  var base64Image = chart.canvas.toDataURL();
            // document.getElementById('rangeBarChat').style.display = 'none';
            //document.getElementById('chartImage').src = base64Image;
            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

    }

})();