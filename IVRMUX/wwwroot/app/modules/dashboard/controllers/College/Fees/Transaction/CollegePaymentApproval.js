
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegePaymentApprovalController', CollegePaymentApprovalController)

    CollegePaymentApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegePaymentApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            apiService.getURI("CollegePaymentApproval/getalldetails", pageid).
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
                            apiService.create("CollegePaymentApproval/savedetails", data).then(function (promise) {
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

                            apiService.create("CollegePaymentApproval/savedetails", data).then(function (promise) {
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

                apiService.create("CollegePaymentApproval/Getreportdetails", data).
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
                                            AMCST_FirstName: dev.AMCST_FirstName,
                                            AMCST_MiddleName: dev.AMCST_MiddleName,
                                            AMCST_LastName: dev.AMCST_LastName,
                                            AMCO_CourseName: dev.AMCO_CourseName,
                                            AMB_BranchName: dev.AMB_BranchName,
                                            AMCST_AdmNo: dev.AMCST_AdmNo,
                                            FYP_ReceiptNo: dev.FYP_ReceiptNo,
                                            FYP_ReceiptDate: dev.FYP_ReceiptDate,
                                            FYP_TotalPaidAmount: dev.FYP_TotalPaidAmount,
                                            FYP_Bank_Or_Cash: dev.FYP_Bank_Or_Cash,
                                            FYP_Id: dev.FYP_Id,
                                            AMCST_Id: dev.AMCST_Id,
                                            FYP_Bank_Name: dev.FYP_Bank_Name,
                                            FYPPM_DDChequeNo: dev.FYPPM_DDChequeNo,
                                            FYPPM_DDChequeDate: dev.FYPPM_DDChequeDate,
                                            HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                            NormalizedUserName: dev.NormalizedUserName,
                                            FYP_Remarks: dev.FYP_Remarks,
                                            IVRMMOD_ModeOfPayment: dev.IVRMMOD_ModeOfPayment,
                                            FYPPM_ClearanceDate: dev.FYPPM_ClearanceDate,
                                            AMSE_Id: dev.AMSE_Id,
                                      
                                        });
                                    } else if ($scope.plannerid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.plannerid, function (emp) {
                                            if (emp.FYP_Id === dev.FYP_Id && emp.AMCST_Id === dev.AMCST_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.plannerid.push({
                                                AMCST_FirstName: dev.AMCST_FirstName,
                                                AMCST_MiddleName: dev.AMCST_MiddleName,
                                                AMCST_LastName: dev.AMCST_LastName,
                                                AMCO_CourseName: dev.AMCO_CourseName,
                                                AMB_BranchName: dev.AMB_BranchName,
                                                AMCST_AdmNo: dev.AMCST_AdmNo,
                                                FYP_ReceiptNo: dev.FYP_ReceiptNo,
                                                FYP_ReceiptDate: dev.FYP_ReceiptDate,
                                                FYP_TotalPaidAmount: dev.FYP_TotalPaidAmount,
                                                FYP_Bank_Or_Cash: dev.FYP_Bank_Or_Cash,
                                                FYP_Id: dev.FYP_Id,
                                                AMCST_Id: dev.AMCST_Id,
                                                FYP_Bank_Name: dev.FYP_Bank_Name,
                                                FYPPM_DDChequeNo: dev.FYPPM_DDChequeNo,
                                                FYPPM_DDChequeDate: dev.FYPPM_DDChequeDate,
                                                HRME_EmployeeFirstName: dev.HRME_EmployeeFirstName,
                                                NormalizedUserName: dev.NormalizedUserName,
                                                FYP_Remarks: dev.FYP_Remarks,
                                                IVRMMOD_ModeOfPayment: dev.IVRMMOD_ModeOfPayment,
                                                FYPPM_ClearanceDate: dev.FYPPM_ClearanceDate,
                                                AMSE_Id: dev.AMSE_Id,
                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.plannerid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach(promise.fillstudentviewdetails, function (dd) {
                                        if (dd.FYP_Id === ddd.FYP_Id && dd.AMCST_Id === ddd.AMCST_Id) {
                                            $scope.templist.push({
                                                FYP_Id: dd.FYP_Id,
                                                AMSE_Id: ddd.AMSE_Id,
                                                FMH_FeeName: dd.FMH_FeeName,
                                                FMH_Id: dd.FMH_Id,
                                                FTI_Name: dd.FTI_Name,
                                                FTI_Id: dd.FTI_Id,
                                                FMA_Id: dd.FMA_Id,
                                                FCSS_NetAmount: dd.FCSS_NetAmount,
                                                FCSS_TotalCharges: dd.FCSS_TotalCharges,
                                                FCSS_ConcessionAmount: dd.FCSS_ConcessionAmount,
                                                FCSS_FineAmount: dd.FCSS_FineAmount,
                                                FCSS_OBArrearAmount: dd.FCSS_OBArrearAmount,
                                                FCSS_ToBePaid: dd.FCSS_ToBePaid,
                                                FCSS_TotalToBePaidaddfine: dd.FCSS_TotalToBePaidaddfine,
                                                FCSS_PaidAmount: dd.FCSS_PaidAmount,
                                                FMG_GroupName: dd.FMG_GroupName,
                                                FTCP_PaidAmount: dd.FTCP_PaidAmount,
                                                FYPAPP_Remarks: "",
                                               
                                            });
                                        }
                                    });
                                    ddd.feeinstallmentdata = $scope.templist;
                                });
                                $scope.templistadvance = [];
                                angular.forEach(promise.fillstudentviewdetailsadvance, function (dd) {
                                    angular.forEach($scope.plannerid, function (ddd) {



                                        angular.forEach(ddd.feeinstallmentdata, function (d) {
                                            if (d.FYP_Id === ddd.FYP_Id) {




                                                if (d.FYP_Id === dd.FYP_Id && d.AMSE_Id != dd.AMSE_Id) {

                                                    if ($scope.templistadvance.length === 0) {
                                                        $scope.templistadvance.push(dd);
                                                    }
                                                    else if ($scope.templistadvance.length > 0) {
                                                        var intcount = 0;
                                                        angular.forEach($scope.templistadvance, function (emp) {
                                                            if (emp.FYP_Id === dd.FYP_Id && emp.FCMAS_Id === dd.FCMAS_Id && emp.FMH_Id === dd.FMH_Id) {
                                                                intcount += 1;
                                                                //$scope.templistadvance.push(dd);
                                                            }
                                                        });
                                                        if (intcount === 0) {
                                                            $scope.templistadvance.push(dd);
                                                        }
                                                    }

                                                }

                                            }
                                        });
                                        ddd.filladvancedetails = $scope.templistadvance;
                                    });
                                    angular.forEach($scope.plannerid, function (ddd) {
                                        $scope.templist = [];
                                        angular.forEach(promise.paymentremark, function (dd) {
                                            if (dd.FYP_Id === ddd.FYP_Id) {
                                                $scope.templist.push(dd);
                                            }
                                        });
                                        ddd.fillremarks = $scope.templist;
                                    });

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
