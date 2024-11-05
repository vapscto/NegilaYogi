(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSubscriptionController', MasterSubscriptionController)

    MasterSubscriptionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function MasterSubscriptionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

       //$scope.minDate = new Date();
        //$scope.maxDate = new Date();
        $scope.submitted = false;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";


        //=====================Load--data.............
        $scope.Loaddata = function () {
          
            debugger;
            var pageid = 2;
            apiService.getURI("MasterSubscription/getdetails", pageid).then(function (promise) {

                $scope.periodicitylist = promise.periodicitylist;
                $scope.publisherlist = promise.publisherlist;
                $scope.deptlist = promise.deptlist;
                $scope.vendorlist = promise.vendorlist;
                $scope.categorylist = promise.categorylist;
                $scope.languagelist = promise.languagelist;

                $scope.alldata = promise.alldata;

            })

        }
        //=====================End-----Load--data----//

       

        //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.lmsU_NoOfCopies == null || $scope.lmsU_NoOfCopies == undefined) {
                $scope.lmsU_NoOfCopies = 0;
            }
            //if ($scope.lmsU_NoOfIssues == null || $scope.lmsU_NoOfIssues == undefined) {
            //    $scope.lmsU_NoOfIssues = 0;
            //}
            
            if ($scope.myForm.$valid) {
                var data = {
                    "LMSU_Id": $scope.lmsU_Id,                   
                    "LMPE_Id": $scope.lmpE_Id,
                    "LMSU_PeriodicalTitle": $scope.lmsU_PeriodicalTitle,
                    "LMSU_OrderNo": $scope.lmsU_OrderNo,
                    "LMD_Id": $scope.lmD_Id,                     
                    "LMSU_EntryDate": $scope.lmsU_EntryDate,
                    "LMSU_OrderDate": $scope.lmsU_OrderDate,
                    "LMC_Id": $scope.lmC_Id,
                    "LML_Id": $scope.lmL_Id,
                    "LMSU_PeriodicalTypeFlg": $scope.lmsU_PeriodicalTypeFlg,                     
                    "LMP_Id": $scope.lmP_Id,

                    "LMV_Id": $scope.lmV_Id,
                    "LMSU_Price": $scope.lmsU_Price,
                    "LMSU_ExpectedDate": $scope.lmsU_ExpectedDate,
                    "LMSU_SubscriptionDate": $scope.lmsU_SubscriptionDate,
                    "LMSU_PreTerminationDate": $scope.lmsU_PreTerminationDate,
                    "LMSU_ExpiryDate": $scope.lmsU_ExpiryDate,

                    "LMSU_DoscountTypeFlg": $scope.lmsU_DoscountTypeFlg,
                    "LMSU_NetPrice": $scope.lmsU_NetPrice,
                    "LMSU_Discount": $scope.lmsU_Discount,
                    "LMSU_NoOfCopies": $scope.lmsU_NoOfCopies,
                    "LMSU_StartVolumeNo": $scope.lmsU_StartVolumeNo,
                    "LMSU_StartIssueNo": $scope.lmsU_StartIssueNo,
                    "LMSU_NoOfIssues": $scope.lmsU_NoOfIssues,
                    "LMSU_CurrencyType": $scope.lmsU_CurrencyType,
                    
                }
                apiService.create("MasterSubscription/Savedata", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.lmsU_Id > 0) {
                                    swal('Record Updated Successfully!!!', 'Update');
                                }
                                else {
                                    swal('Record Saved Successfully!!!', 'Save');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.lmsU_Id > 0) {
                                        swal('Record Not Updated Successfully!!!', 'Not Update');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!', 'Not Save');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---saverecord....


        //=====================Editrecord....
        $scope.eDitData = function (user) {
            debugger;
            var data = {
                "LMSU_Id": user.lmsU_Id
            };
           
            apiService.create("MasterSubscription/EditData", data).then(function (promise) {
                //$scope.editdata = promise.editdata;

                $scope.lmsU_Id = promise.editdata[0].lmsU_Id;
                $scope.lmpE_Id = promise.editdata[0].lmpE_Id;
                $scope.lmsU_PeriodicalTitle = promise.editdata[0].lmsU_PeriodicalTitle;
                $scope.lmsU_OrderNo = promise.editdata[0].lmsU_OrderNo;
                $scope.lmD_Id = promise.editdata[0].lmD_Id;                
                $scope.lmC_Id = promise.editdata[0].lmC_Id;
                $scope.lmL_Id = promise.editdata[0].lmL_Id;
                $scope.lmsU_PeriodicalTypeFlg = promise.editdata[0].lmsU_PeriodicalTypeFlg;
                $scope.lmP_Id = promise.editdata[0].lmP_Id;
                $scope.lmV_Id = promise.editdata[0].lmV_Id;
                $scope.lmsU_Price = promise.editdata[0].lmsU_Price;          

                $scope.lmsU_DoscountTypeFlg = promise.editdata[0].lmsU_DoscountTypeFlg;
                $scope.lmsU_NetPrice = promise.editdata[0].lmsU_NetPrice;
                $scope.lmsU_Discount = promise.editdata[0].lmsU_Discount;
                $scope.lmsU_NoOfCopies = promise.editdata[0].lmsU_NoOfCopies;
                $scope.lmsU_StartVolumeNo = promise.editdata[0].lmsU_StartVolumeNo;
                $scope.lmsU_StartIssueNo = promise.editdata[0].lmsU_StartIssueNo;
                $scope.lmsU_NoOfIssues = promise.editdata[0].lmsU_NoOfIssues;
                $scope.lmsU_CurrencyType = promise.editdata[0].lmsU_CurrencyType;
                $scope.lmsU_EntryDate = new Date(promise.editdata[0].lmsU_EntryDate);
                //$scope.lmsU_ExpiryDate = new Date(promise.editdata[0].lmsU_ExpiryDate);
                $scope.lmsU_OrderDate = new Date(promise.editdata[0].lmsU_OrderDate);
                $scope.lmsU_SubscriptionDate = new Date(promise.editdata[0].lmsU_SubscriptionDate);

                if (promise.editdata[0].lmsU_ExpiryDate != null && promise.editdata[0].lmsU_ExpiryDate != undefined) {
                    $scope.lmsU_ExpiryDate = new Date(promise.editdata[0].lmsU_ExpiryDate);
                }

                if (promise.editdata[0].lmsU_ExpectedDate != null && promise.editdata[0].lmsU_ExpectedDate != undefined) {
                    $scope.lmsU_ExpectedDate = new Date(promise.editdata[0].lmsU_ExpectedDate);
                }

                if (promise.editdata[0].lmsU_PreTerminationDate != null && promise.editdata[0].lmsU_PreTerminationDate != undefined) {
                    $scope.lmsU_PreTerminationDate = new Date(promise.editdata[0].lmsU_PreTerminationDate);
                }

                
            });
        }
        //====================End---edit-record....



        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmsU_Id = user.lmsU_Id;

            var dystring = "";
            if (user.lmsU_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmsU_ActiveFlg == 0) {
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
                        apiService.create("MasterSubscription/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }


        //-------check value for %age and Rs
        $scope.checkvalue = function () {
            debugger;
            if ($scope.lmsU_DoscountTypeFlg == '%age') {

                if (Number($scope.lmsU_Discount) > 100) {
                    swal('If Select %age then Can not Pass more then 100');
                    $scope.lmsU_Discount = '';
                }
                else {
                    var rss = $scope.lmsU_Price * $scope.lmsU_Discount / 100;
                       $scope.lmsU_NetPrice = $scope.lmsU_Price - rss;  
                }
            }
            else if ($scope.lmsU_DoscountTypeFlg == 'Rs') {
                
                if (Number($scope.lmsU_Discount) > $scope.lmsU_Price) {
                    swal('If Select RS then discount Can not Pass more then Price');
                    $scope.lmsU_Discount = '';
                }
                else {
                    $scope.lmsU_NetPrice = $scope.lmsU_Price - $scope.lmsU_Discount;
                }
            }
        }
        //-----------------End-----------------//

        //=================price calculation....
        $scope.lmsU_Discount = 0.00;
        $scope.get_netprice = function () {
            debugger;
            $scope.lmsU_NetPrice = $scope.lmsU_Price
            $scope.lmsU_Discount = 0;
          

        }


        $scope.get_empty = function () {

            $scope.lmsU_Discount = "";  
            $scope.lmsU_Price = "";  
            $scope.lmsU_NetPrice = "";  
        }

        



    }
})();

