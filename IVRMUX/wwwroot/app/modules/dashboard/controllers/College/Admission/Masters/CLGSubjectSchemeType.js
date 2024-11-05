(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLGSubjectSchemeType', CLGSubjectSchemeType);

    CLGSubjectSchemeType.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']

    function CLGSubjectSchemeType($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.submitted = false;
        $scope.submitted1 = false;


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.currentPage1 = 1;

        $scope.itemsPerPage2 = paginationformasters;
        $scope.currentPage2 = 1;


        $scope.clear1 = function () {
            $state.reload();
        };

        // Tab 1

        $scope.loadData = function () {
            apiService.getDATA("CLGSubjectSchemeType/").
                then(function (promise) {

                    if (promise.returnval == true) {
                        $scope.getshedetails = promise.schelist
                    }
                    else if (promise.returnval == false) {
                        swal('No record found');
                    }
                })
        };

        $scope.savemasterSchemetype = function () {
            $scope.search = "";
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ACST_Id": $scope.acsT_Id,
                    "ACST_SchmeType": $scope.scheme_type
                }

                apiService.create("CLGSubjectSchemeType/savetype", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exist");
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.edittype = function (itm) {
            $scope.acsT_Id = itm.acsT_Id;
            $scope.scheme_type = itm.acsT_SchmeType;
        }


        $scope.activedeactivebranch = function (data, SweetAlert) {
            $scope.search = "";
            var mgs = "";
            if (data.acsT_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Scheme type?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CLGSubjectSchemeType/activedeactivebatch", data).
                            then(function (promise) {
                                if (promise.message != "" && promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    swal("Failed to Activate/Deactivate Semester");
                                }
                                $scope.loadData();
                                //$state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        // Tab 2

        $scope.loadData1 = function () {
            apiService.getDATA("CLGSubjectSchemeType/getsubject").
                then(function (promise) {
                    if (promise.returnval == true) {
                        $scope.getsubdetails = promise.sublist;
                    }
                    else if (promise.returnval == false) {
                        swal('No record found');
                    }
                });
        };

        $scope.subschemename = function () {
            $scope.search1 = "";
            if ($scope.myForm2.$valid) {
                var data = {
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACSS_SchmeName": $scope.scheme_Name
                }

                apiService.create("CLGSubjectSchemeType/savename", data).then(function (promise) {

                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exist");
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.ACSS_Id = "";
                    $scope.scheme_Name = "";
                    $scope.loadData1();
                    $scope.myForm2.$setPristine();
                    $scope.myForm2.$setUntouched();
                    $scope.submitted1 = false;
                })
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.editname = function (itm1) {
            $scope.ACSS_Id = itm1.acsS_Id;
            $scope.scheme_Name = itm1.acsS_SchmeName;
        }

        $scope.activedeactivebranch1 = function (data, SweetAlert) {
            $scope.search1 = "";
            var mgs = "";
            if (data.acsT_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Scheme?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("CLGSubjectSchemeType/activedeactivebatch1", data).
                            then(function (promise) {

                                if (promise.message != "" && promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    swal("Failed to Activate/Deactivate Semester");
                                }

                                $scope.loadData1();
                                $scope.myForm2.$setPristine();
                                $scope.myForm2.$setUntouched();
                                // $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }


        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.sortBy1 = function (propertyName1) {
            $scope.reverse1 = ($scope.propertyName1 === propertyName1) ? !$scope.reverse1 : false;
            $scope.propertyName1 = propertyName1;
        };


        // Tab 3

        $scope.loadData2 = function () {
            var pageid = 2;
            apiService.getURI("CLGSubjectSchemeType/getcombination", pageid).then(function (promise) {

                if (promise.returnval == true) {
                    $scope.getdetails = promise.getdetails;
                }
                else if (promise.returnval == false) {
                    swal('No record found');
                }

            });
        };

        $scope.savecombination = function () {

            $scope.search21 = "";
            if ($scope.myForm3.$valid) {
                var data = {
                    "ADMCB_ID": $scope.ADMCB_ID,
                    "ADMCB_NAME": $scope.ADMCB_NAME

                }
                apiService.create("CLGSubjectSchemeType/savecombination", data).then(function (promise) {

                    if (promise.message == "Add") {
                        swal("Record Saved Successfully");
                    }
                    else if (promise.message == "AddF") {
                        swal("Failed To Save Record");
                    }
                    else if (promise.message == "Update") {
                        swal("Record Updated Successfully");
                    }
                    else if (promise.message == "UpdateF") {
                        swal("Failed To Update Record");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exist");
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    //$state.reload();
                    $scope.ADMCB_ID = "";
                    $scope.ADMCB_NAME = "";
                    $scope.loadData2();
                    $scope.myForm3.$setPristine();
                    $scope.myForm3.$setUntouched();
                    $scope.submitted3 = false;

                })
            }
            else {
                $scope.submitted3 = true;
            }
        };

        $scope.editnamecomb = function (itm1) {
            $scope.ADMCB_ID = itm1.admcB_ID;
            $scope.ADMCB_NAME = itm1.admcB_NAME;
        }

        $scope.activedeactivecomb = function (data, SweetAlert) {
            $scope.search21 = "";
            var mgs = "";
            if (data.admcB_Activeflag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Scheme?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("CLGSubjectSchemeType/activedeactivecomb", data).
                            then(function (promise) {

                                if (promise.message != "" && promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    swal("Failed to Activate/Deactivate Combination");
                                }

                                //$state.reload();
                                $scope.loadData2();
                                $scope.myForm3.$setPristine();
                                $scope.myForm3.$setUntouched();
                                // $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }


        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.sortBy2 = function (propertyName2) {
            $scope.reverse1 = ($scope.propertyName2 === propertyName2) ? !$scope.reverse2 : false;
            $scope.propertyName2 = propertyName2;
        };


        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.acsT_SchmeType)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }
    }

})();
