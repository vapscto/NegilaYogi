(function () {
    'use strict';

    angular.module('app').controller('Atten_Batch_Mapping', Atten_Batch_Mapping);

    Atten_Batch_Mapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function Atten_Batch_Mapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Atten_Batch_Mapping';

        activate();

        function activate() { }


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
            else {
                paginationformasters = 10;
            }
        }
        else {
            paginationformasters = 10;
        }

        $scope.BindData = function () {

            apiService.getURI("Atten_Batch_Mapping/getalldetails", 2).then(function (promise) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.year_list = promise.yearlist;
                $scope.batch_list = promise.batchlist;
                $scope.section_list = promise.sectionlist;
                $scope.subject_list = promise.subjectlist;

                angular.forEach($scope.subject_list, function (dd) {
                    dd.ismS_SubjectName = dd.ismS_SubjectName + ':' + dd.ismS_SubjectCode;
                });

                $scope.grid_data = promise.saveddata;
                $scope.searchValue1 = "";
                $scope.searchValue2 = "";
                $scope.student_list = [];
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };



        $scope.clear1 = function () {
            $scope.ACAB_BatchName = "";
            $scope.ACAB_StudentStrength = "";
            $scope.ACAB_Id = 0;
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.searchValue1 = "";
        };

        $scope.submitted1 = false;
        $scope.savedata1 = function () {
            if ($scope.myForm1.$valid) {
                var data = {
                    "ACAB_Id": $scope.ACAB_Id,
                    "ACAB_BatchName": $scope.ACAB_BatchName,
                    "ACAB_StudentStrength": $scope.ACAB_StudentStrength
                };
                apiService.create("Atten_Batch_Mapping/savedata1", data).then(function (promise) {
                    if (promise.returnval) {
                        if ($scope.ACAB_Id > 0) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Record Saved Successfully");
                        }
                    }
                    else if (promise.returnduplicatestatus) {
                        swal('Record Already Exist');
                    }
                    else if (!promise.returnval) {
                        if ($scope.ACAB_Id > 0) {
                            swal("Failed To Update Record");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.clear1();
                    $scope.batch_list = promise.batchlist;
                });
                //$scope.loadData();
                // $scope.clear1();
            }
            else {
                $scope.submitted1 = true;
            }

        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.Edit1 = function (user) {
            $scope.clear1();
            $scope.ACAB_Id = user.acaB_Id;
            $scope.ACAB_BatchName = user.acaB_BatchName;
            $scope.ACAB_StudentStrength = user.acaB_StudentStrength;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.get_courses = function () {
            $scope.student_list = [];
            if ($scope.ASMAY_Id !== undefined && $scope.ASMAY_Id !== null && $scope.ASMAY_Id !== "") {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id
                };
                apiService.create("Atten_Batch_Mapping/get_courses", data).then(function (promise) {
                    $scope.course_list = promise.courselist;
                    $scope.AMCO_Id = "";
                });
            }
            else {
                $scope.course_list = [];
                $scope.AMCO_Id = "";
            }

        };

        $scope.get_branches = function () {
            $scope.student_list = [];
            if ($scope.AMCO_Id !== undefined && $scope.AMCO_Id !== null && $scope.AMCO_Id !== "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                };
                apiService.create("Atten_Batch_Mapping/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                    $scope.AMB_Id = "";

                });
            }
            else {
                $scope.branch_list = [];
                $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            $scope.student_list = [];
            if ($scope.AMB_Id !== undefined && $scope.AMB_Id !== "" && $scope.AMB_Id !== null) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("Atten_Batch_Mapping/get_semisters", data).then(function (promise) {
                    $scope.semister_list = promise.semisterlist;
                    $scope.AMSE_Id = "";
                });
            }
            else {
                $scope.semister_list = [];
                $scope.AMSE_Id = "";
            }
        };

        $scope.get_students = function () {
            $scope.student_list = [];
            if ($scope.myForm2.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ACAB_Id": $scope.acaB_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id
                };

                apiService.create("Atten_Batch_Mapping/get_students", data).then(function (promise) {
                    $scope.student_list = promise.studentlist;
                    $scope.all = false;
                    angular.forEach(promise.saveddata, function (stud_S) {
                        angular.forEach($scope.student_list, function (stud) {
                            if (stud_S == stud.amcsT_Id) {
                                stud.xyz = true;
                            }
                        });
                    });
                    $scope.optionToggled();
                    $scope.saveddata = promise.saveddata;
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.toggleAll = function () {
            angular.forEach($scope.student_list, function (stud) {
                stud.xyz = $scope.all;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.student_list.every(function (itm) { return itm.xyz; });
        };

        $scope.submitted2 = false;
        $scope.savedata2 = function () {

            if ($scope.myForm2.$valid) {
                var Batch_Length = 0;
                var ids = [];
                angular.forEach($scope.student_list, function (stud) {
                    if (stud.xyz) {
                        ids.push(stud.amcsT_Id);
                    }
                });
                if (ids.length > 0) {
                    angular.forEach($scope.batch_list, function (batch) {
                        if (batch.acaB_Id == $scope.acaB_Id) {
                            Batch_Length = batch.acaB_StudentStrength;
                        }
                    })
                    if (ids.length <= Batch_Length) {
                        var data = {
                            // "ACASMP_Id": $scope.ACASMP_Id,
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "ACAB_Id": $scope.acaB_Id,
                            "AMCO_Id": $scope.AMCO_Id,
                            "AMB_Id": $scope.AMB_Id,
                            "AMSE_Id": $scope.AMSE_Id,
                            "ACMS_Id": $scope.ACMS_Id,
                            "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                            "sub_data": ids
                        };

                        apiService.create("Atten_Batch_Mapping/savedata2", data).then(function (promise) {
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
                            $scope.clear2();
                        });
                    }
                    else {
                        swal("Can't Map Students More Than Batch Strength !!!  " + Batch_Length);
                    }
                }
                else {
                    swal("Select Atleast Student To Save Data!!!");
                }
            }
            else {
                $scope.submitted2 = true;
            }
        };


        $scope.viewrecordspopup = function (employee) {

            apiService.create("Atten_Batch_Mapping/view_subjects", employee).then(function (promise) {

                $scope.viewrecordspopupdisplay = promise.subjectlist;
            });
        };

        $scope.Edit_user = function (user) {
            apiService.create("Atten_Batch_Mapping/view_subjects", user).then(function (promise) {

                //$scope.viewrecordspopupdisplay = promise.subjectlist;

                $scope.ACALU_Id = user.acalU_Id;
                $scope.ASMAY_Id = user.asmaY_Id;
                $scope.get_courses();
                $scope.HRME_Id = user.hrmE_Id;


                $scope.selected_temp = [];
                var selected_subjects = [];

                angular.forEach(promise.subjectlist, function (subj_saved) {
                    if (subj_saved.acalD_ActiveFlag) {
                        selected_subjects.push({ ISMS_Id: subj_saved.ismS_Id, ISMS_SubjectName: subj_saved.ismS_SubjectName });
                    }
                });

                $scope.selected_temp.push({
                    ASMAY_Id: user.asmaY_Id, ASMAY_Year: user.asmaY_Year, HRME_Id: user.hrmE_Id, HRME_EmployeeFirstName: user.hrmE_EmployeeFirstName,
                    AMCO_Id: user.amcO_Id, AMCO_CourseName: user.amcO_CourseName, AMB_Id: user.amB_Id, AMB_BranchName: user.amB_BranchName,
                    AMSE_Id: user.amsE_Id, AMSE_SEMName: user.amsE_SEMName, ACMS_Id: user.acmS_Id, ACMS_SectionName: user.acmS_SectionName,
                    subjects: selected_subjects
                });
            });

        };
        $scope.deactivate = function (user, SweetAlert) {

            var dystring = "";
            if (user.acalD_ActiveFlag) {
                dystring = "Deactivate";
            }
            else if (!user.acalD_ActiveFlag) {
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
                        apiService.create("Atten_Batch_Mapping/Deletedetails", user).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + dystring + "d Successfully!!!");
                            }
                            else {
                                swal("Record Not " + dystring + "d Successfully!!!");
                            }
                            $scope.clear();
                            $scope.viewrecordspopup(user);
                        });
                    }
                    else {
                        swal("Record " + dystring.substring(0, dystring.length - 1) + "ion Cancelled!!!");
                    }
                });
        };


        $scope.clear2 = function () {
            $scope.ACABS_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.acaB_Id = "";
            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];
            $scope.student_list = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_Id = "";
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.searchValue2 = "";
        };
    }
})();
