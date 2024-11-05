
(function () {
    'use strict';
    angular
.module('app')
.controller('mastersubsubjectController', mastersubsubjectController)

    mastersubsubjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function mastersubsubjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

      //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("mastersubsubject/Getdetails").
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
              { name: 'emsS_SubSubjectName', displayName: 'Sub-Subject Name' },
              { name: 'emsS_SubSubjectCode', displayName: 'Sub-Subject Code' },
              { name: 'emsS_Order', displayName: 'Sub-Subject Order', type: 'number' },
             
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                '<a ng-if="row.entity.emsS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.emsS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
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
                if (value.EMSS_ID !== 0) {
                    orderarray[key].emsS_Order = key + 1;
                }
            });
            var data = {
                subsubjectDTO: orderarray,
            }
            apiService.create("mastersubsubject/validateordernumber", data).
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




        //to delete the data

        $scope.Deletemastersubsubjectdata = function (DeleteRecord) {
            var confirmPopup = confirm('Are you sure you want to delete this item?');
            if (confirmPopup === true) {
                $scope.deleteId = DeleteRecord.emsS_Id;
                var MdeleteId = $scope.deleteId;
                apiService.DeleteURI("mastersubsubject/MasterDeleteModulesDTO", MdeleteId)
                $scope.$apply();
                swal("Record Deleted Successfully");
                $scope.saved = "Record Deleted Successfully";
                $scope.BindData();
            }
        };




        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            
            var MEditId = EditRecord.emsS_Id;
            apiService.getURI("mastersubsubject/editdeatils/", MEditId).
            then(function (promise) {
                $scope.name = promise.editlist[0].emsS_SubSubjectName; 
                $scope.EMSS_ID = promise.editlist[0].emsS_Id;
                $scope.subcode = promise.editlist[0].emsS_SubSubjectCode;
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
                    "emsS_Id": $scope.EMSS_ID,
                    "EMSS_SubSubjectName": $scope.name,
                    "EMSS_SubSubjectCode": $scope.subcode,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                    apiService.create("mastersubsubject/savedetails", data).
                             then(function (promise) {
                                 $scope.newuser = promise.mastersubsubject;
                                 if (promise.returnval === true) {
                                     // swal('Data successfully Saved');
                                     if (promise.emsS_Id == 0 || promise.emsS_Id < 0) {
                                         swal('Record saved successfully');
                                     }
                                         // else if(promise.emcA_Id!="" && promise.emcA_Id>0 && promise.emcA_Id!=undefined)
                                     else if (promise.emsS_Id > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {
                                     //  swal('Recards AlReady Exist !');
                                     swal('Record already exist');
                                 }
                                 else {
                                     //swal('Data Not Saved !');
                                     if (promise.emsS_Id == 0 || promise.emsS_Id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.emsS_Id > 0) {
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
                                 //else if (promise.returnduplicatestatus===true || promise.returnval===false) {
                                 //    swal('Record is Duplicate','Failed')
                                 //}
                                 //else {
                                     
                                 //    swal('Record Not Saved/Updated Successfully', 'Failed');
                                 //    $scope.cancel();
                                 //    $scope.BindData();
                                 //}
                             })
                }
            };
        //to cleat the data
            //$scope.cancel = function () {
            //    $scope.EMSE_ID = "";
            //$scope.name = "";           
            //$state.reload();
        //}
        $scope.cancel = function () {
            $scope.EMSS_ID = 0;
            $scope.subcode = "";
            $scope.name = "";
            //$scope.exorder = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";
            // $state.reload();
        }

        //to active or deactive 
            $scope.deactive = function (deactiveRecord) {
                var mgs = "";
                var confirmmgs = "";
                if (deactiveRecord.emsS_ActiveFlag == true) {
                    // mgs = "Deactive";
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

                       apiService.create("mastersubsubject/deactivate", deactiveRecord).
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


        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].emsS_Order = Number(index) + 1;

                }
            }
        };

    }

})();