
(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRSMasterLanguagesController', IVRSMasterLanguagesController)

    IVRSMasterLanguagesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function IVRSMasterLanguagesController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.BindData = function () {
            apiService.getDATA("IVRSMasterLanguages/getalldetails").
                then(function (promise) {
                    $scope.institutiondropdown = promise.institute;
                    $scope.gridOptions.data = promise.maindata;
                    $scope.languagesArray = [];
                    $scope.languagesArray.push({ id: 1, name: 'English' })
                    $scope.languagesArray.push({ id: 2, name: 'Kannada' })
                    $scope.languagesArray.push({ id: 3, name: 'Tamil'})
                    $scope.languagesArray.push({ id: 4, name: 'Marathi' })
                    $scope.languagesArray.push({ id: 5, name: 'Hindhi' })
                    $scope.languagesArray.push({ id: 6, name: 'Bengali' })
                    $scope.languaglist = $scope.languagesArray;
                });
        }    //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'imlA_VirtualNo', displayName: 'VirtualNo' },
                { name: 'imlA_SchoolName', displayName: 'SchoolName' },
                { name: 'imlA_Language', displayName: 'Language' },
                { name: 'imlA_LanguageOrder', displayName: 'LanguageOrder' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.imlA_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.imlA_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };

        //TO  academics
        $scope.get_academics = function (iivrsC_Id) {
            angular.forEach($scope.institutiondropdown, function (role) {
                if (Number(iivrsC_Id) === role.iivrsC_Id) {
                    $scope.imlA_VirtualNo = role.iivrsC_VirtualNo;
                    $scope.imlA_SchoolName = role.iivrsC_SchoolName;
                    $scope.imlA_SchoolURL = role.iivrsC_URL;
                    $scope.mI_Id = role.iivrsC_MI_Id
                }
            });
        }
        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {   
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "IMLA_Id": $scope.imlA_Id,
                    "IMLA_VirtualNo": $scope.imlA_VirtualNo,
                    "IMLA_SchoolURL": $scope.imlA_SchoolURL,
                    "IMLA_SchoolName": $scope.imlA_SchoolName,
                    "IMLA_Language": $scope.name,
                    "IMLA_LanguageOrder": $scope.imlA_LanguageOrder                 
                }
                apiService.create("IVRSMasterLanguages/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Saved Successfully');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnduplicatestatus === "Duplicate") {
                            swal('Record Already Exist');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnduplicatestatus === "Order Duplicate") {
                            swal('Order Already Exist with same Instuite and diferent Language');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false) {
                            swal('Record Not Saved');
                            $scope.cleardata();
                            $scope.BindData();
                        }
                    })
            }
        };

        //TO clear  data
        $scope.cleardata = function () {
            $scope.iivrsC_Id = "";
            $scope.imlA_Id = 0;
            $scope.mI_Id = "";
            $scope.imlA_VirtualNo = "";
            $scope.imlA_SchoolURL = "";
            $scope.imlA_SchoolName = "";
            $scope.name = "";
            $scope.imlA_LanguageOrder = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.imlA_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("IVRSMasterLanguages/getdetails_page", pageid).
                then(function (promise) {
                    $scope.imlA_Id = promise.maindata_grid[0].imlA_Id;
                    $scope.mI_Id = promise.maindata_grid[0].mI_Id;
                    $scope.imlA_VirtualNo = promise.maindata_grid[0].imlA_VirtualNo;
                    $scope.imlA_SchoolURL = promise.maindata_grid[0].imlA_SchoolURL;
                    $scope.imlA_SchoolName = promise.maindata_grid[0].imlA_SchoolName;
                    angular.forEach($scope.institutiondropdown, function (role) {
                        if ($scope.imlA_SchoolName === role.iivrsC_SchoolName) {
                            $scope.iivrsC_Id = role.iivrsC_Id;
                        }
                    });
                    $scope.name = promise.maindata_grid[0].imlA_Language;
                    $scope.imlA_LanguageOrder = promise.maindata_grid[0].imlA_LanguageOrder;
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
            if (employee.imlA_ActiveFlg === true) {
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

                        apiService.create("IVRSMasterLanguages/deactivate", employee).
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