(function () {
    'use strict';
    angular
        .module('app')
        .controller('PDASummaryReportController', PDASummaryReportController)
    PDASummaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDASummaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {


        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }





        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("PDASummaryReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.acdyr = promise.fillyear;
                    $scope.classcount = promise.classlist;
                    $scope.sectioncount = promise.searcharray;
                    $scope.pdaheadlist = promise.pdaheadlist;
                    $scope.invheadlist = promise.invheadlist;
                    $scope.earnlen = $scope.pdaheadlist.length;
                    $scope.dedlen = $scope.invheadlist.length;
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }




        $scope.onselectclass = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            apiService.create("PDASummaryReport/getsection", data).
                then(function (promise) {
                    $scope.sectioncount = promise.fillsection;
                })
        }

        $scope.onselectsection = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmC_Id,
            }
            apiService.create("PDASummaryReport/getstudent", data).
                then(function (promise) {
                    $scope.studlist = promise.fillstudent;
                })
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;


        $scope.clear_feedef = function () {
            $state.reload();
        }



        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.ShowReport = function () {

            if ($scope.type == 'Ind') {
                var sid = $scope.AMST_Id.amst_Id
            }
            else {
                var sid = 0;
            }

            angular.forEach($scope.acdyr, function (ty) {
                if (ty.asmaY_Id == $scope.asmaY_Id) {
                    $scope.year = ty.asmaY_Year;
                }
            })


            if ($scope.detailed == undefined || $scope.detailed == 0) {
                $scope.detailed = 0;
            }
            else {
                $scope.detailed = 1;
            }

            if ($scope.stored == undefined || $scope.stored == 0) {
                $scope.stored = 0;
            }
            else {
                $scope.stored = 1;
            }

            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmC_Id,
                    "AMST_Id": sid,
                    // "From_Date": new Date($scope.fromDate).toDateString(),
                    //  "To_Date": new Date($scope.todate).toDateString(),
                    "type": $scope.type,
                    "studentdata": $scope.detailed,
                    "refundamt": $scope.stored,
                }

                apiService.create("PDASummaryReport/radiobtndata", data).
                    then(function (promise) {
                        if ((promise.invdetails != null && promise.invdetails != "") || (promise.pdadetails != null && promise.pdadetails != "") || (promise.obdetails != null && promise.obdetails != "")) {

                            $scope.reportinv = promise.invdetails;
                            $scope.reportpda = promise.pdadetails;

                            $scope.reportinvv = promise.invdetails;

                            $scope.reportob = promise.obdetails;
                            $scope.reportcon = promise.transnumconfig;
                            $scope.Grid_view = true;
                            $scope.print_flag = false;


                            console.log($scope.reportinv);
                            console.log($scope.reportpda);

                            $scope.totallist = [];

                            $scope.StudentDue = 0;
                            $scope.InstDue = 0;

                            // PDA Head Total Amount : 

                            $scope.dispensary_expenses = 0;
                            $scope.educational_excursion = 0;
                            $scope.fines_and_compensations = 0;
                            $scope.lab_expenses = 0;
                            $scope.misc_photo_id_card_exams = 0;
                            $scope.boarding_charges = 0;
                            $scope.edn_establishment = 0;
                            $scope.retreat_expenses = 0;
                            $scope.pocket_money_expenses = 0;
                            $scope.development_fund = 0;
                            $scope.building_fund = 0;
                            $scope.cloth_stores = 0;
                            $scope.stationary_stores = 0;
                            $scope.conamount = 0;
                            $scope.totexpenditure = 0;
                            $scope.depototal = 0;
                            $scope.pdarefund = 0;

                            $scope.totbalance = 0;

                            angular.forEach($scope.reportob, function (PdaDue) {

                                $scope.StudentDue = $scope.StudentDue + PdaDue.PDAS_OBStudentDue;

                                $scope.InstDue = $scope.InstDue + PdaDue.PDAS_OBExcessPaid;

                                });


                            //angular.forEach($scope.reportpda, function (x , y) {

                            //    if (x.  == 'DISPENSARY EXPENSES') {

                            //        $scope.dispensary_expenses = $scope.dispensary_expenses + Pdahead;
                            //    }

                            //});

                            angular.forEach($scope.reportinv, function (stuw1) {

                                angular.forEach($scope.reportpda, function (pdast) {

                                    if (pdast.AMST_Id == stuw1.AMST_Id) {

                                        angular.forEach(pdast, function (x, y) {
                                            var c = x;
                                            var d = y;
                                            if (d == "DISPENSARY EXPENSES") {
                                                $scope.dispensary_expenses = $scope.dispensary_expenses + c;
                                            }
                                            if (d == "EDUCATIONAL EXCURSION") {
                                                $scope.educational_excursion = $scope.educational_excursion + c;
                                            }
                                            if (d == "FINES AND COMPENSATIONS") {
                                                $scope.fines_and_compensations = $scope.fines_and_compensations + c;
                                            }
                                            if (d == "LAB EXPENSES") {
                                                $scope.lab_expenses = $scope.lab_expenses + c;
                                            }
                                            if (d == "MISC PHOTO ID CARD EXAMS OTHER EXPNS") {
                                                $scope.misc_photo_id_card_exams = $scope.misc_photo_id_card_exams + c;
                                            }
                                            if (d == "BOARDING CHARGES") {
                                                $scope.boarding_charges = $scope.boarding_charges + c;
                                            }
                                            if (d == "EDN ESTABLISHMENT and ADMINISTRATION FEE") {
                                                $scope.edn_establishment = $scope.edn_establishment + c;
                                            }
                                            if (d == "RETREAT EXPENSES") {
                                                $scope.retreat_expenses = $scope.retreat_expenses + c;
                                            }
                                            if (d == "POCKET MONEY EXPENSES") {
                                                $scope.pocket_money_expenses = $scope.pocket_money_expenses + c;
                                            }
                                            if (d == "DEVELOPMENT FUND") {
                                                $scope.development_fund = $scope.development_fund + c;
                                            }
                                            if (d == "BUILDING FUND") {
                                                $scope.building_fund = $scope.building_fund + c;
                                            }
                                    });
                                    }

                                })
                            })


                            angular.forEach($scope.reportinv, function (stuw1) {

                                angular.forEach(stuw1, function (a, b) {

                                    var x = a;
                                    var y = b;

                                    if (y == "STATIONARY STORES") {
                                        $scope.stationary_stores = $scope.stationary_stores + x;
                                    }
                                    if (y == "CLOTH STORES") {
                                        $scope.cloth_stores = $scope.cloth_stores + x;
                                    }
                                })

                            });


                            angular.forEach($scope.reportinv, function (inv) {

                                angular.forEach($scope.reportcon, function (con) {

                                    if (inv.AMST_Id == con.AMST_Id) {
                                        $scope.conamount = $scope.conamount + con.FSS_ConcessionAmount;
                                    }

                                })

                            });



                            angular.forEach($scope.reportinv, function (inv) {

                                angular.forEach($scope.reportob, function (depo) {

                                    if (inv.AMST_Id == depo.AMST_Id) {
                                        $scope.depototal = $scope.depototal + depo.PDAS_CYDeposit;
                                    }

                                })

                            });

                            angular.forEach($scope.reportinv, function (inv) {

                                angular.forEach($scope.reportob, function (refund) {

                                    if (inv.AMST_Id == refund.AMST_Id) {
                                        $scope.pdarefund = $scope.pdarefund + refund.PDAS_CYRefundAmt;
                                    }

                                })

                            });



                            //angular.forEach($scope.reportinv, function (k, v) {
                            //    angular.forEach(k, function (x, y) {
                            //        var a = x;
                            //        var b = y;
                            //    });
                            //});


                            angular.forEach($scope.reportinv, function (stuw1) {
                                var p = 0;
                                //var n_stu_id = stuw.AMST_Id;
                                //var n_stu_cnt = 0;
                                //angular.forEach($scope.reportinv, function (stuw1) {
                                //        if (stuw1.AMST_Id == n_stu_id) {
                                //            n_stu_cnt += 1;
                                //        }
                                //if (n_stu_cnt > 0) {
                                for (var q = 0; q < $scope.invheadlist.length; q++) {
                                    angular.forEach(stuw1, function (x, y) {
                                        var a = x;
                                        var b = y;
                                        if (b == $scope.invheadlist[q].invmG_GroupName) {
                                            p = p + a;

                                            console.log(p);
                                        }
                                    });
                                }

                                angular.forEach($scope.reportpda, function (pdast) {
                                    if (pdast.AMST_Id == stuw1.AMST_Id) {

                                        for (var i = 0; i < $scope.pdaheadlist.length; i++) {
                                            angular.forEach(pdast, function (x, y) {
                                                var c = x;
                                                var d = y;
                                                if (d == $scope.pdaheadlist[i].pdaR_RefundRemarks) {
                                                    p = p + c;

                                                    console.log(p);
                                                }
                                            });
                                        }

                                    }
                                })
                                $scope.totallist.push({ Amst_id: stuw1.AMST_Id, totexp: p });
                                console.log($scope.totallist);
                                //}
                                //})
                            })


                            angular.forEach($scope.reportinv, function (invv) {

                                angular.forEach($scope.totallist, function (texp) {

                                    if (invv.AMST_Id == texp.Amst_id) {
                                        $scope.totexpenditure = $scope.totexpenditure + texp.totexp;
                                    }

                                })

                            });

                            $scope.totbalance = ($scope.depototal + $scope.conamount - $scope.totexpenditure );

                        }
                        else {
                            swal("No Record Found");
                            $scope.Grid_view = false;
                            $scope.print_flag = true;
                        }

                    })
            }
            else {
                $scope.submitted = true;

            }
        };

    }
})();