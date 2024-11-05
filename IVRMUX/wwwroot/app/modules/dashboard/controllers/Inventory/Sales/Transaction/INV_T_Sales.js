
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_T_SalesController', INV_T_SalesController);
    INV_T_SalesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_T_SalesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        var date = new Date();
        $scope.invmsL_SalesDate = date;

        $scope.saletrans = false;
        //  $scope.transgrid = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings!=null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //======================Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings[i].imN_Flag === "INVSALES") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
            
        }
       
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addsalesrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.transrows.length > 1) {
                    $scope.cnt = $scope.transrows.length;
                }
                $scope.cnt = $scope.cnt + 1;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.removesalesrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            if (data.amstB_Id > 0) {
                $scope.Deletesalesrows(data);
            }
        };
        //==============================Page Load========================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("INV_T_Sales/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_employee = promise.get_employee;
                    $scope.get_customer = promise.get_customer;
                    $scope.get_Store = promise.get_Store;
                    $scope.get_Product = promise.get_Product;
                    $scope.get_Sale = promise.get_Sale;
                    $scope.presentCountgrid = $scope.get_Sale.length;
                });
        };

        //===========Individual,Class,Class/Section Radio Change
        //$scope.mainradiochange = function () {
        //    if ($scope.invmsL_StuOtherFlg === "Student") {
        //        $scope.get_employee = "";
        //        $scope.get_customer = "";
        //    }
        //    else if ($scope.invmsL_StuOtherFlg === "Staff") {
        //        $scope.loaddata();
        //        $scope.studentlist = "";
        //        $scope.get_customer = "";
        //    }
        //    else if ($scope.invmsL_StuOtherFlg === "Customer") {
        //        $scope.loaddata();
        //        $scope.studentlist = "";
        //        $scope.get_employee = "";

        //    }

        //};

        $scope.radioChange = function () {
            $scope.get_Studentlist = [];
            $scope.searchValue1 = "";
            if ($scope.invmsL_StuOtherFlg === "Student") {
                if ($scope.obj.stu_Flag === "C" || $scope.obj.stu_Flag === "CS") {
                    $scope.transgrid = false;
                }
                else {
                    $scope.transgrid = true;
                }
            }
            var data = {
                "type": $scope.obj.stu_Flag
            };
            apiService.create("INV_T_Sales/getStudentClsSec", data).
                then(function (promise) {
                    $scope.get_Student_Cls_Sec = promise.get_Student_Cls_Sec;
                });
        };

        //=================On Class Change Get Student List
        $scope.classSectionflag = function () {
            var data = {};
            $scope.get_Studentlist = "";
            if ($scope.obj.stu_Flag === "C") {
                data = {
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "type": $scope.obj.stu_Flag
                };
            }
            //if ($scope.obj.stu_Flag === "CS") {
            //    //angular.forEach($scope.get_Student_Cls_Sec, function (cl) {
            //    //    if (cl.ASMS_Id === $scope.asmS_Id) {
            //    //        $scope.cls_id = cl.ASMCL_Id;
            //    //    }
            //    //});
            //    data = {
            //        "ASMCL_Id": $scope.asmcL_Id,
            //       // "ASMS_Id": $scope.asmS_Id,
            //        "type": $scope.obj.stu_Flag
            //    };
            //}
            apiService.create("INV_T_Sales/getStudentlist", data).
                then(function (promise) {
                    $scope.get_Studentlist = promise.get_Studentlist;
                });
        };
        //=================On Class/Section Change Get Student List
        $scope.classSectionC = function () {
            $scope.get_Studentlist = "";
            $scope.get_sectionlist = [];
            var data = {};
            data = {
                "ASMCL_Id": $scope.obj.asmcL_Id,
                "type": $scope.obj.stu_Flag
            };

            apiService.create("INV_T_Sales/getsectionlist", data).
                then(function (promise) {
                    $scope.get_sectionlist = promise.get_sectionlist;
                });
        };
        //=================get student On class and section selection
        $scope.classSectionCS = function () {
            var data = {};
            data = {
                "ASMCL_Id": $scope.obj.asmcL_Id,
                "ASMS_Id": $scope.obj.asmS_Id,
                "type": $scope.obj.stu_Flag
            };

            apiService.create("INV_T_Sales/getStudentlist", data).
                then(function (promise) {
                    $scope.get_Studentlist = promise.get_Studentlist;
                });
        };

        //==================Select All Check
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.get_Studentlist.every(function (itm) {
                return itm.clsck;
            });
            console.log($scope.usercheckC);
        };
        $scope.isOptionsRequired = function () {
            if ($scope.invmsL_StuOtherFlg === 'Student') {
                return !$scope.get_Studentlist.some(function (options) {
                    return options.clsck;
                });
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
                return options.secck;
            });
        };
        $scope.isOptionsRequired1 = function () {
            if ($scope.obj.stu_Flag === 'CS' && $scope.invmsL_StuOtherFlg === 'Student') {
                return !$scope.get_Studentlist.some(function (options) {
                    return options.secck;
                });
            } else {
                return false;
            }

        };
        $scope.all_checkCS = function (adasd) {
            $scope.usercheckCS = adasd;
            var toggleStatus = $scope.usercheckCS;
            angular.forEach($scope.get_Studentlist, function (sec) {
                sec.secck = toggleStatus;
            });
        };

        $scope.showtrans = function () {
            var stucnt = 0;
            if ($scope.get_Studentlist.length > 0) {
                angular.forEach($scope.get_Studentlist, function (cl) {
                    if (cl.clsck === true) {
                        stucnt += 1;
                    }
                    if (cl.secck === true) {
                        stucnt += 1;
                    }
                });
                if (stucnt > 0) {
                    $scope.transgrid = true;
                }
                else {
                    swal("Select Atleast One Checkbox..!!");

                }

            }

        };

        //===================== Get Item on store Change
        $scope.storeChange = function () {
            var data = {
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("INV_T_Sales/getitem", data).
                then(function (promise) {
                    if (promise.get_item.length > 0) {
                        $scope.transgrid = true;
                        $scope.get_item = promise.get_item;
                    }
                    else {
                        swal('For Selected Store, No Item found..!!');
                        $scope.get_item.length = 0;
                    }
                });
        };

        //===========Item Change Transcation Grid
        $scope.onitemchange = function (itemid, objid) {
            $scope.get_itemTax1 = [];
            var data = {};
            var qqty = 0;
            var dsqty = 0;
            var ttqqty = 0;
            var disqqty = 0;
            var stucnt = 0;
            var avstock = 0;
            var INVSTO_BatchNo = "";
            if ($scope.get_item != null && $scope.get_item.length > 0) {
                angular.forEach($scope.get_item, function (ast) {
                    if (ast.INVMI_Id == itemid.INVMI_Id && ast.INVSTO_BatchNo == itemid.INVSTO_BatchNo) {
                        INVSTO_BatchNo = ast.INVSTO_BatchNo;
                    }
                    
                });
            }
            if ($scope.editS === true) {
                data = {
                    "INVMI_Id": itemid.invmI_Id,
                    "INVSTO_SalesRate": itemid.invtsL_SalesPrice,
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVSTO_BatchNo": INVSTO_BatchNo
                };
            }
            else {
                data = {
                    "INVMI_Id": itemid.INVMI_Id,
                    "INVSTO_SalesRate": itemid.INVSTO_SalesRate,
                    "INVMST_Id": $scope.invmsT_Id,
                    "INVSTO_BatchNo": INVSTO_BatchNo
                };
            }
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    $scope.get_itemTax1 = promise.get_itemTax;
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.availableitems = avstock;

                    angular.forEach($scope.transrows, function (obj) {
                        if ($scope.editS === true) {
                            if (obj.itrS_Id === itemid.itrS_Id) {
                                obj.get_itemTax = $scope.get_itemTax1;
                                $scope.get_itemTax = $scope.get_itemTax1;
                                obj.invmuoM_UOM = $scope.get_itemDetail[0].invmuoM_UOMName;
                                obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                                obj.invtsL_BatchNo = $scope.get_itemDetail[0].invstO_BatchNo;
                                if ($scope.get_Studentlist.length > 0) {
                                    angular.forEach($scope.get_Studentlist, function (cl) {
                                        if (cl.clsck === true || cl.secck === true) {
                                            stucnt += 1;
                                        }
                                    });
                                    if (stucnt > 0) {
                                        qqty = parseFloat(obj.invtsL_SalesQty);
                                        dsqty = parseFloat(obj.invtsL_DiscountAmt);
                                        ttqqty = qqty / stucnt;
                                        disqqty = dsqty / stucnt;
                                        obj.salqty = ttqqty;
                                        obj.disctammt = disqqty;
                                    }
                                }
                            }
                        }
                        else {
                            if (obj.itrS_Id === objid.itrS_Id) {
                                obj.get_itemTax = $scope.get_itemTax1;
                                $scope.get_itemTax = $scope.get_itemTax1;
                                obj.invmuoM_UOM = $scope.get_itemDetail[0].invmuoM_UOMName;
                                obj.invmuoM_Id = $scope.get_itemDetail[0].invmuoM_Id;
                                obj.invtsL_BatchNo = $scope.get_itemDetail[0].invstO_BatchNo;
                                //obj.invtsL_SalesPrice = $scope.get_itemDetail[0].invstO_SalesRate;
                                obj.invtsL_SalesQty = 0.00;
                                obj.invtsL_DiscountAmt = 0.00;
                                obj.invtsL_Amount = 0.00;
                                obj.invtsL_TaxAmt = 0.00;
                            }
                        }

                    });
                    if ($scope.invmsL_TotalAmount === 0 || $scope.invmsL_TotalAmount === "" || $scope.invmsL_TotalAmount === undefined || $scope.invmsL_TotalAmount === "NaN") {
                        $scope.invmsL_TotalAmount = 0.00;
                    }
                    if ($scope.invmsL_TotDiscount === 0 || $scope.invmsL_TotDiscount === "" || $scope.invmsL_TotDiscount === undefined || $scope.invmsL_TotDiscount === "NaN") {
                        $scope.invmsL_TotDiscount = 0.00;
                    }
                    if ($scope.invmsL_TotTaxAmt === 0 || $scope.invmsL_TotTaxAmt === "" || $scope.invmsL_TotTaxAmt === undefined || $scope.invmsL_TotTaxAmt === "NaN") {
                        $scope.invmsL_TotTaxAmt = 0.00;
                    }

                    //$scope.countAmt(objid);
                    //$scope.countDiscount(objid);
                });
        };

        //==========================Tax View and Add To Cart
        $('#myModal').modal('hide');
        $scope.viewtxfn = function (qwe) {
            if ($scope.editS === true) {
                $scope.purrate = parseFloat(qwe.invtsL_SalesPrice);
            }
            else {
                $scope.purrate = parseFloat(qwe.invmI_Id.INVSTO_SalesRate);
            }
            //$scope.purrate = parseFloat(qwe.invmI_Id.INVSTO_SalesRate);
            $scope.qty = parseFloat(qwe.invtsL_SalesQty);
            $scope.totdiscout = parseFloat(qwe.invtsL_DiscountAmt);
            $('#myModal').modal('show');
            $scope.get_itemTax = qwe.get_itemTax;
            var cnt = 0;
            angular.forEach($scope.get_itemTax, function (wer) {
                wer.itrS_Id = qwe.itrS_Id;
                $scope.totpurrate = $scope.purrate * $scope.qty;
                if (qwe.invtsL_DiscountAmt === undefined || qwe.invtsL_DiscountAmt === "0" || qwe.invtsL_DiscountAmt === "") {
                    $scope.finalpurrate = $scope.totpurrate;
                }
                else {
                    $scope.finalpurrate = $scope.totpurrate - $scope.totdiscout;
                }
                var taxamt = ($scope.finalpurrate * wer.invmiT_TaxValue) / 100;
                wer.invtslT_TaxAmt = taxamt;//.toFixed(2);
                wer.invtslT_TaxAmt = parseFloat(wer.invtslT_TaxAmt);
                wer.invtslT_TaxAmt = wer.invtslT_TaxAmt.toFixed(2);
                cnt = cnt + wer.invtslT_TaxAmt;
            });
        };

        //=============================Tax Amount        
        $scope.addtocart = function (itemtax) {
            $scope.arraySaletax = [];
            $scope.arraytax = [];
            var ccnt = 0;
            angular.forEach(itemtax, function (tax) {
                $scope.tsid = tax.itrS_Id;
                $scope.arraySaletax.push({ invmT_Id: tax.invmT_Id, invtslT_TaxPer: tax.invmiT_TaxValue, invtslT_TaxAmt: tax.invtslT_TaxAmt, invtslT_Id: tax.invtslT_Id });
                ccnt = ccnt + tax.invtslT_TaxAmt;
            });

            angular.forEach($scope.transrows, function (obj) {
                var tx = 0.00;
                angular.forEach(obj.get_itemTax, function (obj1) {
                    if (obj.itrS_Id === obj1.itrS_Id) {
                        tx += parseFloat(obj1.invtslT_TaxAmt);
                    }
                });
                obj.invtsL_TaxAmt = tx;
            });
            var t = 0;
            angular.forEach($scope.transrows, function (obj) {
                angular.forEach(obj.get_itemTax, function (obj1) {
                    t += parseFloat(obj1.invtslT_TaxAmt);
                });
            });
            $scope.invmsL_TotTaxAmt = t;
            $scope.invmsL_TotTaxAmt = parseFloat($scope.invmsL_TotTaxAmt);
            $scope.invmsL_TotTaxAmt = $scope.invmsL_TotTaxAmt.toFixed(2);
            angular.forEach($scope.transrows, function (stax) {
                if (stax.itrS_Id === $scope.tsid) {
                    $scope.arraytax.push({ invmT_Id: stax.invmT_Id, invtslT_TaxPer: stax.invmiT_TaxValue, invtslT_TaxAmt: stax.invtslT_TaxAmt, invtslT_Id: stax.invtslT_Id });
                    stax.get_itemTax = itemtax;
                }
            });
            $scope.taxchange($scope.invmsL_TotTaxAmt);
        };

        ////===================Transcation Grid Count Tax,Discount,Amount   
        $scope.countAmt = function (objstk, items) {
            var a = 0;
            var availablestok = 0;
            var ds = 0;
            var stucnt = 0;
            var totcnut = 0;
            var qqty = 0;
            var ttqqty = 0;
            $scope.qty = 0;
            if ($scope.invmsL_StuOtherFlg === "Student") {
                if ($scope.get_Studentlist.length > 0 && $scope.get_Studentlist !== undefined) {
                    // $scope.transgrid = true;
                    angular.forEach($scope.get_Studentlist, function (cl) {
                        if ($scope.editS === true) {
                            if (cl.clsck === true || cl.secck === true) {
                                stucnt += 1;
                            }
                        } else {
                            if (cl.clsck === true) {
                                stucnt += 1;
                            }
                            if (cl.secck === true) {
                                stucnt += 1;
                            }
                        }

                    });
                    if (stucnt > 0) {
                        qqty = parseFloat(objstk.salqty);
                        ttqqty = qqty * stucnt;
                        objstk.invtsL_SalesQty = ttqqty;
                        $scope.qty = ttqqty;
                    }
                    else {
                        swal("Select Atleast One Checkbox..!!");
                        objstk.invtsL_SalesQty = "";
                    }

                }
                else {
                    $scope.qty = parseFloat(objstk.invtsL_SalesQty);
                }
            }
            else {
                $scope.qty = parseFloat(objstk.invtsL_SalesQty);
            }
            if ($scope.editS === true) {
                $scope.salerate = parseFloat(objstk.invtsL_SalesPrice);
            }
            else {
                $scope.salerate = parseFloat(objstk.invmI_Id.INVSTO_SalesRate);
            }
            $scope.totdiscout = parseFloat(objstk.invtsL_DiscountAmt);
            availablestok = parseFloat($scope.availableitems);
            if ($scope.qty > availablestok) {
                swal("Please Check Available Stock...!!");
                objstk.invtsL_SalesQty = "";
            }

            objstk.invtsL_Amount = $scope.salerate * $scope.qty;
            objstk.invtsL_Amount = parseFloat(objstk.invtsL_Amount);
            objstk.invtsL_Amount = objstk.invtsL_Amount.toFixed(2);
            if ($scope.editS === true) {
                angular.forEach($scope.transrows, function (obj) {
                    if (obj.itrS_Id === objstk.itrS_Id) {
                        obj.invtsL_TaxAmt = 0;
                        obj.invtsL_DiscountAmt = 0;
                    }

                    ds += parseFloat(obj.invtsL_Amount) + parseFloat(obj.invtsL_DiscountAmt);
                    a = ds.toFixed(2);

                });
            }
            else {
                angular.forEach($scope.transrows, function (obj) {
                    if (obj.itrS_Id === objstk.itrS_Id) {
                        obj.invtsL_TaxAmt = 0;
                    }
                    ds += parseFloat(obj.invtsL_Amount) + parseFloat(obj.invtsL_DiscountAmt);
                    a = ds.toFixed(2);
                });
            }
            var totamt = a;
            $scope.invmsL_TotalAmount = totamt;//.toFixed(2);  
            $scope.invmsL_TotalAmount = parseFloat($scope.invmsL_TotalAmount);
            $scope.invmsL_TotalAmount = $scope.invmsL_TotalAmount.toFixed(2);
            if ($scope.editS === true) {
                $scope.countDiscount(objstk, items);
            }
        };

        $scope.countDiscount = function (objtp, items) {
            var d = 0;
            var qqty = 0;
            var ttqqty = 0;
            var stucnt = 0;
            var disct = 0;
            var totdisct = 0;
            $scope.totdiscout = 0;

            if ($scope.get_Studentlist.length > 0) {

                angular.forEach($scope.get_Studentlist, function (cl) {
                    if ($scope.editS === true) {
                        if (cl.clsck === true || cl.secck === true) {
                            stucnt += 1;
                        }
                    } else {
                        if (cl.clsck === true) {
                            stucnt += 1;
                        }
                        if (cl.secck === true) {
                            stucnt += 1;
                        }
                    }
                });
                if (stucnt > 0) {
                    disct = parseFloat(objtp.disctammt);
                    totdisct = disct * stucnt;
                    objtp.invtsL_DiscountAmt = totdisct;
                    $scope.totdiscout = totdisct;
                }
                else {
                    swal("Select Atleast One Checkbox..!!");
                    objtp.invtsL_DiscountAmt = "";
                }
                if (objtp.invtsL_DiscountAmt > parseFloat(objtp.invtsL_Amount)) {
                    objtp.invtsL_DiscountAmt = 0;
                    objtp.invtsL_Amount = 0;
                    $scope.invmsL_TotDiscount = 0;
                    $scope.invmsL_SalesValue = 0;
                    swal("Basic amount must be greater than Discount Amount....!!");
                    return;
                }
            }
            else {
                if (objtp.invtsL_DiscountAmt > parseFloat(objtp.invtsL_Amount)) {
                    objtp.invtsL_DiscountAmt = 0;
                    objtp.invtsL_Amount = 0;
                    $scope.invmsL_TotDiscount = 0;
                    $scope.invmsL_SalesValue = 0;
                    swal("Basic amount must be greater than Discount Amount....!!");
                    return;
                }
                $scope.qty1 = parseFloat(objtp.invtsL_SalesQty);
                $scope.totdiscout = parseFloat(objtp.invtsL_DiscountAmt);

            }

            if ($scope.editS === true) {
                $scope.salerate = parseFloat(objtp.invtsL_SalesPrice);
            }
            else {
                $scope.salerate = parseFloat(objtp.invmI_Id.INVSTO_SalesRate);
            }
            $scope.qty1 = parseFloat(objtp.invtsL_SalesQty);
            $scope.totamt = $scope.salerate * $scope.qty1;

            $scope.finalsalerate = $scope.totamt - $scope.totdiscout;
            objtp.invtsL_Amount = $scope.finalsalerate;
            objtp.invtsL_Amount = parseFloat(objtp.invtsL_Amount);
            objtp.invtsL_Amount = objtp.invtsL_Amount.toFixed(2);
            angular.forEach($scope.transrows, function (obj) {
                if (obj.invtsL_DiscountAmt === "") {
                    obj.invtsL_DiscountAmt = 0;
                }

                d += parseFloat(obj.invtsL_DiscountAmt);
            });
            var discamt = d;
            $scope.invmsL_TotDiscount = discamt;//.toFixed(2);  

        };

        $scope.$watch('invmsL_TotalAmount', function (totalamoutchange) {
            $scope.invmsL_TotalAmount = parseFloat($scope.invmsL_TotalAmount);
            $scope.invmsL_TotalAmount = $scope.invmsL_TotalAmount.toFixed(2);
            $scope.invmsL_SalesValue = $scope.invmsL_TotalAmount;
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
            //var t = 0.00;
            //angular.forEach($scope.transrows, function (obj) {
            //    t += parseFloat(obj.invtsL_Amount);
            //})
            ////  var tottax = t;
            //$scope.invmsL_TotTaxAmt = t;//.toFixed(2);
            //$scope.invmsL_TotTaxAmt = parseFloat($scope.invmsL_TotTaxAmt);
            //$scope.invmsL_TotTaxAmt = $scope.invmsL_TotTaxAmt.toFixed(2);

        });

        $scope.$watch('invmsL_TotDiscount', function (totalDiscountchange) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);
            var tottx = parseFloat($scope.invmsL_TotTaxAmt);
            var purvalur = tamt - tdiscout + tottx;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);    
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        });

        $scope.taxchange = function (sobj) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);

            var ttax = parseFloat($scope.invmsL_TotTaxAmt);
            if ($scope.invmsL_TotTaxAmt === undefined || $scope.invmsL_TotTaxAmt === "") {
                ttax = 0;
            }
            var purvalur = tamt - tdiscout + ttax;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);             
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        };

        $scope.$watch('invmsL_TotTaxAmt', function (totalTaxchange) {
            var tamt = parseFloat($scope.invmsL_TotalAmount);
            var tottx = parseFloat($scope.invmsL_TotTaxAmt);
            var tdiscout = parseFloat($scope.invmsL_TotDiscount);
            var purvalur = tamt + tottx - tdiscout;
            $scope.invmsL_SalesValue = purvalur;//.toFixed(2);    
            $scope.invmsL_SalesValue = parseFloat($scope.invmsL_SalesValue);
            $scope.invmsL_SalesValue = $scope.invmsL_SalesValue.toFixed(2);
        });



        //=======================Save Data
        $scope.objs = {};
        $scope.savedata = function (objs) {

            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arraySale = [];
                //  $scope.arraySaletax = [];
                //angular.forEach($scope.transrows, function (sale) {
                //    $scope.arraySale.push({
                //        invmI_Id: sale.invmI_Id.INVMI_Id, invmuoM_Id: sale.invmuoM_Id, invtsL_BatchNo: sale.invtsL_BatchNo, invtsL_SalesPrice: sale.invmI_Id.INVSTO_SalesRate,
                //        invtsL_SalesQty: sale.invtsL_SalesQty, invtsL_DiscountAmt: sale.invtsL_DiscountAmt, invtsL_Amount: sale.invtsL_Amount, invtsL_Naration: sale.invtsL_Naration,
                //        saleItemTax: $scope.arraySaletax, invtsL_TaxAmt: sale.invtsL_TaxAmt
                //    });
                //})
                if ($scope.arraySaletax !== undefined) {
                    angular.forEach($scope.transrows, function (sale) {
                        $scope.arraySale.push({ invmI_Id: sale.invmI_Id.INVMI_Id, invmuoM_Id: sale.invmuoM_Id, invtsL_BatchNo: sale.invtsL_BatchNo, invtsL_SalesPrice: sale.invmI_Id.INVSTO_SalesRate, invtsL_SalesQty: sale.invtsL_SalesQty, invtsL_DiscountAmt: sale.invtsL_DiscountAmt, invtsL_Amount: sale.invtsL_Amount, invtsL_Naration: sale.invtsL_Naration, saleItemTax: $scope.arraySaletax, invtsL_TaxAmt: sale.invtsL_TaxAmt });
                    });
                }
                else {
                    $scope.arraySaletax = [];
                    angular.forEach($scope.transrows, function (sale) {
                        $scope.arraySale.push({ invmI_Id: sale.invmI_Id.INVMI_Id, invmuoM_Id: sale.invmuoM_Id, invtsL_BatchNo: sale.invtsL_BatchNo, invtsL_SalesPrice: sale.invmI_Id.INVSTO_SalesRate, invtsL_SalesQty: sale.invtsL_SalesQty, invtsL_DiscountAmt: sale.invtsL_DiscountAmt, invtsL_Amount: sale.invtsL_Amount, invtsL_Naration: sale.invtsL_Naration, saleItemTax: $scope.arraySaletax, invtsL_TaxAmt: sale.invtsL_TaxAmt });
                    });
                }

                if ($scope.invmsL_StuOtherFlg === "Student") {
                    if ($scope.obj.stu_Flag === "I") {
                        $scope.stu_id = $scope.obj.amsT_Id.AMST_Id;
                        data = {
                            "INVMSL_StuOtherFlg": $scope.invmsL_StuOtherFlg,
                            // "INVMSL_SalesNo": $scope.invmsL_SalesNo,
                            "INVMST_Id": $scope.invmsT_Id,
                            "INVMSL_SalesValue": $scope.invmsL_SalesValue,
                            "INVMSL_SalesDate": $scope.invmsL_SalesDate,
                            "INVMSL_TotDiscount": $scope.invmsL_TotDiscount,
                            "INVMSL_TotTaxAmt": $scope.invmsL_TotTaxAmt,
                            "INVMSL_TotalAmount": $scope.invmsL_TotalAmount,
                            "INVMSL_Remarks": $scope.invmsL_Remarks,
                            "INVMP_Id": $scope.invmP_Id,
                            "INVMSL_CreditFlg": $scope.invmsL_CreditFlg,
                            "INVMSL_Id": $scope.invmsL_Id,
                            "SaleItem": $scope.arraySale,
                            "AMST_Id": $scope.stu_id,
                            "Student_flag": $scope.obj.stu_Flag,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                        };
                    }
                    else if ($scope.obj.stu_Flag === "C" || $scope.obj.stu_Flag === "CS") {
                        $scope.arrayStudentname = [];
                        angular.forEach($scope.get_Studentlist, function (cl) {
                            if (cl.clsck === true) {
                                $scope.arrayStudentname.push(cl);
                            }
                            if (cl.secck === true) {
                                $scope.arrayStudentname.push(cl);
                            }
                        });

                        data = {
                            "INVMSL_StuOtherFlg": $scope.invmsL_StuOtherFlg,
                            "INVMST_Id": $scope.invmsT_Id,
                            "INVMSL_SalesValue": $scope.invmsL_SalesValue,
                            "INVMSL_SalesDate": $scope.invmsL_SalesDate,
                            "INVMSL_TotDiscount": $scope.invmsL_TotDiscount,
                            "INVMP_Id": $scope.invmP_Id,
                            "INVMSL_TotTaxAmt": $scope.invmsL_TotTaxAmt,
                            "INVMSL_TotalAmount": $scope.invmsL_TotalAmount,
                            "INVMSL_Remarks": $scope.invmsL_Remarks,
                            "INVMSL_CreditFlg": $scope.invmsL_CreditFlg,
                            "SaleItem": $scope.arraySale,
                            "studentlist": $scope.arrayStudentname,
                            "INVMSL_Id": $scope.invmsL_Id,
                            "Student_flag": $scope.obj.stu_Flag,
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                        };
                    }
                }
                else if ($scope.invmsL_StuOtherFlg === "Staff") {
                    $scope.staff_id = $scope.obj.hrmE_Id.hrmE_Id;
                    data = {
                        "INVMSL_StuOtherFlg": $scope.invmsL_StuOtherFlg,
                        //  "INVMSL_SalesNo": $scope.invmsL_SalesNo,
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMSL_SalesValue": $scope.invmsL_SalesValue,
                        "INVMSL_SalesDate": $scope.invmsL_SalesDate,
                        "INVMSL_TotDiscount": $scope.invmsL_TotDiscount,
                        "INVMSL_TotTaxAmt": $scope.invmsL_TotTaxAmt,
                        "INVMSL_TotalAmount": $scope.invmsL_TotalAmount,
                        "INVMSL_Remarks": $scope.invmsL_Remarks,
                        "INVMP_Id": $scope.invmP_Id,
                        "INVMSL_CreditFlg": $scope.invmsL_CreditFlg,
                        "SaleItem": $scope.arraySale,
                        "HRME_Id": $scope.staff_id,
                        "INVMSL_Id": $scope.invmsL_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }
                else if ($scope.invmsL_StuOtherFlg === "Customer") {
                    $scope.customer_id = $scope.obj.invmC_Id.invmC_Id;
                    data = {
                        "INVMSL_StuOtherFlg": $scope.invmsL_StuOtherFlg,
                        //  "INVMSL_SalesNo": $scope.invmsL_SalesNo,
                        "INVMST_Id": $scope.invmsT_Id,
                        "INVMSL_SalesValue": $scope.invmsL_SalesValue,
                        "INVMSL_SalesDate": $scope.invmsL_SalesDate,
                        "INVMSL_TotDiscount": $scope.invmsL_TotDiscount,
                        "INVMSL_TotTaxAmt": $scope.invmsL_TotTaxAmt,
                        "INVMSL_TotalAmount": $scope.invmsL_TotalAmount,
                        "INVMSL_Remarks": $scope.invmsL_Remarks,
                        "INVMP_Id": $scope.invmP_Id,
                        "INVMSL_CreditFlg": $scope.invmsL_CreditFlg,
                        "SaleItem": $scope.arraySale,
                        "INVMC_Id": $scope.customer_id,
                        "INVMSL_Id": $scope.invmsL_Id,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("INV_T_Sales/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmsL_Id === 0 || promise.invmsL_Id < 0) {
                            if (promise.returnduplicatestatus === 'Updated') {
                                swal('Record saved successfully / Stock Updated');
                            }
                            if (promise.returnduplicatestatus === 'notUpdated') {
                                swal('Record saved successfully / Failed to Update Stock');
                            }
                        }
                        else if (promise.invmsL_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsL_Id === 0 || promise.invmsL_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsL_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.edit = function (editsl) {
            $scope.edit_SaleItemDetails = [];
            $scope.get_Studentlist = [];
            $scope.edit_Saletypes = [];
            $scope.invmsL_StuOtherFlg = "";
            $scope.transrows = [];
            $scope.get_item = [];
            $scope.get_item.length = 1;
            $scope.invmsT_Id = editsl.invmsT_Id;
            $scope.invmS_StoreName = editsl.invmS_StoreName;
            $scope.invmsL_TotalAmount = editsl.invmsL_TotalAmount;
            $scope.invmsL_SalesDate = new Date(editsl.invmsL_SalesDate);
            $scope.invmsL_TotDiscount = editsl.invmsL_TotDiscount;
            $scope.invmsL_TotTaxAmt = editsl.invmsL_TotTaxAmt;
            $scope.invmsL_SalesValue = editsl.invmsL_SalesValue;
            $scope.invmsL_Remarks = editsl.invmsL_Remarks;
            $scope.editS = true;
            $scope.transgrid = true;
            var data = {
                "INVMSL_Id": editsl.invmsL_Id,
                "saletype": editsl.invmsL_StuOtherFlg
            };
            apiService.create("INV_T_Sales/getSaleItemDetails", data).
                then(function (promise) {

                    $scope.edit_SaleItemDetails = promise.get_SaleItemDetails;
                    $scope.edit_Saletypes = promise.get_Saletypes;

                    if (editsl.invmsL_StuOtherFlg === "Student") {

                        $scope.invmsL_StuOtherFlg = "Student";
                        if ($scope.edit_Saletypes.length > 1) {
                            $scope.obj.stu_Flag = "C";
                            angular.forEach($scope.edit_Saletypes, function (obju) {
                                $scope.get_Studentlist.push({ amsT_Id: obju.AMST_Id, studentname: obju.studentname, AMST_AdmNo: obju.AMST_AdmNo, asmcL_ClassName: obju.asmcL_ClassName, clsck: true, secck: true });
                            });
                        }
                        else {
                            $scope.obj.stu_Flag = "I";
                            $scope.amsT_Id = $scope.edit_Saletypes[0].AMST_Id;
                            $scope.studentname = $scope.edit_Saletypes[0].studentname;
                            $scope.AMST_AdmNo = $scope.edit_Saletypes[0].AMST_AdmNo;
                        }
                    }
                    else if (editsl.invmsL_StuOtherFlg === "Staff") {
                        $scope.employeename = $scope.edit_Saletypes[0].employeename;
                        $scope.hrmE_Id = $scope.edit_Saletypes[0].HRME_Id;
                        $scope.invmsL_StuOtherFlg = "Staff";
                    }
                    else if (editsl.invmsL_StuOtherFlg === "Customer") {
                        $scope.invmC_CustomerName = $scope.edit_Saletypes[0].INVMC_CustomerName;
                        $scope.invmslC_Id = $scope.edit_Saletypes[0].INVMSLC_Id;
                        $scope.invmsL_StuOtherFlg = "Customer";
                    }

                    angular.forEach($scope.edit_SaleItemDetails, function (objedit) {
                        $scope.cnt = $scope.cnt + 1;
                        var newItemNo = $scope.cnt;
                        $scope.transrows.push({
                            invtsL_Id: objedit.invtsL_Id,
                            invmsL_Id: objedit.invmsL_Id,
                            invmI_Id: objedit.invmI_Id,
                            invmuoM_Id: objedit.invmuoM_Id,
                            invmsL_SalesNo: objedit.invmsL_SalesNo,
                            invmI_ItemName: objedit.invmI_ItemName,
                            invmuoM_UOMName: objedit.invmuoM_UOMName,
                            invtsL_BatchNo: objedit.invtsL_BatchNo,
                            invtsL_SalesQty: objedit.invtsL_SalesQty,
                            invtsL_SalesPrice: objedit.invtsL_SalesPrice,
                            invtsL_DiscountAmt: objedit.invtsL_DiscountAmt,
                            invtsL_TaxAmt: objedit.invtsL_TaxAmt,
                            invtsL_Amount: objedit.invtsL_Amount,
                            invtsL_Naration: objedit.invtsL_Naration,
                            invtsL_ActiveFlg: objedit.invtsL_ActiveFlg,
                            'itrS_Id': 'trans' + newItemNo
                        });

                    });



                });
        };

        //==============================GRID Model Form
        $scope.typemodelclick = function (typ) {
            $scope.stype = typ.invmsL_StuOtherFlg;
            var data = {
                "INVMSL_Id": typ.invmsL_Id,
                "saletype": typ.invmsL_StuOtherFlg
            };
            apiService.create("INV_T_Sales/getSaletypes", data).
                then(function (promise) {
                    $scope.get_Saletypes = promise.get_Saletypes;
                });
        };

        $scope.mainmodelclick = function (id) {
            var data = {
                "INVMSL_Id": id.invmsL_Id,
                "saletype": id.invmsL_StuOtherFlg
            };
            apiService.create("INV_T_Sales/getSaleItemDetails", data).
                then(function (promise) {
                    $scope.get_SaleItemDetails = promise.get_SaleItemDetails;
                    $scope.salenumber = $scope.get_SaleItemDetails[0].invmsL_SalesNo;
                });
        };

        $scope.transmodelclick = function (id) {
            var data = {
                "INVTSL_Id": id
            };
            apiService.create("INV_T_Sales/getSaleItemTax", data).
                then(function (promise) {
                    $scope.get_SaleItemTax = promise.get_SaleItemTax;
                });
        };

        //==============================GRID Activate And Deactivate
        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMSL_Id = item.invmsL_Id;
            $scope.INVMST_Id = item.invmsT_Id;
            var dystring = "";
            if (item.invmsL_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmsL_ActiveFlg === false) {
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
                        apiService.create("INV_T_Sales/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    if (promise.returnduplicatestatus === 'Updated') {
                                        swal("Record " + dystring + "d Successfully / Stock Updated");
                                    }
                                    else if (promise.returnduplicatestatus === 'notUpdated') {
                                        swal("Record " + dystring + "d Successfully / Failed to Update Stock");
                                    }
                                    else {
                                        swal("Record " + dystring + "d Successfully!!!");
                                    }
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        //$scope.isOptionsRequired = function () {
        //    if ($scope.obj.stu_Flag === 'C' && $scope.invmsL_StuOtherFlg === 'Student') {
        //        return !$scope.get_Studentlist.some(function (options) {
        //            return options.clsck;
        //        });
        //    } else {
        //        return false;
        //    }

        //};

        $scope.deactiveS = function (item, SweetAlert) {
            $scope.INVTSL_Id = item.invtsL_Id;
            var dystring = "";
            if (item.invtsL_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtsL_ActiveFlg === false) {
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
                        apiService.create("INV_T_Sales/deactiveS", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModalS').modal('hide');
                                //  $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.deactivetax = function (item, SweetAlert) {
            $scope.INVTSLT_Id = item.invtslT_Id;
            var dystring = "";
            if (item.invtslT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtslT_ActiveFlg === false) {
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
                        apiService.create("INV_T_Sales/deactivetax", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                angular.element('#myModalTax').modal('hide');
                                //  $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };


        $scope.searchValueC = "";
        $scope.searchValueCS = "";
    }
})();