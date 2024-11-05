(function () {
    'use strict';
    angular
        .module('app')
        .controller('PDAReportController', PDAReportController)
    PDAReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDAReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {




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
            apiService.getURI("PDAReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.acdyr = promise.fillyear;
                    $scope.classcount = promise.classlist;
                    $scope.sectioncount = promise.searcharray;
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
            apiService.create("PDAReport/getsection", data).
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
            apiService.create("PDAReport/getstudent", data).
                then(function (promise) {
                    $scope.studlist = promise.fillstudent;
                })
        }



        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;
        $scope.ShowReport = function () {

            // var sid = $scope.AMST_Id.amst_Id

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

            if ($scope.format1 == undefined || $scope.format1 == 0) {
               
            }
            else {
                $scope.stored = 1;
                $scope.detailed = 0;
            }


            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "AMSC_Id": $scope.asmC_Id,
                    "AMST_Id": sid,
                    "From_Date": new Date($scope.fromDate).toDateString(),
                    "To_Date": new Date($scope.todate).toDateString(),
                    "type": $scope.type,
                    "studentdata": $scope.detailed,
                    "refundamt": $scope.stored,
                }

                apiService.create("PDAReport/radiobtndata", data).
                    then(function (promise) {
                        if (promise.headwise != null && promise.headwise != "") {
                            $scope.reportdetails = promise.headwise;
                            $scope.Grid_view = true;
                            $scope.print_flag = false;
                            $scope.paidlist = promise.searcharray;

                            //for (var i = 0; i < promise.transnumconfig.length; i++) {
                            //    $scope.paidlist.push(promise.transnumconfig[i]);
                            //}

                            $scope.admsudentslist = promise.admsudentslist;

                            if ($scope.detailed == 1) {
                                $scope.consol = false;
                                $scope.detail = true;
                            }
                            else if ($scope.format1 == 1) {
                                $scope.consol = false;
                                $scope.detail = false;
                                $scope.format1 = true;

                             



                            }
                            else {
                                $scope.consol = true;
                                $scope.detail = false;
                            }


                            //$scope.detailed = 0;
                            //$scope.stored = 0;
                            $scope.std_list1 = [];
                            var temp_recp_list1 = $scope.reportdetails;
                            for (var m = 0; m < $scope.reportdetails.length; m++) {
                                var stu_id = $scope.reportdetails[m].AMST_Id;
                                var already_cnt = 0;
                                angular.forEach($scope.std_list1, function (itm1) {
                                    if (itm1.stu_id == stu_id) {
                                        already_cnt += 1;
                                    }
                                })
                                if (already_cnt == 0) {
                                    $scope.temp_stu_lst = [];
                                    $scope.donation = [];
                                    $scope.fees = [];
                                    $scope.other = [];
                                    var amt = 0;
                                    var tot = 0;
                                    var conces = 0;
                                    var final = 0;

                                    var feestot = 0;
                                    var dontot = 0;
                                    var othtot = 0;
                                    angular.forEach(temp_recp_list1, function (itm) {
                                        if (itm.AMST_Id == stu_id) {
                                            for (var n = 0; n < $scope.paidlist.length; n++) {
                                                var paid_amt = $scope.paidlist[n].fsS_PaidAmount;
                                                var stu_id1 = $scope.paidlist[n].amst_Id;
                                                angular.forEach($scope.paidlist, function (itm1) {
                                                    if (itm1.amst_Id == stu_id1 && itm1.amst_Id == itm.AMST_Id) {
                                                        amt = paid_amt;
                                                        $scope.temp_stu_lst.push(itm);
                                                        var total = 0;
                                                        angular.forEach($scope.temp_stu_lst, function (e) {
                                                            total += e.PDAEH_Amount;




                                                        })
                                                        if (itm.PDAMH_HeadName == "EDN ESTABLISHMENT and ADMINISTRATION FEE" || itm.PDAMH_HeadName == "BOARDING CHARGES") {
                                                            $scope.fees.push(itm);
                                                        }

                                                        if (itm.PDAMH_HeadName == "STAFF SALARY FUND" || itm.PDAMH_HeadName == "DEVELOPMENT FUND" || itm.PDAMH_HeadName == "BUILDING FUND") {
                                                            $scope.donation.push(itm);
                                                        }


                                                        if (itm.PDAMH_HeadName != "STAFF SALARY FUND" && itm.PDAMH_HeadName != "DEVELOPMENT FUND" && itm.PDAMH_HeadName == "BUILDING FUND" && itm.PDAMH_HeadName != "EDN ESTABLISHMENT and ADMINISTRATION FEE" && itm.PDAMH_HeadName != "BOARDING CHARGES") {
                                                            $scope.other.push(itm);
                                                        }


                                                        var donationtotal = 0;
                                                        angular.forEach($scope.donation, function (e) {
                                                            donationtotal += e.PDAEH_Amount;

                                                        })
                                                        var feestotal = 0;
                                                        angular.forEach($scope.fees, function (e) {
                                                            feestotal += e.PDAEH_Amount;

                                                        })

                                                        var othertotal = 0;
                                                        angular.forEach($scope.other, function (e) {
                                                            othertotal += e.PDAEH_Amount;

                                                        })

                                                        dontot = donationtotal;
                                                        feestot = feestotal;
                                                        othtot = othertotal;

                                                        tot = total;
                                                        final = Math.abs(amt - tot);
                                                        conces = itm1.fsS_CurrentYrCharges;
                                                    }
                                                })
                                            }
                                        }
                                    })

                                    $scope.std_list1.push({
                                        stu_id: stu_id, conces: conces, paid: amt, tot: tot, final: final, stu_rpt: $scope.temp_stu_lst, donation: $scope.donation, fees: $scope.fees, other: $scope.other, donationtotal: dontot,
                                        feestotal: feestot, othertotal: othtot
                                    });
                                }
                            }






                            $scope.reportlist = $scope.std_list1;

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

        $scope.clear_feedef = function () {
            $state.reload();
        }


        $scope.printToCart = function () {

            var pdss = "";


            if ($scope.detailed == 1) {
                pdss = 'printrcp'
            }
            else if ($scope.format1 == 1) {
                pdss = 'printformat1'
            }
            else {
                pdss = 'printcons'

            }



            var innerContents = document.getElementById(pdss).innerHTML;

            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BArrearReport/pdareportpdf.css"/>' +
                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


    }
})();
