
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_QuotationComparisonController', INV_QuotationComparisonController);
    INV_QuotationComparisonController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function INV_QuotationComparisonController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var date = new Date();
        $scope.invmpI_Doc_Date = date;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_QuotationComparison/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_piNo = promise.get_piNo;
                })
        };

        ////===================================== PI Change
        $scope.onpichange = function (itemid) {
            $scope.get_pisupplier = "";
            $scope.get_Comparison = "";
            var pi_id = itemid.invmpI_Id;

            var data = {
                "INVMPI_Id": pi_id,
            }
            apiService.create("INV_QuotationComparison/getpisupplier", data).
                then(function (promise) {
                    $scope.get_pisupplier = promise.get_pisupplier;
                    $scope.pinum = $scope.get_pisupplier[0].invmpI_PINo;
                })
        }

        //===================================== View
        $scope.onformclick = function (itemid) {
            $scope.get_qtdetails = "";
            var pi_id = itemid.invmpI_Id;
            var qt_id = itemid.invmsQ_Id;
            var data = {
                "INVMPI_Id": pi_id,
                "INVMSQ_Id": qt_id
            }
            apiService.create("INV_QuotationComparison/getqtdetails", data).
                then(function (promise) {

                    $scope.get_qtdata = promise.get_qtdetails;
                    $scope.tolNegoted = $scope.invmsQ_NegotiatedRate;
                    $scope.qtnumber = $scope.get_qtdata[0].invmsQ_QuotationNo;
                    $scope.supname = $scope.get_qtdata[0].invmsQ_SupplierName;
                    $scope.supemail = $scope.get_qtdata[0].invmsQ_SupplierEmailId;
                    $scope.supcontno = $scope.get_qtdata[0].invmsQ_SupplierContactNo;

                    $scope.get_qtdetails = [];
                    angular.forEach($scope.get_qtdata, function (objqt) {
                        $scope.get_qtdetails.push({
                            invmsQ_Id: objqt.invmsQ_Id, invtsQ_Id: objqt.invtsQ_Id, invmI_Id: objqt.invmI_Id, invmsQ_QuotationNo: objqt.invmsQ_QuotationNo, invmsQ_SupplierName: objqt.invmsQ_SupplierName,
                            invmsQ_SupplierEmailId: objqt.invmsQ_SupplierEmailId, invmsQ_SupplierContactNo: objqt.invmsQ_SupplierContactNo,
                            invmI_ItemName: objqt.invmI_ItemName, invmuoM_UOMName: objqt.invmuoM_UOMName, invtsQ_QuotedRate: objqt.invtsQ_QuotedRate, invtsQ_NegotiatedRate: objqt.invtsQ_NegotiatedRate,
                            invmsQ_NegotiatedRate: objqt.invmsQ_NegotiatedRate, invmpI_Id: objqt.invmpI_Id, invmsQ_TotalQuotedRate: objqt.invmsQ_TotalQuotedRate
                        })
                    });
                    //if ($scope.newQuotation_list.length > 0) {
                    //    angular.forEach($scope.newQuotation_list, function (qlst) {
                    //        angular.forEach($scope.get_qtdata, function (objqt) {
                    //            if (qlst.invmsQ_Id == objqt.invmsQ_Id) {
                    //                if (qlst.invtsQ_Id == objqt.invtsQ_Id) {
                    //                    $scope.get_qtdetails.push({
                    //                        invmsQ_Id: objqt.invmsQ_Id, invtsQ_Id: objqt.invtsQ_Id, invmI_Id: objqt.invmI_Id, invmsQ_QuotationNo: objqt.invmsQ_QuotationNo, invmsQ_SupplierName: objqt.invmsQ_SupplierName,invmsQ_SupplierEmailId: objqt.invmsQ_SupplierEmailId, invmsQ_SupplierContactNo: objqt.invmsQ_SupplierContactNo,
                    //                        invmI_ItemName: objqt.invmI_ItemName, invmuoM_UOMName: objqt.invmuoM_UOMName, invtsQ_QuotedRate: objqt.invtsQ_QuotedRate, invtsQ_NegotiatedRate: objqt.invtsQ_NegotiatedRate, invmsQ_NegotiatedRate: objqt.invmsQ_NegotiatedRate, invmpI_Id: objqt.invmpI_Id,
                    //                        invmsQ_TotalQuotedRate: objqt.invmsQ_TotalQuotedRate
                    //                    })

                    //                }
                    //            }

                    //        })
                    //    });
                    //}
                    //else {

                    //}





                    //var b = 0;
                    //angular.forEach($scope.get_qtdetails, function (obj) {
                    //    b += parseFloat(obj.invtsQ_NegotiatedRate);
                    //})
                    //var ntamt = b;
                    //$scope.tol_NegotiatedRate = ntamt;
                    //$scope.tol_NegotiatedRate = parseFloat($scope.tol_NegotiatedRate);
                    //$scope.tol_NegotiatedRate = $scope.tol_NegotiatedRate.toFixed(2);
                })
        }
        //===================================== Supplier Grid Select All
        $scope.toggleAll = function () {
            angular.forEach($scope.get_qtdetails, function (subj) {
                subj.xyz = $scope.all;
            })
        };
        $scope.optionToggled = function () {
            var t = 0;
            angular.forEach($scope.get_qtdetails, function (qts) {
                if (qts.xyz) {
                    t += parseFloat(qts.invtsQ_NegotiatedRate);
                }
            })
            var tamt = t;
            $scope.invmsQ_NegotiatedRate = tamt;
            $scope.invmsQ_NegotiatedRate = parseFloat($scope.invmsQ_NegotiatedRate);
            $scope.invmsQ_NegotiatedRate = $scope.invmsQ_NegotiatedRate.toFixed(2);
            $scope.tol_NegotiatedRate = $scope.invmsQ_NegotiatedRate;

            $scope.all = $scope.get_qtdetails.every(function (itm) {
                return itm.xyz;
            });

        };
        // ============================ Count Total
        $scope.countQCAmt = function (objc) {
            var b = 0;
            angular.forEach($scope.get_qtdetails, function (obj) {
                if (obj.xyz) {
                    b += parseFloat(obj.invtsQ_NegotiatedRate);
                }
            })
            var ntamt = b;
            $scope.invmsQ_NegotiatedRate = ntamt;
            $scope.invmsQ_NegotiatedRate = parseFloat($scope.invmsQ_NegotiatedRate);
            $scope.invmsQ_NegotiatedRate = $scope.invmsQ_NegotiatedRate.toFixed(2);
            $scope.tol_NegotiatedRate = $scope.invmsQ_NegotiatedRate;

            angular.forEach($scope.get_pisupplier, function (objsp) {
                angular.forEach($scope.get_qtdetails, function (obj) {
                    if (objsp.invmsQ_Id == obj.invmsQ_Id) {
                        objsp.invmsQ_NegotiatedRate = $scope.invmsQ_NegotiatedRate;
                    }
                })
            })
        }
        //========================================== Add to Cart  
        $scope.newQuot_list = [];
        $scope.newQuotation_list = [];
        $scope.addtocart = function (objtx) {

            angular.forEach($scope.get_qtdetails, function (oobj) {
                if (oobj.xyz) {
                    //  $scope.newQuot_list.push(oobj);
                    $scope.newQuot_list.push({
                        invmsQ_Id: oobj.invmsQ_Id, invtsQ_Id: oobj.invtsQ_Id, invmI_Id: oobj.invmI_Id, invmsQ_QuotationNo: oobj.invmsQ_QuotationNo, invmsQ_SupplierName: oobj.invmsQ_SupplierName,
                        invmsQ_SupplierEmailId: oobj.invmsQ_SupplierEmailId, invmsQ_SupplierContactNo: oobj.invmsQ_SupplierContactNo,
                        invmI_ItemName: oobj.invmI_ItemName, invmuoM_UOMName: oobj.invmuoM_UOMName, invtsQ_QuotedRate: oobj.invtsQ_QuotedRate, invtsQ_NegotiatedRate: oobj.invtsQ_NegotiatedRate,
                        invmsQ_NegotiatedRate: parseFloat($scope.tol_NegotiatedRate), invmpI_Id: oobj.invmpI_Id, invmsQ_TotalQuotedRate: oobj.invmsQ_TotalQuotedRate
                    })
                }
            })

            if ($scope.newQuot_list.length > 0) {
                $scope.newQuotation_list = $scope.newQuot_list;
            }
            else {
                swal("Select Atleast One Checkbox....!!");
            }


        }
        ////===================================== Comparison
        //$scope.getComparison = function () {
        //    $scope.arrayQSupplier = [];
        //    angular.forEach($scope.get_pisupplier, function (spl) {
        //        if (spl.xyz) {
        //            $scope.arrayQSupplier.push(spl);
        //        }
        //    });
        //    var data = {
        //        "arrayQSupplier": $scope.arrayQSupplier
        //    }
        //    apiService.create("INV_QuotationComparison/get_Comparison", data).
        //        then(function (promise) {
        //            $scope.get_Comparison = promise.get_Comparison;
        //        })

        //};

        //===================================== SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayQcompare = [];
                angular.forEach($scope.newQuotation_list, function (oobj) {
                    $scope.arrayQcompare.push({
                        invmsQ_Id: oobj.invmsQ_Id, invtsQ_Id: oobj.invtsQ_Id, invmI_Id: oobj.invmI_Id, invmsQ_QuotationNo: oobj.invmsQ_QuotationNo, invmsQ_SupplierName: oobj.invmsQ_SupplierName,
                        invmsQ_SupplierEmailId: oobj.invmsQ_SupplierEmailId, invmsQ_SupplierContactNo: oobj.invmsQ_SupplierContactNo,
                        invmI_ItemName: oobj.invmI_ItemName, invmuoM_UOMName: oobj.invmuoM_UOMName, invtsQ_QuotedRate: oobj.invtsQ_QuotedRate, invtsQ_NegotiatedRate: oobj.invtsQ_NegotiatedRate,
                        invmsQ_NegotiatedRate: oobj.invmsQ_NegotiatedRate, invmpI_Id: oobj.invmpI_Id, invmsQ_TotalQuotedRate: oobj.invmsQ_TotalQuotedRate
                    });
                })

                data = {
                    "arrayQcompare": $scope.arrayQcompare,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_QuotationComparison/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmsQ_Id == 0 || promise.invmsQ_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmsQ_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsQ_Id == 0 || promise.invmsQ_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsQ_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //$scope.savedata = function () {
        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {


        //        var invmsqid = 0;
        //        angular.forEach($scope.get_Comparison, function (cp) {
        //            if (cp.fs == "true") {
        //                invmsqid = cp.invmsQ_Id;
        //            }
        //        });
        //        var data = {
        //            "INVMSQ_Id": invmsqid
        //        }
        //        apiService.create("INV_QuotationComparison/savedata", data).
        //            then(function (promise) {
        //                if (promise.returnval == true) {
        //                    swal('Record saved successfully');
        //                }
        //                else {
        //                    swal('Failed to save, please contact administrator');
        //                }
        //                $state.reload();
        //            })
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //};




        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVOB_Id = item.invoB_Id;
            var dystring = "";
            if (item.invoB_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invoB_ActiveFlg == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("INV_PurchaseIndent/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            $scope.invmsT_Id = item.invmsT_Id;
            $scope.invmI_Id = item.invmI_Id;
            $scope.invmuom_id = item.invmuoM_Id;
            $scope.invoB_BatchNo = item.invoB_BatchNo;
            $scope.invoB_PurchaseDate = new Date(item.invoB_PurchaseDate);
            $scope.invoB_PurchaseRate = item.invoB_PurchaseRate;
            $scope.invoB_SaleRate = item.invoB_SaleRate;
            $scope.invoB_DiscountAmt = item.invoB_DiscountAmt;
            $scope.invoB_TaxAmt = item.invoB_TaxAmt;
            $scope.invoB_Amount = item.invoB_Amount;
            $scope.invoB_Qty = item.invoB_Qty;
            $scope.invoB_Naration = item.invoB_Naration;
            $scope.invoB_MfgDate = new Date(item.invoB_MfgDate);
            $scope.invoB_ExpDate = new Date(item.invoB_ExpDate);
            $scope.invoB_Id = item.invoB_Id;
            $scope.onitemchange(item.invmI_Id);
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();