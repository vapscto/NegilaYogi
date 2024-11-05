
(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSubjectGroupController', MasterSubjectGroupController)

    MasterSubjectGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MasterSubjectGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.gridOptions = {

            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNo', displayName: 'SL NO', width: '8%', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'emG_GroupName', displayName: 'Group Name', width: '20%'},
                { name: 'emG_TotSubjects', width: '10%', displayName: 'Total Subjects' },
                { name: 'emG_MaxAplSubjects', displayName: 'Max.Applicable Subjects' },
                { name: 'emG_MinAplSubjects', displayName: 'Min.Applicable Subjects' },
                { name: 'emG_BestOff', width: '12%', displayName: 'BestOff Subjects' },
                {
                    name: 'emG_ElectiveFlg', width: '12%', displayName: 'Elective Subjects', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.emG_ElectiveFlg == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
                        '<a ng-if="row.entity.emG_ElectiveFlg == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
                        '</div>'
                },
                {
                    field: 'id', name: '', width: '11%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +//ng-if="row.entity.emG_ElectiveFlg === true"
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.emG_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.emG_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.sortKey = 'ismS_OrderFlag';
        $scope.sortReverse = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //TO  GEt The Values iN Grid

        $scope.EMGD_From = "";
        $scope.EMGD_To = "";
        $scope.EMGD_Name = "";
        $scope.EMGD_GradePoints = "";
        $scope.EMGD_Remarks = "";
        $scope.EMGR_MarksPerFlag = "P";
        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            // $scope.usercheck = true;


            apiService.getDATA("MasterSubjectGroup/getalldetails").
                then(function (promise) {
                    // $scope.newuser = promise.mastersubsubject;
                    $scope.subject_list = promise.subjectlist;
                    $scope.subj_count = promise.subjectlist.length;
                    $scope.gridOptions.data = promise.group_list;
                    // $scope.all_check();
                })
        };

        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // TO Save The Data         
        $scope.submitted2 = false;
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                $scope.array = [];
                var chk_count = 0;
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.subject == true) {
                        chk_count += 1;
                        $scope.array.push(itm);
                    }
                });
                $scope.array1 = [];
                var chk_count1 = 0;
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.checkedvalue == true) {
                        chk_count1 += 1;
                        $scope.array1.push(itm);
                    }
                });
                if ($scope.EMG_MinAplSubjects == $scope.EMG_MaxAplSubjects && $scope.EMG_MaxAplSubjects == $scope.EMG_TotSubjects) {
                    $scope.EMG_ElectiveFlg = 0;
                }
                else {
                    $scope.EMG_ElectiveFlg = 1;
                }
                //if (chk_count == Number($scope.EMG_TotSubjects)) {
                if (chk_count1 == Number($scope.EMG_TotSubjects)) {
                    var data = {
                        "EMG_Id": $scope.EMG_Id,
                        "EMGS_Id": $scope.EMGS_Id,
                        "EMG_GroupName": $scope.EMG_GroupName,
                        "EMG_TotSubjects": Number($scope.EMG_TotSubjects),
                        "EMG_MaxAplSubjects": Number($scope.EMG_MaxAplSubjects),
                        "EMG_MinAplSubjects": Number($scope.EMG_MinAplSubjects),
                        "EMG_BestOff": Number($scope.EMG_BestOff),
                        "EMG_ElectiveFlg": $scope.EMG_ElectiveFlg,
                        // subj_list: $scope.array,
                        //MB for table type
                        subj_list: $scope.array1,
                        // "EMG_ElectiveFlg": true,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("MasterSubjectGroup/savedetail", data).
                        then(function (promise) {
                            $scope.gridOptions.data = promise.group_list;
                            // $scope.category_list = promise.categorylist;
                            if (promise.returnval === true) {
                                //swal('Data successfully Saved');
                                if (promise.emG_Id == 0 || promise.emG_Id < 0) {
                                    swal('Record saved successfully');
                                }
                                // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                else if (promise.emG_Id > 0) {
                                    swal('Record updated successfully');
                                }


                            }
                            else if (promise.returnduplicatestatus === 'Duplicate') {
                                // swal('Recards AlReady Exist !');
                                swal('Record already exist');
                            }
                            else {
                                if (promise.emG_Id == 0 || promise.emG_Id < 0) {
                                    swal('Failed to save, please contact administrator');
                                }
                                else if (promise.emG_Id > 0) {
                                    swal('Failed to update, please contact administrator');
                                }

                            }
                            $scope.BindData();
                            $scope.clear();
                        })
                    //$scope.BindData();
                    //$scope.clear();

                }
                else {
                    //swal("Select Max.Subjects  No.of Subjects !!!![" + $scope.EMG_TotSubjects + "]");
                    swal("Selected Subjects Count Should be Equal to Max.Subjects Field Value !!!");
                }
            }
            else {
                $scope.submitted2 = true;
            }
        }

        $scope.getorgvalue = function (employee) {
            $scope.clear();
            $scope.editEmployee = employee.emG_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterSubjectGroup/getdetails", pageid).
                then(function (promise) {
                    $scope.EMG_Id = promise.edit_m_group[0].emG_Id;
                    $scope.EMG_GroupName = promise.edit_m_group[0].emG_GroupName;
                    $scope.EMG_TotSubjects = promise.edit_m_group[0].emG_TotSubjects;
                    $scope.EMG_MaxAplSubjects = promise.edit_m_group[0].emG_MaxAplSubjects;
                    $scope.EMG_MinAplSubjects = promise.edit_m_group[0].emG_MinAplSubjects;
                    $scope.EMG_BestOff = promise.edit_m_group[0].emG_BestOff;
                    //$scope.EMG_ElectiveFlg = promise.edit_m_group[0].emG_ElectiveFlg;
                    if (promise.edit_m_group[0].emG_ElectiveFlg == true) {
                        $scope.EMG_ElectiveFlg = 1;
                    }
                    else if (promise.edit_m_group[0].emG_ElectiveFlg == false) {
                        $scope.EMG_ElectiveFlg = 0;
                    }
                    for (var i = 0; i < promise.edit_m_group_subjects.length; i++) {
                        angular.forEach($scope.subject_list, function (itm) {
                            if (itm.ismS_Id == promise.edit_m_group_subjects[i].ismS_Id) {
                                itm.checkedvalue = true;
                                $scope.optionToggled(itm);
                            }
                        });
                    }
                    //for mutilple subjects
                    //for (var i = 0; i < promise.edit_m_group_subjects.length; i++) {
                    //    angular.forEach($scope.subject_list, function (itm) {
                    //        if (itm.ismS_Id == promise.edit_m_group_subjects[i].ismS_Id) {
                    //            itm.subject = true;
                    //            $scope.togchkbx($scope.EMG_TotSubjects, $scope.EMG_MaxAplSubjects, itm);
                    //        }
                    //    });
                    //}
                    //
                    //  $scope.togchkbx($scope.EMG_TotSubjects, $scope.EMG_MaxAplSubjects, itm);

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
            if (employee.emG_ActiveFlag === true) {
                // mgs = "Deactive";
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

                        apiService.create("MasterSubjectGroup/deactivate", employee).
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
                                //if (promise.returnval == true) {
                                //    swal(confirmmgs + " " + "successfully.");
                                //}
                                //else {
                                //    swal(confirmmgs + " " + " successfully");
                                //}
                                $scope.clear();
                                $scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.emG_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("MasterSubjectGroup/getalldetailsviewrecords", pageid).
                then(function (promise) {

                    $scope.Group_Name = promise.view_group_subjects[0].emG_GroupName;
                    $scope.viewrecordspopupdisplay = promise.view_group_subjects;

                })

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };


        $scope.interacted2 = function (field) {

            return $scope.submitted2;
        };


        $scope.totalgridtest = [];
        $scope.addNew = function (totalgrid, index) {


            if ($scope.myForm.$valid) {

                $scope.rows.push({ count: totalgrid.length, EMGD_From: totalgrid.EMGD_From, EMGD_To: totalgrid.EMGD_To, EMGD_Name: totalgrid.EMGD_Name, EMGD_GradePoints: totalgrid.EMGD_GradePoints, EMGD_Remarks: totalgrid.EMGD_Remarks });

            } else {
                $scope.submitted2 = true;
            }
        }

        $scope.removerow = function (totalgrid, index) {
            if (totalgrid.length > 1) {
                $scope.rows.splice(index, 1);
            }

        }

        $scope.valid_max_subs = function (tot_subjs) {


            var num_tot_subjs = Number(tot_subjs);
            if (num_tot_subjs > 0) {
                if (num_tot_subjs > $scope.subj_count) {
                    swal("U can't set Maximum Subjects More Than Available Subjects[" + $scope.subj_count + "] !!!");
                    $scope.EMG_TotSubjects = "";
                }
            }
            else if (num_tot_subjs == 0) {
                swal("Max.Subjects Not Set To Zero ");
                $scope.EMG_TotSubjects = "";
            }
            $scope.EMG_MaxAplSubjects = "";
            $scope.EMG_MinAplSubjects = "";
            $scope.EMG_BestOff = "";
            $scope.usercheck = false;
            angular.forEach($scope.subject_list, function (itm) {
                itm.subject = false;
            });
            $scope.all = false;
            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = false;
            });

        }
        $scope.valid_max_apl_subs = function (tot_subjs, max_apl_subjs) {

            if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {

                var num_tot_subjs = Number(tot_subjs);

                var num_max_apl_subjs = Number(max_apl_subjs);
                if (num_max_apl_subjs > 0) {
                    if (num_max_apl_subjs > num_tot_subjs) {
                        swal("U can't set Max Appicable Subjects More Than Max.Subjects[" + $scope.EMG_TotSubjects + "] !!!");
                        $scope.EMG_MaxAplSubjects = "";
                    }
                }
                else if (num_max_apl_subjs == 0) {
                    swal("Max Applicable Subjects Not Set To Zero ");
                    $scope.EMG_MaxAplSubjects = "";
                }
            }
            else {
                swal("First Set Max.Subjects Field !!!");
                $scope.EMG_MaxAplSubjects = "";
            }
            $scope.EMG_MinAplSubjects = "";
            $scope.EMG_BestOff = "";
        }
        $scope.valid_min_apl_subs = function (max_apl_subjs, min_apl_subjs) {

            if (max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != "") {
                //var num_tot_subjs = Number(tot_subjs);

                var num_max_apl_subjs = Number(max_apl_subjs);
                var num_min_apl_subjs = Number(min_apl_subjs);
                //if (num_min_apl_subjs > 0) {
                //    if (num_min_apl_subjs > num_max_apl_subjs) {
                //        swal("U can't set Min Appicable Subjects More Than Max Applicable Subjects[" + $scope.EMG_MaxAplSubjects + "] !!!");
                //        $scope.EMG_MinAplSubjects = "";
                //    }
                //}
                //else if (num_min_apl_subjs == 0) {
                //    swal("Min Applicable Subjects Not Set To Zero ");
                //    $scope.EMG_MinAplSubjects = "";
                //}
            }
            else {
                swal("First Set Max Applicable Subjects Field !!!");
                $scope.EMG_MinAplSubjects = "";
            }
            $scope.EMG_BestOff = "";
            if ($scope.EMG_MinAplSubjects == $scope.EMG_MaxAplSubjects && $scope.EMG_MaxAplSubjects == $scope.EMG_TotSubjects) {
                $scope.EMG_ElectiveFlg = 0;
            }
            else {
                $scope.EMG_ElectiveFlg = 1;
            }
        }
        $scope.valid_best = function (max_apl_subjs, min_apl_subjs, best_subjs) {

            if (best_subjs != null && best_subjs != "" && best_subjs != undefined) {
                if (max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != "" && min_apl_subjs != null && min_apl_subjs != undefined && min_apl_subjs != "") {
                    //var num_tot_subjs = Number(tot_subjs);&& min_apl_subjs != null && min_apl_subjs != undefined && min_apl_subjs != ""

                    var num_max_apl_subjs = Number(max_apl_subjs);
                    var num_min_apl_subjs = Number(min_apl_subjs);
                    var num_best_subjs = Number(best_subjs);
                    if (num_best_subjs > num_max_apl_subjs) {
                        swal("U can't set Best Off Subjects More Than Max. Applicable Subjects [" + $scope.EMG_MaxAplSubjects + "] !!!");
                        $scope.EMG_BestOff = "";
                    }
                    if (num_best_subjs < num_min_apl_subjs) {
                        swal("U can't set Best Off Subjects Less Than Min. Applicable Subjects [" + $scope.EMG_MinAplSubjects + "] !!!");
                        $scope.EMG_BestOff = "";
                    }

                    //if (num_min_apl_subjs > 0) {
                    //if (num_best_subjs > num_max_apl_subjs || num_best_subjs < num_min_apl_subjs) {
                    //    swal("U can't set Best Off Subjects Less Than Min.[" + $scope.EMG_MinAplSubjects + "] Or  More Than Max.[" + $scope.EMG_MaxAplSubjects + "]  Applicable Subjects!!!");
                    //        $scope.EMG_BestOff = "";
                    //    }
                    // }
                    //else if (num_min_apl_subjs == 0) {
                    // swal("Max Applicable Subjects Not Set To Zero ");
                    //   $scope.EMG_MinAplSubjects = "";
                    // }
                }
                else {
                    swal("First Set Max and Min  Applicable Subjects Field !!!");
                    $scope.EMG_BestOff = "";
                }
            }
        }

        $scope.isOptionsRequired = function () {
            if ($scope.EMG_ElectiveFlg == "1") {
                return !$scope.subject_list.some(function (options) {
                    return options.subject;
                });
            }
            else if ($scope.EMG_ElectiveFlg == "0") {
                return false;
            }

        }
        $scope.EMG_ElectiveFlg = 1;
        $scope.all_check = function (tot_subjs, max_apl_subjs) {

            var toggleStatus = $scope.usercheck;
            if (toggleStatus == true) {
                if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {
                    //&& max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != ""
                    var num_tot_subjs = Number(tot_subjs);
                    if ($scope.subject_list.length <= num_tot_subjs) {

                        angular.forEach($scope.subject_list, function (itm) {
                            itm.subject = toggleStatus;
                        });
                    }
                    else {
                        swal("Selection of Subjects Not More Than Of Max.Subjects");
                        $scope.usercheck = false;
                    }
                }
                else {
                    swal("First Set Max.Subjects Field !!!");
                    angular.forEach($scope.subject_list, function (itm) {
                        itm.subject = false;
                    });
                    $scope.usercheck = false;
                }
            }
            else if (toggleStatus == false) {
                angular.forEach($scope.subject_list, function (itm) {
                    itm.subject = toggleStatus;
                });
            }
        }
        $scope.togchkbx = function (tot_subjs, max_apl_subjs, chk_box) {


            if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {

                var chk_count = 0;
                var num_tot_subjs = Number(tot_subjs);
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.subject == true) {
                        chk_count += 1;
                    }
                });
                if (chk_count <= num_tot_subjs) {
                    $scope.usercheck = $scope.subject_list.every(function (options) {
                        return options.subject;
                    });

                }
                else {
                    swal("Selection of Subjects Not More Than Of Max.Subjects");
                    angular.forEach($scope.subject_list, function (itm) {
                        if (itm.ismS_Id == chk_box.ismS_Id) {
                            itm.subject = false;
                        }
                    });
                }

            } else {
                swal("First Set Max.Subjects Field !!!");
                angular.forEach($scope.subject_list, function (itm) {
                    itm.subject = false;
                });
                $scope.usercheck = false;
            }
            //$scope.usercheck = $scope.subject_list.every(function (options) {
            //    return options.subject;
            //});
            //$scope.all_check(tot_subjs, max_apl_subjs);
        }

        $scope.clear = function () {

            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.EMG_Id = 0;
            $scope.EMGS_Id = 0;
            $scope.EMG_GroupName = "";
            $scope.EMG_TotSubjects = "";
            $scope.EMG_MaxAplSubjects = "";
            $scope.EMG_MinAplSubjects = "";
            $scope.EMG_BestOff = "";
            $scope.EMG_ElectiveFlg = 1;
            $scope.usercheck = false;

            angular.forEach($scope.subject_list, function (itm) {
                itm.subject = false;
            });
            $scope.all = false;

            angular.forEach($scope.subject_list, function (itm) {
                itm.checkedvalue = false;
            });
            $scope.submitted2 = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
        };


        $scope.toggleAll = function () {
            var tot_subjs = $scope.EMG_TotSubjects;

            var toggleStatus = $scope.all;
            if (toggleStatus == true) {
                if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {
                    //&& max_apl_subjs != null && max_apl_subjs != undefined && max_apl_subjs != ""
                    var num_tot_subjs = Number(tot_subjs);
                    if ($scope.subject_list.length <= num_tot_subjs) {

                        angular.forEach($scope.subject_list, function (itm) {
                            itm.checkedvalue = toggleStatus;
                        });
                    }
                    else {
                        swal("Selection of Subjects Not More Than Of Max.Subjects");
                        $scope.all = false;
                    }
                }
                else {
                    swal("First Set Max.Subjects Field !!!");
                    angular.forEach($scope.subject_list, function (itm) {
                        itm.checkedvalue = false;
                    });
                    $scope.all = false;
                }
            }
            else if (toggleStatus == false) {
                angular.forEach($scope.subject_list, function (itm) {
                    itm.checkedvalue = toggleStatus;
                });
            }
            //var toggleStatus = $scope.all;
            //angular.forEach($scope.subject_list, function (itm) {
            //    itm.checkedvalue = toggleStatus;

            //});
        }
        $scope.optionToggled = function (chk_box) {
            var tot_subjs = $scope.EMG_TotSubjects;

            if (tot_subjs != null && tot_subjs != undefined && tot_subjs != "") {

                var chk_count = 0;
                var num_tot_subjs = Number(tot_subjs);
                angular.forEach($scope.subject_list, function (itm) {
                    if (itm.checkedvalue == true) {
                        chk_count += 1;
                    }
                });
                if (chk_count <= num_tot_subjs) {
                    $scope.all = $scope.subject_list.every(function (options) {
                        return options.checkedvalue;
                    });

                }
                else {
                    swal("Selection of Subjects Not More Than Of Max.Subjects");
                    angular.forEach($scope.subject_list, function (itm) {
                        if (itm.ismS_Id == chk_box.ismS_Id) {
                            itm.checkedvalue = false;
                        }
                    });
                }

            } else {
                swal("First Set Max.Subjects Field !!!");
                angular.forEach($scope.subject_list, function (itm) {
                    itm.checkedvalue = false;
                });
                $scope.all = false;
            }

            // $scope.all = $scope.subject_list.every(function (itm) { return itm.checkedvalue; })
        }

    }

})();