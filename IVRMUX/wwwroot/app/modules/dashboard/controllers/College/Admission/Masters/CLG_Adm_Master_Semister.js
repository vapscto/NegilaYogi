(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLG_Adm_Master_Semister', CLG_Adm_Master_Semister);

    CLG_Adm_Master_Semister.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']

    function CLG_Adm_Master_Semister($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var id = 1;


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;


        $scope.clgload = function () {

            apiService.getDATA("CLGMasterSemister/").
                then(function (promise) {

                    if (promise.returnval == true) {
                        $scope.getdetails = promise.semlist;
                        $scope.grouptypeListOrder = promise.semlistorder;
                    }
                    else if (promise.returnval == false) {
                        swal('No record found');
                    }

                });
        }

        $scope.submitted = false;

        $scope.savesem = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMSE_SEMName": $scope.AMSE_SEMName,
                    "AMSE_SEMCode": $scope.AMSE_SEMCode,
                    "AMSE_SEMInfo": $scope.AMSE_SEMInfo,
                    "AMSE_SEMTypeFlag": $scope.radioption,
                    "AMSE_SEMOrder": $scope.AMSE_SEMOrder,
                    "AMSE_Year": $scope.AMSE_Year,
                    "AMSE_EvenOdd": $scope.radiodd
                }
                apiService.create("CLGMasterSemister/savesem", data).then(function (promise) {

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



        $scope.edit = function (data) {

            apiService.create("CLGMasterSemister/editsem", data).then(function (promise) {

                $scope.AMSE_Id = promise.editdetails[0].amsE_Id;
                $scope.AMSE_SEMName = promise.editdetails[0].amsE_SEMName;
                $scope.AMSE_SEMCode = promise.editdetails[0].amsE_SEMCode;
                $scope.AMSE_SEMInfo = promise.editdetails[0].amsE_SEMInfo;
                $scope.radioption = promise.editdetails[0].amsE_SEMTypeFlag;
                $scope.AMSE_SEMOrder = promise.editdetails[0].amsE_SEMOrder;
                $scope.AMSE_Year = promise.editdetails[0].amsE_Year;
                $scope.radiodd = promise.editdetails[0].amsE_EvenOdd;

            })

        }

        $scope.activedeactivebranch = function (data, SweetAlert) {
            var mgs = "";
            if (data.amsE_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Semister?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("CLGMasterSemister/activedeactivesem", data).
                            then(function (promise) {

                                if (promise.message == "Mapped") {
                                    swal("Record Already Mapped You Can Not Deactivate");
                                } else {
                                    if (promise.returnval == true) {
                                        swal("Record Successfully " + mgs)
                                    }
                                    else {
                                        swal("Failed To " + mgs + "Record")
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

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cleardata = function () {
            $state.reload();
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.search = "";
        //$scope.filtervalue = function (obj) {

        //    if (obj.amsE_ActiveFlg == true) {
        //        $scope.active = "Active";
        //    }
        //    else {
        //        $scope.active = "InActive";
        //    }
        //    return (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
        //        (angular.lowercase(obj.amsE_SEMCode)).indexOf(angular.lowercase($scope.search)) >= 0 ||
        //        (JSON.stringify(obj.amsE_SEMOrder)).indexOf($scope.search) >= 0 ||
        //    (JSON.stringify(obj.amsE_SEMInfo)).indexOf($scope.search) >= 0 ||
        //        (JSON.stringify(obj.amsE_SEMTypeFlag)).indexOf($scope.search) >= 0 ||
        //        (JSON.stringify(obj.amsE_EvenOdd)).indexOf($scope.search) >= 0 ||
        //        (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.search)) >= 0;
        //}

        $scope.filtervalue = function (obj) {
            if (obj.amsE_ActiveFlg == true) {
                $scope.active = "Active";
            }
            else {
                $scope.active = "InActive";
            }
            return (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsE_SEMCode)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.amsE_SEMOrder)).indexOf($scope.search) >= 0 ||
                (angular.lowercase(JSON.stringify(obj.amsE_SEMInfo))).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(JSON.stringify(obj.amsE_SEMTypeFlag))).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.amsE_EvenOdd)).indexOf($scope.search) >= 0 ||
                (angular.lowercase($scope.active)).indexOf(angular.lowercase($scope.search)) >= 0;
        }




        //Set Order

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };


        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].amsE_SEMOrder = Number(index) + 1;

                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                sem_temp: orderarray,
            }
            apiService.create("CLGMasterSemister/getOrder", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Semester Order Updated Successfully");
                    }
                    else {
                        swal(" Failed To Update Semester Order");
                    }
                    $state.reload();
                });

        }
    }

})();
