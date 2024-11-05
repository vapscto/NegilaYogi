(function () {
    'use strict';

    angular
        .module('app')
        .controller('Promotion_Marks_Update', Promotion_Marks_Update);

    Promotion_Marks_Update.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter'];

    function Promotion_Marks_Update($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'Promotion_Marks_Update';

        activate();

        function activate() { }

        $scope.BindData = function () {
            
            apiService.getDATA("Promotion_Marks_Update/Getdetails").
       then(function (promise) {
           $scope.year_list = promise.yearlist;
           // $scope.category_list = promise.categorylist;
       })
        };
        $scope.get_categories = function () {
            

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Promotion_Marks_Update/get_categories", data).then(function (promise) {
                $scope.category_list = promise.categorylist;
                $scope.EMCA_Id = "";
                $scope.student_list = [];

            })

        };
        $scope.get_classes = function () {
            

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Promotion_Marks_Update/get_classes", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.ASMCL_Id = "";
            })

        };
        $scope.get_sections = function () {
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Promotion_Marks_Update/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.ASMS_Id = "";
            })
        };
        $scope.get_subjects = function () {
            

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Promotion_Marks_Update/get_subjects", data).then(function (promise) {
                $scope.subject_list = promise.subjectlist;
                $scope.ISMS_Id = "";
                $scope.student_list = [];
                if (promise.subjectlist == null || promise.subjectlist.length == 0) {
                    swal("Promtion Subjects Are Not Mapped");
                    $scope.ASMS_Id = "";
                }
            })

        };
        $scope.submitted = false;
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.get_prommarks = function () {
            

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ISMS_Id": $scope.ISMS_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Promotion_Marks_Update/get_prommarks", data).then(function (promise) {
                $scope.prom_subj_groups_details = promise.prom_subj_groups;
                $scope.student_list = promise.prom_students;
                $scope.prom_subj_marks = promise.prom_subjmarks;
                $scope.prom_subj_group_marks = promise.prom_subj_groupmarks;

                if (promise.prom_students != null && promise.prom_students.length > 0) {
                    angular.forEach($scope.student_list, function (stud) {
                        stud.Marks = [];
                        angular.forEach($scope.prom_subj_marks, function (stu_marks) {
                            if (stu_marks.amsT_Id == stud.amsT_Id) {
                                angular.forEach($scope.prom_subj_group_marks, function (stu_grp_marks) {
                                    if (stu_grp_marks.estmppS_Id == stu_marks.estmppS_Id) {
                                        stud.Marks[stu_grp_marks.empsG_Id] = {};
                                        stud.Marks[stu_grp_marks.empsG_Id].ObtMarks = stu_grp_marks.estmppsG_GroupObtMarks;
                                        stud.Marks[stu_grp_marks.empsG_Id].MaxMarks = stu_grp_marks.estmppsG_GroupMaxMarks;
                                    }
                                })
                                stud.ESTMPPS_ObtainedMarks = stu_marks.estmppS_ObtainedMarks;
                                stud.ESTMPPS_MaxMarks = stu_marks.estmppS_MaxMarks;
                            }
                        })

                    })
                    angular.forEach($scope.prom_subj_groups_details, function (grp) {
                        grp.Group_MaxMarks = $scope.student_list[0].Marks[grp.empsG_Id].MaxMarks;
                    })
                } else {
                    swal("For Selected Details Promotion Calculation Not Done");
                }

            })

        };

        $scope.toggleAll = function () {
            
            var toggleStatus = $scope.all;
            angular.forEach($scope.student_list, function (itm) {
                itm.xyz = toggleStatus;

            });
        }

        $scope.optionToggled = function () {
            $scope.all = $scope.student_list.every(function (itm) { return itm.xyz; })
        }

        var Temp_subs_marks_list = [];
        $scope.valid_marks_G = function (grp, empsG_Id, Marks, amsT_Id, stud) {
            var obtainmarks = Marks[empsG_Id].ObtMarks;
            if (obtainmarks != undefined && obtainmarks != null && obtainmarks != "") {               
                    var totalMarks = 0;
                    totalMarks = Number(Marks[empsG_Id].MaxMarks);
                    obtainmarks = Number(obtainmarks);
                    if (totalMarks < obtainmarks) {
                        Marks[empsG_Id].ObtMarks = "";
                        swal('Entered marks cant be more than Max Marks ...!' + totalMarks);
                    }
                    else if (obtainmarks < 0) {
                        Marks[empsG_Id].ObtMarks = "";
                        swal('Entered marks cant be in nagative values...!');
                    }
                    else {
                        if (Temp_subs_marks_list.length > 0) {
                            var al_cnt = 0;
                            angular.forEach(Temp_subs_marks_list, function (yet) {
                                if (yet.AMST_Id == amsT_Id && yet.EMPSG_Id == empsG_Id) {//&& yet.EMSE_Id == s_exm_id
                                    yet.ESTMPPSG_GroupObtMarks = obtainmarks;                                  
                                    al_cnt += 1;
                                    stud.ESTMPPS_ObtainedMarks = 0;
                                    angular.forEach(Marks, function (obt_mks) {
                                        if (obt_mks.ObtMarks != "" && obt_mks.ObtMarks != null && obt_mks.ObtMarks != undefined) {
                                            // stud.ESTMPPS_ObtainedMarks += Number(obt_mks.ObtMarks);
                                            stud.ESTMPPS_ObtainedMarks += Number($filter('number')(Number(obt_mks.ObtMarks), 2));
                                        }
                                    })
                                }
                            })
                            if (al_cnt == 0) {
                                Temp_subs_marks_list.push({ AMST_Id: amsT_Id, EMPSG_Id: empsG_Id, ESTMPPSG_GroupObtMarks: obtainmarks });
                                stud.ESTMPPS_ObtainedMarks = 0;
                                angular.forEach(Marks, function (obt_mks) {
                                    if (obt_mks.ObtMarks != "" && obt_mks.ObtMarks != null && obt_mks.ObtMarks != undefined) {
                                        // stud.ESTMPPS_ObtainedMarks += Number(obt_mks.ObtMarks);
                                        stud.ESTMPPS_ObtainedMarks += Number($filter('number')(Number(obt_mks.ObtMarks), 2));
                                    }
                                })
                            }
                        }
                        else if (Temp_subs_marks_list.length == 0) {
                            Temp_subs_marks_list.push({ AMST_Id: amsT_Id, EMPSG_Id: empsG_Id, ESTMPPSG_GroupObtMarks: obtainmarks });
                            stud.ESTMPPS_ObtainedMarks = 0;
                            angular.forEach(Marks, function (obt_mks) {
                                if (obt_mks.ObtMarks != "" && obt_mks.ObtMarks != null && obt_mks.ObtMarks != undefined) {
                                    // stud.ESTMPPS_ObtainedMarks += Number(obt_mks.ObtMarks);
                                    stud.ESTMPPS_ObtainedMarks += Number($filter('number')(Number(obt_mks.ObtMarks), 2));
                                }
                            })
                        }
                    }               
            }
        }

        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.selectedstudent = [];
                angular.forEach($scope.student_list, function (stud) {
                    if (stud.xyz == true) {
                        $scope.selectedstudent.push(stud);
                    }
                })
                if ($scope.selectedstudent.length > 0) {

                }
                else if ($scope.selectedstudent.length == 0) {
                    swal("Select Atleast One Student ");
                }
            }
        };
    }
})();
