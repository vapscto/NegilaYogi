
(function () {
    'use strict';
    angular
.module('app')
.controller('CoScholasticActivityAreasController', CoScholasticActivityAreasController)

    CoScholasticActivityAreasController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function CoScholasticActivityAreasController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        // var temp_exam_list = [];
       // $scope.EME_FinalExamFlag = false;
        $scope.Active_flag = true;
        $scope.BindData = function () {
            apiService.getDATA("CoScholasticActivityAreas/Getdetails").
       then(function (promise) {
           

           $scope.gridOptions.data = promise.exammastername;
           $scope.grouptypeListOrder = promise.exammastername;
           //temp_exam_list = promise.exammastername;
           //$scope.final_exm_count = 0;
           //angular.forEach(promise.exammastername, function (emd) {
           //    if (emd.emE_FinalExamFlag == true) {
           //        $scope.final_exm_count += 1;
           //    }
           //})

       })
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ccE_M_Sch_Area_Name', displayName: 'Activity Areas Name' },
              { name: 'ccE_M_Sch_Area_Order', displayName: 'Activity Areas Order' },
             // { name: 'Active_flag ', displayName: 'Co-Scholastic Activity Areas Flag', type: 'number' },
         //      {
         //          name: 'emE_FinalExamFlag', displayName: 'Final-Exam Flag', enableFiltering: false, enableSorting: false, cellTemplate:
         //'<div class="grid-action-cell">' +
         // '<a ng-if="row.entity.emE_FinalExamFlag == true" href="javascript:void(0)" > <i class="fa fa-check  text-green"></i></a>' +
         //  '<a ng-if="row.entity.emE_FinalExamFlag == false" href="javascript:void(0)" > <i class="fa fa-times  text-red"></i></i></a>' +
         // '</div>'
         //      },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.active_flag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.active_flag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };
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
                if (value.ccE_M_Sch_Area_Id !== 0) {
                    orderarray[key].CCE_M_Sch_Area_Order = key + 1;
                }
            });
            var data = {
                examDTO: orderarray,
            }
            apiService.create("CoScholasticActivityAreas/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);


                    }
                    $scope.cancel();
                    $scope.BindData();

                });
            // $state.BindData();
            // $scope.BindData();
        }

        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            //if (deactiveRecord.active_flag == true && deactiveRecord.active_flag == true) {
            //    swal("You Can Not Deactivate Scholastic Areas Record");
            //    //$scope.cancel();
            //    //$scope.BindData();
            //}
            //else {
                var mgs = "";
                var confirmmgs = "";
                if (deactiveRecord.active_flag == true) {
                    //mgs = "Deactive";
                    mgs = "Deactivate";
                    confirmmgs = "De-activated";

                }
                else {
                    // mgs = "Active";
                    mgs = "Activate";
                    confirmmgs = "Activated";

                }
                swal({
                    title: "Are you sure",
                    text: "Do you want to " + mgs + " record??????",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
               function (isConfirm) {
                   if (isConfirm) {

                       var config = {
                           headers: {
                               'Content-Type': 'application/json;'
                           }
                       }

                       apiService.create("CoScholasticActivityAreas/deactivate", deactiveRecord).
                           then(function (promise) {
                               if (promise.already_cnt == true) {
                                   swal("You Can Not Deactivate This Record,It Has Dependency");
                               }
                               else {
                                   if (promise.returnval == true) {
                                       swal("Record " + confirmmgs + " " + "successfully");
                                   }
                                   else {
                                       // swal(confirmmgs + " " + " successfully");
                                       swal("Record " + mgs + " Failed");
                                   }
                               }
                               //if (promise.returnval === true) {
                               //    swal(confirmmgs + ' Successfully');
                               //}
                               //else {
                               //    swal('Record Not  Activated/Deactivated');
                               //}
                               $scope.cancel();
                               $scope.BindData();
                               // $scope.clearid1();
                           })
                   }
                   else {
                       swal("Record " + mgs + " Cancelled");
                   }
               });
            
        };


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            
            var MEditId = EditRecord.ccE_M_Sch_Area_Id;
            apiService.getURI("CoScholasticActivityAreas/editdetails/", MEditId).
            then(function (promise) {
                $scope.CCE_M_Sch_Area_Id = promise.exammastername[0].ccE_M_Sch_Area_Id;
                //$scope.excode = promise.editlist[0].emE_ExamCode;
                $scope.CCE_M_Sch_Area_Name = promise.exammastername[0].ccE_M_Sch_Area_Name;
                //$scope.Active_flag = promise.editlist[0].active_flag;
               // $scope.EME_ActiveFlag = promise.editlist[0].emE_ActiveFlag;
            //    $scope.exorder = promise.editlist[0].emE_ExamOrder;
            //    if (promise.editlist[0].emE_FinalExamFlag == true) {
            //        $scope.final_exm_count = 0;
            //    }
            })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "CCE_M_Sch_Area_Id": $scope.CCE_M_Sch_Area_Id,
                    "CCE_M_Sch_Area_Name": $scope.CCE_M_Sch_Area_Name,
                    //"EME_ExamName": $scope.exname,
                    "Active_flag": $scope.Active_flag,
                    //   "EME_ExamOrder": $scope.exorder,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CoScholasticActivityAreas/savedetails", data).
                             then(function (promise) {
                                 
                                 $scope.newuser = promise.exammastername;
                                 //if (promise.returnval === true) {
                                 //    //$scope.cancel();
                                 //    //$scope.BindData();
                                 //    swal('Record Saved/Updated Successfully', 'success');
                                 //}
                                 //else if (promise.returnduplicatestatus === true || promise.returnval === false) {
                                 //    swal('Record is Duplicate', 'Failed')
                                 //}
                                 //else {
                                 //    //$scope.cancel();
                                 //    //$scope.BindData();
                                 //    swal('Record Not Saved/Updated Successfully', 'Failed');
                                 //}
                                 if (promise.returnval === true) {
                                     // swal('Data successfully Saved');
                                     if (promise.ccE_M_Sch_Area_Id == 0 || promise.ccE_M_Sch_Area_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                         // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                     else if (promise.ccE_M_Sch_Area_Id > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {
                                     //  swal('Recards AlReady Exist !');
                                     swal('Record already exist');
                                 }
                                 else {
                                     //swal('Data Not Saved !');
                                     if (promise.ccE_M_Sch_Area_Id == 0 || promise.ccE_M_Sch_Area_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.ccE_M_Sch_Area_Id > 0) {
                                         swal('Failed to update, please contact administrator');
                                     }
                                 }
                                 $scope.cancel();
                                 $scope.BindData();
                             })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.CCE_M_Sch_Area_Id = 0;            
            //$scope.exname = "";
            $scope.CCE_M_Sch_Area_Name = "";
            $scope.CCE_M_Sch_Area_Order = "";
            $scope.Active_flag = true;
            //angular.forEach($scope.gridOptions.data, function (emd) {
            //    if (emd.emE_FinalExamFlag == true) {
            //        $scope.final_exm_count += 1;
            //    }
            //})
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
            // $state.reload();
        }
        $scope.get_older = function () {
            
            $scope.BindData();
            //$scope.grouptypeListOrder = temp_exam_list;
        }


    }

})();