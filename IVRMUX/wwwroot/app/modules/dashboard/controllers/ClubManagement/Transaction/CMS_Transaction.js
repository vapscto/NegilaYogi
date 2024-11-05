(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_TransactionController', CMS_TransactionController)
    CMS_TransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_TransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.flagDisabble = true;
        $scope.editflag = false;
        $scope.newdate = new Date();
        $scope.obj = {};
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Transaction/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.finacialyear = promise.finacial;
                $scope.allCaste = promise.getname;
            })
        };
        $scope.submitted = false;
       
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {                
                var CMSTRANS_Id = 0;
                if ($scope.CMSTRANS_Id > 0) {
                    CMSTRANS_Id = $scope.CMSTRANS_Id;
                }
                var data = {
                    "CMSMMEM_Id": $scope.obj.cmsmmeM_Id.cmsmmeM_Id,
                    "IMFY_Id": $scope.IMFY_Id,
                    "CMSTRANS_TransactionNo": $scope.CMSTRANS_TransactionNo,
                    "CMSTRANS_Date": new Date($scope.CMSTRANS_Date).toDateString(),
                    "CMSTRANS_TotalAmount": $scope.CMSTRANS_TotalAmount,
                    "CMSTRANS_TotalTax": $scope.CMSTRANS_TotalTax,
                    "CMSTRANS_TotalNetAmount": $scope.CMSTRANS_TotalNetAmount,
                    "CMSTRANS_NoOFGuests": $scope.CMSTRANS_NoOFGuests,
                    "CMSTRANS_GuestsName": $scope.CMSTRANS_GuestsName,
                    "CMSTRANS_GuestContactNo": $scope.CMSTRANS_GuestContactNo,
                    "CMSTRANS_Remarks": $scope.CMSTRANS_Remarks,
                    "CMSTRANS_Id": CMSTRANS_Id,
                    "TransctionNon_Member": $scope.transrows
                }
                apiService.create("CMS_Transaction/savedata", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                       $state.reload();
                    })
            }

        };
        $scope.totalAmount = function () {
            $scope.CMSTRANS_TotalNetAmount = "";
            if ($scope.CMSTRANS_TotalAmount != null && $scope.CMSTRANS_TotalTax != null && $scope.CMSTRANS_TotalAmount > 0 && $scope.CMSTRANS_TotalTax > 0) {
                var NetAmount = 0;
                NetAmount = (Number($scope.CMSTRANS_TotalAmount) - Number($scope.CMSTRANS_TotalTax));
                if (NetAmount > 0) {
                    $scope.CMSTRANS_TotalNetAmount = NetAmount;
                }
                else {
                    swal("Total Amount Not greter than Total Tax amount");
                    $scope.CMSTRANS_TotalTax = "";
                }
            }
            else {
               // swal("Please Enter Total Amount / Total tax");
                $scope.CMSTRANS_TotalTax = "";
            }
        };
        $scope.totalAmountTwo = function () {
            $scope.CMSTRANS_TotalNetAmount = "";
            $scope.CMSTRANS_TotalTax = "";
            
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interactedone = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        //
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {

            var data = {

                "CMSTRANS_Id": item.cmstranS_Id
            }
            var dystring = "";
            if (item.cmstranS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.cmstranS_ActiveFlg == false) {
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
                        apiService.create("CMS_Transaction/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }

                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (user) {
            
             var data = {

                 "CMSTRANS_Id": user.cmstranS_Id
                }
                apiService.create("CMS_Transaction/edit", data).
                    then(function (promise) {
                       
                        if (promise.edittransction != null && promise.edittransction.length > 0) {
                            $scope.CMSTRANS_Id = promise.edittransction[0].cmstranS_Id;
                           
                            $scope.IMFY_Id = promise.edittransction[0].imfY_Id;
                            $scope.CMSTRANS_TransactionNo = promise.edittransction[0].cmstranS_TransactionNo;
                            $scope.CMSTRANS_Date = new Date(promise.edittransction[0].cmstranS_Date);
                            $scope.CMSTRANS_TotalAmount = promise.edittransction[0].cmstranS_TotalAmount;
                            $scope.CMSTRANS_TotalTax = promise.edittransction[0].cmstranS_TotalTax;
                            $scope.CMSTRANS_TotalNetAmount = promise.edittransction[0].cmstranS_TotalNetAmount;
                            $scope.CMSTRANS_NoOFGuests = promise.edittransction[0].cmstranS_NoOFGuests;
                            $scope.CMSTRANS_GuestsName = promise.edittransction[0].cmstranS_GuestsName;
                            $scope.CMSTRANS_GuestContactNo = promise.edittransction[0].cmstranS_GuestContactNo;
                            $scope.CMSTRANS_Remarks = promise.edittransction[0].cmstranS_Remarks;
                            if (promise.editnmember != null && promise.editnmember.length > 0) {
                                $scope.transrows = promise.editnmember;
                            }
                            //$scope.obj.cmsmmeM_Id = promise.edittransction[0].cmsmmeM_Id;
                            if ($scope.allCaste.length > 0) {
                                for (var i = 0; i < $scope.allCaste.length; i++) {
                                    if (promise.edittransction[0].cmsmmeM_Id == $scope.allCaste[i].cmsmmeM_Id) {
                                        $scope.allCaste[i].Selected = true;
                                        $scope.obj.cmsmmeM_Id = $scope.allCaste[i];
                                        $scope.newcaste = promise.edittransction[0].cmsmmeM_Id;
                                        $scope.editflag = true;
                                    }
                                }
                            }
                           
                        }
                        else if (promise.returnval == "admin")
                        {
                            swal("Please Contact Administrator !");
                        }
                        else {
                            swal("Please Contact Administrator !");
                        }

                    })
         


        };
        $scope.clear = function () {
            $state.reload();
        }
        $scope.submitted1 = false;
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            $scope.submitted1 = true;
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
                $scope.submitted1 = true;
            }
        };
        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

        };
    }

})();