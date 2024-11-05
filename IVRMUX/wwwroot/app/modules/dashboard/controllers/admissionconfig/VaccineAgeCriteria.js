(function () {
    'use strict';
    angular.module('app').controller('VaccineAgeCriteriaDetailsController', VaccineAgeCriteriaDetailsController)

    VaccineAgeCriteriaDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter', 'Excel', '$timeout']
    function VaccineAgeCriteriaDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter, Excel, $timeout) {

        $scope.edit = false;
        $scope.excel_flag = true;
        $scope.excel_flag = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }

        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.OnLoadVaccineAgeCriteriaDetails = function () {
            var pageid = 2;
            apiService.getURI("VaccineAgeCriteria/OnLoadVaccineAgeCriteriaDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.GetAgeCriteriaDetails = promise.getAgeCriteriaDetails;
                }
            });
        };

        $scope.SaveVaccineAgeDetails = function () {
            $scope.submitted = false;
            $scope.tempdetails = [];

            if ($scope.myform.$valid) {

                angular.forEach($scope.vaccinedetails, function (dd) {
                    if (dd.ASVACD_VaccineName !== "") {
                        $scope.tempdetails.push({ ASVACD_Id: dd.ASVACD_Id, ASVACD_VaccineName: dd.ASVACD_VaccineName, ASVACD_VaccineType: dd.ASVACD_VaccineType });
                    }
                });

                var data = {
                    "ASVAC_Id": $scope.ASVAC_Id,
                    "ASVAC_AgeStartNo": $scope.ASVAC_AgeStartNo,
                    "ASVAC_AgeEndNo": $scope.ASVAC_AgeEndNo,
                    "VaccineAgeCriteriaDetails": $scope.tempdetails
                };

                apiService.create("VaccineAgeCriteria/SaveVaccineAgeDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnValue === true) {
                            swal("Record Saved / Updated Successfully");
                            $scope.cleardata();
                        } else {
                            swal("Failed To Save / Update Record");
                            $scope.cleardata();
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.EditVaccineAgeDetails = function (obj) {
            $scope.vaccinedetails = [];

            var data = {
                "ASVAC_Id": obj.asvaC_Id
            };
            apiService.create("VaccineAgeCriteria/EditVaccineAgeDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetEditAgeCriteriaDetails = promise.getEditAgeCriteriaDetails;
                    $scope.GetEditViewVaccineDetails = promise.getEditViewVaccineDetails;

                    $scope.ASVAC_Id = $scope.GetEditAgeCriteriaDetails[0].asvaC_Id;
                    $scope.ASVAC_AgeStartNo = $scope.GetEditAgeCriteriaDetails[0].asvaC_AgeStartNo;
                    $scope.ASVAC_AgeEndNo = $scope.GetEditAgeCriteriaDetails[0].asvaC_AgeEndNo;

                    angular.forEach($scope.GetEditViewVaccineDetails, function (dd) {
                        $scope.vaccinedetails.push({ ASVACD_Id: dd.asvacD_Id, ASVACD_VaccineName: dd.asvacD_VaccineName, ASVACD_VaccineType: dd.asvacD_VaccineType });
                    });
                }
            });
        };

        $scope.ActiveDeactiveVaccineAgeDetails = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asvaC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("VaccineAgeCriteria/ActiveDeactiveVaccineAgeDetails/", user).then(function (promise) {
                            if (promise.returnValue == true) {
                                swal("Record" + " " + confirmmgs + " " + "Successfully");
                                $scope.cleardata();
                            }
                            else {
                                swal("Record" + " " + confirmmgs + " " + " Successfully");
                                $scope.cleardata();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }                     
                });
        };

        $scope.OnClickViewDetails = function (obj) {

            $scope.fromage = obj.asvaC_AgeStartNo;
            $scope.toage = obj.asvaC_AgeEndNo;

            var data = {
                "ASVAC_Id": obj.asvaC_Id
            };
            apiService.create("VaccineAgeCriteria/OnClickViewDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetViewVaccineDetails = promise.getViewVaccineDetails;
                    if ($scope.GetViewVaccineDetails !== null && $scope.GetViewVaccineDetails.length > 0) {
                        $('#popup11').modal('show');
                    }
                }
            });
        };

        $scope.ActiveDeactiveVaccineDetails = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.asvacD_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("VaccineAgeCriteria/ActiveDeactiveVaccineDetails/", user).then(function (promise) {
                            if (promise.returnValue == true) {
                                swal("Record" + " " + confirmmgs + " " + "Successfully");                                 
                            }
                            else {
                                swal("Record" + " " + confirmmgs + " " + " Successfully");                                
                            }

                            $scope.GetViewVaccineDetails = promise.getViewVaccineDetails;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.OnChangeVaccineName = function (obj, index) {

            angular.forEach($scope.vaccinedetails, function (dd, i) {
                if (i != Number(index)) {
                    if (dd.ASVACD_VaccineName === obj.ASVACD_VaccineName) {
                        swal("Vaccine Name Already Enter");
                        obj.ASVACD_VaccineName = "";
                    }
                }
            });
        };

        $scope.vaccinedetails = [{ id: 'Student1' }];
        $scope.addstudent = function () {
            var newItemNo = $scope.vaccinedetails.length + 1;
            if (newItemNo <= 30) {
                $scope.vaccinedetails.push({ 'id': 'Student' + newItemNo });
            }
        };

        $scope.removestudent = function (index) {
            var newItemNo = $scope.vaccinedetails.length - 1;
            $scope.vaccinedetails.splice(index, 1);
            if ($scope.vaccinedetails.length === 0) {
                //data
            }
        };

        $scope.cleardata = function () {
            $scope.submitted = false;
            $scope.ASVAC_AgeStartNo = "";
            $scope.ASVAC_AgeEndNo = "";
            $scope.ASVAC_Id = 0;
            $scope.vaccinedetails = [];
            $scope.vaccinedetails = [{ id: 'Student1' }];
            $scope.OnLoadVaccineAgeCriteriaDetails();
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };
    }
})();