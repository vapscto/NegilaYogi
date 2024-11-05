
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeesMakerAndCheckerController', FeesMakerAndCheckerController)

    FeesMakerAndCheckerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeesMakerAndCheckerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        //     $scope.stulstdis = true;
        $scope.btndiv = false;
        $scope.search = "";
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));


        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));





        $scope.resultData = [];
        $scope.resultData1 = [];


        $scope.page1 = "page1";
        $scope.reverse1 = true;

        $scope.page2 = "page2";
        $scope.reverse2 = true;

        $scope.page3 = "page3";
        $scope.reverse3 = true;

        $scope.cfg = {};
        $scope.Grid_View = false;
        $scope.submitted = false;
        $scope.fromDate = new Date();
        $scope.todate = new Date();
        $scope.loaddata = function () {

            $scope.disableconcessionamount = true;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 5




            var pageid = 2;
            apiService.getURI("FeesMakerAndChecker/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.adcyear;
                    $scope.cfg.ASMAY_Id = promise.adcyear[0].asmaY_Id;



                })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        $scope.Cancel = function () {
            $state.reload();
        }




        $scope.approve = function (fyp_id, remark, dta) {

            $scope.studentdetails = [];
            angular.forEach($scope.plannerid, function (itm) {
                if (itm.studchecked == true) {
                    $scope.studentdetails.push(itm);
                }
            });
            if ($scope.studentdetails != null && $scope.studentdetails.length > 0) {
                var data = {
                    ASMAY_Id: $scope.cfg.ASMAY_Id,
                    studentdata: $scope.studentdetails,
                    "FYPAPP_Remarks": remark,
                    "FYPAPP_ApprovedFlg": 1
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Approve The Record? ",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false,
                    showLoaderOnConfirm: true,
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("FeesMakerAndChecker/savedetails", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.returnval == "admin") {
                                        swal("Contact Adminstator");
                                    }
                                    else if (promise.returnval == "save") {
                                        swal("Record Saved Successfully")
                                    }
                                    else if (promise.returnval == "Notsave") {
                                        swal("Record Not Saved Successfully")
                                    }
                                    $state.reload();
                                }
                                else {
                                    swal("Contact Adminstator");
                                }
                            });
                        }
                        else {
                            swal("Request Failed", "Failed");
                        }
                    });

            }
            else {
                swal("Select Atleast One Student");
            }
        }

        $scope.rejected = function (fyp_id, remark) {

            $scope.studentdetails = [];
            angular.forEach($scope.plannerid, function (itm) {
                if (itm.studchecked == true) {


                    $scope.studentdetails.push(itm);

                }
            });
            if ($scope.studentdetails != null && $scope.studentdetails.length > 0) {
                var data = {
                    ASMAY_Id: $scope.cfg.ASMAY_Id,
                    studentdata: $scope.studentdetails,
                    "FYPAPP_ApprovedFlg": 0,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Reject The Record? ",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update it",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false,
                    showLoaderOnConfirm: true,
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            apiService.create("FeesMakerAndChecker/savedetails", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.returnval == "admin") {
                                        swal("Contact Adminstator");
                                    }
                                    else if (promise.returnval == "save") {
                                        swal("Record Saved Successfully")
                                    }
                                    else if (promise.returnval == "Notsave") {
                                        swal("Record Not Saved Successfully")
                                    }
                                    $state.reload();
                                }
                                else {
                                    swal("Contact adminstator");
                                }
                            });
                        }
                        else {
                            swal("Request Failed", "Failed");
                        }
                    }
                );
            }
            else {
                swal("Select Atleast One Student");
            }

        }

        $scope.ShowReport = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {




                var data = {
                    ASMAY_Id: $scope.cfg.ASMAY_Id,

                    Fromdate: $scope.fromDate,
                    Todate: $scope.todate
                }

                $scope.modelist = [];

                apiService.create("FeesMakerAndChecker/Getreportdetails", data).
                    then(function (promise) {
                        $scope.plannerid = [];
                        //$scope.get_approvalreport = [];
                        if (promise.feepaymentreport != null) {


                            if (promise.feepaymentreport.length > 0) {
                                $scope.Grid_View = true;
                                $scope.get_approvalreport = promise.feepaymentreport;
                                angular.forEach($scope.get_approvalreport, function (dev) {
                                    if ($scope.plannerid.length === 0) {

                                        $scope.plannerid.push({
                                            AMST_FirstName: dev.AMST_FirstName,
                                            AMST_MiddleName: dev.AMST_MiddleName,
                                            AMST_LastName: dev.AMST_LastName,
                                            ASMCL_ClassName: dev.ASMCL_ClassName,
                                            ASMC_SectionName: dev.ASMC_SectionName,
                                            AMST_AdmNo: dev.AMST_AdmNo,
                                            FYP_Receipt_No: dev.FYP_Receipt_No,
                                            FYP_ReceiptDate: dev.FYP_ReceiptDate,
                                            FYP_TotalPaidAmount: dev.FYP_TotalPaidAmount,
                                            FYP_Bank_Or_Cash: dev.FYP_Bank_Or_Cash,
                                            FYP_Id: dev.FYP_Id,
                                            AMST_Id: dev.AMST_Id,
                                            FYP_Bank_Name: dev.FYP_Bank_Name,
                                            FYPPM_DDChequeNo: dev.FYPPM_DDChequeNo,
                                            FYPPM_DDChequeDate: dev.FYPPM_DDChequeDate,
                                            HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                            NormalizedUserName: dev.NormalizedUserName,
                                            FYP_Remarks: dev.FYP_Remarks,
                                            IVRMMOD_ModeOfPayment: dev.IVRMMOD_ModeOfPayment,
                                          //  FYPPM_ClearanceDate: dev.FYPPM_ClearanceDate,
                                            FYP_Date: dev.FYP_Date,
                                            FYP_Tot_Amount: dev.FYP_Tot_Amount 

                                        });
                                    } else if ($scope.plannerid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.plannerid, function (emp) {
                                            if (emp.FYP_Id === dev.FYP_Id && emp.AMST_Id === dev.AMST_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.plannerid.push({
                                                AMST_FirstName: dev.AMST_FirstName,
                                                AMST_MiddleName: dev.AMST_MiddleName,
                                                AMST_LastName: dev.AMST_LastName,
                                                ASMCL_ClassName: dev.ASMCL_ClassName,
                                                ASMC_SectionName: dev.ASMC_SectionName,
                                                AMST_AdmNo: dev.AMST_AdmNo,
                                                FYP_Receipt_No: dev.FYP_Receipt_No,
                                                FYP_ReceiptDate: dev.FYP_ReceiptDate,
                                                FYP_TotalPaidAmount: dev.FYP_TotalPaidAmount,
                                                FYP_Bank_Or_Cash: dev.FYP_Bank_Or_Cash,
                                                FYP_Id: dev.FYP_Id,
                                                AMST_Id: dev.AMST_Id,
                                                FYP_Bank_Name: dev.FYP_Bank_Name,
                                                FYPPM_DDChequeNo: dev.FYPPM_DDChequeNo,
                                                FYPPM_DDChequeDate: dev.FYPPM_DDChequeDate,
                                                HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                                NormalizedUserName: dev.NormalizedUserName,
                                                FYP_Remarks: dev.FYP_Remarks,
                                                IVRMMOD_ModeOfPayment: dev.IVRMMOD_ModeOfPayment,
                                                //FYPPM_ClearanceDate: dev.FYPPM_ClearanceDate,
                                              
                                                FYP_Date: dev.FYP_Date,
                                                FYP_Tot_Amount: dev.FYP_Tot_Amount
                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.plannerid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach(promise.fillstudentviewdetails, function (dd) {
                                        if (dd.FYP_Id === ddd.FYP_Id && dd.AMST_Id === ddd.AMST_Id) {
                                            $scope.templist.push({
                                                FYP_Id: dd.FYP_Id,
                                                AMSE_Id: ddd.AMSE_Id,
                                                FMH_FeeName: dd.FMH_FeeName,
                                                FMH_Id: dd.FMH_Id,
                                                FTI_Name: dd.FTI_Name,
                                                FTI_Id: dd.FTI_Id,
                                                FMA_Id: dd.FMA_Id,
                                                FSS_NetAmount: dd.FSS_NetAmount,
                                                FSS_TotalCharges: dd.FSS_TotalCharges,
                                                FSS_ConcessionAmount: dd.FSS_ConcessionAmount,
                                                FSS_FineAmount: dd.FSS_FineAmount,
                                                FSS_OBArrearAmount: dd.FSS_OBArrearAmount,
                                                FSS_ToBePaid: dd.FSS_ToBePaid,
                                                FSS_TotalToBePaidaddfine: dd.FSS_TotalToBePaidaddfine,
                                                FSS_PaidAmount: dd.FSS_PaidAmount,
                                                FMG_GroupName: dd.FMG_GroupName,
                                                FTP_Paid_Amt: dd.FTP_Paid_Amt,
                                                FYPAPP_Remarks: "",
                                        

                                            });
                                        }
                                    });
                                    ddd.feeinstallmentdata = $scope.templist;
                                });
                             




                            }

                            else {
                                swal("No Record Found");

                                $scope.Grid_View = false;
                            }
                        }
                        else {
                            swal("No Record Found");

                            $scope.Grid_View = false;
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };



        $scope.toggleAllstu = function (allchkdatastudent) {

            if (allchkdatastudent == true) {
                var toggleStatusstu = $scope.selectedAllstu;
                angular.forEach($scope.plannerid, function (itm) {
                    itm.studchecked = true;
                });
            }
            else {
                angular.forEach($scope.plannerid, function (itm) {
                    itm.studchecked = false;
                });

            }
        }
    }

})();
