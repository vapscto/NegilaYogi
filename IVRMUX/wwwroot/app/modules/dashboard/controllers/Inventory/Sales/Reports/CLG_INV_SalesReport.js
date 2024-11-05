
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLG_INV_SalesReportController', CLG_INV_SalesReportController);
    CLG_INV_SalesReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function CLG_INV_SalesReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {


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
            apiService.getURI("CLG_INV_SalesReport/getloaddata", pageid).
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
            if ($scope.optionflag === "Student") {
                $scope.optiontype = "ClgStudent";
            }
            else {
                $scope.optiontype = $scope.optionflag;
            }
            var data = {
                "type": $scope.optiontype
            };
            apiService.create("CLG_INV_SalesReport/mainradiochange", data).
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
        //===========Get Branch List on Course Change
        $scope.oncoursechange = function () {
            $scope.get_Studentlist = [];
            var data = {
                "AMCO_Id": $scope.amcO_Id
            };
            apiService.create("CLG_INV_T_Sales/getbranchlist", data).
                then(function (promise) {
                    $scope.branch_list = promise.branch_list;
                });
        };
        //=================Get Semester List on branch change
        $scope.onbranchchange = function () {
            var data = {};
            data = {
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id
            };
            apiService.create("CLG_INV_T_Sales/getsemesterlist", data).
                then(function (promise) {
                    $scope.sem_list = promise.sem_list;
                });
        };

        //=================Get Student List on Semester change
        $scope.onsemesterchange = function () {
            var data = {};
            data = {
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id
            };
            apiService.create("CLG_INV_T_Sales/getStudentlist", data).
                then(function (promise) {
                    $scope.get_Studentlist = promise.get_Studentlist;
                });
        };



        //===================================All check and validation

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
        //==================Select All Student Check      
        $scope.all_checkStu = function (studt) {
            $scope.usercheckStu = studt;
            var toggleStatus = $scope.usercheckStu;
            angular.forEach($scope.get_Studentlist, function (std) {
                std.stuck = toggleStatus;
            });
        };
        $scope.togchkbxStu = function () {
            $scope.usercheckCS = $scope.get_Studentlist.every(function (options) {
                return options.stuck;
            });
        };
        $scope.isOptionsRequiredStu = function () {
            return !$scope.get_Studentlist.some(function (options) {
                return options.stuck;
            });
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
                $scope.clgstuarray = [];
                angular.forEach($scope.get_Studentlist, function (st) {
                    if (st.stuck === true) {
                        $scope.clgstuarray.push(st);
                    }
                });
                data = {
                    "startdate": $scope.sDate,
                    "enddate": $scope.eDate,
                    "optionflag": $scope.optionflag,
                    "typeflag": $scope.typeflag,
                    "hrme_id": "0",
                    "clgstuarray": $scope.clgstuarray,
                    "invmsl_id": "0",
                    "invmi_id": "0",
                    "invmc_id": "0",
                    "selectionflag": $scope.individualflag
                };
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

            apiService.create("CLG_INV_SalesReport/onreport", data).
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

                            {
                                name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                            }
                            ];
                            if ($scope.totaltax.length > 0) {
                                $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                            }
                            if ($scope.totaldiscount.length > 0) {
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
                                {
                                    name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.totaltax.length > 0) {
                                    $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount.length > 0) {
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
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }
                                ];
                                if ($scope.totaltax.length > 0) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount.length > 0) {
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
                                    name: 'INVMSL_SalesDate', field: 'INVMSL_SalesDate', title: 'Sale Date', width: "130px", template: "#= kendo.toString(kendo.parseDate(INVMSL_SalesDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"
                                },
                                {
                                    name: 'INVTSL_SalesQty', field: 'INVTSL_SalesQty', title: 'Quantity', width: "100px"
                                },
                                {
                                    name: 'INVTSL_SalesPrice', field: 'INVTSL_SalesPrice', title: 'Price', width: "100px"
                                },

                                {
                                    name: 'INVTSL_Amount', field: 'INVTSL_Amount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.totaltax1.length > 0) {
                                    $scope.colarrayall.push({ name: 'INVTSL_TaxAmt', field: 'INVTSL_TaxAmt', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount1.length > 0) {
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
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.totaltax.length > 0) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount.length > 0) {
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
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                if ($scope.totaltax.length > 0) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount.length > 0) {
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
                                {
                                    name: 'saleAmount', field: 'saleAmount', title: 'Amount', width: "100px"
                                }];
                                 if ($scope.totaltax.length > 0) {
                                    $scope.colarrayall.push({ name: 'saleTax', field: 'saleTax', title: 'Tax', width: 100 });
                                }
                                if ($scope.totaldiscount.length > 0) {
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