(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgMasterQuotaController', ClgMasterQuotaController)
    ClgMasterQuotaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function ClgMasterQuotaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {


        $scope.searc_button = true;
        $scope.sortKey = 'acQ_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";

        $scope.searc_button1 = true;
        $scope.sortKey1 = 'acqC_Id';
        $scope.searchValue1 = "";

        $scope.searchchkbx = "";


        $scope.isOptionsRequired = function () {
            return !$scope.getdetails1.some(function (options) {
                return options.checkedsub;
            });
        }

        $scope.searc_button2 = true;
        $scope.sortKey2 = 'acqcM_Id';
        $scope.searchValue2 = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //------------------------------------------------------------------------------
        $scope.clgQuotaload = function () {
            var pageid = 2;
            apiService.getURI("ClgMasterQuota/getalldetails", pageid).
                then(function (promise) {

                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;

                    $scope.currentPage1 = 1;
                    $scope.itemsPerPage1 = paginationformasters;

                    $scope.currentPage2 = 1;
                    $scope.itemsPerPage2 = paginationformasters;

                    $scope.getdetails = promise.getdetails;
                    $scope.presentCountgrid = $scope.getdetails.length;

                    $scope.getdetails1 = promise.getdetails1;
                    $scope.presentCountgrid1 = $scope.getdetails1.length;

                    $scope.getdetails2 = promise.getdetails2;
                    $scope.presentCountgrid2 = $scope.getdetails2.length;
                })
        };

        //------------------------------MASTER QUOTA------------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "ACQ_QuotaName": $scope.acQ_QuotaName,
                    "ACQ_QuotaCode": $scope.acQ_QuotaCode,
                    "ACQ_QuotaInfo": $scope.acQ_QuotaInfo,
                    "ACQ_Id": $scope.acQ_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ClgMasterQuota/savedetails", data).then(function (promise) {

                    if (promise.message == "Add") {

                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.returnduplicatestatus == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Something Went Wrong please contact administrator");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.activedeactiveQuota = function (data, SweetAlert) {
            var mgs = "";
            if (data.acQ_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Quota?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgMasterQuota/activedeactiveQuota", data).then(function (promise) {

                            if (promise.message == "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message == "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval === "true") {
                                    swal("Quota " + mgs + "Successfully");
                                } else {
                                    swal("Quota " + mgs + "Successfully");
                                }
                            }
                            $state.reload();
                        })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.editQuota = function (id) {

            var data = {
                "ACQ_Id": id
            }

            apiService.create("ClgMasterQuota/editdetails", data).then(function (promise) {
                if (promise.editdetails.length > 0) {
                    $scope.acQ_QuotaName = promise.editdetails[0].acQ_QuotaName;
                    $scope.acQ_QuotaCode = promise.editdetails[0].acQ_QuotaCode;
                    $scope.acQ_QuotaInfo = promise.editdetails[0].acQ_QuotaInfo;
                    $scope.acQ_Id = promise.editdetails[0].acQ_Id;
                }
            })

        }

        //------------------------------ QUOTA category------------------------------------------------
        $scope.submitted1 = false;
        $scope.savedata1 = function () {
            debugger;


            if ($scope.myForm1.$valid) {
                var data = {
                    "ACQC_CategoryName": $scope.acqC_CategoryName,
                    "ACQC_CategoryCode": $scope.acqC_CategoryCode,
                    "ACQC_CategoryInfo": $scope.acqC_CategoryInfo,
                    "ACQC_Id": $scope.acqC_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ClgMasterQuota/savedetails1", data).then(function (promise) {
                    if (promise.message == "Add") {

                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.returnduplicatestatus == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Something Went Wrong please contact administrator");
                    }
                    // $state.reload();
                    $scope.clgQuotaload();
                    $scope.acqC_CategoryName = "";
                    $scope.acqC_CategoryCode = "";
                    $scope.acqC_CategoryInfo = "";
                    $scope.myForm1.$setPristine();
                    $scope.myForm1.$setUntouched();
                })
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.cancel1 = function () {
            $scope.submitted1 = false;
            $scope.acqC_CategoryName = "";
            $scope.acqC_CategoryCode = "";
            $scope.acqC_CategoryInfo = "";
            $scope.searchValue1 = "";
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        }

        $scope.activedeactiveQuota1 = function (data, SweetAlert) {
            var mgs = "";
            if (data.acqC_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Quota category?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgMasterQuota/activedeactiveQuota1", data).then(function (promise) {

                            if (promise.message == "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message == "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval === "true") {
                                    swal("Quota " + mgs + "Successfully");
                                } else {
                                    swal("Quota " + mgs + "Successfully");
                                }
                            }
                            $scope.clgQuotaload();
                        })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }

        $scope.editQuota1 = function (id) {

            var data = {
                "acqC_Id": id
            }

            apiService.create("ClgMasterQuota/editdetails1", data).then(function (promise) {
                if (promise.editdetails1.length > 0) {
                    $scope.acqC_CategoryName = promise.editdetails1[0].acqC_CategoryName;
                    $scope.acqC_CategoryCode = promise.editdetails1[0].acqC_CategoryCode;
                    $scope.acqC_CategoryInfo = promise.editdetails1[0].acqC_CategoryInfo;
                    $scope.acqC_Id = promise.editdetails1[0].acqC_Id;

                }
            })

        }

        //------------------------------ QUOTA category Mapping------------------------------------------------
        //---- QUOTA category Select All
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.getdetails1, function (itm) {
                itm.checkedsub = toggleStatus;
            });
        }
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.getdetails1.every(function (options) {
                return options.checkedsub;
            });
        }

        $scope.savedata2 = function () {
            $scope.submitted2 = true;

            if ($scope.myForm2.$valid) {

                $scope.array = [];
                var chk_count = 0;
                angular.forEach($scope.getdetails1, function (itm) {
                    if (itm.checkedsub == true) {
                        chk_count += 1;
                        $scope.array.push(itm);
                    }
                });
                var data = {
                    "ACQ_Id": $scope.acQ_Id,
                    "ACQCM_Id": $scope.acqcM_Id,
                    "QuotaClist": $scope.array,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ClgMasterQuota/savedetails2", data).then(function (promise) {

                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.returnduplicatestatus == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {
                        swal("Something Went Wrong please contact administrator");
                    }
                    $scope.cancel2();
                    $scope.clgQuotaload();
                })
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.cancel2 = function () {
            $scope.submitted2 = false;
            $scope.acQ_Id = "";
            $scope.searchchkbx = "";
            $scope.usercheck = false;
            angular.forEach($scope.getdetails1, function (itm) {
                itm.checkedsub = false;
            })
            $scope.searchValue2 = "";
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        }

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.activedeactiveQuota2 = function (data, SweetAlert) {
            var mgs = "";
            if (data.acqcM_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Quota Category Mapping?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgMasterQuota/activedeactiveQuota2", data).
                            then(function (promise) {

                                if (promise.message == "Mapped") {
                                    swal("You Can Not Deactive This Record Its Already Mapped");
                                } else if (promise.message == "Error") {
                                    swal("Something Went Wrong please contact administrator");
                                } else {
                                    if (promise.returnval === "true") {
                                        swal("Quota " + mgs + "Successfully");
                                    } else {
                                        swal("Quota " + mgs + "Successfully");
                                    }
                                }
                                $scope.cancel2();
                                $scope.clgQuotaload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }


        //---------------------------Qouta Grid
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        $scope.filterValue = function (obj) {

            if (obj.acQ_ActiveFlg == true) {
                $scope.active = "Active";
            }
            else {
                $scope.active = "InActive";
            }
            return (angular.lowercase(obj.acQ_QuotaName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.acQ_QuotaCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.acQ_QuotaInfo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
        //---------------------------Qouta Category Grid

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        $scope.filterValue01 = function (obj) {

            if (obj.acqC_ActiveFlg == true) {
                $scope.active = "Active";
            }
            else {
                $scope.active = "InActive";
            }
            return (angular.lowercase(obj.acqC_CategoryName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (angular.lowercase(obj.acqC_CategoryCode)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (JSON.stringify(obj.acqC_CategoryInfo)).indexOf($scope.searchValue1) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.searchValue1)) >= 0;
        }

        //-----------------QUOTA category Mapping- Grid--
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.search_box2 = function () {
            if ($scope.searchValue2 != "" || $scope.searchValue2 != null) {
                $scope.searc_button2 = false;
            }
            else {
                $scope.searc_button2 = true;
            }
        }
        $scope.filterValue02 = function (obj) {

            if (obj.acqcM_ActiveFlg == true) {
                $scope.active = "Active";
            }
            else {
                $scope.active = "InActive";
            }
            return (angular.lowercase(obj.acqC_CategoryName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.acQ_QuotaName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        }




    }

})();