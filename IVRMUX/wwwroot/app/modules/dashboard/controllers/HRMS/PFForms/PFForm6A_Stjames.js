(function () {
    'use strict';
    angular
.module('app')
        .controller('PFForm6StjController', PFForm6StjController)

    PFForm6StjController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFForm6StjController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.counter = 0;
        $scope.totalwagesall = 0;
        $scope.totalpensionfundall = 0;

        $scope.Employee = {};
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFForm6A/getalldetails", pageid).then(function (promise) {

                //if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                //    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                //}

                //if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                //    $scope.monthdropdown = promise.monthdropdown;
                //}

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}


                //Set From date and To date
                $scope.Employee.FromDate = new Date();

                $scope.Employee.ToDate = $scope.Employee.FromDate;
                $scope.minDateTo = new Date(
                    $scope.Employee.ToDate.getFullYear(),
                    $scope.Employee.ToDate.getMonth(),
                    $scope.Employee.ToDate.getDate());
            });
        };

        //setToDate
        $scope.setToDate = function (FromDate) {

            $scope.Employee.ToDate = FromDate;
            $scope.minDateTo = new Date(
                $scope.Employee.ToDate.getFullYear(),
                $scope.Employee.ToDate.getMonth(),
                $scope.Employee.ToDate.getDate());


            $scope.Employee.ToDate = "";
        };


        $scope.pfreport = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.pfreport = [];
                $scope.finYearFromDate = "";
                $scope.finYearToDate = "";
                $scope.payrollStandard = {};

                $scope.counter = 0;
                $scope.totalwagesall = 0;
                $scope.totalpensionfundall = 0;

                $scope.Employee.FromDate = $filter('date')($scope.Employee.FromDate, "yyyy-MM-dd");
                $scope.Employee.ToDate = $filter('date')($scope.Employee.ToDate, "yyyy-MM-dd");
                var data = $scope.Employee;

                apiService.create("PFForm6A/getEmployeedetailsBySelectionStjames", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        if (promise.pfreport !== null && promise.pfreport.length > 0) {
                            $scope.pfreport = promise.pfreport;
                        }

                        angular.forEach($scope.pfreport, function (val) {

                            //if (val.amountofWages >= 15000) {
                            //    var joiningDate = new Date(val.hrmE_DOJ);
                            //    //var checkingDate = new Date("2003-01-31");
                            //    var checkingDate = new Date("2014-08-31");

                            //    if (val.hrmE_EmployeeCode == "303" || val.hrmE_EmployeeCode == "308" || val.hrmE_EmployeeCode == "319" || val.hrmE_EmployeeCode == "321" || val.hrmE_EmployeeCode == "322" || val.hrmE_EmployeeCode == "324" || val.hrmE_EmployeeCode == "336" || val.hrmE_EmployeeCode == "302" || val.hrmE_EmployeeCode == "301" || val.hrmE_EmployeeCode == "334" || val.hrmE_EmployeeCode == "293" || val.hrmE_EmployeeCode == "318" || val.hrmE_EmployeeCode == "326" || val.hrmE_EmployeeCode == "236" || val.hrmE_EmployeeCode == "329" || val.hrmE_EmployeeCode == "311" || val.hrmE_EmployeeCode == "312" || val.hrmE_EmployeeCode == "337") {
                            //        val.pensionFund = 1250.00;
                            //    }
                            //    else if (joiningDate > checkingDate && (val.hrmE_EmployeeCode != '242' && val.hrmE_EmployeeCode != '237' && val.hrmE_EmployeeCode != '70')) {
                            //        val.pensionFund = 0.00;
                            //    }
                            //    else {
                            //        if (parseInt(val.hrmS_Age) >= 58) { val.pensionFund = 0.00; }
                            //        else { val.pensionFund = 1250.00; }
                            //    }
                            //}
                            //else {
                            //    if (parseInt(val.hrmS_Age) >= 58) { val.pensionFund = 0.00; }
                            //    else { val.pensionFund = Math.round((8.33 / 100) * val.amountofWages, 2); }
                               
                            //}


                            if (val.amountofWages < 15000 && val.hrmE_FPFNotApplicableFlg == true)
                            {
                                val.pensionFund = Math.round((8.33 / 100) * val.amountofWages, 2);

                            }
                            else if (val.hrmE_FPFNotApplicableFlg == true) {
                                val.pensionFund = 1250.00;
                            }
                            else {
                                val.pensionFund = 0.00;
                            }



                            //if (temp.hrmE_FPFNotApplicableFlg == false) {
                            //    temp.schoolpf = temp.stjOwnPF;
                            //}
                            //else {
                            //    temp.schoolpf = temp.stjOwnPF - temp.pensionFund;
                            //}
                            //temp.emptotdedSal = (temp.stjOwnPF + temp.vpfAmount);



                            if (val.pensionFund > 0) { $scope.counter++; }
                            if (val.pensionFund == 0) { val.amountofWages = 0; }
                            else if (val.amountofWages > 15000) { val.amountofWages = 15000; }
                            else { val.amountofWages = val.amountofWages; }

                            $scope.totalwagesall = $scope.totalwagesall + val.amountofWages;
                            $scope.totalpensionfundall += val.pensionFund;
                            console.log($scope.totalpensionfundall);
                        });


                        if (promise.finYearFromDate != "") {
                            $scope.finYearFromDate = promise.finYearFromDate;
                        }
                        if (promise.finYearToDate != "") {
                            $scope.finYearToDate = promise.finYearToDate;
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
                    });
            }
        };



        //Clear data
      
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.pfreport = [];
            $scope.submitted = false;
            $scope.finYearFromDate = "";
            $scope.finYearToDate = "";
            $scope.payrollStandard = {};
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        };

        $scope.getAmountofWagesTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.amountofWages;
                }
            }

            return total;
        };

        $scope.getpfAmountTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.pfAmount;
                }
            }

            return total;
        };
        $scope.gethreS_EPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_EPF;
                }
            }

            return total;
        };
        $scope.gethreS_FPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_FPF;
                }
            }

            return total;
        };

        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        };

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
        };


    }


})();