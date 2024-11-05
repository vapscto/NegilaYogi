(function () {
    'use strict';
    angular
.module('app')
.controller('ExamTTTransSettingsController', ExamTTTransSettingsController)

    ExamTTTransSettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamTTTransSettingsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                  { name: 'academicyear', displayName: 'Academic Year' },
                  { name: 'classname', displayName: 'Class Name' },
                  { name: 'sectionname', displayName: 'Section Name' },
                   { name: 'emG_GroupName', displayName: 'Group Category' },
                  { name: 'examname', displayName: 'Exam' },
                  { name: 'extT_FromDate', displayName: 'From Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },
                  { name: 'extT_EndDate', displayName: 'To Date', type: 'date', cellFilter: 'date:"dd-MM-yyyy"' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                  '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                '</div>'
               }
            ]

        };
        //TO  View Record
        $scope.viewrecordspopup = function (employee) {
            $scope.editEmployee = employee.extT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ExamTTTransSettings/getalldetailsviewrecords", pageid).
                    then(function (promise) {
                        $scope.viewrecordspopupdisplay = promise.viewdata;

                    })

        };

        $scope.deactive = function (groupData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupData.exttS_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("ExamTTTransSettings/deactivate", groupData).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                        $scope.viewrecordspopup(groupData);
                    }
                    else {
                        swal("Record is already been used !!!");
                    }
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.main_list_1 = [];
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.extT_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ExamTTTransSettings/editgetdetails", pageid).
            then(function (promise) {
                $scope.EXTT_Id = employee.extT_Id;
                $scope.ASMAY_Id = promise.listedit[0].asmaY_Id;
                // $scope.onselectAcdYear(promise.listedit[0].asmaY_Id);
                $scope.ASMCL_Id = promise.listedit[0].asmcL_Id;
                $scope.ASMS_Id = promise.listedit[0].asmS_Id;
                $scope.EME_Id = promise.listedit[0].emE_Id;
                $scope.EMG_Id = promise.listedit[0].emG_Id;
                $scope.myDate1 = new Date(promise.listedit[0].extT_FromDate);
                $scope.myDate2 = new Date(promise.listedit[0].extT_EndDate);
                //  $scope.ISMS_Id = promise.listedit[0].ismS_Id;
                //  $scope.exttS_Date = promise.listedit[0].exttS_Date;

                $scope.onselectAcdYear($scope.ASMAY_Id);

                $scope.onselectclass($scope.ASMCL_Id, $scope.ASMAY_Id);

                $scope.onselectSection($scope.ASMS_Id, $scope.ASMCL_Id, $scope.ASMAY_Id);



                

                $scope.main_list_ = [];
                for (var i = 0; i < promise.subject_name.length ; i++) {
                    var a = 0;
                    for (var j = 0; j < promise.listedit.length ; j++) {
                        if (promise.subject_name[i].ismS_Id == promise.listedit[j].ismS_Id) {
                            $scope.main_list_.push({ ismS_Id: promise.subject_name[i].ismS_Id, check_save: 1, exttS_Date: new Date(promise.listedit[j].exttS_Date), ettS_Id: promise.listedit[j].ettS_Id, ismS_SubjectName: promise.subject_name[i].ismS_SubjectName });
                            a = 1;
                        }

                    }
                    if (a == 0) {
                        $scope.main_list_.push({ ismS_Id: promise.subject_name[i].ismS_Id, check_save: 0, exttS_Date: null, ettS_Id: '', ismS_SubjectName: promise.subject_name[i].ismS_SubjectName });
                    }
                }

                $scope.main_list = ''
                $scope.main_list = $scope.main_list_;



            })
        }


        $scope.BindData = function () {
            apiService.getDATA("ExamTTTransSettings/getdetails").
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.subject_group = promise.subject_group;
                $scope.examlist = promise.examlist;
                $scope.time_slot = promise.time_slot;
                $scope.gridOptions.data = promise.detailslist;
            })
        };


        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamTTTransSettings/onselectAcdYear", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("ExamTTTransSettings/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.onselectSection = function (ASMS_Id ,ASMCL_Id,ASMAY_Id) {
            var data = {
                "ASMS_Id":ASMS_Id, // $scope.ASMS_Id,
                "ASMCL_Id":ASMCL_Id,// $scope.ASMCL_Id,
                "ASMAY_Id": ASMAY_Id //$scope.ASMAY_Id
        }
        apiService.create("ExamTTTransSettings/onselectSection", data).
        then(function (promise) {
            $scope.subject_group = promise.subject_group;
            $scope.examlist = promise.examlist;

        })
    };

    $scope.onselectSubject = function () {
        var data = {
            "ASMS_Id": $scope.ASMS_Id,
            "ASMCL_Id": $scope.ASMCL_Id,
            "ASMAY_Id": $scope.ASMAY_Id,
            "EMG_Id": $scope.EMG_Id,
            "EME_Id": $scope.EME_Id
        }
        apiService.create("ExamTTTransSettings/onselectSubject", data).
        then(function (promise) {

            $scope.main_list = promise.subject_name;
            $scope.subject_name = promise.subject_name;
            $scope.time_slot = promise.time_slot;
        })
    };

    $scope.onselectSubSubject = function (obj, ismS_Id) {
        var data = {
            "ASMS_Id": $scope.ASMS_Id,
            "ASMCL_Id": $scope.ASMCL_Id,
            "ASMAY_Id": $scope.ASMAY_Id,
            "ISMS_Id": ismS_Id
        }
        apiService.create("ExamTTTransSettings/onselectSubSubject", data).
        then(function (promise) {

            obj.subSubject = promise.subSubject;
        })
    };



    $scope.submitted = false;
    $scope.savedetail = function () {
        $scope.submitted = true;
        

        if ($scope.myForm.$valid) {
            $scope.arraylist = [];
            angular.forEach($scope.main_list, function (option) {
                if (!!option.check_save) {
                    $scope.arraylist.push({ ismS_Id: option.ismS_Id, check_save: option.check_save, exttS_Date: new Date(option.exttS_Date).toDateString(), ettS_Id: option.ettS_Id, ismS_SubjectName: option.ismS_SubjectName });
                }
                //else {
                //    $scope.arraylist.push({ ismS_Id: option.ismS_Id, check_save: 0, exttS_Date: null, ettS_Id: 0, ismS_SubjectName: option.ismS_SubjectName });
                //}
            })

            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id,
                "EMG_Id": $scope.EMG_Id,
                "EXTT_FromDate": new Date($scope.myDate1).toDateString(),
                "EXTT_EndDate": new Date($scope.myDate2).toDateString(),
                "TempararyArrayList": $scope.arraylist,
                "EXTT_Id": $scope.EXTT_Id
            }
            apiService.create("ExamTTTransSettings/savedetail", data).
                then(function (promise) {
                    if (promise.returnval === true) {

                        swal('Data Saved successfully');
                        $scope.cancel();
                        $scope.BindData();
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Records Already Exist !');
                    }
                    else {
                        swal('Data Not Saved !');
                    }

                });
        }
        else {
            $scope.submitted = false;
        }
    };

    $scope.cancel = function () {
        $scope.ASMAY_Id = '';
        $scope.ASMCL_Id = '';
        $scope.ASMS_Id = '';
        $scope.EME_Id = '';
        $scope.EMG_Id = '';
        $scope.EXTT_Id = 0;
        $scope.myDate1 = '';
        $scope.myDate2 = '';
        $scope.main_list = [];
        $scope.submitted = false;
        $scope.myForm.$setPristine();
        $scope.myForm.$setUntouched();
        $scope.search = "";
    };

    $scope.interacted = function (field) {

        return $scope.submitted;
    };
}

})();