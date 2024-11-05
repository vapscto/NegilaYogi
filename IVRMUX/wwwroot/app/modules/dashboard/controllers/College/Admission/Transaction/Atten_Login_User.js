(function () {
    'use strict';

    angular
        .module('app')
        .controller('Atten_Login_User', Atten_Login_User);

    Atten_Login_User.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function Atten_Login_User($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Atten_Login_User';

        activate();

        function activate() { }


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
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
            
            apiService.getURI("Atten_Login_User/getalldetails", 2).
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.year_list = promise.yearlist;
                    $scope.staff_list = promise.stafflist;                  
                    $scope.section_list = promise.sectionlist;
                    $scope.subject_list = promise.subjectlist;
                    $scope.grid_data = promise.saveddata;
                    $scope.searchValue = "";
                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.get_courses = function () {
            if ($scope.ASMAY_Id != undefined && $scope.ASMAY_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("Atten_Login_User/get_courses", data).then(function (promise) {
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
            if ($scope.AMCO_Id != undefined && $scope.AMCO_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                }
                apiService.create("Atten_Login_User/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                    // $scope.AMB_Id = "";
                    if (!$scope.edit_flag) {
                        $scope.AMB_Id = "";
                    }
                })
            }
            else {
                $scope.branch_list = [];
                $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            if ($scope.AMB_Id != undefined && $scope.AMB_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                }
                apiService.create("Atten_Login_User/get_semisters", data).then(function (promise) {
                    $scope.semister_list = promise.semisterlist;
                    if ($scope.edit_flag) {
                        angular.forEach($scope.semister_list, function (stu1) {
                            if (stu1.amsE_Id == $scope.Edit_AMSE_Id) {
                                stu1.checked = true;
                            }
                        })
                        $scope.optiontoggled_Sem();
                    }
                })
            }
            else {
                $scope.semister_list = [];
            }
            $scope.All_Sem = false;
        };

        $scope.toggleAll_S = function () {
            angular.forEach($scope.subject_list, function (subj) {
                subj.checked = $scope.All_S;
            })
        };

        $scope.optiontoggled_S = function () {
            $scope.All_S = $scope.subject_list.every(function (itm) { return itm.checked; });
        };
        $scope.toggleAll_Sem = function () {
            angular.forEach($scope.semister_list, function (semi) {
                semi.checked = $scope.All_Sem;
            })
        };

        $scope.optiontoggled_Sem = function () {
            $scope.All_Sem = $scope.semister_list.every(function (itm) { return itm.checked; });
        };
        $scope.toggleAll_Sec = function () {
            angular.forEach($scope.section_list, function (sec) {
                sec.checked = $scope.All_Sec;
            })
        };

        $scope.optiontoggled_Sec = function () {
            $scope.All_Sec = $scope.section_list.every(function (itm) { return itm.checked; });
        };

        $scope.selected_temp = [];
        $scope.save_temp = function () {
            // var selected_semisters = [];
            //var selected_sections = [];
            //var selected_subjects = [];
            var ASMAY_Year = "";
            var HRME_EmployeeFirstName = "";
            var AMCO_CourseName = "";
            var AMB_BranchName = "";
            if ($scope.myForm.$valid) {
                
                angular.forEach($scope.year_list, function (stu) {
                    if (stu.asmaY_Id == $scope.ASMAY_Id) {
                        ASMAY_Year = stu.asmaY_Year;
                    }
                })
                angular.forEach($scope.staff_list, function (stu1) {
                    if (stu1.hrmE_Id == $scope.HRME_Id) {
                        HRME_EmployeeFirstName = stu1.hrmE_EmployeeFirstName;
                    }
                })
                angular.forEach($scope.course_list, function (stu2) {
                    if (stu2.amcO_Id == $scope.AMCO_Id) {
                        AMCO_CourseName = stu2.amcO_CourseName;
                    }
                })
                angular.forEach($scope.branch_list, function (stu3) {
                    if (stu3.amB_Id == $scope.AMB_Id) {
                        AMB_BranchName = stu3.amB_BranchName;
                    }
                })

                if ($scope.selected_temp.length == 0) {
                    angular.forEach($scope.semister_list, function (stu4) {
                        if (stu4.checked == true) {
                            angular.forEach($scope.section_list, function (stu5) {
                                if (stu5.checked == true) {
                                    var selected_subjects = [];
                                    angular.forEach($scope.subject_list, function (stu6) {
                                        if (stu6.checked == true) {
                                            selected_subjects.push({ ISMS_Id: stu6.ismS_Id, ISMS_SubjectName: stu6.ismS_SubjectName });
                                        }
                                    })

                                    $scope.selected_temp.push({ ASMAY_Id: $scope.ASMAY_Id, ASMAY_Year: ASMAY_Year, HRME_Id: $scope.HRME_Id, HRME_EmployeeFirstName: HRME_EmployeeFirstName, AMCO_Id: $scope.AMCO_Id, AMCO_CourseName: AMCO_CourseName, AMB_Id: $scope.AMB_Id, AMB_BranchName: AMB_BranchName, AMSE_Id: stu4.amsE_Id, AMSE_SEMName: stu4.amsE_SEMName, ACMS_Id: stu5.acmS_Id, ACMS_SectionName: stu5.acmS_SectionName, subjects: selected_subjects });
                                }
                            })
                        }
                    })

                }
                else if ($scope.selected_temp.length > 0) {
                    angular.forEach($scope.semister_list, function (stu4) {
                        if (stu4.checked == true) {
                            angular.forEach($scope.section_list, function (stu5) {
                                if (stu5.checked == true) {
                                    var selected_subjects = [];
                                    angular.forEach($scope.subject_list, function (stu6) {
                                        if (stu6.checked == true) {
                                            selected_subjects.push({ ISMS_Id: stu6.ismS_Id, ISMS_SubjectName: stu6.ismS_SubjectName });
                                        }
                                    })

                                    var already_cnt = 0;
                                    angular.forEach($scope.selected_temp, function (opq) {

                                        if (opq.ASMAY_Id == $scope.ASMAY_Id && opq.HRME_Id == $scope.HRME_Id && opq.AMCO_Id == $scope.AMCO_Id && opq.AMB_Id == $scope.AMB_Id && opq.AMSE_Id == stu4.amsE_Id && opq.ACMS_Id == stu5.acmS_Id) {
                                            opq.subjects = selected_subjects;
                                            already_cnt += 1;
                                        }
                                    })
                                    if (already_cnt == 0) {

                                        $scope.selected_temp.push({ ASMAY_Id: $scope.ASMAY_Id, ASMAY_Year: ASMAY_Year, HRME_Id: $scope.HRME_Id, HRME_EmployeeFirstName: HRME_EmployeeFirstName, AMCO_Id: $scope.AMCO_Id, AMCO_CourseName: AMCO_CourseName, AMB_Id: $scope.AMB_Id, AMB_BranchName: AMB_BranchName, AMSE_Id: stu4.amsE_Id, AMSE_SEMName: stu4.amsE_SEMName, ACMS_Id: stu5.acmS_Id, ACMS_SectionName: stu5.acmS_SectionName, subjects: selected_subjects });
                                    }
                                }
                            })
                        }
                    })
                }
                $scope.clear_temp();
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clear_temp = function () {
            
            if ($scope.selected_temp.length == 0) {
                $scope.ASMAY_Id = "";
                $scope.course_list = [];
                //$scope.branch_list = [];               
                //$scope.AMCO_Id = "";
                $scope.HRME_Id = "";
            }
            $scope.AMCO_Id = "";
            $scope.branch_list = [];
            $scope.AMB_Id = "";
            angular.forEach($scope.semister_list, function (stu1) {
                stu1.checked = false;
            })
            angular.forEach($scope.section_list, function (stu2) {
                stu2.checked = false;
            })
            angular.forEach($scope.subject_list, function (stu3) {
                stu3.checked = false;
            })
            $scope.All_Sem = false;
            $scope.All_Sec = false;
            $scope.All_S = false;
            $scope.semister_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.filterValue_S = "";
            $scope.filterValue_Sem = "";
            $scope.filterValue_Sec = "";
            $scope.edit_flag = false;
            //$scope.searchValue = "";
        };

        $scope.delete = function (row, index) {
            
            for (var x = 0; x < $scope.selected_temp.length; x++) {
                if (x == index) {
                    $scope.selected_temp.splice(x, 1);
                }
            }
            $scope.clear_temp();
        }
        $scope.edit = function (row, index) {
            $scope.clear_temp();
            
            $scope.ASMAY_Id = row.ASMAY_Id;
            $scope.HRME_Id = row.HRME_Id;
            $scope.AMCO_Id = row.AMCO_Id;
            $scope.get_branches();
            $scope.AMB_Id = row.AMB_Id;
            $scope.edit_flag = true;
            $scope.Edit_AMSE_Id = row.AMSE_Id;
            $scope.get_semisters();          
            angular.forEach($scope.section_list, function (stu2) {
                if (stu2.acmS_Id == row.ACMS_Id) {
                    stu2.checked = true;
                }
            })
            angular.forEach(row.subjects, function (subj) {
                angular.forEach($scope.subject_list, function (stu3) {
                    if (subj.ISMS_Id == stu3.ismS_Id) {
                        stu3.checked = true;
                    }
                })
            })
            $scope.optiontoggled_S();
            $scope.optiontoggled_Sec();
            //$scope.optiontoggled_Sem();            
        }


        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.selected_temp.length > 0) {
                $scope.myForm.$valid = true;
            }
            if ($scope.myForm.$valid) {
                var ids = [];
                angular.forEach($scope.selected_temp, function (opq) {
                    var sub_ids = [];
                    angular.forEach(opq.subjects, function (opq_sub) {
                        sub_ids.push(opq_sub.ISMS_Id);
                    })
                    ids.push({ AMCO_Id: opq.AMCO_Id, AMB_Id: opq.AMB_Id, AMSE_Id: opq.AMSE_Id, ACMS_Id: opq.ACMS_Id, ISMS_Ids: sub_ids });
                })

                var data = {
                    //"ACALU_Id": $scope.ACALU_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "Details": ids
                }
                apiService.create("Atten_Login_User/savedata", data).then(function (promise) {
                    if (promise.returnval) {
                        if ($scope.ACALU_Id > 0) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Record Saved Successfully");
                        }
                    }
                    else if (!promise.returnval) {
                        if ($scope.ACALU_Id > 0) {
                            swal("Failed To Update Record");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.clear();
                    $scope.grid_data = promise.saveddata;
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.viewrecordspopup = function (employee) {
            
            apiService.create("Atten_Login_User/view_subjects", employee).
                    then(function (promise) {
                        
                        $scope.viewrecordspopupdisplay = promise.subjectlist;
                    })
        };

        $scope.Edit_user = function (user) {
            apiService.create("Atten_Login_User/view_subjects", user).
                 then(function (promise) {
                     
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
                     })

                     $scope.selected_temp.push({ ASMAY_Id: user.asmaY_Id, ASMAY_Year: user.asmaY_Year, HRME_Id: user.hrmE_Id, HRME_EmployeeFirstName: user.hrmE_EmployeeFirstName, AMCO_Id: user.amcO_Id, AMCO_CourseName: user.amcO_CourseName, AMB_Id: user.amB_Id, AMB_BranchName: user.amB_BranchName, AMSE_Id: user.amsE_Id, AMSE_SEMName: user.amsE_SEMName, ACMS_Id: user.acmS_Id, ACMS_SectionName: user.acmS_SectionName, subjects: selected_subjects });

                 })

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
                    apiService.create("Atten_Login_User/Deletedetails", user).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            
                            swal("Record " + dystring + "d Successfully!!!");
                        }
                        else {
                            
                            swal("Record Not " + dystring + "d Successfully!!!");
                        }
                        $scope.clear();
                        $scope.viewrecordspopup(user);
                    })
                }
                else {
                    swal("Record " + dystring.substring(0, dystring.length - 1) + "ion Cancelled!!!");
                }
            })
        };
        $scope.isOptionsRequired_S = function () {
            return !$scope.subject_list.some(function (options) {
                return options.checked;
            });
        };
        $scope.isOptionsRequired_Sem = function () {
            return !$scope.semister_list.some(function (options) {
                return options.checked;
            });
        };
        $scope.isOptionsRequired_Sec = function () {
            return !$scope.section_list.some(function (options) {
                return options.checked;
            });
        };
        $scope.clear = function () {
            $scope.ACALU_Id = 0;
            $scope.ASMAY_Id = "";
            $scope.HRME_Id = "";
            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.All_S = false;
            $scope.All_Sec = false;
            $scope.All_Sem = false;
            angular.forEach($scope.semister_list, function (stu1) {
                stu1.checked = false;
            })
            angular.forEach($scope.section_list, function (stu2) {
                stu2.checked = false;
            })
            angular.forEach($scope.subject_list, function (stu3) {
                stu3.checked = false;
            })
            $scope.selected_temp = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.filterValue_S = "";
            $scope.filterValue_Sem = "";
            $scope.filterValue_Sec = "";
            $scope.edit_flag = false;
            $scope.searchValue = "";
        };
    }
})();
