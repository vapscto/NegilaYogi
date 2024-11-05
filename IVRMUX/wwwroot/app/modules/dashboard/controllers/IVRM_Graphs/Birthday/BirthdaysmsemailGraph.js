(function () {
    'use strict';
    angular
        .module('app')
        .controller('BirthdaysmsemailGraphController', BirthdaysmsemailGraphController);
    BirthdaysmsemailGraphController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function BirthdaysmsemailGraphController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.LoadData = function () {
            var pageid = 1;
            apiService.getURI("BirthDayGraphs/", pageid).
                then(function (promise) {
                    $scope.AccdemicYear = promise.accyear;
                });
        }
        $scope.Report = function () {
            $scope.submitted = true; $scope.sms_mail_count = [];
            $scope.templist = []; $scope.templisttwo = []; $scope.monthname = [];
            if ($scope.myForm.$valid) {                
                var data = {
                    // "ASMAY_IdList": $scope.ASMAY_IdList,
                    "ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("BirthDayGraphs/Sendmsg", data).then(function (promise) {  
                    $scope.sms_mail_count = promise.sms_mail_count;            
                    if ($scope.sms_mail_count != null && $scope.sms_mail_count.length > 0) {  
                        angular.forEach($scope.sms_mail_count, function (ddy) {                            
                            $scope.templist.push(ddy.email);   
                            $scope.templisttwo.push(ddy.sms); 
                            $scope.monthname.push(ddy.IVRM_Month_Name); 
                        })
                        $("#chart123").kendoChart({
                            title: {
                                text: "Month wise Student B'day Email and SMS Count"
                            },
                            legend: {
                                position: "top"
                            },
                            seriesDefaults: {
                                type: "column"
                            },
                            series: [{
                                name: "Email Count",
                                color: "#2874A6 ",
                                data: $scope.templist,
                            },
                            {
                                name: "SMS Count",
                                color: "#F3A00E",
                                data: $scope.templisttwo,
                            },                              
                            ],
                            valueAxis: {
                                labels: {
                                    format: "{0}%"
                                },
                                line: {
                                    visible: false
                                },
                                axisCrossingValue: 0
                            },
                            categoryAxis: {
                                 categories: $scope.monthname,
                                line: {
                                    visible: false
                                },
                                labels: {
                                    padding: { top: 10 }
                                }
                            },
                            tooltip: {
                                visible: true,
                                format: "{0}%",
                                template: "#= series.name #: #= value #"
                            }
                        });
                        function createChart1() {
                            $("#chart1").kendoChart({
                                title: {
                                    text: "Month wise Student B'day Email and SMS Count"
                                },
                                legend: {
                                    position: "bottom"
                                },
                                chartArea: {
                                    background: ""
                                },
                                seriesDefaults: {
                                    type: "line",
                                    style: "smooth"
                                },
                                series: [{
                                    name: "Email Count",
                                    data: $scope.templist,
                                }, {
                                    name: "SMS Count",
                                        data: $scope.templisttwo,
                                },
                                ],
                                valueAxis: {
                                    labels: {
                                        format: "{0}%"
                                    },
                                    line: {
                                        visible: false
                                    },
                                    axisCrossingValue: -10
                                },
                                categoryAxis: {
                                    categories: $scope.monthname,
                                    majorGridLines: {
                                        visible: false
                                    },
                                    labels: {
                                        rotation: "auto"
                                    }
                                },
                                tooltip: {
                                    visible: true,
                                    format: "{0}%",
                                    template: "#= series.name #: #= value #"
                                }
                            });
                        }
                        $(document).ready(createChart1);
                        $(document).bind("kendo:skinChange", createChart1);
                    }
                    else {
                        swal("Record Not Found !");
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.clear = function () {
            $state.reload();
        }
    }
})();