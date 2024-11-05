(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgSubjectMasterController', ClgSubjectMasterController)

    ClgSubjectMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q']
    function ClgSubjectMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q) {

        $scope.sortKey = 'ismS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.obj = {};
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
            apiService.getURI("ClgSubjectMaster/getalldetails", pageid).then(function (promise) {

                $scope.grouptypeListOrder = promise.subject_m_list_new;
                $scope.subject_list = promise.subject_m_list;
                $scope.presentCountgrid = $scope.subject_list.length;

                $scope.courst_list = promise.courst_list;
                $scope.branch_list = promise.branch_list;
                $scope.sub_list = promise.sub_list;
                $scope.year_list = promise.year_list;
                $scope.yearOfintro = promise.yearOfintro;

                $scope.mappinglistdata = promise.mappinglistdata;

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


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.ISMS_SubjectFlag = "1";
        $scope.EditRecord = function (employee, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

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
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes,  " + mgs + " It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ClgSubjectMaster/Deletedetails", orgaid).then(function (promise) {
                            if (promise.already_cnt == true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval == true) {
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
                            mgs == "Activation";
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
            apiService.getURI("ClgSubjectMaster/Editdetails", pageid).
                then(function (promise) {
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

                    if (promise.edit_m_subject[0].ismS_TTFlag == true) {
                        $scope.ISMS_TTFlag = 1;
                    }
                    else if (promise.edit_m_subject[0].ismS_TTFlag == false) {
                        $scope.ISMS_TTFlag = 0;
                    }
                    if (promise.edit_m_subject[0].ismS_AttendanceFlag == true) {
                        $scope.ISMS_AttendanceFlag = 1;
                    }
                    else if (promise.edit_m_subject[0].ismS_AttendanceFlag == false) {
                        $scope.ISMS_AttendanceFlag = 0;
                    }
                });
        };

        $scope.obj.ISMS_AttendanceFlag = false;
        $scope.obj.ISMS_TTFlag = false;
        $scope.submitted = false;
        $scope.saveMasterdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.obj.ISMS_PreadmFlag == 0) {
                    $scope.obj.ISMS_Max_Marks = 0;
                    $scope.obj.ISMS_Min_Marks = 0;

                    $scope.obj.ISMS_SubjectFlag = "1";
                }
                var ISMS_ExamFlag = 0;
                if ($scope.obj.ISMS_ExamFlag == true) {
                    ISMS_ExamFlag = 1;
                }
                var ISMS_PreadmFlag = 0;
                if ($scope.obj.ISMS_PreadmFlag == true) {
                    ISMS_PreadmFlag = 1;
                }
                var ISMS_SubjectFlag = 0;
                if ($scope.obj.ISMS_SubjectFlag == true) {
                    ISMS_SubjectFlag = 1;
                }
                var ISMS_BatchAppl = 0;
                if ($scope.obj.ISMS_BatchAppl == true) {
                    ISMS_BatchAppl = 1;
                }
                var ISMS_LanguageFlg = 0;
                if ($scope.obj.ISMS_LanguageFlg == true) {
                    ISMS_LanguageFlg = 1;
                }
                var ISMS_AtExtraFeeFlg = 0;
                if ($scope.obj.ISMS_AtExtraFeeFlg == true) {
                    ISMS_AtExtraFeeFlg = 1;
                }
                var data = {

                    "ISMS_Id": $scope.ISMS_Id,
                    "ISMS_SubjectName": $scope.obj.ISMS_SubjectName,
                    "ISMS_IVRSSubjectName": $scope.obj.ISMS_IVRSSubjectName,
                    "ISMS_SubjectNameNew": $scope.obj.ISMS_SubjectNameNew,
                    "ISMS_SubjectCode": $scope.obj.ISMS_SubjectCode,
                    "ISMS_Max_Marks": $scope.obj.ISMS_Max_Marks,
                    "ISMS_Min_Marks": $scope.obj.ISMS_Min_Marks,
                    "ISMS_ExamFlag": ISMS_ExamFlag,
                    "ISMS_PreadmFlag": ISMS_PreadmFlag,
                    "ISMS_SubjectFlag": ISMS_SubjectFlag,
                    "ISMS_BatchAppl": ISMS_BatchAppl,
                    "ISMS_TTFlag": $scope.obj.ISMS_TTFlag,
                    "ISMS_AttendanceFlag": $scope.obj.ISMS_AttendanceFlag,
                    "ISMS_LanguageFlg": ISMS_LanguageFlg,
                    "ISMS_AtExtraFeeFlg": ISMS_AtExtraFeeFlg,

                };


                apiService.create("ClgSubjectMaster/savedetail", data).then(function (promise) {
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
                    })
                    if (promise.returnval === true) {
                        if (promise.ismS_Id == 0 || promise.ismS_Id < 0) {
                            swal('Record saved successfully', "", "success");
                            $state.reload();
                        }
                        else if (promise.ismS_Id > 0) {
                            swal('Record updated successfully', "","success");
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        if (promise.returnvaluetype === 'subjectname') {
                            swal('Subject Name already exist');
                        }
                        else {
                            swal('Subject Code already exist');
                        }
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
            // $scope.ISMS_PreadmFlag = 0;
            $scope.ISMS_BatchAppl = 0;
            // $scope.ISMS_OrderFlag = 0;
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

        }

        $scope.valid_min = function (max, min) {

            if (min != null && min != undefined && min != "") {
                if (max != null && max != undefined && max != "") {
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
        }

        //fix the order drag
        //ConfigA is an Items
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
                else if (opq.ismS_SubjectFlag == 'Written' && opq.ismS_PreadmFlag == 1) {
                    opq.ismS_SubjectFlag = 1;

                }
                else if (opq.ismS_SubjectFlag == 'Oral' && opq.ismS_PreadmFlag == 1) {
                    opq.ismS_SubjectFlag = 0;
                }

            })


            var data = {
                subjectDTO: orderarray,
            }
            apiService.create("ClgSubjectMaster/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);

                    }
                    $scope.MasterSubjectCl();
                    $scope.clear();
                });

        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_IVRSSubjectName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                //(angular.lowercase(obj.ismS_SubjectNameNew)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectFlag)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.ismS_Max_Marks)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.ismS_Min_Marks)).indexOf($scope.searchValue) >= 0
        }

        ///===============================================================================Tab2.

        $scope.submitted2 = false;
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.searchValue23 = "";
        $scope.currentPage2 = 1;

        $scope.itemsPerPage2 = paginationformasters ;

        $scope.sortReverse2 = true;
        $scope.sort2 = function (key) {
            $scope.sortReverse2 = ($scope.sortKey == key) ? !$scope.sortReverse2 : $scope.sortReverse2;
            $scope.sortKey = key;
        };


        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.branch_list, function (itm) {
                itm.select = checkStatus;
            });
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.branch_list.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.branch_list.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }


        $scope.clearid = function () {

            $scope.ISMS_DiscontinuedFlg = false;
            $scope.Images = "";
            $scope.searchchkbx = "";
            $scope.AMCO_Id = "";
            $scope.ISMS_Id = "";
            $scope.ISMS_DiscontinuedYear = "";
            $scope.ISMS_IntroYear = "";
            $scope.ISMS_DiscontinuedReason = "";
            $scope.submitted2 = false;
            $scope.usercheck = "";
            $scope.submitted2 = false;
            angular.forEach($scope.branch_list, function (brnch) {
                brnch.select = false;
            })

            $scope.remove_file();

        }


        $scope.savedata2 = function () {

            $scope.branch_list_data = [];


            angular.forEach($scope.branch_list, function (brnch) {
                if (brnch.select == true) {
                    $scope.branch_list_data.push({ amB_Id: brnch.amB_Id });
                }
            });

            if ($scope.myForm2.$valid) {
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "ISMS_Id": $scope.ISMS_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "ISMS_DiscontinuedFlg": $scope.ISMS_DiscontinuedFlg,
                        "ISMS_DiscontinuedYear": $scope.asmaY_Id,
                        "ISMS_DiscontinuedReason": $scope.ISMS_DiscontinuedReason,
                        "ISMS_IntroYear": $scope.ISMS_IntroYear,

                        branch_list_data: $scope.branch_list_data,
                    }
                }
                else {

                    var att_file = "";
                    $scope.filedoc = [];
                    $scope.filedoc = [$scope.notice];
                    if ($scope.filedoc.length > 0) {
                        for (var i = 0; i < $scope.filedoc.length; i++) {
                            att_file = $scope.filedoc[0];
                        }
                    }
                    var att_file11 = att_file.toString();

                    var data = {
                        "ISMS_Id": $scope.ISMS_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "ISMS_DiscontinuedFlg": $scope.ISMS_DiscontinuedFlg,
                        "ISMS_DiscontinuedYear": $scope.ISMS_DiscontinuedYear,
                        "ISMS_DiscontinuedReason": $scope.ISMS_DiscontinuedReason,
                        "ISMS_IntroYear": $scope.ISMS_IntroYear,
                        "ISMS_FileName": $scope.file_detail,
                        "ISMS_FilePath": att_file11,

                        branch_list_data: $scope.branch_list_data,
                    }
                }

                apiService.create("ClgSubjectMaster/savedata2", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record Saved Successfully');
                        $scope.clearid();

                        $scope.mappinglistdata = promise.mappinglistdata;
                    }
                });
            }
            else {
                $scope.submitted2 = true;
            }

        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
            debugger;
            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }
        function Uploadprofile() {
            debugger;
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].ismS_OrderFlag = Number(index) + 1;

                }
            }
        };

    }

})();