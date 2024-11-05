﻿

(function () {
    'use strict';
    angular
        .module('app')
        .controller('InstituteWiseFeeCollectionController', InstituteWiseFeeCollectionController)

    InstituteWiseFeeCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function InstituteWiseFeeCollectionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.tablegraph = false;
        $scope.tablegrid = false;

        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();
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
        $scope.isOptionsRequired = function () {
            return !$scope.arrlist6.some(function (options) {
                return options.selected;
            });
        }
        //$scope.isOptionsRequired1 = function () {
        //    return !$scope.arrlist7.some(function (options) {
        //        return options.selected1;
        //    });
        //}
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.loadbasicdata = function () {
            $scope.fields();


            apiService.getDATA("InstituteWiseFeeCollection/Getdetails").
                then(function (promise) {



                    $scope.arrlist6 = promise.institutename;
                    //$scope.arrlist7 = promise.monthname;

                });

        $scope.showsectionGrid = function () {
            if ($scope.myForm.$valid) {
                var ASMAY_Ids = [];
                angular.forEach($scope.arrlist6, function (ty) {
                    if (ty.selected) {
                        ASMAY_Ids.push(ty.MI_Id);
                    }
                })

                //var monthids = [];
                //angular.forEach($scope.arrlist7, function (ty) {
                //    if (ty.selected1) {
                //        monthids.push(ty.ivrM_Month_Id);
                //    }
                //})
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


                var data = {

                    mi_ids: ASMAY_Ids,
                   // monthids: monthids,
                    COEE_EStartDate: $scope.from_date,
                    COEE_EEndDate: $scope.to_date

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("InstituteWiseFeeCollection/Getsectioncount", data).
                    then(function (promise) {
                        $scope.get_approvalreport = [];
                        $scope.plannerid = [];
                        $scope.institutename = [];
                        $scope.monthname = [];
                        $scope.COLLECTION = [];
                        $scope.BALANCE = [];
                        $scope.CONCESSION = [];
                        $scope.WAIVED = [];
                        $scope.REBATE = [];
                        $scope.FINE = [];
                        $scope.RECEIVABLE = [];

                        if (promise.sectionarray.length > 0 && promise.sectionarray.length != null) {
                            $scope.tablegrid = true;
                            if (promise.sectionarray.length > 0) {
                                $scope.get_approvalreport = promise.sectionarray;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.plannerid.length === 0) {

                                        $scope.plannerid.push(dev);
                                        $scope.institutename.push(dev.mI_Name);
                                    } else if ($scope.plannerid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.plannerid, function (emp) {
                                            if (emp.mI_Id === dev.mI_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.plannerid.push(dev);
                                            $scope.institutename.push(dev.mI_Name);
                                        }
                                    }
                                });

                                angular.forEach($scope.plannerid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.get_approvalreport, function (dd) {
                                        if (dd.mI_Id === ddd.mI_Id) {
                                            $scope.templist.push(dd);
                                            $scope.monthname.push(dd.ivrM_Month_Name);
                                            $scope.COLLECTION.push(dd.paid);
                                            $scope.BALANCE.push(dd.ballance);
                                            $scope.CONCESSION.push(dd.concession);
                                            $scope.WAIVED.push(dd.waived);
                                            $scope.REBATE.push(dd.rebate);
                                            $scope.FINE.push(dd.fine);
                                            $scope.RECEIVABLE.push(dd.receivable);

                                        }
                                    });
                                    //if ($scope.templist.length > 0) {
                                    //for (var i = 0; i < $scope.templist.length; i++) {
                                    //$scope.monthname.push(promise.sectionarray[i].ivrM_Month_Name);
                                    //$scope.COLLECTION.push(promise.sectionarray[i].paid);
                                    //$scope.BALANCE.push(promise.sectionarray[i].ballance);
                                    //$scope.CONCESSION.push(promise.sectionarray[i].concession);
                                    //$scope.WAIVED.push(promise.sectionarray[i].waived);
                                    //$scope.REBATE.push(promise.sectionarray[i].rebate);
                                    //$scope.FINE.push(promise.sectionarray[i].fine);
                                    //$scope.RECEIVABLE.push(promise.sectionarray[i].receivable);
                                    //}
                                    // }

                                    ddd.feeinstallmentdata = $scope.templist;
                                });
                            }





                            


                            function createChart1() {

                                $("#chart1").kendoChart({
                                    title: {
                                        text: "Institute Wise Fee Collection Graph"
                                    },
                                    legend: {
                                        position: "top"
                                    },
                                    seriesDefaults: {
                                        type: "column",
                                        style: "smooth"
                                    },


                                    series: [{
                                        name: "COLLECTION",
                                        data: $scope.COLLECTION,
                                    }, {
                                        name: "BALANCE",
                                        data: $scope.BALANCE,

                                    }, {
                                        name: "CONCESSION",
                                        data: $scope.CONCESSION,
                                    }, {
                                        name: "WAIVED",
                                        data: $scope.WAIVED,
                                    }, {
                                        name: "REBATE",
                                        data: $scope.REBATE,

                                    }, {
                                        name: "FINE",
                                        data: $scope.FINE,
                                    }, {
                                        name: "RECEIVABLE",
                                        data: $scope.RECEIVABLE,
                                    }
                                    ],
                                    valueAxis: {
                                        labels: {
                                            format: "{0}"
                                        },
                                        line: {
                                            visible: true
                                        },
                                        axisCrossingValue: 0
                                    },
                                    categoryAxis: [{
                                        categories: $scope.monthname,
                                        line: {
                                            visible: true,

                                        }
                                    },
                                    {
                                        categories: $scope.institutename,
                                        line: {
                                            visible: true,
                                            step: $scope.institutename.length,
                                        }
                                    }
                                    ],
                                    tooltip: {
                                        visible: true,
                                        format: "{0}",
                                        template: "#= series.name #: #= value #"
                                    }
                                });



                            }

                            $(document).ready(createChart1);
                            $(document).bind("kendo:skinChange", createChart1);










                        }
                        else {
                            swal("Record Not Found");
                        }





                    })
            }
            else {
                $scope.submitted = true;

            }
        }

   

        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.arrlist6, function (itm) {
                itm.selected = toggleStatus1;
            });
        }
        //$scope.hrdallcheck1 = function () {
        //    var toggleStatus1 = $scope.checkallhrd1;
        //    angular.forEach($scope.arrlist7, function (itm) {
        //        itm.selected1 = toggleStatus1;
        //    });
        //}

        $scope.oninstitutechg = function () {

            $scope.checkallhrd = $scope.arrlist6.every(function (itm) {

                return itm.selected;
            });
        }
        //$scope.onmonthchg = function () {

        //    $scope.checkallhrd1 = $scope.arrlist7.every(function (itm) {

        //        return itm.selected1;
        //    });
        //}


        $scope.Clearid = function () {

            $scope.tablegrid = false;
            $state.reload();
        }

    };
})();