

(function () {
    'use strict';
    angular
.module('app')
.controller('MasterMenuPageMappingInstitutionwiseController', MasterMenuPageMappingInstitutionwiseController)

    MasterMenuPageMappingInstitutionwiseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function MasterMenuPageMappingInstitutionwiseController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {
        var HostName = location.host;

        $scope.nulldisply == false

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterSubMenuINS/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/institutionmodulepage/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.clearid = function () {
            $state.reload();
        }
        $scope.showdelete = false;
        $scope.reverse1 = true;
        $scope.reverse2 = true;
        $scope.reverse3 = true;
        $scope.reverse4 = true;

        $scope.clearForm = function () {
            $state.reload();
        }

        $scope.BindData = function () {

            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.page3 = "page3";
            $scope.page4 = "page4";
            $scope.page5 = "page5";


            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10

            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10

            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10

            $scope.currentPage5 = 1;
            $scope.itemsPerPage5 = 10

            var pageid = 1;

            apiService.getURI("MasterMenuPageMappingInstitutionwise/Getdetails", pageid).
       then(function (promise) {

           $scope.masterMainMenuName = promise.mastermenuarray;
           $scope.mastersubmenu = promise.mastersubmenuarray;
           $scope.modulename = promise.mastermodule;
           $scope.institution = promise.fillinstitution

           if (promise.fillgrid.length > 0) {
               $scope.finalgrid = true;
               $scope.GridDetails = promise.fillgrid;
               $scope.sortKey4 = "ivrmmmpmI_Id";
               $scope.reverse4 = true;
           }
           else {
               swal("No records Saved")
               $scope.finalgrid = true;
           }

           $scope.secondgrid = [];

       })

            $scope.order4 = function (keyname) {
                $scope.sortKey4 = keyname;   //set the sortKey to the param passed
                $scope.reverse4 = !$scope.reverse4; //if true make it false and vice versa
            }

            $scope.order5 = function (keyname) {
                $scope.sortKey5 = keyname;   //set the sortKey to the param passed
                $scope.reverse5 = !$scope.reverse5; //if true make it false and vice versa
            }

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


        };

        $scope.institutionchange = function (institutionid) {
            apiService.getURI("MasterMenuPageMappingInstitutionwise/institutionchange/", institutionid).
          then(function (promise) {
              $scope.firstgrid = false;
              $scope.previouslysaveddata = false;
              $scope.gridview2 = false;
              $scope.fillmodulename = promise.fillmodule;
          })
        }

        $scope.modulechange = function (moduleid) {

            var data = {
                "IVRMM_Id": moduleid,
                "MI_Id": $scope.MI_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterMenuPageMappingInstitutionwise/modulechange/", data).
            then(function (promise) {
                $scope.firstgrid = false;
                $scope.previouslysaveddata = false;
                $scope.gridview2 = false;
                $scope.masterMainMenuName = promise.mastermenuarray;
            })
        }


        $scope.mainmenuchange = function (mainmenuid) {

            var data = {
                "IVRMMMI_Id": mainmenuid,
                "MI_Id": $scope.MI_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterMenuPageMappingInstitutionwise/mainmenuchange/", data).
           then(function (promise) {
               $scope.firstgrid = false;
               $scope.previouslysaveddata = false;
               $scope.gridview2 = false;
               $scope.mastersubmenu = promise.mastersubmenuarray;
           })
        }

        $scope.submenuchange = function (menuinstitutionwise) {

            var data = {
                "IVRMM_Id": $scope.IVRMM_Id,
                "IVRMMMI_Id": menuinstitutionwise,
                "MI_Id": $scope.MI_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("MasterMenuPageMappingInstitutionwise/submenuchange/", data).
         then(function (promise) {
             if (promise.fillpages.length > 0) {

                 $scope.firstgrid = true;
                 $scope.pages = promise.fillpages;

                 if (promise.fillprioussavedgrid.length > 0) {
                     $scope.previousGridDetails = promise.fillprioussavedgrid;
                     $scope.GridDetails = promise.gridDetails;
                     $scope.institutionname = promise.gridDetails[0].mI_Name;
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

        //    }
        //    else {
        //        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);

        //    }

        //};

        $scope.addtocart = function (record, previouslysaveddata, index) {
            $scope.gridview2 = true;

            //$scope.btns = true;
            
            if (previouslysaveddata != undefined) {
                var valid;
                for (var i = 0; i < previouslysaveddata.length; i++) {
                    if (previouslysaveddata[i].ivrmP_Id == record.ivrmP_Id) {
                        if ($scope.secondgrid.length == 0) {

                            $scope.gridview2 = false;

                        }

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
                    if ($scope.secondgrid.length == 0) {
                        $scope.$apply(function () {
                            $scope.gridview2 = false;
                        });
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
                text: "Do you want to Delete Record????",
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
                    console.log($scope.secondgrid.indexOf(stuDelRecord));
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

        $scope.cance = function () {
            //   $scope.IVRMMM_Id = "";
            $state.reload();
        }

        $scope.editEmployee = {}
        $scope.DeleteMasterSubMenudata = function (employee) {
            $scope.editEmployee = employee.ivrmmmpmI_Id;
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
                    apiService.DeleteURI("MasterMenuPageMappingInstitutionwise/deletemasterdata", MdeleteId).
                    then(function (promise) {

                        swal(promise.returnval);
                     
                        $state.reload();

                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
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
                    "IVRMMMI_Id": $scope.IVRMMMI_Id,
                    savetmpdata: array,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterMenuPageMappingInstitutionwise/savemenudata", data).
                        then(function (promise) {

                            if (promise.returnval != "") {
                                swal(promise.returnval);
                                $state.reload();
                            }

                            //if (promise.returnval == true) {
                            //    swal('Record Saved/Updated Successfully', 'success');
                            //    $state.reload();
                            //}
                            //else if (promise.returnval == false) {
                            //    swal('Record Not Saved/Updated Successfully');
                            //}

                        })
            };
        }

        $scope.getOrder = function (orderarray) {
            $scope.nulldisply = false;
            angular.forEach(orderarray, function (value, key) {
                if (value.ivrmmmI_Id !== 0) {
                    //orderarray[key].pageorder = key + 1;
                    orderarray[key].pageorder = value.pageorder;
                }
                if (value.displayname == null || value.displayname == undefined) {
                    $scope.nulldisply = true;
                }
               
            });

            if ($scope.nulldisply == false) {
                var data = {
                    menuDTO: orderarray,
                }
                apiService.create("MasterMenuPageMappingInstitutionwise/validateordernumber", data).
                    then(function (promise) {
                        if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                            swal(promise.retrunMsg);
                            $state.reload();

                        }
                    });
            }
            else {
                swal("Enter display name!! ");
            }
          
        }


        $scope.toggleAllmaster = function () {
            $scope.deletegrid = [];
            if ($scope.searchfinal == '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.GridDetails, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.deletegrid.push(itm);
                    }
                    else {
                        $scope.deletegrid.splice(itm);
                    }
                });
            }

            if ($scope.searchfinal != '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.filterValue1master, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.deletegrid.push(itm);
                    }
                    else {
                        $scope.deletegrid.splice(itm);
                    }
                });
            }
            if ($scope.deletegrid.length > 0) {
                $scope.showdelete = true;

            }
            else {
                $scope.showdelete = false;

            }

        }

        $scope.deletegrid = [];
        $scope.searchfinal = "";
        $scope.addtocartdelete = function (SelectedStudentRecord, index) {
           
            $scope.btns = true;
            if ($scope.searchfinal == '') {
                $scope.alll = $scope.GridDetails.every(function (itm) { return itm.checked; });

                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.GridDetails, function (qwe) {
                        if (qwe.ivrmmmpmI_Id == SelectedStudentRecord.ivrmmmpmI_Id) {

                            $scope.deletegrid.push(SelectedStudentRecord);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletegrid, function (qwe) {
                        if (qwe.ivrmmmpmI_Id == SelectedStudentRecord.ivrmmmpmI_Id) {

                            $scope.deletegrid.splice($scope.deletegrid.indexOf(SelectedStudentRecord), 1);
                        }
                    })

                }

            }

            if ($scope.searchfinal != '') {
                $scope.alll = $scope.filterValue1master.every(function (itm) { return itm.checked; });
                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.GridDetails, function (qwe) {
                        if (qwe.ivrmmmpmI_Id == SelectedStudentRecord.ivrmmmpmI_Id) {

                            $scope.deletegrid.push(SelectedStudentRecord);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletegrid, function (qwe) {
                        if (qwe.ivrmmmpmI_Id == SelectedStudentRecord.ivrmmmpmI_Id) {

                            //$scope.secondgrid.splice(qwe);
                            $scope.deletegrid.splice($scope.deletegrid.indexOf(SelectedStudentRecord), 1);
                        }
                    })

                }
            }
            if ($scope.deletegrid.length > 0) {
                $scope.showdelete = true;
            }
            else {
                $scope.showdelete = false;
            }

        }


        $scope.submitted = false;

        $scope.deletedata = function (savedpagesrecord) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.submitted = true;

                        if (savedpagesrecord != undefined) {
                            var savedarray = $.map(savedpagesrecord, function (value, index) {
                                return [value];
                            });
                        }

                        var data = {
                            privilagedata: savedarray
                        };

                        apiService.create("MasterMenuPageMappingInstitutionwise/deletemasterdata",data).
                            then(function (promise) {

                                if (promise.returnval == 'true') {
                                    swal("Data Deleted Successfully!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Failed to Deleted")
                                    $state.reload();
                                }

                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });


            // };
        }
    }

})();

