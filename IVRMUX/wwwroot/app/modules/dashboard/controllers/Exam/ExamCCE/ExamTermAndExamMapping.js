(function () {
    'use strict';
    angular.module('app').controller('ExamTermAndExamMappingController', ExamTermAndExamMappingController);
    ExamTermAndExamMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function ExamTermAndExamMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.viewrecord = {};
        $scope.deactiveRecord = {};
        $scope.deactiveRecordchild = {};
        $scope.dis = false;
        $scope.ECT_ActiveFlag = true;
        $scope.ECTEX_RoundOffReqFlg = false;

        $scope.cfg = {};
        $scope.currentPage = 1;
        $scope.BindData = function () {
            apiService.getDATA("ExamTermAndExamMapping/Getdetails").then(function (promise) {
                $scope.yearlst = promise.getyear;
                $scope.gradelist = promise.getgradelist;
                $scope.gridOptions.data = promise.mapgridlist;
            });
        };

        $scope.onchangeyear = function () {
            $scope.examlist = [];
            $scope.categorylist = [];
            $scope.EMCA_Id = "";
            $scope.EME_Id = "";
            $scope.ECT_Name = "";
            $scope.ECT_Marks = "";
            $scope.ECT_MinMarks = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("ExamTermAndExamMapping/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.categorylist = promise.categorylist;

                    angular.forEach($scope.yearlst, function (dd) {

                        if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                            $scope.mindate = new Date(dd.asmaY_From_Date);
                            $scope.maxdate = new Date(dd.asmaY_To_Date);
                        }
                    });

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onchangecategory = function () {
            $scope.EME_Id = "";
            $scope.ECT_Name = "";
            $scope.ECT_Marks = "";
            // $scope.ECT_MinMarks = "";
            $scope.examlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": $scope.EMCA_Id
            };
            apiService.create("ExamTermAndExamMapping/onchangecategory", data).then(function (promise) {
                if (promise !== null) {
                    $scope.examlist = promise.examlist;
                } else {
                    swal("No Records Found");
                }

            });
        };

        $scope.checktermname = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ECT_TermName": $scope.ECT_Name
            };
            apiService.create("ExamTermAndExamMapping/checktermname", data).then(function (promise) {
                if (promise.message !== null && promise.message !== "") {
                    $scope.ECT_Name = "";
                    swal(promise.message);
                }
            });
        };

        $scope.onchangedate = function () {
            $scope.ECT_PublishDate = null;
            $scope.ECT_TermEndDate   = null;
            $scope.fromdate = new Date($scope.ECT_TermStartDate);
            $scope.minDatef = new Date(
                $scope.fromdate.getFullYear(),
                $scope.fromdate.getMonth(),
                $scope.fromdate.getDate() + 1);
        };

        $scope.onchangetodate = function () {
            $scope.ECT_PublishDate = null;
            $scope.todate = new Date($scope.ECT_TermEndDate);
            $scope.minpbuDatef = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate() + 1);
        };

        $scope.selectedclass = [];
        $scope.submitted = false;

        // -------- ADD BUTTON -----------//
        $scope.adddetails = function () {
            $scope.searchchkbx = '';
            if ($scope.myForm.$valid) {

                var dd = Number($scope.ECT_Marks);
                var ddd = Number($scope.ECT_MinMarks);

                if (ddd < dd) {
                    //dd
                    $scope.exmname = "";
                    $scope.dis = true;
                    angular.forEach($scope.examlist, function (d) {
                        if (d.emE_Id === parseInt($scope.EME_Id)) {
                            $scope.exmname = d.emE_ExamName;
                        }
                    });
                    var ECTEXConversionReqFlg = false;
                    if ($scope.ECTEX_ConversionReqFlg === true) {
                        ECTEXConversionReqFlg = true;
                    } else {
                        ECTEXConversionReqFlg = false;
                    }

                    var ECTEX_NotApplToTotalFlg = false;
                    if ($scope.ECTEX_NotApplToTotalFlg === true) {
                        ECTEX_NotApplToTotalFlg = true;
                    } else {
                        ECTEX_NotApplToTotalFlg = false;
                    }

                    if ($scope.selectedclass.length === 0) {
                        $scope.selectedclass.push({
                            examname: $scope.exmname, examid: $scope.EME_Id, marksorpercentage: $scope.ECTEX_MarksPercentValue,
                            marksorpercentageflag: $scope.ECTEX_MarksPerFlag, roundofflag: $scope.ECTEX_RoundOffReqFlg, converstionreqflag: ECTEXConversionReqFlg,
                            ECTEX_NotApplToTotalFlg: ECTEX_NotApplToTotalFlg
                        });
                    } else if ($scope.selectedclass.length > 0) {
                        var count = 0;
                        angular.forEach($scope.selectedclass, function (dd) {
                            if (dd.examid === parseInt($scope.EME_Id)) {
                                count += 1;
                            }
                        });

                        if (count === 0) {
                            var sum = 0;
                            sum = parseFloat($scope.ECTEX_MarksPercentValue);
                            angular.forEach($scope.selectedclass, function (dd) {
                                sum += parseFloat(dd.marksorpercentage);
                            });

                            if (sum <= parseFloat($scope.ECT_Marks)) {
                                $scope.selectedclass.push({
                                    examname: $scope.exmname, examid: $scope.EME_Id, marksorpercentage: $scope.ECTEX_MarksPercentValue,
                                    marksorpercentageflag: $scope.ECTEX_MarksPerFlag, roundofflag: $scope.ECTEX_RoundOffReqFlg, converstionreqflag: ECTEXConversionReqFlg,
                                    ECTEX_NotApplToTotalFlg: ECTEX_NotApplToTotalFlg
                                });

                            } else {
                                swal("Marks / Percentage Should Be Equal To Term Marks");
                            }
                        } else {
                            swal("Already Exam Details Are Added You Can Not Add Same Exam For Same Term");
                            return;
                        }
                    }
                    $scope.ECTEX_MarksPercentValue = "";
                    $scope.ECTEX_MarksPerFlag = 'M';
                    $scope.ECTEX_RoundOffReqFlg = false;
                    $scope.ECTEX_ConversionReqFlg = false;
                    $scope.ECTEX_NotApplToTotalFlg = false;
                    $scope.EME_Id = "";
                }
                else {
                    swal("Min Marks Should be Less than Term Marks");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        // ----- Selected Edit Button -----//
        $scope.edit = function (row, index) {
            $scope.dis = true;
            $scope.clldis = true;
            $scope.all = row.elpflg;
        };

        // ----- Selected Delete Button -----//
        $scope.delete = function (row, index) {
            $scope.act = 'add';
            $scope.dis = true;
            $scope.ivrmulF_Id = "";
            $scope.hrmE_Id = "";
            for (var x = 0; x < $scope.selectedclass.length; x++) {
                if (x === index) {
                    $scope.selectedclass.splice(x, 1);
                }
            }
        };

        // Save Details
        $scope.saveddata = function () {

            var sumnew = 0;
            angular.forEach($scope.selectedclass, function (d) {
                sumnew += parseFloat(d.marksorpercentage);
            });

            if (sumnew < parseFloat($scope.ECT_Marks)) {
                swal("Marks / Percentage Should Be Equal To Term Marks");
                return;
            }


            var data = {
                "ECT_Id": $scope.ecT_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ECT_TermName": $scope.ECT_Name,
                "ECT_MinMarks": $scope.ECT_MinMarks,
                "EMGR_Id": $scope.EMGR_Id,
                "ECT_TermStartDate": new Date($scope.ECT_TermStartDate).toDateString(),
                "ECT_TermEndDate": new Date($scope.ECT_TermEndDate).toDateString(),
                "ECT_PublishDate": new Date($scope.ECT_PublishDate).toDateString(),
                "ECT_Marks": $scope.ECT_Marks,
                "saveddetails": $scope.selectedclass
            };
            apiService.create("ExamTermAndExamMapping/saveddata", data).then(function (promise) {

                if (promise.message === "Add") {
                    if (promise.returnval === true) {
                        swal("Record Saved / Update Successfully");
                    } else {
                        swal("Failed To Save / Update Record");
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
                $state.reload();
            });
        };

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            apiService.create("ExamTermAndExamMapping/editdetailsnew/", EditRecord).then(function (promise) {
                $scope.dis = true;
                $scope.ecT_Id = promise.editlist[0].ecT_Id;
                $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                $scope.EMGR_Id = promise.editlist[0].emgR_Id;
                $scope.onchangeyear();
                $scope.EMCA_Id = promise.editlist[0].emcA_Id;
                $scope.onchangecategory();

                $scope.ECT_Name = promise.editlist[0].ecT_TermName;
                $scope.ECT_MinMarks = promise.editlist[0].ecT_MinMarks;
                $scope.ECT_TermStartDate = new Date(promise.editlist[0].ecT_TermStartDate);
                $scope.ECT_TermEndDate = new Date(promise.editlist[0].ecT_TermEndDate);
                $scope.ECT_PublishDate = new Date(promise.editlist[0].ecT_PublishDate);
                $scope.ECT_Marks = promise.editlist[0].ecT_Marks;
                $scope.selectedclass = promise.getexamlist;

                angular.forEach($scope.selectedclass, function (dd) {
                    dd.examname = dd.emE_ExamName;
                    dd.marksorpercentage = dd.ecteX_MarksPercentValue;
                    dd.marksorpercentageflag = dd.ecteX_MarksPerFlag;
                    dd.examid = dd.emE_Id;
                    dd.marksorpercentageflag = dd.ecteX_MarksPerFlag;
                    dd.roundofflag = dd.ecteX_RoundOffReqFlg;
                    dd.converstionreqflag = dd.ecteX_ConversionReqFlg;
                    if (dd.ecteX_NotApplToTotalFlg === true) {
                        dd.ECTEX_NotApplToTotalFlg = true;
                    } else {
                        dd.ECTEX_NotApplToTotalFlg = false;
                    }
                    //dd.ECTEX_Id = dd.ECTEX_Id
                });

            });
        };

        // To View Exam Data
        $scope.viewrecordspopup = function (viewrecord) {
            apiService.create("ExamTermAndExamMapping/viewrecordspopup", viewrecord).then(function (promise) {
                if (promise !== null) {
                    $scope.viewdetails = promise.viewdetails;
                }
            });
        };

        // To Deactive Main
        $scope.deactive = function (deactiveRecord) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ecT_ActiveFlag === true) {
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
                        apiService.create("ExamTermAndExamMapping/deactivatenew", deactiveRecord).then(function (promise) {
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
        };

        // To Deactive Child
        $scope.deactivesub = function (deactiveRecordchild) {

            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecordchild.ecteX_ActiveFlag === true) {
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
                        apiService.create("ExamTermAndExamMapping/deactivesub", deactiveRecordchild).then(function (promise) {
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

                            $scope.viewdetails = promise.viewdetails;
                            $scope.gridOptions.data = promise.mapgridlist;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.clearpopupgrid = function () {
            $("#popup").hide();
            $("#popup").modal({ backdrop: false });
            $state.BindData();
        };

        //--------Tab 1-----//
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'Termname', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'emcA_CategoryName', displayName: 'Category' },
                { name: 'ecT_TermName', displayName: 'Term Name' },
                { name: 'emgR_GradeName', displayName: 'Grade' },
                { name: 'ecT_Marks', displayName: 'Term Marks' },
                { name: 'ecT_MinMarks', displayName: 'Min Marks' },
                { name: 'ecT_TermStartDate', displayName: 'Term Start Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ecT_TermEndDate', displayName: 'Term End Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'ecT_PublishDate', displayName: 'Term Publish Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
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
                    "ECT_TermName": $scope.ecT_Name
                };
                apiService.create("ExamTermAndExamMapping/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.ecT_Id === 0 || promise.ecT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ecT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ecT_Id === 0 || promise.ecT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ecT_Id > 0) {
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

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();
