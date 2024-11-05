(function () {
    'use strict';
    angular.module('app').controller('ClientProformaInvoiceController', ClientProformaInvoiceController)
    ClientProformaInvoiceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ClientProformaInvoiceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.searchchkbx = "";
        $scope.editflag = false;
        $scope.submitted = false;
        $scope.maxdate = new Date();
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "CLIPINV") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }
        $scope.imagename = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
            $scope.imagename = logopath;
        }
    

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.totalgrid = [];
        $scope.submitted = false;
        $scope.Projectchange = function () {
            var data = {
                "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                "ISMMPR_Id": $scope.ISMMPR_Id,
            };
            apiService.create("ClientProformaInvoice/getbom", data).then(function (promise) {
                $scope.bomlist = promise.projectlist;
            });
        };
        $scope.countAmtTWO = function (grnobjj, items) {
            if (grnobjj.ismincD_SACCode > 0) {
                grnobjj.ismprincD_SACCode = "";
            }
        };
        $scope.countAmtThree = function (grnobjjj, items) {
            if (grnobjjj.ismprincD_HSNCode > 0) {
                grnobjjj.ismprincD_SACCode = "";
            }
        };
        $scope.viewProformaInvoice = function () {
            if ($scope.myForm.$valid) {
                $scope.bankarray = [];
                $scope.imagename = $scope.instlist[0].mI_Logo;
                $scope.miname = $scope.instlist[0].mI_Name;
                $scope.miname = angular.uppercase($scope.miname);
                $scope.ISMMCLT_Address = $scope.clientdetails[0].ismmclT_Address;
                $scope.ISMMCLT_Desc = $scope.clientdetails[0].ismmclT_Desc;
                $scope.client = $scope.clientdetails[0].ismmclT_ClientName;
                $scope.cgst = $scope.instlist[0].mI_GSTNO;
                $scope.ccin = $scope.instlist[0].mI_CINNo;
                $scope.ccontact = "";
                $scope.cmail = "";
                if ($scope.instlistemail != null) {
                    if ($scope.instlistemail.length > 0) {
                        $scope.cmail = $scope.instlistemail[0].miE_EmailId;
                    }
                }
                if ($scope.instlistmobile != null) {
                    if ($scope.instlistmobile.length > 0) {
                        $scope.ccontact = $scope.instlistmobile[0].mipN_PhoneNo;
                    }
                }
                $scope.INVMSL_SalesDate = $scope.ISMCLTPRP_PaymentDate;
                $scope.Remarks = $scope.ISMPRINC_Remarks;
                $scope.ttl = $scope.invmgrN_TotalAmount;
                $scope.per1 = $scope.per;
                $scope.ttln1 = $scope.ttln;
                $scope.get_SaleItemDetails = $scope.transrows;
                $scope.masterTax = $scope.appliedtax;
                $scope.mainttl = $scope.finaltotal;
                $scope.Math = window.Math;
                $scope.roundoffttl = $scope.Math.round($scope.mainttl, 0)
                $scope.words = $scope.amountinwords($scope.roundoffttl);
                $scope.mdate = $scope.ISMPRINC_MOUDate;
                $scope.mno = $scope.ISMPRINC_MOURefNo;
                $scope.mof = $scope.ISMPRINC_ModeOfPayment;
                $scope.prinC_InstallmentName = $scope.ISMPRINC_InstallmentName;
                if ($scope.HRMBD_Id != undefined && $scope.HRMBD_Id != null && $scope.HRMBD_Id != '') {

                    angular.forEach($scope.banklist, function (dd) {
                        if (dd.hrmbD_Id == $scope.HRMBD_Id) {
                            $scope.bankarray.push(dd);
                        }
                    })
                }
                if ($scope.ISMPRINC_Id > 0) {
                    $('#popup111').modal('show');
                }
                else {

                    var data = {
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign

                    };
                    apiService.create("ClientProformaInvoice/getinvoiceno", data).then(function (promise) {
                        $scope.INVMSL_SalesNo = promise.trans_id;
                    });
                    $('#popup111').modal('show');
                }

            } else {
                $scope.submitted = true;
            }
        }
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            $scope.Deletegrnrows();
        };
        $scope.Deletegrnrows = function () {
            var a = 0;
            var ab = 0;
            var ds = 0;
            angular.forEach($scope.transrows, function (obj) {
                ds += parseFloat(obj.ismprincD_Amount) + parseFloat(obj.invtgrN_DiscountAmt);
                a = ds.toFixed(2);
            });
            var totamt = a;
            $scope.invmgrN_TotalAmount = totamt;
            $scope.invmgrN_TotalAmount = parseFloat($scope.invmgrN_TotalAmount);
            $scope.invmgrN_TotalAmount = $scope.invmgrN_TotalAmount.toFixed(2);
            $scope.addtocart($scope.invmgrN_TotalAmount);
        }
        $scope.countAmt = function (grnobj, items) {
            if (grnobj.ismprincD_UnitRate != null && grnobj.ismprincD_UnitRate != undefined && grnobj.ismprincD_Qty != null && grnobj.ismprincD_Qty != undefined) {
                var a = 0;
                var ab = 0;
                var ds = 0;
                grnobj.invtgrN_DiscountAmt = 0;
                $scope.purrate = parseFloat(grnobj.ismprincD_UnitRate);
                $scope.qty = parseFloat(grnobj.ismprincD_Qty);
                ab = $scope.purrate * $scope.qty;
                grnobj.ismprincD_Amount = parseFloat(ab);
                grnobj.ismprincD_Amount = grnobj.ismprincD_Amount.toFixed(2);
                angular.forEach($scope.transrows, function (obj) {
                    obj.invtgrN_DiscountAmt = 0;
                    ds += parseFloat(obj.ismprincD_Amount) + parseFloat(obj.invtgrN_DiscountAmt);
                    a = ds.toFixed(2);
                });
                var totamt = a;
                $scope.invmgrN_TotalAmount = totamt;
                $scope.invmgrN_TotalAmount = parseFloat($scope.invmgrN_TotalAmount);
                $scope.invmgrN_TotalAmount = $scope.invmgrN_TotalAmount.toFixed(2);
                $scope.addtocart($scope.invmgrN_TotalAmount);
                $scope.changeadvance();
            }

        };
        $scope.gettax = function () {
            $('#myModalTax').modal('show');
        }
        //-----------Tax Grid Select All
        $scope.toggleAll = function (ss) {
            debugger;
            angular.forEach($scope.get_tax, function (subj) {
                subj.xyz = ss;
            })
        };
        $scope.optionToggled = function () {
            $scope.all = $scope.get_tax.every(function (itm) { return itm.xyz; });
        };
        $scope.appliedtax = [];
        $scope.totaltaxamount = 0;
        $scope.addtocart = function (percentageamount) {
            $scope.appliedtax = [];
            $scope.totaltaxamount = 0;
            if (percentageamount === undefined || percentageamount === null || percentageamount === "") {
                if ($scope.per === undefined || $scope.per === null || $scope.per === "") {
                    $scope.invmgrN_TotalAmount_temp = $scope.invmgrN_TotalAmount;
                }
                else {
                    $scope.invmgrN_TotalAmount_temp = $scope.ttln;
                }
            } else {
                $scope.invmgrN_TotalAmount_temp = percentageamount;
            }
            angular.forEach($scope.get_tax, function (subj) {
                var taxamount = 0;
                if (subj.xyz == true) {
                    if (subj.invmiT_TaxValue == undefined || subj.invmiT_TaxValue == null || subj.invmiT_TaxValue == '' || subj.invmiT_TaxValue == 0) {
                        subj.invmiT_TaxValue = 0;
                        taxamount = 0;
                        $scope.totaltaxamount = parseFloat($scope.totaltaxamount) + parseFloat(taxamount);
                    }
                    else {
                        taxamount = (parseFloat(subj.invmiT_TaxValue) / 100) * parseFloat($scope.invmgrN_TotalAmount_temp);
                        $scope.totaltaxamount = parseFloat($scope.totaltaxamount) + parseFloat(taxamount);
                    }
                    $scope.appliedtax.push({ invmT_Id: subj.invmT_Id, invmT_TaxName: subj.invmT_TaxName, taxamount: taxamount, invmiT_TaxValue: subj.invmiT_TaxValue });
                }
            })
            $scope.ttln = (parseFloat($scope.per) / 100) * parseFloat($scope.invmgrN_TotalAmount);
            $scope.finaltotal = parseFloat($scope.ttln) + parseFloat($scope.totaltaxamount);
        }
        $scope.per = 100;
        $scope.changeadvance = function () {            //extra
            var a = 0;
            $scope.ttln = (parseFloat($scope.per) / 100) * parseFloat($scope.invmgrN_TotalAmount);
            $scope.addtocart($scope.ttln);
            $scope.finaltotal = parseFloat($scope.totaltaxamount) + $scope.ttln;
            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.finaltotal, 0)
            $scope.words = $scope.amountinwords($scope.roundoffttl);

            //appliedtax           
            //extra close                      
        }
        $scope.loaddata = function () {
            $scope.ISMMCLT_Id = "";
            $scope.ISMMPR_Id = "";
            $scope.ISMCLTPRP_Year = "";
            $scope.FTI_Id = "";
            $scope.ISMCLTPRP_InstallmentAmt = "";
            $scope.ISMCLTPRP_PaymentDate = null;
            $scope.ismcltprP_Id = 0;
            $scope.projectlist = [];
            $scope.editflag = false;
            var pageid = 2;
            apiService.getURI("ClientProformaInvoice/loaddata", pageid).then(function (promise) {
                $scope.allcompany = promise.allcompany;
            });
        };
        $scope.cleardata = function () {
            $scope.editflag = false;
            $scope.ISMPRINC_Id = 0;
            $scope.ISMMCLT_Id = "";
            $scope.ISMMPR_Id = "";
            $scope.ISMPRINC_Remarks = "";
            $scope.invmgrN_TotalAmount = 0;
            $scope.finaltotal = 0;
            $scope.totaltaxamount = 0;
            $scope.ISMPRINC_WorkOrder = '';
            $scope.ISMPRINC_ModeOfPayment = '';
            $scope.ISMPRINC_MOURefNo = '';
            $scope.HRMBD_Id = '';
            $scope.ISMCLTPRP_PaymentDate = '';
            $scope.ISMPRINC_MOUDate = null;
            $scope.transrows = [{ itrS_Id: 'trans1' }];
            $scope.appliedtax = [];
            $scope.bankarrayll = [];
            $scope.bankarray = [];
            $scope.submitted = false;
        }
        $scope.modeofPayment1 = function (mode) {
            $scope.ISMPRINC_ModeOfPayment = mode;

        }
        $scope.oncmpchange = function () {
            $scope.cleardata();
            $scope.alldata = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("ClientProformaInvoice/companychange", data).then(function (promise) {
                $scope.clientlist = promise.clientlist;
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata;
                $scope.get_tax = promise.taxlist;
                $scope.instlist = promise.instlist;
                $scope.modeofpaymentlist = promise.modeofpaymentlist;
                $scope.banklist = promise.banklist;
                $scope.instlistmobile = promise.instlistmobile;
                $scope.instlistemail = promise.instlistemail;
            });
        }
        $scope.clientdetails = [];
        $scope.onchangeProject = function (ISMMCLT_Id) {
            var data = {
                "ISMMCLT_Id": ISMMCLT_Id.ismmclT_Id
            };
            apiService.create("ClientProformaInvoice/getProject", data).then(function (promise) {
                $scope.projectlist = promise.projectlist;
                $scope.clientdetails = promise.clientlist;
            });
        };
        $scope.savedata = function () {
            if ($scope.ISMPRINC_MOUDate == '' || $scope.ISMPRINC_MOUDate == undefined) {
                $scope.ISMPRINC_MOUDate = null;
            }
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.ISMPRINC_MOUDate == null ? "" : $filter('date')($scope.ISMPRINC_MOUDate, "yyyy-MM-dd");
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "ISMPRINC_Id": $scope.ISMPRINC_Id,
                    "ismmclT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                    "ISMMPR_Id": $scope.ISMMPR_Id,
                    "ISMPRINC_Remarks": $scope.ISMPRINC_Remarks,
                    "ISMPRINC_TotalAmount": $scope.invmgrN_TotalAmount,
                    "ISMPRINC_TotalTaxAmount": $scope.totaltaxamount,
                    "ISMPRINC_WorkOrder": $scope.ISMPRINC_WorkOrder,
                    "ISMPRINC_AdvPer": $scope.per,
                    "ISMPRINC_AdvanceAmount": $scope.ttln,
                    "ISMCLTPRP_PaymentDate": new Date($scope.ISMCLTPRP_PaymentDate).toDateString(),
                    "ISMPRINC_ModeOfPayment": $scope.ISMPRINC_ModeOfPayment,
                    "ISMPRINC_MOURefNo": $scope.ISMPRINC_MOURefNo,
                    "HRMBD_Id": $scope.HRMBD_Id,
                    "ISMPRINC_MOUDate": fromdate1,
                    "ISMPRINC_TotalBasicAmount": $scope.ttln,
                    "ISMPRINC_TotalPercentage": $scope.per,
                    taxdto: $scope.appliedtax,
                    itemsdto: $scope.transrows,
                    //  transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                    "ISMPRINC_InstallmentName": $scope.ISMPRINC_InstallmentName
                };
                apiService.create("ClientProformaInvoice/savedata", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Data Saved/Updated Successfully");
                    } else {
                        swal("Failed To Save/Update Record");
                    }
                    // $scope.oncmpchange();
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.appliedtax = [];
        $scope.Editdata = function (user) {
            $scope.appliedtax = [];
            var data = {
                "ISMPRINC_Id": user.ISMPRINC_Id,
                "MI_Id": $scope.MI_Id,
            };
            apiService.create("ClientProformaInvoice/Editdata", data).then(function (promise) {
                if (promise.editlist !== null) {
                    $scope.editflag = true;
                    $scope.geteditclient = promise.geteditclient;
                    $scope.projectlist = promise.projectlist;
                    $scope.clientdetails = promise.clientlist;
                    $scope.ISMPRINC_WorkOrder = promise.editlist[0].ismprinC_WorkOrder;
                    $scope.ISMPRINC_Remarks = promise.editlist[0].ismprinC_Remarks;
                    $scope.ISMCLTPRP_PaymentDate = new Date(promise.editlist[0].ismprinC_Date);
                    $scope.clientid = promise.editlist[0].ismmclT_Id;
                    $scope.invmgrN_TotalAmount = promise.editlist[0].ismprinC_TotalAmount;
                    $scope.totaltaxamount = promise.editlist[0].ismprinC_TotalTaxAmount;
                    $scope.per = promise.editlist[0].ismprinC_AdvPer;
                    $scope.INVMSL_SalesNo = promise.editlist[0].ismprinC_PrInviceNo;
                    $scope.ttln = promise.editlist[0].ismprinC_AdvanceAmount;
                    $scope.ISMPRINC_InstallmentName = promise.editlist[0].ismprinC_InstallmentName
                    $scope.finaltotal = parseFloat($scope.ttln) + parseFloat($scope.totaltaxamount);
                    $scope.ISMPRINC_MOUDate = null;
                    if (promise.editlist[0].ismprinC_MOUDate != null && promise.editlist[0].ismprinC_MOUDate != '') {
                        $scope.ISMPRINC_MOUDate = new Date(promise.editlist[0].ismprinC_MOUDate);
                    }
                    $scope.ISMPRINC_MOURefNo = promise.editlist[0].ismprinC_MOURefNo;
                    $scope.ISMPRINC_ModeOfPayment = promise.editlist[0].ismprinC_ModeOfPayment;
                    $scope.HRMBD_Id = '';
                    $scope.bankarrayll = [];
                    if (promise.editlist[0].hrmbD_Id != null && promise.editlist[0].hrmbD_Id != '') {
                        $scope.HRMBD_Id = promise.editlist[0].hrmbD_Id;
                        angular.forEach($scope.banklist, function (dd) {
                            if (dd.hrmbD_Id == $scope.HRMBD_Id) {
                                $scope.bankarrayll.push(dd);
                            }
                        })
                    }
                    if (promise.editlistdetails != null) {
                        if (promise.editlistdetails.length > 0) {
                            $scope.transrows = promise.editlistdetails;
                        }
                    }
                    if (promise.editlisttax != null) {
                        if (promise.editlisttax.length > 0) {
                            angular.forEach(promise.editlisttax, function (pp) {
                                angular.forEach($scope.get_tax, function (ll) {
                                    if (pp.invmT_Id == ll.invmT_Id) {
                                        ll.xyz = true;
                                        ll.invmiT_TaxValue = pp.ismmttX_TaxPercent;

                                        $scope.appliedtax.push({ invmT_Id: ll.invmT_Id, invmT_TaxName: ll.invmT_TaxName, taxamount: pp.ismmttX_TaxAmount, invmiT_TaxValue: pp.ismmttX_TaxPercent });
                                    }

                                })

                            })
                        }
                    }
                    $scope.ISMPRINC_Id = promise.editlist[0].ismprinC_Id;
                    $scope.ISMMPR_Id = promise.editlist[0].ismmpR_Id;
                    //$scope.ISMCLTPRP_Year = promise.editlist[0].ismcltprP_Year;
                    //$scope.ISMCLTPRP_InstallmentName = promise.editlist[0].ismcltprP_InstallmentName;
                    //$scope.ISMCLTPRP_InstallmentAmt = promise.editlist[0].ismcltprP_InstallmentAmt;

                    //$scope.FTI_Id = promise.editlist[0].ftI_Id;
                    angular.forEach($scope.clientlist, function (dd) {
                        if (dd.ismmclT_Id === $scope.clientid) {
                            $scope.ISMMCLT_Id = $scope.geteditclient[0];
                        }
                    });
                }
            });
        };
        $scope.bankarrayll = [];
        $scope.bankchange = function () {
            $scope.bankarrayll = [];
            angular.forEach($scope.banklist, function (dd) {
                if (dd.hrmbD_Id == $scope.HRMBD_Id) {
                    $scope.bankarrayll.push(dd);
                }
            })
        }
        //check
        $scope.reschedule = function (user) {
            var data = {
                "ISMPRINC_Id": user.ISMPRINC_Id,
                "MI_Id": $scope.MI_Id,
            };
            apiService.create("ClientProformaInvoice/Editdata", data).then(function (promise) {
                $scope.clientdetails = promise.clientlist;
                $scope.imagename = $scope.instlist[0].mI_Logo;
                $scope.miname = $scope.instlist[0].mI_Name;
                $scope.miname = angular.uppercase($scope.miname);
                $scope.cgst = $scope.instlist[0].mI_GSTNO;
                $scope.ccin = $scope.instlist[0].mI_CINNo;
                $scope.ccontact = "";
                $scope.cmail = "";
                if ($scope.instlistemail != null) {
                    if ($scope.instlistemail.length > 0) {
                        $scope.cmail = $scope.instlistemail[0].miE_EmailId;
                    }
                }
                if ($scope.instlistmobile != null) {
                    if ($scope.instlistmobile.length > 0) {
                        $scope.ccontact = $scope.instlistmobile[0].mipN_PhoneNo;
                    }
                }
                $scope.ISMMCLT_Address = $scope.clientdetails[0].ismmclT_Address;
                $scope.ISMMCLT_Desc = $scope.clientdetails[0].ismmclT_Desc;
                $scope.client = $scope.clientdetails[0].ismmclT_ClientName;
                $scope.clmobile = $scope.clientdetails[0].ismmclT_ContactNo;
                $scope.clemail = $scope.clientdetails[0].ismmclT_EmailId;
                $scope.INVMSL_SalesDate = promise.editlist[0].ismprinC_Date;
                $scope.Remarks = promise.editlist[0].ismprinC_Remarks;
                $scope.INVMSL_SalesNo = promise.editlist[0].ismprinC_PrInviceNo;
                $scope.prinC_InstallmentName = promise.editlist[0].ismprinC_InstallmentName;
                $scope.ttl = $scope.invmgrN_TotalAmount;
                $scope.get_SaleItemDetails = promise.editlistdetails;
                $scope.clientgst = $scope.clientdetails[0].ismmclT_GSTNO;
                $scope.appliedtax1 = []
                if (promise.editlisttax != null) {
                    if (promise.editlisttax.length > 0) {
                        angular.forEach(promise.editlisttax, function (pp) {
                            angular.forEach($scope.get_tax, function (ll) {
                                if (pp.invmT_Id == ll.invmT_Id) {
                                    ll.xyz = true;
                                    ll.invmiT_TaxValue = pp.ismmttX_TaxPercent;

                                    $scope.appliedtax1.push({ invmT_Id: ll.invmT_Id, invmT_TaxName: ll.invmT_TaxName, taxamount: pp.ismmttX_TaxAmount, invmiT_TaxValue: pp.ismmttX_TaxPercent });
                                }

                            })

                        })
                    }
                }
                $scope.ttl = promise.editlist[0].ismprinC_TotalAmount;
                $scope.ttln1 = promise.editlist[0].ismprinC_AdvanceAmount;
                $scope.per1 = promise.editlist[0].ismprinC_AdvPer;
                var totaltax = promise.editlist[0].ismprinC_TotalTaxAmount;
                $scope.masterTax = $scope.appliedtax1;
                if ($scope.masterTax != null && $scope.masterTax.length > 0) {
                    angular.forEach($scope.get_tax, function (pp) {
                        angular.forEach($scope.masterTax, function (dd) {
                            if (pp.invmT_Id == dd.invmT_Id) {
                                var invmT_TaxName = dd.invmT_TaxName;
                                invmT_TaxName = invmT_TaxName.replace(/[^a-z]/gi, '');
                                dd.invmT_TaxName = invmT_TaxName + '(@' + pp.invmiT_TaxValue + '%)';

                            }

                        })

                    })

                }
                $scope.mainttl = $scope.ttln1 + totaltax;
                $scope.Math = window.Math;
                $scope.roundoffttl = $scope.Math.round($scope.mainttl, 0)
                $scope.words = $scope.amountinwords($scope.roundoffttl);
                $scope.mdate = promise.editlist[0].ismprinC_MOUDate;
                $scope.mno = promise.editlist[0].ismprinC_MOURefNo;
                $scope.mof = promise.editlist[0].ismprinC_ModeOfPayment;
                $scope.bankarray = [];
                if (promise.editlist[0].hrmbD_Id != undefined && promise.editlist[0].hrmbD_Id != null && promise.editlist[0].hrmbD_Id != '') {
                    angular.forEach($scope.banklist, function (dd) {
                        if (dd.hrmbD_Id == promise.editlist[0].hrmbD_Id) {
                            $scope.bankarray.push(dd);
                        }
                    })
                }
                $('#popup111').modal('show');
            });
        }
        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.clientDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ISMPRINC_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.ISMPRINC_ActiveFlag === false) {
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
                        apiService.create("ClientProformaInvoice/clientDecative", usersem).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };
        $scope.viewdetails = function (dd) {
            $scope.submitted1 = false;
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'viewdetails1' }];
            $scope.ISMCLTPRP_Id_New = 0;
            var data = {
                "ISMCLTPRP_Id": dd.ismcltprP_Id
            };
            apiService.create("ClientProformaInvoice/viewdetails", data).then(function (promise) {
                $scope.clientname = dd.ismmclT_ClientName;
                $scope.projectname = dd.ismmpR_ProjectName;
                $scope.yearname = dd.asmaY_Year;
                $scope.installmentname = dd.ismcltprP_InstallmentName;
                $scope.paymentstatusflagnew = dd.paymentstatusflag;
                $scope.installmentamount = dd.ismcltprP_InstallmentAmt;
                $scope.ISMCLTPRP_Id_New = dd.ismcltprP_Id;
                $scope.duedate = new Date(dd.ismcltprP_PaymentDate);
                $scope.paymentmodedetails = promise.paymentmodedetails;
                $scope.getpaymentdetails = promise.getpaymentdetails;
                if ($scope.getpaymentdetails !== null && $scope.getpaymentdetails.length > 0) {
                    $scope.totalgrid = $scope.getpaymentdetails;
                    angular.forEach($scope.totalgrid, function (dd) {
                        dd.ismcppD_ReceivedDate = new Date(dd.ismcppD_ReceivedDate);
                        if (dd.ismcppD_ChequeDate !== null) {
                            dd.ismcppD_ChequeDate = new Date(dd.ismcppD_ChequeDate);
                        }
                        angular.forEach($scope.paymentmodedetails, function (ddd) {
                            if (ddd.ivrmmoD_Id === parseInt(dd.ivrmmoD_Id)) {
                                if (ddd.ivrmmoD_Flag === 'C' || ddd.ivrmmoD_Flag === 'c') {
                                    dd.flag = 0;
                                } else {
                                    dd.flag = 1;
                                }
                            }
                        });
                    });
                }
                $('#myModalreadmitview').modal('show');

            });
        };
        $scope.onchangepaymentmode = function (user) {
            angular.forEach($scope.paymentmodedetails, function (dd) {
                if (dd.ivrmmoD_Id === parseInt(user.ivrmmoD_Id)) {
                    if (dd.ivrmmoD_Flag === 'C' || dd.ivrmmoD_Flag === 'c') {
                        user.flag = 0;
                    } else {
                        user.flag = 1;
                    }
                }
            });
        };
        $scope.savepaymentdetailsrecord = function () {
            $scope.submitted1 = false;
            if ($scope.myForm1.$valid) {
                $scope.savepaymentdetailstemp = [];
                angular.forEach($scope.totalgrid, function (dd) {
                    var date = "";
                    if (dd.flag === 1) {
                        date = new Date(dd.ismcppD_ChequeDate).toDateString();
                    } else {
                        date = new Date().toDateString();
                    }

                    $scope.savepaymentdetailstemp.push({
                        ISMCPPD_Id: dd.ismcppD_Id, ISMCPPD_ReceivedAmount: dd.ismcppD_ReceivedAmount,
                        ISMCPPD_ReceivedDate: new Date(dd.ismcppD_ReceivedDate).toDateString(),
                        IVRMMOD_Id: dd.ivrmmoD_Id, ISMCPPD_PaymentRefNo: dd.ismcppD_PaymentRefNo,
                        ISMCPPD_Remarks: dd.ismcppD_Remarks, ISMCPPD_ChequeDate: date, flag: dd.flag
                    });
                });
                var data = {
                    "ISM_Client_Project_Payment_Details": $scope.savepaymentdetailstemp,
                    "ISMCLTPRP_Id": $scope.ISMCLTPRP_Id_New
                };
                apiService.create("ClientProformaInvoice/savepaymentdetailsrecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $('#myModalreadmitview').modal('hide');
                        $scope.loaddata();
                    }
                });

            } else {
                $scope.submitted1 = true;
            }
        };
        $scope.totalgrid = [{ id: 'viewdetails1' }];
        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.totalgrid.length + 1;
            if (newItemNo <= 10) {
                $scope.totalgrid.push({ 'id': 'viewdetails' + newItemNo, editfalguser: false, editadduser: false });
            }
            console.log($scope.totalgrid);
            if ($scope.editflag === true) {
                $scope.editadd = false;
            }
        };
        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.totalgrid.length - 1;
            $scope.totalgrid.splice(index, 1);
            if ($scope.totalgrid.length === 0) {
                //data
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.clearpayment = function () {
            $scope.clientname = "";
            $scope.projectname = "";
            $scope.yearname = "";
            $scope.installmentname = "";
            $scope.installmentamount = "";
            $scope.ISMCLTPRP_Id_New = 0;
            $scope.duedate = "";
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'viewdetails1' }];
            $('#myModalreadmitview').modal('hide');
            $scope.loaddata();
        };
        $scope.search = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.ismcltprP_PaymentDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (JSON.stringify(obj.ismcltprP_InstallmentAmt)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ismmclT_ClientName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismmpR_ProjectName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.ismcltprP_InstallmentName)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        //======Amount Convert into word=========//
        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        //==============End==============//


        $scope.closemodal = function () {
            $('#popup111').modal('hide');
        };
        $scope.SendData = function () {

            $('#popup11134').modal('show');
        }
        $scope.closemodal = function () {
            $('#popup111').modal('hide');
        };

        $scope.SendDataview = function () {
            $scope.taxdto = [];
            var Template = document.getElementById("asdf").innerHTML;
            var msg = document.getElementById("invoicemail").innerHTML;
            var cfilepath = "";
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (dd) {
                    if (dd.vbabrsF_FilePath != null && dd.vbabrsF_FilePath != "") {
                        $scope.taxdto.push({
                            cfilepath: dd.vbabrsF_FilePath
                        })
                    }

                });
            }
            if ($scope.taxdto != null && $scope.taxdto.length > 0) {
                var data = {
                    ISMMCLT_ClientName: Template,
                    "esubject": $scope.esubject,
                    "msg": msg,
                    //"FHEAD": $scope.FHEAD,                 
                    //"cfilepath": $scope.cfilepath,
                    "ClientMail": $scope.clemail,
                    "taxdto": $scope.taxdto
                };
                apiService.create("ClientInvoice/paymentnotification", data).then(function (promise) {
                    swal("Email  Sent SucessFully ");
                    $scope.esubjec = "";
                    $scope.MSG = "";
                    $scope.FHEAD = "";
                    $scope.Footer = "";
                    $scope.cfilepath = "";
                    $scope.clemail = "";
                    $('#popup11134').modal('hide');
                    $('#popup111').modal('hide');
                });
            }
            else {
                swal("Please Attached  Invoice Pdf !");
            }

        }
        //$scope.SendDataview = function () {
        //    var Template = document.getElementById("asdf").innerHTML;
        //    var msg = document.getElementById("invoicemail").innerHTML;;
        //    var data = {
        //        ISMMCLT_ClientName: Template,
        //        "esubject": $scope.esubject,
        //        "msg": msg,
        //        "FHEAD": $scope.FHEAD,
        //        "Footer": $scope.Footer,
        //        "cfilepath": $scope.cfilepath,
        //        "ClientMail": $scope.clemail,
        //    };
        //    apiService.create("ClientInvoice/paymentnotification", data).then(function (promise) {
        //        swal("Email  Sent SucessFully ");
        //        $scope.esubjec = "";
        //        $scope.MSG = "";
        //        $scope.FHEAD = "";
        //        $scope.Footer = "";
        //        $scope.cfilepath = "";
        //        $scope.clemail = "";
        //        $('#popup11134').modal('hide');
        //        $('#popup111').modal('hide');
        //    });
        //}

        ///New application
        $scope.materaldocuupload = [];
        $scope.materaldocuupload = [{ id: 'materal' }];
        //
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vbabrsF_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 5242880)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/png" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/jpg" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "image/JPG" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                //5242880
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && input.files[0].size <= 5242880) {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 5242880) {
                    swal("File size should be less than 5 MB");
                    return;
                }
            }
        };
        function UploaddianmateralPhoto(data) {
            console.log("check Book upload  Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= 1; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.vbabrsF_FilePath = d;
                    data.vbabrsF_FileName = $scope.vbabrsF_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        };
        //
        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                    //$('#myModalfile').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };
    }
})();
