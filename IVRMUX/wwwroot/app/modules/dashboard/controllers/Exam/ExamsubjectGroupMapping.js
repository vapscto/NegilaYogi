
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamsubjectGroupMappingController', ExamsubjectGroupMappingController)

    ExamsubjectGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamsubjectGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.EME_FinalExamFlag = false;
        $scope.EME_ActiveFlag = true;


        $scope.BindData = function () {
            apiService.getDATA("ExamsubjectGroupMapping/Getdetails/2").then(function (promise) {
                $scope.acdlist = promise.getyear;
                $scope.gridOptions.data = promise.getdetails;
                $scope.indiexam = true;
                $scope.proexam = false;
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'emcA_CategoryName', displayName: 'Category' },
                { name: 'emE_ExamName', displayName: 'Exam Name' },
                { name: 'grpname', displayName: 'Subject Group' },
                { name: 'percentage', displayName: 'Minimum Marks' },
                { name: 'esG_GroupMaxMarks', displayName: 'Maximum Marks' },
                { name: 'compulsory1', displayName: 'Compulsory Flag' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        //'<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata1(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Year' },
                { name: 'emcA_CategoryName', displayName: 'Category' },
                { name: 'grpname', displayName: 'Subject Group' },
                { name: 'percentage', displayName: 'Minimum Marks' },
                { name: 'esG_GroupMaxMarks', displayName: 'Maximum Marks' },
                { name: 'compulsory1', displayName: 'Compulsory Flag' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        //'<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata2(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup1" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup1(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi1) {
                $scope.gridApi1 = gridApi1;
            }
        };

        $scope.sub = {};

        //getlist 
        $scope.getlist = function () {
            $scope.gridOptions.data = [];
            $scope.gridOptions1.data = [];
            var data = {
                "Flag": $scope.examtype
            };
            apiService.create("ExamsubjectGroupMapping/getlist", data).then(function (promise) {
                if (promise !== null) {
                    if ($scope.examtype == "0") {
                        if (promise.getdetails.length > 0) {
                            $scope.gridOptions.data = promise.getdetails;
                            $scope.indiexam = true;
                            $scope.proexam = false;
                        } else {
                            swal("No Records Found");
                            $scope.proexam = false;
                            $scope.indiexam = false;
                        }
                    } else {
                        if (promise.getdetails.length > 0) {
                            $scope.gridOptions1.data = promise.getdetails;
                            $scope.indiexam = false;
                            $scope.proexam = true;
                        } else {
                            swal("No Records Found");
                            $scope.proexam = false;
                            $scope.indiexam = false;
                        }

                    }
                }
            });
        };

        // get category
        $scope.getcategory1 = function (sub, ASMAY_Id) {
            $scope.getcategory = "";
            $scope.EMCA_Id = "";
            $scope.getexam = "";
            $scope.EME_Id = "";
            $scope.getsubject = "";
            $scope.sub.checkedsub = false;
            $scope.searchchkbx = "";
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("ExamsubjectGroupMapping/getcategory", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getcategory.length > 0) {
                        $scope.getcategory = promise.getcategory;
                    } else {
                        swal("No Category Mapped For This Academic Year");
                    }
                } else {
                    swal("No Category Mapped For This Academic Year");
                }
            });
        };

        //Get exam
        $scope.getexam1 = function (sub, EMCA_Id, Flag1) {
            if (Flag1 == "A") {
                Flag1 = "A";
            } else {
                Flag1 = "E";
            }

            $scope.getexam = "";
            $scope.getsubject = "";
            $scope.EME_Id = "";
            $scope.sub.checkedsub = false;
            $scope.searchchkbx = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": EMCA_Id,
                "Flag": $scope.examtype,
                "Flag1": Flag1
            };
            apiService.create("ExamsubjectGroupMapping/getexam", data).then(function (promise) {
                if (promise !== null) {

                    if ($scope.examtype == "0") {
                        if (promise.getexam.length > 0) {
                            $scope.getexam = promise.getexam;
                        }
                        else {
                            swal("No Exam Is Mapped For This Category");
                        }
                    } else {
                        if (promise.getsubject.length > 0) {
                            $scope.getsubject = promise.getsubject;
                            if ($scope.ESG_Id != undefined && $scope.ESG_Id != "" && $scope.ESG_Id != null && $scope.ESG_Id > 0) {
                                angular.forEach($scope.editdata, function (y) {
                                    angular.forEach($scope.getsubject, function (x) {
                                        if (y.ismS_Id == x.ismS_Id) {
                                            x.checkedsub = true;
                                        }
                                    });
                                });
                            }
                        }
                        else {
                            swal("No Subject Is Mapped For This Category");
                        }
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        //get subject
        $scope.getsubject1 = function (sub, EME_Id, Flag1) {

            if (Flag1 == "A") {
                Flag1 = "A";
            } else {
                Flag1 = "E";
            }

            $scope.getsubject = "";
            $scope.sub.checkedsub = false;
            $scope.searchchkbx = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "EME_Id": EME_Id,
                "Flag1": Flag1
            };
            apiService.create("ExamsubjectGroupMapping/getsubject", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getsubject.length > 0) {
                        $scope.getsubject = promise.getsubject;
                        if ($scope.ESG_Id != undefined && $scope.ESG_Id != "" && $scope.ESG_Id != null && $scope.ESG_Id > 0) {
                            angular.forEach($scope.editdata, function (y) {
                                angular.forEach($scope.getsubject, function (x) {
                                    if (y.ismS_Id == x.ismS_Id) {
                                        x.checkedsub = true;
                                    }
                                });
                            });
                        }
                    }
                    else {
                        swal("No Subject Is Mapped For This Category");
                    }
                } else {
                    swal("No Subject Is Mapped For This Category");
                }
            });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.getsubject.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var Compulsory1 = "";
                var Promotion1 = "";
                var examid = "";

                var ClassIDs = [];
                angular.forEach($scope.getsubject, function (hi) {
                    if (hi.checkedsub) {
                        ClassIDs.push({ ISMS_Id: hi.ismS_Id, ISMS_SubjectName: hi.ismS_SubjectName });
                    }
                });
                if ($scope.Compulsory == true) {
                    Compulsory1 = "Y";
                } else {
                    Compulsory1 = "N";
                }

                if ($scope.examtype == "0") {
                    Promotion1 = "IE";
                    examid = $scope.EME_Id;
                } else {
                    Promotion1 = "PE";
                    examid = 0;
                }
                if (ClassIDs.length <= 1) {
                    swal("Select Alteast Two Subjects For One Group");
                    return;
                }

                var data = {
                    "EME_Id": examid,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EMCA_Id": $scope.EMCA_Id,
                    "grpname": $scope.grpname,
                    "percentage": $scope.percentage,
                    "ESG_GroupMaxMarks": $scope.ESG_GroupMaxMarks,
                    "Promotion1": Promotion1,
                    "Compulsory1": Compulsory1,
                    "get_subjectlist": ClassIDs,
                    "Flag": $scope.examtype,
                    "ESG_Id": $scope.ESG_Id
                };

                apiService.create("ExamsubjectGroupMapping/savedetails", data).then(function (promise) {
                    swal(promise.message);
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //to edit individual
        $scope.Editexammasterdata1 = function (obj) {
            $scope.sub = {};
            $scope.ESG_Id = obj.esG_Id;

            var data = {
                "ESG_Id": obj.esG_Id,
                "Flag2": "N"
            };
            apiService.create("ExamsubjectGroupMapping/Editexammasterdata1", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.grpname = $scope.editdata[0].grpname;
                $scope.percentage = $scope.editdata[0].percentage;
                $scope.ESG_GroupMaxMarks = $scope.editdata[0].esG_GroupMaxMarks;
                $scope.ASMAY_Id = $scope.editdata[0].asmaY_Id;
                $scope.getcategory1($scope.sub, $scope.ASMAY_Id);
                $scope.EMCA_Id = $scope.editdata[0].emcA_Id;
                $scope.getexam1($scope.sub, $scope.EMCA_Id, 'E');
                $scope.EME_Id = $scope.editdata[0].emE_Id;
                $scope.getsubject1($scope.sub, $scope.EME_Id, 'E');
                $scope.ESG_Id = $scope.editdata[0].esG_Id;
                $scope.edit = true;

                if ($scope.editdata[0].compulsory1 == "Y") {
                    $scope.Compulsory = true;
                } else {
                    $scope.Compulsory = false;
                }
            });
        };

        $scope.Editexammasterdata2 = function (obj) {
            $scope.sub = {};
            $scope.ESG_Id = obj.esG_Id;
            var data = {
                "ESG_Id": obj.esG_Id,
                "Flag2": "P"
            };
            apiService.create("ExamsubjectGroupMapping/Editexammasterdata1", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.grpname = $scope.editdata[0].grpname;
                $scope.percentage = $scope.editdata[0].percentage;
                $scope.ESG_GroupMaxMarks = $scope.editdata[0].esG_GroupMaxMarks;
                $scope.ASMAY_Id = $scope.editdata[0].asmaY_Id;
                $scope.getcategory1($scope.sub, $scope.ASMAY_Id);
                $scope.EMCA_Id = $scope.editdata[0].emcA_Id;
                $scope.getexam1($scope.sub, $scope.EMCA_Id, 'E');
                $scope.EME_Id = $scope.editdata[0].emE_Id;
                $scope.ESG_Id = $scope.editdata[0].esG_Id;
                $scope.edit = true;

                if ($scope.editdata[0].compulsory1 == "Y") {
                    $scope.Compulsory = true;
                } else {
                    $scope.Compulsory = false;
                }
            });
        };

        //view

        $scope.viewrecordspopup = function (employee) {
            $scope.editEmployee = employee.esG_Id;
            var pageid = $scope.editEmployee;
            var yrid = employee.asmaY_Id;
            var data = {
                "ESG_Id": $scope.editEmployee,
                "Flag2": "N",
                "ASMAY_Id": yrid
            };

            apiService.create("ExamsubjectGroupMapping/viewrecordspopup", data).then(function (promise) {
                $scope.viewrecordspopupdisplay = promise.viewdata;
            });
        };

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;

        $scope.viewrecordspopup1 = function (employee) {

            $scope.editEmployee = employee.esG_Id;
            var pageid = $scope.editEmployee;
            var yrid = employee.asmaY_Id;
            var data = {
                "ESG_Id": $scope.editEmployee,
                "Flag2": "P",
                "ASMAY_Id": yrid
            };

            apiService.create("ExamsubjectGroupMapping/viewrecordspopup", data).then(function (promise) {
                $scope.viewrecordspopupdisplay = promise.viewdata;
            });
        };

        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.esgS_ActiveFlag == true) {
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
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        };
                        apiService.create("ExamsubjectGroupMapping/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                                if ($scope.examtype == "0") {
                                    $('#popup').modal('hide');
                                } else {
                                    $('#popup1').modal('hide');
                                }
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                        if ($scope.examtype == "0") {
                            $('#popup').modal('hide');
                        } else {
                            $('#popup1').modal('hide');
                        }
                    }
                });
        };


        $scope.checksubject = function (subqw) {
            angular.forEach($scope.getsubject, function (z) {
                z.checkdis = false;
            });

            if (subqw.checkedsub == true) {
                angular.forEach($scope.getsubject, function (y) {
                    if (subqw.appornonresult != y.appornonresult) {
                        y.checkdis = true;
                    } else {
                        y.checkdis = false;
                    }
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }

})();