(function () {
    'use strict';
    angular.module('app').controller('PC_ExpenditureController', PC_ExpenditureController)

    PC_ExpenditureController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_ExpenditureController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.submitted = false;
        $scope.maxdate = new Date();
        $scope.PCEXPTR_Date = new Date();
        $scope.obj = {};

        $scope.indentdetails = 0;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.divdetails = false;
        $scope.imgname = logopath;
        $scope.submitted = false;

        $scope.ExpenditureLoaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Indent_Approval/ExpenditureLoaddata", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.getexpenditureloaddata = promise.getexpenditureloaddata;
                $scope.MI_Id = promise.mI_Id;
            });
        };

        $scope.OnChangeExpenditureInstitution = function () {
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.getindentapprovaldetails = [];
            $scope.getexpenditureloaddata = [];
            $scope.PCINDENTAP_Id = "";
            $scope.indentdetails = 0;
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("PC_Indent_Approval/OnChangeExpenditureInstitution", data).then(function (promise) {
                $scope.getindentapprovaldetails = promise.getindentapprovaldetails;
                $scope.getparticularsdetails = promise.getparticularsdetails;
                $scope.getexpenditureloaddata = promise.getexpenditureloaddata;

            });
        };

        $scope.OnChangeExpenditureIndent = function () {
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.PCMPART_Id = "";
            $scope.indentdetails = 0;
            var data = {
                "MI_Id": $scope.MI_Id,
                "PCINDENTAP_Id": $scope.PCINDENTAP_Id
            };

            apiService.create("PC_Indent_Approval/OnChangeExpenditureIndent", data).then(function (promise) {
                $scope.getparticularsdetails = promise.getparticularsdetails;
            });
        };

        $scope.OnChangeExpenditureParticular = function () {
            $scope.indentdetails = 0;
            $scope.submitted = false;
            if ($scope.PCINDENTAP_Id !== undefined && $scope.PCINDENTAP_Id !== null && $scope.PCINDENTAP_Id !== "") {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id
                };
                apiService.create("PC_Indent_Approval/OnChangeExpenditureParticular", data).then(function (promise) {
                    $scope.divdetails = true;
                    $scope.indentdetails = 1;

                    $scope.getparticularsindentdetails = promise.getparticularsindentdetails;
                    $scope.PCMPART_ParticularName = $scope.getparticularsindentdetails[0].pcmparT_ParticularName;
                    $scope.PCINDENTAPDT_RequestedAmount = $scope.getparticularsindentdetails[0].pcindentapdT_RequestedAmount;
                    $scope.PCINDENTAPDT_SanctionedAmt = $scope.getparticularsindentdetails[0].pcindentapdT_SanctionedAmt;
                    $scope.PCINDENTAPDT_AmountSpent = $scope.getparticularsindentdetails[0].pcindentapdT_AmountSpent;
                    $scope.PCINDENTAPDT_BalanceAmount = $scope.getparticularsindentdetails[0].pcindentapdT_BalanceAmount;

                    $scope.getindentdetails = promise.getindentdetails;
                    $scope.PCINDENT_IndentNo = $scope.getindentdetails[0].pcindenT_IndentNo;
                    $scope.PCINDENTAPT_RequestedAmount = $scope.getindentdetails[0].pcindentapT_RequestedAmount;
                    $scope.PCINDENTAPT_SanctionedAmt = $scope.getindentdetails[0].pcindentapT_SanctionedAmt;
                    $scope.PCINDENTAPT_AmountSpent = $scope.getindentdetails[0].pcindentapT_AmountSpent;
                    $scope.PCINDENTAPT_BalanceAmount = $scope.getindentdetails[0].pcindentapT_BalanceAmount;
                });
            } else {
                $scope.divdetails = true;
            }
        };

        $scope.SaveExpenditure = function () {

            if ($scope.myForm.$valid) {

                $scope.PCEXPTR_Id = $scope.PCEXPTR_Id !== undefined && $scope.PCEXPTR_Id !== null && $scope.PCEXPTR_Id !== "" ? $scope.PCEXPTR_Id : 0;

                $scope.PCEXPTR_ReferenceNo = $scope.PCEXPTR_ReferenceNo !== undefined && $scope.PCEXPTR_ReferenceNo !== null && $scope.PCEXPTR_ReferenceNo !== "" ? $scope.PCEXPTR_ReferenceNo : "";

                $scope.PCEXPTR_Desc = $scope.PCEXPTR_Desc !== undefined && $scope.PCEXPTR_Desc !== null && $scope.PCEXPTR_Desc !== "" ? $scope.PCEXPTR_Desc : "";

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "PCEXPTR_Id": $scope.PCEXPTR_Id,
                    "PCINDENTAP_Id": $scope.PCINDENTAP_Id,
                    "PCMPART_Id": $scope.PCMPART_Id,
                    "PCEXPTR_Date": new Date($scope.PCEXPTR_Date).toDateString(),
                    "PCEXPTR_Amount": $scope.obj.PCEXPTR_Amount,
                    "PCEXPTR_ReferenceNo": $scope.obj.PCEXPTR_ReferenceNo,
                    "PCEXPTR_ModeOfPayment": $scope.obj.PCEXPTR_ModeOfPayment,
                    "PCEXPTR_Desc": $scope.obj.PCEXPTR_Desc
                };
                apiService.create("PC_Indent_Approval/SaveExpenditure", data).then(function (promise) {

                    if (promise.saveorupdate === "ReferenceNotGenerated") {
                        swal("Reference No. Not Generated");
                    } else if (promise.saveorupdate === "Add") {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    } else if (promise.saveorupdate === "Update") {
                        if (promise.returnval === true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else {
                        swal("Failed To Save / Update Record");
                    }
                    
                    $scope.clearData();
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.DeleteExpenditure = function (obj) {

            swal({
                title: "Delete Expenditure!",
                text: "Are You Sure You Want To Delete This Record",
                type: "input",
                showCancelButton: false,
                closeOnConfirm: false,
                inputPlaceholder: "Enter Remarks",
                confirmButtonText: "OK"
            }, function (inputValue) {
                if (inputValue === false) return false;
                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false;
                }
                if (inputValue !== "") {                     
                    var data = {
                        "PCEXPTR_Id": obj.pcexptR_Id,
                        "MI_Id": $scope.MI_Id,
                        "PCEXPTR_DeletedRemarks": inputValue
                    };
                    apiService.create("PC_Indent_Approval/DeleteExpenditure", data).then(function (promise) {
                        if (promise !== null) {
                            swal("OK");
                        }
                    });
                }
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };      

        $scope.clearData = function () {
            $scope.PCEXPTR_Date = new Date();
            $scope.MI_Id = "";
            $scope.getuserinstitution = [];
            $scope.divdetails = false;
            $scope.submitted = false;
            $scope.getparticularsdetails = [];
            $scope.getindentapprovaldetails = [];
            $scope.getexpenditureloaddata = [];
            $scope.PCINDENTAP_Id = "";
            $scope.PCMPART_Id = "";
            $scope.indentdetails = 0;

            $scope.PCINDENT_IndentNo = "";
            $scope.PCINDENTAPT_RequestedAmount = "";
            $scope.PCINDENTAPT_SanctionedAmt = "";
            $scope.PCINDENTAPT_AmountSpent = "";
            $scope.PCINDENTAPT_BalanceAmount = "";

            $scope.PCMPART_ParticularName = "";
            $scope.PCINDENTAPDT_RequestedAmount = "";
            $scope.PCINDENTAPDT_SanctionedAmt = "";
            $scope.PCINDENTAPDT_AmountSpent = "";
            $scope.PCINDENTAPDT_BalanceAmount = "";

            $scope.PCEXPTR_Amount = "";
            $scope.PCEXPTR_ReferenceNo = "";
            $scope.PCEXPTR_ModeOfPayment = "";
            $scope.PCEXPTR_Desc = "";

            $scope.ExpenditureLoaddata();
        };

        $scope.search = '';

        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.pcexptR_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||                
                (angular.lowercase(obj.pcexptR_ExpenditureNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcexptR_ModeOfPayment)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcmparT_ParticularName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.pcexptR_Amount)).indexOf($scope.search) >= 0;
        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };
    }
})();
