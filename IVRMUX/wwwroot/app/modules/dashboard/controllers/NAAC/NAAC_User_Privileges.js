(function () {
    'use strict';
    angular.module('app').controller('NaacuserprivilegesController', NaacuserprivilegesController)
    NaacuserprivilegesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function NaacuserprivilegesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.NAACUPRI_AddFlg = false;
        $scope.NAACUPRI_UpdateFlg = false;
        $scope.NAACUPRI_DeleteFlg = false;

        $scope.NAACUPRI_TrustUserFlag = false;
        $scope.NAACUPRI_IQACInchargeFlg = false;
        $scope.NAACUPRI_ConsultantFlg = false;
        $scope.NAACUPRI_FinalFlg = false;
        $scope.NAACUPRI_ApproverFlg = false;

        $scope.NAACUPRI_Order = "1";

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("NAAC_User_Privileges/Getdetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.getemployee = promise.getemployee;
                    $scope.getsavedata = promise.getsavedata;
                    $scope.gridOptions.data = $scope.getsavedata;
                }
            });
        };

        $scope.onchangeemployee = function (HRME_Id) {
            $scope.getcriteria = [];
            $scope.getinstitution = [];
            $scope.tempmid = [];
            $scope.HRME_Id_New = "";
            $scope.HRME_Id_New = HRME_Id.HRME_Id.hrmE_Id;
            var data = {
                "HRME_Id": HRME_Id.HRME_Id.hrmE_Id
            };

            apiService.create("NAAC_User_Privileges/onchangeemployee", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getcriteria !== null && promise.getcriteria.length > 0) {
                        $scope.getcriteria = promise.getcriteria;
                        if (promise.getinstitution !== null && promise.getinstitution.length > 0) {
                            $scope.getinstitution = promise.getinstitution;
                        } else {
                            swal("No Institution Mapped For This Employee");
                        }

                        $scope.getsavedinstitution = promise.getsavedinstitution;

                        if ($scope.getsavedinstitution !== null && $scope.getsavedinstitution.length > 0) {
                            angular.forEach($scope.getinstitution, function (d) {
                                angular.forEach($scope.getsavedinstitution, function (dd) {
                                    if (d.mI_Id === dd.mI_Id) {
                                        d.checked = true;
                                    }
                                });
                            });
                        }


                        //=================================================================

                        $scope.getparentidzero = promise.getcriteria;
                        $scope.getalldata = promise.getalldata;

                        $scope.array = [];

                        angular.forEach($scope.getparentidzero, function (dd) {
                            $scope.temparra1d = [];
                            angular.forEach($scope.getalldata, function (ddd) {
                                if (ddd.naacsL_ParentId === dd.naacsL_Id) {
                                    $scope.temparra1d.push(ddd);
                                }
                            })
                            //  console.log($scope.temparra1d);

                            // console.log('==============================================');

                            $scope.temparray2d = [];
                            angular.forEach($scope.temparra1d, function (levelii) {
                                $scope.temparray2d = [];
                                angular.forEach($scope.getalldata, function (ddd) {
                                    if (ddd.naacsL_ParentId === levelii.naacsL_Id) {
                                        $scope.temparray2d.push(ddd);
                                    }
                                });

                                levelii.temparray2 = $scope.temparray2d;
                                //console.log($scope.temparra1d);
                            });


                            $scope.array.push({ NAACSL_Id: dd.naacsL_Id, NAACSL_SLNo: dd.naacsL_SLNo, NAACSL_SLNoDescription: dd.naacsL_SLNoDescription, temparra1: $scope.temparra1d });

                            console.log($scope.array);

                        })


                        //==================================================================


                        if (promise.getusersaveddetails !== null && promise.getusersaveddetails.length > 0) {
                            $scope.getusersaveddetails = promise.getusersaveddetails;
                            $scope.NAACUPRI_AddFlg = $scope.getusersaveddetails[0].naacuprI_AddFlg;
                            $scope.NAACUPRI_UpdateFlg = $scope.getusersaveddetails[0].naacuprI_UpdateFlg;
                            $scope.NAACUPRI_DeleteFlg = $scope.getusersaveddetails[0].naacuprI_DeleteFlg;
                            $scope.NAACUPRI_TrustUserFlag = $scope.getusersaveddetails[0].naacuprI_TrustUserFlag;
                            $scope.NAACUPRI_IQACInchargeFlg = $scope.getusersaveddetails[0].naacuprI_IQACInchargeFlg;
                            $scope.NAACUPRI_ConsultantFlg = $scope.getusersaveddetails[0].naacuprI_ConsultantFlg;
                            $scope.NAACUPRI_FinalFlg = $scope.getusersaveddetails[0].naacuprI_FinalFlg;
                            $scope.NAACUPRI_ApproverFlg = $scope.getusersaveddetails[0].naacuprI_ApproverFlg;
                            $scope.NAACUPRI_Order = $scope.getusersaveddetails[0].naacuprI_Order;
                        }

                    } else {
                        swal("No Criteria Found");
                    }
                }
            });
        };

        $scope.onchangecriteria = function (HRME_Id) {
            var data = {
                "HRME_Id": $scope.HRME_Id_New,
                "NAACSL_Id": $scope.NAACSL_Id
            };

            apiService.create("NAAC_User_Privileges/onchangecriteria", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsavedinstitution = promise.getsavedinstitution;

                    if ($scope.getsavedinstitution !== null && $scope.getsavedinstitution.length > 0) {
                        angular.forEach($scope.getinstitution, function (d) {
                            angular.forEach($scope.getsavedinstitution, function (dd) {
                                if (d.mI_Id === dd.mI_Id) {
                                    d.checked = true;
                                }
                            });
                        });
                    }
                }
            });
        };

        $state.obj = {};
        $scope.savedata = function (HRME_Id) {
            if ($scope.myForm.$valid) {

                $scope.mainheaderlist = [];
                $scope.headerlist = [];
                $scope.subheaderlist = [];
                $scope.tempmid = [];

                angular.forEach($scope.getinstitution, function (dd) {
                    if (dd.checked) {
                        $scope.tempmid.push({ MI_Id: dd.mI_Id });
                    }
                });

                if ($scope.NAACUPRI_AddFlg === false && $scope.NAACUPRI_UpdateFlg === false && $scope.NAACUPRI_DeleteFlg === false) {
                    swal("Select Atleast Any One Check Box To Save The Details");
                }
                else {
                    angular.forEach($scope.array, function (mhead) {
                        if (mhead.checkedgrplst_s == true) {
                            $scope.mainheaderlist.push(mhead);
                        }
                        angular.forEach(mhead.temparra1, function (head) {
                            if (head.checkedheadlst_s == true) {
                                $scope.headerlist.push(head);
                            }
                            angular.forEach(head.temparray2, function (shead) {
                                if (shead.checkedinstallmentlst_s == true) {
                                    $scope.subheaderlist.push(shead);
                                }
                            })
                        })
                    })
                    
                    var data = {
                        "NAACUPRI_Id": $scope.NAACUPRI_Id,
                        "HRME_Id": $scope.HRME_Id_New,
                        "temp_miid": $scope.tempmid,
                        "NAACSL_Id": $scope.NAACSL_Id,
                        "NAACUPRI_AddFlg": $scope.NAACUPRI_AddFlg,
                        "NAACUPRI_UpdateFlg": $scope.NAACUPRI_UpdateFlg,
                        "NAACUPRI_DeleteFlg": $scope.NAACUPRI_DeleteFlg,
                        "NAACUPRI_TrustUserFlag": $scope.NAACUPRI_TrustUserFlag,
                        "NAACUPRI_IQACInchargeFlg": $scope.NAACUPRI_IQACInchargeFlg,
                        "NAACUPRI_ConsultantFlg": $scope.NAACUPRI_ConsultantFlg,
                        "NAACUPRI_FinalFlg": $scope.NAACUPRI_FinalFlg,
                        "NAACUPRI_ApproverFlg": $scope.NAACUPRI_ApproverFlg,
                        "NAACUPRI_Order": $scope.NAACUPRI_Order,

                        mainheaderlist: $scope.mainheaderlist,
                        headerlist: $scope.headerlist,
                        subheaderlist: $scope.subheaderlist,

                    };


                    apiService.create("NAAC_User_Privileges/savedata", data).then(function (promise) {
                        if (promise !== null) {
                            if (promise.returnval === true) {
                                swal("Record Saved/Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save/Update Record");
                            } else {
                                swal("Something Went Wrong Contant Administrator");
                            }
                            $state.reload();
                        }
                    });
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmE_EmployeeFirstName', width: '15%', displayName: 'Employee Name' },
                { name: 'naacuprI_Order', width: '10%', displayName: 'Order' },
                {
                    name: 'naacuprI_AddFlg', width: '9%', displayName: 'Add', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_AddFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_AddFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'naacuprI_UpdateFlg', width: '9%', displayName: 'Update', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_UpdateFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_UpdateFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'naacuprI_DeleteFlg', width: '9%', displayName: 'Delete', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_DeleteFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_DeleteFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'naacuprI_IQACInchargeFlg', width: '12%', displayName: 'IQACIncharge', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_IQACInchargeFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_IQACInchargeFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'naacuprI_ConsultantFlg', width: '10%', displayName: 'Consultant', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_ConsultantFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_ConsultantFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    name: 'naacuprI_TrustUserFlag', width: '10%', displayName: 'Trust User', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.naacuprI_TrustUserFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.naacuprI_TrustUserFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },

                {
                    field: 'id1', name: 'Criteria', width: '9%', displayName: 'Criteria', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#popup" ng-click="grid.appScope.viewrecordcriteria(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '</div>'
                },
                {
                    field: 'id2', name: 'Institution', width: '12%', displayName: 'Institution', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#popupinst" ng-click="grid.appScope.viewrecordinstitution(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '</div>'
                },

                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +

                        '<a ng-if="row.entity.naacuprI_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.naacuprI_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.viewrecordcriteria = function (obj) {
            var data = obj;
            data.flag = 1;
            apiService.create("NAAC_User_Privileges/viewrecord", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsavedcriteria = promise.getsavedcriteria;
                }
            });
        };

        $scope.viewrecordinstitution = function (obj) {
            var data = obj;
            data.flag = 2;
            apiService.create("NAAC_User_Privileges/viewrecord", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsavedinstituiton = promise.getsavedinstituiton;
                }
            });
        };

        // User Deactivate
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            deactiveRecord.flag = 1;
            if (deactiveRecord.naacuprI_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("NAAC_User_Privileges/deactivate", deactiveRecord).
                            then(function (promise) {
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
                                $state.reload();
                                // $scope.BindData();

                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        // Criteria Deactivate
        $scope.deactivecriteria = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            deactiveRecord.flag = 2;
            if (deactiveRecord.naacuprisL_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("NAAC_User_Privileges/deactivate", deactiveRecord).
                            then(function (promise) {
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
                                $state.reload();
                                // $scope.BindData();

                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        // Institution Deactivate
        $scope.deactiveinst = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            deactiveRecord.flag = 3;
            if (deactiveRecord.naacupriiN_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("NAAC_User_Privileges/deactivate", deactiveRecord).
                            then(function (promise) {
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
                                $state.reload();
                                // $scope.BindData();

                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getinstitution.some(function (options) {
                return options.checked;
            });
        };

        $scope.cancel = function () {
            $state.reload();
        };




        $scope.isOptionsRequired1_s = function () {
            return !$scope.array.some(function (options) {
                return options.checkedgrplst_s;
            });
        }


        $scope.firstfnc_s = function (vlobj_s) {
            if (vlobj_s.checkedgrplst_s == true) {
                angular.forEach($scope.array, function (obj) {
                    if (vlobj_s.NAACSL_Id === obj.NAACSL_Id) {
                        angular.forEach(obj.temparra1, function (obj1) {
                            obj1.checkedheadlst_s = true;
                            angular.forEach(obj1.temparray2, function (ss) {
                                ss.checkedinstallmentlst_s = true;
                            });
                        });
                    }
                });
            }
            else {
                angular.forEach($scope.array, function (obj) {
                    if (vlobj_s.NAACSL_Id === obj.NAACSL_Id) {
                        angular.forEach(obj.temparra1, function (obj1) {
                            obj1.checkedheadlst_s = false;
                            angular.forEach(obj1.temparray2, function (ss) {
                                ss.checkedinstallmentlst_s = false;
                            });
                        });
                    }
                });
            }
        }


        $scope.secfnc_s = function (vlobj1_s, alldd) {
            debugger;
            if (vlobj1_s.checkedheadlst_s == true) {
                angular.forEach(vlobj1_s.temparray2, function (val) {
                    val.checkedinstallmentlst_s = true;
                });
            }
            else {
                angular.forEach(vlobj1_s.temparray2, function (val) {
                    val.checkedinstallmentlst_s = false;
                });
            }
            var cnt = 0;
            angular.forEach(alldd.temparra1, function (jj) {
                if (jj.checkedheadlst_s == true) {
                    cnt += 1;
                }
            })
            if (cnt == 0) {
                alldd.checkedgrplst_s = false;
            }
            else {
                alldd.checkedgrplst_s = true;
            }

        }


        $scope.trdfnc_s = function (aa, bb, cc) {
            debugger;
            var cnt = 0;
            angular.forEach(bb.temparray2, function (jj) {
                if (jj.checkedinstallmentlst_s == true) {
                    cnt += 1;
                }
            })
            if (cnt == 0) {
                bb.checkedheadlst_s = false;
            }
            else {
                bb.checkedheadlst_s = true;
            }
            var cnt1 = 0;
            angular.forEach(cc.temparra1, function (ff) {
                if (ff.checkedheadlst_s == true) {
                    cnt1 += 1;
                }
            })
            if (cnt1 == 0) {
                cc.checkedgrplst_s = false;
            }
            else {
                cc.checkedgrplst_s = true;
            }

        }







    }
})();