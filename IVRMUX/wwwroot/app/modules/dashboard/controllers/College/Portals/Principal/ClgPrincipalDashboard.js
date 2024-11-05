(function () {
    'use strict';
    angular.module('app').controller('ClgPrincipalDashboardController', ClgPrincipalDashboardController)
    ClgPrincipalDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgPrincipalDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
        $scope.sms = 0;
        $scope.email = 0;
        $scope.staffbrthlist = [];
        $scope.stdabsentlist = [];
        $scope.loadbasicdata = function () {
            $scope.Todaydate = new Date();

            apiService.getDATA("ClgPrincipalDashboard/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.currentYear = promise.currentAcademicYear;
                $scope.notification = promise.notification;
                $scope.leavenotification = promise.leavenotification;

                $scope.staffbrthlist = promise.staffbrthlist;
                $scope.stdabsentlist = promise.stdabsentlist;

                for (var i = 0; i < $scope.yearlt.length; i++) {
                    if ($scope.currentYear[0].asmaY_Id == $scope.yearlt[i].asmaY_Id) {
                        $scope.asmaY_Id = promise.yearlist[i].asmaY_Id;
                    }
                }
                $scope.OnAcdyear();

                if (promise.smscount != null) {
                    $scope.sms = promise.smscount.length;
                }
                if (promise.emailcount != null) {
                    $scope.email = promise.emailcount.length;
                }

                if (promise.paymentNootificationCollegePrinicipal === 0) {
                    if (promise.getpaymentnotificationdetails !== null && promise.getpaymentnotificationdetails.length > 0) {
                        var ISMCLTPRP_InstallmentAmt = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentAmt;
                        var ISMCLTPRP_InstallmentName = promise.getpaymentnotificationdetails[0].ISMCLTPRP_InstallmentName;
                        var ISMCLTPRP_PaymentDate = new Date(promise.getpaymentnotificationdetails[0].ISMCLTPRP_PaymentDate);
                        var dated = $filter('date')(new Date(ISMCLTPRP_PaymentDate), 'dd/MM/yyyy');

                        var stringdisplay = "Dear Sir/Madam,\n Digital Campus Project Bill Payment is overdue as on " + dated + " Please pay for uninterrupted service.\n If already paid kindly ignore.";

                        $scope.DeletRecord(stringdisplay);
                    }
                }
            });
        };

        $scope.DeletRecord = function (stringdisplay, SweetAlert) {
            swal({
                //html: true,
                title: "Payment Subscription !",
                //text: stringdisplay,
                text: stringdisplay,
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {
                    var data = {
                        "subscriptionremarks": inputValue,
                        "paymentsubscriptiontype": "PaymentNootificationCollegePrinicipal"

                    };
                    apiService.create("InstitutionUserMapping/savepaymentremarks", data).then(function (promise) {
                        if (promise !== null) {
                            swal("Remarks Captured");
                        }
                    });
                }
            });
        };

        $scope.Principal = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/SendSMS';
            $scope.sms = true;
        };

        $scope.Timetable = function () {
            // alert("p");
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/TimeTable';
            $scope.sms = true;
        };

        //$scope.StudentSearch = function () {
        //    // alert("p");
        //    var HostName = location.host;
        //    $window.location.href = 'http://' + HostName + '/#/app/TimeTable';
        //    $scope.sms = true;
        //};


        $scope.OnAcdyear = function () {
            apiService.getURI("ClgPrincipalDashboard/GetDataByYear/", $scope.asmaY_Id).
                then(function (promise) {
                    $scope.studentstrenth = promise.fillstudentstrenth;

                    $scope.coedata = promise.coedata;
                    // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
                    $scope.absentgraph = promise.fillabsent;
                    $scope.asmaY_Id = promise.asmaY_Id;
                    $scope.feedetailsgraph = promise.fillfee;
                    if ($scope.coedata != "" && $scope.coedata != null) {
                        if ($scope.coedata.length > 0) {
                            $scope.hidecoe = true;
                        }
                    }
                    else {
                        $scope.hidecoe = false;
                    }


                    $scope.datagraph = [];
                    if ($scope.studentstrenth != null) {

                        for (var i = 0; i < $scope.studentstrenth.length; i++) {
                            $scope.datagraph.push({ label: $scope.studentstrenth[i].class_Name, "y": $scope.studentstrenth[i].stud_count })
                        }
                    }
                    console.log($scope.datagraph);

                    $scope.dataabsentgraph = [];
                    if ($scope.absentgraph != null) {

                        for (var i = 0; i < $scope.absentgraph.length; i++) {
                            $scope.dataabsentgraph.push({ label: $scope.absentgraph[i].nameOfDesig, "y": $scope.absentgraph[i].absentee })
                        }
                    }
                    console.log($scope.dataabsentgraph);



                    $scope.feegraphseries1 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries1.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].recived })
                        }
                    }


                    $scope.feegraphseries2 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries2.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].paid })
                        }
                    }

                    $scope.feegraphseries3 = [];
                    if ($scope.feedetailsgraph != null) {

                        for (var i = 0; i < $scope.feedetailsgraph.length; i++) {
                            $scope.feegraphseries3.push({ label: $scope.feedetailsgraph[i].feeclass, "y": $scope.feedetailsgraph[i].ballance })
                        }
                    }
                    console.log($scope.feegraphseries3);


                    //columnchart
                    var chart = new CanvasJS.Chart("chartContainer", {
                        //columnchart
                        responsive: true,
                        legend: {
                            maxWidth: 150,
                            fontSize: 12,

                        },
                        axisX: {
                            labelFontSize: 9,
                            interval: 1,
                            // title:"Class",
                        },
                        axisY: {
                            labelFontSize: 9,
                            // title: "Students",
                        },

                        data: [
                            {
                                type: "pie",
                                showInLegend: true,
                                toolTipContent: "{y} - #percent %",
                                legendText: "{label}",
                                dataPoints: $scope.datagraph,
                                labelFontSize: 9,

                            }
                        ]

                    });

                    chart.render();

                    var chart = new CanvasJS.Chart("areachart",
                        {
                            responsive: true,

                            axisX: {
                                labelFontSize: 9,
                                interval: 1,
                                //title: "Department",
                            },
                            axisY: {
                                labelFontSize: 9,
                                // title: "No.of. Staffs",

                            },

                            legend: {
                                maxWidth: 300,
                                fontSize: 10,
                            },


                            data: [
                                {
                                    type: "pie",
                                    showInLegend: true,
                                    toolTipContent: "{y} - #percent %",
                                    legendText: "{label}",
                                    dataPoints: $scope.dataabsentgraph
                                }
                            ]
                        });

                    chart.render();


                    var chart = new CanvasJS.Chart("rangeBarChat");

                    chart.options.axisX = { interval: 1, labelAngle: -20, labelFontSize: 11 };

                    // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                    var series1 = { //dataSeries - first quarter
                        type: "column",
                        name: "receivable",
                        showInLegend: true
                    };

                    var series2 = { //dataSeries - second quarter
                        type: "column",
                        name: "collected",
                        showInLegend: true
                    };

                    var series3 = { //dataSeries - Third quarter
                        type: "column",
                        name: "balance",
                        showInLegend: true
                    };


                    chart.options.data = [];
                    chart.options.data.push(series1);
                    chart.options.data.push(series2);
                    chart.options.data.push(series3);

                    series1.dataPoints = $scope.feegraphseries1;
                    series2.dataPoints = $scope.feegraphseries2;
                    series3.dataPoints = $scope.feegraphseries3;
                    chart.render();
                });
        }

        var HostName = location.host;

        $scope.oncertificate = function () {
            $window.location.href = 'http://' + HostName + '/#/app/OnlineTransferCertificate/';
        };
        $scope.onleave = function () {
            $window.location.href = 'http://' + HostName + '/#/app/OnlineLeaveApp/';
        };

        $scope.stdabs = function () {
            // $window.location.href = 'http://' + HostName + '/#/app/ClassWiseDailyAttendance/';
        };
    }
})();