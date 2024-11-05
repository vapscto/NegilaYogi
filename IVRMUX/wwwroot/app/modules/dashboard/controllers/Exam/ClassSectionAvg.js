(function () {
    'use strict';
    angular
.module('app')
.controller('ClassSectionAvgController', ClassSectionAvgController)

    ClassSectionAvgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ClassSectionAvgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("ClassSectionAvg/getdetails").
            then(function (promise) {

                $scope.qualification_type = 'individual';
                $scope.acdlist = promise.acdlist;
                $scope.catlist = promise.catlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.sublist = promise.sublist;
                $scope.examlist = promise.examlist;
            })
        };


        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("ClassSectionAvg/onselectCategory", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("ClassSectionAvg/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type == 'all') {
                $scope.sec_disable = true;
                $scope.ASMS_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                $scope.sec_disable = false;
            }
        };

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.qualification_type == 'all') {
                    $scope.ASMS_Id = 0;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "report_type": $scope.qualification_type,
                    "check_type": $scope.emp_checked
                }

                apiService.create("ClassSectionAvg/onreport", data).
                then(function (promise) {
                    if ($scope.qualification_type == 'all') {
                        $scope.ASMS_Id = "";
                    }
                    $scope.main_list = promise.datareport;

                    $scope.stud_graph = [];
                    if (promise.datareport != "0" && promise.datareport != null) {
                        $scope.studgraph = false;
                        for (var i = 0; i < $scope.main_list.length; i++) {
                            $scope.stud_graph.push({ label: $scope.main_list[i].estmpS_ClassAverage, "y": $scope.main_list[i].estmpS_ClassAverage })
                        }

                        $scope.stud_graph2 = [];

                        for (var i = 0; i < $scope.main_list.length; i++) {
                            $scope.stud_graph2.push({ label: $scope.main_list[i].estmpS_SectionAverage, "y": parseInt($scope.main_list[i].estmpS_SectionAverage) })
                        }


                        var chart = new CanvasJS.Chart("linechart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            axisX: {
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "Class Average",
                                showInLegend: true,
                                type: "column",
                                color: "red",
                                dataPoints: $scope.stud_graph
                            },
                            {
                                name: "Section Average",
                                showInLegend: true,
                                type: "column",
                                color: "green",
                                dataPoints: $scope.stud_graph2
                            }
                            ]
                        });
                        chart.render();
                    }
                    else {
                        swal("No Record Found....")
                    }


                    
                })
            }
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EMCA_Id = '';
            $scope.EME_Id = '';
            $scope.ISMS_Id = '';
            $scope.Main_list = [];
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