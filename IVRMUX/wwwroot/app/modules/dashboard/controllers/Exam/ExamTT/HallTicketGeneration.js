(function () {
    'use strict';
    angular.module('app').controller('HallTicketGenerationController', HallTicketGenerationController)
    HallTicketGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HallTicketGenerationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.all123 = false;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("HallTicketGeneration/getdetails", pageid).then(function (promise) {
                $scope.acdlist = promise.acdlist;               
                $scope.alldata = promise.alldata;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.onselectAcdYear($scope.ASMAY_Id);
                $scope.gridOptions.data = $scope.alldata;
            });
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'asmcL_ClassName', displayName: 'Class' },
                { name: 'asmC_SectionName', displayName: 'Section' },
                { name: 'emE_ExamName', displayName: 'Exam' },
                {
                    field: 'id', name: '',
                    displayName: 'Student List', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" title="View Student List" ng-click="grid.appScope.ViewStudentDetails(row.entity);"> <i class="fa fa-eye text-purple" ></i></a></div >'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("HallTicketGeneration/onselectAcdYear", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;
            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("HallTicketGeneration/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };

        $scope.onselectsection = function () {
            $scope.tempsection = [];
            $scope.examlist = [];
            $scope.EME_Id = "";

            angular.forEach($scope.seclist, function (dd) {
                if (dd.section) {
                    $scope.tempsection.push({ ASMS_Id: dd.asmS_Id });
                }
            });

            if ($scope.tempsection.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "section_Array": $scope.tempsection
                };

                apiService.create("HallTicketGeneration/onselectsection", data).then(function (promise) {
                    $scope.examlist = promise.examlist;
                });
            }

        };

        $scope.submitted = false;
        $scope.savedetail = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.checkseclist = [];
                angular.forEach($scope.seclist, function (option3) {
                    if (option3.section === true) $scope.checkseclist.push(option3);
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "section_Array": $scope.checkseclist,
                    "EME_Id": $scope.EME_Id,
                    "prefixstr": $scope.prefix,
                    "startno": $scope.startno,
                    "increment": $scope.increment,
                    "leadingzeros": $scope.preno
                };

                apiService.create("HallTicketGeneration/savedetail", data).then(function (promise) {
                    if (promise.returnval === true) {

                        swal('Data Saved successfully');
                        $scope.cancel();
                        $scope.BindData();
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Records Already Exist !');
                    }
                    else {
                        swal('Data Not Saved !');
                    }
                });
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.seclist.some(function (option3) {
                return option3.asmS_Id;
            });
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.prefix = '';
            $scope.startno = '';
            $scope.increment = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.ViewStudentDetails = function (viweobj) {

            $scope.yearname = "";
            $scope.classname = "";
            $scope.sectioname = "";
            $scope.examname = "";
            $scope.datareport = [];

            var data = {
                "ASMCL_Id": viweobj.asmcL_Id,
                "ASMS_Id": viweobj.asmS_Id,
                "ASMAY_Id": viweobj.asmaY_Id,
                "EME_Id": viweobj.emE_Id
            };

            apiService.create("HallTicketGeneration/ViewStudentDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.datareport = promise.datareport;
                    if ($scope.datareport !== null && $scope.datareport.length > 0) {
                        angular.forEach($scope.datareport, function (dd) {
                            dd.checkedvalue = false;
                        });
                        $scope.all123 = false;
                        //$scope.qualification_type = "1";
                        $scope.yearname = viweobj.asmaY_Year;
                        $scope.classname = viweobj.asmcL_ClassName;
                        $scope.sectioname = viweobj.asmC_SectionName;
                        $scope.examname = viweobj.emE_ExamName;

                        $('#popup4546').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.SaveStudentStatus = function (saveobj) {

            $scope.selectedstudents = [];

            angular.forEach($scope.datareport, function (dd) {
                if (dd.checkedvalue === true) {
                    $scope.selectedstudents.push({ AMST_Id: dd.amsT_Id, EHT_Id: dd.ehT_Id });
                }
            });

            if ($scope.selectedstudents.length > 0) {
                var data = {
                    "selectedstudents": $scope.selectedstudents
                    //"statusflag": saveobj
                };
                apiService.create("HallTicketGeneration/SaveStudentStatus", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Records Updated");                        
                        $('#popup4546').modal('hide');
                    } else {
                        swal("Failed To Update Record");
                    }
                });
            } else {
                swal("Select The Students To Change The Status");
            }
        };

        $scope.optionToggled123 = function () {
            $scope.all123 = $scope.datareport.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.toggleAll123 = function () {
            angular.forEach($scope.datareport, function (dd) {
                dd.checkedvalue = $scope.all123;
            });
        };
    }
})();