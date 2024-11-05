


(function () { 
    'use strict';
    angular
.module('app')
.controller('mastersubexamController', mastersubexamController)

    mastersubexamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function mastersubexamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};


      //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("mastersubexam/Getdetails").
       then(function (promise) {
           $scope.gridOptions.data = promise.getlist;
           $scope.grouptypeListOrder = promise.getlist;
       })
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
               { name: 'emsE_SubExamName', displayName: 'Sub-Exam Name' },
               { name: 'emsE_SubExamCode', displayName: 'Sub-Exam Code' },
               { name: 'emsE_SubExamOrder', displayName: 'Sub-Exam Order', type: 'number' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                '<a ng-if="row.entity.emsE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.emsE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
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
                if (value.EMSE_Id !== 0) {
                    orderarray[key].emsE_SubExamOrder = key + 1;
                }
            });
            var data = {
                subexamDTO: orderarray,
            }
            apiService.create("mastersubexam/validateordernumber", data).
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



        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.emsE_ActiveFlag === true) {
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

                   apiService.create("mastersubexam/deactivate", deactiveRecord).
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
                                   //swal(mgs + " " + "Failed");
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




        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var MEditId = EditRecord.emsE_Id;
            apiService.getURI("mastersubexam/editdeatils", MEditId).
            then(function (promise) {
                $scope.EMSE_Id = promise.editlist[0].emsE_Id;
                $scope.subexname = promise.editlist[0].emsE_SubExamName;
                $scope.subexcode = promise.editlist[0].emsE_SubExamCode;
            })
        };

            $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            $scope.interacted = function (field) {
                return $scope.submitted || field.$dirty;
            };

        // TO Save The Data
            $scope.submitted = false;
            $scope.saveddata = function () {
                $scope.submitted = true;
                if ($scope.myForm.$valid) {
              
                var data = {
                    "EMSE_Id": $scope.EMSE_Id,
                    "EMSE_SubExamName": $scope.subexname,
                    "EMSE_SubExamCode": $scope.subexcode
                  
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("mastersubexam/savedetails", data).
                             then(function (promise) {
                                 $scope.newuser = promise.mastersubexam;
                                 if (promise.returnval === true) {
                                     // swal('Data successfully Saved');
                                     if (promise.emsE_Id == 0 || promise.emsE_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                         // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                     else if (promise.emsE_Id > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {
                                     //  swal('Recards AlReady Exist !');
                                     swal('Record already exist');
                                 }
                                 else {
                                     //swal('Data Not Saved !');
                                     if (promise.emsE_Id == 0 || promise.emsE_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.emsE_Id > 0) {
                                         swal('Failed to update, please contact administrator');
                                     }
                                 }
                                 $scope.cancel();
                                 $scope.BindData();
                                 //if (promise.returnval === true) {
                                    
                                 //    swal('Record Saved/Updated Successfully', 'success');
                                 //    $scope.cancel();
                                 //    $scope.BindData();
                                 //}
                                 //else if (promise.returnduplicatestatus === true || promise.returnval === false) {
                                 //    swal('Record is Duplicate', 'Failed')
                                 //}
                                 //else {
                                     
                                 //    swal('Record Not Saved/Updated Successfully', 'Failed');
                                 //    $scope.cancel();
                                 //    $scope.BindData();
                                 //}
                             })
                }

        };


        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].emsE_SubExamOrder = Number(index) + 1;

                }
            }
        };




            $scope.cancel = function () {
                $scope.EMSE_Id = 0;
                $scope.subexcode = "";
                $scope.subexname = "";
                //$scope.exorder = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.gridApi.grid.clearAllFilters();
                $scope.search = "";
                // $state.reload();
            }
        //    $scope.cancel = function () {
        //    $scope.EMSE_ID = "";
        //    $scope.name = "";           
        //    $state.reload();
        //}
            

    }

})();