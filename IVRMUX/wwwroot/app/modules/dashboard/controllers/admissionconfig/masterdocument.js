
(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterdocumentController', masterdocumentController)

    masterdocumentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function masterdocumentController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        $scope.sortKey = 'amsmD_Id';
        $scope.sortReverse = true;

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.searchValue = "";
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
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




        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            apiService.getDATA("MasterDocument/Getdetails").then(function (promise) {
                if (promise.count == 0) {
                    //  swal("No Records Found.....!!");
                    return;
                }
                else {
                    $scope.gridviewDetails = promise.gridviewDetails;
                    $scope.presentCountgrid = $scope.gridviewDetails.length;
                }

            })
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //

        //delete record
        $scope.Deletedata = function (id, SweetAlert) {
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete The Record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("MasterDocument/DeleteData", id).
                            then(function (promise) {
                                if (promise.message == "Delete") {
                                    swal('You Can Not Delete This Record It Is Already Mapped With Student');
                                }
                                else {
                                    swal('Record Deleted Successfully');
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }

                });
        }



        //

        $scope.Deactive = function (id, SweetAlert) {
            // swal(id);
            var mgs = "";
            if (id.amsmD_ActiveFlag == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want to " + mgs + " Class?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("MasterDocument/DeleteData", id).
                            then(function (promise) {

                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.message == true) {
                                        swal('Record Activated Successfully');
                                    }
                                    else {
                                        swal('Record deActivated Successfully');
                                    }
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                    }

                });
        }



        $scope.Editdata = function (AMSMD_Id) {
            apiService.getURI("MasterDocument/GetSelectedRowDetails/", AMSMD_Id).
                then(function (promise) {
                    $scope.AMSMD_Id = promise.selectedRowDetails[0].amsmD_Id;
                    $scope.DocumentName = promise.selectedRowDetails[0].amsmD_DocumentName;
                    $scope.Description = promise.selectedRowDetails[0].amsmD_Description;
                    $scope.checkoruncheck = promise.selectedRowDetails[0].amsmD_FLAG;
                })
        };

        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMSMD_Id": $scope.AMSMD_Id,
                    "AMSMD_DocumentName": $scope.DocumentName,
                    "AMSMD_Description": $scope.Description,
                    "AMSMD_FLAG": $scope.checkoruncheck
                }

                apiService.create("MasterDocument/", data).then(function (promise) {
                    $scope.gridviewDetails = promise.gridviewDetails;
                    $scope.presentCountgrid = $scope.gridviewDetails.length;

                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }
                    else {
                        if (promise.returnVal == true) {
                            if (promise.messageupdate == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else {
                                swal('Record Saved Successfully');
                            }

                            $state.reload();
                        }
                        else if (promise.returnVal == false) {
                            if (promise.messageupdate == "Update") {
                                swal('Failed To Update Record');
                            }
                            else {
                                swal('Failed To Save Record');
                            }
                            $state.reload();
                        }
                    }

                })
            }

        };
        $scope.cancel = function () {
            $state.reload();
        }

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }


        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amsmD_DocumentName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amsmD_Description)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

    }

})();