(function () {
    'use strict';
    angular
.module('app')
        .controller('FeePaymentGatewayDetailsController', FeePaymentGatewayDetailsController)
    FeePaymentGatewayDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$stateParams']
    function FeePaymentGatewayDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {
        $scope.search = "";
        $scope.disableaddbtn = false;
        $scope.savedisable = true;
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }

        $scope.submerchantId = [{ id: 'submerchantId' }];
        $scope.addNew = function () {
            var newItem = $scope.submerchantId.length + 1;
            if (newItem <= 5) {
                $scope.submerchantId.push({ 'id': 'submerchantId' + newItem });
            }
            if (newItem == 4) {
                $scope.myForm1.$setPristine();
            }
        };
        $scope.delsubmerchantId = [];
        $scope.removeSubMerchantId = function (index) {            
            var newItem1 = $scope.submerchantId.length - 1;
            if (newItem1 !== 0) {
                $scope.delsubmerchantId = $scope.submerchantId.splice(index, 1);
            }
        };
        $scope.getPaymentGatewayDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeePaymentGatewayDetails/getPaymentGatewayDetails", pageid).
            then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.gateway_list = promise.gateway_list;
                $scope.MI_Id = promise.institutionlist[0].mI_Id;              
                if (promise.count > 0) {
                    $scope.PaymentGatewayDetailList = promise.paymentGatewayDetailList;
                    $scope.totcountfirst = promise.paymentGatewayDetailList.length;
                    $scope.count = 1; 
                }
                else {
                    swal("No Records Found");
                    $scope.count = 0;
                }
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };
        $scope.delete = function (det, SweetAlert) {
            $scope.Id = det.fpgD_Id;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeePaymentGatewayDetails/deletePaymentGatewayDetails/", $scope.Id).
                    then(function (promise) {                        
                        if (promise.returnval == "deleted") {
                            swal('Record Deleted Successfully');
                            $state.reload();
                            return;
                        }
                        else if (promise.returnval === "deletefailed") {
                            swal('Record Not Deleted');
                            return;
                        }
                        else if (promise.returnval === "Mapped") {
                            swal("Sorry.....You can not delete this record.Because it is already mapped");
                        }
                    })
                }
                else {
                    swal("Cancelled");
                }
            });
        }
        $scope.edit = function (det) {
            $scope.FPGD_Id = det.fpgD_Id;
            $scope.submerchantId.length = 1;
            apiService.getURI("FeePaymentGatewayDetails/editPaymentGatewayDetails/", $scope.FPGD_Id).
            then(function (promise) {
                $scope.MI_Id = promise.paymentGatewayDetailList[0].mI_Id;
                $scope.FPGD_MerchantId = promise.paymentGatewayDetailList[0].fpgD_MerchantId;
                $scope.FPGD_SaltKey = promise.paymentGatewayDetailList[0].fpgD_SaltKey;
                $scope.FPGD_AuthorisationKey = promise.paymentGatewayDetailList[0].fpgD_AuthorisationKey;
                $scope.FPGD_URL = promise.paymentGatewayDetailList[0].fpgD_URL;
                $scope.IMPG_Id = promise.paymentGatewayDetailList[0].impG_Id;
                $scope.FPGD_Image = promise.paymentGatewayDetailList[0].fpgD_Image;
                $scope.FPGD_AccNo = promise.paymentGatewayDetailList[0].fpgD_AccNo;

                if (promise.paymentGatewayDetailList[0].fpgD_MobileActiveFlag != null) {
                    if (promise.paymentGatewayDetailList[0].fpgD_MobileActiveFlag == 0) {
                        $scope.FPGD_MobileActiveFlag = false;
                    }
                    else {
                        $scope.FPGD_MobileActiveFlag = true;
                    }
                }
                else {
                    $scope.FPGD_MobileActiveFlag = false;
                }

                $scope.submerchantId[0].FPGD_SubMerchantId = promise.paymentGatewayDetailList[0].fpgD_SubMerchantId;

                $scope.disableaddbtn = true;
            })
        }
        $scope.submitted = false;
        $scope.savepaymentDetails = function () {          
            if ($scope.myForm.$valid) {                
                var data = {
                    "FPGD_Id":$scope.FPGD_Id,
                    "MI_Id": $scope.MI_Id,
                    "FPGD_MerchantId": $scope.FPGD_MerchantId,
                    "FPGD_SaltKey": $scope.FPGD_SaltKey,
                    "FPGD_AuthorisationKey": $scope.FPGD_AuthorisationKey,
                    "FPGD_URL": $scope.FPGD_URL,
                    "IMPG_Id": $scope.IMPG_Id,
                    "FPGD_Image": $scope.FPGD_Image,
                    "FPGD_AccNo": $scope.FPGD_AccNo,
                    "FPGD_MobileActiveFlag": $scope.FPGD_MobileActiveFlag,                   
                    "SubmerchantIds": $scope.submerchantId,                   
                }
                apiService.create("FeePaymentGatewayDetails/", data)
                .then(function (promise) {
                    if (promise.isduplicate == "duplicate" && promise.returnval == "saved") {
                        alert('Some Record already exist');
                    }
                   else if (promise.isduplicate == "duplicate") {
                        swal('Record already exist');
                    }
                    if (promise.returnval == "saved") {
                        swal('Record Saved Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "savingfailed") {
                        swal('Record Saving Failed');
                    }
                    else if (promise.returnval == "updated") {
                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnval == "updatefailed") {
                        swal('Record Updation Failed');
                    }
                    else if (promise.returnval == "failed") {
                        swal('Operation Failed');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();
