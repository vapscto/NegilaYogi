(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgSeatDistributionController', ClgSeatDistributionController)

    ClgSeatDistributionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgSeatDistributionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'pamS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;
            var pageid = 2;
            apiService.getDATA("ClgSeatDistribution/getexamdetails", pageid).
                then(function (promise) {
                    $scope.examdetailsarray = promise.examdetailsarray;
                    $scope.subdetailsarray = promise.subdetailsarray;
                    $scope.presentCountgrid = $scope.examdetailsarray.length;
                    $scope.presentCountgrid1 = $scope.subdetailsarray.length;
                    clearfields();
                    clearfields_sub();
                })

        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchsource = function () {
            var data = {
                "PAMS_SourceName": $scope.search,
                "PAMS_SourceDesc": $scope.type
            }
            apiService.create("CLGStatus/1", data).
                then(function (promise) {
                    $scope.pages = promise.pagesdata;
                })
        }

        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.amcexM_CompetitiveExams)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcexM_CompulsoryFlg)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.filterValue2 = function (obj) {

            return (angular.lowercase(obj.amcexM_CompetitiveExams)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (angular.lowercase(obj.amcexmsuB_SubjectName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0
        }


        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.amcexM_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ClgSeatDistribution/deleterecord", pageid).
                            then(function (promise) {
                                if (promise.returnMsg == "Delete") {
                                    swal('You Can Not Delete This Record. It Is Already Mapped With Student');
                                }
                                else {
                                    if (promise.returnval === true) {
                                        swal('Record Deleted Successfully');

                                    }
                                    else {
                                        swal('Record Not Deleted Successfully');
                                    }
                                }
                                $scope.pages = promise.pagesdata;


                                $state.reload();
                            })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.deletedatasec = function (employees, SweetAlert) {
            $scope.editEmployee = employees.amcexmsuB_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ClgSeatDistribution/deleterecordsub", pageid).
                            then(function (promise) {
                                if (promise.returnMsg == "Delete") {
                                    swal('You Can Not Delete This Record. It Is Already Mapped With Student');
                                }
                                else {
                                    if (promise.returnval === true) {
                                        swal('Record Deleted Successfully');
                                        $scope.loaddata();
                                    }
                                    else {
                                        swal('Record Not Deleted Successfully');
                                        $scope.loaddata();
                                    }
                                }
                              
                            })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.amcexM_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ClgSeatDistribution/getexamedit", pageid).
                then(function (promise) {
                    if (promise.pagesdata.length > 0) {
                        $scope.AMCEXM_Id = promise.pagesdata[0].amcexM_Id;
                        $scope.AMCEXM_CompetitiveExams = promise.pagesdata[0].amcexM_CompetitiveExams;
                        // $scope.PAMCEXM_CompulsoryFlg = promise.pagesdata[0].pamcexM_CompulsoryFlg;
                        if (promise.pagesdata[0].amcexM_CompulsoryFlg == 1) {
                            $scope.AMCEXM_CompulsoryFlg = true;

                        }
                        else {
                            $scope.AMCEXM_CompulsoryFlg = false;
                        }
                    }
                  


                })
        }

        $scope.getorgvaluesec = function (rec) {
            $scope.editEmployee = rec.amcexmsuB_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ClgSeatDistribution/getsubedit", pageid).
                then(function (promise) {
                    if (promise.pagesdata.length > 0) {
                        $scope.amcexmsuB_Id = promise.pagesdata[0].amcexmsuB_Id;
                        $scope.amcexM_Id = promise.pagesdata[0].amcexM_Id;
                        $scope.AMCEXMSUB_SubjectName = promise.pagesdata[0].amcexmsuB_SubjectName;
                        $scope.AMCEXMSUB_MaxMarks = promise.pagesdata[0].amcexmsuB_MaxMarks;
                    }
                 
                })
        }

        $scope.clearfields = function () {

            $scope.AMCEXM_CompetitiveExams = "";
            $scope.AMCEXM_CompulsoryFlg = false;

        }
        $scope.clearfields_sub = function () {

            $scope.amcexM_Id = "";
            $scope.AMCEXMSUB_SubjectName = "";
            $scope.AMCEXMSUB_MaxMarks = "";

        }
        $scope.clearall = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.savepages = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMCEXM_CompetitiveExams": $scope.AMCEXM_CompetitiveExams,
                    "AMCEXM_CompulsoryFlg": $scope.AMCEXM_CompulsoryFlg,
                    "AMCEXM_Id": $scope.AMCEXM_Id
                 
                }
                apiService.create("ClgSeatDistribution/saveExamDetails", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.returnMsg == "duplicate") {
                                swal('Master Source Record Already Exist');
                                $state.reload();
                                return;

                            } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully');
                                $scope.AMCEXMSUB_SubjectName = "";
                                $scope.AMCEXMSUB_MaxMarks = "";
                                $state.reload();
                                clearfields();
                               // clearfields_sub();


                            } else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                clearfields();
                               // clearfields_sub();
                            }
                        } else {
                            swal('Failed To Save/Update Record');
                            $state.reload();
                        }
                    })
            }


        };

        //firsttab
        $scope.submittedsec = false;
        $scope.saveSecondtab = function () {
            $scope.submittedsec = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    
                    "AMCEXMSUB_SubjectName": $scope.AMCEXMSUB_SubjectName,
                    "AMCEXM_Id": $scope.amcexM_Id,
                    "AMCEXMSUB_Id": $scope.amcexmsuB_Id,
                    "AMCEXMSUB_MaxMarks": $scope.AMCEXMSUB_MaxMarks

                }
                apiService.create("ClgSeatDistribution/saveExamMapDetails", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.returnMsg == "duplicate") {
                                swal('Master Source Record Already Exist');
                                $state.reload();
                                return;

                            } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully');
                               // $state.reload();
                                $scope.loaddata();
                               // clearfields();
                                clearfields_sub();

                            } else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully');
                               // $state.reload();
                                $scope.loaddata();
                               // clearfields();
                                clearfields_sub();
                            }
                        } else {
                            swal('Failed To Save/Update Record');
                            $state.reload();
                        }
                    })
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interactedsec = function (field) {
            return $scope.submittedsec || field.$dirty;
        };

       // $scope.clearfields_sub = function (field) {

    }

})();