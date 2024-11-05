
(function () {
    'use strict';
    angular
.module('app')
.controller('clg_CB_SEM_MappingController', clg_CB_SEM_MappingController)

    clg_CB_SEM_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function clg_CB_SEM_MappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.submitted = false;
        $scope.sortKey = 'imcC_Id';
        $scope.sortReverse = true;
        $scope.dis = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            $scope.All_S = false;
            $scope.dis = false;
            $scope.coursebranchlist = [];
            //$scope.amcobM_Id = '';
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("clg_CB_SEM_Mapping/Getdetails").
       then(function (promise) {
           if (promise.courselist !=null) {
               $scope.courselist = promise.courselist;
               $scope.semlist = promise.semlist;
               $scope.griddata = promise.griddata;

           }
           else {

           }
       })
            //$scope.reverse = false;
            //$scope.sort = function (keyname) {
            //    $scope.sortKey = keyname;   //set the sortKey to the param passed
            //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            //}

        };

        $scope.isOptionsRequired1 = function () {

            return !$scope.semlist.some(function (options) {
                return options.checked;
            });

        }



        

        $scope.showsemtGrid = function (sem) {
           

            var id = sem.amcobM_Id
            var data = {

                "AMCOBM_Id": id,

            }

            apiService.create("clg_CB_SEM_Mapping/sempopup", data).
        then(function (promise) {

            if (promise.semdetails != null) {
                $scope.semdetails = promise.semdetails;
            }
            else {


                swal("No Branch");

            }
        }
        );

        }
        $scope.Getbranch = function (amcO_Id) {
            $scope.amcobM_Id = '';
            $scope.coursebranchlist = [];


            var data = {
               
                "AMCO_Id": amcO_Id,
               
            }

            apiService.create("clg_CB_SEM_Mapping/Getbranch", data).
        then(function (promise) {
         
            if (promise.coursebranchlist != null) {
                $scope.coursebranchlist = promise.coursebranchlist;
            }
            else {


                swal("No Branch");

            }
        }
        );

        }


        $scope.toggleAll_S = function () {
            angular.forEach($scope.semlist, function (subj) {
                subj.checked = $scope.All_S;
            })
        };
        $scope.optiontoggled = function () {
            $scope.All_S = $scope.semlist.every(function (itm) { return itm.checked; });
        };




        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.Deactivate = function (sem, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (sem.amcobmS_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
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

                apiService.create("clg_CB_SEM_Mapping/deactivate", sem).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                        $scope.showsemtGrid(sem);
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                        $scope.showsemtGrid(sem);
                    }

                   $scope.BindData();
                })

              
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }



        //delete record
        $scope.Deletecastecategorydata = function (DeleteRecord, SweetAlert) {
            // swal(id);
            $scope.deleteId = DeleteRecord.amcobmS_Id;
            var MdeleteId = $scope.deleteId;
            swal({
                title: "Are You Sure?",
                text: "Do You Want To Delete The Record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("castecategory/MasterDeleteModulesDTO", MdeleteId).
                    then(function (promise) {
                        if (promise.message == "Delete") {
                            swal("You Can Not Delete This Record It Is Already Mapped With Student");
                        }
                        else {
                            swal('Record Deleted Successfully');
                        }

                        $state.reload();
                    })
                }
                else {
                    swal("Cancelled");
                }

            });
        }

        // to Edit Data
        $scope.edit = function (EditRecord) {

           
            var MEditId = EditRecord.amcobM_Id;
            apiService.getURI("clg_CB_SEM_Mapping/Editrecord/", MEditId).
            then(function (promise) {
                $scope.dis = true;
                $scope.editgriddata = promise.editgriddata;

                $scope.amcO_Id = $scope.editgriddata[0].amcO_Id;
              $scope.Getbranch($scope.amcO_Id);
                $scope.amcobM_Id = $scope.editgriddata[0].amcobM_Id;
               

                angular.forEach($scope.editgriddata, function (s1) {


                    angular.forEach($scope.semlist, function (s2) {

                        if (s1.amsE_Id == s2.amsE_Id) {
                            s2.checked = true;
                        }

                    })

                    $scope.All_S = $scope.semlist.every(function (itm) { return itm.checked; });

                })
                

                //$scope.IMCC_Id = promise.castecategoryname[0].imcC_Id;
                //$scope.name = promise.castecategoryname[0].imcC_CategoryName;
                //$scope.description = promise.castecategoryname[0].imcC_CategoryDesc;

            })

        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savetmpldata = function () {
         
            if ($scope.myForm.$valid) {

                $scope.albumNameArray = [];
                angular.forEach($scope.semlist, function (role) {
                    if (!!role.checked) $scope.albumNameArray.push(role);
                })
                var data = {
                    "AMCOBM_Id": $scope.amcobM_Id,
                    selectedsem: $scope.albumNameArray
                    
                }
             
                apiService.create("clg_CB_SEM_Mapping/savesem", data).then(function (promise) {
                    if (promise.returnduplicatestatus =="Saved") {
                      
                        swal("Data Saved/Updated Successfully ")

                        $scope.BindData();
                      
                    }

                
                    else {
                        swal("Error ")

                        $scope.BindData();
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.cancel = function () {
            $state.reload();
        }


       

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
        
            return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }

})();