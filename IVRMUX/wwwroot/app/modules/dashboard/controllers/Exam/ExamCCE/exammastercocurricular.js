
(function () {
    'use strict';
    angular.module('app').controller('ExamMasterCOCourricularController', ExamMasterCOCourricularController)

    ExamMasterCOCourricularController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamMasterCOCourricularController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.EME_FinalExamFlag = false;
        $scope.EME_ActiveFlag = true;
        $scope.BindData = function () {
            apiService.getDATA("exammasterCoCurricular/Getdetails").then(function (promise) {
                $scope.gridOptions.data = promise.exammasterCoCurricularname;
                $scope.exammasterCoCurricularname = promise.exammasterCoCurricularname;
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecC_CoCurricularName', displayName: 'CO-Curricular  Name' },
                { name: 'ecC_CoCurricularCode', displayName: 'CO-Curricular  Code' },
                { name: 'ecC_CoCurricularOrder', displayName: 'CO-Curricular  Order', type: 'number' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };

        $scope.init = function () {
            $scope.resetLists();
        };

        $scope.init();

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ecC_Id != 0) {
                    orderarray[key].ecC_CoCurricularOrder = key + 1;
                }
            });

            var data = {
                examCoCurricularDTO: orderarray,
            };

            apiService.create("exammasterCoCurricular/validateordernumber", data).then(function (promise) {
                if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                    swal(promise.retrunMsg);
                }
                $scope.cancel();
            });
        };

        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecC_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }else {
                mgs = "Activate";
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
                        apiService.create("exammasterCoCurricular/deactivate", deactiveRecord).then(function (promise) {
                            if (promise.already_cnt === true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.cancel();                            
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
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

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            var MEditId = EditRecord.ecC_Id;
            apiService.getURI("exammasterCoCurricular/editdetails/", MEditId).then(function (promise) {
                $scope.c_Id = promise.exammCoCurricularname[0].ecC_Id;
                $scope.c_Code = promise.exammCoCurricularname[0].ecC_CoCurricularCode;
                $scope.c_Name = promise.exammCoCurricularname[0].ecC_CoCurricularName;
                $scope.c_ActiveFlag = promise.exammCoCurricularname[0].ecC_ActiveFlag;
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ECC_Id": $scope.c_Id,
                    "ECC_CoCurricularCode": $scope.c_Code,
                    "ECC_CoCurricularName": $scope.c_Name
                };

                apiService.create("exammasterCoCurricular/savedetails", data).then(function (promise) {
                    $scope.newuser = promise.exammastername;
                    if (promise.returnval === true) {
                        if (promise.ecC_Id === 0 || promise.ecC_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ecC_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ecC_Id == 0 || promise.ecC_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ecC_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.cancel();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.get_older = function () {
            $scope.BindData();
        };
    }
})();