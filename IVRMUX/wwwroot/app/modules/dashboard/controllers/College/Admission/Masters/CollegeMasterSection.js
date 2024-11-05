
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeMasterSectionController', CollegeMasterSectionController)

    CollegeMasterSectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeMasterSectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'asmS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.MasterSectionCl = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("CollegeMasterSection/getalldetails", pageid).then(function (promise) {
                $scope.masterse = promise.getdetails;
                $scope.presentCountgrid = $scope.masterse.length;
            });
        };

        $scope.submitted = false;
        $scope.saveMasterdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ACMS_SectionName": $scope.ACMS_SectionName,
                    "ACMS_Order": $scope.ACMS_Order,
                    "ACMS_SectionCode": $scope.ACMS_SectionCode,
                    "ACMS_MaxCapacity": $scope.ACMS_MaxCapacity,
                    "ACMS_Id": $scope.ACMS_Id
                };

                apiService.create("CollegeMasterSection/saveMasterdata", data).then(function (promise) {

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
                    } else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    } else {
                        swal("Something Went Wrong Contact Administrator");
                    }
                    $state.reload();
                });
            }
        };

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.asmS_Id;
            var orgid = employee;
            apiService.create("CollegeMasterSection/Editdetails", employee).then(function (promise) {
                $scope.ACMS_Id = promise.editdetails[0].acmS_Id;
                $scope.ACMS_SectionName = promise.editdetails[0].acmS_SectionName;
                $scope.ACMS_SectionCode = promise.editdetails[0].acmS_SectionCode;
                $scope.ACMS_Order = promise.editdetails[0].acmS_Order;
                $scope.ACMS_MaxCapacity = promise.editdetails[0].acmS_MaxCapacity;
            });
        };

        $scope.DeletRecord = function (data, SweetAlertt) {

            var mgs = "";
            if (data.asmC_ActiveFlag == 0) {

                mgs = "Active";

            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to to " + mgs + " Section?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CollegeMasterSection/Deletedetails", data).then(function (promise) {
                            if (promise.message == "Mapped") {
                                swal("Record Already Mapped You Can Not Deactivate");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Section " + mgs + " Successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Failed To " + mgs + " Section");
                                    $state.reload();
                                }
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.cance = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };


        //section order
        $scope.getclassorder = function () {
            var pageid = 2;
            apiService.getURI("CollegeMasterSection/getalldetails", pageid).then(function (promosie) {
                if (promosie != null) {
                    $scope.newuser3 = promosie.getdetails1;
                }
                else {
                    swal("No Records Found");
                }
            });
        };

        $scope.resetLists = function () {
            $scope.configB = {
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
                for (var index in $scope.table1) {
                    $scope.newuser3[index].order = Number(index) + 1;
                }
            }
        };

        $scope.saveorder = function (newuser3) {
            var data = {
                "Master_Section_CLg_Temp": $scope.newuser3,
            };

            apiService.create("CollegeMasterSection/saveorder/", data).then(function (promoise) {
                if (promoise != null) {
                    if (promoise.returnval == true) {
                        swal("Records Updated Sucessfully");
                    }
                    else {
                        swal("Failed to Update the Record");
                    }
                }
                else {
                    swal("No Records Updated");
                }
                $scope.ACMS_SectionName = "";
                $scope.ACMS_Order = "";
                $scope.ACMS_SectionCode = "";
                $scope.ACMS_MaxCapacity = "";
                $scope.ACMS_Id = 0;
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.acmS_SectionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.acmS_MaxCapacity)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.acmS_SectionCode)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.acmS_Order)).indexOf($scope.searchValue) >= 0;
        }
    }
})();