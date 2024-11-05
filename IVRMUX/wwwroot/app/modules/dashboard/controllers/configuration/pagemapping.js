(function () {
    'use strict';
    angular
.module('app')
.controller('MasterPageModuleMappingController', MasterPageModuleMappingController)

    MasterPageModuleMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', 'superCache', '$mdDialog']
    function MasterPageModuleMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache, $mdDialog) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterpage/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/rolemaster/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };


        $scope.page1 = "pag1";
        $scope.page2 = "pag2";
        $scope.page3 = "pag3";

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;

        $scope.itemsPerPage1 = 10;
       $scope.itemsPerPage2 = 10;
       $scope.itemsPerPage3 = 10;
      
        $scope.pagesrecord = {};
        var stuDelRecord = {};

        $scope.modulefill = function () {

            $scope.reverse = true;
            $scope.reverse1 = true;
            $scope.reverse2 = true;
            $scope.reverse3 = true;
            $scope.reverse4 = true;

            var pageid = 2;
            apiService.getURI("MasterPageModuleMapping/getalldetails", pageid).
        then(function (promise) {
            $scope.arrlist = promise.fillmodule;
            $scope.user = promise.fillpagesdata;
             $scope.students = promise.thirdgriddata;

           
            $scope.secondgrid = [];

        })

            
        }

        $scope.getmoduledetails = function (arrlist) {
            $scope.firstgrid = true;
            $scope.previousgrid = true;
            var moduleid = $scope.IVRMM_Id;
            var pageid = 2;
            apiService.getURI("MasterPageModuleMapping/getsaveddetails", moduleid).
        then(function (promise) {

            if (promise.fillmodule.length > 0)
            {
                $scope.previousgrid = promise.fillmodule;
                $scope.user = promise.fillpagesdata;
            }
            else
            {
                $scope.previousgrid = false;
            }
        });
        }

        $scope.clearid=function()
        {
            //$scope.firstgrid = false;
            //$scope.gridview2 = false;
            //$scope.IVRMM_Id = "";

            $state.reload();
        }


        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};

        $scope.addtocart = function (record) {
            $scope.gridview2 = true;
            if ($scope.secondgrid.indexOf(record) === -1) {
                $scope.secondgrid.push(record);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
           
        };

      
      
        $scope.currenrow = {};
        $scope.deletesecondgriddata = function (ev, index, stuDelRecord, currenrow) {
            

            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
      function (isConfirm) {
          if (isConfirm) {
              $("#check-" + stuDelRecord.ivrmP_Id).attr('checked', false);
              $scope.$apply(function () {
                  $scope.secondgrid.splice($scope.secondgrid.indexOf(stuDelRecord), 1);
              });
              swal("Row as been removed from list Successfully");
          }
          else {
              swal("Record Deletion Cancelled");
          }
      });




        };

        $scope.deletrec = function (employee, SweetAlert) {

            $scope.editEmployee = employee.ivrmmP_Id;
            var modulepageid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterPageModuleMapping/deletemodpages", modulepageid).
                    then(function (promise) {

                        $scope.students = promise.thirdgriddata;

                        swal(promise.returnval)
                        $state.reload();

                        //if (promise.returnval == true) {
                        //    swal('Record Deleted Successfully!', 'success');
                        //    $state.reload();
                        //}
                        //else {
                        //    swal('Record Not Deleted Successfully!', 'Failed');
                        //    $state.reload();
                        //}
                    })
                }
                else {
                    swal("Record Deletion Cancelled")
                    //swal(promise.returnval)
                    //$state.reload();
                }
            });
        }

       
        $scope.savadata = function (pagesrecord) {
            //pagesrecord = $scope.pages;

            var array = $.map(pagesrecord, function (value, index) {
                return [value];
            });

            var data = {
                "IVRMM_Id": $scope.IVRMM_Id,
                //"IVRMP_Id": $scope.IVRMP_Id,
                // "savetmpdata": $scope.secondgrid
                "IVRMMP_Id": $scope.IVRMMP_Id,
                savetmpdata:array
            }

            apiService.create("MasterPageModuleMapping/", data).
            then(function (promise) {
                
                $scope.students = promise.fillpagesdata;

                swal(promise.returnval)
                $state.reload();

                //if (promise.returnval == true) {
                //    swal('Record Saved/Updated Successfully', 'success');
                //    $state.reload();
                //}
                //else {
                //    swal('Record Not Saved/Updated Successfully', 'Failed');
                //    $state.reload();
                //}
            })

        };


        $scope.orderPage = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.orderPageMaped = function (keyname) {
            if ($scope.reverse2 == true) {
                $scope.reverse2 = !$scope.reverse2;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
            }
            else if ($scope.reverse2 == false) {
                $scope.reverse2 = !$scope.reverse2;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
            }
        }

       
        $scope.order = function (keyname) {
            if ($scope.reverse3 == true) {
                $scope.reverse3 = !$scope.reverse3;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
            }
            else if ($scope.reverse3 == false) {
                $scope.reverse3 = !$scope.reverse3;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
            }
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.sortSaved = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
        }


    }

})();