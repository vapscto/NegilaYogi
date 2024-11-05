(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterLifeSkillAreaController', MasterLifeSkillAreaController);

    MasterLifeSkillAreaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http'];

    function MasterLifeSkillAreaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.BindData = function () {
            
            apiService.getDATA("MasterLifeSkillArea/Getdetails").
       then(function (promise) {
           
           $scope.gridOptions.data = promise.filldata;
           $scope.grouptypeListOrder = promise.filldata;
       })
        };






        $scope.Savedata = function () {

            if ($scope.myForm.$valid) {

                var data = {
                    "CCE_MLSA_ID": $scope.CCE_MLSA_ID,
                    "CCE_MLSA_NAME": $scope.CCE_MLSA_NAME,
                    "CCE_MLSA_Order": $scope.CCE_MLSA_Order
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                
                apiService.create("MasterLifeSkillArea/savedata", data).
                             then(function (promise) {


                                 if (promise.returnval === true) {
                                   
                                     if (promise.ccE_MLSA_ID == 0 || promise.ccE_MLSA_ID < 0) {
                                         swal('Record saved successfully');
                                     }
                                      
                                     else if (promise.ccE_MLSA_ID > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {
                                    
                                     swal('Record already exist');
                                 }
                                 else {
                                     if (promise.ccE_MLSA_ID == 0 || promise.ccE_MLSA_ID < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.ccE_MLSA_ID > 0) {
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

        }


        $scope.cancel = function () {
            $scope.CCE_MLSA_ID = 0;
            $scope.CCE_MLSA_NAME = "";
            $scope.CCE_MLSA_Order = "";
            
           // $scope.EME_ActiveFlag = true;
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




        $scope.Editexammasterdata = function (EditRecord) {
            
            var ccE_MLSA_ID = EditRecord.ccE_MLSA_ID;
            apiService.getURI("MasterLifeSkillArea/editdetails/", ccE_MLSA_ID).
            then(function (promise) {
                
                $scope.CCE_MLSA_ID = promise.editlist[0].ccE_MLSA_ID;
                $scope.CCE_MLSA_NAME = promise.editlist[0].ccE_MLSA_NAME;
                $scope.CCE_MLSA_Order = promise.editlist[0].ccE_MLSA_Order;
               // $scope.flag = promise.editlist[0].flag;
               // $scope.EME_ActiveFlag = promise.editlist[0].emE_ActiveFlag;
                //$scope.exorder = promise.editlist[0].emE_ExamOrder;
                //if (promise.editlist[0].emE_FinalExamFlag == true) {
                //    $scope.final_exm_count = 0;
                //}
            })
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
                if (value.CCE_MLSA_ID !== 0) {
                    orderarray[key].ccE_MLSA_Order = key + 1;
                }
            });
            var data = {
                subexamDTO: orderarray,
            }
            apiService.create("MasterLifeSkillArea/validateordernumber", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);


                    }
                    $scope.cancel();
                    $scope.BindData();
                });
            //$state.BindData();
            // $scope.BindData();
        }





        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            
            
                var mgs = "";
                var confirmmgs = "";
                if (deactiveRecord.ccE_MLSA_ActiveFlag == true) {
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

                       apiService.create("MasterLifeSkillArea/deactivate", deactiveRecord).
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





        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ccE_MLSA_NAME', displayName: 'Life Skill Area Name' },
              { name: 'ccE_MLSA_Order', displayName: 'Life Skill Area Order' },
              
             
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ccE_MLSA_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ccE_MLSA_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };



    }
})();
