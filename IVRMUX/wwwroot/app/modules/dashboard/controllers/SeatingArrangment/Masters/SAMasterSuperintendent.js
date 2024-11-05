(function () {
    'use strict';
    angular.module('app').controller('SuperintendentController', SuperintendentController)
    SuperintendentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function SuperintendentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {      

        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.search = "";
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.loaddata = function () {
           var data = 2;
            apiService.getURI("SAMasterSuperintendent/load_sup", data).then(function (promise) {
                if (promise !== null) {
                    $scope.yearlst = promise.yearlst;
                    $scope.examlist = promise.examlist;
                    $scope.university_examlist = promise.university_examlist;
                    $scope.superintendentgridlist = promise.superintendentgridlist;
                }
            });
        };

        $scope.OnChangeYear = function () {
            angular.forEach($scope.yearlst, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.mindate = new Date(dd.asmaY_From_Date);
                    $scope.maxdate = new Date(dd.asmaY_To_Date);

                }
            });
        };

        $scope.OnChangeFromDate = function () {
            $scope.ESASINTDNT_ToDate = null;
        };
        
        $scope.Save_sup = function () {
            if ($scope.myForm.$valid) {
                var fmdate = $scope.ESASINTDNT_FromDate === null ? "" : $filter('date')($scope.ESASINTDNT_FromDate, "yyyy-MM-dd");
                var tdate = $scope.ESASINTDNT_ToDate === null ? "" : $filter('date')($scope.ESASINTDNT_ToDate, "yyyy-MM-dd");
                var data = {
                    "ESASINTDNT_Id": $scope.ESASINTDNT_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESASINTDNT_ChiefSupName": $scope.ESASINTDNT_ChiefSupName,
                    "ESASINTDNT_ChiefSupCollege": $scope.ESASINTDNT_ChiefSupCollege,
                    "ESASINTDNT_DeptChiefSupName": $scope.ESASINTDNT_DeptChiefSupName,
                    "ESASINTDNT_DeptChiefSupCollege": $scope.ESASINTDNT_DeptChiefSupCollege,
                    "ESASINTDNT_FromDate": fmdate,
                    "ESASINTDNT_ToDate": tdate
                };
                apiService.create("SAMasterSuperintendent/Save_sup", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.message === "Update") {
                            swal("Record Update Successfully");
                        }
                        else if (promise.message === "Error") {
                            swal("Failed To Save/Update");
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.Edit_sup = function (obj) {
            
            var data = {
                "ESASINTDNT_Id": obj.esasintdnT_Id
            };
            apiService.create("SAMasterSuperintendent/Edit_sup", data).then(function (promise) {
                if (promise.edit_suplist !== null && promise.edit_suplist.length > 0) {

                    angular.forEach($scope.yearlst, function (dd) {
                        if (dd.asmaY_Id === parseInt(promise.edit_suplist[0].asmaY_Id)) {
                            $scope.mindate = new Date(dd.asmaY_From_Date);
                            $scope.maxdate = new Date(dd.asmaY_To_Date);
                        }
                    });


                    $scope.ASMAY_Id = promise.edit_suplist[0].asmaY_Id;
                    $scope.EME_Id = promise.edit_suplist[0].emE_Id;
                    $scope.ESAUE_Id = promise.edit_suplist[0].esauE_Id;
                    $scope.ESASINTDNT_ChiefSupName = promise.edit_suplist[0].esasintdnT_ChiefSupName;
                    $scope.ESASINTDNT_ChiefSupCollege = promise.edit_suplist[0].esasintdnT_ChiefSupCollege;
                    $scope.ESASINTDNT_DeptChiefSupName = promise.edit_suplist[0].esasintdnT_DeptChiefSupName;
                    $scope.ESASINTDNT_DeptChiefSupCollege = promise.edit_suplist[0].esasintdnT_DeptChiefSupCollege;
                    $scope.ESASINTDNT_FromDate = new Date(promise.edit_suplist[0].esasintdnT_FromDate);
                    $scope.ESASINTDNT_ToDate = new Date(promise.edit_suplist[0].esasintdnT_ToDate);
                    $scope.ESASINTDNT_Id = promise.edit_suplist[0].esasintdnT_Id;
                    $scope.yearlst = promise.yearlst;
                    $scope.examlist = promise.examlist;
                    $scope.university_examlist = promise.university_examlist;
                }
            });
        };

        $scope.ActiveDeactive_sup = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.esasintdnT_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESASINTDNT_Id": user.esasintdnT_Id,
                "ESASINTDNT_ActiveFlg": user.esasintdnT_ActiveFlg
            };

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
                        apiService.create("SAMasterSuperintendent/ActiveDeactive_sup", data).then(function (promise) {
                            if (promise.message === 'true') {
                                swal("Record Activated Successfully");
                            }
                            else if (promise.message === 'false') {
                                swal("Record De-Activated Successfully");
                            } else if (promise.message === 'error') {
                                swal("Operation Failed..!!");
                            }
                            $state.reload();
                        });
                       
                    }
                    
                });
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();


