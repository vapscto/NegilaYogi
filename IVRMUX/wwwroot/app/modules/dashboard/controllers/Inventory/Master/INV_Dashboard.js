
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_DashboardController', INV_DashboardController);
    INV_DashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_DashboardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = '';
        $scope.typeflg = "Stock";
        $scope.expiredays = 0;
        $scope.CurrentDate = new Date();
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.usrname = localStorage.getItem('username');
        CanvasJS.addColorSet("graphcolor",
            [
                "#00c0ef ",
                "#4fc0b5",
                "#fdc0bc",
                "#a893f7",
                "#fddd6a"
            ]);
        //================ Tab Click
        $scope.onStock = function () {
            $scope.typeflg = "Stock";
            $scope.expiredays = 0;
            $scope.loaddata();
        };
        $scope.onTag = function () {
            $scope.typeflg = "Tag";
            $scope.expiredays = 30;
            $scope.loaddata();
        };
        //===============================Load Page
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 5;
            $scope.itemsPerPage1 = 5;
            $scope.search = "";
            var data = {
                "typeflg": $scope.typeflg,
                "expiredays": $scope.expiredays
            };
            apiService.create("INV_Dashboard/getloaddata", data).
                //   apiService.getURI("INV_Dashboard/getloaddata", pageid).
                then(function (promise) {

                    //========== Purchase
                    if (promise.totPurchase > 0) {
                        $scope.totPurchase = promise.totPurchase;
                    }
                    else {
                        $scope.totPurchase = 0;
                    }
                    //========== Sales
                    if (promise.totSales > 0) {
                        $scope.totSales = promise.totSales;
                    }
                    else {
                        $scope.totSales = 0;
                    }
                    //========== Items
                    if (promise.totItem > 0) {
                        $scope.totItem = promise.totItem;
                    }
                    else {
                        $scope.totItem = 0;
                    }
                    //========== AvailableStock
                    if (promise.totAvailableStock > 0) {
                        $scope.totAvailableStock = promise.totAvailableStock;
                    }
                    else {
                        $scope.totAvailableStock = 0;
                    }
                    //========== Checkout-Item 
                    if (promise.totCheckout > 0) {
                        $scope.totCheckout = promise.totCheckout;
                    }
                    else {
                        $scope.totCheckout = 0;
                    }
                    //========== Low Stock item
                    if (promise.totalowStock.length > 0) {
                        $scope.totalowStock = promise.totalowStock;
                        $scope.totlowStock = $scope.totalowStock.length;
                    }
                    else {
                        $scope.totalowStock = "";
                        $scope.totlowStock = 0;
                    }
                    //========= Warranty Expire
                    if (promise.totalWexpire.length > 0) {
                        $scope.totalWexpire = promise.totalWexpire;
                        $scope.totalexpire = $scope.totalWexpire.length;
                    }
                    else {
                        $scope.totalWexpire = "";
                        $scope.totalexpire = 0;
                    }
                    //======== Warranty Expired
                    if (promise.totalWexpired.length > 0) {
                        $scope.totalWexpired = promise.totalWexpired;
                        $scope.totalexpired = $scope.totalWexpired.length;
                    }
                    else {
                        $scope.totalWexpired = "";
                        $scope.totalexpired = 0;
                    }
                    //======== GRID
                    if (promise.dashboardgrid.length > 0) {
                        $scope.dashboardgrid = promise.dashboardgrid;
                    }
                    else {
                        $scope.dashboardgrid = "";
                        $scope.dashboardgrid = 0;
                    }
                    //============================ Graph                 
                    $scope.overallDetails = [];
                    $scope.overallDetails.push({ label: "Purchase", y: $scope.totPurchase });
                    $scope.overallDetails.push({ label: "Sales", y: $scope.totSales });
                    $scope.overallDetails.push({ label: "Check-Out", y: $scope.totCheckout });
                    $scope.overallDetails.push({ label: "Available Stock", y: $scope.totAvailableStock });
                    $scope.overallDetails.push({ label: "Expire", y: $scope.totalexpire });
                    $scope.overallDetails.push({ label: "Expired", y: $scope.totalexpired });
                    var chart = new CanvasJS.Chart("chartPie", {
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 300,
                        // width: 350,
                        theme: "light2",
                        colorSet: "graphcolor",
                        title: {
                            horizontalAlign: "left"
                        },
                        axisX: {
                            interval: 1,
                            labelFontSize: 12
                        },
                        axisY: {
                            labelFontSize: 12
                        },
                        data: [
                            {
                                type: "doughnut",
                                indexLabelFontSize: 12,
                                startAngle: 60,
                                showInLegend: false,
                                dataPoints: $scope.overallDetails
                            }
                        ]
                    });
                    chart.render();
                    function explodePie(e) {
                        if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                            e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                        } else {
                            e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                        }
                        e.chart.render();
                    }


                    //============== Column Chart For low Stock Item    
                    $scope.lowStockgraph = [];
                    angular.forEach($scope.totalowStock, function (lsi) {
                        $scope.lowStockgraph.push({ label: lsi.invmI_ItemName, y: parseFloat(lsi.invstO_AvaiableStock), name: lsi.invmI_ItemName });
                    });

                    chart = new CanvasJS.Chart("chartColumn", {
                        animationEnabled: true,
                        animationDuration: 3000,
                        height: 300,
                        //  width: 650,
                        colorSet: "graphcolor",
                        title: {
                            text: "Low Stock Items"
                        },
                        axisX: {
                            labelFontSize: 12
                        },
                        axisY: {
                            title: "Avaiable Stock",
                            labelFontSize: 12
                        },
                        toolTip: {
                            shared: true
                        },
                        data: [
                            {
                                type: "column",
                                legendText: "Item's",
                                showInLegend: false,
                                dataPoints: $scope.lowStockgraph
                            }
                        ]
                    });
                    chart.render();

                });
        };

        //========================= warranty Model
        $scope.ViewexpireModel = function () {
            $scope.expiredays = 15;
            $scope.typeflg = "Tag";
            var data = {
                "typeflg": $scope.typeflg,
                "expiredays": $scope.expiredays
            };
            apiService.create("INV_Dashboard/getwarrantydetails", data).
                then(function (promise) {

                    if (promise.warrantydetails.length > 0) {
                        $scope.warrantydetails = promise.warrantydetails;
                    }
                    else {
                        $scope.warrantydetails = "";
                    }

                });

        };
        $scope.getexpiredays = function (expiredays) {
            if ($scope.expiredays === 0 || $scope.expiredays === '') {
                $scope.expdays = 15;
            }
            else {
                $scope.expdays = expiredays;
            }
            $scope.typeflg = "Tag";
            var data = {
                "typeflg": $scope.typeflg,
                "expiredays": $scope.expdays
            };
            apiService.create("INV_Dashboard/getwarrantydetails", data).
                then(function (promise) {

                    if (promise.warrantydetails.length > 0) {
                        $scope.warrantydetails = promise.warrantydetails;
                    }
                    else {
                        $scope.warrantydetails = "";
                    }

                });

        };
    }
})();