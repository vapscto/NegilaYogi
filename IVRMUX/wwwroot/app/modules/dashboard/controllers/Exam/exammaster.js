(function () {
    'use strict';
    angular.module('app').controller('ExamMasterController', exammasterController)

    exammasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function exammasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.EME_FinalExamFlag = false;
        $scope.EME_ActiveFlag = true;
        $scope.submitted = false;
        $scope.submitted_PT = false;

        var paginationformasters = 10;        
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.BindData = function () {
            $scope.exname = "";
            $scope.excode = "";
            $scope.ivrsexname = "";
            $scope.examdescription = "";
            $scope.EME_FinalExamFlag = false;
            $scope.emE_ID = 0;

            apiService.getDATA("exammaster/Getdetails").then(function (promise) {
                $scope.gridOptions.data = promise.exammastername;
                $scope.grouptypeListOrder = promise.exammastername;
                $scope.final_exm_count = 0;
                angular.forEach(promise.exammastername, function (emd) {
                    if (emd.emE_FinalExamFlag === true) {
                        $scope.final_exm_count += 1;
                    }
                });
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'emE_ExamName', displayName: 'Exam Name' },
                { name: 'emE_IVRSExamName', displayName: 'IVRS Exam Name' },
                { name: 'emE_ExamDescription', displayName: 'Exam Description' },
                { name: 'emE_ExamCode', displayName: 'Exam Code' },
                { name: 'emE_ExamOrder', displayName: 'Exam Order', type: 'number' },
                {
                    name: 'emE_FinalExamFlag', displayName: 'Final-Exam Flag', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emE_FinalExamFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emE_FinalExamFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.emE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
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
                if (value.EME_ID !== 0) {
                    orderarray[key].emE_ExamOrder = key + 1;
                }
            });
            var data = {
                examDTO: orderarray
            };
            apiService.create("exammaster/validateordernumber", data).then(function (promise) {
                if (promise.retrunMsg !== "" && promise.retrunMsg !== undefined && promise.retrunMsg !== null) {
                    swal(promise.retrunMsg);
                }
                //$scope.cancel();
                //$scope.BindData();
              //  $("myModal")
                $('#myModal').modal('hide');
            });
        };

        // TO Save The Data
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EME_Id": $scope.EME_ID,
                    "EME_ExamCode": $scope.excode,
                    "EME_ExamName": $scope.exname,
                    "EME_IVRSExamName": $scope.ivrsexname,
                    "EME_ExamDescription": $scope.examdescription,
                    "EME_FinalExamFlag": $scope.EME_FinalExamFlag
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("exammaster/savedetails", data).then(function (promise) {
                    $scope.newuser = promise.exammastername;
                    if (promise.returnval === true) {
                        if (promise.emE_Id === 0 || promise.emE_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.emE_Id > 0) {
                            swal('Record updated successfully');
                        }

                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {

                        if (promise.emE_Id === 0 || promise.emE_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.emE_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.cancel();
                    $scope.BindData();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //to deactive the data
        $scope.deactive = function (deactiveRecord) {
            if (deactiveRecord.emE_FinalExamFlag === true && deactiveRecord.emE_ActiveFlag === true) {
                swal("You Can Not Deactivate Final Exam Record");
            }
            else {
                var mgs = "";
                var confirmmgs = "";
                if (deactiveRecord.emE_ActiveFlag === true) {
                    mgs = "Deactivate";
                    confirmmgs = "De-activated";

                }
                else {
                    mgs = "Activate";
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

                            var config = {
                                headers: {
                                    'Content-Type': 'application/json;'
                                }
                            };

                            apiService.create("exammaster/deactivate", deactiveRecord).then(function (promise) {
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
                                $scope.BindData();
                            });
                        }
                        else {
                            swal("Record " + mgs + " Cancelled");
                        }
                    });
            }
        };

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            var MEditId = EditRecord.emE_Id;
            apiService.getURI("exammaster/editdetails/", MEditId).then(function (promise) {
                $scope.EME_ID = promise.editlist[0].emE_Id;
                $scope.excode = promise.editlist[0].emE_ExamCode;
                $scope.exname = promise.editlist[0].emE_ExamName;
                $scope.ivrsexname = promise.editlist[0].emE_IVRSExamName;
                $scope.EME_FinalExamFlag = promise.editlist[0].emE_FinalExamFlag;
                $scope.examdescription = promise.editlist[0].emE_ExamDescription;
                $scope.EME_ActiveFlag = promise.editlist[0].emE_ActiveFlag;
                $scope.exorder = promise.editlist[0].emE_ExamOrder;
                if (promise.editlist[0].emE_FinalExamFlag === true) {
                    $scope.final_exm_count = 0;
                }
            });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.get_older = function () {
            $scope.BindData();
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].emE_ExamOrder = Number(index) + 1;

                }
            }
        };


        // Load Paper Type
        $scope.BindData_PaperType = function () {
            $scope.EMPATY_PaperTypeName = "";
            $scope.EMPATY_PaperTypeDescription = "";
            $scope.EMPATY_Color = "";
            $scope.EMPATY_Id = 0;
            var pageid = 2;
            apiService.getURI("exammaster/BindData_PaperType", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.GetExamPTLoadDetails = promise.getExamPTLoadDetails
                }

            });
        };

        $scope.Saveddata_PT = function () {
            if ($scope.myForm_PT.$valid) {

                if ($scope.EMPATY_PaperTypeDescription === undefined || $scope.EMPATY_PaperTypeDescription === null || $scope.EMPATY_PaperTypeDescription === "") {
                    $scope.EMPATY_PaperTypeDescription = "";
                }

                if ($scope.EMPATY_Color === undefined || $scope.EMPATY_Color === null || $scope.EMPATY_Color === "") {
                    $scope.EMPATY_Color = "";
                }
                var data = {
                    "EMPATY_Id": $scope.EMPATY_Id,
                    "EMPATY_PaperTypeName": $scope.EMPATY_PaperTypeName,
                    "EMPATY_Color": $scope.EMPATY_Color,
                    "EMPATY_PaperTypeDescription": $scope.EMPATY_PaperTypeDescription
                };
                apiService.create("exammaster/Saveddata_PT", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.retrunMsg === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.retrunMsg === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.retrunMsg === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.BindData_PaperType();
                    }
                });
            } else {
                $scope.submitted_PT = true;
            }
        };

        $scope.Editdata_PT = function (Edit_user) {
            var data = {
                "EMPATY_Id": Edit_user.empatY_Id
            };
            apiService.create("exammaster/Editdata_PT", data).then(function (promise) {
                if (promise !== null) {
                    $scope.EMPATY_PaperTypeName = promise.getExamPTEditDetails[0].empatY_PaperTypeName;
                    $scope.EMPATY_Color = promise.getExamPTEditDetails[0].empatY_Color;
                    $scope.EMPATY_PaperTypeDescription = promise.getExamPTEditDetails[0].empatY_PaperTypeDescription;
                    $scope.EMPATY_Id = promise.getExamPTEditDetails[0].empatY_Id;
                }
            });
        };

        $scope.DeactivateActivateMasterExam_PT = function (deactiveRecord_user) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord_user.empatY_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
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

                        var data = {
                            "EMPATY_Id": deactiveRecord_user.empatY_Id
                        };

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };

                        apiService.create("exammaster/DeactivateActivateMasterExam_PT", data).then(function (promise) {
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
                            $scope.BindData_PaperType();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cancel_PT = function () {
            $scope.BindData_PaperType();
        };

        $scope.interacted_PT = function () {
            return $scope.submitted_PT || field.$dirty;
        };
    }
})();