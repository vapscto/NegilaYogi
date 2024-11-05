(function () {
    'use strict';

    angular
        .module('app')
        .controller('Atten_Subject_MaxPeriod', Atten_Subject_MaxPeriod);

    Atten_Subject_MaxPeriod.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function Atten_Subject_MaxPeriod($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Atten_Subject_MaxPeriod';

        activate();

        function activate() { }


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
        }
        else {
            paginationformasters = 10;
        }

        $scope.BindData = function () {
            
            apiService.getURI("Atten_Subject_MaxPeriod/getalldetails", 2).
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.year_list = promise.yearlist;
                    $scope.section_list = promise.sectionlist;
                    $scope.subject_list = [];
                    if (promise.alldetails.length > 0) {
                        $scope.alldetails = promise.alldetails;
                        $scope.grid_data = promise.alldetails.length;
                    } else {
                        swal("No Records Found");
                    }

                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.get_courses = function () {
            $scope.subject_list = [];
            if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("Atten_Subject_MaxPeriod/get_courses", data).then(function (promise) {
                    $scope.course_list = promise.courselist;
                    $scope.AMCO_Id = "";
                })
            }
            else {
                $scope.course_list = [];
                $scope.AMCO_Id = "";
            }

        };

        $scope.get_branches = function () {
            $scope.subject_list = [];
            if ($scope.AMCO_Id != undefined && $scope.AMCO_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                }
                apiService.create("Atten_Subject_MaxPeriod/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                    $scope.AMB_Id = "";
                })
            }
            else {
                $scope.branch_list = [];
                $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            $scope.subject_list = [];
            if ($scope.AMB_Id != undefined && $scope.AMB_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                }
                apiService.create("Atten_Subject_MaxPeriod/get_semisters", data).then(function (promise) {
                    $scope.semister_list = promise.semisterlist;
                    $scope.AMSE_Id = "";
                })
            }
            else {
                $scope.semister_list = [];
                $scope.AMSE_Id = "";
            }
        };

        $scope.get_subjects = function () {
            $scope.subject_list = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id
                }
                apiService.create("Atten_Subject_MaxPeriod/get_subjects", data).then(function (promise) {
                    $scope.subject_list = promise.subjectlist;
                    $scope.All_S = false;
                    angular.forEach($scope.subject_list, function (subj) {
                        angular.forEach(promise.saveddata, function (subj_S) {
                            if (subj_S.ismS_Id == subj.ismS_Id) {
                                subj.xyz = true;
                                subj.ACASMP_MaxPeriod = subj_S.acasmP_MaxPeriod;
                            }
                        })
                    })
                    $scope.optionToggled();
                    $scope.saveddata = promise.saveddata;
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.toggleAll = function () {
            angular.forEach($scope.subject_list, function (subj) {
                subj.xyz = $scope.all;
            })
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.subject_list.every(function (itm) { return itm.xyz; });
        };

        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var ids = [];
                angular.forEach($scope.subject_list, function (subj) {
                    if (subj.xyz) {
                        ids.push({ ISMS_Id: subj.ismS_Id, ACASMP_MaxPeriod: subj.ACASMP_MaxPeriod });
                    }
                })
                if (ids.length > 0) {
                    var data = {
                        // "ACASMP_Id": $scope.ACASMP_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id,
                        "sub_data": ids
                    }
                    apiService.create("Atten_Subject_MaxPeriod/savedata", data).then(function (promise) {
                        if (promise.returnval) {
                            if ($scope.saveddata.length > 0) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Record Saved Successfully");
                            }
                        }
                        else if (!promise.returnval) {
                            if ($scope.saveddata.length > 0) {
                                swal("Failed To Update Record");
                            } else {
                                swal("Failed To Save Record");
                            }
                        }
                        else {
                            swal("Something Went Wrong Please Contact Administrator");
                        }
                        $scope.clear();
                    })
                }
                else {
                    swal("Select Atleast Subject To Save Data!!!");
                }

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Edit = function (user) {
            $scope.ACASMP_Id = user.acasmP_Id;
            $scope.ASMAY_Id = user.asmaY_Id;
            $scope.get_courses();
            $scope.AMCO_Id = user.amcO_Id;
            $scope.get_branches();
            $scope.AMB_Id = user.amB_Id;
            $scope.get_semisters();
            $scope.AMSE_Id = user.amsE_Id;
            $scope.ACMS_Id = user.acmS_Id;
            $scope.ISMS_Id = user.ismS_Id;
            $scope.get_subjects();
            $scope.ACASMP_MaxPeriod = user.acasmP_MaxPeriod;
        };

        $scope.deactivate = function (user, SweetAlert) {
            
            var dystring = "";
            if (user.acasmP_ActiveFlag) {
                dystring = "Deactivate";
            }
            else if (!user.acasmP_ActiveFlag) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("Atten_Subject_MaxPeriod/Deletedetails", user).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            
                            swal("Record " + dystring + "d Successfully!!!");
                        }
                        else {
                            
                            swal("Record Not " + dystring + "d Successfully!!!");
                        }
                        $state.reload();
                        $scope.clear();
                    })
                }
                else {
                    swal("Record " + dystring.substring(0, dystring.length - 1) + "ion Cancelled!!!");
                }
            })
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.subject_list.some(function (options) {
                return options.checked;
            });
        };

        $scope.clear = function () {
            $scope.ACASMP_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.all = false;
            $scope.subject_list = [];
            $scope.saveddata = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.search = "";
        $scope.filtervalue = function (obj) {
            return (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.amB_BranchName)).indexOf($scope.search) >= 0 ||
            (JSON.stringify(obj.amsE_SEMName)).indexOf($scope.search) >= 0;
        }

        $scope.showmodaldetails = function (user) {
            apiService.create("Atten_Subject_MaxPeriod/showmodaldetails", user).then(function (promise) {
                $scope.alldetailsshow = promise.alldetailsshow;
            })
        }

        $scope.deactivesem = function (usersem, SweetAlert) {
            
            var dystring = "";
            if (usersem.acasmP_ActiveFlag) {
                dystring = "Deactivate";
            }
            else if (!usersem.acasmP_ActiveFlag) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.create("Atten_Subject_MaxPeriod/deactivesem", usersem).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            
                            swal("Record " + dystring + "Successfully!!!");
                        }
                        else {
                            
                            swal("Record Not " + dystring + "Successfully!!!");
                        }
                        $state.reload();                        
                    })
                }
                else {
                    swal("Record Cancelled");
                }
            })
        }

    }
})();
