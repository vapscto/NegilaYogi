

(function () {
    'use strict';
    angular
        .module('app')
        .controller('HODPaycareStaffDetailsController', HODPaycareStaffDetailsController)

    HODPaycareStaffDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'appSettings']
    function HODPaycareStaffDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, appSettings) {

        $scope.tbgrid = false;
        $scope.tbgraph = false;
        $scope.tbgraphdes = false;
        $scope.totalregstudent = 0;

        $scope.totalnewstudent = 0;
        $scope.sms = 0;
        $scope.email = 0;
        $scope.fields = function () {

            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];


            $scope.Todaydate = new Date();
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        if ($scope.itemsPerPage == undefined) {
            $scope.itemsPerPage = 15
        }

        $scope.loadbasicdata = function () {
            $scope.filldesiganation = [];
            $scope.departmentgraph = [];
            apiService.getDATA("HODPaycareStaffDetails/Getdetails").
                then(function (promise) {



                    $scope.departmentdropdown = promise.departmentdropdown;
                    if ($scope.departmentdropdown != null) {
                        $scope.hrmD_Id = promise.hrmd_id;

                        $scope.filldesiganation = promise.filldesiganation;

                        $scope.departmentgraph = promise.departmentgraph;


                        if ($scope.filldesiganation.length > 0) {
                            $scope.tbgrid = true;
                            $scope.tbgraphdes = true;
                        }


                        $scope.designationloadcharts();

                        $scope.dtploadcharts();

                        if ($scope.departmentgraph.length > 0) {
                            $scope.tbgraph = true;
                        }

                    }
                    else {
                        swal("No Record");
                    }




                })

        }

        $scope.Ondepartment = function (hrmD_Id) {
            $scope.tbgrid = false;
            $scope.tbgraph = false;
            $scope.submitted = true;
            $scope.tbgraphdes = false;
            if ($scope.myForm.$valid) {
                $scope.fields();
                var data = {
                    "hrmd_id": hrmD_Id,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("HODPaycareStaffDetails/ondptchange", data).
                    then(function (promise) {

                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.filldesiganation = promise.filldesiganation;
                        if ($scope.departmentdropdown.length > 0) {
                            $scope.hrmD_Id = promise.hrmd_id;

                            $scope.designationloadcharts();

                            $scope.tbgraph = true;
                        }
                        if ($scope.filldesiganation.length > 0) {
                            $scope.tbgrid = true;

                            $scope.tbgraphdes = true;

                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.showempGrid = function (hrmdeS_Id) {
            $scope.emppop = [];
            var data = {
                "hrmd_id": $scope.hrmD_Id,
                "HRMDES_Id": hrmdeS_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODPaycareStaffDetails/Getemppop", data).
                then(function (promise) {



                    $scope.emppop = promise.employeeDetails;

                    $scope.exam_list = [];
                    $scope.overalltotalmax = 0;
                    angular.forEach($scope.emppop, function (st) {
                        if ($scope.exam_list.length == 0) {
                            $scope.exam_list.push({ hrmE_Id: st.hrmE_Id, doj: st.doj, empname: st.empname, gender: st.gender, mobileno: st.mobileno, email: st.email, mstatus: st.mstatus });
                        }
                        else if ($scope.exam_list.length > 0) {
                            var al_exm_cnt = 0;
                            angular.forEach($scope.exam_list, function (exm) {
                                if (exm.hrmE_Id == st.hrmE_Id) {
                                    al_exm_cnt += 1;
                                }
                            })
                            if (al_exm_cnt == 0) {

                                $scope.exam_list.push({ hrmE_Id: st.hrmE_Id, doj: st.doj, empname: st.empname, gender: st.gender, mobileno: st.mobileno, email: st.email, mstatus: st.mstatus });
                            }
                        }
                    })

                    console.log($scope.exam_list);
                    $scope.emppop = $scope.exam_list;

                    if ($scope.emppop.length > 0) {
                        angular.forEach($scope.emppop, function (t) {
                            if (t.doj != null) {
                                t.doj = $filter('date')(new Date(t.doj), 'dd/MM/yyyy');
                            }
                        })
                    }
                })
        }



        $scope.dtploadcharts = function () {
            $scope.dptdatagraph = [];
            if ($scope.departmentgraph != null) {

                for (var i = 0; i < $scope.departmentgraph.length; i++) {
                    $scope.dptdatagraph.push({ label: $scope.departmentgraph[i].departmentName, "y": $scope.departmentgraph[i].depttotalEmployees })
                }
            }

            var chart = new CanvasJS.Chart("rangeBarChat", {
                height: 260,
                width: 1000,
                axisX: {
                    labelFontSize: 12,
                    //interval: 1,
                    //labelAngle: -20
                    // title:"Class",
                },
                axisY: {
                    labelFontSize: 12,
                    //  title: "Students",
                },

                data: [
                    {
                        type: "column",
                        showInLegend: true,
                        dataPoints: $scope.dptdatagraph

                    }
                ]

            });

            chart.render();

        }



        $scope.designationloadcharts = function () {
            $scope.desdatagraph = [];
            if ($scope.filldesiganation != null) {

                for (var i = 0; i < $scope.filldesiganation.length; i++) {
                    $scope.desdatagraph.push({ label: $scope.filldesiganation[i].designationname, "y": $scope.filldesiganation[i].totalEmployees })
                }
            }

            var chart = new CanvasJS.Chart("designationgraph", {
                height: 260,
                width: 1000,
                axisX: {
                    labelFontSize: 12,
                    //interval: 1,
                    //labelAngle: -20
                    // title:"Class",
                },
                axisY: {
                    labelFontSize: 12,
                    //  title: "Students",
                },

                data: [
                    {
                        type: "column",
                        showInLegend: true,
                        dataPoints: $scope.desdatagraph

                    }
                ]

            });

            chart.render();

        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

    };
})();