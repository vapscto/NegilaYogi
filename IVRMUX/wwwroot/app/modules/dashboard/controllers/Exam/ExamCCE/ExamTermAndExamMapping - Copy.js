(function () {
    'use strict';

    angular
        .module('app')
        .controller('ExamTermAndExamMappingController', ExamTermAndExamMappingController);

    ExamTermAndExamMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function ExamTermAndExamMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        // $scope.EME_FinalExamFlag = false;
        $scope.ECT_ActiveFlag = true;
        //var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        $scope.tab2click = function () {
            $scope.termdtl = false;
        }

        $scope.tab1click = function () {
            $scope.termdtl = false;
            $scope.BindData();
        }
        $scope.termdtl = false;

        $scope.Toggle_header = function () {

            var toggleStatus = $scope.exmall;
            angular.forEach($scope.examnamelist, function (itm) {

                itm.selected = toggleStatus;
            });
        }

        $scope.Toggle_field = function (chk_box) {
            $scope.exmall = $scope.examnamelist.every(function (itm) { return itm.selected; })
        }


        $scope.isOptionsRequired1 = function () {
            return !$scope.examnamelist.some(function (options) {
                return options.selected;
            });
        }

        $scope.BindData = function () {

            apiService.getDATA("ExamTermAndExamMapping/Getdetails").
                then(function (promise) {

                    angular.forEach(promise.mapgridlist, function (dd) {

                        dd.ectmP_TermStartDate = $filter('date')(new Date(dd.ectmP_TermStartDate), "dd-MM-yyyy");
                        dd.ectmP_TermEndDate = $filter('date')(new Date(dd.ectmP_TermEndDate), "dd-MM-yyyy");
                        
                    })

                    $scope.gridOptions2.data = promise.mapgridlist;

                    if (promise.termlist != null) {
                        $scope.gridOptions.data = promise.termgridlist;
                        $scope.yearlst = promise.getyear;
                        $scope.termlist = promise.termlist;
                        $scope.categorylist = promise.categorylist;
                    }
                    else {
                        swal("Exam Term is not found")
                    }

                })

        };



        //--------Tab 1-----//
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecT_TermName', displayName: 'Exam Term' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.ecT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ecT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "ECT_Id": $scope.ecT_Id,
                    "ECT_TermName": $scope.ecT_Name,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("ExamTermAndExamMapping/savedetails", data).
                    then(function (promise) {

                        if (promise.returnval === true) {
                            // swal('Data successfully Saved');
                            if (promise.ecT_Id == 0 || promise.ecT_Id < 0) {
                                swal('Record saved successfully');
                            }
                            // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                            else if (promise.ecT_Id > 0) {
                                swal('Record updated successfully');
                            }

                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            //  swal('Recards AlReady Exist !');
                            swal('Record already exist');
                        }
                        else {
                            //swal('Data Not Saved !');
                            if (promise.ecT_Id == 0 || promise.ecT_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.ecT_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel();
                        $scope.BindData();
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            var MEditId = EditRecord.ecT_Id;
            apiService.getURI("ExamTermAndExamMapping/editdetails/", MEditId).
                then(function (promise) {
                    $scope.ecT_Id = promise.editlist[0].ecT_Id;
                    $scope.ecT_Name = promise.editlist[0].ecT_TermName;
                })
        };

        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecT_ActiveFlag === true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                // mgs = "Active";
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

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("ExamTermAndExamMapping/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.already_cnt == true) {
                                    swal("You Can Not Deactivate This Record,It Has Dependency");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        // swal(confirmmgs + " " + " successfully");
                                        swal("Record " + mgs + " Failed");
                                    }
                                }
                                //if (promise.returnval === true) {
                                //    swal(confirmmgs + ' Successfully');
                                //}
                                //else {
                                //    swal('Record Not  Activated/Deactivated');
                                //}
                                $scope.cancel();
                                $scope.BindData();
                                // $scope.clearid1();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $scope.ecT_Id = 0;
            $scope.ecT_TermName = "";
            $scope.ecT_TermStartDate = "";
            $scope.ecT_TermEndDate = "";
            $state.reload();
        }


        //--------Tab 2-----//

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ecT_TermName', displayName: 'Term Name' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'ectmP_Name', displayName: 'Group Name' },
                { name: 'emcA_CategoryName', displayName: 'Category Name' },
                { name: 'ectmP_MarksPerFlag', displayName: 'Status' },
                { name: 'ectmP_MarksPercentValue', displayName: 'Marks/Percentage' },
                { name: 'ectmP_TermStartDate', displayName: 'Term Start Date' },
                { name: 'ectmP_TermEndDate', displayName: 'Term End Date' },

                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +

                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.edittermmap(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;'
                        +
                        '<a ng-if="row.entity.ectmP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ectmP_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        // get Exam - catagory change
        $scope.get_exam = function () {

            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id1,
                "EYC_Id": $scope.eyC_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ExamTermAndExamMapping/get_exam", data).
                then(function (promise) {
                    $scope.examnamelist = promise.examnamelist;
                    if ($scope.ECTMP_Id > 0) {
                        angular.forEach($scope.examnamelist, function (exm) {
                            angular.forEach($scope.editexmlist, function (exm1) {
                                if (exm1.emE_Id == exm.emE_Id) {
                                    exm.selected = true;
                                }
                            })

                        })
                    }
                    if ($scope.examnamelist == null || $scope.examnamelist == "") {
                        swal("Exam are Not Mapped To Selected Category!!!");
                        // $scope.emE_Id = "";

                    }
                })

        }

        $scope.ontermchange = function () {

            angular.forEach($scope.examnamelist, function (st1) {
                st1.selected = false;

            })

            var data = {
                "ECT_Id": $scope.ecT_Id,
            }

            apiService.create("ExamTermAndExamMapping/ontermchange", data).
                then(function (promise) {

                    $scope.editlist = promise.editlist;
                    angular.forEach($scope.examnamelist, function (st1) {
                        angular.forEach($scope.editlist, function (st2) {

                            if (st2.emE_ID == st1.emE_Id) {
                                st1.selected = true;
                            }
                        })
                    })


                });
        }

        $scope.mapsave = function () {
            $scope.exmname = [];
            if ($scope.myForm2.$valid) {

                angular.forEach($scope.examnamelist, function (exm) {
                    if (exm.selected == true) {
                        $scope.exmname.push(exm.emE_Id);
                    }
                })

                var data = {
                    "ECT_Id": $scope.ecT_Name,
                    "EME_Ids": $scope.exmname,
                    "ECTMP_Name": $scope.ectmP_Name,
                    "EMCA_Id": $scope.eyC_Id,
                    "ECTMP_MarksPerFlag": $scope.rdo_mrksper,
                    "ECTMP_MarksPercentValue": $scope.ectmP_MarksPercentValue,
                    "ECTMP_Id": $scope.ECTMP_Id,
                    "ECTMPE_Id": $scope.ECTMPE_Id,
                    "ECTMP_TermStartDate": new Date($scope.FMCB_fromDATE).toDateString(),
                    "ECTMP_TermEndDate": new Date($scope.FMCB_toDATE).toDateString(),
                    "ASMAY_Id": $scope.cfg.ASMAY_Id1
                }

                apiService.create("ExamTermAndExamMapping/savetermmap", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            swal("Record Saved Successfully")
                        }
                        else {
                            swal("Record Not Saved")
                        }

                        $scope.clear2();
                        $scope.BindData();

                    });

            }
            else {

                $scope.submitted2 = true;
                $scope.myForm2.$setPristine();
                $scope.myForm2.$setUntouched();
            }

        }

        $scope.edittermmap = function (data) {
            var id = data.ectmP_Id;
            apiService.getURI("ExamTermAndExamMapping/edittermmap/", id).
                then(function (promise) {

                    $scope.ECTMP_Id = promise.editlist[0].ectmP_Id;
                    $scope.ecT_Id = promise.editlist[0].ecT_Id;
                    $scope.ecT_Name = promise.editlist[0].ecT_Id;
                    $scope.eyC_Id = promise.editlist[0].emcA_Id;
                    $scope.ectmP_Name = promise.editlist[0].ectmP_Name;
                    $scope.rdo_mrksper = promise.editlist[0].ectmP_MarksPerFlag;
                    $scope.ectmP_MarksPercentValue = promise.editlist[0].ectmP_MarksPercentValue;
                    // $scope.emE_Id = promise.editexmlist[0].EME_ID;
                    $scope.FMCB_fromDATE = new Date(promise.editlist[0].ectmP_TermStartDate);
                    $scope.FMCB_toDATE = new Date(promise.editlist[0].ectmP_TermEndDate);

                    $scope.ECTMPE_Id = promise.editexmlist[0].ectmpE_Id;
                    $scope.cfg.ASMAY_Id1 = promise.editlist[0].asmaY_Id;

                    $scope.editexmlist = promise.editexmlist;

                    $scope.get_exam($scope.eyC_Id);
                })
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.clear2 = function () {
            angular.forEach($scope.examnamelist, function (exm) {

                exm.selected = false;
            })
            $scope.ecT_Name = "";
            $scope.eyC_Id = "";
            $scope.rdo_mrksper = "M";
            $scope.ectmP_MarksPercentValue = "";
            $scope.examnamelist = "";
            $scope.ectmP_Name = "";
            $scope.FMCB_fromDATE = "";
            $scope.FMCB_toDATE = "";


            $scope.submitted2 = false;

        }

        $scope.viewrecordspopup = function (employee) {
            $scope.editEmployee = employee.ectmP_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamTermAndExamMapping/getexampopup", pageid).
                then(function (promise) {
                    $scope.viewrecordspopupdisplay = promise.exampopup;
                })
        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        //act/deact mapgrid
        $scope.deactive1 = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            var id = deactiveRecord.ectmP_Id
            if (deactiveRecord.ectmP_ActiveFlag == true) {
                //mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                // mgs = "Active";
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

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.getURI("ExamTermAndExamMapping/deactivate1", id).
                            then(function (promise) {

                                if (promise.already_cnt == true) {
                                    swal("You Can Not Deactivate This Record,It Has Dependency");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        // swal(confirmmgs + " " + " successfully");
                                        swal("Record " + mgs + " Failed");
                                    }
                                }
                                //if (promise.returnval === true) {
                                //    swal(confirmmgs + ' Successfully');
                                //}
                                //else {
                                //    swal('Record Not  Activated/Deactivated');
                                //}
                                $scope.clear2();
                                $scope.BindData();
                                // $scope.clearid1();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.deactive_sub = function (employee) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ectmpE_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
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

                        apiService.create("ExamTermAndExamMapping/deactive_sub", employee).
                            then(function (promise) {
                                if (promise.already_cnt == true) {
                                    swal("You Can Not Deactivate This Record,It Has Dependency");
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        swal("Record " + mgs + " Failed");
                                    }
                                }
                                $('#popup').modal('hide');                              
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
})();
