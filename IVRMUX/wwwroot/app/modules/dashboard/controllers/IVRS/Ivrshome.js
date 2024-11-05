
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IvrshomeController', IvrshomeController)

    IvrshomeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function IvrshomeController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {


        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "IIVRSC_Id": $scope.IIVRSC_Id,
                    "IIVRSC_MI_Id": $scope.mI_Id,
                    "ivrM_Month_Name": $scope.ivrM_Month_Name,
                    "IIVRSC_VirtualNo": $scope.IIVRSC_VirtualNo,
                    "IIVRSC_CallCharge": $scope.IIVRSC_CallCharge,
                    "ASMAY_ID": $scope.asmaY_Id,
                    "IIVRSC_PerMonthCall": $scope.IIVRSC_PerMonthCall,
                    "IIVRSC_VFORTTSFlg": $scope.IIVRSC_VFORTTSFlg,
                    "IIVRSC_URL": $scope.IIVRSC_URL,
                    "IVRS_MOBILE_URL": $scope.IVRS_MOBILE_URL,
                    "IVRS_UPDATE_URL": $scope.IVRS_UPDATE_URL
                }
                apiService.create("Ivrshome/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Saved Successfully');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false) {
                            swal('Record Not Saved');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnduplicatestatus === "Duplicate") {
                            swal('Record Already Exist');
                            $scope.BindData();
                        }
                    })
            }

        };

        //TO clear  data
        $scope.cleardata = function () {
            $scope.IIVRSC_Id = 0;
            $scope.mI_Id = "";
            $scope.ivrM_Month_Name = "";
            $scope.IIVRSC_VirtualNo = "";
            $scope.asmaY_Id = "";
            $scope.IIVRSC_PerMonthCall = "";
            $scope.IIVRSC_VFORTTSFlg = "";
            $scope.IIVRSC_URL = "";
            $scope.IVRS_MOBILE_URL = "";
            $scope.IVRS_UPDATE_URL = "";
            $scope.IIVRSC_CallCharge = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        //TO  academics
        $scope.get_academics = function () {
            if ($scope.mI_Id !== 0 || $scope.mI_Id === undefined) {
                apiService.getURI("Ivrshome/getdetails", $scope.mI_Id).
                    then(function (promise) {
                        $scope.academic = promise.yearlist;

                        $scope.ASMAY_Id = promise.asmaY_Id;



                    })
            }

        }



        $scope.BindData = function () {
            apiService.getDATA("Ivrshome/getalldetails").
                then(function (promise) {
                    $scope.institutiondropdown = promise.institute;
                    $scope.monthdropdown = promise.monthdropdown;
                    $scope.gridOptions.data = promise.maindata;


                });
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'iivrsC_VirtualNo', displayName: 'VirtualNo' },
                { name: 'iivrsC_SchoolName', displayName: 'SchoolName' },
                { name: 'iivrsC_PerMonthCall', displayName: 'PerMonthCall' },
                { name: 'iivrsC_CallCharge', displayName: 'CallCharge' },
                { name: 'iivrsC_VFORTTSFlg', displayName: 'Type' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.iivrsC_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.iivrsC_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.iivrsC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Ivrshome/getdetails_page", pageid).
                then(function (promise) {
                    $scope.IIVRSC_Id = promise.maindata_grid[0].iivrsC_Id;
                    $scope.mI_Id = promise.maindata_grid[0].iivrsC_MI_Id;
                    $scope.ivrM_Month_Name = promise.ivrM_Month_Name;
                    $scope.IIVRSC_VirtualNo = promise.maindata_grid[0].iivrsC_VirtualNo;
                    $scope.IIVRSC_CallCharge = promise.maindata_grid[0].iivrsC_CallCharge;
                    $scope.IIVRSC_PerMonthCall = promise.maindata_grid[0].iivrsC_PerMonthCall;
                    $scope.IIVRSC_VFORTTSFlg = promise.maindata_grid[0].iivrsC_VFORTTSFlg;
                    $scope.IIVRSC_URL = promise.maindata_grid[0].iivrsC_URL;
                    $scope.IVRS_MOBILE_URL = promise.maindata_grid[0].ivrS_MOBILE_URL;
                    $scope.IVRS_UPDATE_URL = promise.maindata_grid[0].ivrS_UPDATE_URL;
                    $scope.get_academics();
                    for (var i = 0; i < $scope.academic.length; i++) {
                        if ($scope.academic[i].asmaY_Id === promise.asmaY_ID) {
                            $scope.asmaY_Id = promise.asmaY_ID;
                        }
                    }

                })
        }
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.iivrsC_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("Ivrshome/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }









    }

})();