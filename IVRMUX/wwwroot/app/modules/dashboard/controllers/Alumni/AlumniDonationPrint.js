(function () {
    'use strict';
    angular.module('app')
        .controller('alumnidonationprintreportController', alumnidonationprintreportController)
    alumnidonationprintreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function alumnidonationprintreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;
        $scope.todatedate = new Date();

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;
        $scope.search = "";

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];

        $scope.submitted = false;
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {

                $scope.fromdate = new Date($scope.fromdate).toDateString();
                $scope.todate = new Date($scope.todate).toDateString();
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate
                 
                }
                apiService.create("AlumniDonation/getdonationprint/", data).
                    then(function (promise) {
                        if (promise.donationlist > 0 || promise.donationlist !== null) {
                            $scope.donationlist = promise.donationlist;
                            $scope.flag = promise.flag;
                        }
                        else {
                            swal('No Data Found!!!');
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.optionToggled = function (user) {
            $scope.listdata = [];
            $scope.panmi ="";
            $scope.ReceiptNo = "";
            $scope.ALDON_DonorName = "";
            $scope.ALDON_Amount = "";
            $scope.paymentid = "";
            $scope.Donationname = "";
            $scope.ALMST_StudentPANCard = "";
            angular.forEach($scope.donationlist, function (qq) {
                if (qq.selected === true) {
                    $scope.listdata.push(user);

                    $scope.panmi = user.MI_PAN;
                    $scope.ReceiptNo = user.ALDON_ReceiptNo;
                    $scope.ALDON_DonorName = user.ALDON_DonorName;
                    $scope.ALDON_Amount = user.ALDON_Amount;
                    $scope.paymentid = user.ALDON_ReferenceNo;
                    $scope.Donationname = user.ALMDON_DonationName;
                    $scope.ALMST_StudentPANCard = user.ALDON_DonarPANNo;
                }
            });
            angular.forEach($scope.donationlist, function (ww) {
                ww.selected = false;
            });
            angular.forEach($scope.donationlist, function (ee) {
                angular.forEach($scope.listdata, function (rr) {
                    if (ee.ALDON_Id === rr.ALDON_Id) {
                        ee.selected = true;
                    }
                });
            });


            $scope.wordamount = toWordsconver($scope.ALDON_Amount);
        };

        $scope.PrintReceipt = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        var th_val = ['', 'Thousand', 'Million', 'Billion', 'Trillion'];
        var dg_val = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn_val = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw_val = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
        function toWordsconver(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s))
                return 'not a number ';
            var x_val = s.indexOf('.');
            if (x_val == -1)
                x_val = s.length;
            if (x_val > 15)
                return 'too big';
            var n_val = s.split('');
            var str_val = '';
            var sk_val = 0;
            for (var i = 0; i < x_val; i++) {
                if ((x_val - i) % 3 == 2) {
                    if (n_val[i] == '1') {
                        str_val += tn_val[Number(n_val[i + 1])] + ' ';
                        i++;
                        sk_val = 1;
                    } else if (n_val[i] != 0) {
                        str_val += tw_val[n_val[i] - 2] + ' ';
                        sk_val = 1;
                    }
                } else if (n_val[i] != 0) {
                    str_val += dg_val[n_val[i]] + ' ';
                    if ((x_val - i) % 3 == 0)
                        str_val += 'hundred ';
                    sk_val = 1;
                }
                if ((x_val - i) % 3 == 1) {
                    if (sk_val)
                        str_val += th_val[(x_val - i - 1) / 3] + ' ';
                    sk_val = 0;
                }
            }
            if (x_val != s.length) {
                var y_val = s.length;
                str_val += 'point ';
                for (var i = x_val + 1; i < y_val; i++)
                    str_val += dg_val[n_val[i]] + ' ';
            }
            return str_val.replace(/\s+/g, ' ');
        }
    }
})();