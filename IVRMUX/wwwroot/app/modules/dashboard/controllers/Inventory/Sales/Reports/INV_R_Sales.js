
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_R_SalesController', INV_R_SalesController);
    INV_R_SalesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function INV_R_SalesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


        $scope.tablediv = false;
        $scope.printsale = false;
        $scope.typeflag = "All";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.imgheader = false;
        //===============================================================================

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_R_Sales/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_salesno = promise.get_salesno;
                });
        };
        //=====================================Main Radio Change
        $scope.mainrdochange = function () {
            $scope.tablediv = false;
            $scope.get_salesdetails = "";
            $scope.get_class = "";
            $scope.get_Section = "";
            $scope.get_Studentlist = "";
            $scope.startdate = "";
            $scope.enddate = "";
            $scope.individualflag = false;
            $scope.bw_dates = false;
            var data = {
                "type": $scope.optionflag
            };
            apiService.create("INV_R_Sales/mainradiochange", data).
                then(function (promise) {
                    if (promise.get_salesdetails.length > 0) {
                        $scope.get_salesdetails = promise.get_salesdetails;
                    }
                    else if ($scope.optionflag === 'All') {
                        $scope.get_salesdetails = "";
                    }
                    else {

                        swal("No Record found..");
                    }
                });
        };
        //=====================================Radio Change
        $scope.onrdochange = function () {
            $scope.tablediv = false;
            $scope.get_salesdetails = "";
            $scope.get_class = "";
            $scope.get_Section = "";
            $scope.get_Studentlist = "";
            $scope.get_SalesReport = "";
            $scope.startdate = "";
            $scope.enddate = "";
            var data = {
                "type": $scope.individualflag
            };
            apiService.create("INV_R_Sales/radiochange", data).
                then(function (promise) {
                    if (promise.get_class.length > 0) {
                        $scope.get_class = promise.get_class;
                    }
                    else {
                        swal("No Record found..");
                    }
                });
        };
        //=================On Class and Class/Section Change Get Student List
        $scope.classchange = function () {
            $scope.get_salesdetails = "";
            $scope.get_Section = "";
            $scope.get_Studentlist = "";
            $scope.get_SalesReport = "";
            $scope.startdate = "";
            $scope.enddate = "";
            var data = {};
            if ($scope.individualflag === "C") {
                data = {
                    "ASMCL_Id": $scope.asmcL_Id,
                    "type": $scope.individualflag
                };
            }
            if ($scope.individualflag === "CS") {
                data = {
                    "ASMCL_Id": $scope.asmcL_Id,
                    "type": $scope.individualflag
                };
            }
            apiService.create("INV_R_Sales/getStudentlist", data).
                then(function (promise) {
                    if ($scope.individualflag === "C") {
                        if (promise.get_Studentlist.length > 0) {
                            $scope.get_Studentlist = promise.get_Studentlist;
                        }
                        else {
                            swal("No Record found..");
                        }
                    }
                    else {
                        if (promise.get_Section.length > 0) {
                            $scope.get_Section = promise.get_Section;
                        }
                        else {
                            swal("No Record found..");
                        }
                    }
                });
        };

        $scope.sectionchange = function () {
            $scope.get_salesdetails = "";
            $scope.get_Studentlist = "";
            $scope.get_SalesReport = "";
            $scope.startdate = "";
            $scope.enddate = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "type": "C"
            };
            apiService.create("INV_R_Sales/getStudentlist", data).
                then(function (promise) {
                    if (promise.get_Studentlist.length > 0) {
                        $scope.get_Studentlist = promise.get_Studentlist;
                    }
                    else {
                        swal("No Record found..");
                    }
                });
        };


        //===================================All check and validation

        //================ Store No
        $scope.togchkbxSTR = function () {
            $scope.storeall = $scope.get_salesdetails.every(function (itm) {
                return itm.storeck;
            });
        };
        $scope.isOptionsRequiredSTR = function () {
            return !$scope.get_salesdetails.some(function (options) {
                return options.storeck;
            });
        };

        $scope.all_checkSTR = function (storeal) {
            $scope.storeall = storeal;
            var toggleStatus = $scope.storeall;
            angular.forEach($scope.get_salesdetails, function (store) {
                store.storeck = toggleStatus;
            });
        };
        //================ Sale No
        $scope.togchkbxS = function () {
            $scope.saleall = $scope.get_salesdetails.every(function (itm) {
                return itm.saleck;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.get_salesdetails.some(function (options) {
                return options.saleck;
            });
        };
        $scope.all_check = function (saleal) {
            $scope.saleall = saleal;
            var toggleStatus = $scope.saleall;
            angular.forEach($scope.get_salesdetails, function (sale) {
                sale.saleck = toggleStatus;
            });
        };

        //========================ITEM
        $scope.togchkbxI = function () {
            $scope.itemall = $scope.get_salesdetails.every(function (itm) {
                return itm.itemck;
            });
        };
        $scope.isOptionsRequiredI = function () {
            if ($scope.individualflag === 'I') {
                return !$scope.get_salesdetails.some(function (options) {
                    return options.itemck;
                });
            } else {
                return false;
            }
        };
        $scope.all_checkI = function (itmi) {
            $scope.itemall = itmi;
            var toggleStatus = $scope.itemall;
            angular.forEach($scope.get_salesdetails, function (itm) {
                itm.itemck = toggleStatus;
            });
        };
        //===================Class
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.get_Studentlist.every(function (itm) {
                return itm.clsck;
            });
            console.log($scope.usercheckC);
        };
        $scope.isOptionsRequired = function () {
            if ($scope.individualflag === 'C') {
                return !$scope.get_Studentlist.some(function (options) {
                    return options.clsck;
                });
            }
            else if ($scope.individualflag === 'CS') {

                return !$scope.get_Studentlist.some(function (options) {
                    return options.secck;
                });
            } else {
                return false;
            }

        };
        $scope.all_checkC = function (adasd) {
            $scope.usercheckC = adasd;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.get_Studentlist, function (cls) {
                cls.clsck = toggleStatus;
            });
        };
        $scope.togchkbxCS = function () {
            $scope.usercheckCS = $scope.get_Studentlist.every(function (options) {
                return options.clsck;
            });
        };
        $scope.isOptionsRequired1 = function () {
            if ($scope.individualflag === 'CS') {

                return !$scope.get_Studentlist.some(function (options) {
                    return options.secck;
                });
            } else {
                return false;
            }
        };

        //-==================================SALES Report
        $scope.onreport = function () {
            $scope.get_SalesReport = [];
            $scope.salesReport = [];
            $scope.start_Date = $filter('date')($scope.startdate, "yyyy-MM-dd");
            $scope.end_Date = $filter('date')($scope.enddate, "yyyy-MM-dd");
            var data = {};
            var totalamt = 0;
            var totaltax = 0;
            var totaldiscount = 0;
            if ($scope.bw_dates === true) {
                $scope.sDate = $scope.start_Date;
                $scope.eDate = $scope.end_Date;
            }
            else {
                $scope.sDate = "";
                $scope.eDate = "";
            }

            if ($scope.typeflag === "All") {
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": "",
                    "typeflag": $scope.typeflag,
                    "hrme_id": "0",
                    "amst_id": "0",
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": "0"
                };
            }

            //else if ($scope.optionflag === "Store") {
            //    $scope.storenoarray = [];
            //    angular.forEach($scope.get_salesdetails, function (store) {
            //        if (store.storeck === true) {
            //            $scope.storenoarray.push(store);
            //        }
            //    });
            //    data = {
            //        "startdate": $scope.sDate,
            //        "enddate": $scope.eDate,
            //        "optionflag": $scope.optionflag,
            //        "typeflag": $scope.typeflag,
            //        "hrme_id": "0",
            //        "storenoarray": $scope.storenoarray,
            //        "invmsl_id": "0",
            //        "invmi_id": "0",
            //        "invmc_id": "0",
            //        "selectionflag": $scope.individualflag
            //    };
            //}

            else if ($scope.optionflag === "Saleno") {
                $scope.salenoarray = [];
                angular.forEach($scope.get_salesdetails, function (sale) {
                    if (sale.saleck === true) {
                        $scope.salenoarray.push(sale);
                    }
                });
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": $scope.optionflag,
                    "typeflag": $scope.typeflag,
                    "hrme_id": "0",
                    "salenoarray": $scope.salenoarray,
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": "0",
                    "selectionflag": $scope.individualflag
                };
            }

            else if ($scope.optionflag === "Student") {
                if ($scope.individualflag !== "C" && $scope.individualflag !== "CS" || $scope.individualflag === false) {
                    data = {
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "hrme_id": "0",
                        "studentid": $scope.obj.amsT_Id.AMST_Id,
                        "invmsl_id": "0",
                        "invmi_id": "0",
                        "invmc_id": "0",
                        "selectionflag": $scope.individualflag
                    };
                }
                else if ($scope.individualflag === "C") {
                    $scope.clsarray = [];
                    angular.forEach($scope.get_Studentlist, function (cls) {
                        if (cls.clsck === true) {
                            $scope.clsarray.push(cls);
                        }
                    });
                    data = {
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "hrme_id": "0",
                        "clsarray": $scope.clsarray,
                        "invmsl_id": "0",
                        "invmi_id": "0",
                        "invmc_id": "0",
                        "selectionflag": $scope.individualflag
                    };
                }

                else if ($scope.individualflag === "CS") {
                    $scope.secarray = [];
                    angular.forEach($scope.get_Studentlist, function (sec) {
                        if (sec.clsck === true) {
                            $scope.secarray.push(sec);
                        }
                    });
                    data = {
                        "startdate": $scope.sDate,
                        "enddate": $scope.eDate,
                        "optionflag": $scope.optionflag,
                        "typeflag": $scope.typeflag,
                        "hrme_id": "0",
                        "secarray": $scope.secarray,
                        "invmsl_id": "0",
                        "invmi_id": "0",
                        "invmc_id": "0",
                        "selectionflag": $scope.individualflag
                    };
                }
            }

            else if ($scope.optionflag === "Item") {
                $scope.itemarray = [];
                angular.forEach($scope.get_salesdetails, function (itm) {
                    if (itm.itemck === true) {
                        $scope.itemarray.push(itm);
                    }
                });
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": $scope.optionflag,
                    "typeflag": $scope.typeflag,
                    "hrme_id": "0",
                    "itemarray": $scope.itemarray,
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": "0",
                    "selectionflag": $scope.individualflag
                };
            }

            else if ($scope.optionflag === "Staff") {
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": $scope.optionflag,
                    "typeflag": $scope.typeflag,
                    "hrme_id": $scope.HRME_Id.HRME_Id,
                    "amst_id": "0",
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": "0",
                    "selectionflag": $scope.individualflag
                };
            }

            else if ($scope.optionflag === "Customer") {
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": $scope.optionflag,
                    "typeflag": $scope.typeflag,
                    "hrme_id": "0",
                    "amst_id": "0",
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": obj.INVMC_Id.INVMC_Id,
                    "selectionflag": $scope.individualflag
                };
            }

            apiService.create("INV_R_Sales/onreport", data).
                then(function (promise) {

                    $scope.get_SalesReport = promise.get_SalesReport;
                    $scope.presentCountgrid = $scope.get_SalesReport.length;
                    if (promise.get_SalesReport.length > 0) {
                        $scope.tablediv = true;
                        $scope.printsale = true;

                        if ($scope.typeflag === "All") {
                            angular.forEach($scope.get_SalesReport, function (sl) {
                                totalamt += parseFloat(sl.INVTSL_Amount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);

                                totaltax += parseFloat(sl.INVTSL_TaxAmt);
                                $scope.totaltax = totaltax;
                                $scope.totaltax = parseFloat($scope.totaltax);
                                $scope.totaltax = $scope.totaltax.toFixed(2);

                                totaldiscount += parseFloat(sl.INVTSL_DiscountAmt);
                                $scope.totaldiscount = totaldiscount;
                                $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                $scope.totaldiscount = $scope.totaldiscount.toFixed(2);

                            });
                            $scope.colarrayall = [{
                                title: "SLNO",
                                template: "<span class='row-number'></span>", width: "80px"
                            },
                            {
                                name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                            },
                            {
                                name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sale No.', width: "140px"
                            },
                            {
                                name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                            },
                            {
                                name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Quantity', width: "100px"
                            },
                            {
                                name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Price', width: "100px"
                            },

                            //{
                            //    name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: "100px"
                            //},
                            //{
                            //    name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: "100px"
                            //},
                            {
                                name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                            }
                            ];
                            if ($scope.tax === 1) {
                                $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                            }
                            if ($scope.discount === 1) {
                                $scope.colarrayall.push({ name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: 100 });
                            }
                        }
                        else if ($scope.typeflag === "Detailed") {

                            if ($scope.optionflag === "Item") {
                                angular.forEach($scope.get_SalesReport, function (sl) {
                                    totalamt += parseFloat(sl.INVTSL_Amount);
                                    $scope.totalamt = totalamt;
                                    $scope.totalamt = parseFloat($scope.totalamt);
                                    $scope.totalamt = $scope.totalamt.toFixed(2);

                                    totaltax += parseFloat(sl.INVTSL_TaxAmt);
                                    $scope.totaltax = totaltax;
                                    $scope.totaltax = parseFloat($scope.totaltax);
                                    $scope.totaltax = $scope.totaltax.toFixed(2);

                                    totaldiscount += parseFloat(sl.INVTSL_DiscountAmt);
                                    $scope.totaldiscount = totaldiscount;
                                    $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                    $scope.totaldiscount = $scope.totaldiscount.toFixed(2);

                                });

                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sale No.', width: "140px"
                                },
                                {
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                },
                                {
                                    name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Price', width: "100px"
                                },
                                //{
                                //    name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: "100px"
                                //},
                                //{
                                //    name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: 100 });
                                }
                            }

                            else if ($scope.optionflag === "Student") {
                                angular.forEach($scope.get_SalesReport, function (sl) {
                                    totalamt += parseFloat(sl.saleAmount);
                                    $scope.totalamt = totalamt;
                                    $scope.totalamt = parseFloat($scope.totalamt);
                                    $scope.totalamt = $scope.totalamt.toFixed(2);

                                    totaltax += parseFloat(sl.saleTax);
                                    $scope.totaltax = totaltax;
                                    $scope.totaltax = parseFloat($scope.totaltax);
                                    $scope.totaltax = $scope.totaltax.toFixed(2);

                                    totaldiscount += parseFloat(sl.saleDiscount);
                                    $scope.totaldiscount = totaldiscount;
                                    $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                    $scope.totaldiscount = $scope.totaldiscount.toFixed(2);

                                });
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sale No.', width: "140px"
                                },
                                {
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                },
                                {
                                    name: 'membername', field: 'membername', title: 'Name', width: "150px"
                                },
                                {
                                    name: 'saleQty', field: 'saleQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'salePrice', field: 'salePrice', title: 'Price', width: "100px"
                                },
                                //{
                                //    name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: "100px"
                                //},
                                //{
                                //    name: 'saleTax', field: 'saleTax', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }
                                ];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: 100 });
                                }
                            }

                            else if ($scope.optionflag === "Saleno") {
                                var totalamt1 = 0;
                                var totaltax1 = 0;
                                var totaldiscount1 = 0;

                                angular.forEach($scope.get_SalesReport, function (sl) {
                                    totalamt1 += parseFloat(sl.INVTSL_Amount);
                                    $scope.totalamt1 = totalamt1;
                                    $scope.totalamt1 = parseFloat($scope.totalamt1);
                                    $scope.totalamt1 = $scope.totalamt1.toFixed(2);

                                    totaltax1 += parseFloat(sl.INVTSL_TaxAmt);
                                    $scope.totaltax1 = totaltax1;
                                    $scope.totaltax1 = parseFloat($scope.totaltax1);
                                    $scope.totaltax1 = $scope.totaltax1.toFixed(2);

                                    totaldiscount1 += parseFloat(sl.INVTSL_DiscountAmt);
                                    $scope.totaldiscount1 = totaldiscount1;
                                    $scope.totaldiscount1 = parseFloat($scope.totaldiscount1);
                                    $scope.totaldiscount1 = $scope.totaldiscount1.toFixed(2);

                                });
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'INVMSL_SalesNo', field: 'INVMSL_SalesNo', title: 'Sale No.', width: "140px"
                                },
                                {
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },

                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                },
                                {
                                    name: 'membername', field: 'membername', title: 'Staff Name', width: "150px"
                                },
                                {
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Price', width: "100px"
                                },

                                //{
                                //    name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: "100px"                                            
                                //},
                                //{
                                //    name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'INVTSL_DiscountAmt', field: 'INVTSL_DiscountAmt', title: 'Discount', width: 100 });
                                }
                            }

                        }

                        else if ($scope.typeflag === "Overall") {
                            angular.forEach($scope.get_SalesReport, function (sl) {
                                totalamt += parseFloat(sl.saleAmount);
                                $scope.totalamt = totalamt;
                                $scope.totalamt = parseFloat($scope.totalamt);
                                $scope.totalamt = $scope.totalamt.toFixed(2);

                                totaltax += parseFloat(sl.saleTax);
                                $scope.totaltax = totaltax;
                                $scope.totaltax = parseFloat($scope.totaltax);
                                $scope.totaltax = $scope.totaltax.toFixed(2);

                                totaldiscount += parseFloat(sl.saleDiscount);
                                $scope.totaldiscount = totaldiscount;
                                $scope.totaldiscount = parseFloat($scope.totaldiscount);
                                $scope.totaldiscount = $scope.totaldiscount.toFixed(2);

                            });
                            if ($scope.optionflag === "Item") {

                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'INVMI_ItemName', field: 'INVMI_ItemName', title: 'Item', width: "150px"
                                },
                                {
                                    name: 'saleQty', field: 'saleQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'salePrice', field: 'salePrice', title: 'Price', width: "100px"
                                },
                                //{
                                //    name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: "100px"
                                //},
                                //{
                                //    name: 'saleTax', field: 'saleTax', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: 100 });
                                }
                            }

                            else if ($scope.optionflag === "Student") {
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'membername', field: 'membername', title: 'Student Name', width: "150px"
                                },
                                {
                                    name: 'saleQty', field: 'saleQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'salePrice', field: 'salePrice', title: 'Price', width: "100px"
                                },
                                //{
                                //    name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: "100px"
                                //},
                                //{
                                //    name: 'saleTax', field: 'saleTax', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: 100 });
                                }
                            }

                            else if ($scope.optionflag === "Staff") {
                                $scope.colarrayall = [{
                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: "80px"
                                },
                                {
                                    name: 'membername', field: 'membername', title: 'Staff Name', width: "150px"
                                },
                                {
                                    name: 'saleQty', field: 'saleQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'salePrice', field: 'salePrice', title: 'Price', width: "100px"
                                },
                                //{
                                //    name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: "100px"
                                //},
                                //{
                                //    name: 'saleTax', field: 'saleTax', title: 'Tax', width: "100px"
                                //},
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.tax === 1) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.discount === 1) {
                                    $scope.colarrayall.push({ name: 'saleDiscount', field: 'saleDiscount', title: 'Discount', width: 100 });
                                }
                            }

                        }

                        $(document).ready(function () {
                            initGridall();
                        });
                        function initGridall() {
                            $('#gridall').empty();
                            //gridall =
                            $("#gridall").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "SalesReport.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                pdf: {
                                    avoidLinks: true,
                                    landscape: true,
                                    repeatHeaders: true,
                                    fileName: "SalesReport.pdf",
                                    allPages: true
                                },
                                dataSource: {
                                    data: $scope.get_SalesReport,
                                    pageSize: 10,
                                },
                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,
                                columns: $scope.colarrayall,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize()
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }

                            }).data("kendoGrid");
                        }
                    }
                    else {
                        swal("No Record Found...!!");
                        $scope.get_SalesReport = "";
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        };
        //======================================Print & Export to Excel
        //$scope.exportToExcel = function (export_id) {

        //    var exportHref = Excel.tableToExcel(export_id, 'printsale');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 100);
        //};
        $scope.printData = function () {
            var innerContents = document.getElementById("printsale").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            $scope.imgheader = true;
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();