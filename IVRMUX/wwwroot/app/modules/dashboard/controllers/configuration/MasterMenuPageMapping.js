


(function () {
    'use strict';
    angular
.module('app')
.controller('MasterMenuPageMappingController', MasterMenuPageMappingController)

    MasterMenuPageMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function MasterMenuPageMappingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/mastersubmenu/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterschooltype/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};


        $scope.reverse1 = true;
        $scope.reverse2 = true;
        $scope.reverse3 = true;
        $scope.reverse4 = true;

        $scope.BindData = function () {

            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.page3 = "page3";
            $scope.page4 = "page4";


            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10

            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10

            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10

            var pageid = 1;

            apiService.getURI("MasterMenuPageMapping/Getdetails", pageid).
       then(function (promise) {

           $scope.masterMainMenuName = promise.mastermenuarray;
           $scope.mastersubmenu = promise.mastersubmenuarray;
           $scope.modulename = promise.mastermodule;

           if (promise.fillgrid.length > 0) {
               $scope.finalgrid = true;
               $scope.GridDetails = promise.fillgrid;
           }
           else {
               swal("No records Saved")
               $scope.finalgrid = true;
           }
           $scope.secondgrid = [];

       })



            $scope.order1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }

            $scope.order2 = function (keyname) {
                $scope.sortKey2 = keyname;   //set the sortKey to the param passed
                $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
            }

            $scope.order3 = function (keyname) {
                $scope.sortKey3 = keyname;   //set the sortKey to the param passed
                $scope.reverse3 = !$scope.reverse3; //if true make it false and vice versa
            }

            $scope.order4 = function (keyname) {
                $scope.sortKey4 = keyname;   //set the sortKey to the param passed
                $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
            }

        };

        $scope.modulechange = function (moduleid) {
            apiService.getURI("MasterMenuPageMapping/modulechange/", moduleid).
            then(function (promise) {
                $scope.firstgrid = false;
                $scope.previouslysaveddata = false;
                $scope.gridview2 = false;
                $scope.masterMainMenuName = promise.mastermenuarray;
            })
        }


        $scope.mainmenuchange = function (mainmenuid) {
            apiService.getURI("MasterMenuPageMapping/mainmenuchange/", mainmenuid).
           then(function (promise) {
               $scope.firstgrid = false;
               $scope.previouslysaveddata = false;
               $scope.gridview2 = false;
               $scope.mastersubmenu = promise.mastersubmenuarray;
           })
        }

        $scope.submenuchange = function (submenuid) {
            
            var data = {
                "IVRMM_Id": $scope.IVRMM_Id,
                "IVRMMM_Id": submenuid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterMenuPageMapping/submenuchange/", data).
         then(function (promise) {
             if (promise.fillpages.length > 0) {
                 $scope.firstgrid = true;
                 $scope.previouslysaveddata = true;
                 $scope.pages = promise.fillpages;

                 if (promise.fillprioussavedgrid.length > 0) {
                     $scope.previousGridDetails = promise.fillprioussavedgrid;
                     $scope.previouslysaveddata = true;
                 }
             }
             else {
                 $scope.firstgrid = false;
                 swal("No Records Found")
             }
         })
        }

        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};

        //$scope.addtocart = function (record) {
        //    $scope.gridview2 = true;
        //    if ($scope.secondgrid.indexOf(record) === -1) {
        //        $scope.secondgrid.push(record);
        //        //return $filter('filter')($scope.newuser1, { checked: true });
        //    }
        //    else {
        //        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
        //        //return $filter('filter')($scope.newuser1, { checked: true });
        //    }

        //};


        $scope.addtocart = function (record, previouslysaveddata, index) {
            $scope.gridview2 = true;
            //$scope.btns = true;

            if (previouslysaveddata != undefined) {
                var valid;
                for (var i = 0; i < previouslysaveddata.length; i++) {
                    if (previouslysaveddata[i].ivrmP_Id == record.ivrmP_Id) {
                        swal("Already this page is saved with Current role and Module..Kindly select other pages")
                        valid = "committ";
                        record.checked = false;
                        // $scope.disablecheck[index] = true;
                    }
                }

                if (valid != "committ") {
                    if ($scope.secondgrid.indexOf(record) === -1) {
                        $scope.secondgrid.push(record);
                    }
                    else {
                        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
                    }
                }
            }
            else {
                if ($scope.secondgrid.indexOf(record) === -1) {
                    $scope.secondgrid.push(record);
                }
                else {
                    $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
                }
            }


        };



        $scope.currenrow = {};
        $scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {


            swal({
                title: "Are you sure",
                text: "Do you want to Delete Record?",
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
                
                if ($scope.secondgrid.length == 0) {
                    $scope.$apply(function () {
                        $scope.gridview2 = false;
                    });
                }
                swal("Row as been removed from list Successfully");
            }
            else {
                swal("Cancelled Successfully");
            }
        });




        };
        $scope.editEmployee = {}
        $scope.DeleteMasterSubMenudata = function (employee) {
            $scope.editEmployee = employee.ivrmmmpM_Id;
            var MdeleteId = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterMenuPageMapping/deletemasterdata", MdeleteId).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            swal('Record Deleted Successfully!', 'success');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Deleted', 'Failed');
                        }

                        $state.reload();

                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }

        $scope.clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;
        $scope.savedata = function (secondgrid) {

            $scope.submitted = true;

            if ($scope.frmmodule.$valid) {

                var array = $.map(secondgrid, function (value, index) {
                    return [value];
                });


                var data = {
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "IVRMMM_Id": $scope.IVRMMM_Idsubmenu,
                    savetmpdata: array,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterMenuPageMapping/savemenudata", data).
                        then(function (promise) {

                            if (promise.returnval == true) {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval == false) {
                                swal('Record Not Saved Successfully');
                            }
                            $state.reload();
                        })
            };
        }
    }

})();

