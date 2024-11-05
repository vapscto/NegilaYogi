
(function () {
    'use strict';
    angular.module('app').controller('MasterSubjectAllMController', MasterSubjectAllMController)

    MasterSubjectAllMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function MasterSubjectAllMController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.sortKey = 'ismS_Id';
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


        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.reverse = false;
        $scope.MasterSubjectCl = function () {
            var pageid = 2;
            apiService.getURI("MasterSubjectAllM/getalldetails", pageid).then(function (promise) {

                $scope.grouptypeListOrder = promise.subject_m_listOrder;

                $scope.subject_list = promise.subject_m_list;
                $scope.presentCountgrid = $scope.subject_list.length;
                angular.forEach($scope.subject_list, function (opq) {
                    if (opq.ismS_PreadmFlag == 0) {
                        opq.ismS_SubjectFlag = "--";
                    }
                    else if (opq.ismS_SubjectFlag == 1 && opq.ismS_PreadmFlag == 1) {
                        opq.ismS_SubjectFlag = 'Written';

                    }
                    else if (opq.ismS_SubjectFlag == 0 && opq.ismS_PreadmFlag == 1) {
                        opq.ismS_SubjectFlag = 'Oral';
                    }
                });
                console.log($scope.subject_list);
            });
        };

        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.ISMS_SubjectFlag = "1";
        $scope.EditRecord = function (employee) {

            $scope.editEmployee = employee.ismS_Id;
            var orgaid = $scope.editEmployee;
            var mgs = "";
            var confirmmgs = "";
            if (employee.ismS_ActiveFlag == 0) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else if (employee.ismS_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " " + "the" + " " + employee.ismS_SubjectName + " " + "Subject ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,  " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("MasterSubjectAllM/Deletedetails", orgaid).then(function (promise) {
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
                            $scope.clear();
                            $scope.MasterSubjectCl();
                        });
                    }
                    else {
                        if (mgs === "Activate") {
                            mgs = "Activation";
                        }
                        if (mgs === "Deactivate") {
                            mgs = "De-Activation";
                        }
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.EditMasterSubvalue = function (employee) {
            $scope.editEmployee = employee.ismS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterSubjectAllM/Editdetails", pageid).then(function (promise) {
                $scope.ISMS_Id = promise.edit_m_subject[0].ismS_Id;
                $scope.ISMS_SubjectName = promise.edit_m_subject[0].ismS_SubjectName;
                $scope.ISMS_IVRSSubjectName = promise.edit_m_subject[0].ismS_IVRSSubjectName;
                $scope.ISMS_SubjectNameNew = promise.edit_m_subject[0].ismS_SubjectNameNew;
                $scope.ISMS_SubjectCode = promise.edit_m_subject[0].ismS_SubjectCode;
                $scope.ISMS_Max_Marks = promise.edit_m_subject[0].ismS_Max_Marks;
                $scope.ISMS_Min_Marks = promise.edit_m_subject[0].ismS_Min_Marks;
                $scope.ISMS_ExamFlag = promise.edit_m_subject[0].ismS_ExamFlag;
                $scope.ISMS_PreadmFlag = promise.edit_m_subject[0].ismS_PreadmFlag;
                $scope.ISMS_SubjectFlag = promise.edit_m_subject[0].ismS_SubjectFlag;
                $scope.ISMS_BatchAppl = promise.edit_m_subject[0].ismS_BatchAppl;
                $scope.ISMS_LanguageFlg = promise.edit_m_subject[0].ismS_LanguageFlg;
                $scope.ISMS_AtExtraFeeFlg = promise.edit_m_subject[0].ismS_AtExtraFeeFlg;
                //   $scope.ISMS_OrderFlag = promise.edit_m_subject[0].ismS_OrderFlag;
                // $scope.ISMS_TTFlag = promise.edit_m_subject[0].ismS_TTFlag;
                // $scope.ISMS_AttendanceFlag =promise.edit_m_subject[0].ismS_AttendanceFlag;
                if (promise.edit_m_subject[0].ismS_TTFlag === true) {
                    $scope.ISMS_TTFlag = 1;
                }
                else if (promise.edit_m_subject[0].ismS_TTFlag === false) {
                    $scope.ISMS_TTFlag = 0;
                }
                if (promise.edit_m_subject[0].ismS_AttendanceFlag === true) {
                    $scope.ISMS_AttendanceFlag = 1;
                }
                else if (promise.edit_m_subject[0].ismS_AttendanceFlag === false) {
                    $scope.ISMS_AttendanceFlag = 0;
                }
            });
        };

        $scope.submitted = false;
        $scope.saveMasterdata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.ISMS_PreadmFlag == 0) {
                    $scope.ISMS_Max_Marks = 0;
                    $scope.ISMS_Min_Marks = 0;
                    $scope.ISMS_SubjectFlag = "1";
                }
                var data = {

                    "ISMS_Id": $scope.ISMS_Id,
                    "ISMS_SubjectName": $scope.ISMS_SubjectName,
                    "ISMS_IVRSSubjectName": $scope.ISMS_IVRSSubjectName,
                    "ISMS_SubjectNameNew": $scope.ISMS_SubjectNameNew,
                    "ISMS_SubjectCode": $scope.ISMS_SubjectCode,
                    "ISMS_Max_Marks": $scope.ISMS_Max_Marks,
                    "ISMS_Min_Marks": $scope.ISMS_Min_Marks,
                    "ISMS_ExamFlag": $scope.ISMS_ExamFlag,
                    "ISMS_PreadmFlag": $scope.ISMS_PreadmFlag,
                    "ISMS_SubjectFlag": $scope.ISMS_SubjectFlag,
                    "ISMS_BatchAppl": $scope.ISMS_BatchAppl,
                    "ISMS_TTFlag": $scope.ISMS_TTFlag,
                    "ISMS_AttendanceFlag": $scope.ISMS_AttendanceFlag,
                    "ISMS_LanguageFlg": $scope.ISMS_LanguageFlg,
                    "ISMS_AtExtraFeeFlg": $scope.ISMS_AtExtraFeeFlg,

                };


                apiService.create("MasterSubjectAllM/savedetail", data).then(function (promise) {
                    $scope.subject_list = promise.subject_m_list;
                    $scope.presentCountgrid = $scope.subject_list.length;
                    angular.forEach($scope.subject_list, function (opq) {
                        if (opq.ismS_PreadmFlag == 0) {
                            opq.ismS_SubjectFlag = "--";
                        }
                        else if (opq.ismS_SubjectFlag == 1 && opq.ismS_PreadmFlag == 1) {
                            opq.ismS_SubjectFlag = 'Written';

                        }
                        else if (opq.ismS_SubjectFlag == 0 && opq.ismS_PreadmFlag == 1) {
                            opq.ismS_SubjectFlag = 'Oral';
                        }
                    });

                    if (promise.returnval === true) {
                        if (promise.ismS_Id == 0 || promise.ismS_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.ismS_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.ismS_Id == 0 || promise.ismS_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.ismS_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.MasterSubjectCl();
                    $scope.clear();
                });
            }

        };

        $scope.clear = function () {
            $scope.ISMS_Id = 0;
            $scope.ISMS_SubjectName = "";
            $scope.ISMS_IVRSSubjectName = "";
            $scope.ISMS_SubjectNameNew = "";
            $scope.ISMS_SubjectCode = "";
            $scope.ISMS_Max_Marks = "";
            $scope.ISMS_Min_Marks = "";
            $scope.ISMS_ExamFlag = 0;
            $scope.ISMS_PreadmFlag = 0;
            $scope.ISMS_SubjectFlag = 1;
            $scope.ISMS_BatchAppl = 0;
            $scope.ISMS_TTFlag = 0;
            $scope.ISMS_AttendanceFlag = 0;
            $scope.ISMS_LanguageFlg = 0;
            $scope.ISMS_AtExtraFeeFlg = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.searchValue = "";
        };

        $scope.valid_max = function (max) {

            var num_max = Number(max);
            if (num_max > 1000) {
                swal("Max Value Is 1000");
                $scope.ISMS_Max_Marks = "";
            }
            $scope.ISMS_Min_Marks = "";
        };

        $scope.valid_min = function (max, min) {

            if (min !== null && min !== undefined && min !== "") {
                if (max !== null && max !== undefined && max !== "") {
                    var num_max = Number(max);
                    var num_min = Number(min);
                    if (num_min >= num_max) {
                        swal("Min. Marks Value  Should Be Less Than Max. Marks Value");
                        $scope.ISMS_Min_Marks = "";
                    }
                }
                else {
                    swal("First Enter Max. Marks");
                    $scope.ISMS_Min_Marks = "";
                }
            }
        };

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();
        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                if (value.ISMS_Id !== 0) {
                    orderarray[key].ismS_OrderFlag = key + 1;
                }
            });

            angular.forEach(orderarray, function (opq) {
                if (opq.ismS_PreadmFlag == 0) {
                    opq.ismS_SubjectFlag = 0;
                }
                else if (opq.ismS_SubjectFlag === 'Written' && opq.ismS_PreadmFlag == 1) {
                    opq.ismS_SubjectFlag = 1;

                }
                else if (opq.ismS_SubjectFlag === 'Oral' && opq.ismS_PreadmFlag == 1) {
                    opq.ismS_SubjectFlag = 0;
                }

            });
            var data = {
                subjectDTO: orderarray
            };
            apiService.create("MasterSubjectAllM/validateordernumber", data).then(function (promise) {
                if (promise.retrunMsg !== "" && promise.retrunMsg !== undefined && promise.retrunMsg !== null) {
                    swal(promise.retrunMsg);
                }
                $scope.MasterSubjectCl();
                $scope.clear();
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_IVRSSubjectName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                //(angular.lowercase(obj.ismS_SubjectNameNew)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectFlag)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.ismS_Max_Marks)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.ismS_Min_Marks)).indexOf($scope.searchValue) >= 0
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].ismS_OrderFlag = Number(index) + 1;

                }
            }
        };

    }

})();