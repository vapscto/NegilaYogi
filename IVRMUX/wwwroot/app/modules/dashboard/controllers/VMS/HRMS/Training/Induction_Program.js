(function () {
    'use strict';
    angular
        .module('app')
        .controller('InductionController', inductionController)

    inductionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function inductionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.sortKey = 'hrtcR_Id';
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.hrtcR_PrgogramName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0 ||
                ($filter('hrtcR_StartDate')(obj.hrtcR_StartDate, 'dd/MM/yyyy').indexOf($scope.searchValue2) >= 0) ||
                ($filter('hrtcR_EndDate')(obj.hrtcR_EndDate, 'dd/MM/yyyy').indexOf($scope.searchValue2) >= 0) ||
                (angular.lowercase(obj.hrmB_BuildingName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0;
        };

        $scope.filterValue1 = function (obj1) {
            return (angular.lowercase(obj1.hrtcR_PrgogramName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                ($filter('hrtcR_StartDate')(obj1.hrtcR_StartDate, 'dd/MM/yyyy').indexOf($scope.searchValue1) >= 0) ||
                ($filter('hrtcR_EndDate')(obj1.hrtcR_EndDate, 'dd/MM/yyyy').indexOf($scope.searchValue1)) >= 0;
        };

        //============================check box data move============================================
        $scope.chckedIndexs3 = [];
        $scope.revtest3 = function (rr) {
            if (rr.selectedd1 == false) {
                //  for (var i = 0; i < $scope.participates_Employee_list.length; i++) {
                angular.forEach($scope.participates_Employee_list, function (tt) {
                    if (tt.hrmE_EmployeeFirstName == rr.hrmE_EmployeeFirstName) {
                        tt.selectedd = false;
                        $scope.resultData.splice($scope.resultData.indexOf(rr), 1);
                        return;
                    }
                });
            }
        };

        $scope.test3 = function (data, participates_Employee_list) {
            if ($scope.chckedIndexs3.indexOf(data) === -1) {
                $scope.chckedIndexs3.push(data);
                $scope.resultData = $scope.chckedIndexs3;
                for (var i = 0; i < $scope.chckedIndexs3.length; i++) {
                    if ($scope.chckedIndexs3[i].selectedd == true) {
                        $scope.resultData[i].selectedd1 = true;
                    } else {
                        $scope.resultData[i].selectedd1 = false;
                    }
                }
            }
            else {
                $scope.chckedIndexs3.splice($scope.chckedIndexs3.indexOf(data), 1);
            }
        };
        //==================================End==========================================
        $scope.date_change = function () {
            $scope.enddate = "";
        };

        //============================Load Data=============================================
        $scope.loaddata = function () {
            $scope.searchValue1 = "";
            $scope.searchValue2 = "";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.currentPageA = 1;
            $scope.itemsPerPageA = 10;


            var pageid = 2;
            apiService.getURI("Induction_Training/getalldata", pageid).then(function (Promise) {
                $scope.pgm = false;
                $scope.deptlists = Promise.deptlist;
                $scope.buillist = Promise.buillist;
                $scope.training_details_list = Promise.training_details_list;
                $scope.program_dd_list = Promise.program_dd_list;
                $scope.training_creation_list = Promise.training_creation_list;
                $scope.presentCountgrid = $scope.training_creation_list.length;
                $scope.presentCountgrid1 = $scope.training_details_list.length;
                $scope.checkadmin = Promise.training_details_Check[0].checkadmin;
                if ($scope.checkadmin === 2 || $scope.checkadmin === 3) {
                    $scope.hide1 = false;
                    $scope.disabled1 = true;
                }
                else {
                    $scope.hide1 = true;
                    $scope.disabled1 = false;
                }
                $scope.disabled = true;
            });
        };

        $scope.question_evaluation = function (aa) {
            var data = {
                "HRTCR_Id": aa.hrtcR_Id
            };
            apiService.create("Training_Master/question_evaluation_TQM", data).then(function (promise) {
                $scope.employeename = promise.question_emp_details_list[0].employeename;
                $scope.HRTCR_PrgogramName = promise.question_emp_details_list[0].hrtcR_PrgogramName;
                $scope.start_date = promise.question_emp_details_list[0].start_date;
                $scope.end_date = promise.question_emp_details_list[0].end_date;
                $scope.HRTCR_InternalORExternalFlg = promise.question_emp_details_list[0].hrtcR_InternalORExternalFlg;
                $scope.HRMDES_DesignationName = promise.question_emp_details_list[0].hrmdeS_DesignationName;
                $scope.HRTFEED_TrainerId = promise.question_emp_details_list[0].hrmE_Id;
                $scope.HRTCR_Id = promise.question_emp_details_list[0].hrtcR_Id;

                $scope.feedback_question = promise.feedback_question;
                $scope.feedback_option = promise.feedback_option;
                angular.forEach($scope.feedback_question, function (yy) {
                    var temparr = [];
                    angular.forEach($scope.feedback_option, function (dd) {
                        if (yy.hrmfqnS_Id == dd.hrmfqnS_Id) {
                            temparr.push(dd);
                        }
                    });
                    yy.anslist = temparr;
                });
            });
        };

        $scope.distcourselist = [];

        angular.forEach($scope.time_Table, function (st) {
            if ($scope.distcourselist.length == 0) {

                $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
            }
            else if ($scope.distcourselist.length > 0) {
                var cntt = 0;
                angular.forEach($scope.distcourselist, function (exm) {
                    if (exm.AMCO_Id == st.AMCO_Id && exm.AMB_Id == st.AMB_Id && exm.AMSE_Id == st.AMSE_Id && exm.ACMS_Id == st.ACMS_Id) {
                        cntt += 1;
                    }
                })
                if (cntt == 0) {

                    $scope.distcourselist.push({ AMCO_Id: st.AMCO_Id, AMB_Id: st.AMB_Id, AMSE_Id: st.AMSE_Id, ACMS_Id: st.ACMS_Id, CRSDETAILS: st.CRSDETAILS });
                }
            }
        })

        $scope.Savequestion_eve = function (HRTCR_Id, HRTFEED_TrainerId, HRTCR_InternalORExternalFlg) {
            $scope.questionoption_new = [];
            angular.forEach($scope.feedback_question, function (aa) {
                angular.forEach(aa.anslist, function (kk) {
                    if (kk.hrmfopT_Id1 == true) {
                        $scope.questionoption_new.push({ HRMFQNS_Id: kk.hrmfqnS_Id, HRMFOPT_Id: kk.hrmfopT_Id })
                    }
                });

            });



            if (HRTCR_InternalORExternalFlg = false) {
                var int = "Internal";
            }
            else {
                var int = "External";
            }
            var data = {
                questionoption_new: $scope.questionoption_new,
                "HRTFEED_AboutTraining": $scope.HRTFEED_AboutTraining,
                "HRTFEED_Improvement": $scope.HRTFEED_Improvement,
                "HRTFEED_Response": $scope.HRTFEED_Response,
                "HRTCR_Id": HRTCR_Id,
                "HRTFEED_TrainerId": HRTFEED_TrainerId,
                "HRTCR_InternalORExternalFlg1": int

            };


            apiService.create("Training_Master/Training_Feedback_TQM", data).then(function (promise) {
                if (promise.returnvalue == "Add") {
                    swal('Feedback Record Saved Successfully.')
                    $state.reload();
                }
                else {
                    swal('Feedback Record Not Saved Successfully !!!')
                    $state.reload();
                }
            });


        };
        //=========================Get trainer ==================
        $scope.get_trainer = function () {
            var data = {
                "HRTCR_Id": $scope.hrtcR_Id
            };
            apiService.create("Induction_Training/get_trainer", data).then(function (promise) {
                $scope.trinee_list = promise.trinee_list;
                $scope.topiclist = promise.topiclist;
            });
        };

        //======================================get all dd=============
        $scope.get_AllEmployee = function () {
            var data = {
                "HRMD_Id": $scope.hrmD_Id

            };
            apiService.create("Induction_Training/getEmpDD", data).then(function (promise) {
                $scope.participates_Employee_list = promise.participates_Employee_list;

            });
        };
        $scope.get_AllFloor = function () {
            var data = {
                "HRMB_Id": $scope.hrmB_Id

            };
            apiService.create("Induction_Training/getFloorDD", data).then(function (promise) {
                $scope.flrlist = promise.floor_list;
            });
        };
        $scope.get_AllRoom = function () {
            var data = {
                "HRMB_Id": $scope.hrmB_Id,
                "HRMF_Id": $scope.hrmF_Id
            };
            apiService.create("Induction_Training/getRoomDD", data).then(function (promise) {
                $scope.roomlist = promise.room_list;
            });
        };
        //==================================ADD Coloum + - ==============================
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addpirows = function () {
            //$scope.submitted = true;
            //if ($scope.myForm.$valid) {
            if ($scope.transrows.length > 1) {
                for (var i = 0; i === $scope.transrows.length; i++) {
                    var id = $scope.transrows[i].itrS_Id;
                    var lastChar = id.substr(id.length - 1);
                    $scope.cnt = parseInt(lastChar);
                }
            }
            $scope.cnt = $scope.cnt + 1;
            $scope.tet = 'trans' + $scope.cnt;
            var newItemNo = $scope.cnt;
            $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            // else {
            // $scope.submitted = true;
            // }

        };
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };
        //================================end==========================================


        //=================================checkbox==================================
        $scope.usercheckC2 = "";
        $scope.get_evalistt = function () {


            $scope.usercheckC2 = $scope.evetrainer_list.every(function (options) {
                return options.selectedd2;
            });
        };
        $scope.selectedAll = '';
        $scope.filterchkbxhous1 = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.selectedAll)) >= 0;
        };
        $scope.selectedAll1 = '';
        $scope.filterchkbxhous2 = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.selectedAll1)) >= 0;
        };

        $scope.selectedAll1 = "";
        $scope.all_checkC2 = function () {
            var checkStatus2 = $scope.usercheckC2;
            angular.forEach($scope.evetrainer_list, function (itm) {
                itm.selectedd2 = checkStatus2;
            });
        };
        //==============================end===================================================


        //=======================================View==============================================
        $scope.view1 = {}
        $scope.view = function (vi) {
            $scope.view1 = vi.hrtcR_Id;
            var pageid = $scope.view1;
            apiService.getURI("Induction_Training/Training_Views", pageid).then(function (promise) {
                $scope.HRTCR_InternalORExternalFlg = promise.induction_view_list[0].HRTCR_InternalORExternalFlg;
                $scope.HRTCR_StatusFlg = promise.induction_view_list[0].HRTCR_StatusFlg;
                $scope.Date = promise.induction_view_list[0].CDate;
                // $scope.Pricing = promise.induction_view_list[0].HRINPC_CostFee; 
                $scope.HRTCR_ProgramDesc = promise.induction_view_list[0].HRTCR_ProgramDesc;
                $scope.HRMB_BuildingName = promise.induction_view_list[0].HRMB_BuildingName;
                $scope.CreDet_list = promise.induction_view_list_details;
                $scope.V_HRTCR_Id = promise.induction_view_list[0].HRTCR_Id;
                $scope.HRTCR_PrgogramName = promise.induction_view_list[0].HRTCR_PrgogramName;
                $scope.presentCountgrid = $scope.CreDet_list.length;

            });
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.Clearid_2tab = function () {
            $state.reload();
        };
        //=======================evaluation===================================
        $scope.eve = {}
        $scope.evaluation = function (ev) {
            $scope.evetrainer_list = [];
            $scope.eve = ev.hrtcR_Id;
            var pageid = $scope.eve;
            apiService.getURI("Induction_Training/EveGet", pageid).then(function (promise) {
                if (promise.evaluation_trainer_list != 0) {
                    $scope.evetrainer_list = promise.evaluation_participants_list;
                    $scope.evaluation_trainer_list = promise.evaluation_trainer_list;

                }
                else {
                    swal('Already Evaluation Marks Entered.')

                }
            });

        };

        ////=================================== Add/Edit===============================================
        $scope.submitted = false;
        $scope.Allsavedata = function () {

            $scope.empchlist = [];
            $scope.trainerlist = [];
            angular.forEach($scope.resultData, function (cls) {
                if (cls.selectedd1 === true) {
                    $scope.empchlist.push(cls);
                }
            });
            //HRINPC_Id: li.HRICD_Date, HRICD_StartTime: li.HRICD_StartTime, HRICD_EndTime: li.HRICD_EndTime, HRTRC_Id: li.hrtrC_Id
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.startdate1 = new Date($scope.startdate).toDateString();
                $scope.enddate1 = new Date($scope.enddate).toDateString();

                var data = {
                    "HRTCR_Id": $scope.V_HRTCR_Id,
                    "HRTCR_PrgogramName": $scope.HRTCR_PrgogramName,
                    "HRMD_Id": $scope.hrmD_Id,
                    "HRTCR_ProgramDesc": $scope.HRTCR_ProgramDesc,
                    "HRMB_Id": $scope.hrmB_Id,
                    "HRMF_Id": $scope.hrmF_Id,
                    "HRMR_Id": $scope.hrmR_Id,
                    "HRTCR_CostFeeFlg": $scope.cost_check,
                    emplyee: $scope.empchlist,
                    "HRTCR_Cost": $scope.cost,
                    "HRTCR_InternalORExternalFlg": $scope.HRTCR_InternalORExternalFlg,
                    "HRTCR_StartDate": $scope.startdate1,
                    "HRTCR_EndDate": $scope.enddate1,
                    "CHECK_Notification": $scope.checknoti
                };
                apiService.create("Induction_Training/SaveEdit_Induction", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });

            }
            else {

                $scope.submitted = true;
            }

        };
        //======================================save data==================================
        $scope.submittedug = false;
        $scope.Savedata_td = function () {

            $scope.trainerlist = [];
            angular.forEach($scope.transrows, function (lia) {
                $scope.trainerlist.push({ HRME_Id: lia.hrmE_Id, HRTCRD_Content: lia.HRTCRD_Content, HRTCRD_StartTime: $filter('date')(lia.HRTCRD_StartTime, "HH:mm"), HRTCRD_EndTime: $filter('date')(lia.HRTCRD_EndTime, "HH:mm"), HRTCRINTTR_StartDate: new Date(lia.HRTCRINTTR_StartDate).toDateString(), HRMTT_Id: lia.hrmtT_Id });
            });
            $scope.trainerlist11 = $scope.transrows;
            angular.forEach($scope.trainerlist11, function (ss) {
                ss.HRTCRINTTR_StartDate = new Date(ss.HRTCRINTTR_StartDate).toDateString();
            });

            //$scope.trainerlist = [];
            //angular.forEach($scope.transrows, function (li) {
            //    $scope.trainerlist.push({
            //        HRME_Id: li.hrmE_Id, HRTCRD_Content: li.HRTCRD_Content, HRTCRD_StartTime: li.HRTCRD_StartTime, HRTCRD_EndTime: li.HRTCRD_EndTime, trtcr_startdate: new date(li.HRTCRINTTR_StartDate)
            //    });
            //});
            $scope.submittedug = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "Notification": $scope.trainernoti,
                    trainingdetails: $scope.trainerlist
                };
                apiService.create("Induction_Training/SaveEdit_training_details", data).then(function (promise) {
                    if (promise.returnvalue !== "") {

                        if (promise.returnvalue === "Update") {
                            swal('Record Updated Successfully');


                        }
                        else if (promise.returnvalue === "Add") {
                            swal('Record Saved Successfully');


                        }
                        else if (promise.returnvalue === "false") {
                            swal('Record Not Saved/Updated successfully');

                        }
                        else if (promise.returnvalue === "Duplicate") {
                            swal('Category Already Exist');


                        }
                        $state.reload();
                    }

                    else {
                        swal('Operation Failed');
                        $state.reload();
                    }
                });
            }
            else {

                $scope.submittedug = true;
            }
        }
        //=============================save eva======================================================
        $scope.submittedug1 = false;
        $scope.SaveEve = function (ii) {


            $scope.empchlist2 = [];
            angular.forEach($scope.evetrainer_list, function (emp) {
                if (emp.selectedd2 === true) {

                    $scope.empchlist2.push({ HRME_Id: emp.hrmE_Id, HRTCRD_Rating: emp.rating, HRTCRD_TrainerRemarks: emp.remark, HRTCR_Id: emp.hrtcR_Id });
                }
            });
            $scope.submittedug5 = true;
            if ($scope.myForm5.$valid) {
                var data = {

                    trainingdetails: $scope.empchlist2,
                    "HRMET_Id": $scope.HRMET_Id,
                };

                apiService.create("Induction_Training/SaveEvalution_trinee_rating", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');

                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');

                        }
                    })

            }
            else {

                $scope.submittedug5 = true;
            }

        };

        //=====================================Get Edit======================================
        $scope.edi = {};
        $scope.edit = function (edit) {
            $scope.transrows = [];
            $scope.edi = edit.hrtcR_Id;
            var data = {
                "HRTCR_Id": $scope.edi
            }
            apiService.create("Induction_Training/edit_training_details", data).then(function (promise) {
                $scope.trinee_list = promise.trinee_list;
                $scope.program_dd_list = promise.program_dd_list;
                promise.program_dd_list_one.length > 0
                {
                    $scope.pgm = true;
                }
                $scope.hrtcR_Id = promise.program_dd_list_one[0].hrtcR_Id;
                $scope.hrtcR_PrgogramName1 = promise.program_dd_list_one[0].hrtcR_PrgogramName;
                $scope.differentarray = [];
                $scope.differentarray = promise.training_create_Details_list;

                if ($scope.differentarray.length > 0) {
                    angular.forEach($scope.differentarray, function (tt) {
                        $scope.transrows.push({
                            HRTCRINTTR_StartDate: new Date(tt.hrtcrinttR_StartDate),
                            HRTCRD_StartTime: moment(tt.hrtcrD_StartTime, 'HH:mm a').format(),
                            HRTCRD_EndTime: moment(tt.hrtcrD_EndTime, 'HH:mm a').format(),
                            hrmE_Id: tt.hrmE_Id,
                            hrmE_EmployeeFirstName: tt.hrmE_EmployeeFirstName,
                            HRTCRD_Content: tt.hrtcrD_Content,

                        });
                    })
                }
            })
        }

        //============================= De-Active==============================
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.hrtcR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Induction_Training/deactivate_Induction_create", flr).
                            then(function (promise) {

                                if (promise.hrtcR_ActiveFlag === true) {
                                    swal('Induction Program Activated Successfully');
                                }

                                else {
                                    swal('Induction Program Deactivated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };
        //========================================end=============================================

        //========================================Edit==============================================
        $scope.edit1 = {};
        $scope.edit_induction_create = function (ed) {
            $scope.edit1 = ed.hrtcR_Id;
            var data = {
                "HRTCR_Id": $scope.edit1
            }
            apiService.create("Induction_Training/edit_induction_create", data).then(function (promise) {
                $scope.HRTCR_PrgogramName = promise.training_create_Details[0].hrtcR_PrgogramName;
                $scope.hrmD_DepartmentName = promise.training_create_Details[0].hrmD_DepartmentName;
                $scope.hrmD_Id = promise.training_create_Details[0].hrmD_Id;
                $scope.HRTCR_ProgramDesc = promise.training_create_Details[0].hrtcR_ProgramDesc;
                $scope.cost_check = promise.training_create_Details[0].hrtcR_CostFeeFlg;
                $scope.cost = promise.training_create_Details[0].hrtcR_Cost;
                $scope.hrmB_BuildingName = promise.training_create_Details[0].hrmB_BuildingName;
                $scope.hrmB_Id = promise.training_create_Details[0].hrmB_Id;
                $scope.hrmF_FloorName = promise.training_create_Details[0].hrmF_FloorName;
                $scope.hrmF_Id = promise.training_create_Details[0].hrmF_Id;
                $scope.hrmR_RoomName = promise.training_create_Details[0].hrmR_RoomName;
                $scope.hrmR_Id = promise.training_create_Details[0].hrmR_Id;
                $scope.HRTCR_InternalORExternalFlg = promise.training_create_Details[0].hrtcR_InternalORExternalFlg;
                $scope.V_HRTCR_Id = promise.training_create_Details[0].hrtcR_Id;
                $scope.startdate = new Date(promise.training_create_Details[0].hrtcR_StartDate);
                $scope.enddate = new Date(promise.training_create_Details[0].hrtcR_EndDate);
                $scope.resultData_temp = [];
                $scope.resultData = promise.training_create_Trainee_list;
                $scope.get_AllEmployee();
                $scope.get_AllFloor();
                $scope.get_AllRoom();

                if ($scope.resultData.length > 0) {
                    angular.forEach($scope.resultData, function (ss) {
                        ss.selectedd1 = true;

                    });
                }
            })
        }


        //=================================Update================================================
        $scope.statusupdate = function () {
            var data = {
                "Status": $scope.Status,
                "HRTCR_Id": $scope.V_HRTCR_Id
            }
            apiService.create("Induction_Training/update_status", data).then(function (promise) {
                if (promise.returnvalue === "Update") {
                    swal('Record Updated Successfully');
                    $state.reload();
                    return;
                }
                else {
                    swal('Operation Failed!');
                    $state.reload();
                    return;
                }
            })
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted1 = function (field) {
            return $scope.submittedug || field.$dirty;
        };
        $scope.interacted2 = function (field) {
            return $scope.submittedug1 || field.$dirty;
        };
        $scope.interacted5 = function (field) {
            return $scope.submittedug5 || field.$dirty;
        };
    }

})();