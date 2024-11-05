(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterLifeSkillandAreasMappingController', MasterLifeSkillandAreasMappingController);

    MasterLifeSkillandAreasMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http'];

    function MasterLifeSkillandAreasMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.GradePoint = "";
        $scope.BindData = function () {

            
            apiService.getDATA("MasterLifeSkillNameAndAreaMapping/Getdetails").
       then(function (promise) {
           
           $scope.fillskill = promise.fillskill;
           $scope.fillskillarea = promise.fillskillarea;
           $scope.fillMastergrade = promise.fillMastergrade;
           $scope.gridOptions.data = promise.filldata;
       })

        }

     


        $scope.Gradepointselect = function (fillgradename, emgD_Id) {
            
            
            angular.forEach(fillgradename, function (tr) {
                if(tr.emgD_Id==emgD_Id)
                {
                    $scope.GradePoint = tr.emgD_GradePoints;
                }
            })
           
            //$scope.GradePoint = fillgrade[parseInt(EMGR_Id)].emgD_GradePoints;


        }

        $scope.MasterGradeselect = function (emgR_Id) {
            $scope.GradePoint = "";
            $scope.emgD_Id = "";
            var emgR_Id = emgR_Id;
            apiService.getURI("MasterLifeSkillNameAndAreaMapping/getgrade/", emgR_Id).
            then(function (promise) {

                $scope.fillgradename = promise.fillgradename;
                if($scope.CCE_MLSAMap_id>0)
                {
                    $scope.emgD_Id = temp_emgd_id;
                    angular.forEach($scope.fillgradename, function (gd) {
                        if(gd.emgD_Id==$scope.emgD_Id)
                        {
                            gd.Selected = true;
                            $scope.Gradepointselect($scope.fillgradename, $scope.emgD_Id)
                        }
                    })

                }
                

            });


        }
        $scope.submitted = false;
        $scope.Savedata = function () {

            if ($scope.myForm.$valid) {

                var data = {
                    "CCE_MLSAMap_id": $scope.CCE_MLSAMap_id,
                    "CCE_MLS_ID": $scope.CCE_MLS_ID,
                    "CCE_MLSA_ID": $scope.CCE_MLSA_ID,
                    "CCE_Indicator_Description": $scope.CCE_Indicator_Description,
                    "EMGD_Id": $scope.emgD_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterLifeSkillNameAndAreaMapping/savedata", data).
                             then(function (promise) {


                                 if (promise.returnval === true) {

                                     if (promise.ccE_MLSAMap_id == 0 || promise.ccE_MLSAMap_id < 0) {
                                         swal('Record saved successfully');
                                     }

                                     else if (promise.ccE_MLSAMap_id > 0) {
                                         swal('Record updated successfully');
                                     }

                                 }
                                 else if (promise.returnduplicatestatus === 'Duplicate') {

                                     swal('Record already exist');
                                 }
                                 else {
                                     if (promise.ccE_MLSAMap_id == 0 || promise.ccE_MLSAMap_id < 0) {
                                         swal('Failed to save, please contact administrator');
                                     }
                                     else if (promise.ccE_MLSAMap_id > 0) {
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


        $scope.deactive = function (deactiveRecord) {

            
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ccE_MLSAMap_ActiveFlag == true) {
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

                   apiService.create("MasterLifeSkillNameAndAreaMapping/deactivate", deactiveRecord).
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




        $scope.cancel = function () {
            $scope.CCE_MLSAMap_id = "";
            $scope.CCE_MLS_ID = "";
            $scope.CCE_MLSA_ID = "";
            $scope.CCE_Indicator_Description = "";
            $scope.emgD_Id = "";
            $scope.GradePoint = ""; 
           
            $scope.emgR_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.gridApi.grid.clearAllFilters();
            $scope.search = "";

        }

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ccE_MLS_NAME', displayName: 'Life Skill Name' },
              { name: 'ccE_MLSA_NAME', displayName: 'Life Skill Area' },
               { name: 'ccE_Indicator_Description', displayName: 'Description' },
              { name: 'emgR_NAME', displayName: 'Grade Name' },


               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ccE_MLSAMap_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ccE_MLSAMap_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };
        var temp_emgd_id="";

        $scope.Editexammasterdata = function (EditRecord) {
            
            var ccE_MLSAMap_id = EditRecord.ccE_MLSAMap_id;
            apiService.getURI("MasterLifeSkillNameAndAreaMapping/editdetails/", ccE_MLSAMap_id).
            then(function (promise) {
                
                //$scope.CCE_MLS_ID = promise.editlist[0].ccE_MLS_ID;
                //$scope.CCE_MLS_NAME = promise.editlist[0].ccE_MLS_NAME;
                //$scope.CCE_MLS_CODE = promise.editlist[0].ccE_MLS_CODE;
                //$scope.CCE_MLS_Flag = promise.editlist[0].ccE_MLS_Flag;
                $scope.CCE_MLSAMap_id = promise.editlist[0].ccE_MLSAMap_id
                $scope.CCE_MLS_ID = promise.editlist[0].ccE_MLS_ID;
                $scope.CCE_MLSA_ID = promise.editlist[0].ccE_MLSA_ID;
                $scope.CCE_Indicator_Description = promise.editlist[0].ccE_Indicator_Description;
              
                $scope.GradePoint = promise.editlist[0].emgR_Point; 
                $scope.emgR_Id = promise.editlist[0].emgR_Id;

                $scope.MasterGradeselect($scope.emgR_Id);
                $scope.emgD_Id = promise.editlist[0].emgD_Id;
                temp_emgd_id = promise.editlist[0].emgD_Id;
               
            })
        };






    }
})();
