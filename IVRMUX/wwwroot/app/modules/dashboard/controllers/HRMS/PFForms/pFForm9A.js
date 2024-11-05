(function () {
    'use strict';
    angular
.module('app')
        .controller('pFForm9AController', pFForm9AController)

    pFForm9AController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function pFForm9AController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

       
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFForm9A/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}



            })
        }


        $scope.pfreport= [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.payrollStandard = {};
                $scope.pfreport = [];
               var data = $scope.Employee;

                apiService.create("PFForm9A/getEmployeedetailsBySelection", data).
                            then(function (promise) {
                                $scope.EmployeeDis = true;
                                if (promise.professionaltaxamount !== null && promise.professionaltaxamount > 0) {

                                    $scope.tax = promise.professionaltaxamount;
                                    $scope.hretdS_TaxDeposited_totalwords = toWords($scope.tax)

                                }

                                if (promise.bankdetails !== null && promise.bankdetails.length > 0) {

                                    $scope.bankdetails = promise.bankdetails[0];
                                }


                                if (promise.hreS_Month != "") {
                                    $scope.month = promise.hreS_Month;
                                }
                                if (promise.hreS_Year != "") {
                                    $scope.year = promise.hreS_Year;
                                }
                                if (promise.payrollStandard !== null && promise.payrollStandard.length > 0) {

                                    $scope.payrollStandard = promise.payrollStandard[0];
                                }
                                //Institution Details
                                if (promise.institutionDetails !== null && promise.institutionDetails.length > 0) {

                                    $scope.institution = promise.institutionDetails[0];

                                    var instuteAddress = "";
                                    if ($scope.institution.mI_Address1 != null && $scope.institution.mI_Address1 != "") {

                                        instuteAddress = $scope.institution.mI_Address1;

                                    }
                                    if ($scope.institution.mI_Address2 != null && $scope.institution.mI_Address2 != "") {

                                        instuteAddress = instuteAddress + ', ' + $scope.institution.mI_Address2;

                                    }

                                    if ($scope.institution.mI_Address3 != null && $scope.institution.mI_Address3 != "") {

                                        instuteAddress = instuteAddress + ', ' + $scope.institution.mI_Address3;

                                    }

                                    $scope.CurrentInstuteAddress = instuteAddress;
                                }



                            })
            }

        }

        var th = ['', 'thousand', 'million', 'billion', 'trillion'];
        var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];


        function toWords(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s)) return 'not a number';
            var x = s.indexOf('.');
            if (x == -1) x = s.length;
            if (x > 15) return 'too big';
            var n = s.split('');
            var str = '';
            var sk = 0;
            for (var i = 0; i < x; i++) {
                if ((x - i) % 3 == 2) {
                    if (n[i] == '1') {
                        str += tn[Number(n[i + 1])] + ' ';
                        i++;
                        sk = 1;
                    }
                    else if (n[i] != 0) {
                        str += tw[n[i] - 2] + ' ';
                        sk = 1;
                    }
                }
                else if (n[i] != 0) {
                    str += dg[n[i]] + ' ';
                    if ((x - i) % 3 == 0) str += 'hundred ';
                    sk = 1;
                }


                if ((x - i) % 3 == 1) {
                    if (sk) str += th[(x - i - 1) / 3] + ' ';
                    sk = 0;
                }
            }
            if (x != s.length) {
                var y = s.length;
                str += 'point ';
                for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            }
            return str.replace(/\s+/g, ' ');
        }

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.pfreport= [];
            $scope.submitted = false;
            $scope.payrollStandard = {};
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.CurrentInstuteAddress = "";
            $scope.institution = {};
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


    }


})();