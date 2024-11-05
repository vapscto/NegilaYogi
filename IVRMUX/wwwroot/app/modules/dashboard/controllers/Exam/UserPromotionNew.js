
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamUserPromotionNewController', ExamUserPromotionNewController)

    ExamUserPromotionNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamUserPromotionNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {

        $scope.searchValue = "";
        $scope.currentPage = 1; 
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse3 = !$scope.reverse3;
        }
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";
        $scope.currentPage3 = 1;
        $scope.EYCESU_MarksPublishApproverFlg = false;
        $scope.ediflag = false;
        //================================Load Data.
        $scope.BindData = function () {

            apiService.getDATA("ExamMarksprocesscondition/Getdetails").
                then(function (promise) {
                    $scope.year_list = promise.yearlist;
                    $scope.userlist = promise.userlist;
                    console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    console.log($scope.userlist);
                    $scope.userpromotionlist = promise.userPromotionlist;

                    $scope.gridOptions.data = promise.userPromotionlist;

                });



        };

        //==========================Get Category...
        //$scope.minDatemf = new Date();
        $scope.get_category = function (yr_id) {

            $scope.EYCESU_MarksEntryFromDate = "";
            $scope.EYCESU_MarksEntryToDate = "";
            $scope.EYCESU_MarksProcessFromDate = "";
            $scope.EYCESU_MarksProcessToDate = "";
            $scope.EYCESU_MarksPublishDate = "";
            $scope.EYC_Id = "";
            $scope.EME_Id = "";
            if ($scope.ediflag == true) {
                $scope.ivrmulF_Id = "";
            }
            else {
                $scope.allmi = $scope.userlist.every(function (itm) {
                    return itm.ivrmulF_Id = false;;
                });
            }
           
          
          

          
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            };
            apiService.create("ExamMarksprocesscondition/get_category", data).
                then(function (promise) {
                    $scope.category_list = promise.categorylist;

                    angular.forEach($scope.year_list, function (yea) {
                        if (parseInt(yea.asmaY_Id) === parseInt($scope.ASMAY_Id)) {
                            $scope.minMarksEntryTodate = new Date(yea.asmaY_From_Date);
                            $scope.maxEntrydate = new Date(yea.asmaY_To_Date);
                        }
                    });



                    if (promise.categorylist == "" || promise.categorylist == null) {
                        swal("No Categories Are Mapped To Selected Academic Year");
                        $scope.ASMAY_Id = "";
                    }
                });
        };

        //=========================Get Exam
        $scope.get_subjects = function (cat_id) {
            //$scope.ivrmulF_Id = "";
            $scope.allmi = $scope.userlist.every(function (itm) {
                return itm.ivrmulF_Id = false;;
            });

            if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != null && $scope.ASMAY_Id != undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EYC_Id": $scope.EYC_Id,
                };
                apiService.create("ExamMarksprocesscondition/get_subjects", data).
                    then(function (promise) {
                        $scope.exam_list = promise.examlist;
                    });
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.EYC_Id = "";
            }
        };

        //==========================Change user
        $scope.chech_username = function () {
            //$scope.ivrmulF_Id = "";
            $scope.allmi = $scope.userlist.every(function (itm) {
                return itm.ivrmulF_Id = false;;
            });

        }

        $scope.clear = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //=====================================Save records
        $scope.savedata = function () {
            $scope.IVRMULF_IdTemp = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var IVRMULF_Id = 0;
                if ($scope.eycesU_Id > 0) {
                    IVRMULF_Id = $scope.ivrmulF_Id.ivrmulF_Id;
                }
                else {
                    angular.forEach($scope.userlist, function (yea) {
                        if (yea.selected == true) {
                            $scope.IVRMULF_IdTemp.push({
                                IVRMULF_Id: yea.ivrmulF_Id

                            });
                        }
                    });
                }
                
                var data = {
                    "EYCESU_Id": $scope.eycesU_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EYC_Id": $scope.EYC_Id,
                    "EME_Id": $scope.EME_Id,
                    "EYCESU_MarksEntryFromDate": new Date($scope.EYCESU_MarksEntryFromDate).toDateString(),
                    "EYCESU_MarksEntryToDate": new Date($scope.EYCESU_MarksEntryToDate).toDateString(),
                    "EYCESU_MarksProcessFromDate": new Date($scope.EYCESU_MarksProcessFromDate).toDateString(),
                    "EYCESU_MarksProcessToDate": new Date($scope.EYCESU_MarksProcessToDate).toDateString(),
                    "EYCESU_MarksPublishDate": new Date($scope.EYCESU_MarksPublishDate).toDateString(),
                    "IVRMULF_Id": IVRMULF_Id ,
                    "ivrmulF_IdList": $scope.IVRMULF_IdTemp,
                    "EYCESU_MarksPublishApproverFlg": $scope.EYCESU_MarksPublishApproverFlg,
                };

                apiService.create("ExamMarksprocesscondition/saveUserPromotionDataNew", data).
                    then(function (promise) {

                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.eycesU_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully ! Duplicate Count:' + promise.dulicateCount + ' Saved Count : ' + promise.savedcount+'');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.eycesU_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal('Record Already exist Duplicate Count:' + promise.dulicateCount + '');
                            }
                           
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        }

        //========================================active / decative records
        $scope.deactiveY = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.eycesU_ActiveFlg == false) {

                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
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
                        apiService.create("ExamMarksprocesscondition/deActiveUserPromotion/", newuser1).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                        $state.reload();
                    }
                })
        }
        $scope.mi_click = function () {
            
            $scope.allmi = $scope.userlist.every(function (itm) {
                return itm.selected;
            });
            

        };
        $scope.isRequiredmi = function () {

            return !$scope.userlist.some(function (options) {
                return options.selected;
            });

        };
        $scope.all_miid = function (ac) {

            var toggleStatus = $scope.allmi;
            angular.forEach($scope.userlist, function (itm) {
                itm.selected = toggleStatus;
            });
           
        };

        //=============================Edit records
        $scope.edit = function (data) {
            $scope.EYCESU_MarksPublishApproverFlg = false;
            apiService.create("ExamMarksprocesscondition/editUserPromotion", data).then(function (promise) {

                $scope.ASMAY_Id = promise.editPromotionUserlist[0].asmaY_Id;
                $scope.eycesU_Id = promise.editPromotionUserlist[0].eycesU_Id;
                //$scope.get_category();
                $scope.category_list = promise.categorylist;

                $scope.EYC_Id = promise.editPromotionUserlist[0].eyC_Id;
                $scope.exam_list = promise.examlist;

                //$scope.get_subjects();
                $scope.EME_Id = promise.editPromotionUserlist[0].emE_Id;

                if (promise.editPromotionUserlist[0].eycesU_MarksEntryFromDate != null || promise.editPromotionUserlist[0].eycesU_MarksEntryFromDate != "") {
                    $scope.EYCESU_MarksEntryFromDate = new Date(promise.editPromotionUserlist[0].eycesU_MarksEntryFromDate);
                }
                if (promise.editPromotionUserlist[0].eycesU_MarksEntryToDate != null || promise.editPromotionUserlist[0].eycesU_MarksEntryToDate != "") {
                    $scope.EYCESU_MarksEntryToDate = new Date(promise.editPromotionUserlist[0].eycesU_MarksEntryToDate);
                }
                if (promise.editPromotionUserlist[0].eycesU_MarksProcessFromDate != null || promise.editPromotionUserlist[0].eycesU_MarksProcessFromDate != "") {
                    $scope.EYCESU_MarksProcessFromDate = new Date(promise.editPromotionUserlist[0].eycesU_MarksProcessFromDate);
                }
                if (promise.editPromotionUserlist[0].eycesU_MarksProcessToDate != null || promise.editPromotionUserlist[0].eycesU_MarksProcessToDate != "") {
                    $scope.EYCESU_MarksProcessToDate = new Date(promise.editPromotionUserlist[0].eycesU_MarksProcessToDate);
                }
                if (promise.editPromotionUserlist[0].eycesU_MarksPublishDate != null || promise.editPromotionUserlist[0].eycesU_MarksPublishDate != "") {
                    $scope.EYCESU_MarksPublishDate = new Date(promise.editPromotionUserlist[0].eycesU_MarksPublishDate);
                }


                $scope.ivrmulF_Id = promise.editPromotionUserlist[0];
                $scope.ivrmulF_Id.ivrmulF_Id = promise.editPromotionUserlist[0].ivrmulF_Id;
                $scope.ediflag = true;
                $scope.EYCESU_MarksPublishApproverFlg = promise.editPromotionUserlist[0].eycESU_MarksPublishApproverFlg;
            });
        }



        //==========================Check Date Conditions....
        $scope.check_MarksEntryFdate = function () {
            debugger;
            $scope.EYCESU_MarksEntryToDate = "";
            $scope.EYCESU_MarksProcessFromDate = "";
            $scope.EYCESU_MarksProcessToDate = "";
            $scope.EYCESU_MarksPublishDate = "";

            $scope.marksentryfdate = new Date($scope.EYCESU_MarksEntryFromDate);
            $scope.minEntrydate = new Date(
                $scope.marksentryfdate.getFullYear(),
                $scope.marksentryfdate.getMonth(),
                $scope.marksentryfdate.getDate() + 1);
        };

        $scope.setEntryTodate = function () {
            debugger;
            $scope.EYCESU_MarksProcessFromDate = "";
            $scope.EYCESU_MarksProcessToDate = "";
            $scope.EYCESU_MarksPublishDate = "";

            $scope.marksEntryTodate = new Date($scope.EYCESU_MarksEntryToDate);
            $scope.minMarksEntryTodate = new Date(
                $scope.marksEntryTodate.getFullYear(),
                $scope.marksEntryTodate.getMonth(),
                $scope.marksEntryTodate.getDate() + 1);
        };

        $scope.check_MarksProcessFdate = function () {
            $scope.EYCESU_MarksProcessToDate = "";
            $scope.EYCESU_MarksPublishDate = "";

            $scope.marksprocessdate = new Date($scope.EYCESU_MarksProcessFromDate);
            $scope.minMarksProcessTodate = new Date(
                $scope.marksprocessdate.getFullYear(),
                $scope.marksprocessdate.getMonth(),
                $scope.marksprocessdate.getDate() + 1);
        };

        $scope.check_MarksProcessTodate = function () {
            $scope.EYCESU_MarksPublishDate = "";
            $scope.marksPublishDate = new Date($scope.EYCESU_MarksProcessToDate);
            $scope.minMarksPublishdated = new Date(
                $scope.marksPublishDate.getFullYear(),
                $scope.marksPublishDate.getMonth(),
                $scope.marksPublishDate.getDate() + 1);
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: '6%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '11%', displayName: 'Academic Year' },
                { name: 'emcA_CategoryName', width: '12%', displayName: 'Category Name' },
                { name: 'emE_ExamName', width: '15%', displayName: 'Exam Name' },
                { name: 'hrmE_EmployeeFirstName', width: '15%', displayName: 'User Name' },
                { name: 'eycesU_MarksEntryFromDate', width: '12%', displayName: 'Marks Entry From Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycesU_MarksEntryToDate', width: '12%', displayName: 'Marks Entry To Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycesU_MarksProcessFromDate', width: '15%', displayName: 'Marks Process From Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycesU_MarksProcessToDate', width: '15%', displayName: 'Marks Process To Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                { name: 'eycesU_MarksPublishDate', width: '15%', displayName: 'Marks Publish Date', type: 'date', filterCellFiltered: true, cellFilter: 'date:"dd-MM-yyyy"' },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.edit(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;'
                },
                {
                    field: 'id', name: '', width: '10%',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.edit(row.entity);"> <md-tooltip md-direction="down">Edit Now</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.eycesU_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactiveY(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.eycesU_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactiveY(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

            }
        };



    }

})();