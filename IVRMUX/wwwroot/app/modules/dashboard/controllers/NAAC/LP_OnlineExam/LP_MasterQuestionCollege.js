

(function () {
    'use strict';
    angular
.module('app')
        .controller('MasterQuestionCollegeController', MasterQuestionCollegeController)

    MasterQuestionCollegeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function MasterQuestionCollegeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
               { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
               { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
               { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.answer = "";
        $scope.show_ansOption = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterQuestionCollege/getloaddata", pageid).
            then(function (promise) {
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.getQuestiondetails = promise.getQuestiondetails;
                $scope.presentCountgrid = $scope.getQuestiondetails.length;
                $scope.result = promise.result;
                $scope.getclass = promise.getclass;
                $scope.getSubjects = promise.getSubjects;
                $scope.getAnsOptions = promise.getAnsOptions;
                $scope.coursecount = promise.courselist;
                $scope.getFQuestiondetails = promise.getFQuestiondetails;


                $scope.getFQOptiondetails = promise.getFQOptiondetails;
                $scope.test = promise.getQuestiondetails;
                $scope.Option_Qus = [];
                $scope.presentCountgrid2 = promise.result.length;
                $scope.presentCountgrid3 = promise.getFQOptiondetails.length;


                //angular.forEach($scope.getFQOptiondetails, function (Qus_op) {
                //    angular.forEach($scope.test, function (Qus) {
                //        if (Qus.LMSMOEQ_Id == Qus_op.LMSMOEQ_Id) {
                //            var index = $scope.test.indexOf(Qus);
                //            $scope.test.splice(index, 1);
                //        }
                //    })

                //})
            })
        };


        //------------------1st Tab 
        $scope.savedata = function () {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMSMOEQ_Question": $scope.LMSMOEQ_Question,
                    "LMSMOEQ_QuestionDesc": $scope.LMSMOEQ_QuestionDesc,
                    "LMSMOEQ_Marks": $scope.LMSMOEQ_Marks,
                    "LMSMOEQ_Id": $scope.LMSMOEQ_Id,
                    "ISMS_Id": $scope.ismS_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("MasterQuestionCollege/savedetails", data).
                             then(function (promise) {

                                 if (promise.returnval == true) {
                                     if (promise.LMSMOEQ_Id == 0 || promise.LMSMOEQ_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                     else if (promise.LMSMOEQ_Id > 0) {
                                         swal('Record updated successfully');
                                     }
                                 }
                                 else if (promise.returnduplicatestatus == 'Duplicate') {
                                     swal('Record already exist');
                                 }
                                 else {
                                     if (promise.LMSMOEQ_Id == 0 || promise.LMSMOEQ_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.LMSMOEQ_Id > 0) {
                                         swal('Failed to update, please contact administrator');
                                     }
                                 }
                                 $state.reload();
                             })
            }
            else {
                $scope.submitted1 = true;
            }
        };

        //2nd tab save

        $scope.savedatabranch = function (id) {
            $scope.submitted2 = true;
            $scope.submitted1 = false;
            $scope.submitted3 = false;
            debugger;
            if ($scope.myForm2.$valid) {

                var AMCO_Ids = [];
                var AMB_Ids = [];
                var AMSE_Ids = [];
                angular.forEach($scope.coursecount, function (ty) {
                    if (ty.selectedcourse) {
                        AMCO_Ids.push(ty.amcO_Id);
                    }
                })
                angular.forEach($scope.branchcount, function (ty) {
                    if (ty.selectedbranch) {
                        AMB_Ids.push(ty.amB_Id);
                    }
                })
                angular.forEach($scope.arrlistchkgroup, function (ty) {
                    if (ty.selected) {
                        AMSE_Ids.push(ty.amsE_Id);
                    }
                })





                var data = {
                    "LMSMOEQ_Question": $scope.LMSMOEQ_Id.lmsmoeQ_Question,
                    "LMSMOEQ_Id": $scope.LMSMOEQ_Id.lmsmoeQ_Id,
                    "LMSMOEQB_Id": $scope.LMSMOEQB_Id,
                    AMCO_Ids: AMCO_Ids,
                    AMB_Ids: AMB_Ids,
                    AMSE_Ids: AMSE_Ids
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                debugger;
                apiService.create("MasterQuestionCollege/savedataclass", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            if (promise.lmsmoeQ_Id >0 ) {
                                swal('Record saved successfully');
                            }
                            else {
                                swal('Record saved successfully');
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.LMSMOEQ_Id == 0 || promise.LMSMOEQ_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.LMSMOEQ_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $state.reload();
                    })
            }
            else {
                $scope.submitted2 = true;
            }
        };
        
        $scope.editQuestion = function (id) {
            debugger;
            var data = {
                "LMSMOEQ_Id": id
            }

            apiService.create("MasterQuestionCollege/editQuestion", data).then(function (promise) {
                if (promise.editQus.length > 0) {
                    $scope.LMSMOEQ_Question = promise.editQus[0].lmsmoeQ_Question;
                    $scope.LMSMOEQ_QuestionDesc = promise.editQus[0].lmsmoeQ_QuestionDesc;
                    $scope.LMSMOEQ_Marks = promise.editQus[0].lmsmoeQ_Marks;
                    $scope.LMSMOEQ_Id = promise.editQus[0].lmsmoeQ_Id;
                    $scope.ismS_Id = promise.editQus[0].ismS_Id;
                }
            })
        }

        $scope.editQuestion1 = function (id) {
            debugger;
            var data = {
                "LMSMOEQB_Id": id
            }
            apiService.create("MasterQuestionCollege/editbranchquestion", data).then(function (promise) {

                $scope.result1 = promise.result;
                $scope.coursecount = promise.courselist;
                $scope.branchcount = promise.branchlist;
                $scope.arrlistchkgroup = promise.semisterlist;


                angular.forEach($scope.result1, function (opq) {
                    if (id == opq.lmsmoeqB_Id) {
                      //  $scope.asmcL_Id = opq.asmcL_Id;
                        $scope.LMSMOEQ_Id = opq;
                        $scope.LMSMOEQB_Id = id;
                    }
                });
                angular.forEach($scope.coursecount, function (opq) {
                    opq.selectedcourse = true;
                });
                angular.forEach($scope.branchcount, function (opq) {
                    opq.selectedbranch = true;

                });
                angular.forEach($scope.arrlistchkgroup, function (opq) {
                    opq.selected = true;
                });
            });
        };




        $scope.cancel = function () {
            $state.reload();
        }
        
        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }



        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };



        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }
        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                (angular.lowercase(obj.LMSMOEQ_Marks)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        }

      
        $scope.optionToggled = function (user) {
            debugger;
            $scope.ques_id = user.LMSMOEQ_Id;
            $scope.xyz = $scope.getQuestiondetails.every(function (itm) { return itm.xyz; });
            $scope.show_ansOption = true;
        };
        //---------------------------------------
        $scope.toggleAllC = function () {
            angular.forEach($scope.getAnsOptions, function (subj) {
                subj.abc = $scope.allC;
            })
        };

        $scope.optionToggledC = function () {
            $scope.allC = $scope.getAnsOptions.every(function (itm) { return itm.abc; });
        };
       



        $scope.addNew = function (totalgrid) {
            $scope.totalgrid = [];
            var LMBANO_No = '';
            if ($scope.getAnsOptions != null || $scope.getAnsOptions != '') {
                var a = $scope.getAnsOptions;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid.push({
                        LMSMOEQOA_Id: '',
                        LMSMOEQOA_Option: '',
                        LMSMOEQOA_OptionCode: '',
                    });
                }
                $scope.show_ansOption = true;
            }
        };
        

        $scope.optionChange = function () {
            debugger;
            var data = {
                "OptionType": $scope.att.xyz,
            }
            apiService.create("MasterQuestionCollege/optionChange", data).
                   then(function (promise) {
                       debugger;
                       $scope.getAnsOptions = promise.noopt;
                       $scope.addNew();
                   })
        }
        

        $scope.get_branches = function () {

            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })
            var data = {
                AMCO_Ids: AMCO_Ids,

            }
            apiService.create("MasterQuestionCollege/selectcourse", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;
                    angular.forEach($scope.branchcount, function (fy) {
                       // fy.selectedbranch = true;
                    })
                })

        };


        $scope.get_semester = function () {

            var AMCO_Ids = [];
            var AMB_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })
            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })
            var data = {
                AMCO_Ids: AMCO_Ids,
                AMB_Ids: AMB_Ids,

            }
            apiService.create("MasterQuestionCollege/selectbran", data).
                then(function (promise) {
                    $scope.arrlistchkgroup = promise.semisterlist;
                    angular.forEach($scope.branchcount, function (fy) {
                        // fy.selectedbranch = true;
                    })
                })
         
        };





        $scope.att = {};

        $scope.savedata1 = function (att) {
            $scope.submitted3 = true;
            $scope.submitted1 = false;
            $scope.submitted2 = false;
            debugger;
            if ($scope.myForm3.$valid) {
                var cnt = 0;
                $scope.test = $scope.totalgrid;
                for (var m = 0; m < $scope.totalgrid.length; m++) {
                    var stu_id1 = $scope.totalgrid[m].LMSMOEQOA_Option;
                    var stu_id2 = $scope.totalgrid[m].LMSMOEQOA_OptionCode;
                    var already_cnt = 0;
                    angular.forEach($scope.test, function (itm1) {
                        if (itm1.LMSMOEQOA_Option == stu_id1 && itm1.LMSMOEQOA_OptionCode == stu_id2) {
                            already_cnt += 1;
                        }
                    })
                    if (already_cnt == 1) {

                    }
                    else {
                        cnt += 1;
                    }
                }

                //angular.forEach($scope.getFQuestiondetails, function (itm1) {
                //    if (itm1.LMSMOEQOA_Option == stu_id1 && itm1.LMSMOEQOA_OptionCode == stu_id2) {
                //        already_cnt += 1;
                //    }
                //})




                var data = {
                    "LMSMOEQ_Id": $scope.att.xyz,
                    seleted_Ans: $scope.totalgrid,
                    "Answer": $scope.LMSMOEQOA_AnswerFlag
                }

                if (cnt < 1) {
                    apiService.create("MasterQuestionCollege/savedetails1", data).then(function (promise) {
                        if (promise.returnval == true) {
                            if (promise.LMSMOEQOA_Id == 0 || promise.LMSMOEQOA_Id < 0) {
                                swal("Record saved Successfully");
                            } else if (promise.LMSMOEQOA_Id > 0) {
                                swal("Record Upadte Successfully");
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.LMSMOEQOA_Id == 0 || promise.LMSMOEQOA_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.LMSMOEQOA_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel1();
                        $scope.loaddata();
                    })
                }
                else {
                    swal('Duplicate Answer Options Entered');
                }
              
            }
            else {
                $scope.submitted3 = true;
            }
        };


        $scope.addColumn = function (role, indexx, totalgrid) {

            angular.forEach(totalgrid, function (subscription, index) {
                if (indexx != index)
                    subscription.LMSMOEQOA_AnswerFlag = false;
            });
        }



        $scope.onformclick = function (LMSMOEQ_Id) {
            debugger;
            var data = {
                "LMSMOEQ_Id": LMSMOEQ_Id
            }
            apiService.create("MasterQuestionCollege/optiondetails", data).
               then(function (promise) {
                   $scope.getoptiondetails = promise.getoptiondetails;
                   $scope.Question = $scope.getoptiondetails[0].LMSMOEQ_Question;
               })
        }

        $scope.cancel1 = function (user) {
            $scope.ques_id = "";
           $scope.loaddata();
            $scope.show_ansOption = false;
        }


        $scope.search_box1 = function () {
            if ($scope.searchValue1 != "" || $scope.searchValue1 != null) {
                $scope.searc_button1 = false;
            }
            else {
                $scope.searc_button1 = true;
            }
        }
        $scope.filterValue1 = function (obj) {
            return (angular.lowercase(obj.LMSMOEQ_Question)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.LMSMOEQ_QuestionDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.LMSMOEQ_Marks)).indexOf($scope.searchValue) >= 0;
        }



        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee;
            var data = {
                "LMSMOEQ_Id": $scope.editEmployee,
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterQuestionCollege/Deletedetails", data).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }




    }
})();