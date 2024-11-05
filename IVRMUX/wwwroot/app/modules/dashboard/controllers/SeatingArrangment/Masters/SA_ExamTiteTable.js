(function () {
    'use strict';
    angular
        .module('app')
        .controller('SA_ExamTiteTableController', SA_ExamTiteTableController)

    SA_ExamTiteTableController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function SA_ExamTiteTableController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.tempcldrlst = [];


        var date = new Date();
        $scope.ESAETT_FromDate = date;
        $scope.ESAETT_ToDate = date;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.search = "";

        $scope.loaddata = function () {
            var id = 1;
            apiService.getURI("SA_Exam_Titetable/load_TT", id).then(function (promise) {
                $scope.edt = false;
                $scope.yearlst = promise.yearlst;
                $scope.examlist = promise.examlist;
                $scope.university_examlist = promise.university_examlist;
                $scope.courselist = promise.courselist;
                $scope.branchlist = promise.branchlist;
                $scope.semesterlist = promise.semesterlist;

                $scope.examslotlist = promise.examslotlist;
                $scope.subjectlist = promise.subjectlist;
                $scope.subjectschemalist = promise.subjectschemalist;
                if (promise.satimetablelist != null || promise.satimetablelist > 0) {
                    $scope.satimetablelist = promise.satimetablelist;
                }


            })

        };
       
        //==============save data==========
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arraypush = [];
                if ($scope.edt = false) {
                    angular.forEach($scope.transrows, function (pr) {
                        $scope.arraypush.push({
                            ACSS_Id: pr.ACSS_Id.acsS_Id, ESAESLOT_Id: pr.ESAESLOT_Id.esaesloT_Id, ISMS_Id: pr.ISMS_Id.ismS_Id,
                            ESAETT_ExamDate: pr.ESAETT_ExamDate
                        });
                    })
                }
                else if ($scope.edt = true) {
                    angular.forEach($scope.transrows, function (pr) {
                        $scope.arraypush.push({
                            ACSS_Id: pr.ACSS_Id, ESAESLOT_Id: pr.ESAESLOT_Id, ISMS_Id: pr.ISMS_Id,
                            ESAETT_ExamDate: pr.ESAETT_ExamDate
                        });
                    })
                }
               

                var data = {
                    "ESAETT_Id": $scope.ESAETT_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ESAETT_FromDate": $scope.ESAETT_FromDate,
                    "ESAETT_ToDate": $scope.ESAETT_ToDate,
                    "examdetailsarray": $scope.arraypush,
                 
                }
                apiService.create("SA_Exam_Titetable/Save_TT", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal('Record Save Successfully.');
                    }
                    else if (promise.message == "Update") {
                        swal('Record Update Successfully.');
                    }
                    if (promise.message == "Error") {
                        swal('Record Save/Update Failed..!!.');
                    }
                    $state.reload();
                    $scope.edt = false;
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //============ edit data============
        $scope.Edit = function (obj) {

            var data = {
                "ESAETT_Id": obj.ESAETT_Id,
                "ASMAY_Id": obj.ASMAY_Id
            };
            apiService.create("SA_Exam_Titetable/Edit_TT", data).then(function (promise) {
                if (promise.edit_tt_list !== null && promise.edit_tt_list.length > 0) {
                    $scope.edt = true;
                    $scope.ESAETT_Id = promise.edit_tt_list[0].esaetT_Id;
                    $scope.ASMAY_Id = promise.edit_tt_list[0].asmaY_Id;
                    $scope.EME_Id = promise.edit_tt_list[0].emE_Id;
                    $scope.ESAUE_Id = promise.edit_tt_list[0].esauE_Id;
                    $scope.AMCO_Id = promise.edit_tt_list[0].amcO_Id;
                    $scope.AMB_Id = promise.edit_tt_list[0].amB_Id;
                    $scope.AMSE_Id = promise.edit_tt_list[0].amsE_Id;
                    $scope.ESAETT_FromDate = new Date(promise.edit_tt_list[0].esaetT_FromDate);
                    $scope.ESAETT_ToDate = new Date(promise.edit_tt_list[0].esaetT_ToDate);
                  
                    $scope.yearlst = promise.yearlst;
                    $scope.examlist = promise.examlist;
                    $scope.university_examlist = promise.university_examlist;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semesterlist = promise.semesterlist;
                 
                 

                    if(promise.edit_tt_details > 0 || promise.edit_tt_details != null)
                    {
                        $scope.examslotlist = promise.examslotlist;

                        console.log($scope.examslotlist);

                        $scope.subjectlist = promise.subjectlist;
                        $scope.subjectschemalist = promise.subjectschemalist;


                        $scope.transrows = [];
                        $scope.editdetails = [];
                        $scope.editdetails = promise.edit_tt_details
                        angular.forEach($scope.editdetails, function (qq) {
                            $scope.transrows.push({
                                ESAESLOT_Id: qq.esaesloT_Id, ACSS_Id: qq.acsS_Id,
                                ISMS_Id: qq.ismS_Id, ESAETT_ExamDate: new Date(qq.esaetT_ExamDate)

                            })
                            
                        })
                    }
                  
                
                }
            });
        };
        //========== active/De-active==================
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.ESAETT_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAETT_Id": user.ESAETT_Id,
                "ESAETT_ActiveFlg": user.ESAETT_ActiveFlg
            };

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SA_Exam_Titetable/Deactive_TT", data).then(function (promise) {
                            if (promise.message === 'true') {
                                swal("Record Activated Successfully");
                            }
                            else if (promise.message === 'false') {
                                swal("Record De-Activated Successfully");
                            } else if (promise.message === 'error') {
                                swal("Operation Failed..!!");
                            }
                            $state.reload();
                        });

                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }   

                });
        };

        //================== View TT Details=
        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "ESAETT_Id": option.ESAETT_Id
               
            };
            apiService.create("SA_Exam_Titetable/viewTTdetails", data)
                .then(function (promise) {

                    if (promise.view_tt_details.length > 0) {

                        $scope.view_tt_details = promise.view_tt_details;
                        $('#myModalCoverview').modal('show');
                      
                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };
      
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addprrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i == $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted1 = true;
            }

        };
        $scope.removeprrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deleteprrows(data);
            }
            if ($scope.transrows.length == 0) {
            }
        };
        
        
        //==========
        $scope.cleardata = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        //=================







    };
})();